using System;
using System.ComponentModel;
using System.Linq;

namespace Happy_Apps_Core
{
#pragma warning disable 1591
    // ReSharper disable UnusedMember.Global


    [Flags]
    public enum UnreleasedFilter
    {
        WithoutReleaseDate = 1,
        WithReleaseDate = 2,
        Released = 4,
        AllUnreleased = WithReleaseDate | WithoutReleaseDate,
        ReleasedOrWithDate = WithReleaseDate | Released
    }

    [Flags]
    public enum WishlistFilter
    {
        NA = 1,
        High = 2,
        Medium = 4,
        Low = 8,
        Any = High | Medium | Low
    }

    [Flags]
    public enum UserlistFilter
    {
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
    public struct DateRange
    {
        public static DateRange Default()
        {
            return new DateRange(new DateTime(DateTime.Now.Year, 1, 1), new DateTime(DateTime.Now.Year + 1, 1, 1));
        }
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
        public CustomTraitFilter(string name, DumpFiles.WrittenTrait[] filters)
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
        public DumpFiles.WrittenTrait[] Filters { get; set; }

        /// <summary>
        ///     Date of last update to custom filter
        /// </summary>
        public DateTime Updated { get; set; }

        public override string ToString() => Name;
    }

    public enum RefreshType { None, UserChanged, NamedFilter }
#pragma warning restore 1591
}
