using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Happy_Search.Other_Forms;
using Happy_Search.Properties;
using Newtonsoft.Json;
using static Happy_Search.StaticHelpers;

namespace Happy_Search
{
    public partial class FormMain
    {
        private readonly List<CustomTagFilter> _customTagFilters;
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
            var customFilter = _customTagFilters.Find(x => x.Name.Equals(filterName));
            if (customFilter != null)
            {
                var askBox = MessageBox.Show(@"Do you wish to overwrite present custom filter?",
                    Resources.ask_overwrite, MessageBoxButtons.YesNo);
                if (askBox != DialogResult.Yes) return;
                customFilter.Filters = TagsPre.ToList();
                customFilter.Updated = DateTime.UtcNow;
                customTagFilters.SelectedIndex = customTagFilters.Items.IndexOf(filterName);
            }
            else
            {
                customTagFilters.Items.Add(filterName);
                _customTagFilters.Add(new CustomTagFilter(filterName, TagsPre.ToList()));
                customTagFilters.SelectedIndex = customTagFilters.Items.Count - 1;
            }
            SaveMainXML();
            WriteText(tagReply, Resources.filter_saved);
        }

        /// <summary>
        /// Clear list of active tag filters (Does not change custom filters).
        /// </summary>
        private void ClearTagFilter(object sender, EventArgs e)
        {
            TagsPre.Clear();
            customTagFilters.SelectedIndex = 0;
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
            int[] childrenForThisRound = PlainTags.Where(x => x.Parents.Contains(writtenTag.ID))
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
                foreach (var child in childrenForThisRound) tmp.AddRange(PlainTags.Where(x => x.Parents.Contains(child)).Select(x => x.ID));
                childrenForThisRound = tmp.ToArray();
            }
            var newFilter = new TagFilter(writtenTag.ID, writtenTag.Name, -1, children);
            var count = _vnList.Count(vn => VNMatchesSingleTag(vn, newFilter));
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
            if (customTagFilters.SelectedIndex > 1)
            {
                var selectedFilter = _customTagFilters[customTagFilters.SelectedIndex - 2];
                var message = selectedFilter.Updated != DateTime.MinValue
                    ? $"This filter was last updated {DaysSince(selectedFilter.Updated)} days ago.\n{Resources.update_custom_filter}"
                    : Resources.update_custom_filter;
                var askBox2 = MessageBox.Show(message, Resources.are_you_sure, MessageBoxButtons.YesNo);
                if (askBox2 != DialogResult.Yes) return;
                var result = StartQuery(tagReply, "Update Filter Results", true, true, false);
                if (!result) return;
                await UpdateFilterResults();
                _customTagFilters[customTagFilters.SelectedIndex - 2].Updated = DateTime.UtcNow;
                SaveMainXML();
            }
            else
            {
                var askBox = MessageBox.Show(Resources.update_custom_filter, Resources.are_you_sure, MessageBoxButtons.YesNo);
                if (askBox != DialogResult.Yes) return;
                var result = StartQuery(tagReply, "Update Filter Results", true, true, false);
                if (!result) return;
                await UpdateFilterResults();
            }
            ChangeAPIStatus(VndbConnection.APIStatus.Ready);
        }

        /// <summary>
        /// Get all VNs that match the list of active filters which were not already in local database, if '10 Year Limit' is enabled, only titles released in the last 10 years will be fetched.
        /// </summary>
        private async Task UpdateFilterResults()
        {
            _vnsAdded = 0;
            _vnsSkipped = 0;
            IEnumerable<string> betterTags = TagsPre.Select(x => x.ID).Select(s => $"tags = {s}");
            var tags = string.Join(" and ", betterTags);
            string tagQuery = $"get vn basic ({tags}) {{{MaxResultsString}}}";
            var result = await TryQuery(tagQuery, "UCF Query Error");
            if (!result) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            if (vnRoot.Num == 0) return;
            List<VNItem> vnItems = vnRoot.Items;
            await GetMultipleVN(vnItems.Select(x => x.ID).ToList(), false);
            var pageNo = 1;
            var moreResults = vnRoot.More;
            while (moreResults)
            {
                pageNo++;
                string moreTagQuery = $"get vn basic ({tags}) {{{MaxResultsString}, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(moreTagQuery, "UCFM Query Error");
                if (!moreResult) return;
                var moreVNRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                if (vnRoot.Num == 0) break;
                List<VNItem> moreVNItems = moreVNRoot.Items;
                await GetMultipleVN(moreVNItems.Select(x => x.ID).ToList(), false);
                moreResults = moreVNRoot.More;
            }
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            WriteText(ActiveQuery.ReplyLabel, $"Update complete, added {_vnsAdded} and skipped {_vnsSkipped} titles.");
        }

        /// <summary>
        /// Display VNs matching tags in selected custom filter.
        /// </summary>
        private void Filter_CustomTags(object sender, EventArgs e)
        {
            if (DontTriggerEvent) return;
            var dropdownlist = (ComboBox)sender;
            switch (dropdownlist.SelectedIndex)
            {
                case 0:
                    deleteCustomTagFilterButton.Enabled = false;
                    return;
                case 1:
                    dropdownlist.SelectedIndex = 0;
                    return;
                default:
                    deleteCustomTagFilterButton.Enabled = true;
                    TagsPre.Clear();
                    TagsPre.AddRange(_customTagFilters[dropdownlist.SelectedIndex - 2].Filters.ToArray());
                    customTagFilterNameBox.Text = _customTagFilters[dropdownlist.SelectedIndex - 2].Name;
                    break;
            }
        }

        /// <summary>
        /// Delete custom tag filter.
        /// </summary>
        private void DeleteCustomTagFilter(object sender, EventArgs e)
        {
            if (customTagFilters.SelectedIndex < 2) return; //shouldnt happen
            var askBox = MessageBox.Show(Resources.are_you_sure, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            var selectedFilter = customTagFilters.SelectedIndex;
            customTagFilters.Items.RemoveAt(selectedFilter);
            _customTagFilters.RemoveAt(selectedFilter - 2);
            SaveMainXML();
            WriteText(tagReply, Resources.filter_deleted);
            customTagFilterNameBox.Text = "";
            customTagFilters.SelectedIndex = 0;
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
            var filtersMatched = Filters.Tags.Count(filter => vnTags.Any(vntag => filter.AllIDs.Contains(vntag)));
            return filtersMatched == Filters.Tags.Length;
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
            var exact = PlainTags.Find(tag => tag.Name.Equals(text, StringComparison.InvariantCultureIgnoreCase));
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
            var results = PlainTags.Where(t => t.Name.ToLowerInvariant().Contains(text) ||
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

        /// <summary>
        ///     Holds details of user-created custom filter
        /// </summary>
        [Serializable, XmlRoot("CustomTagFilter")]
        public class CustomTagFilter
        {
            /// <summary>
            ///     Constructor for Custom Tag Filter.
            /// </summary>
            /// <param name="name">User-set name of filter</param>
            /// <param name="filters">List of Tags in filter</param>
            public CustomTagFilter(string name, List<TagFilter> filters)
            {
                Name = name;
                Filters = filters;
            }

            /// <summary>
            ///     Empty Constructor needed for XML.
            /// </summary>
            public CustomTagFilter()
            {
            }

            /// <summary>
            ///     User-set name of custom filter
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            ///     List of tags in custom filter
            /// </summary>
            public List<TagFilter> Filters { get; set; }

            /// <summary>
            ///     Date of last update to custom filter
            /// </summary>
            public DateTime Updated { get; set; }
        }

    }

    /// <summary>
    ///     Holds details of a VNDB Tag and its subtags
    /// </summary>
    [Serializable, XmlRoot("TagFilter")]
    public class TagFilter
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="titles"></param>
        /// <param name="children"></param>
        public TagFilter(int id, string name, int titles, int[] children)
        {
            ID = id;
            Name = name;
            Titles = titles;
            Children = children;
            AllIDs = children.Union(new[] { id }).ToArray();
        }

        /// <summary>
        ///     Empty Constructor needed for XML.
        /// </summary>
        public TagFilter()
        {
        }

        /// <summary>
        ///     ID of tag.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     Name of tag.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Number of titles with tag.
        /// </summary>
        public int Titles { get; set; }

        /// <summary>
        ///     Subtag IDs of tag.
        /// </summary>
        public int[] Children { get; set; }

        /// <summary>
        ///     Tag ID and subtag IDs
        /// </summary>
        public int[] AllIDs { get; set; }


        /// <summary>
        ///     Check if given tag is a child tag of TagFilter
        /// </summary>
        /// <param name="tag">Tag to be checked</param>
        /// <returns>Whether tag is child of TagFilter</returns>
        public bool HasChild(int tag)
        {
            return Children.Contains(tag);
        }


        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return $"{ID} - {Name}";
        }
    }
}
