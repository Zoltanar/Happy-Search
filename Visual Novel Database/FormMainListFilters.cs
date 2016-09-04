using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using BrightIdeasSoftware;
using Happy_Search.Properties;

namespace Happy_Search
{
    partial class FormMain
    {
        private static readonly ToggleArray Toggles = new ToggleArray();

        /// <summary>
        /// Display all Visual Novels in local database.
        /// </summary>
        private void Filter_All(object sender, EventArgs e)
        {
            Filter_ClearOther();
            _currentList = x => true;
            currentListLabel.Text = @"List: " + @"All Titles";
            RefreshVNList();
        }

        /// <summary>
        /// Clear other listing options.
        /// </summary>
        private void Filter_ClearOther()
        {
            _dontTriggerEvent = true;
            ulStatusDropDown.SelectedIndex = 0;
            wlStatusDropDown.SelectedIndex = 0;
            ProducerListBox.Text = "";
            _dontTriggerEvent = false;
        }

        /// <summary>
        /// Display VNs by producers in Favorite Producers list.
        /// </summary>
        private void Filter_FavoriteProducers(object sender, EventArgs e)
        {
            if (_dontTriggerEvent) return;
            Filter_ClearOther();
            if (olFavoriteProducers.Items.Count == 0)
            {
                WriteError(replyText, "No Favorite Producers.", true);
                return;
            }
            IEnumerable<string> prodList = from ListedProducer producer in olFavoriteProducers.Objects
                                           select producer.Name;
            _currentList = vn => prodList.Contains(vn.Producer);
            currentListLabel.Text = @"List: " + @"Favorite Producers";
            RefreshVNList();
        }
        
        /// <summary>
        /// Display VNs with selected Userlist status.
        /// </summary>
        private void Filter_ULStatus(object sender, EventArgs e)
        {
            if (_dontTriggerEvent) return;
            var dropdownlist = (ComboBox)sender;
            switch (dropdownlist.SelectedIndex)
            {
                case 0:
                    Filter_All(null, null);
                    return;
                case 1:
                    dropdownlist.SelectedIndex = 0;
                    return;
                case 2:
                    _currentList = x => !x.ULStatus.Equals("");
                    currentListLabel.Text = @"List: " + @"Userlist Titles";
                    break;
                case 3:
                    _currentList = x => x.ULStatus.Equals("Unknown");
                    currentListLabel.Text = @"List: " + @"UL Unknown";
                    break;
                case 4:
                    _currentList = x => x.ULStatus.Equals("Playing");
                    currentListLabel.Text = @"List: " + @"UL Playing";
                    break;
                case 5:
                    _currentList = x => x.ULStatus.Equals("Finished");
                    currentListLabel.Text = @"List: " + @"UL Finished";
                    break;
                case 6:
                    _currentList = x => x.ULStatus.Equals("Stalled");
                    currentListLabel.Text = @"List: " + @"UL Stalled";
                    break;
                case 7:
                    _currentList = x => x.ULStatus.Equals("Dropped");
                    currentListLabel.Text = @"List: " + @"UL Dropped";
                    break;
            }
            var value = dropdownlist.SelectedIndex;
            Filter_ClearOther();
            _dontTriggerEvent = true;
            ulStatusDropDown.SelectedIndex = value;
            _dontTriggerEvent = false;
            RefreshVNList();
        }

        /// <summary>
        /// Display VNs with selected Wishlist status.
        /// </summary>
        private void Filter_WLStatus(object sender, EventArgs e)
        {
            if (_dontTriggerEvent) return;
            var dropdownlist = (ComboBox)sender;
            switch (dropdownlist.SelectedIndex)
            {
                case 0:
                    Filter_All(null, null);
                    return;
                case 1:
                    dropdownlist.SelectedIndex = 0;
                    return;
                case 2:
                    _currentList = x => !x.WLStatus.Equals("");
                    currentListLabel.Text = @"List: " + @"Wishlist Titles";
                    break;
                case 3:
                    _currentList = x => x.WLStatus.Equals("High");
                    currentListLabel.Text = @"List: " + @"WL High";
                    break;
                case 4:
                    _currentList = x => x.WLStatus.Equals("Medium");
                    currentListLabel.Text = @"List: " + @"WL Medium";
                    break;
                case 5:
                    _currentList = x => x.WLStatus.Equals("Low");
                    currentListLabel.Text = @"List: " + @"WL Low";
                    break;
                case 6:
                    _currentList = x => x.WLStatus.Equals("Blacklist");
                    currentListLabel.Text = @"List: " + @"WL Blacklist";
                    break;
            }
            var value = dropdownlist.SelectedIndex;
            Filter_ClearOther();
            _dontTriggerEvent = true;
            wlStatusDropDown.SelectedIndex = value;
            _dontTriggerEvent = false;
            RefreshVNList();
        }
        /// <summary>
        /// Display VNs by producer typed/selected in box.
        /// </summary>
        private void Filter_Producer(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            var producerName = ProducerListBox.Text;
            if (!producerName.Any()) return;
            Filter_ClearOther();
            _currentList = x => x.Producer.Equals(producerName, StringComparison.InvariantCultureIgnoreCase);
            currentListLabel.Text = @"List: " + $"{producerName} (Producer)";
            ProducerListBox.Text = producerName;
            RefreshVNList();
        }

        /// <summary>
        /// Get new VNs from VNDB that match selected producer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateProducerTitles(object sender, EventArgs e)
        {
            var producer = ProducerListBox.Text;
            if (producer.Equals("")) return;
            var askBox = MessageBox.Show(Resources.update_custom_filter + '\n' + Resources.are_you_sure, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            var producerItem = _producerList.Find(x => x.Name.Equals(producer, StringComparison.InvariantCultureIgnoreCase));
            if (producerItem == null)
            {
                //TODO
                WriteError(replyText, "NYI (Producer not in local db)", true);
                return;
            }
            ReloadLists();
            _vnsAdded = 0;
            _vnsSkipped = 0;
            await GetProducerTitles(producerItem, replyText);
            WriteText(replyText, $"Got new VNs for {producerItem.Name}, added {_vnsAdded} titles.");
            ReloadLists();
            RefreshVNList();
        }

        /// <summary>
        /// Filter titles by URT status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void URTToggle(object sender, EventArgs e)
        {
            if (_dontTriggerEvent) return;
            Toggles.URTToggleSetting = (ToggleSetting)URTToggleBox.SelectedIndex;
            ApplyToggleFilters();
        }

        /// <summary>
        /// Filter titles by released status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnreleasedToggle(object sender, EventArgs e)
        {
            if (_dontTriggerEvent) return;
            Toggles.UnreleasedToggleSetting = (ToggleSetting)UnreleasedToggleBox.SelectedIndex;
            ApplyToggleFilters();
        }

        /// <summary>
        /// Filter titles by blacklist status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BlacklistToggle(object sender, EventArgs e)
        {
            if (_dontTriggerEvent) return;
            Toggles.BlacklistToggleSetting = (ToggleSetting)BlacklistToggleBox.SelectedIndex;
            ApplyToggleFilters();
        }

        /// <summary>
        /// Get function for the specified filter.
        /// </summary>
        /// <param name="toggle">Which filter to get function from</param>
        /// <returns>Function for specified filter</returns>
        public Func<ListedVN, bool> GetFunc(ToggleFilter toggle)
        {
            var function = new Func<ListedVN, bool>(x => true);
            switch (toggle)
            {
                case ToggleFilter.URT:
                    switch (Toggles.URTToggleSetting)
                    {
                        case ToggleSetting.Show:
                            return function;
                        case ToggleSetting.Hide:
                            return x => URTList.Find(y => y.VNID == x.VNID) == null;
                        case ToggleSetting.Only:
                            return x => URTList.Find(y => y.VNID == x.VNID) != null;
                        case ToggleSetting.OnlyUnplayed:
                            return x => !x.ULStatus.Equals("Finished") && !x.ULStatus.Equals("Dropped");
                        default: return function;
                    }
                case ToggleFilter.Unreleased:
                    switch (Toggles.UnreleasedToggleSetting)
                    {
                        case ToggleSetting.Show:
                            return function;
                        case ToggleSetting.Hide:
                            return x => !IsUnreleased(x.RelDate);
                        case ToggleSetting.Only:
                            return x => IsUnreleased(x.RelDate);
                        default: return function;
                    }
                case ToggleFilter.Blacklisted:
                    switch (Toggles.BlacklistToggleSetting)
                    {
                        case ToggleSetting.Show:
                            return function;
                        case ToggleSetting.Hide:
                            return x => !x.WLStatus.Equals("Blacklist");
                        case ToggleSetting.Only:
                            return x => x.WLStatus.Equals("Blacklist");
                        default: return function;
                    }
                default:
                    return function;
            }
        }

        /// <summary>
        /// Apply toggle filters to list.
        /// </summary>
        private void ApplyToggleFilters()
        {
            Func<ListedVN, bool>[] funcArray = { GetFunc(ToggleFilter.URT), GetFunc(ToggleFilter.Unreleased), GetFunc(ToggleFilter.Blacklisted), VNMatchesFilter };
            tileOLV.ModelFilter = new ModelFilter(vn => funcArray.Select(filter => filter((ListedVN)vn)).All(valid => valid));
            objectList_ItemsChanged(null, null);
            SaveMainXML();
        }

        /// <summary>
        /// Specifies toggle filter
        /// </summary>
        public enum ToggleFilter
        {
#pragma warning disable 1591
            URT,
            Unreleased,
            Blacklisted
        }

        /// <summary>
        /// Specifies toggle setting
        /// </summary>
        public enum ToggleSetting
        {
            Show,
            Hide,
            Only,
            OnlyUnplayed
        }

        /// <summary>
        /// Class holding toggle filter settings.
        /// </summary>
        [Serializable, XmlRoot("ToggleArray")]
        public class ToggleArray
        {
            /// <summary>
            /// Empty constructor needed for XML
            /// </summary>
            public ToggleArray()
            {
                URTToggleSetting = 0;
                UnreleasedToggleSetting = 0;
                BlacklistToggleSetting = 0;
            }
            public ToggleSetting URTToggleSetting { get; set; }
            public ToggleSetting UnreleasedToggleSetting { get; set; }
            public ToggleSetting BlacklistToggleSetting { get; set; }
#pragma warning restore 1591

        }
    }
}
