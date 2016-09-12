using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Happy_Search.Properties;
using Newtonsoft.Json;

namespace Happy_Search
{
    partial class FormMain
    {

        private const string TagLabel = "tagFilterLabel";

        /// <summary>
        /// Bring up dialog explaining features of the 'Tag Filtering' section.
        /// </summary>
        private void Help_TagFiltering(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            if (path == null)
            {
                WriteError(tagReply, @"Unknown Path Error");
                return;
            }
            var helpFile = $"{Path.Combine(path, "help\\tagfiltering.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

        /// <summary>
        /// Event from checking a MostCommonTags checkbox, adds tag to list of active tag filters.
        /// </summary>
        private void TagFilterAdded(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            if (!checkbox.Checked) return;
            var indexOfLastBracket = checkbox.Text.LastIndexOf('(');
            var tagName = checkbox.Text.Substring(0, indexOfLastBracket).Trim();
            _dontTriggerEvent = true;
            customFilters.SelectedIndex = 0;
            deleteCustomFilterButton.Enabled = false;
            _dontTriggerEvent = false;
            AddFilterTag(tagName);
        }

        /// <summary>
        /// Event from unchecking an active tag filter's checkbox, removes tag from list of active tag filters.
        /// </summary>
        private void TagFilterRemoved(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            if (checkbox.Checked)
            {
                //shouldnt happen
                return;
            }
            //Filter Removed
            var filterNo = Convert.ToInt32(checkbox.Name.Remove(0, TagLabel.Length));
            _activeTagFilter.RemoveAt(filterNo);
            _dontTriggerEvent = true;
            customFilters.SelectedIndex = 0;
            deleteCustomFilterButton.Enabled = false;
            _dontTriggerEvent = false;
            DisplayFilterTags();
        }

        /// <summary>
        /// Save list of active tag filters to file as a custom filter with user-entered name.
        /// </summary>
        private void SaveCustomFilter(object sender, EventArgs e)
        {
            var filterName = filterNameBox.Text;
            if (filterName.Length == 0)
            {
                WriteText(tagReply, "Enter name of filter.", true);
                return;
            }
            //Ask to overwrite if name entered is already in use
            var customFilter = _customFilters.Find(x => x.Name.Equals(filterName));
            if (customFilter != null)
            {
                var askBox = MessageBox.Show(@"Do you wish to overwrite present custom filter?", Resources.ask_overwrite, MessageBoxButtons.YesNo);
                if (askBox != DialogResult.Yes) return;
                customFilter.Filters = new List<TagFilter>(_activeTagFilter);
                customFilter.Updated = DateTime.UtcNow;
                SaveMainXML();
                WriteText(tagReply, Resources.filter_saved, true);
                customFilters.SelectedIndex = customFilters.Items.IndexOf(filterName);
                return;
            }
            customFilters.Items.Add(filterName);
            _customFilters.Add(new ComplexFilter(filterName, new List<TagFilter>(_activeTagFilter)));
            SaveMainXML();
            WriteText(tagReply, Resources.filter_saved, true);
            customFilters.SelectedIndex = customFilters.Items.Count - 1;
        }

        /// <summary>
        /// Clear list of active tag filters (Does not change custom filters).
        /// </summary>
        private void ClearTagFilter(object sender, EventArgs e)
        {
            DisplayFilterTags(true);
            customFilters.SelectedIndex = 0;
            ApplyListFilters();
            WriteText(tagReply, Resources.filter_cleared, true);
        }

        /// <summary>
        /// Add tag (by name) to list of active tag filters.
        /// </summary>
        /// <param name="tagName">Name of tag</param>
        private void AddFilterTag(string tagName)
        {
            var writtenTag =
                PlainTags.Find(item => item.Name.Equals(tagName, StringComparison.InvariantCultureIgnoreCase));
            if (writtenTag == null)
            {
                WriteError(tagReply, $"Tag {tagName} not found", true);
                return;
            }
            foreach (var filter in _activeTagFilter)
            {
                if (!filter.HasChild(writtenTag.ID)) continue;
                //this happens when tag added is more precise than a present tag, eg transfer student heroine is more precise than student heroine
                _activeTagFilter.Remove(filter);
                Debug.Print($"{writtenTag.Name} Tag Displaced {filter.Name} Tag.");
                break;
            }
            //save tag as tag and children
            //var children = new List<int>();
            int[] children = Enumerable.Empty<int>().ToArray();
            Debug.Print($"Getting Child Tags for {tagName}");
            var difference = 1;
            //new
            int[] childrenForThisRound = PlainTags.Where(x => x.Parents.Contains(writtenTag.ID))
                .Select(x => x.ID).ToArray(); //at this moment, it contains direct subtags
            while (difference > 0)
            {
                var initial = children.Length;
                //debug printout
                IEnumerable<WrittenTag> debuglist = childrenForThisRound.Select(childID => PlainTags.Find(x => x.ID == childID));
                Debug.WriteLine(string.Join(", ", debuglist));
                //
                children = children.Union(childrenForThisRound).ToArray(); //first time, adds direct subtags, second time it adds 2-away subtags, etc...
                difference = children.Length - initial;
                var tmp = new List<int>();
                foreach (var child in childrenForThisRound) tmp.AddRange(PlainTags.Where(x => x.Parents.Contains(child)).Select(x => x.ID));
                childrenForThisRound = tmp.ToArray();
            }
            var newFilter = new TagFilter(writtenTag.ID, writtenTag.Name, -1, children);
            var notNeeded = false;
            var count = _vnList.Count(vn => VNMatchesSingleTag(vn, newFilter));
            newFilter.Titles = count;
            WriteText(tagReply, $"Tag {tagName} has {count} VNs in local database.", true);
            foreach (var filter in _activeTagFilter)
            {
                if (!newFilter.HasChild(filter.ID)) continue;
                Debug.Print($"{writtenTag.Name} not necessary, {filter.Name} is already included");
                notNeeded = true;
                break;
            }
            if (notNeeded) return;
            _activeTagFilter.Add(newFilter);
            DisplayFilterTags();
        }

        /// <summary>
        /// Display or clear list of active tag filters.
        /// </summary>
        /// <param name="clear">Should list be cleared?</param>
        private void DisplayFilterTags(bool clear = false)
        {
            //clear old labels
            var oldCount = 0;
            var oldLabel = (CheckBox)Controls.Find(TagLabel + 0, true).FirstOrDefault();
            while (oldLabel != null)
            {
                oldLabel.Dispose();
                oldCount++;
                oldLabel = (CheckBox)Controls.Find(TagLabel + oldCount, true).FirstOrDefault();
            }
            if (clear)
            {
                _activeTagFilter = new List<TagFilter>();
                filterNameBox.Text = "";
                return;
            }
            //add labels
            var count = 0;
            foreach (var filter in _activeTagFilter)
            {
                var filterLabel = new CheckBox
                {
                    AutoSize = false,
                    Location = new Point(264, 34 + count * 22),
                    Name = TagLabel + count,
                    Size = new Size(173, 17),
                    Text = $"{filter.Name} (Total: {filter.Titles})",
                    //MaximumSize = new Size(173, 17),
                    Checked = true,
                    AutoEllipsis = true
                };

                filterLabel.CheckedChanged += TagFilterRemoved;
                count++;
                tagFilteringBox.Controls.Add(filterLabel);
            }
            ApplyListFilters();
        }

        /// <summary>
        /// Update results of list of active filters.
        /// </summary>
        private async void UpdateResults(object sender, EventArgs e)
        {
            if (Conn.Status != VndbConnection.APIStatus.Ready)
            {
                WriteWarning(tagReply, "Connection busy with previous request...", true);
                return;
            }
            if (customFilters.SelectedIndex > 1)
            {
                var selectedFilter = _customFilters[customFilters.SelectedIndex - 2];
                var message = selectedFilter.Updated != DateTime.MinValue
                    ? $"This filter was last updated {DaysSince(selectedFilter.Updated)} days ago.\n{Resources.update_custom_filter}"
                    : Resources.update_custom_filter;
                var askBox2 = MessageBox.Show(message, Resources.are_you_sure, MessageBoxButtons.YesNo);
                if (askBox2 != DialogResult.Yes) return;
                await UpdateFilterResults(tagReply);
                _customFilters[customFilters.SelectedIndex - 2].Updated = DateTime.UtcNow;
                SaveMainXML();
            }
            else
            {
                var askBox = MessageBox.Show(Resources.update_custom_filter, Resources.are_you_sure, MessageBoxButtons.YesNo);
                if (askBox != DialogResult.Yes) return;
                await UpdateFilterResults(tagReply);
            }
        }

        /// <summary>
        /// Get all VNs that match the list of active filters which were not already in local database, if '10 Year Limit' is enabled, only titles released in the last 10 years will be fetched.
        /// </summary>
        private async Task UpdateFilterResults(Label replyLabel)
        {
            ReloadLists();
            _vnsAdded = 0;
            _vnsSkipped = 0;
            IEnumerable<string> betterTags = _activeTagFilter.Select(x => x.ID).Select(s => $"tags = {s}");
            var tags = string.Join(" and ", betterTags);
            string tagQuery = $"get vn basic ({tags}) {{{APIMaxResults}}}";
            var result = await TryQuery(tagQuery, "UCF Query Error", replyLabel, true, true);
            if (!result) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            if (vnRoot.Num == 0) return;
            List<VNItem> vnItems = vnRoot.Items;
            await GetMultipleVN(vnItems.Select(x => x.ID).ToList(), replyLabel, true);
            var pageNo = 1;
            var moreResults = vnRoot.More;
            while (moreResults)
            {
                pageNo++;
                string moreTagQuery = $"get vn basic ({tags}) {{{APIMaxResults}, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(moreTagQuery, "UCFM Query Error", replyLabel, true, true);
                if (!moreResult) return;
                var moreVNRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                if (vnRoot.Num == 0) break;
                List<VNItem> moreVNItems = moreVNRoot.Items;
                await GetMultipleVN(moreVNItems.Select(x => x.ID).ToList(), replyLabel, true);
                moreResults = moreVNRoot.More;
            }
            ReloadLists();
            ApplyListFilters();
            WriteText(replyLabel, $"Update complete, added {_vnsAdded} and skipped {_vnsSkipped} titles.", true);
        }

        /// <summary>
        /// Display VNs matching tags in selected custom filter.
        /// </summary>
        private void Filter_Custom(object sender, EventArgs e)
        {
            if (_dontTriggerEvent) return;
            var dropdownlist = (ComboBox)sender;
            switch (dropdownlist.SelectedIndex)
            {
                case 0:
                    deleteCustomFilterButton.Enabled = false;
                    return;
                case 1:
                    dropdownlist.SelectedIndex = 0;
                    return;
                default:
                    deleteCustomFilterButton.Enabled = true;
                    DisplayFilterTags(true);
                    _activeTagFilter = new List<TagFilter>(_customFilters[dropdownlist.SelectedIndex - 2].Filters);
                    filterNameBox.Text = _customFilters[dropdownlist.SelectedIndex - 2].Name;
                    DisplayFilterTags();
                    break;
            }
            RefreshVNList();
        }

        /// <summary>
        /// Delete custom filter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteCustomFilter(object sender, EventArgs e)
        {
            if (customFilters.SelectedIndex < 2) return; //shouldnt happen
            var askBox = MessageBox.Show(Resources.are_you_sure, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            var selectedFilter = customFilters.SelectedIndex;
            customFilters.Items.RemoveAt(selectedFilter);
            _customFilters.RemoveAt(selectedFilter - 2);
            SaveMainXML();
            WriteText(replyText, Resources.filter_deleted, true);
            filterNameBox.Text = "";
            customFilters.SelectedIndex = 0;
        }

        /// <summary>
        /// Whether Visual Novel contains a single tag (or a subtag).
        /// </summary>
        /// <param name="vn">Visual Novel to be checked</param>
        /// <param name="tag">Tag to be checked</param>
        /// <returns>Whether it contains the tag or a subtag.</returns>
        private static bool VNMatchesSingleTag(ListedVN vn, TagFilter tag)
        {
            int[] vnTags = StringToTags(vn.Tags).Select(x => x.ID).ToArray();
            return vnTags.Any(vntag => tag.AllIDs.Contains(vntag));
        }

        /// <summary>
        /// Whether Visual Novel matches list of active tag filters.
        /// </summary>
        /// <param name="vn">Visual Novel to be checked</param>
        /// <returns>Whether it matches</returns>
        private bool VNMatchesTagFilter(ListedVN vn)
        {
            int[] vnTags = StringToTags(vn.Tags).Select(x => x.ID).ToArray();
            //for each tag in list of active tag filters, add 1 to counter if vn has a tag that is either the specified tag or a subtag
            //if counter is the same as the amount of tags in active tag filters, it means it matched all of them, thus, it matches the whole filter.
            /*var filtersMatched = 0;
            foreach (var filter in _activeFilter)
            {
                if (vnTags.Any(vntag => filter.AllIDs.Contains(vntag))) filtersMatched++;
            }*/
            var filtersMatched = _activeTagFilter.Count(filter => vnTags.Any(vntag => filter.AllIDs.Contains(vntag)));
            return filtersMatched == _activeTagFilter.Count;
        }


    }
}
