using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Happy_Search.Other_Forms;
using Newtonsoft.Json;
using static Happy_Search.StaticHelpers;

namespace Happy_Search
{
    /// <summary>
    /// class holding state of filters.
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
                Refresh(value != _length);
                _length = value;
            }
        }

        public DateRange ReleaseDate
        {
            get => _releaseDate;
            set
            {
                if (ReleaseDateFixed && !_ignoreFixed) return;
                Refresh(value != _releaseDate);
                _releaseDate = value;
            }
        }

        public UnreleasedFilter Unreleased
        {
            get => _unreleased;
            set
            {
                if (UnreleasedFixed && !_ignoreFixed) return;
                Refresh(value != _unreleased);
                _unreleased = value;
            }
        }

        public YesNoFilter Blacklisted
        {
            get => _blacklisted;
            set
            {
                if (BlacklistedFixed && !_ignoreFixed) return;
                Refresh(value != _blacklisted);
                _blacklisted = value;
            }
        }

        public YesNoFilter Voted
        {
            get => _voted;
            set
            {
                if (VotedFixed && !_ignoreFixed) return;
                Refresh(value != _voted);
                _voted = value;
            }
        }

        public YesNoFilter FavoriteProducers
        {
            get => _favoriteProducers;
            set
            {
                if (FavoriteProducersFixed && !_ignoreFixed) return;
                Refresh(value != _favoriteProducers);
                _favoriteProducers = value;
            }
        }

        public WishlistFilter Wishlist
        {
            get => _wishlist;
            set
            {
                if (WishlistFixed && !_ignoreFixed) return;
                Refresh(value != _wishlist);
                _wishlist = value;
            }
        }

        public UserlistFilter Userlist
        {
            get => _userlist;
            set
            {
                if (UserlistFixed && !_ignoreFixed) return;
                Refresh(value != _userlist);
                _userlist = value;
            }
        }

        public readonly BindingList<string> Language = new BindingList<string>();
        public readonly BindingList<string> OriginalLanguage = new BindingList<string>();
        public readonly BindingList<TagFilter> Tags = new BindingList<TagFilter>();
        public readonly BindingList<WrittenTrait> Traits = new BindingList<WrittenTrait>();
        /// <summary>
        /// True means OR, false means AND
        /// </summary>
        public bool TagsTraitsMode
        {
            get => _tagsTraitsMode;
            set
            {
                Refresh(value != _tagsTraitsMode);
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
        private bool _tagsTraitsMode;
        private bool _ignoreFixed = true;

        public static Filters FromFixedFilter(FixedFilter filter)
        {
            var filters = new Filters
            {
                Length = filter.Length,
                ReleaseDate = filter.ReleaseDate,
                Unreleased = filter.Unreleased,
                Blacklisted = filter.Blacklisted,
                Voted = filter.Voted,
                FavoriteProducers = filter.FavoriteProducers,
                Wishlist = filter.Wishlist,
                Userlist = filter.Userlist,
                TagsTraitsMode = filter.TagsTraitsMode,

                LengthFixed = filter.LengthFixed,
                ReleaseDateFixed = filter.ReleaseDateFixed,
                UnreleasedFixed = filter.UnreleasedFixed,
                BlacklistedFixed = filter.BlacklistFixed,
                VotedFixed = filter.VotedFixed,
                FavoriteProducersFixed = filter.FavoriteProducersFixed,
                WishlistFixed = filter.WishlistFixed,
                UserlistFixed = filter.UserlistFixed,
                LanguageFixed = filter.LanguageFixed,
                OriginalLanguageFixed = filter.OriginalLanguageFixed,
                TagsFixed = filter.TagsFixed,
                TraitsFixed = filter.TraitsFixed,

                OriginalLanguageOn = filter.OriginalLanguageOn
            };
            filters.Language.AddRange(filter.Language);
            filters.OriginalLanguage.AddRange(filter.OriginalLanguage);
            filters.Tags.AddRange(filter.Tags);
            filters.Traits.AddRange(filter.Traits);
            return filters;
        }

        public bool LengthFixed { get; set; }
        public bool ReleaseDateFixed { get; set; }
        public bool UnreleasedFixed { get; set; }
        public bool BlacklistedFixed { get; set; }
        public bool VotedFixed { get; set; }
        public bool FavoriteProducersFixed { get; set; }
        public bool WishlistFixed { get; set; }
        public bool UserlistFixed { get; set; }
        public bool LanguageFixed { get; set; }
        public bool OriginalLanguageFixed { get; set; }
        public bool TagsFixed { get; set; }
        public bool TraitsFixed { get; set; }

        [JsonIgnore]
        public RefreshType RefreshKind { get; set; }

        public bool LengthOn { get; set; }
        public bool ReleaseDateOn { get; set; }
        public bool UnreleasedOn { get; set; }
        public bool BlacklistedOn { get; set; }
        public bool VotedOn { get; set; }
        public bool FavoriteProducersOn { get; set; }
        public bool WishlistOn { get; set; }
        public bool UserlistOn { get; set; }
        public bool LanguageOn { get; set; }
        public bool OriginalLanguageOn { get; set; }
        public bool TagsOn { get; set; }
        public bool TraitsOn { get; set; }

        private void Refresh(bool value)
        {
            if (!value) return;
            RefreshKind = RefreshType.UserChanged;
        }


#pragma warning restore 1591

        /// <summary>
        /// Load Filters from file.
        /// </summary>
        public static Filters LoadFixedFilter()
        {
            Filters result;
            try
            {
                var fixedFilter = JsonConvert.DeserializeObject<FixedFilter>(File.ReadAllText(FiltersJson));
                result = FromFixedFilter(fixedFilter);
            }
            catch (Exception e)
            {
                LogToFile("Failed to load filters.json");
                LogToFile(e);
                result = new Filters();
            }
            return result;
        }

        /// <summary>
        /// Load from filter but don't overwrite fixed parameters.
        /// </summary>
        public void SetFromSavedFilter(CustomFilter filter)
        {
            var previousRefreshKind = RefreshKind;
            _ignoreFixed = false;
            Name = filter.Name;
            Length = filter.Length;
            ReleaseDate = filter.ReleaseDate;
            Unreleased = filter.Unreleased;
            Blacklisted = filter.Blacklisted;
            Voted = filter.Voted;
            FavoriteProducers = filter.FavoriteProducers;
            Wishlist = filter.Wishlist;
            Userlist = filter.Userlist;
            Language.AddRange(filter.Language);
            OriginalLanguage.AddRange(filter.OriginalLanguage);
            Tags.AddRange(filter.Tags);
            Traits.AddRange(filter.Traits);
            TagsTraitsMode = filter.TagsTraitsMode;
            _ignoreFixed = true;
            //if it wasn't set to refresh previously, then set it to refresh if any change occured, else set it to refresh anyway
            if (previousRefreshKind == RefreshType.None) RefreshKind = RefreshKind == RefreshType.UserChanged ? RefreshType.NamedFilter : RefreshType.None;
            else RefreshKind = RefreshType.NamedFilter;
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
            if (LengthOn) andFunctions.Add(vn => (t.Length & vn.Length) != 0);
            if (ReleaseDateOn) andFunctions.Add(vn => vn.DateForSorting > t.ReleaseDate.From && vn.DateForSorting < t.ReleaseDate.To);
            if (UnreleasedOn) andFunctions.Add(vn => (t.Unreleased & vn.Unreleased) != 0);
            if (BlacklistedOn) andFunctions.Add(vn => t.Blacklisted == vn.Blacklisted);
            if (VotedOn) andFunctions.Add(vn => t.Voted == vn.Voted);
            if (FavoriteProducersOn) andFunctions.Add(vn => t.FavoriteProducers == vn.ByFavoriteProducer(form.FavoriteProducerList));
            if (WishlistOn) andFunctions.Add(vn => (t.Wishlist & vn.Wishlist) != 0);
            if (UserlistOn) andFunctions.Add(vn => (t.Userlist & vn.Userlist) != 0);
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            if (LanguageOn && Language.Count > 0)
            {
                var fLanguages =
                    Language.Select(lang => cultures.FirstOrDefault(c => c.DisplayName.Equals(lang))?.Name ?? lang);
                andFunctions.Add(vn => vn.Languages?.All.Any(l => fLanguages.Contains(l)) ?? false);
            }
            if (OriginalLanguageOn && OriginalLanguage.Count > 0)
            {
                var fOriginalLanguages =
                    OriginalLanguage.Select(lang => cultures.FirstOrDefault(c => c.DisplayName.Equals(lang))?.Name ?? lang);
                andFunctions.Add(vn => vn.Languages?.Originals.Any(l => fOriginalLanguages.Contains(l)) ?? false);
            }
            if (TagsOn && Tags.Count > 0)
            {
                if (TagsTraitsMode) orFunctions.Add(tab.VNMatchesTagFilter);
                else andFunctions.Add(tab.VNMatchesTagFilter);
            }
            if (TraitsOn && Traits.Count > 0)
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
                File.WriteAllText(filePath, JsonConvert.SerializeObject((FixedFilter)this, Formatting.Indented));
            }
            catch (Exception e)
            {
                LogToFile("Failed to save filters.");
                LogToFile(e);
            }
        }

        /// <inheritdoc />
        public override string ToString() => Name;

        /// <summary>
        /// Sets filters from given custom filter to active filter.
        /// </summary>
        public void SetCustomFilter(CustomFilter customFilter)
        {
            Name = customFilter.Name;
            Length = customFilter.Length;
            ReleaseDate = customFilter.ReleaseDate;
            Unreleased = customFilter.Unreleased;
            Blacklisted = customFilter.Blacklisted;
            Voted = customFilter.Voted;
            FavoriteProducers = customFilter.FavoriteProducers;
            Wishlist = customFilter.Wishlist;
            Userlist = customFilter.Userlist;
            Language.AddRange(customFilter.Language);
            OriginalLanguage.AddRange(customFilter.OriginalLanguage);
            Tags.AddRange(customFilter.Tags);
            Traits.AddRange(customFilter.Traits);
            TagsTraitsMode = customFilter.TagsTraitsMode;
            LanguageOn = customFilter.LanguageOn;
            OriginalLanguageOn = customFilter.LanguageOn;
            TagsOn = customFilter.LanguageOn;
            TraitsOn = customFilter.LanguageOn;
        }
    }


}
