using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Happy_Search.Properties;
using Newtonsoft.Json;
using static Happy_Search.StaticHelpers;

namespace Happy_Search
{

    /// <summary>
    /// Contains original and other languages available for vn.
    /// </summary>
    [Serializable]
    public class VNLanguages
    {
        /// <summary>
        /// Languages for original release
        /// </summary>
        public string[] Originals { get; set; }
        /// <summary>
        /// Languages for other releases
        /// </summary>
        public string[] Others { get; set; }

        /// <summary>
        /// Languages for all releases
        /// </summary>
        public IEnumerable<string> All => Originals.Concat(Others);

        /// <summary>
        /// Empty Constructor for serialization
        /// </summary>
        public VNLanguages() { }

        /// <summary>
        /// Constructor for vn languages.
        /// </summary>
        /// <param name="originals">Languages for original release</param>
        /// <param name="all">Languages for all releases</param>
        public VNLanguages(string[] originals, string[] all)
        {
            Originals = originals;
            Others = all.Except(originals).ToArray();
        }

        /// <summary>
        /// Displays a json-serialized string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

#pragma warning disable 1591
    /// <summary>
    /// Map Wishlist status numbers to words.
    /// </summary>
    public enum WishlistStatus
    {
        None = -1,
        High = 0,
        Medium = 1,
        Low = 2,
        Blacklist = 3
    }

    public enum UserlistStatus
    {
        None = -1,
        Unknown = 0,
        Playing = 1,
        Finished = 2,
        Stalled = 3,
        Dropped = 4
    }
#pragma warning restore 1591

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
            var length = DbHelper.DbInt(reader["LengthTime"]);
            var vote = DbHelper.DbInt(reader["Vote"]);

            if (relDate.Equals("") || relDate.Equals("tba")) relDate = "N/A";
            ULStatus = (UserlistStatus)DbHelper.DbInt(reader["ULStatus"]);
            WLStatus = (WishlistStatus)DbHelper.DbInt(reader["WLStatus"]);
            if (vote != -1) Vote = (double)vote / 10;
            Title = reader["Title"].ToString();
            KanjiTitle = reader["KanjiTitle"].ToString();
            RelDate = relDate;
            DateForSorting = StringToDate(relDate);
            Producer = reader["Name"].ToString();
            Length = length != -1 ? LengthMap[length] : LengthFilter.NA;
            ULAdded = DateTimeOffset.FromUnixTimeSeconds(DbHelper.DbInt(reader["ULAdded"])).UtcDateTime;
            ULNote = reader["ULNote"].ToString();
            WLAdded = DateTimeOffset.FromUnixTimeSeconds(DbHelper.DbInt(reader["WLAdded"])).UtcDateTime;
            VoteAdded = DateTimeOffset.FromUnixTimeSeconds(DbHelper.DbInt(reader["VoteAdded"])).UtcDateTime;
            var tagList = StringToTags(reader["Tags"].ToString());
            foreach (var tag in tagList) tag.SetCategory(FormMain.PlainTags);
            TagList = tagList;
            VNID = DbHelper.DbInt(reader["VNID"]);
            UpdatedDate = DaysSince(DbHelper.DbDateTime(reader["DateUpdated"]));
            ImageURL = reader["ImageURL"].ToString();
            ImageNSFW = DbHelper.GetImageStatus(reader["ImageNSFW"]);
            Description = reader["Description"].ToString();
            Popularity = DbHelper.DbDouble(reader["Popularity"]);
            Rating = DbHelper.DbDouble(reader["Rating"]);
            VoteCount = DbHelper.DbInt(reader["VoteCount"]);
            Relations = reader["Relations"].ToString();
            Screens = reader["Screens"].ToString();
            Anime = reader["Anime"].ToString();
            Aliases = reader["Aliases"].ToString();
            Languages = JsonConvert.DeserializeObject<VNLanguages>(reader["Languages"].ToString());
            DateFullyUpdated = DaysSince(DbHelper.DbDateTime(reader["DateFullyUpdated"]));
        }
        
        /// <summary>
        /// Returns whether vn is by a favorite producer.
        /// </summary>
        /// <param name="favoriteProducers">List of favorite producers.</param>
        internal bool ByFavoriteProducer(IEnumerable<ListedProducer> favoriteProducers)
        {
            return favoriteProducers.FirstOrDefault(fp => fp.Name == Producer) != null;
        }

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
        public List<TagItem> TagList { get; }

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

        /// <summary>
        /// JSON Array string containing List of Relation Items
        /// </summary>
        [OLVIgnore]
        public string Relations { get; }

        /// <summary>
        /// JSON Array string containing List of Screenshot Items
        /// </summary>
        [OLVIgnore]
        public string Screens { get; }

        /// <summary>
        /// JSON Array string containing List of Anime Items
        /// </summary>
        [OLVIgnore]
        public string Anime { get; }

        /// <summary>
        /// Newline separated string of aliases
        /// </summary>
        [OLVIgnore]
        public string Aliases { get; }

        /// <summary>
        /// Language of producer
        /// </summary>
        [OLVIgnore]
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

        /// <summary>
        /// Gets wishlist status of vn.
        /// </summary>
        public WishlistFilter Wishlist
        {
            get
            {
                switch (WLStatus)
                {
                    case WishlistStatus.High:
                        return WishlistFilter.High;
                    case WishlistStatus.Medium:
                        return WishlistFilter.Medium;
                    case WishlistStatus.Low:
                        return WishlistFilter.Low;
                    default:
                        return WishlistFilter.NA;
                }
            }
        }

        /// <summary>
        /// Returns Userlist Status of vn.
        /// </summary>
        public UserlistFilter Userlist
        {
            get
            {
                switch (ULStatus)
                {
                    case UserlistStatus.Unknown:
                        return UserlistFilter.Unknown;
                    case UserlistStatus.Playing:
                        return UserlistFilter.Playing;
                    case UserlistStatus.Finished:
                        return UserlistFilter.Finished;
                    case UserlistStatus.Stalled:
                        return UserlistFilter.Stalled;
                    case UserlistStatus.Dropped:
                        return UserlistFilter.Dropped;
                    default:
                        return UserlistFilter.NA;
                }
            }
        }



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
        public CustomItemNotes GetCustomItemNotes()
        {
            //
            if (ULNote.Equals("")) return new CustomItemNotes("", new List<string>());
            if (!ULNote.StartsWith("Notes: "))
            {
                //escape ulnote
                string fixedNote = ULNote.Replace("|", "(sep)");
                fixedNote = fixedNote.Replace("Groups: ", "groups: ");
                return new CustomItemNotes(fixedNote, new List<string>());
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
            return new CustomItemNotes(notes, groups);
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
    }

    /// <summary>
    /// Object for Favorite Producers in Object List View.
    /// </summary>
    public class ListedProducer
    {
        /// <summary>
        /// Constructor for ListedProducer, not favorite producers.
        /// </summary>
        /// <param name="name">Producer Name</param>
        /// <param name="numberOfTitles">Number of Producer's titles</param>
        /// <param name="updated">Date of last update to Producer</param>
        /// <param name="id">Producer ID</param>
        /// <param name="language">Language of producer</param>
        public ListedProducer(string name, int numberOfTitles, DateTime updated, int id, string language)
        {
            Name = name;
            NumberOfTitles = numberOfTitles;
            Updated = DaysSince(updated);
            ID = id;
            Language = language;
        }

        /// <summary>
        /// Constructor for ListedProducer for favorite producers.
        /// </summary>
        /// <param name="name">Producer Name</param>
        /// <param name="numberOfTitles">Number of Producer's titles</param>
        /// <param name="updated">Date of last update to Producer</param>
        /// <param name="id">Producer ID</param>
        /// <param name="language">Language of producer</param>
        /// <param name="userAverageVote">User's average vote on Producer titles. (Only titles with votes)</param>
        /// <param name="userDropRate">User's average drop rate on Producer titles. (Dropped / (Finished+Dropped)</param>
        public ListedProducer(string name, int numberOfTitles, DateTime updated, int id, string language,
            double userAverageVote, int userDropRate)
        {
            Name = name;
            NumberOfTitles = numberOfTitles;
            Updated = DaysSince(updated);
            ID = id;
            Language = language;
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
        /// Date of last update to Producer
        /// </summary>
        public int Updated { get; set; }
        /// <summary>
        /// Producer ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Language of Producer
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// User's average vote on Producer titles. (Only titles with votes)
        /// </summary>
        public double UserAverageVote { get; set; }
        /// <summary>
        /// User's average drop rate on Producer titles. (Dropped / (Finished+Dropped)
        /// </summary>
        public int UserDropRate { get; set; }
        /// <summary>
        /// Bayesian average score of votes by all users.
        /// </summary>
        public double GeneralRating { get; set; }


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
        public override string ToString() => $"ID={ID} Name={Name}";
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
        /// <param name="language">Language of producer</param>
        /// <param name="finishedTitles">Number of producer's titles finished by user</param>
        /// <param name="urtTitles">Number of producer's titles related to user</param>
        public ListedSearchedProducer(string name, string inList, int id, string language, int finishedTitles, int urtTitles)
        {
            Name = name;
            InList = inList;
            ID = id;
            Language = language;
            FinishedTitles = finishedTitles;
            URTTitles = urtTitles;

        }

        /// <summary>
        /// Convert ListedSearchedProducer to ListedProducer.
        /// </summary>
        /// <param name="searchedProducer">Producer to be converted</param>
        /// <returns>ListedProducer with name and ID of ListedSearchedProducer</returns>
        public static explicit operator ListedProducer(ListedSearchedProducer searchedProducer)
        {
            return new ListedProducer(searchedProducer.Name, -1, DateTime.MinValue, searchedProducer.ID, searchedProducer.Language);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"ID={ID} Name={Name}";

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
        /// Language of Producer
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// Number of producer's titles finished by user
        /// </summary>
        public int FinishedTitles { get; set; }
        /// <summary>
        /// Number of producer's titles related to user
        /// </summary>
        public int URTTitles { get; set; }
    }


    /// <summary>
    ///     Class for drawing individual tiles in ObjectListView
    /// </summary>
    public class VNTileRenderer : AbstractRenderer
    {
        private const int LinesOfTextAbovePicture = 1;
        private const int LinesOfTextBelowPicture = 3;
        private readonly Pen _borderPen = new Pen(Color.FromArgb(0x33, 0x33, 0x33));
        private readonly Pen _selectedBorderPen = Pens.Gold;
        private static readonly Brush TextBrush = new SolidBrush(Color.FromArgb(0x22, 0x22, 0x22));
        private static readonly Font BoldFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
        private static readonly Font NormalFont = new Font("Microsoft Sans Serif", 8.25f);

        /// <summary>
        ///     Render the whole item within an ObjectListView. This is only used in non-Details views.
        /// </summary>
        /// <param name="e">The event</param>
        /// <param name="g">A Graphics for rendering</param>
        /// <param name="itemBounds">The bounds of the item</param>
        /// <param name="rowObject">The model object to be drawn</param>
        /// <returns>Return true to indicate that the event was handled and no further processing is needed.</returns>
        public override bool RenderItem(DrawListViewItemEventArgs e, Graphics g, Rectangle itemBounds,
            object rowObject)
        {
            // If we're in any other view than Tile, return false to say that we haven't done
            // the renderering and the default process should do it's stuff
            var olv = e.Item.ListView as ObjectListView;
            if (olv == null || olv.View != View.Tile)
                return false;

            // Use buffered graphics to kill flickers
            var buffered = BufferedGraphicsManager.Current.Allocate(g, itemBounds);
            g = buffered.Graphics;
            g.Clear(olv.BackColor);
            g.SmoothingMode = ObjectListView.SmoothingMode;
            g.TextRenderingHint = ObjectListView.TextRenderingHint;
            DrawVNTile(g, itemBounds, rowObject, olv, e.Item.Selected);
            // Finally render the buffered graphics
            buffered.Render();
            buffered.Dispose();

            // Return true to say that we've handled the drawing
            return true;
        }

        /// <summary>
        ///     Draw Visual Novel Tile, displayed in Visual Novels Object List View.
        /// </summary>
        /// <param name="g">A Graphics for rendering</param>
        /// <param name="itemBounds">The bounds of the item</param>
        /// <param name="rowObject">The model object to be drawn</param>
        /// <param name="olv">OLV where tile is drawn</param>
        /// <param name="isSelected">Whether tile is selected</param>
        public void DrawVNTile(Graphics g, Rectangle itemBounds, object rowObject, ObjectListView olv, bool isSelected)
        {
            var backBrush = DefaultTileBrush;
            //tile size 230,375
            //image 230-spacing, 300-spacing
            const int spacing = 8;
            var textHeight = g.MeasureString("Wj", NormalFont).Height;
            // Allow a border around the card
            itemBounds.Inflate(-2, -2);
            var vn = rowObject as ListedVN;
            // Draw card background
            const int rounding = 20;
            var path = GetRoundedRect(itemBounds, rounding);
            switch (vn?.WLStatus)
            {
                case WishlistStatus.High:
                    backBrush = WLHighBrush;
                    break;
                case WishlistStatus.Medium:
                    backBrush = WLMediumBrush;
                    break;
                case WishlistStatus.Low:
                    backBrush = WLLowBrush;
                    break;
            }
            switch (vn?.ULStatus)
            {
                case UserlistStatus.Finished:
                    backBrush = ULFinishedBrush;
                    break;
                case UserlistStatus.Stalled:
                    backBrush = ULStalledBrush;
                    break;
                case UserlistStatus.Dropped:
                    backBrush = ULDroppedBrush;
                    break;
                case UserlistStatus.Unknown:
                    backBrush = ULUnknownBrush;
                    break;
            }
            g.FillPath(backBrush, path);
            if (isSelected) path.Widen(_selectedBorderPen);
            g.DrawPath(isSelected ? _selectedBorderPen : _borderPen, path);
            g.Clip = new Region(itemBounds);

            // Draw the photo
            var photoAreaY = itemBounds.Y + (int)textHeight * LinesOfTextAbovePicture;
            var photoAreaHeight = itemBounds.Height -
                                  (int)textHeight * (LinesOfTextAbovePicture + LinesOfTextBelowPicture);
            var photoArea = new Rectangle(itemBounds.X, photoAreaY, itemBounds.Width, photoAreaHeight);
            photoArea.Inflate(-spacing, -spacing);
            if (vn == null) return;
            var ext = Path.GetExtension(vn.ImageURL);
            var photoFile = string.Format($"{VNImagesFolder}{vn.VNID}{ext}");
            if (vn.ImageNSFW && !FormMain.Settings.NSFWImages) g.DrawImage(Resources.nsfw_image, photoArea);
            else if (File.Exists(photoFile))
            {
                DrawImageFitToSize(g, photoArea, photoFile);
            }
            else g.DrawImage(Resources.no_image, photoArea);
            if (vn.Languages != null)
            {
                var startingY = photoArea.Y;
                foreach (var language in vn.Languages.Originals)
                {
                    var flagPath = $"{FlagsFolder}{language}.png";
                    if (!File.Exists(flagPath)) continue;
                    //g.DrawImage(Image.FromFile(flagPath), new Point(photoArea.X + photoArea.Width - 24, startingY));
                    var point = new Point(photoArea.X + photoArea.Width - 24, startingY);
                    var size = new Size(24, 12);
                    g.DrawImageUnscaledAndClipped(Image.FromFile(flagPath), new Rectangle(point, size));
                    startingY += 16;
                }
            }
            DrawTileText(photoArea, textHeight, g, vn, olv);
        }

        private void DrawTileText(RectangleF photoArea, float textHeight, Graphics g, ListedVN vn, ObjectListView olv)
        {
            var dateWidth = g.MeasureString("9999-99-99", NormalFont).Width;
            var fmtNear = new StringFormat(StringFormatFlags.NoWrap)
            {
                Trimming = StringTrimming.EllipsisCharacter,
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Near
            };
            var fmtFar = new StringFormat(StringFormatFlags.NoWrap)
            {
                Trimming = StringTrimming.EllipsisCharacter,
                Alignment = StringAlignment.Far,
                LineAlignment = StringAlignment.Far
            };
            // Now draw the text portion
            //text above picture
            RectangleF textBoxRect = photoArea;
            textBoxRect.Y -= textHeight;
            textBoxRect.Height = textHeight;
            fmtNear.Alignment = StringAlignment.Near;
            g.DrawString(vn.Title, BoldFont, TextBrush, textBoxRect, fmtNear); //line 1: vn title
                                                                               //text below picture
            textBoxRect.Y += textHeight + photoArea.Height;
            var favoriteProducers = (olv.FindForm() as FormMain)?.FavoriteProducerList;
            Brush producerBrush = favoriteProducers != null && favoriteProducers.Exists(x => x.Name == vn.Producer) ? FavoriteProducerBrush : TextBrush;
            Brush dateBrush = DateIsUnreleased(vn.RelDate) ? UnreleasedBrush : TextBrush;
            g.DrawString(vn.Producer, NormalFont, producerBrush, textBoxRect, fmtNear); //line 2: vn producer 
            textBoxRect.Y += textHeight;
            textBoxRect.Width -= dateWidth;
            g.DrawString($"Rating: {vn.RatingAndVoteCount()}", NormalFont, TextBrush, textBoxRect, fmtNear);//line 3 left: rating/votecount
            textBoxRect.Width = photoArea.Width;
            g.DrawString(vn.RelDate, NormalFont, dateBrush, textBoxRect, fmtFar); //line 3 right: vn release date
            textBoxRect.Y += textHeight;
            if (vn.ULStatus == UserlistStatus.Playing)
            {
                var ulWidth = g.MeasureString("Userlist: ", NormalFont).Width;
                var playingRectangle = new RectangleF(textBoxRect.X + ulWidth, textBoxRect.Y, textBoxRect.Width - ulWidth, textBoxRect.Height);
                g.DrawString("Userlist: ", NormalFont, TextBrush, textBoxRect, fmtNear);
                g.DrawString("Playing", NormalFont, ULPlayingBrush, playingRectangle, fmtNear);
                if (vn.Vote > 0) g.DrawString($" (Vote:{vn.Vote:0.00})", NormalFont, TextBrush, textBoxRect, fmtFar); //line 3 right: vn release date
            }
            else
            {
                g.DrawString(vn.UserRelatedStatus(), NormalFont, TextBrush, textBoxRect, fmtNear);
            }
            //line 4: user-related status
        }

        private static GraphicsPath GetRoundedRect(RectangleF rect, float diameter)
        {
            var path = new GraphicsPath();

            var arc = new RectangleF(rect.X, rect.Y, diameter, diameter);
            path.AddArc(arc, 180, 90);
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();

            return path;
        }

        private static void DrawImageFitToSize(Graphics g, Rectangle photoArea, string photoFile)
        {
            Image photo;
            using (var ms = new MemoryStream(File.ReadAllBytes(photoFile))) photo = Image.FromStream(ms);
            //var photo = Image.FromFile(photoFile);
            var photoAreaRatio = (double)photoArea.Width / photoArea.Height;
            var photoRatio = (double)photo.Width / photo.Height;
            //show whole image but do not occupy whole area
            if (photoRatio > photoAreaRatio) //if image is wider
            {
                var shrinkratio = (double)photo.Width / photoArea.Width;
                var newWidth = photoArea.Width;
                var newHeight = (int)Math.Floor(photo.Height / shrinkratio);
                var newX = photoArea.X;
                var halfNewY = (double)newHeight / 2;
                var halfPhotoHeight = (double)photoArea.Height / 2;
                var newY = photoArea.Y + (int)Math.Floor(halfPhotoHeight) - (int)Math.Floor(halfNewY);
                var newPhotoRect = new Rectangle(newX, newY, newWidth, newHeight);
                g.DrawImage(photo, newPhotoRect);
            }
            else //if image is taller
            {
                var shrinkratio = (double)photo.Height / photoArea.Height;
                var newWidth = (int)Math.Floor(photo.Width / shrinkratio);
                var newHeight = photoArea.Height;
                var halfNewX = (double)newWidth / 2;
                var halfPhotoWidth = (double)photoArea.Width / 2;
                var newX = photoArea.X + (int)Math.Floor(halfPhotoWidth) - (int)Math.Floor(halfNewX);
                var newY = photoArea.Y;
                var newPhotoRect = new Rectangle(newX, newY, newWidth, newHeight);
                g.DrawImage(photo, newPhotoRect);
            }
        }

        /// <summary>
        ///     Scales an Image to fit the given dimensions.
        /// </summary>
        /// <param name="image">Image to be scaled</param>
        /// <param name="maxWidth">Maximum width</param>
        /// <param name="maxHeight">Maximum height</param>
        /// <returns>Scaled image</returns>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
    }

}