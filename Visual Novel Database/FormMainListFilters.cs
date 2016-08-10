using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Happy_Search.Properties;

namespace Happy_Search
{
    partial class FormMain
    {
        /// <summary>
        /// Display all Visual Novels in local database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter_All(object sender, EventArgs e)
        {
            tileOLV.ModelFilter = null;
            _currentList = x => true;
            RefreshList();
        }

        /// <summary>
        /// Display VNs by producers in Favorite Producers list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter_FavoriteProducers(object sender, EventArgs e)
        {
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
            RefreshList();
        }

        /// <summary>
        /// Display VNs matching tags in selected custom filter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter_Custom(object sender, EventArgs e)
        {
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
                    DisplayFilterTags();
                    _currentList = VNMatchesFilter;
                    break;
            }
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
            if (!ProducerFilterBox.Text.Any()) return;
            _currentList = x => x.Producer.Equals(ProducerFilterBox.Text, StringComparison.InvariantCultureIgnoreCase);
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
            _added = 0;
            _skipped = 0;
            await GetProducerTitles(producerItem, replyText);
            WriteText(replyText, $"Got new VNs for {producerItem.Name}, added {_added} titles.");
            RefreshList();
        }

        /// <summary>
        /// Filter titles by blacklist status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BlacklistToggle(object sender, EventArgs e)
        {
            //TODO change ThreeStateBox to ComboBox
            Func<ListedVN, bool> function = x => true;
            switch (BlacklistToggleBox.CheckState)
            {
                case CheckState.Unchecked:
                    BlacklistToggleBox.Text = @"Hide Blacklisted";
                    function = x => !x.WLStatus.Equals("Blacklist");
                    break;
                case CheckState.Indeterminate:
                    BlacklistToggleBox.Text = @"Show Blacklisted";
                    break;
                case CheckState.Checked:
                    BlacklistToggleBox.Text = @"Only Blacklisted";
                    function = x => x.WLStatus.Equals("Blacklist");
                    break;
            }
            ApplyToggleFilters(ToggleFilter.Blacklisted, function);
        }

        /// <summary>
        /// Filter titles by released status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnreleasedToggle(object sender, EventArgs e)
        {
            //TODO change ThreeStateBox to ComboBox
            Func<ListedVN, bool> function = x => true;
            switch (UnreleasedToggleBox.CheckState)
            {
                case CheckState.Unchecked:
                    UnreleasedToggleBox.Text = @"Hide Unreleased";
                    function = x => !CheckUnreleased(x.RelDate);
                    break;
                case CheckState.Indeterminate:
                    UnreleasedToggleBox.Text = @"Show Unreleased";
                    break;
                case CheckState.Checked:
                    UnreleasedToggleBox.Text = @"Only Unreleased";
                    function = x => CheckUnreleased(x.RelDate);
                    break;
            }
            ApplyToggleFilters(ToggleFilter.Unreleased, function);
        }

        /// <summary>
        /// Filter titles by URT status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void URTToggle(object sender, EventArgs e)
        {
            //TODO change ThreeStateBox to ComboBox
            Func<ListedVN, bool> function = x => true;
            switch (URTToggleBox.CheckState)
            {
                case CheckState.Unchecked:
                    URTToggleBox.Text = @"Hide URT";
                    function = x => UserList.Find(y => y.VNID == x.VNID) == null;
                    break;
                case CheckState.Indeterminate:
                    URTToggleBox.Text = @"Show URT";
                    break;
                case CheckState.Checked:
                    URTToggleBox.Text = @"Only URT";
                    function = x => UserList.Find(y => y.VNID == x.VNID) != null;
                    break;
            }
            ApplyToggleFilters(ToggleFilter.URT, function);
        }

        /// <summary>
        /// Change filter and apply it to VN List.
        /// </summary>
        /// <param name="toggleFilter">Filter that should be changed</param>
        /// <param name="function">What the filter should be changed to</param>
        private void ApplyToggleFilters(ToggleFilter toggleFilter, Func<ListedVN, bool> function)
        {
            _filters[(int)toggleFilter] = function;
            tileOLV.ModelFilter = _filters.Any()
                ? new ModelFilter(vn => _filters.Select(filter => filter((ListedVN)vn)).All(valid => valid))
                : null;
            objectList_ItemsChanged(null, null);
        }

        /// <summary>
        /// Specifies toggle filter
        /// </summary>
        private enum ToggleFilter
        {
            URT,
            Unreleased,
            Blacklisted
        }
    }
}
