using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Happy_Search.Properties;

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
        /// <param name="popularity">Popularity of VN, percentage of most popular VN</param>
        /// <param name="rating">Bayesian rating of VN, 1-10</param>
        /// <param name="voteCount">Number of votes cast on VN</param>
        /// <param name="relations">JSON Array string containing List of Relation Items</param>
        /// <param name="screens">JSON Array string containing List of Screenshot Items</param>
        /// <param name="anime">JSON Array string containing List of Anime Items</param>
        public ListedVN(string title, string kanjiTitle, string reldate, string producer, int length, int ulstatus,
            int uladded, string ulnote,int wlstatus, int wladded, int vote, int voteadded, 
            string tags, int vnid, DateTime updatedDate,string imageURL, bool imageNSFW, string description,
            double popularity, double rating, int voteCount, string relations, string screens, string anime)
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
            if (Vote > 0) parts[2] = $" (Vote:{Vote.ToString("0.00")})";
            return string.Join(" ", parts);
        }


        /// <summary>
        /// Get VN's rating, votecount and popularity as a string.
        /// </summary>
        /// <returns>Rating, votecount and popularity</returns>
        public string RatingAndVoteCount()
        {
            return VoteCount == 0 ? "No votes yet." : $"{Rating.ToString("0.00")} ({VoteCount} votes)";
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
        /// Constructor for ListedProducer for favorite producers.
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
        /// Convert ListedSearchedProducer to ListedProducer.
        /// </summary>
        /// <param name="searchedProducer">Producer to be converted</param>
        /// <returns>ListedProducer with name and ID of ListedSearchedProducer</returns>
        public static explicit operator ListedProducer(ListedSearchedProducer searchedProducer)
        {
            return new ListedProducer(searchedProducer.Name,-1,"No",DateTime.MinValue, searchedProducer.ID);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"ID={ID} \t\tName={Name}";

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
            DrawVNTile(g, itemBounds, rowObject, olv, (OLVListItem)e.Item);

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
        /// <param name="item">OLV Item</param>
        public void DrawVNTile(Graphics g, Rectangle itemBounds, object rowObject, ObjectListView olv,
            OLVListItem item)
        {
            Brush textBrush = new SolidBrush(Color.FromArgb(0x22, 0x22, 0x22));
            var backBrush = Brushes.LightBlue;
            //tile size 230,375
            //image 230-spacing, 300-spacing
            const int spacing = 8;
            var font = new Font("Microsoft Sans Serif", 8.25f);
            var boldFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
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
            var textHeight = g.MeasureString("Wj", font).Height;
            var dateWidth = g.MeasureString("9999-99-99", font).Width;
            // Allow a border around the card
            itemBounds.Inflate(-2, -2);
            var vn = rowObject as ListedVN;
            // Draw card background
            const int rounding = 20;
            var path = GetRoundedRect(itemBounds, rounding);
            switch (vn?.WLStatus)
            {
                case "High":
                    backBrush = Brushes.DeepPink;
                    break;
                case "Medium":
                    backBrush = Brushes.Pink;
                    break;
                case "Low":
                    backBrush = Brushes.LightPink;
                    break;
            }
            switch (vn?.ULStatus)
            {
                case "Finished":
                    backBrush = Brushes.LightGreen;
                    break;
                case "Stalled":
                    backBrush = Brushes.LightYellow;
                    break;
                case "Dropped":
                    backBrush = Brushes.DarkOrange;
                    break;
                case "Unknown":
                    backBrush = Brushes.Gray;
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
            var imageUrl = vn.ImageURL;
            var ext = Path.GetExtension(imageUrl);
            var photoFile = string.Format($"vnImages\\{vn.VNID}{ext}");
            if (vn.ImageNSFW && !Settings.Default.ShowNSFWImages) g.DrawImage(Resources.nsfw_image, photoArea);
            else if (File.Exists(photoFile))
            {
                DrawImageFitToSize(g, photoArea, photoFile);
            }
            else g.DrawImage(Resources.no_image, photoArea);

            // Now draw the text portion
            //text above picture
            RectangleF textBoxRect = photoArea;
            textBoxRect.Y -= textHeight;
            textBoxRect.Height = textHeight;
            fmtNear.Alignment = StringAlignment.Near;
            g.DrawString(vn.Title, boldFont, textBrush, textBoxRect, fmtNear); //line 1: vn title
                                                                               //text below picture
            textBoxRect.Y += textHeight + photoArea.Height;
            var producerBrush = textBrush;
            List<ListedProducer> producers =
                Application.OpenForms.OfType<FormMain>()
                    .Select(main => main.olFavoriteProducers.Objects as List<ListedProducer>)
                    .FirstOrDefault();
            if (producers != null && producers.Exists(x => x.Name == vn.Producer)) producerBrush = Brushes.Yellow;
            g.DrawString(vn.Producer, font, producerBrush, textBoxRect, fmtNear); //line 2: vn producer 
            textBoxRect.Y += textHeight;
            textBoxRect.Width -= dateWidth;
            g.DrawString($"Rating: {vn.RatingAndVoteCount()}", font, textBrush, textBoxRect, fmtNear);//line 3 left: rating/votecount
            textBoxRect.Width = photoArea.Width;
            g.DrawString(vn.RelDate, font, textBrush, textBoxRect, fmtFar); //line 3 right: vn release date
            textBoxRect.Y += textHeight;
            g.DrawString(vn.UserRelatedStatus(), font, textBrush, textBoxRect, fmtNear);
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