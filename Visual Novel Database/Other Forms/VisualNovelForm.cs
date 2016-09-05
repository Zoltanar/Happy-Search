using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Happy_Search.Properties;

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

        private void SetData(ListedVN vnItem)
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