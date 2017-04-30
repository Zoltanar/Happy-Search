using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Happy_Search.Properties;
using Newtonsoft.Json;
using static Happy_Search.StaticHelpers;

namespace Happy_Search.Other_Forms
{
    partial class FiltersTab
    {
        /// <summary>
        /// List of saved custom tag filters.
        /// </summary>
        private readonly BindingList<CustomTagFilter> _customTagFilters = new BindingList<CustomTagFilter>();
        /// <summary>
        /// Contains contents of TagsTFLB (not active tags);
        /// </summary>
        public readonly BindingList<TagFilter> TagsPre = new BindingList<TagFilter>();

        /// <summary>
        /// Bring up dialog explaining features of the 'Tag Filtering' section.
        /// </summary>
        private void Help_TagFiltering(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            Debug.Assert(path != null, "path != null");
            var helpFile = $"{Path.Combine(path, "Program Data\\Help\\tagfiltering.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

        private void EnterCustomTagFilterName(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SaveCustomTagFilter(sender, e);
        }

        /// <summary>
        /// Save list of active tag filters to file as a custom filter with user-entered name.
        /// </summary>
        private void SaveCustomTagFilter(object sender, EventArgs e)
        {
            var filterName = customTagFilterNameBox.Text;
            if (filterName.Length == 0)
            {
                WriteText(tagReply, "Enter name of filter.");
                return;
            }
            //Ask to overwrite if name entered is already in use
            var customFilter = _customTagFilters.FirstOrDefault(x => x.Name.Equals(filterName));
            if (customFilter != null)
            {
                var askBox = MessageBox.Show(@"Do you wish to overwrite present custom filter?",
                    Resources.ask_overwrite, MessageBoxButtons.YesNo);
                if (askBox != DialogResult.Yes) return;
                customFilter.Filters = TagsPre.ToArray();
                customFilter.Updated = DateTime.UtcNow;
                DontTriggerEvent = true;
                tagFiltersCB.SelectedIndex = tagFiltersCB.Items.IndexOf(customFilter);
                DontTriggerEvent = false;
            }
            else
            {
                DontTriggerEvent = true;
                _customTagFilters.Add(new CustomTagFilter(filterName, TagsPre.ToArray()));
                tagFiltersCB.SelectedIndex = _customTagFilters.Count - 1;
                DontTriggerEvent = false;
            }
            SaveObjectToJsonFile(_customTagFilters, CustomTagFiltersJson);
            WriteText(tagReply, Resources.filter_saved);
        }

        /// <summary>
        /// Clear list of active tag filters (Does not change custom filters).
        /// </summary>
        private void ClearTagFilter(object sender, EventArgs e)
        {
            TagsPre.Clear();
            tagFiltersCB.SelectedIndex = 0;
            WriteText(tagReply, Resources.filter_cleared);
        }

        /// <summary>
        /// Add tag to list of active tag filters.
        /// </summary>
        /// <param name="writtenTag">Tag to be added to active filter.</param>
        private void AddFilterTag(WrittenTag writtenTag)
        {
            if (writtenTag == null)
            {
                WriteError(tagReply, "Tag not found.");
                return;
            }
            foreach (var filter in TagsPre)
            {
                //if tag is already in filter, do nothing.
                if (filter.ID == writtenTag.ID)
                {
                    WriteError(tagReply, "Tag is already in filter.");
                    return;
                }
                //if filter isn't parent of tag, continue
                if (!filter.HasChild(writtenTag.ID)) continue;
                //if filter is parent of tag, remove filter (because it would be useless in the presence of more specific tag)
                //eg transfer student heroine is more precise than student heroine
                TagsPre.Remove(filter);
                break;
            }
            //save tag as tag and children
            int[] children = Enumerable.Empty<int>().ToArray();
            var difference = 1;
            //new
            int[] childrenForThisRound = FormMain.PlainTags.Where(x => x.Parents.Contains(writtenTag.ID))
                .Select(x => x.ID).ToArray(); //at this moment, it contains direct subtags
            while (difference > 0)
            {
                var initial = children.Length;
                //debug printout
                //IEnumerable<WrittenTag> debuglist = childrenForThisRound.Select(childID => PlainTags.Find(x => x.ID == childID));
                //Debug.WriteLine(string.Join(", ", debuglist));
                //
                children = children.Union(childrenForThisRound).ToArray(); //first time, adds direct subtags, second time it adds 2-away subtags, etc...
                difference = children.Length - initial;
                var tmp = new List<int>();
                foreach (var child in childrenForThisRound) tmp.AddRange(FormMain.PlainTags.Where(x => x.Parents.Contains(child)).Select(x => x.ID));
                childrenForThisRound = tmp.ToArray();
            }
            var newFilter = new TagFilter(writtenTag.ID, writtenTag.Name, -1, children);
            var count = _mainForm.VNList.Count(vn => VNMatchesSingleTag(vn, newFilter));
            newFilter.Titles = count;
            TagFilter moreSpecificFilter = null;
            var notNeeded = false;
            foreach (var filter in TagsPre)
            {
                if (!newFilter.HasChild(filter.ID)) continue;
                notNeeded = true;
                moreSpecificFilter = filter;
                break;
            }
            if (notNeeded)
            {
                WriteText(tagReply, $"Tag isn't needed because {moreSpecificFilter.Name} is more specific.");
                return;
            }
            TagsPre.Add(newFilter);
            WriteText(tagReply, $"Tag {writtenTag.Name} has {count} VNs in local database.");
        }

        /// <summary>
        /// Update results of list of active filters.
        /// </summary>
        private async void UpdateTagResults(object sender, EventArgs e)
        {
            if (tagFiltersCB.SelectedIndex > 0)
            {
                var selectedFilter = tagFiltersCB.SelectedItem as CustomTagFilter;
                if (selectedFilter == null) return; //shouldnt happen
                var message = selectedFilter.Updated != DateTime.MinValue
                    ? $"This filter was last updated {DaysSince(selectedFilter.Updated)} days ago.\n{Resources.update_custom_filter}"
                    : Resources.update_custom_filter;
                var askBox2 = MessageBox.Show(message, Resources.are_you_sure, MessageBoxButtons.YesNo);
                if (askBox2 != DialogResult.Yes) return;
                var result = _mainForm.StartQuery(tagReply, "Update Filter Results", true, true, false);
                if (!result) return;
                await UpdateFilterResults();
                selectedFilter.Updated = DateTime.UtcNow;
                SaveObjectToJsonFile(_customTagFilters, CustomTagFiltersJson);
            }
            else
            {
                var askBox = MessageBox.Show(Resources.update_custom_filter, Resources.are_you_sure, MessageBoxButtons.YesNo);
                if (askBox != DialogResult.Yes) return;
                var result = _mainForm.StartQuery(tagReply, "Update Filter Results", true, true, false);
                if (!result) return;
                await UpdateFilterResults();
            }
            _mainForm.ChangeAPIStatus(VndbConnection.APIStatus.Ready);
        }

        /// <summary>
        /// Get all VNs that match the list of active filters which were not already in local database, if '10 Year Limit' is enabled, only titles released in the last 10 years will be fetched.
        /// </summary>
        private async Task UpdateFilterResults()
        {
            IEnumerable<string> betterTags = TagsPre.Select(x => x.ID).Select(s => $"tags = {s}");
            var tags = string.Join(" and ", betterTags);
            string tagQuery = $"get vn basic ({tags}) {{{MaxResultsString}}}";
            var result = await _mainForm.TryQuery(tagQuery, "UCF Query Error");
            if (!result) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(_mainForm.Conn.LastResponse.JsonPayload);
            if (vnRoot.Num == 0) return;
            List<VNItem> vnItems = vnRoot.Items;
            await _mainForm.GetMultipleVN(vnItems.Select(x => x.ID).ToList(), false);
            var pageNo = 1;
            var moreResults = vnRoot.More;
            while (moreResults)
            {
                pageNo++;
                string moreTagQuery = $"get vn basic ({tags}) {{{MaxResultsString}, \"page\":{pageNo}}}";
                var moreResult = await _mainForm.TryQuery(moreTagQuery, "UCFM Query Error");
                if (!moreResult) return;
                var moreVNRoot = JsonConvert.DeserializeObject<VNRoot>(_mainForm.Conn.LastResponse.JsonPayload);
                if (vnRoot.Num == 0) break;
                List<VNItem> moreVNItems = moreVNRoot.Items;
                await _mainForm.GetMultipleVN(moreVNItems.Select(x => x.ID).ToList(), false);
                moreResults = moreVNRoot.More;
            }
            await _mainForm.ReloadListsFromDbAsync();
            _mainForm.LoadVNListToGui();
            WriteText(_mainForm.ActiveQuery.ReplyLabel, $"Update complete, added {_mainForm.TitlesAdded} and skipped {_mainForm.TitlesSkipped} titles.");
        }

        /// <summary>
        /// Display VNs matching tags in selected custom filter.
        /// </summary>
        private void Filter_CustomTags(object sender, EventArgs e)
        {
            if (DontTriggerEvent) return;
            var selectedItem = tagFiltersCB.SelectedItem as CustomTagFilter;
            if (selectedItem == null || selectedItem.Name == "Select Filter")
            {
                customTagFilterNameBox.Text = "";
                deleteCustomTagFilterButton.Enabled = false;
                return;
            }
            deleteCustomTagFilterButton.Enabled = true;
            customTagFilterNameBox.Text = selectedItem.Name;
            TagsPre.Clear();
            TagsPre.AddRange(selectedItem.Filters.ToArray());
        }

        /// <summary>
        /// Delete custom tag filter.
        /// </summary>
        private void DeleteCustomTagFilter(object sender, EventArgs e)
        {
            if (tagFiltersCB.SelectedIndex < 1) return; //shouldnt happen
            var askBox = MessageBox.Show(Resources.are_you_sure, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            _customTagFilters.RemoveAt(tagFiltersCB.SelectedIndex);
            SaveObjectToJsonFile(_customTagFilters, CustomTagFiltersJson);
            WriteText(tagReply, Resources.filter_deleted);
            tagFiltersCB.SelectedIndex = 0;
        }

        /// <summary>
        /// Whether Visual Novel contains a single tag (or a subtag).
        /// </summary>
        /// <param name="vn">Visual Novel to be checked</param>
        /// <param name="tag">Tag to be checked</param>
        /// <returns>Whether it contains the tag or a subtag.</returns>
        private static bool VNMatchesSingleTag(ListedVN vn, TagFilter tag)
        {
            return vn.TagList.Select(t => t.ID).Any(vntag => tag.AllIDs.Contains(vntag));
        }

        /// <summary>
        /// Whether Visual Novel matches list of active tag filters.
        /// </summary>
        /// <param name="vn">Visual Novel to be checked</param>
        /// <returns>Whether it matches</returns>
        public bool VNMatchesTagFilter(ListedVN vn)
        {
            int[] vnTags = vn.TagList.Select(t => t.ID).ToArray();
            var filtersMatched = _filters.Tags.Count(filter => vnTags.Any(vntag => filter.AllIDs.Contains(vntag)));
            return filtersMatched == _filters.Tags.Length;
        }

        /// <summary>
        /// Search for tags by name/alias entered by user.
        /// </summary>
        private void AddTagBySearch(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            tagSearchResultBox.Visible = false;
            if (tagSearchBox.Text == "") //check if box is empty
            {
                WriteError(tagReply, "Enter tag name.");
                return;
            }
            var tb = (TextBox)sender;
            string text = tb.Text.ToLowerInvariant();
            //if exact match is found, add it
            var exact = FormMain.PlainTags.Find(tag => tag.Name.Equals(text, StringComparison.InvariantCultureIgnoreCase));
            if (exact != null)
            {
                tagSearchBox.Text = "";
                AddFilterTag(exact);
                return;
            }
            SearchTags(null, null);

        }

        /// <summary>
        /// Add tag selected from list.
        /// </summary>
        private void AddTagFromList(object sender, EventArgs e)
        {
            var lb = (ListBox)sender;
            tagSearchBox.Text = "";
            lb.Visible = false;
            AddFilterTag(lb.SelectedItem as WrittenTag);
        }

        /// <summary>
        /// Search for tags by name/alias entered by user, doesn't simply add exact match.
        /// </summary>
        private void SearchTags(object sender, EventArgs e)
        {
            tagSearchResultBox.Visible = false;
            string text = tagSearchBox.Text.ToLowerInvariant();
            if (e != null)
            {
                if (tagSearchBox.Text == "") //check if box is empty
                {
                    WriteError(tagReply, "Enter tag name.");
                    return;
                }
            }
            //find all results with similar name or alias
            var results = FormMain.PlainTags.Where(t => t.Name.ToLowerInvariant().Contains(text) ||
                                               t.Aliases.Exists(a => a.ToLowerInvariant().Contains(text))).ToArray();
            //if no results, return not found
            if (results.Length == 0)
            {
                WriteError(tagReply, $"Tag {tagSearchBox.Text} not found.");
                return;
            }
            //if only one result, add it
            if (results.Length == 1)
            {
                tagSearchBox.Text = "";
                AddFilterTag(results.First());
                return;
            }
            //if several results, show list.
            tagSearchResultBox.Items.Clear();
            // ReSharper disable once CoVariantArrayConversion
            tagSearchResultBox.Items.AddRange(results);
            tagSearchResultBox.Visible = true;
        }
    }
}
