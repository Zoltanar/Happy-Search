using System;
using BrightIdeasSoftware;

namespace Happy_Search
{
    /// <summary>
    /// Object for displaying Visual Novel in Object List View.
    /// </summary>
    public class ListedVN
    {
        internal static string[] StatusUL = {"Unknown", "Playing", "Finished", "Stalled", "Dropped"};
        internal static string[] PriorityWL = {"High", "Medium", "Low", "Blacklist"};

        internal static string[] LengthTime =
        {
            "", "Very short (< 2 hours)", "Short (2 - 10 hours)",
            "Medium (10 - 30 hours)", "Long (30 - 50 hours)", "Very long(> 50 hours)"
        };

        /// <summary>
        /// Constructor for ListedVN.
        /// </summary>
        /// <param name="title">VN title</param>
        /// <param name="kanjiTitle">VN kanji title</param>
        /// <param name="reldate">VN's first non-trial release date</param>
        /// <param name="producer">VN producer</param>
        /// <param name="length">VN length</param>
        /// <param name="ulstatus">User's userlist status of VN</param>
        /// <param name="uladded">Date of ULStatus change</param>
        /// <param name="ulnote">User's note</param>
        /// <param name="wlstatus">User's wishlist priority of VN</param>
        /// <param name="wladded">Date of WLStatus change</param>
        /// <param name="vote">User's Vote</param>
        /// <param name="voteadded">Date of Vote change</param>
        /// <param name="tags">VN's tags (in JSON array)</param>
        /// <param name="vnid">VN's ID</param>
        /// <param name="updatedDate">Date of last VN update</param>
        /// <param name="imageURL">URL of VN's cover image</param>
        /// <param name="imageNSFW">Is VN's cover NSFW?</param>
        /// <param name="description">VN description</param>
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

        /// <summary>
        /// VN title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// VN kanji title
        /// </summary>
        public string KanjiTitle { get; set; }
        /// <summary>
        /// VN's first non-trial release date
        /// </summary>
        public string RelDate { get; set; }
        /// <summary>
        /// VN producer
        /// </summary>
        public string Producer { get; set; }
        /// <summary>
        /// VN length
        /// </summary>
        public string Length { get; set; }
        /// <summary>
        /// User's userlist status of VN
        /// </summary>
        public string ULStatus { get; set; }
        /// <summary>
        /// Date of ULStatus change
        /// </summary>
        public DateTime ULAdded { get; set; }
        /// <summary>
        /// User's note
        /// </summary>
        public string ULNote { get; set; }
        /// <summary>
        /// User's wishlist priority of VN
        /// </summary>
        public string WLStatus { get; set; }
        /// <summary>
        /// Date of WLStatus change
        /// </summary>
        public DateTime WLAdded { get; set; }
        /// <summary>
        /// User's Vote
        /// </summary>
        public double Vote { get; set; }
        /// <summary>
        /// Date of Vote change
        /// </summary>
        public DateTime VoteAdded { get; set; }
        /// <summary>
        /// VN's ID
        /// </summary>
        public int VNID { get; set; }
        /// <summary>
        /// VN's tags (in JSON array)
        /// </summary>
        [OLVIgnore]
        public string Tags { get; set; }
        /// <summary>
        /// Date of last VN update
        /// </summary>
        public int UpdatedDate { get; set; }
        /// <summary>
        /// URL of VN's cover image
        /// </summary>
        [OLVIgnore]
        public string ImageURL { get; set; }
        /// <summary>
        /// Is VN's cover NSFW?
        /// </summary>
        [OLVIgnore]
        public bool ImageNSFW { get; set; }
        /// <summary>
        /// VN description
        /// </summary>
        [OLVIgnore]
        public string Description { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"ID={VNID}\t\tTitle={Title}";
    }

    /// <summary>
    /// Object for Favorite Producers in Object List View.
    /// </summary>
    public class ListedProducer
    {
        /// <summary>
        /// Constructor for ListedProducer.
        /// </summary>
        /// <param name="name">Producer Name</param>
        /// <param name="numberOfTitles">Number of Producer's titles</param>
        /// <param name="loaded">Has producer been loaded? (Yes/No)</param>
        /// <param name="updated">Date of last update to Producer</param>
        /// <param name="id">Producer ID</param>
        public ListedProducer(string name, int numberOfTitles, string loaded, DateTime updated, int id)
        {
            Name = name;
            NumberOfTitles = numberOfTitles;
            Loaded = loaded;
            Updated = DaysSince(updated);
            ID = id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Producer Name</param>
        /// <param name="numberOfTitles">Number of Producer's titles</param>
        /// <param name="loaded">Has producer been loaded? (Yes/No)</param>
        /// <param name="updated">Date of last update to Producer</param>
        /// <param name="id">Producer ID</param>
        /// <param name="userAverageVote">User's average vote on Producer titles. (Only titles with votes)</param>
        /// <param name="userDropRate">User's average drop rate on Producer titles. (Dropped / (Finished+Dropped)</param>
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

        /// <summary>
        /// Producer Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Number of Producer's titles
        /// </summary>
        public int NumberOfTitles { get; set; }
        /// <summary>
        /// Has producer been loaded? (Yes/No)
        /// </summary>
        public string Loaded { get; set; }
        /// <summary>
        /// Date of last update to Producer
        /// </summary>
        public int Updated { get; set; }
        /// <summary>
        /// Producer ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// User's average vote on Producer titles. (Only titles with votes)
        /// </summary>
        public double UserAverageVote { get; set; }
        /// <summary>
        /// User's average drop rate on Producer titles. (Dropped / (Finished+Dropped)
        /// </summary>
        public int UserDropRate { get; set; }

        /// <summary>
        /// Get Days passed since date of last update.
        /// </summary>
        /// <param name="updatedDate">Date of last update</param>
        /// <returns>Number of days since last update</returns>
        private static int DaysSince(DateTime updatedDate)
        {
            if (updatedDate == DateTime.MinValue) return -1;
            var days = (DateTime.Today - updatedDate).Days;
            return days;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"ID={ID} \t\tName={Name}";
    }

    /// <summary>
    /// Object for displaying producer search results in OLV.
    /// </summary>
    public class ListedSearchedProducer
    {
        /// <summary>
        /// Constructor for Searched Producer.
        /// </summary>
        /// <param name="name">Name of producer</param>
        /// <param name="inList">Is producer already in favorite producers list? (Yes/No)</param>
        /// <param name="id">ID of producer</param>
        /// <param name="finishedTitles">Number of producer's titles finished by user</param>
        /// <param name="urtTitles">Number of producer's titles related to user</param>
        public ListedSearchedProducer(string name, string inList, int id, int finishedTitles, int urtTitles)
        {
            Name = name;
            InList = inList;
            ID = id;
            FinishedTitles = finishedTitles;
            URTTitles = urtTitles;

        }

        /// <summary>
        /// Name of producer
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Is producer already in favorite producers list? (Yes/No)
        /// </summary>
        public string InList { get; set; }
        /// <summary>
        /// ID of producer
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Number of producer's titles finished by user
        /// </summary>
        public int FinishedTitles { get; set; }
        /// <summary>
        /// Number of producer's titles related to user
        /// </summary>
        public int URTTitles { get; set; }
    }
}