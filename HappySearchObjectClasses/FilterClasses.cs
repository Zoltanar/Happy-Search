using System;
using System.ComponentModel;
using System.Linq;

namespace Happy_Apps_Core
{
#pragma warning disable 1591
    // ReSharper disable UnusedMember.Global


    [Flags]
    public enum UnreleasedFilter : long
    {
        [Description("Unreleased without date")]
        WithoutReleaseDate = 1,
        [Description("Unreleased with date")]
        WithReleaseDate = 2,
        [Description("Released")]
        Released = 3
    }
    
    public enum LengthFilter : long
    {
        [Description("Not Available")]
        NA = 0,
        [Description("<2 Hours")]
        UnderTwoHours = 1,
        [Description("2-10 Hours")]
        TwoToTenHours = 2,
        [Description("10-30 Hours")]
        TenToThirtyHours = 3,
        [Description("30-50 Hours")]
        ThirtyToFiftyHours = 4,
        [Description(">50 Hours")]
        OverFiftyHours = 5,
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
        public TagFilter(int id, string name, int[] children)
        {
            ID = id;
            Name = name;
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
    public enum RefreshType { None, UserChanged, NamedFilter }
#pragma warning restore 1591
}
