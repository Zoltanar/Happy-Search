using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Happy_Search.Properties;
using static Happy_Search.StaticHelpers;

namespace Happy_Search
{
    /// <summary>
    /// Object for displaying Visual Novel in Object List View.
    /// </summary>
    public class ListedVN
    {
        internal static readonly string[] StatusUL = {"Unknown", "Playing", "Finished", "Stalled", "Dropped"};
        internal static readonly string[] PriorityWL = {"High", "Medium", "Low", "Blacklist"};

        internal static readonly string[] LengthTime =
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
        /// <param name="popularity">Popularity of VN, percentage of most popular VN</param>
        /// <param name="rating">Bayesian rating of VN, 1-10</param>
        /// <param name="voteCount">Number of votes cast on VN</param>
        /// <param name="relations">JSON Array string containing List of Relation Items</param>
        /// <param name="screens">JSON Array string containing List of Screenshot Items</param>
        /// <param name="anime">JSON Array string containing List of Anime Items</param>
        public ListedVN(string title, string kanjiTitle, string reldate, string producer, int length, int ulstatus,
            int uladded, string ulnote, int wlstatus, int wladded, int vote, int voteadded,
            string tags, int vnid, DateTime updatedDate, string imageURL, bool imageNSFW, string description,
            double popularity, double rating, int voteCount, string relations, string screens, string anime)
        {
            if (reldate.Equals("") || reldate.Equals("tba")) reldate = "N/A";
            ULStatus = ulstatus != -1 ? StatusUL[ulstatus] : "";
            WLStatus = wlstatus != -1 ? PriorityWL[wlstatus] : "";
            if (vote != -1) Vote = (double) vote/10;
            Title = title;
            KanjiTitle = kanjiTitle;
            RelDate = reldate;
            DateForSorting = StringToDate(reldate);
            Producer = producer;
            Length = LengthTime[length];
            ULAdded = DateTimeOffset.FromUnixTimeSeconds(uladded).UtcDateTime;
            ULNote = ulnote;
            WLAdded = DateTimeOffset.FromUnixTimeSeconds(wladded).UtcDateTime;
            VoteAdded = DateTimeOffset.FromUnixTimeSeconds(voteadded).UtcDateTime;
            Tags = tags;
            VNID = vnid;
            UpdatedDate = DaysSince(updatedDate);
            ImageURL = imageURL;
            ImageNSFW = imageNSFW;
            Description = description;
            Popularity = popularity;
            Rating = rating;
            VoteCount = voteCount;
            Relations = relations;
            Screens = screens;
            Anime = anime;
        }

        /// <summary>
        /// Constructor for empty ListedVN for when null cannot be used.
        /// </summary>
        public ListedVN()
        {
            VNID = -1;
        }

        /// <summary>
        /// List of Tags, must be set prior to use.
        /// </summary>
        public List<TagItem> TagList { get; set; }

        /// <summary>
        /// Sets TagList field.
        /// </summary>
        /// <param name="plainTags">List of tags from tagdump</param>
        public void SetTags(List<WrittenTag> plainTags)
        {
            var tagList = StringToTags(Tags);
            foreach (TagItem tag in tagList)
            {
                tag.SetCategory(plainTags);
            }
            TagList = tagList;
        }

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
        /// Date used for sorting rather than display string.
        /// </summary>
        public DateTime DateForSorting { get; set; }

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
        public int VoteCount { get; set; }

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

        /// <summary>
        /// JSON Array string containing List of Relation Items
        /// </summary>
        [OLVIgnore]
        public string Relations { get; set; }

        /// <summary>
        /// JSON Array string containing List of Screenshot Items
        /// </summary>
        [OLVIgnore]
        public string Screens { get; set; }

        /// <summary>
        /// JSON Array string containing List of Anime Items
        /// </summary>
        [OLVIgnore]
        public string Anime { get; set; }

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
            string[] parts = {"", "", ""};
            if (!ULStatus.Equals(""))
            {
                parts[0] = "Userlist: ";
                parts[1] = ULStatus;
            }
            else if (!WLStatus.Equals(""))
            {
                parts[0] = "Wishlist: ";
                parts[1] = WLStatus;
            }
            if (Vote > 0) parts[2] = $" (Vote:{Vote:0.00})";
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
            return new CustomItemNotes(notes,groups);
        }
        /// <summary>
        /// Checks if title was released between now and a past date, the current date is included.
        /// Make sure to enter arguments in correct order.
        /// </summary>
        /// <param name="oldDate">Date furthest from the present</param>
        public bool ReleasedBetweenNowAnd(DateTime oldDate)
        {
            return DateForSorting > oldDate && DateForSorting <= DateTime.UtcNow;
        }

        /// <summary>
        /// Check if title was released in specified year.
        /// </summary>
        /// <param name="year">Year of release</param>
        public bool ReleasedInYear(int year)
        {
            return DateForSorting.Year == year;
        }
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
        public ListedProducer(string name, int numberOfTitles, DateTime updated, int id)
        {
            Name = name;
            NumberOfTitles = numberOfTitles;
            Updated = DaysSince(updated);
            ID = id;
        }

        /// <summary>
        /// Constructor for ListedProducer for favorite producers.
        /// </summary>
        /// <param name="name">Producer Name</param>
        /// <param name="numberOfTitles">Number of Producer's titles</param>
        /// <param name="updated">Date of last update to Producer</param>
        /// <param name="id">Producer ID</param>
        /// <param name="userAverageVote">User's average vote on Producer titles. (Only titles with votes)</param>
        /// <param name="userDropRate">User's average drop rate on Producer titles. (Dropped / (Finished+Dropped)</param>
        public ListedProducer(string name, int numberOfTitles, DateTime updated, int id,
            double userAverageVote, int userDropRate)
        {
            Name = name;
            NumberOfTitles = numberOfTitles;
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
        /// Convert ListedSearchedProducer to ListedProducer.
        /// </summary>
        /// <param name="searchedProducer">Producer to be converted</param>
        /// <returns>ListedProducer with name and ID of ListedSearchedProducer</returns>
        public static explicit operator ListedProducer(ListedSearchedProducer searchedProducer)
        {
            return new ListedProducer(searchedProducer.Name, -1, DateTime.MinValue, searchedProducer.ID);
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
        private Pen _borderPen = new Pen(Color.FromArgb(0x33, 0x33, 0x33));
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

            _borderPen = e.Item.Selected ? Pens.Blue : new Pen(Color.FromArgb(0x33, 0x33, 0x33));
            DrawVNTile(g, itemBounds, rowObject, olv);
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
        /// <param name="olv">OLV where tile is drawn.</param>
        public void DrawVNTile(Graphics g, Rectangle itemBounds, object rowObject, ObjectListView olv)
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
                case "High":
                    backBrush = WLHighBrush;
                    break;
                case "Medium":
                    backBrush = WLMediumBrush;
                    break;
                case "Low":
                    backBrush = WLLowBrush;
                    break;
            }
            switch (vn?.ULStatus)
            {
                case "Finished":
                    backBrush = ULFinishedBrush;
                    break;
                case "Stalled":
                    backBrush = ULStalledBrush;
                    break;
                case "Dropped":
                    backBrush = ULDroppedBrush;
                    break;
                case "Unknown":
                    backBrush = ULUnknownBrush;
                    break;
            }
            g.FillPath(backBrush, path);
            g.DrawPath(_borderPen, path);
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
            if (vn.ImageNSFW && !Settings.Default.ShowNSFWImages) g.DrawImage(Resources.nsfw_image, photoArea);
            else if (File.Exists(photoFile))
            {
                DrawImageFitToSize(g, photoArea, photoFile);
            }
            else g.DrawImage(Resources.no_image, photoArea);
            DrawTileText(photoArea,textHeight,g,vn, olv);
        }
         
        private void DrawTileText(RectangleF photoArea, float textHeight, Graphics g,ListedVN vn, ObjectListView olv)
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
            if (vn.ULStatus.Equals("Playing"))
            {
                var ulWidth = g.MeasureString("Userlist: ", NormalFont).Width;
                var playingRectangle = new RectangleF(textBoxRect.X+ulWidth,textBoxRect.Y, textBoxRect.Width-ulWidth,textBoxRect.Height);
                g.DrawString("Userlist: ", NormalFont, TextBrush, textBoxRect, fmtNear);
                g.DrawString("Playing", NormalFont, ULPlayingBrush, playingRectangle, fmtNear);
                if(vn.Vote > 0) g.DrawString($" (Vote:{vn.Vote:0.00})", NormalFont, TextBrush, textBoxRect, fmtFar); //line 3 right: vn release date
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
            var photo = Image.FromFile(photoFile);
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