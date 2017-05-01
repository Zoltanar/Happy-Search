using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Happy_Search.Properties;
using Newtonsoft.Json;
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
                filterDropdown.DataSource = _mainForm.FiltersList;
                _mainForm.filterDropdown.DataSource = _mainForm.FiltersList;
                tagFiltersCB.DataSource = _customTagFilters;
                traitFiltersCB.DataSource = _customTraitFilters;
                if(_customTagFilters.Count > 0) tagFiltersCB.SelectedIndex = 0;
                if (_customTraitFilters.Count > 0) traitFiltersCB.SelectedIndex = 0;
                if (_mainForm.FiltersList.Count > 0) filterDropdown.SelectedIndex = 0;
                if (_mainForm.FiltersList.Count > 0) _mainForm.filterDropdown.SelectedIndex = 0;
                DontTriggerEvent = false;
                DisplayFilters();
                ApplyFilters(_filters);

                void LoadSaveFilters()
                {
                    _mainForm.FiltersList.Clear();
                    List<Filters> loadedCustomFilters = LoadObjectFromJsonFile<List<Filters>>(CustomFiltersJson) ??
                                                        LoadObjectFromJsonFile<List<Filters>>(DefaultFiltersJson);
                    if (loadedCustomFilters != null) _mainForm.FiltersList.AddRange(loadedCustomFilters);
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
            if (sender is CheckBox filter) filter.Text = filter.Checked ? "On" : "Off";
        }

        private void DisplayFilters()
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
                languageTF.Checked = _filters.Language != null;
                _languagesPre.Clear();
                if (_filters.Language != null)
                {
                    _languagesPre.AddRange(_filters.Language.ToArray());
                }
            }
            void SetOriginalLanguageToggleFilter()
            {
                originalLanguageTF.Checked = _filters.OriginalLanguage != null;
                _originalLanguagesPre.Clear();
                if (_filters.OriginalLanguage != null)
                {
                    _originalLanguagesPre.AddRange(_filters.OriginalLanguage.ToArray());
                }
            }
            void SetTagsToggleFilter()
            {
                tagsTF.Checked = _filters.Tags != null;
                TagsPre.Clear();
                if (_filters.Tags != null)
                {
                    TagsPre.AddRange(_filters.Tags.ToArray());
                }
            }
            void SetTraitsToggleFilter()
            {
                traitsTF.Checked = _filters.Traits != null;
                TraitsPre.Clear();
                if (_filters.Traits != null) TraitsPre.AddRange(_filters.Traits);
            }
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
            Filters selectedItem = (Filters)filterDropdown.SelectedItem;
            ChangeCustomFilter(sender, selectedItem);
        }

        private bool _customFilterBlock;

        /// <summary>
        /// Change to selected filter.
        /// </summary>
        public void ChangeCustomFilter(object sender, Filters selectedItem)
        {
            if (selectedItem == null) return;
            if (_customFilterBlock) return;
            _customFilterBlock = true;
            filterDropdown.SelectedItem = selectedItem;
            _mainForm.filterDropdown.SelectedItem = selectedItem;
            _customFilterBlock = false;
            if (DontTriggerEvent) return;
            _filters.SetFromSavedFilter(this, selectedItem);
            DisplayFilters();
            if (sender == _mainForm)
            {
                ApplyFilters(_filters);
                _mainForm.LoadVNListToGui();
            }
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

        /// <summary>
        /// Get whether vn list should be refreshed
        /// </summary>
        private bool RefreshFilters => _filters.Refresh;

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

        /// <summary>
        /// Apply filters.
        /// </summary>
        private void ApplyFilters(Filters filters)
        {
            if (DontTriggerEvent) return;
            filters.SetFixedStatusesFromForm(this);
            filters.Length = (LengthFilter)GetIntFromCheckboxes(lengthPanel);
            releaseDateResponse.Visible = false;
            if (releaseDateTF.Checked)
            {
                try
                {
                    var fromDate = new DateTime((int)releaseDateFromYear.Value,
                        releaseDateFromMonth.Items.IndexOf(releaseDateFromMonth.Text) + 1,
                        (int)releaseDateFromDay.Value);
                    var toDate = new DateTime((int)releaseDateToYear.Value,
                        releaseDateToMonth.Items.IndexOf(releaseDateToMonth.Text) + 1,
                        (int)releaseDateToDay.Value);
                    filters.ReleaseDate = new DateRange(fromDate, toDate);
                }
                catch (ArgumentOutOfRangeException)
                {
                    releaseDateResponse.Visible = true;
                    filters.ReleaseDate = null;
                }

            }
            else filters.ReleaseDate = null;
            filters.Unreleased = (UnreleasedFilter)GetIntFromCheckboxes(unreleasedPanel);
            filters.Blacklisted = blacklistedPanel.GetRadioOption<YesNoFilter>();
            filters.Voted = votedPanel.GetRadioOption<YesNoFilter>();
            filters.FavoriteProducers = favoriteProducerPanel.GetRadioOption<YesNoFilter>();
            filters.Wishlist = (WishlistFilter)GetIntFromCheckboxes(wishlistPanel);
            filters.Userlist = (UserlistFilter)GetIntFromCheckboxes(userlistPanel);
            filters.Language = languageTF.Checked ? _languagesPre.ToArray() : null;
            filters.OriginalLanguage = originalLanguageTF.Checked ? _originalLanguagesPre.ToArray() : null;
            filters.Tags = tagsTF.Checked ? TagsPre.ToArray() : null;
            filters.Traits = traitsTF.Checked ? TraitsPre.ToArray() : null;
            _mainForm.SetVNList(filters.GetFunction(_mainForm, this), filters.Name);
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
            var customFilter = _mainForm.FiltersList.FirstOrDefault(x => x.Name.Equals(filterName));
            if (customFilter != null)
            {
                var askBox = MessageBox.Show(@"Do you wish to overwrite present custom filter?",
                    Resources.ask_overwrite, MessageBoxButtons.YesNo);
                if (askBox != DialogResult.Yes) return;
                ApplyFilters(customFilter);
                DontTriggerEvent = true;
                filterDropdown.SelectedIndex = filterDropdown.Items.IndexOf(customFilter);
                DontTriggerEvent = false;
            }
            else
            {
                DontTriggerEvent = true;
                _filters.Name = filterName;
                var filterString = JsonConvert.SerializeObject(_filters);
                _mainForm.FiltersList.Add(JsonConvert.DeserializeObject<Filters>(filterString));
                filterDropdown.SelectedIndex = _mainForm.FiltersList.Count - 1;
                DontTriggerEvent = false;
            }
            SaveObjectToJsonFile(_mainForm.FiltersList, CustomFiltersJson);
            WriteText(customFilterReply, Resources.filter_saved);
        }
        
        internal void EnteredMainTab()
        {
            if (RefreshFilters)
            {
                _mainForm.CurrentFilterLabel = "Custom Filter";
                _mainForm.LoadVNListToGui();
                _filters.SetRefreshFalse();
            }
        }

        internal void LeftFiltersTab()
        {
            ApplyFilters(_filters);
            SaveFilters();
        }
    }
}
