using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Happy_Search.Properties;
using Happy_Apps_Core;
using static Happy_Apps_Core.StaticHelpers;

namespace Happy_Search.Other_Forms
{
    /// <summary>
    /// Tab containing controls to alter filter of VN list.
    /// </summary>
    public partial class FiltersTab : UserControl
    {
        private readonly FormMain _mainForm;
        private Filters _filters;
        private readonly BindingList<CustomFilter> _customFilters = new BindingList<CustomFilter>();

        /// <summary>
        /// Tab containing controls to alter filter of VN list.
        /// </summary>
        public FiltersTab(FormMain parentForm)
        {
            InitializeComponent();
            _mainForm = parentForm;
            var tagSource = new AutoCompleteStringCollection();
            tagSource.AddRange(DumpFiles.PlainTags.Select(v => v.Name).ToArray());
            tagOrTraitSearchBox.AutoCompleteCustomSource = tagSource;
            string[] traitRootNames = DumpFiles.PlainTraits.Where(x => x.TopmostParentName == null).Select(x => x.Name).ToArray();
            traitRootsDropdown.Items.Clear();
            foreach (var rootName in traitRootNames)
            {
                if (rootName == null) continue;
                traitRootsDropdown.Items.Add(rootName);
            }
            traitRootsDropdown.SelectedIndex = 0;
            customFilterReply.Text = "";
            tagOrTraitSearchResultBox.Text = "";
            _languages = LocalDatabase.VNList.Where(vn => vn.Languages != null).SelectMany(x => x.Languages.All).Distinct().Select(x => new CultureInfo(x).DisplayName).OrderBy(x => x).ToArray();
            _originalLanguages = LocalDatabase.VNList.Where(vn => vn.Languages != null).SelectMany(x => x.Languages.Originals).Distinct().Select(x => new CultureInfo(x).DisplayName).OrderBy(x => x).ToArray();
            InitialLoadFromFile();
            _mainForm.SetVNList(_filters.GetFunction(this), _filters.Name);
            _mainForm.LoadVNListToGui();

        }

        /// <summary>
        /// Languages of all releases
        /// </summary>
        private readonly string[] _languages;

        /// <summary>
        /// Languages of all original releases
        /// </summary>
        private readonly string[] _originalLanguages;


        /// <summary>
        /// Show Filters from object on GUI
        /// </summary>
        private void SetFiltersToGui()
        {
            //TODO
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            CustomFilter selectedItem = (CustomFilter)filterDropdown.SelectedItem;
            ChangeCustomFilter(sender, selectedItem);
        }

        private bool _customFilterBlock;

        /// <summary>
        /// Change to selected filter.
        /// </summary>
        public void ChangeCustomFilter(object sender, CustomFilter selectedItem)
        {
            if (string.IsNullOrWhiteSpace(selectedItem.Name)) return;
            if (_customFilterBlock) return;
            _customFilterBlock = true;
            filterDropdown.SelectedItem = selectedItem;
            _mainForm.filterDropdown.SelectedItem = selectedItem;
            _customFilterBlock = false;
            if (DontTriggerEvent) return;
            _filters.LoadCustomFilter(selectedItem);
            SetFiltersToGui();
            if (sender == _mainForm)
            {
                _mainForm.SetVNList(_filters.GetFunction(this), _filters.Name);
                _mainForm.LoadVNListToGui();
            }
        }

        private void RemoveFromListBox(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete) return;
            if (!(sender is ListBox listbox)) return;
            var selectedItem = listbox.SelectedItem;
            ((IList)listbox.DataSource).Remove(selectedItem);
        }

        private void SaveFilters() => _filters.SaveFilters(FiltersJson);

        private void SaveCustomFilter(object sender, EventArgs e)
        {
            var filterName = customFilterNameBox.Text;
            if (filterName.Length == 0)
            {
                WriteText(customFilterReply, "Enter name of filter.");
                return;
            }
            //Ask to overwrite if name entered is already in use
            var itemIndex = GetIndexOfCustomFilter(filterName);
            if (itemIndex > -1)
            {
                var askBox = MessageBox.Show(@"Do you wish to overwrite present custom filter?",
                    Resources.ask_overwrite, MessageBoxButtons.YesNo);
                if (askBox != DialogResult.Yes) return;
                DontTriggerEvent = true;
                _filters.Name = filterName;
                _customFilters[itemIndex] = (CustomFilter)_filters;
                filterDropdown.SelectedIndex = itemIndex;
                _filters.RefreshKind = RefreshType.NamedFilter;
                DontTriggerEvent = false;
            }
            else
            {
                _filters.Name = filterName;
                DontTriggerEvent = true;
                _customFilters.Add((CustomFilter)_filters);
                filterDropdown.SelectedIndex = _customFilters.Count - 1;
                _filters.RefreshKind = RefreshType.NamedFilter;
                DontTriggerEvent = false;
            }
            SaveObjectToJsonFile(_customFilters, CustomFiltersJson);
            WriteText(customFilterReply, Resources.filter_saved);
        }

        private int GetIndexOfCustomFilter(string filterName)
        {
            int index = 0;
            foreach (var customFilter in _customFilters)
            {
                if (customFilter.Name == filterName) return index;
                index++;
            }
            return -1;
        }

        internal void LeftFiltersTab()
        {
            switch (_filters.RefreshKind)
            {
                case RefreshType.None:
                    SaveFilters();
                    return;
                case RefreshType.UserChanged:
                    _mainForm.CurrentFilterLabel = "Custom Filter";
                    break;
                case RefreshType.NamedFilter:
                    _mainForm.CurrentFilterLabel = _filters.Name;
                    break;
            }
            _mainForm.SetVNList(_filters.GetFunction(this), _filters.Name);
            SaveFilters();
            _mainForm.LoadVNListToGui();
            _filters.RefreshKind = RefreshType.None;
        }

        private void Help_Filters(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            Debug.Assert(path != null, nameof(path) + " != null");
            var helpFile = $"{Path.Combine(path, "Program Data\\Help\\filtering.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

        private void InitialLoadFromFile()
        {
            _customFilters.Clear();
            var loadedCustomFilters = LoadObjectFromJsonFile<List<CustomFilter>>(CustomFiltersJson) ?? LoadObjectFromJsonFile<List<CustomFilter>>(DefaultFiltersJson);
            if (loadedCustomFilters != null) _customFilters.AddRange(loadedCustomFilters);
            _filters = Filters.LoadFixedFilter();
            DontTriggerEvent = true;
            filterDropdown.DataSource = _customFilters;
            _mainForm.filterDropdown.DataSource = _customFilters;
            if (_customFilters.Count > 0) filterDropdown.SelectedIndex = 0;
            if (_customFilters.Count > 0) _mainForm.filterDropdown.SelectedIndex = 0;
            DontTriggerEvent = false;
            SetFiltersToGui();
        }

        #region Tags


        /// <summary>
        /// Search for tags by name/alias entered by user.
        /// </summary>
        private void AddTagBySearch(string tagName, bool showResults)
        {
            //if exact match is found, add it
            var exact = DumpFiles.PlainTags.Find(tag => tag.Name.Equals(tagName, StringComparison.InvariantCultureIgnoreCase));
            if (exact != null && !showResults)
            {
                tagOrTraitSearchBox.Text = "";
                AddFilterTag(exact);
                return;
            }
            //find all results with similar name or alias
            var results = DumpFiles.PlainTags.Where(t => t.Name.ToLowerInvariant().Contains(tagName) ||
                                                         t.Aliases.Exists(a => a.ToLowerInvariant().Contains(tagName))).ToArray();
            //if no results, return not found
            if (results.Length == 0)
            {
                WriteError(tagOrTraitReply, $"Tag {tagOrTraitSearchBox.Text} not found.");
                return;
            }
            //if only one result, add it
            if (results.Length == 1)
            {
                tagOrTraitSearchBox.Text = "";
                AddFilterTag(results.First());
                return;
            }
            //if several results, show list.
            tagOrTraitSearchResultBox.Items.Clear();
            // ReSharper disable once CoVariantArrayConversion
            tagOrTraitSearchResultBox.Items.AddRange(results);
            tagOrTraitSearchResultBox.Visible = true;

        }

        /// <summary>
        /// Add tag to list of active tag filters.
        /// </summary>
        /// <param name="writtenTag">Tag to be added to active filter.</param>
        private void AddFilterTag(DumpFiles.WrittenTag writtenTag)
        {
            if (writtenTag == null)
            {
                WriteError(tagOrTraitReply, "Tag not found.");
                return;
            }
            //save tag as tag and children
            int[] children = Enumerable.Empty<int>().ToArray();
            var difference = 1;
            //new
            int[] childrenForThisRound = DumpFiles.PlainTags.Where(x => x.Parents.Contains(writtenTag.ID))
                .Select(x => x.ID).ToArray(); //at this moment, it contains direct subtags
            while (difference > 0)
            {
                var initial = children.Length;
                children = children.Union(childrenForThisRound).ToArray(); //first time, adds direct subtags, second time it adds 2-away subtags, etc...
                difference = children.Length - initial;
                var tmp = new List<int>();
                foreach (var child in childrenForThisRound) tmp.AddRange(DumpFiles.PlainTags.Where(x => x.Parents.Contains(child)).Select(x => x.ID));
                childrenForThisRound = tmp.ToArray();
            }
            var newFilter = new TagFilter(writtenTag.ID, writtenTag.Name, children);
            filterValueCombobox.DataSource = new[] { newFilter };
            WriteText(tagOrTraitReply, $"Tag {writtenTag.Name} selected.");
        }

        /// <summary>
        /// Whether Visual Novel matches list of active tag filters.
        /// </summary>
        /// <param name="vn">Visual Novel to be checked</param>
        /// <returns>Whether it matches</returns>
        public bool VNMatchesTagFilter(ListedVN vn)
        {
            //TODO
            /*int[] vnTags = vn.TagList.Select(t => t.ID).ToArray();
            var filtersMatched = _filters.Tags?.Count(filter => vnTags.Any(vntag => filter.AllIDs.Contains(vntag)));
            if (filtersMatched == null) return false;
            return filtersMatched == _filters.Tags.Count;*/
            return true;
        }


        #endregion

        #region Traits


        /// <summary>
        ///     Add trait with name entered by user under selected trait type.
        /// </summary>
        private void AddTraitBySearch(string traitName, bool showResults)
        {
            var root = DumpFiles.PlainTraits.Find(x => x.Name.Equals(traitRootsDropdown.SelectedItem));
            var exact = DumpFiles.PlainTraits.Find(x => x.Name.Equals(traitName, StringComparison.InvariantCultureIgnoreCase) &&
                    x.TopmostParent == root.ID);
            if (exact != null && !showResults)
            {
                tagOrTraitSearchBox.Text = "";
                filterValueCombobox.DataSource = new[] { exact };
                WriteText(tagOrTraitReply, $"Added trait {exact}");
                return;
            }
            tagOrTraitSearchResultBox.Visible = false;
            if (tagOrTraitSearchBox.Text == "") //check if box is empty
            {
                WriteError(tagOrTraitReply, "Enter trait name.");
                return;
            }
            var text = tagOrTraitSearchBox.Text.ToLowerInvariant();
            var results = DumpFiles.PlainTraits.Where(t => t.Name.ToLowerInvariant().Contains(text) ||
                                                           t.Aliases.Exists(a => a.ToLowerInvariant().Contains(text))).ToArray();
            if (results.Length == 0)
            {
                WriteError(tagOrTraitReply, "No traits with that name/alias found.");
                return;
            }
            if (results.Length == 1)
            {
                tagOrTraitSearchBox.Text = "";
                WriteText(tagOrTraitReply, $"Added trait {results.First()}");
                filterValueCombobox.DataSource = new[] { results.First() };
                return;
            }
            tagOrTraitSearchResultBox.Items.Clear();
            // ReSharper disable once CoVariantArrayConversion
            tagOrTraitSearchResultBox.Items.AddRange(results);
            tagOrTraitSearchResultBox.Visible = true;
        }

        /// <summary>
        ///     Change selected trait type.
        /// </summary>
        private void TraitRootChanged(object sender, EventArgs e)
        {
            if (traitRootsDropdown.SelectedIndex < 0) return;
            var trait = DumpFiles.PlainTraits.Find(x => x.Name.Equals(traitRootsDropdown.SelectedItem));
            if (trait == null)
            {
                WriteError(tagOrTraitReply, "Root trait not found.");
                return;
            }
            var traitSource = new AutoCompleteStringCollection();
            traitSource.AddRange(DumpFiles.PlainTraits.Where(x => x.TopmostParent == trait.ID).Select(x => x.Name).ToArray());
            tagOrTraitSearchBox.AutoCompleteCustomSource = traitSource;
        }

        #endregion
        //new

        /// <summary>
        /// Clear results list from view.
        /// </summary>
        private void ClearTagOrTraitResults(object sender, EventArgs e)
        {
            tagOrTraitSearchResultBox.Visible = false;
        }
        
        private void AddTagOrTraitBySearch(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            SearchTagsOrTraits(false);
        }

        private void AddTagOrTraitFromList(object sender, EventArgs e)
        {
            var type = filterTypeCombobox.SelectedItem as string;
            Debug.Assert(type != null, nameof(type) + " != null");
            if (type.Equals("Tags")) AddTagOrTraitFromList<DumpFiles.WrittenTag>();
            if (type.Equals("Traits")) AddTagOrTraitFromList<DumpFiles.WrittenTrait>();
        }

        private void SearchTagsOrTraits(object sender, EventArgs e)
        {
            SearchTagsOrTraits(true);
        }

        private void SearchTagsOrTraits(bool showResults)
        {
            var type = filterTypeCombobox.SelectedItem as string;
            Debug.Assert(type != null, nameof(type) + " != null");
            if (tagOrTraitSearchBox.Text == "") //check if box is empty
            {
                WriteError(tagOrTraitReply, $"Enter {type.Replace("s", "")} name.");
                return;
            }
            tagOrTraitSearchResultBox.Visible = false;
            string searchString = tagOrTraitSearchBox.Text.ToLowerInvariant();
            if (type.Equals("Tags")) AddTagBySearch(searchString, showResults);
            if (type.Equals("Traits")) AddTraitBySearch(searchString, showResults);
        }

        private void FilterTypeChanged(object sender, EventArgs e)
        {
            excludeFilterCheckbox.Visible = true;
            tagOrTraitReply.Text = "";
            traitRootsDropdown.Visible = false;
            tagOrTraitSearchBox.Visible = false;
            tagOrTraitSearchButton.Visible = false;
            object dataSource = null;
            switch (filterTypeCombobox.SelectedItem as string)
            {
                case "Length":
                    dataSource = (from LengthFilter item in Enum.GetValues(typeof(LengthFilter))
                                  select new ComboBoxItem(item, item.GetDescription())).ToList();
                    break;
                case "Released Between":
                    //TODO
                    break;
                case "Release Status":
                    dataSource = (from UnreleasedFilter item in Enum.GetValues(typeof(UnreleasedFilter))
                                  select new ComboBoxItem(item, item.GetDescription())).ToList();
                    break;
                case "Voted":
                case "Blacklisted":
                case "By Favorite Producer":
                    excludeFilterCheckbox.Visible = false;
                    dataSource = new[] { "Include", "Exclude" };
                    break;
                case "Wishlist Status":
                    dataSource = (from WishlistStatus item in Enum.GetValues(typeof(WishlistStatus))
                                  select new ComboBoxItem(item, item.ToString())).ToList();
                    break;
                case "Userlist Status":
                    dataSource = (from UserlistStatus item in Enum.GetValues(typeof(UserlistStatus))
                                  select new ComboBoxItem(item, item.ToString())).ToList();
                    break;
                case "Language":
                    dataSource = _languages;
                    break;
                case "Original Language":
                    dataSource = _originalLanguages;
                    break;
                case "Tags":
                    //TODO
                    tagOrTraitSearchBox.Visible = true;
                    tagOrTraitSearchButton.Visible = true;
                    break;
                case "Traits":
                    //TODO
                    traitRootsDropdown.Visible = true;
                    tagOrTraitSearchBox.Visible = true;
                    tagOrTraitSearchButton.Visible = true;
                    break;
            }
            filterValueCombobox.DataSource = dataSource;
        }

        private void AddPermanentFilterClick(object sender, EventArgs e)
        {
            var item = filterValueCombobox.SelectedItem;
            var filter = new NewFilter(filterTypeCombobox.SelectedItem as string, item, excludeFilterCheckbox.Checked);
            if (orGroupCheckbox.Checked) permanentOrListbox.Items.Add(filter);
            else permanentAndListbox.Items.Add(filter);
        }
        private void AddFilterClick(object sender, EventArgs e)
        {
            var item = filterValueCombobox.SelectedItem;
            var filter = new NewFilter(filterTypeCombobox.SelectedItem as string, item, excludeFilterCheckbox.Checked);
            if (orGroupCheckbox.Checked) customOrListbox.Items.Add(filter);
            else customAndListbox.Items.Add(filter);
        }

        /// <summary>
        /// Add trait from search results.
        /// </summary>
        private void AddTagOrTraitFromList<T>() where T : DumpFiles.ItemWithParents
        {
            tagOrTraitSearchBox.Text = "";
            tagOrTraitSearchResultBox.Visible = false;
            filterValueCombobox.DataSource = new[] { tagOrTraitSearchResultBox.SelectedItem as T };
        }


        /// <summary>
        /// New custom filter class
        /// </summary>
        public class NewFilter
        {
            private readonly string _filterType;
            private readonly object _value;
            private readonly bool _exclude;
            private readonly bool _yesNoType;

            /// <summary>
            /// Create custom filter
            /// </summary>
            /// <param name="type"></param>
            /// <param name="value"></param>
            /// <param name="exclude"></param>
            public NewFilter(string type, object value, bool exclude)
            {
                _filterType = type;
                _value = value;
                if (type == "Voted" || type == "Blacklisted" || type == "By Favorite Producer")
                {
                    _yesNoType = true;
                    _exclude = false;
                }
                else _exclude = exclude;
            }

            /// <inheritdoc />
            public override string ToString()
            {
                return _yesNoType ? $"{_value}: {_filterType}" : $"{(_exclude ? "Exclude" : "Include")}: {_filterType} - {_value}";
            }
        }

    }
}
