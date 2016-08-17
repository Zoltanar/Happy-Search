﻿using System;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter_All(object sender, EventArgs e)
        {
            Filter_ClearOther();
            _currentList = x => true;
            RefreshList();
        }

        private void Filter_ClearOther(bool clearFilterTags = true)
        {
            _dontTriggerEvent = true;
            customFilters.SelectedIndex = 0;
            ULStatusDropDown.SelectedIndex = 0;
            if (clearFilterTags)
            {
                DisplayFilterTags(true);
                deleteCustomFilterButton.Enabled = false;
                updateCustomFilterButton.Enabled = false;
                filterNameBox.Text = "";
            }
            ProducerFilterBox.Text = "";
            _dontTriggerEvent = false;
        }
        /// <summary>
        /// Display VNs by producers in Favorite Producers list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            RefreshList();
        }

        /// <summary>
        /// Display VNs in user's wishlist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter_Wishlist(object sender, EventArgs e)
        {
            if (_dontTriggerEvent) return;
            Filter_ClearOther();
            _currentList = x => !x.WLStatus.Equals("");
            RefreshList();
        }

        /// <summary>
        /// Display VNs with select Userlist status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    _currentList = x => x.ULStatus.Equals("Unknown");
                    break;
                case 3:
                    _currentList = x => x.ULStatus.Equals("Playing");
                    break;
                case 4:
                    _currentList = x => x.ULStatus.Equals("Finished");
                    break;
                case 5:
                    _currentList = x => x.ULStatus.Equals("Dropped");
                    break;
            }
            var value = dropdownlist.SelectedIndex;
            Filter_ClearOther();
            _dontTriggerEvent = true;
            ULStatusDropDown.SelectedIndex = value;
            _dontTriggerEvent = false;
            RefreshList();
        }

        /// <summary>
        /// Display VNs matching tags in selected custom filter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter_Custom(object sender, EventArgs e)
        {
            if (_dontTriggerEvent) return;
            var dropdownlist = (ComboBox)sender;
            switch (dropdownlist.SelectedIndex)
            {
                case 0:
                    deleteCustomFilterButton.Enabled = false;
                    updateCustomFilterButton.Enabled = false;
                    Filter_All(null, null);
                    return;
                case 1:
                    dropdownlist.SelectedIndex = 0;
                    return;
                default:
                    deleteCustomFilterButton.Enabled = true;
                    updateCustomFilterButton.Enabled = true;
                    _activeFilter = _customFilters[dropdownlist.SelectedIndex - 2]?.Filters;
                    filterNameBox.Text = _customFilters[dropdownlist.SelectedIndex - 2].Name;
                    DisplayFilterTags();
                    _currentList = VNMatchesFilter;
                    break;
            }
            var value = dropdownlist.SelectedIndex;
            Filter_ClearOther(false);
            _dontTriggerEvent = true;
            dropdownlist.SelectedIndex = value;
            _dontTriggerEvent = false;
            RefreshList();
        }

        /// <summary>
        /// Get new VNs from VNDB that match tags in custom filter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateCustomFilter(object sender, EventArgs e)
        {
            if (Conn.Status != VndbConnection.APIStatus.Ready)
            {
                WriteWarning(replyText, "Connection busy with previous request...", true);
                return;
            }
            var selectedFilter = _customFilters[customFilters.SelectedIndex - 2];
            var message = selectedFilter.Updated != DateTime.MinValue
                ? $"This filter was last updated {DaysSince(selectedFilter.Updated)} days ago.\n{Resources.update_custom_filter}"
                : Resources.update_custom_filter;
            var askBox = MessageBox.Show(message, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            await UpdateFilterResults(replyText);
            _customFilters[customFilters.SelectedIndex - 2].Updated = DateTime.UtcNow;
            SaveCustomFiltersXML();
        }

        /// <summary>
        /// Delete custom filter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteCustomFilter(object sender, EventArgs e)
        {
            var askBox = MessageBox.Show(Resources.are_you_sure, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            var selectedFilter = customFilters.SelectedIndex;
            customFilters.Items.RemoveAt(selectedFilter);
            _customFilters.RemoveAt(selectedFilter - 2);
            SaveCustomFiltersXML();
            replyText.Text = Resources.filter_deleted;
            FadeLabel(replyText);
            customFilters.SelectedIndex = 0;
        }

        /// <summary>
        /// Display VNs by producer typed/selected in box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter_Producer(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            var producerName = ProducerFilterBox.Text;
            if (!producerName.Any()) return;
            Filter_ClearOther();
            _currentList = x => x.Producer.Equals(producerName, StringComparison.InvariantCultureIgnoreCase);
            ProducerFilterBox.Text = producerName;
            RefreshList();
        }

        /// <summary>
        /// Get new VNs from VNDB that match selected producer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateProducerTitles(object sender, EventArgs e)
        {
            var producer = ProducerFilterBox.Text;
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
            _vnsAdded = 0;
            _vnsSkipped = 0;
            await GetProducerTitles(producerItem, replyText);
            WriteText(replyText, $"Got new VNs for {producerItem.Name}, added {_vnsAdded} titles.");
            RefreshList();
        }

        /// <summary>
        /// Filter titles by URT status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void URTToggle(object sender, EventArgs e)
        {
            if (_dontTriggerEvent) return;
            Toggles.URTToggleFunc = (ToggleSetting)URTToggleBox.SelectedIndex;
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
            Toggles.UnreleasedToggleFunc = (ToggleSetting)UnreleasedToggleBox.SelectedIndex;
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
            Toggles.BlacklistToggleFunc = (ToggleSetting)BlacklistToggleBox.SelectedIndex;
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
                    switch (Toggles.URTToggleFunc)
                    {
                        case ToggleSetting.Show:
                            return function;
                        case ToggleSetting.Hide:
                            return x => URTList.Find(y => y.VNID == x.VNID) == null;
                        case ToggleSetting.Only:
                            return x => URTList.Find(y => y.VNID == x.VNID) != null;
                        default: return function;
                    }
                case ToggleFilter.Unreleased:
                    switch (Toggles.UnreleasedToggleFunc)
                    {
                        case ToggleSetting.Show:
                            return function;
                        case ToggleSetting.Hide:
                            return x => !CheckUnreleased(x.RelDate);
                        case ToggleSetting.Only:
                            return x => CheckUnreleased(x.RelDate);
                        default: return function;
                    }
                case ToggleFilter.Blacklisted:
                    switch (Toggles.BlacklistToggleFunc)
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
            Func<ListedVN, bool>[] funcArray = { GetFunc(ToggleFilter.URT), GetFunc(ToggleFilter.Unreleased), GetFunc(ToggleFilter.Blacklisted) };
            tileOLV.ModelFilter = new ModelFilter(vn => funcArray.Select(filter => filter((ListedVN)vn)).All(valid => valid));
            objectList_ItemsChanged(null, null);
            XmlHelper.ToXmlFile(new MainXml(_customFilters, Toggles), MainXmlFile);
        }

        /// <summary>
        /// Specifies toggle filter
        /// </summary>
        public enum ToggleFilter
        {
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
            Only
        }

        /// <summary>
        /// Class holding toggle filter settings.
        /// </summary>
        [Serializable, XmlRoot("ToggleArray")]
        public class ToggleArray
        {
            public ToggleArray()
            {
                URTToggleFunc = 0;
                UnreleasedToggleFunc = 0;
                BlacklistToggleFunc = 0;
            }
            public ToggleSetting URTToggleFunc { get; set; }
            public ToggleSetting UnreleasedToggleFunc { get; set; }
            public ToggleSetting BlacklistToggleFunc { get; set; }

        }
    }
}