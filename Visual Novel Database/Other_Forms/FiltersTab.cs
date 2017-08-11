using System;
using System.Collections;
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
        private readonly BindingList<CustomFilter> _customFilters = new BindingList<CustomFilter>();

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
            traitRootsDropdown.SelectedIndex = 0;
            customFilterReply.Text = "";
            traitReply.Text = "";
            tagReply.Text = "";
            PopulateLanguages(true);
            releaseDateResponse.Visible = false;
            _doubleClickTimer.Tick += delegate { _doubleClickTimer.Stop(); };
            _doubleClickTimer.Interval = 250;
            SetFilterTags();
            InitialLoadFromFile();
            _mainForm.SetVNList(_filters.GetFunction(_mainForm, this), _filters.Name);
            _mainForm.LoadVNListToGui();
            _filters.RefreshKind = RefreshType.None;

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

                blacklistedYes.Tag = true;
                blacklistedNo.Tag = false;

                votedYes.Tag = true;
                votedNo.Tag = false;

                favoriteProducerYes.Tag = true;
                favoriteProducerNo.Tag = false;

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
            CheckBox checkBox = (CheckBox) sender;
            checkBox.Text = checkBox.Checked ? "On" : "Off";
            var panel = checkBox.Parent;
            //12 panels
            if (panel == lengthPanel) //1
            {
                _filters.LengthOn = checkBox.Checked;
            }
            else if (panel == releaseDatePanel) //2
            {
                _filters.ReleaseDateOn = checkBox.Checked;
            }
            else if (panel == unreleasedPanel) //3
            {
                _filters.UnreleasedOn = checkBox.Checked;
            }
            else if (panel == blacklistedPanel) //4
            {
                _filters.BlacklistedOn = checkBox.Checked;
            }
            else if (panel == votedPanel) //5
            {
                _filters.VotedOn = checkBox.Checked;
            }
            else if (panel == favoriteProducerPanel) //6
            {
                _filters.FavoriteProducersOn = checkBox.Checked;
            }
            else if (panel == wishlistPanel) //7
            {
                _filters.WishlistOn = checkBox.Checked;
            }
            else if (panel == userlistPanel) //8
            {
                _filters.UserlistOn = checkBox.Checked;
            }
            else if (panel == languagePanel) //9
            {
                _filters.LanguageOn = checkBox.Checked;
            }
            else if (panel == originalLanguagePanel) //10
            {
                _filters.OriginalLanguageOn = checkBox.Checked;
            }
            else if (panel == tagsPanel) //11
            {
                _filters.TagsOn = checkBox.Checked;
            }
            else if (panel == traitsPanel) //12
            {
                _filters.TraitsOn = checkBox.Checked;
            }
        }

        private DateRange GetReleaseDateFromGui()
        {
            releaseDateResponse.Visible = false;
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
                return new DateRange();
            }
        }

        /// <summary>
        /// Show Filters from object on GUI
        /// </summary>
        private void SetFiltersToGui()
        {
            lengthTF.Checked = _filters.LengthOn;
            releaseDateTF.Checked = _filters.ReleaseDateOn;
            unreleasedTF.Checked = _filters.UnreleasedOn;
            blacklistedTF.Checked = _filters.BlacklistedOn;
            votedTF.Checked = _filters.VotedOn;
            favoriteProducerTF.Checked = _filters.FavoriteProducersOn;
            wishlistTF.Checked = _filters.WishlistOn;
            userlistTF.Checked = _filters.UserlistOn;
            languageTF.Checked = _filters.LanguageOn;
            originalLanguageTF.Checked = _filters.OriginalLanguageOn;
            tagsTF.Checked = _filters.TagsOn;
            traitsTF.Checked = _filters.TraitsOn;

            SetLengthFilter();
            SetReleaseDateFilter();
            SetUnreleasedFilter();
            (_filters.Blacklisted ? blacklistedYes : blacklistedNo).Checked = true;
            (_filters.Voted ? votedYes : votedNo).Checked = true;
            (_filters.FavoriteProducers ? favoriteProducerYes : favoriteProducerNo).Checked = true;
            SetWishlistFilter();
            SetUserlistFilter();

            lengthFixed.Checked = _filters.LengthFixed;
            releaseDateFixed.Checked = _filters.ReleaseDateFixed;
            unreleasedFixed.Checked = _filters.UnreleasedFixed;
            blacklistedFixed.Checked = _filters.BlacklistedFixed;
            votedFixed.Checked = _filters.VotedFixed;
            favoriteProducerFixed.Checked = _filters.FavoriteProducersFixed;
            wishlistFixed.Checked = _filters.WishlistFixed;
            userlistFixed.Checked = _filters.UserlistFixed;
            languageFixed.Checked = _filters.LanguageFixed;
            originalLanguageFixed.Checked = _filters.OriginalLanguageFixed;
            tagsFixed.Checked = _filters.TagsFixed;
            traitsFixed.Checked = _filters.TraitsFixed;

            void SetLengthFilter()
            {
                lengthNA.Checked = _filters.Length.HasFlag(LengthFilter.NA);
                lengthUnderTwo.Checked = _filters.Length.HasFlag(LengthFilter.UnderTwoHours);
                lengthTwoToTen.Checked = _filters.Length.HasFlag(LengthFilter.TwoToTenHours);
                lengthTenToThirty.Checked = _filters.Length.HasFlag(LengthFilter.TenToThirtyHours);
                lengthThirtyToFifty.Checked = _filters.Length.HasFlag(LengthFilter.ThirtyToFiftyHours);
                lengthOverFifty.Checked = _filters.Length.HasFlag(LengthFilter.OverFiftyHours);
            }
            void SetReleaseDateFilter()
            {
                releaseDateFromDay.Value = _filters.ReleaseDate.From.Day;
                releaseDateFromMonth.SelectedIndex = _filters.ReleaseDate.From.Month-1;
                releaseDateFromYear.Value = _filters.ReleaseDate.From.Year;
                releaseDateToDay.Value = _filters.ReleaseDate.To.Day;
                releaseDateToMonth.SelectedIndex = _filters.ReleaseDate.To.Month-1;
                releaseDateToYear.Value = _filters.ReleaseDate.To.Year;
            }
            void SetUnreleasedFilter()
            {
                unreleasedWithoutRD.Checked = _filters.Unreleased.HasFlag(UnreleasedFilter.WithoutReleaseDate);
                unreleasedWithRD.Checked = _filters.Unreleased.HasFlag(UnreleasedFilter.WithReleaseDate);
                unreleasedReleased.Checked = _filters.Unreleased.HasFlag(UnreleasedFilter.Released);
            }
            void SetWishlistFilter()
            {
                wishlistNA.Checked = _filters.Wishlist.HasFlag(WishlistFilter.NA);
                wishlistHigh.Checked = _filters.Wishlist.HasFlag(WishlistFilter.High);
                wishlistMedium.Checked = _filters.Wishlist.HasFlag(WishlistFilter.Medium);
                wishlistLow.Checked = _filters.Wishlist.HasFlag(WishlistFilter.Low);
            }
            void SetUserlistFilter()
            {
                userlistNA.Checked = _filters.Userlist.HasFlag(UserlistFilter.NA);
                userlistUnknown.Checked = _filters.Userlist.HasFlag(UserlistFilter.Unknown);
                userlistPlaying.Checked = _filters.Userlist.HasFlag(UserlistFilter.Playing);
                userlistFinished.Checked = _filters.Userlist.HasFlag(UserlistFilter.Finished);
                userlistStalled.Checked = _filters.Userlist.HasFlag(UserlistFilter.Stalled);
                userlistDropped.Checked = _filters.Userlist.HasFlag(UserlistFilter.Dropped);
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
                _mainForm.SetVNList(_filters.GetFunction(_mainForm, this), _filters.Name);
                _mainForm.LoadVNListToGui();
            }
        }

        private void FilterFixedChanged(object sender, EventArgs e)
        {
            var checkBox = (CheckBox)sender;
            checkBox.Text = checkBox.Checked ? "Fixed" : "Not Fixed";
            //
            var panel = checkBox.Parent;
            //12 panels
            if (panel == lengthPanel) //1
            {
                _filters.LengthFixed = checkBox.Checked;
            }
            else if (panel == releaseDatePanel) //2
            {
                _filters.ReleaseDateFixed = checkBox.Checked;
            }
            else if (panel == unreleasedPanel) //3
            {
                _filters.UnreleasedFixed = checkBox.Checked;
            }
            else if (panel == blacklistedPanel) //4
            {
                _filters.BlacklistedFixed = checkBox.Checked;
            }
            else if (panel == votedPanel) //5
            {
                _filters.VotedFixed = checkBox.Checked;
            }
            else if (panel == favoriteProducerPanel) //6
            {
                _filters.FavoriteProducersFixed = checkBox.Checked;
            }
            else if (panel == wishlistPanel) //7
            {
                _filters.WishlistFixed = checkBox.Checked;
            }
            else if (panel == userlistPanel) //8
            {
                _filters.UserlistFixed = checkBox.Checked;
            }
            else if (panel == languagePanel) //9
            {
                _filters.LanguageFixed = checkBox.Checked;
            }
            else if (panel == originalLanguagePanel) //10
            {
                _filters.OriginalLanguageFixed = checkBox.Checked;
            }
            else if (panel == tagsPanel) //11
            {
                _filters.TagsFixed = checkBox.Checked;
            }
            else if (panel == traitsPanel) //12
            {
                _filters.TraitsFixed = checkBox.Checked;
            }
        }

        private void ChangeTagsOrTraits(object sender, EventArgs e)
        {
            tagsOrTraits.Text = tagsOrTraits.Checked ? "Tags OR Traits" : "Tags AND Traits";
            _filters.TagsTraitsMode = tagsOrTraits.Checked;
        }

        private void AddOriginalLanguage(object sender, EventArgs e)
        {
            var language = originalLanguageCB.Text;
            if (language == "(Language)") return;
            if (_filters.OriginalLanguage.Contains(language)) return;
            if (!_filters.OriginalLanguage.Contains(language)) _filters.OriginalLanguage.Add(language);
        }

        private void AddOriginalLanguageEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) AddOriginalLanguage(sender, null);
        }

        private void AddLanguage(object sender, EventArgs e)
        {
            var language = languageCB.Text;
            if (language == "(Language)") return;
            if (_filters.Language.Contains(language)) return;
            if (!_filters.Language.Contains(language)) _filters.Language.Add(language);
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
            ((IList) listbox.DataSource).Remove(selectedItem);
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
            _mainForm.SetVNList(_filters.GetFunction(_mainForm, this), _filters.Name);
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

        private void InitialLoadFromFile()
        {
            _customTagFilters.Clear();
            var loadedTagFilters = LoadObjectFromJsonFile<List<CustomTagFilter>>(CustomTagFiltersJson);
            if (loadedTagFilters != null) _customTagFilters.AddRange(loadedTagFilters);
            _customTraitFilters.Clear();
            var loadedTraitFilters = LoadObjectFromJsonFile<List<CustomTraitFilter>>(CustomTraitFiltersJson);
            if (loadedTraitFilters != null) _customTraitFilters.AddRange(loadedTraitFilters);
            _customFilters.Clear();
            var loadedCustomFilters = LoadObjectFromJsonFile<List<CustomFilter>>(CustomFiltersJson) ?? LoadObjectFromJsonFile<List<CustomFilter>>(DefaultFiltersJson);
            if (loadedCustomFilters != null) _customFilters.AddRange(loadedCustomFilters);
            _filters = Filters.LoadFixedFilter();
            DontTriggerEvent = true;
            filterDropdown.DataSource = _customFilters;
            _mainForm.filterDropdown.DataSource = _customFilters;
            tagFiltersCB.DataSource = _customTagFilters;
            traitFiltersCB.DataSource = _customTraitFilters;
            originalLanguageLB.DataSource = _customTraitFilters;
            if (_customTagFilters.Count > 0) tagFiltersCB.SelectedIndex = 0;
            if (_customTraitFilters.Count > 0) traitFiltersCB.SelectedIndex = 0;
            if (_customFilters.Count > 0) filterDropdown.SelectedIndex = 0;
            if (_customFilters.Count > 0) _mainForm.filterDropdown.SelectedIndex = 0;
            languageLB.DataSource = _filters.Language;
            originalLanguageLB.DataSource = _filters.OriginalLanguage;
            tagsLB.DataSource = _filters.Tags;
            traitsLB.DataSource = _filters.Traits;
            DontTriggerEvent = false;
            SetFiltersToGui();
        }

        private void FilterStateChanged(object sender, EventArgs e)
        {
            var control = (Control) sender;
            if (!(control as RadioButton)?.Checked ?? false) return;
            var panel = control.Parent;
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
                _filters.Blacklisted = blacklistedPanel.GetRadioOption();
            }
            else if (panel == votedPanel) //5
            {
                _filters.Voted = votedPanel.GetRadioOption();
            }
            else if (panel == favoriteProducerPanel) //6
            {
                _filters.FavoriteProducers = favoriteProducerPanel.GetRadioOption();
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
                _filters.LanguageOn = languageTF.Checked;
            }
            else if (panel == originalLanguagePanel) //10
            {
                _filters.OriginalLanguageOn = originalLanguageTF.Checked;
            }
            else if (panel == tagsPanel) //11
            {
                _filters.TagsOn = tagsTF.Checked;
            }
            else if (panel == traitsPanel) //12
            {
                _filters.TraitsOn = traitsTF.Checked;
            }
        }
    }
}
