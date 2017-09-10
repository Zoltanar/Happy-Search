using System;
using System.ComponentModel;

namespace Happy_Apps_Core
{


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

    public enum RefreshType { None, UserChanged, NamedFilter }
#pragma warning restore 1591
}
