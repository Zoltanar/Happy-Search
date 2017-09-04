using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Happy_Search.Properties;
using Happy_Apps_Core;

namespace Happy_Search
{
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
            if (!(e.Item.ListView is ObjectListView olv) || olv.View != View.Tile) return false;
            
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
            var backBrush = StaticHelpers.GetBrushFromStatuses(vn) ?? StaticHelpers.DefaultTileBrush;
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
            var photoFile = string.Format($"{StaticHelpers.VNImagesFolder}{vn.VNID}{ext}");
            if (vn.ImageNSFW && !FormMain.GuiSettings.NSFWImages) g.DrawImage(Resources.nsfw_image, photoArea);
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
                    var flagPath = $"{StaticHelpers.FlagsFolder}{language}.png";
                    if (!File.Exists(flagPath)) continue;
                    var point = new Point(photoArea.X + photoArea.Width - 24, startingY);
                    var size = new Size(24, 12);
                    g.DrawImageUnscaledAndClipped(Image.FromFile(flagPath), new Rectangle(point, size));
                    startingY += 16;
                }
            }
            DrawTileText(photoArea, textHeight, g, vn);
        }

        private void DrawTileText(RectangleF photoArea, float textHeight, Graphics g, ListedVN vn)
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
            var favoriteProducers = StaticHelpers.LocalDatabase.FavoriteProducerList;
            Brush producerBrush = favoriteProducers != null && favoriteProducers.Exists(x => x.Name == vn.Producer) ? StaticHelpers.FavoriteProducerBrush : TextBrush;
            Brush dateBrush = StaticHelpers.DateIsUnreleased(vn.RelDate) ? StaticHelpers.UnreleasedBrush : TextBrush;
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
                g.DrawString("Playing", NormalFont, StaticHelpers.ULPlayingBrush, playingRectangle, fmtNear);
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