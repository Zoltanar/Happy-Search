using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Happy_Search.Properties;
using static Happy_Search.StaticHelpers;

namespace Happy_Search.Other_Forms
{
    /// <summary>
    /// Tab containing controls to alter filter of VN list.
    /// </summary>
    public partial class FiltersTab : UserControl
    {
        private readonly FormMain _mainForm;
        private Filters _filters;
        internal readonly BindingList<CustomFilter> FiltersList = new BindingList<CustomFilter>();

        /// <summary>
        /// Tab containing controls to alter filter of VN list.
        /// </summary>
        public FiltersTab(FormMain parentForm)
        {
            InitializeComponent();
            _mainForm = parentForm;
            var tagSource = new AutoCompleteStringCollection();
            tagSource.AddRange(FormMain.PlainTags.Select(v => v.Name).ToArray());
            tagSearchBox.AutoCompleteCustomSource = tagSource;
            string[] traitRootNames = FormMain.PlainTraits.Where(x => x.TopmostParentName == null).Select(x => x.Name).ToArray();
            traitRootsDropdown.Items.Clear();
            foreach (var rootName in traitRootNames)
            {
                if (rootName == null) continue;
                traitRootsDropdown.Items.Add(rootName);
            }
            customFilterReply.Text = "";
            traitReply.Text = "";
            tagReply.Text = "";
            tagsLB.DataSource = TagsPre;
            traitsLB.DataSource = TraitsPre;
            PopulateLanguages(true);
            originalLanguageLB.DataSource = _originalLanguagesPre;
            languageLB.DataSource = _languagesPre;
            releaseDateResponse.Visible = false;
            _doubleClickTimer.Tick += delegate { _doubleClickTimer.Stop(); };
            _doubleClickTimer.Interval = 250;
            SetFilterTags();
            LoadFromFile();

            void SetFilterTags()
            {
                lengthNA.Tag = (int)LengthFilter.NA;
                lengthUnderTwo.Tag = (int)LengthFilter.UnderTwoHours;
                lengthTwoToTen.Tag = (int)LengthFilter.TwoToTenHours;
                lengthTenToThirty.Tag = (int)LengthFilter.TenToThirtyHours;
                lengthThirtyToFifty.Tag = (int)LengthFilter.ThirtyToFiftyHours;
                lengthOverFifty.Tag = (int)LengthFilter.OverFiftyHours;
                unreleasedWithRD.Tag = (int)UnreleasedFilter.WithReleaseDate;
                unreleasedWithoutRD.Tag = (int)UnreleasedFilter.WithoutReleaseDate;
                unreleasedReleased.Tag = (int)UnreleasedFilter.Released;

                blacklistedYes.Tag = (int)YesNoFilter.Yes;
                blacklistedNo.Tag = (int)YesNoFilter.No;

                votedYes.Tag = (int)YesNoFilter.Yes;
                votedNo.Tag = (int)YesNoFilter.No;

                favoriteProducerYes.Tag = (int)YesNoFilter.Yes;
                favoriteProducerNo.Tag = (int)YesNoFilter.No;

                wishlistNA.Tag = (int)WishlistFilter.NA;
                wishlistHigh.Tag = (int)WishlistFilter.High;
                wishlistMedium.Tag = (int)WishlistFilter.Medium;
                wishlistLow.Tag = (int)WishlistFilter.Low;

                userlistNA.Tag = (int)UserlistFilter.NA;
                userlistUnknown.Tag = (int)UserlistFilter.Unknown;
                userlistPlaying.Tag = (int)UserlistFilter.Playing;
                userlistFinished.Tag = (int)UserlistFilter.Finished;
                userlistStalled.Tag = (int)UserlistFilter.Stalled;
                userlistDropped.Tag = (int)UserlistFilter.Dropped;
            }
            void LoadFromFile()
            {
                _customTagFilters.Clear();
                var loadedTagFilters = LoadObjectFromJsonFile<List<CustomTagFilter>>(CustomTagFiltersJson);
                if (loadedTagFilters != null) _customTagFilters.AddRange(loadedTagFilters);
                _customTraitFilters.Clear();
                var loadedTraitFilters = LoadObjectFromJsonFile<List<CustomTraitFilter>>(CustomTraitFiltersJson);
                if (loadedTraitFilters != null) _customTraitFilters.AddRange(loadedTraitFilters);
                LoadSaveFilters();
                _filters = Filters.LoadFilters(this);
                DontTriggerEvent = true;
                traitRootsDropdown.SelectedIndex = 0;
                filterDropdown.DataSource = FiltersList;
                _mainForm.filterDropdown.DataSource = FiltersList;
                tagFiltersCB.DataSource = _customTagFilters;
                traitFiltersCB.DataSource = _customTraitFilters;
                if (_customTagFilters.Count > 0) tagFiltersCB.SelectedIndex = 0;
                if (_customTraitFilters.Count > 0) traitFiltersCB.SelectedIndex = 0;
                if (FiltersList.Count > 0) filterDropdown.SelectedIndex = 0;
                if (FiltersList.Count > 0) _mainForm.filterDropdown.SelectedIndex = 0;
                DontTriggerEvent = false;
                SetFiltersToGui();

                void LoadSaveFilters()
                {
                    FiltersList.Clear();
                    List<CustomFilter> loadedCustomFilters = LoadObjectFromJsonFile<List<CustomFilter>>(CustomFiltersJson) ??
                                                        LoadObjectFromJsonFile<List<CustomFilter>>(DefaultFiltersJson);
                    if (loadedCustomFilters != null) FiltersList.AddRange(loadedCustomFilters);
                }
            }
        }

        /// <summary>
        /// Populates Languages in comboboxes.
        /// </summary>
        internal void PopulateLanguages(bool firstTime)
        {
            var autoCompleteLanguages = new AutoCompleteStringCollection { "(Language)" };
            var autoCompleteOriginalLanguages = new AutoCompleteStringCollection { "(Language)" };
            var langList = new HashSet<string>();
            var origLangList = new HashSet<string>();
            foreach (var vn in _mainForm.VNList)
            {
                if (vn.Languages == null) continue;
                foreach (var lang in vn.Languages.All)
                {
                    langList.Add(new CultureInfo(lang).DisplayName);
                }
                foreach (var lang in vn.Languages.Originals)
                {
                    origLangList.Add(new CultureInfo(lang).DisplayName);
                }
            }
            autoCompleteLanguages.AddRange(langList.ToArray());
            autoCompleteOriginalLanguages.AddRange(origLangList.ToArray());
            if (firstTime)
            {
                languageCB.AutoCompleteCustomSource = autoCompleteLanguages;
                languageCB.DataSource = autoCompleteLanguages;
                originalLanguageCB.AutoCompleteCustomSource = autoCompleteOriginalLanguages;
                originalLanguageCB.DataSource = autoCompleteOriginalLanguages;
            }
            else
            {
                Invoke(new MethodInvoker(() =>
                {
                    languageCB.AutoCompleteCustomSource = autoCompleteLanguages;
                    languageCB.DataSource = autoCompleteLanguages;
                    originalLanguageCB.AutoCompleteCustomSource = autoCompleteOriginalLanguages;
                    originalLanguageCB.DataSource = autoCompleteOriginalLanguages;
                }));
            }
        }

        private void ToggleThisFilter(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null) return;
            checkBox.Text = checkBox.Checked ? "On" : "Off";
            var panel = checkBox.Parent;
            //12 panels
            if (panel == lengthPanel) //1
            {
                _filters.Length = (LengthFilter)GetIntFromCheckboxes(lengthPanel);
            }
            else if (panel == releaseDatePanel) //2
            {
                _filters.ReleaseDate = GetReleaseDateFromGui();
            }
            else if (panel == unreleasedPanel) //3
            {
                _filters.Unreleased = (UnreleasedFilter)GetIntFromCheckboxes(unreleasedPanel);
            }
            else if (panel == blacklistedPanel) //4
            {
                _filters.Blacklisted = blacklistedPanel.GetRadioOption<YesNoFilter>();
            }
            else if (panel == votedPanel) //5
            {
                _filters.Blacklisted = votedPanel.GetRadioOption<YesNoFilter>();
            }
            else if (panel == favoriteProducerPanel) //6
            {
                _filters.Blacklisted = favoriteProducerPanel.GetRadioOption<YesNoFilter>();
            }
            else if (panel == wishlistPanel) //7
            {
                _filters.Wishlist = (WishlistFilter)GetIntFromCheckboxes(wishlistPanel);
            }
            else if (panel == userlistPanel) //8
            {
                _filters.Userlist = (UserlistFilter)GetIntFromCheckboxes(userlistPanel);
            }
            else if (panel == languagePanel) //9
            {
                _filters.Language = languageTF.Checked ? _languagesPre.ToArray() : null;
            }
            else if (panel == originalLanguagePanel) //10
            {
                _filters.OriginalLanguage = originalLanguageTF.Checked ? _originalLanguagesPre.ToArray() : null;
            }
            else if (panel == tagsPanel) //11
            {
                _filters.Tags = tagsTF.Checked ? TagsPre.ToArray() : null;
            }
            else if (panel == traitsPanel) //12
            {
                _filters.Traits = traitsTF.Checked ? TraitsPre.ToArray() : null;
            }
        }

        private DateRange GetReleaseDateFromGui()
        {
            releaseDateResponse.Visible = false;
            if (!releaseDateTF.Checked) return null;
            try
            {
                var fromDate = new DateTime((int)releaseDateFromYear.Value,
                    releaseDateFromMonth.Items.IndexOf(releaseDateFromMonth.Text) + 1,
                    (int)releaseDateFromDay.Value);
                var toDate = new DateTime((int)releaseDateToYear.Value,
                    releaseDateToMonth.Items.IndexOf(releaseDateToMonth.Text) + 1,
                    (int)releaseDateToDay.Value);
                return new DateRange(fromDate, toDate);
            }
            catch (ArgumentOutOfRangeException)
            {
                releaseDateResponse.Visible = true;
                return null;
            }
        }

        /// <summary>
        /// Show Filters from object on GUI
        /// </summary>
        private void SetFiltersToGui()
        {
            SetLengthToggleFilter();
            SetReleaseDateFilter();
            SetUnreleasedToggleFilter();
            SetBlacklistToggleFilter();
            SetFavoriteProducerToggleFilter();
            SetWishlistToggleFilter();
            SetUserlistToggleFilter();
            SetLanguageToggleFilter();
            SetOriginalLanguageToggleFilter();
            SetTagsToggleFilter();
            SetTraitsToggleFilter();

            void SetLengthToggleFilter()
            {
                if (_filters.Length == LengthFilter.Off)
                {
                    lengthTF.Checked = false;
                    return;
                }
                lengthTF.Checked = true;
                if (_filters.Length.HasFlag(LengthFilter.NA)) lengthNA.Checked = true;
                if (_filters.Length.HasFlag(LengthFilter.UnderTwoHours)) lengthUnderTwo.Checked = true;
                if (_filters.Length.HasFlag(LengthFilter.TwoToTenHours)) lengthTwoToTen.Checked = true;
                if (_filters.Length.HasFlag(LengthFilter.TenToThirtyHours)) lengthTenToThirty.Checked = true;
                if (_filters.Length.HasFlag(LengthFilter.ThirtyToFiftyHours)) lengthThirtyToFifty.Checked = true;
                if (_filters.Length.HasFlag(LengthFilter.OverFiftyHours)) lengthOverFifty.Checked = true;
            }
            void SetReleaseDateFilter()
            {
                if (_filters.ReleaseDate == null)
                {
                    releaseDateTF.Checked = false;
                    return;
                }
                releaseDateTF.Checked = true;
                releaseDateFromDay.Value = _filters.ReleaseDate.From.Day;
                releaseDateFromMonth.SelectedIndex = _filters.ReleaseDate.From.Month;
                releaseDateFromYear.Value = _filters.ReleaseDate.From.Year;
                releaseDateToDay.Value = _filters.ReleaseDate.To.Day;
                releaseDateToMonth.SelectedIndex = _filters.ReleaseDate.To.Month;
                releaseDateToYear.Value = _filters.ReleaseDate.To.Year;
            }
            void SetUnreleasedToggleFilter()
            {
                if (_filters.Unreleased == UnreleasedFilter.Off)
                {
                    unreleasedTF.Checked = false;
                    return;
                }
                unreleasedTF.Checked = true;
                if (_filters.Unreleased.HasFlag(UnreleasedFilter.WithReleaseDate)) unreleasedWithRD.Checked = true;
                if (_filters.Unreleased.HasFlag(UnreleasedFilter.WithoutReleaseDate)) unreleasedWithoutRD.Checked = true;
                if (_filters.Unreleased.HasFlag(UnreleasedFilter.Released)) unreleasedReleased.Checked = true;
            }
            void SetBlacklistToggleFilter()
            {
                switch (_filters.Blacklisted)
                {
                    case YesNoFilter.No:
                        blacklistedTF.Checked = true;
                        blacklistedNo.Checked = true;
                        break;
                    case YesNoFilter.Yes:
                        blacklistedTF.Checked = true;
                        blacklistedYes.Checked = true;
                        break;
                    case YesNoFilter.Off:
                        blacklistedTF.Checked = false;
                        break;
                }
            }
            void SetWishlistToggleFilter()
            {
                if (_filters.Wishlist == WishlistFilter.Off)
                {
                    wishlistTF.Checked = false;
                    return;
                }
                wishlistTF.Checked = true;
                if (_filters.Wishlist.HasFlag(WishlistFilter.NA)) wishlistNA.Checked = true;
                if (_filters.Wishlist.HasFlag(WishlistFilter.High)) wishlistHigh.Checked = true;
                if (_filters.Wishlist.HasFlag(WishlistFilter.Medium)) wishlistMedium.Checked = true;
                if (_filters.Wishlist.HasFlag(WishlistFilter.Low)) wishlistLow.Checked = true;
            }
            void SetUserlistToggleFilter()
            {
                if (_filters.Userlist == UserlistFilter.Off)
                {
                    userlistTF.Checked = false;
                    return;
                }
                userlistTF.Checked = true;
                if (_filters.Userlist.HasFlag(UserlistFilter.NA)) userlistNA.Checked = true;
                if (_filters.Userlist.HasFlag(UserlistFilter.Unknown)) userlistUnknown.Checked = true;
                if (_filters.Userlist.HasFlag(UserlistFilter.Playing)) userlistPlaying.Checked = true;
                if (_filters.Userlist.HasFlag(UserlistFilter.Finished)) userlistFinished.Checked = true;
                if (_filters.Userlist.HasFlag(UserlistFilter.Stalled)) userlistStalled.Checked = true;
                if (_filters.Userlist.HasFlag(UserlistFilter.Dropped)) userlistDropped.Checked = true;
            }
            void SetFavoriteProducerToggleFilter()
            {
                switch (_filters.FavoriteProducers)
                {
                    case YesNoFilter.Off:
                        favoriteProducerTF.Checked = false;
                        break;
                    case YesNoFilter.Yes:
                        favoriteProducerTF.Checked = true;
                        favoriteProducerYes.Checked = true;
                        break;
                    case YesNoFilter.No:
                        favoriteProducerTF.Checked = true;
                        favoriteProducerNo.Checked = true;
                        break;
                }
            }
            void SetLanguageToggleFilter()
            {
                _languagesPre.Clear();
                if (_filters.Language != null)
                {
                    _languagesPre.AddRange(_filters.Language.ToArray());
                }
                languageTF.Checked = _filters.Language != null;
            }
            void SetOriginalLanguageToggleFilter()
            {
                _originalLanguagesPre.Clear();
                if (_filters.OriginalLanguage != null)
                {
                    _originalLanguagesPre.AddRange(_filters.OriginalLanguage.ToArray());
                }
                originalLanguageTF.Checked = _filters.OriginalLanguage != null;
            }
            void SetTagsToggleFilter()
            {
                TagsPre.Clear();
                if (_filters.Tags != null)
                {
                    TagsPre.AddRange(_filters.Tags.ToArray());
                }
                tagsTF.Checked = _filters.Tags != null;
            }
            void SetTraitsToggleFilter()
            {
                TraitsPre.Clear();
                if (_filters.Traits != null) TraitsPre.AddRange(_filters.Traits);
            }
            traitsTF.Checked = _filters.Traits != null;
        }

        private readonly Timer _doubleClickTimer = new Timer();
        private int _doubleClickCount;

        private void FilterCheckboxClick(object sender, MouseEventArgs e)
        {
            if (!_doubleClickTimer.Enabled)
            {
                _doubleClickTimer.Start();
                _doubleClickCount = 0;
            }
            _doubleClickCount++;
            if (_doubleClickCount < 2) return;
            _doubleClickTimer.Stop();
            var filter = (CheckBox)sender;
            foreach (Control control in filter.Parent.Controls)
            {
                var box = control as CheckBox;
                if (box == null || box.Appearance == Appearance.Button) continue;
                box.Checked = box == sender;
            }
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
            _filters.SetCustomFilter(selectedItem);
            SetFiltersToGui();
            if (sender == _mainForm)
            {
                _mainForm.SetVNList(_filters.GetFunction(_mainForm,this),_filters.Name);
                _mainForm.LoadVNListToGui();
                return;
            }
            _filters.RefreshKind = RefreshType.NamedFilter;
        }

        private void FilterFixedChanged(object sender, EventArgs e)
        {
            var checkBox = (CheckBox)sender;
            checkBox.Text = checkBox.Checked ? "Fixed" : "Not Fixed";
        }

        private void ChangeTagsOrTraits(object sender, EventArgs e)
        {
            tagsOrTraits.Text = tagsOrTraits.Checked ? "Tags OR Traits" : "Tags AND Traits";
            _filters.TagsTraitsMode = tagsOrTraits.Checked;
        }

        private readonly BindingList<string> _originalLanguagesPre = new BindingList<string>();
        private readonly BindingList<string> _languagesPre = new BindingList<string>();

        private void AddOriginalLanguage(object sender, EventArgs e)
        {
            var language = originalLanguageCB.Text;
            if (language == "(Language)") return;
            if (_originalLanguagesPre.Contains(language)) return;
            if (!originalLanguageCB.Items.Contains(language)) return;
            if (!_originalLanguagesPre.Contains(language)) _originalLanguagesPre.Add(language);
        }

        private void AddOriginalLanguageEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) AddOriginalLanguage(sender, null);
        }

        private void AddLanguage(object sender, EventArgs e)
        {
            var language = languageCB.Text;
            if (language == "(Language)") return;
            if (_languagesPre.Contains(language)) return;
            if (!languageCB.Items.Contains(language)) return;
            if (!_languagesPre.Contains(language)) _languagesPre.Add(language);
        }

        private void AddLanguageEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) AddLanguage(sender, null);
        }

        private void RemoveFromListBox(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete) return;
            var listbox = sender as ListBox;
            if (listbox == null) return;
            var selectedItem = listbox.SelectedItem;
            (listbox.DataSource as System.Collections.IList)?.Remove(selectedItem);
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
                FiltersList[itemIndex] = _filters.GetCustomFilter();
                filterDropdown.SelectedIndex = itemIndex;
                _filters.RefreshKind = RefreshType.NamedFilter;
                DontTriggerEvent = false;
            }
            else
            {
                _filters.Name = filterName;
                DontTriggerEvent = true;
                FiltersList.Add(_filters.GetCustomFilter());
                filterDropdown.SelectedIndex = FiltersList.Count - 1;
                _filters.RefreshKind = RefreshType.NamedFilter;
                DontTriggerEvent = false;
            }
            SaveObjectToJsonFile(FiltersList, CustomFiltersJson);
            WriteText(customFilterReply, Resources.filter_saved);
        }

        private int GetIndexOfCustomFilter(string filterName)
        {
            int index = 0;
            foreach (var customFilter in FiltersList)
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
            _mainForm.SetVNList(_filters.GetFunction(_mainForm,this),_filters.Name);
            SaveFilters();
            _mainForm.LoadVNListToGui();
            _filters.RefreshKind = RefreshType.None;
        }

        private void Help_Filters(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            Debug.Assert(path != null, "path != null");
            var helpFile = $"{Path.Combine(path, "Program Data\\Help\\filtering.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

        private void ChangeMultiFilter(object sender, EventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var panel = checkBox.Parent;
            var toggle = panel.Controls.Find(panel.Name.Replace("Panel", "TF"), false).FirstOrDefault();
            if (toggle == null || !((CheckBox)toggle).Checked) return;
            if (panel == lengthPanel)
                _filters.Length = _filters.Length.SetFlag((LengthFilter)checkBox.Tag, checkBox.Checked);
            else if (panel == unreleasedPanel)
                _filters.Unreleased = _filters.Unreleased.SetFlag((UnreleasedFilter)checkBox.Tag, checkBox.Checked);
            else if (panel == wishlistPanel)
                _filters.Wishlist = _filters.Wishlist.SetFlag((WishlistFilter)checkBox.Tag, checkBox.Checked);
            else if (panel == userlistPanel)
                _filters.Userlist = _filters.Userlist.SetFlag((UserlistFilter)checkBox.Tag, checkBox.Checked);
        }
    }
}
