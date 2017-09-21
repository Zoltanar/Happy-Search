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
        private CustomFilter _permanentFilter;
        private CustomFilter _customFilter;
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
            filterTypeCombobox.DataSource = Enum.GetValues(typeof(FilterItem.FilterType))
                .Cast<FilterItem.FilterType>()
                .Select(x => new ComboBoxItem(x, x.GetDescription())).ToList();
            IEnumerable<CultureInfo> languagesPre = LocalDatabase.VNList.Where(vn => vn.Languages != null).SelectMany(x => x.Languages.All).Distinct().Select(CultureInfo.GetCultureInfo);
            _languages = languagesPre.OrderBy(x => x.DisplayName).Select(ci => new ComboBoxItem(ci.Name, ci.DisplayName)).ToArray();
            IEnumerable<CultureInfo> originalLanguagesPre = LocalDatabase.VNList.Where(vn => vn.Languages != null).SelectMany(x => x.Languages.Originals).Distinct().Select(CultureInfo.GetCultureInfo);
            _originalLanguages = originalLanguagesPre.OrderBy(x => x.DisplayName).Select(ci => new ComboBoxItem(ci.Name, ci.DisplayName)).ToArray();
            InitialLoadFromFile();
            _mainForm.SetVNList(GetFilterFunction(), _customFilter.Name);
            _mainForm.LoadVNListToGui();

        }

        private Func<ListedVN, bool> GetFilterFunction()
        {
            Func<ListedVN, bool>[] andFunctions = _permanentFilter.AndFilters.Concat(_customFilter.AndFilters).Select(filter => filter.GetFunction()).ToArray();
            Func<ListedVN, bool>[] orFunctions = _permanentFilter.OrFilters.Concat(_customFilter.OrFilters).Select(filter => filter.GetFunction()).ToArray();
            //if all and functions are true and 1+ or function is true
            if (andFunctions.Length + orFunctions.Length == 0) return vn => true;
            if (andFunctions.Length > 0 && orFunctions.Length == 0) return vn => andFunctions.All(x => x(vn));
            if (andFunctions.Length == 0 && orFunctions.Length > 0) return vn => orFunctions.Any(x => x(vn));
            return vn => andFunctions.All(x => x(vn)) && orFunctions.Any(x => x(vn));
        }

        /// <summary>
        /// Languages of all releases
        /// </summary>
        private readonly ComboBoxItem[] _languages;

        /// <summary>
        /// Languages of all original releases
        /// </summary>
        private readonly ComboBoxItem[] _originalLanguages;


        /// <summary>
        /// Show Filters from object on GUI
        /// </summary>
        private void SetFiltersToGui()
        {
            permanentAndListbox.DataSource = _permanentFilter.AndFilters;
            permanentOrListbox.DataSource = _permanentFilter.OrFilters;
            customAndListbox.DataSource = _customFilter.AndFilters;
            customOrListbox.DataSource = _customFilter.OrFilters;
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            var selectedItem = (CustomFilter)filterDropdown.SelectedItem;
            ChangeCustomFilter(sender, selectedItem);
        }

        private bool _customFilterBlock;

        /// <summary>
        /// Change to selected filter.
        /// </summary>
        public void ChangeCustomFilter(object sender, CustomFilter selectedItem)
        {
            if (string.IsNullOrWhiteSpace(selectedItem.Name)) return;
            if (selectedItem.Name == _customFilter.Name) return;
            if (_customFilterBlock) return;
            _customFilterBlock = true;
            filterDropdown.SelectedItem = selectedItem;
            _mainForm.filterDropdown.SelectedItem = selectedItem;
            _customFilterBlock = false;
            if (DontTriggerEvent) return;
            _customFilter = new CustomFilter(selectedItem);
            SetFiltersToGui();
            if (sender == _mainForm)
            {
                _mainForm.SetVNList(GetFilterFunction(), _customFilter.Name);
                _mainForm.LoadVNListToGui();
            }
            else _refreshKind = RefreshType.NamedFilter;
        }

        private void RemoveFromListBox(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete) return;
            if (!(sender is ListBox listbox)) return;
            var selectedItem = listbox.SelectedItem;
            ((IList)listbox.DataSource).Remove(selectedItem);
        }

        private void SaveFilters()
        {
            SaveObjectToJsonFile(_permanentFilter, PermanentFilterJson);
            SaveObjectToJsonFile(_customFilters, CustomFiltersJson);
        }

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
                _customFilter.Name = filterName;
                _customFilters[itemIndex] = new CustomFilter(_customFilter);
                filterDropdown.SelectedIndex = itemIndex;
                _refreshKind = RefreshType.NamedFilter;
                DontTriggerEvent = false;
            }
            else
            {
                _customFilter.Name = filterName;
                DontTriggerEvent = true;
                _customFilters.Add(new CustomFilter(_customFilter));
                filterDropdown.SelectedIndex = _customFilters.Count - 1;
                _refreshKind = RefreshType.NamedFilter;
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

        private RefreshType _refreshKind;

        internal void LeftFiltersTab()
        {
            switch (_refreshKind)
            {
                case RefreshType.None:
                    return;
                case RefreshType.UserChanged:
                    _customFilter.Name = "Custom Filter";
                    break;
            }
            _mainForm.SetVNList(GetFilterFunction(), _customFilter.Name);
            SaveFilters();
            _mainForm.LoadVNListToGui();
            _refreshKind = RefreshType.None;
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
            _permanentFilter = LoadObjectFromJsonFile<CustomFilter>(PermanentFilterJson) ?? new CustomFilter();
            DontTriggerEvent = true;
            _customFilter = _customFilters.Count > 0 ? new CustomFilter(_customFilters.First()) : new CustomFilter();
            filterDropdown.DataSource = _customFilters;
            _mainForm.filterDropdown.DataSource = _customFilters;
            filterDropdown.SelectedIndex = 0;
            _mainForm.filterDropdown.SelectedIndex = 0;
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
            DumpFiles.WrittenTag[] results = DumpFiles.PlainTags.Where(t => t.Name.ToLowerInvariant().Contains(tagName) ||
                                                         t.Aliases.Exists(a => a.ToLowerInvariant().Contains(tagName))).ToArray();
            //if no results, return not found
            switch (results.Length)
            {
                case 0:
                    WriteError(tagOrTraitReply, $"Tag {tagOrTraitSearchBox.Text} not found.");
                    return;
                //if only one result, add it
                case 1:
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
            filterValueCombobox.DataSource = new[] { new ComboBoxItem(writtenTag.ID, writtenTag.Name) };
            WriteText(tagOrTraitReply, $"Tag {writtenTag.Name} selected.");
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
                filterValueCombobox.DataSource = new[] { new ComboBoxItem(exact.ID, exact.Name) };
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
            DumpFiles.WrittenTrait[] results = DumpFiles.PlainTraits.Where(t => t.Name.ToLowerInvariant().Contains(text) ||
                                                           t.Aliases.Exists(a => a.ToLowerInvariant().Contains(text))).ToArray();
            switch (results.Length)
            {
                case 0:
                    WriteError(tagOrTraitReply, "No traits with that name/alias found.");
                    return;
                case 1:
                    tagOrTraitSearchBox.Text = "";
                    WriteText(tagOrTraitReply, $"Added trait {results.First()}");
                    filterValueCombobox.DataSource = new[] { new ComboBoxItem(results.First().ID, results.First().Name) };
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
            // ReSharper disable once PossibleNullReferenceException
            var type = (FilterItem.FilterType)(filterTypeCombobox.SelectedItem as ComboBoxItem)?.Key;
            if (type == FilterItem.FilterType.Tags) AddTagOrTraitFromList<DumpFiles.WrittenTag>();
            if (type == FilterItem.FilterType.Traits) AddTagOrTraitFromList<DumpFiles.WrittenTrait>();
        }

        private void SearchTagsOrTraits(object sender, EventArgs e)
        {
            SearchTagsOrTraits(true);
        }

        private void SearchTagsOrTraits(bool showResults)
        {
            // ReSharper disable once PossibleNullReferenceException
            var type = (FilterItem.FilterType)(filterTypeCombobox.SelectedItem as ComboBoxItem)?.Key;
            if (tagOrTraitSearchBox.Text == "") //check if box is empty
            {
                WriteError(tagOrTraitReply, $"Enter {type.ToString().Replace("s", "")} name.");
                return;
            }
            tagOrTraitSearchResultBox.Visible = false;
            string searchString = tagOrTraitSearchBox.Text.ToLowerInvariant();
            if (type == FilterItem.FilterType.Tags) AddTagBySearch(searchString, showResults);
            if (type == FilterItem.FilterType.Traits) AddTraitBySearch(searchString, showResults);
        }

        private void FilterTypeChanged(object sender, EventArgs e)
        {
            if (!(filterTypeCombobox.SelectedItem is ComboBoxItem uiObj)) return;
            var type = (FilterItem.FilterType)uiObj.Key;
            excludeFilterCheckbox.Visible = true;
            tagOrTraitReply.Text = "";
            traitRootsDropdown.Visible = false;
            tagOrTraitSearchBox.Visible = false;
            tagOrTraitSearchButton.Visible = false;
            object dataSource = null;
            switch (type)
            {
                case FilterItem.FilterType.Length:
                    dataSource = (from LengthFilter item in Enum.GetValues(typeof(LengthFilter))
                                  select new ComboBoxItem(item, item.GetDescription())).ToList();
                    break;
                case FilterItem.FilterType.ReleaseStatus:
                    dataSource = (from UnreleasedFilter item in Enum.GetValues(typeof(UnreleasedFilter))
                                  select new ComboBoxItem(item, item.GetDescription())).ToList();
                    break;
                case FilterItem.FilterType.Voted:
                case FilterItem.FilterType.Blacklisted:
                case FilterItem.FilterType.ByFavoriteProducer:
                    excludeFilterCheckbox.Visible = false;
                    dataSource = new[] { new ComboBoxItem(true, "Include"), new ComboBoxItem(false, "Exclude") };
                    break;
                case FilterItem.FilterType.WishlistStatus:
                    dataSource = (from WishlistStatus item in Enum.GetValues(typeof(WishlistStatus))
                                  select new ComboBoxItem(item, item.ToString())).ToList();
                    break;
                case FilterItem.FilterType.UserlistStatus:
                    dataSource = (from UserlistStatus item in Enum.GetValues(typeof(UserlistStatus))
                                  select new ComboBoxItem(item, item.ToString())).ToList();
                    break;
                case FilterItem.FilterType.Language:
                    dataSource = _languages;
                    break;
                case FilterItem.FilterType.OriginalLanguage:
                    dataSource = _originalLanguages;
                    break;
                case FilterItem.FilterType.Tags:
                    tagOrTraitSearchBox.Visible = true;
                    tagOrTraitSearchButton.Visible = true;
                    break;
                case FilterItem.FilterType.Traits:
                    traitRootsDropdown.Visible = true;
                    tagOrTraitSearchBox.Visible = true;
                    tagOrTraitSearchButton.Visible = true;
                    break;
            }
            filterValueCombobox.DataSource = dataSource;
        }

        private void AddPermanentFilterClick(object sender, EventArgs e)
        {
            var typeItem = (filterTypeCombobox.SelectedItem as ComboBoxItem)?.Key;
            var value = (filterValueCombobox.SelectedItem as ComboBoxItem)?.Key;
            if (typeItem == null || value == null) return;
            var type = (FilterItem.FilterType)typeItem;
            var filter = new FilterItem(type, value, excludeFilterCheckbox.Checked);
            if (orGroupCheckbox.Checked) _permanentFilter.OrFilters.Add(filter);
            else _permanentFilter.AndFilters.Add(filter);
            _refreshKind = RefreshType.UserChanged;
        }
        private void AddFilterClick(object sender, EventArgs e)
        {
            var typeItem = (filterTypeCombobox.SelectedItem as ComboBoxItem)?.Key;
            var value = (filterValueCombobox.SelectedItem as ComboBoxItem)?.Key;
            if (typeItem == null || value == null) return;
            var type = (FilterItem.FilterType)typeItem;
            var filter = new FilterItem(type, value, excludeFilterCheckbox.Checked);
            if (orGroupCheckbox.Checked) _customFilter.OrFilters.Add(filter);
            else _customFilter.AndFilters.Add(filter);
            _refreshKind = RefreshType.UserChanged;
        }

        /// <summary>
        /// Add trait from search results.
        /// </summary>
        private void AddTagOrTraitFromList<T>() where T : DumpFiles.ItemWithParents
        {
            tagOrTraitSearchBox.Text = "";
            tagOrTraitSearchResultBox.Visible = false;
            var item = tagOrTraitSearchResultBox.SelectedItem as T;
            Debug.Assert(item != null, nameof(item) + " != null");
            filterValueCombobox.DataSource = new[] { new ComboBoxItem(item.ID, item.ToString()) };
        }

        private void DeleteFilterClick(object sender, EventArgs e)
        {
            if (!(filterDropdown.SelectedItem is CustomFilter filter)) return;
            var result = MessageBox.Show($"Are you sure you wish to delete the filter {filter.Name}?",
                "Delete Custom Filter", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;
            ((IList)filterDropdown.DataSource).Remove(filter);
            SaveFilters();
        }
    }

    /// <summary>
    /// Type of refresh determines how to reset VNList in GUI.
    /// </summary>
#pragma warning disable 1591
    public enum RefreshType { None, UserChanged, NamedFilter }
#pragma warning restore 1591
}
