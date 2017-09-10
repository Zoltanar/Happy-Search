using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using static Happy_Apps_Core.StaticHelpers;

namespace Happy_Apps_Core
{
    /// <summary>
    /// Object for displaying Visual Novel in Object List View.
    /// </summary>
    public class ListedVN
    {
        private static readonly Dictionary<int, LengthFilter> LengthMap = new Dictionary<int, LengthFilter>
        {
            {-1, LengthFilter.NA},
            {0, LengthFilter.NA},
            {1, LengthFilter.UnderTwoHours},
            {2, LengthFilter.TwoToTenHours},
            {3, LengthFilter.TenToThirtyHours},
            {4, LengthFilter.ThirtyToFiftyHours},
            {5, LengthFilter.OverFiftyHours}
        };
        
        /// <summary>
        /// Get ListedVN from DataReader.
        /// </summary>
        public ListedVN(IDataRecord reader)
        {
            var relDate = reader["RelDate"].ToString();
            var length = DbInt(reader["LengthTime"]);
            var vote = DbInt(reader["Vote"]);

            if (relDate.Equals("") || relDate.Equals("tba")) relDate = "N/A";
            ULStatus = (UserlistStatus)DbInt(reader["ULStatus"]);
            WLStatus = (WishlistStatus)DbInt(reader["WLStatus"]);
            if (vote != -1) Vote = (double)vote / 10;
            Title = reader["Title"].ToString();
            KanjiTitle = reader["KanjiTitle"].ToString();
            RelDate = relDate;
            DateForSorting = StringToDate(relDate);
            Producer = reader["Name"].ToString();
            Length = LengthMap[length];
            ULAdded = DateTimeOffset.FromUnixTimeSeconds(DbInt(reader["ULAdded"])).UtcDateTime;
            ULNote = reader["ULNote"].ToString();
            WLAdded = DateTimeOffset.FromUnixTimeSeconds(DbInt(reader["WLAdded"])).UtcDateTime;
            VoteAdded = DateTimeOffset.FromUnixTimeSeconds(DbInt(reader["VoteAdded"])).UtcDateTime;
            List<VNItem.TagItem> tagList = StringToTags(reader["Tags"].ToString());
            foreach (var tag in tagList) tag.SetCategory(DumpFiles.PlainTags);
            TagList = tagList;
            VNID = DbInt(reader["VNID"]);
            UpdatedDate = DaysSince(DbDateTime(reader["DateUpdated"]));
            ImageURL = reader["ImageURL"].ToString();
            ImageNSFW = GetImageStatus(reader["ImageNSFW"]);
            Description = reader["Description"].ToString();
            Popularity = DbDouble(reader["Popularity"]);
            Rating = DbDouble(reader["Rating"]);
            VoteCount = DbInt(reader["VoteCount"]);
            Relations = reader["Relations"].ToString();
            Screens = reader["Screens"].ToString();
            Anime = reader["Anime"].ToString();
            Aliases = reader["Aliases"].ToString();
            Languages = JsonConvert.DeserializeObject<VNLanguages>(reader["Languages"].ToString());
            DateFullyUpdated = DaysSince(DbDateTime(reader["DateFullyUpdated"]));
        }

        // ReSharper disable once UnusedMember.Global
        public ListedVN() { }
        
        /// <summary>
        /// Returns true if a title was last updated over x days ago.
        /// </summary>
        /// <param name="days">Days since last update</param>
        /// <param name="fullyUpdated">Use days since full update</param>
        /// <returns></returns>
        public bool LastUpdatedOverDaysAgo(int days, bool fullyUpdated = false)
        {
            var dateToUse = fullyUpdated ? DateFullyUpdated : UpdatedDate;
            if (dateToUse == -1) return true;
            return dateToUse > days;
        }

        /// <summary>
        /// Days since all fields were updated
        /// </summary>
        public int DateFullyUpdated { get; }

        /// <summary>
        /// List of Tags in this VN.
        /// </summary>
        public List<VNItem.TagItem> TagList { get; }

        /// <summary>
        /// Gets characters involved in VN.
        /// </summary>
        /// <param name="characterList">List of all characters</param>
        /// <returns>Array of Characters</returns>
        public CharacterItem[] GetCharacters(IEnumerable<CharacterItem> characterList)
        {
            return characterList.Where(x => x.CharacterIsInVN(VNID)).ToArray();
        }

        /// <summary>
        /// VN title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// VN kanji title
        /// </summary>
        public string KanjiTitle { get; }

        /// <summary>
        /// VN's first non-trial release date
        /// </summary>
        public string RelDate { get; }

        /// <summary>
        /// Date used for sorting rather than display string.
        /// </summary>
        public DateTime DateForSorting { get; }

        /// <summary>
        /// VN producer
        /// </summary>
        public string Producer { get; }

        /// <summary>
        /// VN length
        /// </summary>
        public LengthFilter Length { get; }

        /// <summary>
        /// String for Length
        /// </summary>
        public string LengthString => Length.GetDescription();

        /// <summary>
        /// User's userlist status of VN
        /// </summary>
        public UserlistStatus ULStatus { get; }

        /// <summary>
        /// Date of ULStatus change
        /// </summary>
        public DateTime ULAdded { get; }

        /// <summary>
        /// User's note
        /// </summary>
        public string ULNote { get; }

        /// <summary>
        /// User's wishlist priority of VN
        /// -1: null, 0: high, 1: medium, 2: low, 3: blacklist
        /// </summary>
        public WishlistStatus WLStatus { get; }

        /// <summary>
        /// Date of WLStatus change
        /// </summary>
        public DateTime WLAdded { get; }

        /// <summary>
        /// User's Vote
        /// </summary>
        public double Vote { get; }

        /// <summary>
        /// Date of Vote change
        /// </summary>
        public DateTime VoteAdded { get; }

        /// <summary>
        /// Popularity of VN, percentage of most popular VN
        /// </summary>
        public double Popularity { get; set; }

        /// <summary>
        /// Bayesian rating of VN, 1-10
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// Number of votes cast on VN
        /// </summary>
        public int VoteCount { get; }

        /// <summary>
        /// VN's ID
        /// </summary>
        public int VNID { get; }

        /// <summary>
        /// Days since last tags/stats/traits update
        /// </summary>
        public int UpdatedDate { get; set; }

        /// <summary>
        /// URL of VN's cover image
        /// </summary>
        public string ImageURL { get; set; }

        /// <summary>
        /// Is VN's cover NSFW?
        /// </summary>
        public bool ImageNSFW { get; set; }

        /// <summary>
        /// VN description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// JSON Array string containing List of Relation Items
        /// </summary>
        public string Relations { get; }

        /// <summary>
        /// JSON Array string containing List of Screenshot Items
        /// </summary>
        public string Screens { get; }

        /// <summary>
        /// JSON Array string containing List of Anime Items
        /// </summary>
        public string Anime { get; }

        /// <summary>
        /// Newline separated string of aliases
        /// </summary>
        public string Aliases { get; }

        /// <summary>
        /// Language of producer
        /// </summary>
        public VNLanguages Languages { get; }

        /// <summary>
        /// Return unreleased status of vn.
        /// </summary>
        public UnreleasedFilter Unreleased
        {
            get
            {
                if (DateForSorting == DateTime.MaxValue) return UnreleasedFilter.WithoutReleaseDate;
                if (DateForSorting > DateTime.Today) return UnreleasedFilter.WithReleaseDate;
                return UnreleasedFilter.Released;
            }
        }

        /// <summary>
        /// Gets blacklisted status of vn.
        /// </summary>
        public bool Blacklisted => WLStatus == WishlistStatus.Blacklist;

        /// <summary>
        /// Gets voted status of vn.
        /// </summary>
        public bool Voted => Vote >= 1;
        
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"ID={VNID} Title={Title}";

        /// <summary>
        /// Get VN's User-related status as a string.
        /// </summary>
        /// <returns>User-related status</returns>
        public string UserRelatedStatus()
        {
            string[] parts = { "", "", "" };
            if (ULStatus > UserlistStatus.None)
            {
                parts[0] = "Userlist: ";
                parts[1] = ULStatus.ToString();
            }
            else if (WLStatus > WishlistStatus.None)
            {
                parts[0] = "Wishlist: ";
                parts[1] = WLStatus.ToString();
            }
            if (Vote > 0) parts[2] = $" (Vote: {Vote:0.#})";
            return string.Join(" ", parts);
        }

        /// <summary>
        /// Get VN's rating, votecount and popularity as a string.
        /// </summary>
        /// <returns>Rating, votecount and popularity</returns>
        public string RatingAndVoteCount()
        {
            return VoteCount == 0 ? "No votes yet." : $"{Rating:0.00} ({VoteCount} votes)";
        }

        /// <summary>
        /// Checks if title was released between two dates, the recent date is inclusive.
        /// Make sure to enter arguments in correct order.
        /// </summary>
        /// <param name="oldDate">Date furthest from the present</param>
        /// <param name="recentDate">Date closest to the present</param>
        /// <returns></returns>
        public bool ReleasedBetween(DateTime oldDate, DateTime recentDate)
        {
            return DateForSorting > oldDate && DateForSorting <= recentDate;
        }

        /// <summary>
        /// Checks if VN is in specified user-defined group.
        /// </summary>
        /// <param name="groupName">User-defined name of group</param>
        /// <returns>Whether VN is in the specified group</returns>
        public bool IsInGroup(string groupName)
        {
            var itemNotes = GetCustomItemNotes();
            return itemNotes.Groups.Contains(groupName);
        }

        /// <summary>
        /// Get CustomItemNotes containing note and list of groups that vn is in.
        /// </summary>
        public VNItem.CustomItemNotes GetCustomItemNotes()
        {
            //
            if (ULNote.Equals("")) return new VNItem.CustomItemNotes("", new List<string>());
            if (!ULNote.StartsWith("Notes: "))
            {
                //escape ulnote
                string fixedNote = ULNote.Replace("|", "(sep)");
                fixedNote = fixedNote.Replace("Groups: ", "groups: ");
                return new VNItem.CustomItemNotes(fixedNote, new List<string>());
            }
            int endOfNotes = ULNote.IndexOf("|", StringComparison.InvariantCulture);
            string notes;
            string groupsString;
            try
            {
                notes = ULNote.Substring(7, endOfNotes - 7);
                groupsString = ULNote.Substring(endOfNotes + 1 + 8);
            }
            catch (ArgumentOutOfRangeException)
            {
                notes = "";
                groupsString = "";
            }
            List<string> groups = groupsString.Equals("") ? new List<string>() : groupsString.Split(',').ToList();
            return new VNItem.CustomItemNotes(notes, groups);
        }

        /// <summary>
        /// Check if title was released in specified year.
        /// </summary>
        /// <param name="year">Year of release</param>
        public bool ReleasedInYear(int year)
        {
            return DateForSorting.Year == year;
        }

        /// <summary>
        /// Get location of cover image in system (not online)
        /// </summary>
        public string GetImageLocation() => $"{VNImagesFolder}{VNID}{Path.GetExtension(ImageURL)}";

        /// <summary>
        /// Returns whether vn is by a favorite producer.
        /// </summary>
        /// <param name="favoriteProducers">List of favorite producers.</param>
        public bool ByFavoriteProducer(IEnumerable<ListedProducer> favoriteProducers)
        {
            return favoriteProducers.FirstOrDefault(fp => fp.Name == Producer) != null;
        }

        public bool HasLanguage(string value)
        {
            return Languages?.All.Contains(value) ?? false;
        }

        public bool HasOriginalLanguage(string value)
        {
            return Languages?.Originals.Contains(value) ?? false;
        }
    }
}