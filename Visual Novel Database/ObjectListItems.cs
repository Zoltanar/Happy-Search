using System;
using BrightIdeasSoftware;

namespace Happy_Search
{
    public class ListedVN
    {
        internal static string[] StatusUL = {"Unknown", "Playing", "Finished", "Stalled", "Dropped"};
        internal static string[] PriorityWL = {"High", "Medium", "Low", "Blacklist"};

        internal static string[] LengthTime =
        {
            "", "Very short (< 2 hours)", "Short (2 - 10 hours)",
            "Medium (10 - 30 hours)", "Long (30 - 50 hours)", "Very long(> 50 hours)"
        };

        public ListedVN(string title, string kanjiTitle, string reldate, string producer, int length, int ulstatus,
            int uladded, string ulnote,
            int wlstatus, int wladded, int vote, int voteadded, string tags, int vnid, DateTime updatedDate,
            string imageURL, bool imageNSFW, string description)
        {
            if (reldate.Equals("") || reldate.Equals("tba")) reldate = "N/A";
            ULStatus = ulstatus != -1 ? StatusUL[ulstatus] : "";
            WLStatus = wlstatus != -1 ? PriorityWL[wlstatus] : "";
            if (vote != -1) Vote = (double) vote/10;
            Title = title;
            KanjiTitle = kanjiTitle;
            RelDate = reldate;
            Producer = producer;
            Length = LengthTime[length];
            ULAdded = DateTimeOffset.FromUnixTimeSeconds(uladded).UtcDateTime;
            ULNote = ulnote;
            WLAdded = DateTimeOffset.FromUnixTimeSeconds(wladded).UtcDateTime;
            VoteAdded = DateTimeOffset.FromUnixTimeSeconds(voteadded).UtcDateTime;
            Tags = tags;
            VNID = vnid;
            UpdatedDate = FormMain.DaysSince(updatedDate);
            ImageURL = imageURL;
            ImageNSFW = imageNSFW;
            Description = description;
        }

        //For UserRelated Only
        public ListedVN(int ulstatus, int uladded, string ulnote, int wlstatus, int wladded, int vote, int voteadded,
            int vnid)
        {
            ULStatus = ulstatus != -1 ? StatusUL[ulstatus] : "";
            WLStatus = wlstatus != -1 ? PriorityWL[wlstatus] : "";
            if (vote != -1) Vote = (double) vote/10;
            ULAdded = DateTimeOffset.FromUnixTimeSeconds(uladded).UtcDateTime;
            ULNote = ulnote;
            WLAdded = DateTimeOffset.FromUnixTimeSeconds(wladded).UtcDateTime;
            VoteAdded = DateTimeOffset.FromUnixTimeSeconds(voteadded).UtcDateTime;
            VNID = vnid;
        }

        public string Title { get; set; }
        public string KanjiTitle { get; set; }
        public string RelDate { get; set; }
        public string Producer { get; set; }
        public string Length { get; set; }
        public string ULStatus { get; set; }
        public DateTime ULAdded { get; set; }
        public string ULNote { get; set; }
        public string WLStatus { get; set; }
        public DateTime WLAdded { get; set; }
        public double Vote { get; set; }
        public DateTime VoteAdded { get; set; }

        [OLVIgnore]
        public string Tags { get; set; }

        public int VNID { get; set; }
        public int UpdatedDate { get; set; }

        [OLVIgnore]
        public string ImageURL { get; set; }

        [OLVIgnore]
        public bool ImageNSFW { get; set; }

        [OLVIgnore]
        public string Description { get; set; }


        public override string ToString() => $"ID={VNID}\t\tTitle={Title}";
    }

    public class ListedProducer
    {
        public ListedProducer(string name, int numberOfTitles, string loaded, DateTime updated, int id)
        {
            Name = name;
            NumberOfTitles = numberOfTitles;
            Loaded = loaded;
            Updated = DaysSince(updated);
            ID = id;
        }

        public ListedProducer(string name, int numberOfTitles, string loaded, DateTime updated, int id,
            double userAverageVote, int userDropRate)
        {
            Name = name;
            NumberOfTitles = numberOfTitles;
            Loaded = loaded;
            Updated = DaysSince(updated);
            ID = id;
            UserAverageVote = Math.Round(userAverageVote, 2);
            UserDropRate = userDropRate;
        }

        public string Name { get; set; }
        public int NumberOfTitles { get; set; }
        public string Loaded { get; set; }
        public int Updated { get; set; }
        public int ID { get; set; }
        public double UserAverageVote { get; set; }
        public int UserDropRate { get; set; }


        private static int DaysSince(DateTime updatedDate)
        {
            if (updatedDate == DateTime.MinValue) return -1;
            var days = (DateTime.Today - updatedDate).Days;
            return days;
        }

        public override string ToString() => $"ID={ID}\t\tName={Name}";
    }

    public class ListedSearchedProducer
    {
        public ListedSearchedProducer(string name, string inList, int id)
        {
            Name = name;
            InList = inList;
            ID = id;
        }

        public string Name { get; set; }
        public string InList { get; set; }
        public int ID { get; set; }
    }
}