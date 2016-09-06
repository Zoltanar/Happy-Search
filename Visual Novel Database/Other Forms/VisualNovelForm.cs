using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Happy_Search.Properties;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming

namespace Happy_Search
{
    /// <summary>
    /// Form for showing Visual Novel Information
    /// </summary>
    public partial class VisualNovelForm : Form
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        private readonly FormMain _parentForm;
        private const int ScreenshotPadding = 10;

        /// <summary>
        /// Load VN form with specified Visual Novel.
        /// </summary>
        /// <param name="vnItem">Visual Novel to be shown</param>
        /// <param name="parentForm">Parent form</param>
        public VisualNovelForm(ListedVN vnItem, FormMain parentForm)
        {
            _parentForm = parentForm;
            Text = $"{FormMain.ClientName} - {vnItem.Title}";
            InitializeComponent();
            SetData(vnItem);
        }

        /// <returns>The text associated with this control.</returns>
        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private async void SetData(ListedVN vnItem)
        {
            if (vnItem.VNID == -1)
            {
                SetDeletedData();
                return;
            }
            //prepare data
            var ext = Path.GetExtension(vnItem.ImageURL);
            var imageLoc = $"vnImages\\{vnItem.VNID}{ext}";
            if (vnItem.Tags == string.Empty) vnTagCB.DataSource = "No Tags Found";
            else
            {
                List<string> taglist = FormMain.StringToTags(vnItem.Tags).Select(tag => tag.Print(_parentForm.PlainTags)).Where(printed => !printed.Equals("Not Approved")).ToList();
                taglist.Sort();
                vnTagCB.DataSource = taglist;
            }
            //relations section
            await _parentForm.TryQuery($"get vn relations (id = {vnItem.VNID})", "Relations Query Error", vnUpdateLink);
            var root = JsonConvert.DeserializeObject<VNRoot>(_parentForm.Conn.LastResponse.JsonPayload);
            if (root.Num == 0)
            {
                _parentForm.DBConn.Open();
                _parentForm.DBConn.RemoveVisualNovel(vnItem.VNID);
                _parentForm.DBConn.Close();
                SetDeletedData();
                return;
            }
            List<RelationsItem> relations = root.Items[0].Relations;
            int relationsCount = relations.Count;
            if (relationsCount > 0)
            {
                var relationsList = new List<string> { $"{relationsCount} Relations", "--------------" };
                relationsList.AddRange(relations.Select(relation => relation.Print()));
                vnRelationsCB.DataSource = relationsList;
            }
            else
            {
                vnRelationsCB.DataSource = new List<string> { "No relations found." };
            }
            //set screenshots
            await _parentForm.TryQuery($"get vn screens (id = {vnItem.VNID})", "Relations Query Error", vnUpdateLink);
            root = JsonConvert.DeserializeObject<VNRoot>(_parentForm.Conn.LastResponse.JsonPayload);
            if (root.Num == 0)
            {
                _parentForm.DBConn.Open();
                _parentForm.DBConn.RemoveVisualNovel(vnItem.VNID);
                _parentForm.DBConn.Close();
                SetDeletedData();
                return;
            }
            List<ScreenItem> screens = root.Items[0].Screens;
            if (screens.Any())
            {
                int imageX = 0;
                foreach (var screen in screens)
                {
                    if (screen.Nsfw && !Settings.Default.ShowNSFWImages)
                    {
                        imageX += DrawNSFWImageFitToHeight(picturePanel, 400, imageX, Resources.nsfw_image) + ScreenshotPadding;
                    }
                    else
                    {
                        imageX += DrawImageFitToHeight(picturePanel, 400, imageX, screen) + ScreenshotPadding;
                    }
                }
            }
            else
            {
                picturePanel.Controls.Add(new Label
                {
                    Text = @"No Screenshots Found",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 0),
                    Size = new Size(picturePanel.Size.Width, picturePanel.Size.Height),
                    Font = new Font(DefaultFont.FontFamily, DefaultFont.SizeInPoints * 1.8f)
                });
            }
            //set data
            vnName.Text = vnItem.Title;
            vnID.Text = vnItem.VNID.ToString();
            vnKanjiName.Text = vnItem.KanjiTitle;
            vnProducer.Text = vnItem.Producer;
            vnDate.Text = vnItem.RelDate;
            vnDesc.Text = vnItem.Description;
            vnRating.Text = vnItem.RatingAndVoteCount();
            vnPopularity.Text = $"Popularity: {vnItem.Popularity.ToString("0.00")}";
            vnLength.Text = vnItem.Length;
            vnUserStatus.Text = vnItem.UserRelatedStatus();
            vnUpdateLink.Text = $"Updated {vnItem.UpdatedDate} days ago. Click to update.";
            if (vnItem.ImageNSFW && !Settings.Default.ShowNSFWImages) pcbImages.Image = Resources.nsfw_image;
            else if (File.Exists(imageLoc)) pcbImages.ImageLocation = imageLoc;
            else pcbImages.Image = Resources.no_image;
        }

        private void SetDeletedData()
        {
            vnName.Text = @"This VN was deleted.";
            vnKanjiName.Text = "";
            vnProducer.Text = "";
            vnDate.Text = "";
            vnUserStatus.Text = "";
            vnLength.Text = "";
            vnID.Text = "";
            vnUpdateLink.Text = "";
            vnRating.Text = "";
            vnPopularity.Text = "";
            pcbImages.Image = Resources.no_image;
        }

        private async void vnUpdateLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (vnUpdateLink.Text.Equals(Resources.vn_updated)) return;
            if (vnUpdateLink.Text.Equals("")) return;
            var vnItem = await _parentForm.UpdateSingleVN(Convert.ToInt32(vnID.Text), vnUpdateLink);
            SetData(vnItem);
        }

        private void vnID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (vnID.Text.Equals("")) return;
            Process.Start("http://vndb.org/v" + vnID.Text + '/');
        }

        private void RelationSelected(object sender, EventArgs e)
        {
            ComboBox dropdownlist = (ComboBox)sender;
            switch (dropdownlist.SelectedIndex)
            {
                case 0:
                    return;
                case 1:
                    dropdownlist.SelectedIndex = 0;
                    return;
                default:
                    string[] parts = dropdownlist.SelectedItem.ToString().Split('-');
                    var vnid = Convert.ToInt32(parts.Last());
                    _parentForm.DBConn.Open();
                    var vn = _parentForm.DBConn.GetSingleVN(vnid, _parentForm.UserID);
                    _parentForm.DBConn.Close();
                    SetData(vn);
                    return;
            }
        }


        private static int DrawImageFitToHeight(Control control, int height, int locationX, ScreenItem screenItem)
        {
            string[] urlSplit = screenItem.Image.Split('/');
            string photoString = $"{FormMain.VNScreensFolder}{urlSplit[urlSplit.Length - 2]}\\{urlSplit[urlSplit.Length - 1]}";
            if (!File.Exists(photoString))
            {
                FormMain.SaveScreenshot(screenItem.Image, photoString);
            }
            var photo = Image.FromFile(photoString);
            int newWidth;
            var photoToAreaHeightRatio = (double)screenItem.Height / height;
            //show whole image but do not occupy whole area
            //if image is taller than height
            if (screenItem.Height > height)
            {
                newWidth = (int)(screenItem.Width / photoToAreaHeightRatio);
            } //if image is exactly  height
            else if (screenItem.Height == height)
            {
                newWidth = screenItem.Width;
            }
            //if image is shorter than height
            else
            {
                newWidth = (int)(screenItem.Width * photoToAreaHeightRatio);
            }
            var pictureBox = new PictureBox
            {
                BackgroundImage = photo,
                Size = new Size(newWidth, height),
                Location = new Point(locationX, 0),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            control.Controls.Add(pictureBox);
            return newWidth;
        }

        private static int DrawNSFWImageFitToHeight(Control control, int height, int locationX, Image nsfwImage)
        {
            int newWidth;
            var photoToAreaHeightRatio = (double)nsfwImage.Height / height;
            //show whole image but do not occupy whole area
            //if image is taller than height
            if (nsfwImage.Height > height)
            {
                newWidth = (int)(nsfwImage.Width / photoToAreaHeightRatio);
            } //if image is exactly  height
            else if (nsfwImage.Height == height)
            {
                newWidth = nsfwImage.Width;
            }
            //if image is shorter than height
            else
            {
                newWidth = (int)(nsfwImage.Width * photoToAreaHeightRatio);
            }
            var pictureBox = new PictureBox
            {
                BackgroundImage = nsfwImage,
                Size = new Size(newWidth, height),
                Location = new Point(locationX, 0),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            control.Controls.Add(pictureBox);
            return newWidth;
        }

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private void MoveWindowLeftclick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void CloseButton(object sender, EventArgs e)
        {
            Close();
        }

        private void CloseByEscape(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

    }
}