using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Happy_Search.Other_Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static Happy_Search.StaticHelpers;

namespace Happy_Search
{
    /// <summary>
    /// Struct holding state of filters.
    /// </summary>
    public class Filters
    {
#pragma warning disable 1591
        public string Name = "Custom Filter";

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

        private bool _tagsTraitsMode;
        /// <summary>
        /// True means OR, false means AND
        /// </summary>
        public bool TagsTraitsMode
        {
            get => _tagsTraitsMode;
            set
            {
                _tagsTraitsMode = value;
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
        private bool _ignoreFixed = true;

        private bool LengthFixed { get; set; }
        private bool ReleaseDateFixed { get; set; }
        private bool UnreleasedFixed { get; set; }
        private bool BlacklistFixed { get; set; }
        private bool VotedFixed { get; set; }
        private bool FavoriteProducersFixed { get; set; }
        private bool WishlistFixed { get; set; }
        private bool UserlistFixed { get; set; }
        private bool LanguageFixed { get; set; }
        private bool OriginalLanguageFixed { get; set; }
        private bool TagsFixed { get; set; }
        private bool TraitsFixed { get; set; }

        [JsonIgnore]
        public bool Refresh
        {
            get => _refresh;
            private set
            {
                if (value) _refresh = true;
            }
        }

        public void SetRefreshFalse() => _refresh = false;
#pragma warning restore 1591

        /// <summary>
        /// Load Filters from file.
        /// </summary>
        /// <param name="tab">Control containing fixed statuses.</param>
        public static Filters LoadFilters(FiltersTab tab)
        {
            Filters result;
            try
            {
                var settings = new JsonSerializerSettings() { ContractResolver = new MyContractResolver() };
                result = JsonConvert.DeserializeObject<Filters>(File.ReadAllText(FiltersJson), settings);
            }
            catch (Exception e)
            {
                LogToFile("Failed to load filters.json");
                LogToFile(e);
                result = new Filters();
            }
            result.SetFixedStatusesToForm(tab);
            return result;
        }

        /// <summary>
        /// Load from filter but don't overwrite fixed parameters.
        /// </summary>
        public void SetFromSavedFilter(FiltersTab tab, Filters filter)
        {
            _ignoreFixed = false;
            ClearAll(tab);
            Name = filter.Name;
            Length = filter.Length;
            ReleaseDate = filter.ReleaseDate;
            Unreleased = filter.Unreleased;
            Blacklisted = filter.Blacklisted;
            Voted = filter.Voted;
            FavoriteProducers = filter.FavoriteProducers;
            Wishlist = filter.Wishlist;
            Userlist = filter.Userlist;
            Language = filter.Language;
            OriginalLanguage = filter.OriginalLanguage;
            Tags = filter.Tags;
            Traits = filter.Traits;
            TagsTraitsMode = filter.TagsTraitsMode;
            _ignoreFixed = true;
        }

        /// <summary>
        /// Clear all filters.
        /// </summary>
        /// <param name="tab">Control containing indicators for filter fixed statuses.</param>
        public void ClearAll(FiltersTab tab)
        {
            SetFixedStatusesFromForm(tab);
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
        public void SetFixedStatusesFromForm(FiltersTab tab)
        {
            LengthFixed = tab.lengthFixed.Checked;
            ReleaseDateFixed = tab.releaseDateFixed.Checked;
            UnreleasedFixed = tab.unreleasedFixed.Checked;
            BlacklistFixed = tab.blacklistedFixed.Checked;
            VotedFixed = tab.votedFixed.Checked;
            FavoriteProducersFixed = tab.favoriteProducerFixed.Checked;
            WishlistFixed = tab.wishlistFixed.Checked;
            UserlistFixed = tab.userlistFixed.Checked;
            LanguageFixed = tab.languageFixed.Checked;
            OriginalLanguageFixed = tab.originalLanguageFixed.Checked;
            TagsFixed = tab.tagsFixed.Checked;
            TraitsFixed = tab.traitsFixed.Checked;
        }

        /// <summary>
        /// Sets Fixed Status from filters into form.
        /// </summary>
        public void SetFixedStatusesToForm(FiltersTab tab)
        {
            tab.lengthFixed.Checked = LengthFixed;
            tab.releaseDateFixed.Checked = ReleaseDateFixed;
            tab.unreleasedFixed.Checked = UnreleasedFixed;
            tab.blacklistedFixed.Checked = BlacklistFixed;
            tab.favoriteProducerFixed.Checked = FavoriteProducersFixed;
            tab.wishlistFixed.Checked = WishlistFixed;
            tab.userlistFixed.Checked = UserlistFixed;
            tab.languageFixed.Checked = LanguageFixed;
            tab.originalLanguageFixed.Checked = OriginalLanguageFixed;
            tab.tagsFixed.Checked = TagsFixed;
            tab.traitsFixed.Checked = TraitsFixed;
        }

        /// <summary>
        /// Returns function to filter VNList.
        /// </summary>
        public Func<ListedVN, bool> GetFunction(FormMain form, FiltersTab tab)
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
                var fLanguages =
                    Language.Select(lang => cultures.FirstOrDefault(c => c.DisplayName.Equals(lang))?.Name ?? lang);
                andFunctions.Add(vn => vn.Languages?.All.Any(l => fLanguages.Contains(l)) ?? false);
            }
            if (OriginalLanguage != null && OriginalLanguage.Length > 0)
            {
                var fOriginalLanguages =
                    OriginalLanguage.Select(lang => cultures.FirstOrDefault(c => c.DisplayName.Equals(lang))?.Name ?? lang);
                andFunctions.Add(vn => vn.Languages?.Originals.Any(l => fOriginalLanguages.Contains(l)) ?? false);
            }
            if (Tags != null && Tags.Length > 0)
            {
                if (TagsTraitsMode) orFunctions.Add(tab.VNMatchesTagFilter);
                else andFunctions.Add(tab.VNMatchesTagFilter);
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
                var settings = new JsonSerializerSettings() { ContractResolver = new MyContractResolver() };
                File.WriteAllText(filePath, JsonConvert.SerializeObject(this, Formatting.Indented, settings));
            }
            catch (Exception e)
            {
                LogToFile("Failed to save filters.");
                LogToFile(e);
            }
        }

        /// <inheritdoc />
        public override string ToString() => Name;
    }

#pragma warning disable 1591

    // ReSharper disable UnusedMember.Global
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
        [Description("")]
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
    // ReSharper restore UnusedMember.Global
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

    /// <summary>
    ///     Holds details of a VNDB Tag and its subtags
    /// </summary>
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

    /// <summary>
    ///     Holds details of user-created custom filter
    /// </summary>
    public class CustomTagFilter
    {
        /// <summary>
        ///     Constructor for Custom Tag Filter.
        /// </summary>
        /// <param name="name">User-set name of filter</param>
        /// <param name="filters">List of Tags in filter</param>
        public CustomTagFilter(string name, TagFilter[] filters)
        {
            Name = name;
            Filters = filters;
        }

        /// <summary>
        ///     User-set name of custom filter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     List of tags in custom filter
        /// </summary>
        public TagFilter[] Filters { get; set; }

        /// <summary>
        ///     Date of last update to custom filter
        /// </summary>
        public DateTime Updated { get; set; }

        public override string ToString() => Name;
    }

    /// <summary>
    ///     Holds details of user-created custom trait filter.
    /// </summary>
    public class CustomTraitFilter
    {
        /// <summary>
        ///     Constructor for ComplexFilter (Custom Filter).
        /// </summary>
        /// <param name="name">User-set name of filter</param>
        /// <param name="filters">List of traits in filter</param>
        public CustomTraitFilter(string name, WrittenTrait[] filters)
        {
            Name = name;
            Filters = filters;
        }

        /// <summary>
        ///     User-set name of custom filter
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     List of traits in custom filter
        /// </summary>
        public WrittenTrait[] Filters { get; set; }

        /// <summary>
        ///     Date of last update to custom filter
        /// </summary>
        public DateTime Updated { get; set; }

        public override string ToString() => Name;
    }

    public class MyContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(p => CreateProperty(p, memberSerialization))
                .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(f => CreateProperty(f, memberSerialization)))
                .ToList();
            props.ForEach(p => { p.Writable = true; p.Readable = true; });
            return props;
        }
    }
#pragma warning restore 1591
}
