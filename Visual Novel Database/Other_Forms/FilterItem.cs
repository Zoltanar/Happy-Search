using System;
using System.ComponentModel;
using System.Globalization;
using Happy_Apps_Core;

namespace Happy_Search.Other_Forms
{
    /// <summary>
    /// New custom filter class (second overhaul)
    /// </summary>
    public class FilterItem
    {
#pragma warning disable 1591
        public FilterType Type { get; set; }
        public object Value { get; set; }
        public bool Exclude { get; set; }
#pragma warning restore 1591

        /// <summary>
        /// Create custom filter
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="exclude"></param>
        public FilterItem(FilterType type, object value, bool exclude)
        {
            Type = type;
            Value = value;
            if (type == FilterType.Voted || type == FilterType.Blacklisted || type == FilterType.ByFavoriteProducer)
            {
                Exclude = !(bool)value;
            }
            else Exclude = exclude;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            switch (Type)
            {
                case FilterType.Length:
                    return $"{(Exclude ? "Exclude" : "Include")}: {Type.GetDescription()} - {((LengthFilter)Value).GetDescription()}";
                case FilterType.ReleaseStatus:
                    return $"{(Exclude ? "Exclude" : "Include")}: {Type.GetDescription()} - {(UnreleasedFilter)Value}";
                case FilterType.WishlistStatus:
                    return $"{(Exclude ? "Exclude" : "Include")}: {Type.GetDescription()} - {(WishlistStatus)Value}";
                case FilterType.UserlistStatus:
                    return $"{(Exclude ? "Exclude" : "Include")}: {Type.GetDescription()} - {(UserlistStatus)Value}";
                case FilterType.Voted:
                case FilterType.Blacklisted:
                case FilterType.ByFavoriteProducer:
                    return $"{(Exclude ? "Exclude" : "Include")}: {Type.GetDescription()}";
                case FilterType.Language:
                case FilterType.OriginalLanguage:
                    return $"{(Exclude ? "Exclude" : "Include")}: {Type.GetDescription()} - { CultureInfo.GetCultureInfo((string)Value).DisplayName}";
                case FilterType.Tags:
                    return $"{(Exclude ? "Exclude" : "Include")}: {Type.GetDescription()} - {DumpFiles.PlainTags.Find(x=>x.ID == (int)(long)Value).Name}";
                case FilterType.Traits:
                    return $"{(Exclude ? "Exclude" : "Include")}: {Type.GetDescription()} - {DumpFiles.PlainTraits.Find(x => x.ID == (int)(long)Value).Name}";
                default:
                    return $"{(Exclude ? "Exclude" : "Include")}: {Type.GetDescription()} - {Value}";
            }
        }

        /// <summary>
        /// Describes type of filter
        /// </summary>
        public enum FilterType
        {
#pragma warning disable 1591
            Length = 0,
            //ReleasedBetween = 1,
            [Description("Release Status")]
            ReleaseStatus = 2,
            Blacklisted = 3,
            Voted = 4,
            [Description("By Favorite Producer")]
            ByFavoriteProducer = 5,
            [Description("Wishlist Status")]
            WishlistStatus = 6,
            [Description("Userlist Status")]
            UserlistStatus = 7,
            Language = 8,
            [Description("Original Language")]
            OriginalLanguage = 9,
            Tags = 10,
            Traits = 11
#pragma warning restore 1591
        }

        /// <summary>
        /// Gets function that determines if vn matches filter.
        /// </summary>
        /// <returns></returns>
        public Func<ListedVN, bool> GetFunction()
        {
            switch (Type)
            {
                case FilterType.Length:
                    return vn => vn.Length == (LengthFilter)Value != Exclude;
                case FilterType.ReleaseStatus:
                    return vn => vn.Unreleased == (UnreleasedFilter)Value != Exclude;
                case FilterType.Voted:
                    return vn => vn.Voted != Exclude;
                case FilterType.Blacklisted:
                    return vn => vn.Blacklisted != Exclude;
                case FilterType.ByFavoriteProducer:
                    return vn => vn.ByFavoriteProducer(StaticHelpers.LocalDatabase.FavoriteProducerList) != Exclude;
                case FilterType.WishlistStatus:
                    return vn => vn.WLStatus == (WishlistStatus)Value != Exclude;
                case FilterType.UserlistStatus:
                    return vn => vn.ULStatus == (UserlistStatus)Value != Exclude;
                case FilterType.Language:
                    return vn => vn.HasLanguage((string)Value) != Exclude;
                case FilterType.OriginalLanguage:
                    return vn => vn.HasOriginalLanguage((string)Value) != Exclude;
                case FilterType.Tags:
                    return vn => vn.MatchesSingleTag((int)(long)Value);
                case FilterType.Traits:
                    return vn => vn.MatchesSingleTrait((int)(long)Value);
            }
            return vn => true;
        }
    }
}