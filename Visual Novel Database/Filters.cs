using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Happy_Search.Other_Forms;
using Newtonsoft.Json;
using Happy_Apps_Core;
using static Happy_Apps_Core.StaticHelpers;

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
                Refresh(value != _length);
                _length = value;
            }
        }

        public DateRange ReleaseDate
        {
            get => _releaseDate;
            set
            {
                Refresh(!value.Equals(_releaseDate));
                _releaseDate = value;
            }
        }

        public UnreleasedFilter Unreleased
        {
            get => _unreleased;
            set
            {
                Refresh(value != _unreleased);
                _unreleased = value;
            }
        }

        public bool Blacklisted
        {
            get => _blacklisted;
            set
            {
                Refresh(value != _blacklisted);
                _blacklisted = value;
            }
        }

        public bool Voted
        {
            get => _voted;
            set
            {
                Refresh(value != _voted);
                _voted = value;
            }
        }

        public bool FavoriteProducers
        {
            get => _favoriteProducers;
            set
            {
                Refresh(value != _favoriteProducers);
                _favoriteProducers = value;
            }
        }

        public readonly BindingList<string> Language = new BindingList<string>();
        public readonly BindingList<string> OriginalLanguage = new BindingList<string>();
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
        private bool _blacklisted;
        private bool _voted;
        private bool _favoriteProducers;
        private bool _tagsTraitsMode;

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

                LengthOn = filter.LengthOn,
                ReleaseDateOn = filter.ReleaseDateOn,
                UnreleasedOn = filter.UnreleasedOn,
                BlacklistedOn = filter.BlacklistedOn,
                VotedOn = filter.VotedOn,
                FavoriteProducersOn = filter.FavoriteProducersOn,
                WishlistOn = filter.WishlistOn,
                UserlistOn = filter.UserlistOn,
                LanguageOn = filter.LanguageOn,
                OriginalLanguageOn = filter.OriginalLanguageOn,
                TagsOn = filter.TagsOn,
                TraitsOn = filter.TraitsOn
            };
            filters.Language.AddRange(filter.Language);
            filters.OriginalLanguage.AddRange(filter.OriginalLanguage);
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

        private void Refresh(bool valueIsDifferent)
        {
            if (!valueIsDifferent) return;
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
                LogToFile("Failed to load filters.json", e);
                result = new Filters();
            }
            if (result.ReleaseDate.Equals(default(DateRange))) result.ReleaseDate = DateRange.Default();
            return result;
        }

        /// <summary>
        /// Returns function to filter VNList.
        /// </summary>
        public Func<ListedVN, bool> GetFunction(FiltersTab tab)
        {
            var andFunctions = new List<Func<ListedVN, bool>>();
            var orFunctions = new List<Func<ListedVN, bool>>();
            Filters t = this;
            if (LengthOn) andFunctions.Add(vn => t.Length == vn.Length);
            if (ReleaseDateOn) andFunctions.Add(vn => vn.DateForSorting > t.ReleaseDate.From && vn.DateForSorting < t.ReleaseDate.To);
            if (UnreleasedOn) andFunctions.Add(vn => (t.Unreleased & vn.Unreleased) != 0);
            if (BlacklistedOn) andFunctions.Add(vn => t.Blacklisted == vn.Blacklisted);
            if (VotedOn) andFunctions.Add(vn => t.Voted == vn.Voted);
            if (FavoriteProducersOn) andFunctions.Add(vn => t.FavoriteProducers == vn.ByFavoriteProducer(LocalDatabase.FavoriteProducerList));
            //if (WishlistOn) andFunctions.Add(vn => (t.Wishlist & vn.Wishlist) != 0);
            //if (UserlistOn) andFunctions.Add(vn => (t.Userlist & vn.Userlist) != 0);
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
            /*if (TagsOn && Tags.Count > 0)
            {
                if (TagsTraitsMode) orFunctions.Add(tab.VNMatchesTagFilter);
                else andFunctions.Add(tab.VNMatchesTagFilter);
            }*/
            /*if (TraitsOn && Traits.Count > 0)
            {
                var watch = Stopwatch.StartNew();
                //Make list of characters which contain traits
                List<CharacterItem> chars = LocalDatabase.CharacterList.ToList();
                IEnumerable<CharacterItem> traitCharacters = chars.Where(x => x.ContainsTraits(t.Traits)).ToList();
                //get all vns that those characters are in
                var characterVNs = traitCharacters.SelectMany(ci => ci.VNs.Select(civn => civn.ID));
                Debug.WriteLine($"Preparing Trait Function took {watch.ElapsedMilliseconds}ms.");
                if (TagsTraitsMode) orFunctions.Add(vn => characterVNs.Contains(vn.VNID));
                else andFunctions.Add(vn => characterVNs.Contains(vn.VNID));
            }*/
            if (andFunctions.Count + orFunctions.Count == 0) return vn => true;
            if (andFunctions.Count > 0 & orFunctions.Count == 0) return vn => andFunctions.Select(filter => filter(vn)).All(valid => valid);
            if (andFunctions.Count == 0 & orFunctions.Count > 0) return vn => orFunctions.Select(orFilter => orFilter(vn)).Any(valid => valid);
            return vn => andFunctions.Select(filter => filter(vn)).All(valid => valid) && orFunctions.Select(orFilter => orFilter(vn)).Any(valid => valid);

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
                LogToFile("Failed to save filters.", e);
            }
        }

        /// <inheritdoc />
        public override string ToString() => Name;

        /// <summary>
        /// Load from filter but don't overwrite fixed parameters.
        /// </summary>
        public void LoadCustomFilter(CustomFilter customFilter)
        {
            Name = customFilter.Name;
            if (!LengthFixed) Length = customFilter.Length;
            if (!ReleaseDateFixed) ReleaseDate = customFilter.ReleaseDate;
            if (ReleaseDate.Equals(default(DateRange))) ReleaseDate = DateRange.Default();
            if (!UnreleasedFixed) Unreleased = customFilter.Unreleased;
            if (!BlacklistedFixed) Blacklisted = customFilter.Blacklisted;
            if (!VotedFixed) Voted = customFilter.Voted;
            if (!FavoriteProducersFixed) FavoriteProducers = customFilter.FavoriteProducers;
            if (!LengthFixed) Language.SetRange(customFilter.Language.ToArray());
            if (!OriginalLanguageFixed) OriginalLanguage.SetRange(customFilter.OriginalLanguage.ToArray());
            TagsTraitsMode = customFilter.TagsTraitsMode;

            if (!LengthFixed) LengthOn = customFilter.LengthOn;
            if (!ReleaseDateFixed) ReleaseDateOn = customFilter.ReleaseDateOn;
            if (!UnreleasedFixed) UnreleasedOn = customFilter.UnreleasedOn;
            if (!BlacklistedFixed) BlacklistedOn = customFilter.BlacklistedOn;
            if (!VotedFixed) VotedOn = customFilter.VotedOn;
            if (!FavoriteProducersFixed) FavoriteProducersOn = customFilter.FavoriteProducersOn;
            if (!WishlistFixed) WishlistOn = customFilter.WishlistOn;
            if (!UserlistFixed) UserlistOn = customFilter.UserlistOn;
            if (!LengthFixed) LanguageOn = customFilter.LanguageOn;
            if (!OriginalLanguageFixed) OriginalLanguageOn = customFilter.OriginalLanguageOn;
            if (!TagsFixed) TagsOn = customFilter.TagsOn;
            if (!TraitsFixed) TraitsOn = customFilter.TraitsOn;

            if(RefreshKind != RefreshType.None) RefreshKind = RefreshType.NamedFilter;

        }
    }


}
