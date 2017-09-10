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
                case FilterType.Voted:
                case FilterType.Blacklisted:
                case FilterType.ByFavoriteProducer:
                    return $"{(Exclude ? "Exclude" : "Include")}: {Type.GetDescription()}";
                case FilterType.Language:
                case FilterType.OriginalLanguage:
                    return $"{(Exclude ? "Exclude" : "Include")}: {Type.GetDescription()} - { CultureInfo.GetCultureInfo((string)Value).DisplayName}";
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
            Length,
            [Description("Released Between")]
            ReleasedBetween,
            [Description("Release Status")]
            ReleaseStatus,
            Blacklisted,
            Voted,
            [Description("By Favorite Producer")]
            ByFavoriteProducer,
            [Description("WishlistStatus")]
            WishlistStatus,
            [Description("Userlist Status")]
            UserlistStatus,
            Language,
            [Description("Original Language")]
            OriginalLanguage,
            Tags,
            Traits
#pragma warning restore 1591
        }

        public Func<ListedVN, bool> GetFunction()
        {
            switch (Type)
            {
                case FilterType.Length:
                    return vn => vn.Length == (LengthFilter)Value != Exclude;
                case FilterType.ReleasedBetween:
                    //TODO
                    break;
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
                    //TODO
                    break;
                case FilterType.Traits:
                    //TODO
                    break;
            }
            return vn => true;
        }
    }
}