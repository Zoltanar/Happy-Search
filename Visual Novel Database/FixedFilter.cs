using System.Linq;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
#pragma warning disable 1591

namespace Happy_Search
{
    public struct FixedFilter
    {
        public LengthFilter Length { get; set; }
        public DateRange ReleaseDate { get; set; }
        public UnreleasedFilter Unreleased { get; set; }
        public YesNoFilter Blacklisted { get; set; }
        public YesNoFilter Voted { get; set; }
        public YesNoFilter FavoriteProducers { get; set; }
        public WishlistFilter Wishlist { get; set; }
        public UserlistFilter Userlist { get; set; }
        public string[] Language { get; set; }
        public string[] OriginalLanguage { get; set; }
        public TagFilter[] Tags { get; set; }
        public WrittenTrait[] Traits { get; set; }
        /// <summary>
        /// True means OR, false means AND
        /// </summary>
        public bool TagsTraitsMode { get; set; }

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

        public static explicit operator FixedFilter(Filters filter)
        {
            var result = new FixedFilter
            {
                Length = filter.Length,
                ReleaseDate = filter.ReleaseDate,
                Unreleased = filter.Unreleased,
                Blacklisted = filter.Blacklisted,
                Voted = filter.Voted,
                FavoriteProducers = filter.FavoriteProducers,
                Wishlist = filter.Wishlist,
                Userlist = filter.Userlist,
                Language = filter.Language.ToArray(),
                OriginalLanguage = filter.OriginalLanguage.ToArray(),
                Tags = filter.Tags.ToArray(),
                Traits = filter.Traits.ToArray(),
                TagsTraitsMode = filter.TagsTraitsMode,

                LengthFixed = filter.LengthFixed,
                ReleaseDateFixed = filter.ReleaseDateFixed,
                UnreleasedFixed = filter.UnreleasedFixed,
                BlacklistFixed = filter.BlacklistedFixed,
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
            return result;
        }

    }

    public class CustomFilter
    {
        public override string ToString() => Name;

        public string Name { get; set; }
        public LengthFilter Length { get; set; }
        public DateRange ReleaseDate { get; set; }
        public UnreleasedFilter Unreleased { get; set; }
        public YesNoFilter Blacklisted { get; set; }
        public YesNoFilter Voted { get; set; }
        public YesNoFilter FavoriteProducers { get; set; }
        public WishlistFilter Wishlist { get; set; }
        public UserlistFilter Userlist { get; set; }
        public string[] Language { get; set; }
        public string[] OriginalLanguage { get; set; }
        public TagFilter[] Tags { get; set; }
        public WrittenTrait[] Traits { get; set; }
        /// <summary>
        /// True means OR, false means AND
        /// </summary>
        public bool TagsTraitsMode { get; set; }

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

        public static explicit operator CustomFilter(Filters filter)
        {
            return new CustomFilter
            {
                Name = filter.Name,
                Length = filter.Length,
                ReleaseDate = filter.ReleaseDate,
                Unreleased = filter.Unreleased,
                Blacklisted = filter.Blacklisted,
                Voted = filter.Voted,
                FavoriteProducers = filter.FavoriteProducers,
                Wishlist = filter.Wishlist,
                Userlist = filter.Userlist,
                Language = filter.Language.ToArray(),
                OriginalLanguage = filter.OriginalLanguage.ToArray(),
                Tags = filter.Tags.ToArray(),
                Traits = filter.Traits.ToArray(),
                TagsTraitsMode = filter.TagsTraitsMode,

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
        }
    }
}
