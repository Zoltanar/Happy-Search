using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using static Happy_Search.StaticHelpers;

namespace Happy_Search
{
    /// <summary>
    /// Struct holding state of filters.
    /// </summary>
    public struct Filters
    {
#pragma warning disable 1591
        public LengthFilter Length
        {
            get => _length;
            set
            {
                if (LengthFixed && !_ignoreFixed) return;
                Refresh = value != _length;
                _length = value;
            }
        }
        public DateRange ReleaseDate
        {
            get => _releaseDate;
            set
            {
                if (ReleaseDateFixed && !_ignoreFixed) return;
                Refresh = value != _releaseDate;
                _releaseDate = value;
            }
        }
        public UnreleasedFilter Unreleased
        {
            get => _unreleased;
            set
            {
                if (UnreleasedFixed && !_ignoreFixed) return;
                Refresh = value != _unreleased;
                _unreleased = value;
            }
        }
        public YesNoFilter Blacklisted
        {
            get => _blacklisted;
            set
            {
                if (BlacklistFixed && !_ignoreFixed) return;
                Refresh = value != _blacklisted;
                _blacklisted = value;
            }
        }
        public YesNoFilter Voted
        {
            get => _voted;
            set
            {
                if (VotedFixed && !_ignoreFixed) return;
                Refresh = value != _voted;
                _voted = value;
            }
        }
        public YesNoFilter FavoriteProducers
        {
            get => _favoriteProducers;
            set
            {
                if (FavoriteProducersFixed && !_ignoreFixed) return;
                Refresh = value != _favoriteProducers;
                _favoriteProducers = value;
            }
        }
        public WishlistFilter Wishlist
        {
            get => _wishlist;
            set
            {
                if (WishlistFixed && !_ignoreFixed) return;
                Refresh = value != _wishlist;
                _wishlist = value;
            }
        }
        public UserlistFilter Userlist
        {
            get => _userlist;
            set
            {
                if (UserlistFixed && !_ignoreFixed) return;
                Refresh = value != _userlist;
                _userlist = value;
            }
        }
        public string[] Language
        {
            get => _language;
            set
            {
                if (LanguageFixed && !_ignoreFixed) return;
                Refresh = !value.SequenceEqualNullAware(_language);
                _language = value;
            }
        }
        public string[] OriginalLanguage
        {
            get => _originalLanguage;
            set
            {
                if (OriginalLanguageFixed && !_ignoreFixed) return;
                Refresh = !value.SequenceEqualNullAware(_originalLanguage);
                _originalLanguage = value;
            }
        }
        public TagFilter[] Tags
        {
            get => _tags;
            set
            {
                if (TagsFixed && !_ignoreFixed) return;
                Refresh = !value.SequenceEqualNullAware(_tags);
                _tags = value;
            }
        }
        public WrittenTrait[] Traits
        {
            get => _traits;
            set
            {
                if (TraitsFixed && !_ignoreFixed) return;
                Refresh = !value.SequenceEqualNullAware(_traits);
                _traits = value;
            }
        }

        private LengthFilter _length;
        private DateRange _releaseDate;
        private UnreleasedFilter _unreleased;
        private YesNoFilter _blacklisted;
        private YesNoFilter _voted;
        private YesNoFilter _favoriteProducers;
        private WishlistFilter _wishlist;
        private UserlistFilter _userlist;
        private string[] _language;
        private string[] _originalLanguage;
        private TagFilter[] _tags;
        private WrittenTrait[] _traits;
        private bool _refresh;
        private bool _ignoreFixed;

        public bool LengthFixed { get; set; }
        public bool ReleaseDateFixed { get; set; }
        public bool UnreleasedFixed { get; set; }
        public bool BlacklistFixed { get; set; }
        public bool VotedFixed { get; set; }
        public bool FavoriteProducersFixed { get; set; }
        public bool WishlistFixed { get; set; }
        public bool UserlistFixed { get; set; }
        public bool LanguageFixed { get; set; }
        public bool OriginalLanguageFixed { get; set; }
        public bool TagsFixed { get; set; }
        public bool TraitsFixed { get; set; }
        /// <summary>
        /// True means OR, false means AND
        /// </summary>
        public bool TagsTraitsMode { get; internal set; }

        public bool Refresh
        {
            get { return _refresh; }
            private set
            {
                if (value) _refresh = true;
            }
        }

        public void SetRefreshFalse() => _refresh = false;
#pragma warning restore 1591


        /// <summary>
        /// Clear all filters.
        /// </summary>
        /// <param name="form">Form containing indicators for filter fixed statuses.</param>
        public void ClearAll(FormMain form)
        {
            SetFixedStatusesFromForm(form);
            Length = LengthFilter.Off;
            ReleaseDate = null;
            Unreleased = UnreleasedFilter.Off;
            Blacklisted = YesNoFilter.Off;
            Voted = YesNoFilter.Off;
            FavoriteProducers = YesNoFilter.Off;
            Wishlist = WishlistFilter.Off;
            Userlist = UserlistFilter.Off;
            Language = null;
            OriginalLanguage = null;
            Tags = null;
            Traits = null;
        }

        /// <summary>
        /// Sets Fixed Status for filters from info in form.
        /// </summary>
        public void SetFixedStatusesFromForm(FormMain form)
        {
            LengthFixed = form.lengthFixed.Checked;
            ReleaseDateFixed = form.releaseDateFixed.Checked;
            UnreleasedFixed = form.unreleasedFixed.Checked;
            BlacklistFixed = form.blacklistedFixed.Checked;
            VotedFixed = form.votedFixed.Checked;
            FavoriteProducersFixed = form.favoriteProducersFixed.Checked;
            WishlistFixed = form.wishlistFixed.Checked;
            UserlistFixed = form.userlistFixed.Checked;
            LanguageFixed = form.languageFixed.Checked;
            OriginalLanguageFixed = form.originalLanguageFixed.Checked;
            TagsFixed = form.tagsFixed.Checked;
            TraitsFixed = form.traitsFixed.Checked;
        }


        /// <summary>
        /// Sets Fixed Status from filters into form.
        /// </summary>
        public void SetFixedStatusesToForm(FormMain form)
        {
            form.lengthFixed.Checked = LengthFixed;
            form.releaseDateFixed.Checked = ReleaseDateFixed;
            form.unreleasedFixed.Checked = UnreleasedFixed;
            form.blacklistedFixed.Checked = BlacklistFixed;
            form.favoriteProducersFixed.Checked = FavoriteProducersFixed;
            form.wishlistFixed.Checked = WishlistFixed;
            form.userlistFixed.Checked = UserlistFixed;
            form.languageFixed.Checked = LanguageFixed;
            form.originalLanguageFixed.Checked = OriginalLanguageFixed;
            form.tagsFixed.Checked = TagsFixed;
            form.traitsFixed.Checked = TraitsFixed;
        }

        /// <summary>
        /// Returns function to filter VNList.
        /// </summary>
        public Func<ListedVN, bool> GetFunction(FormMain form)
        {
            //private ListBox.ObjectCollection _tags;
            //private ListBox.ObjectCollection _traits;
            var andFunctions = new List<Func<ListedVN, bool>>();
            var orFunctions = new List<Func<ListedVN, bool>>();
            Filters t = this;
            if (Length != LengthFilter.Off) andFunctions.Add(vn => (t.Length & vn.Length) != 0);
            if (ReleaseDate != null) andFunctions.Add(vn => vn.DateForSorting > t.ReleaseDate.From && vn.DateForSorting < t.ReleaseDate.To);
            if (Unreleased != UnreleasedFilter.Off) andFunctions.Add(vn => (t.Unreleased & vn.Unreleased) != 0);
            if (Blacklisted != YesNoFilter.Off) andFunctions.Add(vn => t.Blacklisted == vn.Blacklisted);
            if (Voted != YesNoFilter.Off) andFunctions.Add(vn => t.Voted == vn.Voted);
            if (FavoriteProducers != YesNoFilter.Off) andFunctions.Add(vn => t.FavoriteProducers == vn.ByFavoriteProducer(form.FavoriteProducerList));
            if (Wishlist != WishlistFilter.Off) andFunctions.Add(vn => (t.Wishlist & vn.Wishlist) != 0);
            if (Userlist != UserlistFilter.Off) andFunctions.Add(vn => (t.Userlist & vn.Userlist) != 0);
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            if (Language != null && Language.Length > 0)
            {
                try
                {
                    var fLanguages =
                        Language.Select(lang => cultures.FirstOrDefault(c => c.DisplayName.Equals(lang)).Name);
                    andFunctions.Add(vn => vn.Languages?.All.Any(l => fLanguages.Contains(l)) ?? false);
                }
                catch (Exception e)
                {
                    LogToFile(e);
                }
            }
            if (OriginalLanguage != null && OriginalLanguage.Length > 0)
            {
                try
                {
                    var fOriginalLanguages =
                        OriginalLanguage.Select(lang => cultures.FirstOrDefault(c => c.DisplayName.Equals(lang)).Name);
                    andFunctions.Add(vn => vn.Languages?.Originals.Any(l => fOriginalLanguages.Contains(l)) ?? false);
                }
                catch (Exception e)
                {
                    LogToFile(e);
                }
            }
            if (Tags != null && Tags.Length > 0)
            {
                if (TagsTraitsMode) orFunctions.Add(form.VNMatchesTagFilter);
                else andFunctions.Add(form.VNMatchesTagFilter);
            }
            if (Traits != null && Traits.Length > 0)
            {
                Stopwatch watch = Stopwatch.StartNew();
                //Make list of characters which contain traits
                List<CharacterItem> chars = form.CharacterList.ToList();
                IEnumerable<CharacterItem> traitCharacters = chars.Where(x => x.ContainsTraits(t.Traits)).ToList();
                //get all vns that those characters are in
                var characterVNs = traitCharacters.SelectMany(ci => ci.VNs.Select(civn => civn.ID));
                Debug.WriteLine($"Preparing Trait Function took {watch.ElapsedMilliseconds}ms.");
                if (TagsTraitsMode) orFunctions.Add(vn => characterVNs.Contains(vn.VNID));
                else andFunctions.Add(vn => characterVNs.Contains(vn.VNID));
            }
            if (andFunctions.Count + orFunctions.Count == 0) return vn => true;
            if (andFunctions.Count > 0 & orFunctions.Count == 0) return vn => andFunctions.Select(filter => filter(vn)).All(valid => valid);
            if (andFunctions.Count == 0 & orFunctions.Count > 0) return vn => orFunctions.Select(orFilter => orFilter(vn)).Any(valid => valid);
            return vn => andFunctions.Select(filter => filter(vn)).All(valid => valid) &&
                         orFunctions.Select(orFilter => orFilter(vn)).Any(valid => valid);

        }

        /// <summary>
        /// Save filters to a file as a JSON string.
        /// </summary>
        public void SaveFilters(string filePath)
        {
            try
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(this, Formatting.Indented));
            }
            catch (Exception e)
            {
                LogToFile("Failed to save filters.");
                LogToFile(e);
            }
        }

        /// <summary>
        /// To be called before setting a custom filter.
        /// </summary>
        public void StartLoadingCustomFilter(FormMain form)
        {
            _ignoreFixed = false;
            ClearAll(form);
        }

        /// <summary>
        /// To be called after setting a custom filter.
        /// </summary>
        public void EndLoadingCustomFilter()
        {
            _ignoreFixed = true;
        }

        /// <summary>
        /// To be called once on startup.
        /// </summary>
        public void Startup()
        {
            _ignoreFixed = true;
        }
    }

#pragma warning disable 1591

    [Flags]
    public enum UnreleasedFilter
    {
        Off = 0,
        WithoutReleaseDate = 1,
        WithReleaseDate = 2,
        Released = 4,
        AllUnreleased = WithReleaseDate | WithoutReleaseDate,
        ReleasedOrWithDate = WithReleaseDate | Released
    }

    public enum YesNoFilter
    {
        Off = 0,
        No = 1,
        Yes = 2
    }

    [Flags]
    public enum WishlistFilter
    {
        Off = 0,
        NA = 1,
        High = 2,
        Medium = 4,
        Low = 8,
        Any = High | Medium | Low
    }

    [Flags]
    public enum UserlistFilter
    {
        Off = 0,
        NA = 1,
        Unknown = 2,
        Playing = 4,
        Finished = 8,
        Stalled = 16,
        Dropped = 32,
        Unplayed = NA | Unknown,
        Any = Unknown | Playing | Finished | Stalled | Dropped
    }

    [Flags]
    public enum LengthFilter
    {
        Off = 0,
        [Description("N/A")]
        NA = 1,
        [Description("<2 Hours")]
        UnderTwoHours = 2,
        [Description("2-10 Hours")]
        TwoToTenHours = 4,
        [Description("10-30 Hours")]
        TenToThirtyHours = 8,
        [Description("30-50 Hours")]
        ThirtyToFiftyHours = 16,
        [Description(">50 Hours")]
        OverFiftyHours = 32,
        Any = UnderTwoHours | TwoToTenHours | TenToThirtyHours | ThirtyToFiftyHours | OverFiftyHours
    }
    /// <summary>
    /// Readonly class to store from and to dates of a date range.
    /// </summary>
    public class DateRange
    {
        public readonly DateTime From;
        public readonly DateTime To;
        public DateRange(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }
    }
#pragma warning restore 1591
}
