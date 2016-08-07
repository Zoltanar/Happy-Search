using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Visual_Novel_Database.Properties;
// ReSharper disable InconsistentNaming

namespace Visual_Novel_Database
{
    public partial class VisualNovelForm : Form
    {
        private readonly FormMain _parentForm;
        public VisualNovelForm(ListedVN vnItem, FormMain parentForm)
        {
            _parentForm = parentForm;
            Text = $"{FormMain.ClientName} - {vnItem.Title}";
            InitializeComponent();

            SetData(vnItem);
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void SetData(ListedVN vnItem)
        {
            //prepare data
            string ext = Path.GetExtension(vnItem.ImageURL);
            var imageLoc = $"vnImages\\{vnItem.VNID}{ext}";
            List<string> taglist = vnItem.Tags != string.Empty ? FormMain.StringToTags(vnItem.Tags).Select(tag => _parentForm.PlainTags.Find(item => item.ID == tag.ID)?.Name).Where(tagName => tagName != null).ToList() : new List<string>{"No Tags Found"};
            string[] parts = { "", "", "" };
            if (!vnItem.ULStatus.Equals(""))
            {
                parts[0] = "Userlist: ";
                parts[1] = vnItem.ULStatus;
            }
            else if (!vnItem.WLStatus.Equals(""))
            {
                parts[0] = "Wishlist: ";
                parts[1] = vnItem.WLStatus;
            }
            if (vnItem.Vote > 0) parts[2] = $" (Vote:{vnItem.Vote.ToString("#.##")})";
            string complete = string.Join(" ", parts);

            //set data
            vnName.Text = vnItem.Title;
            vnID.Text = vnItem.VNID.ToString();
            vnKanjiName.Text = vnItem.KanjiTitle;
            vnProducer.Text = vnItem.Producer;
            vnDate.Text = vnItem.RelDate;
            vnDesc.Text = vnItem.Description;
            vnUpdateLink.Text = $"Updated {vnItem.UpdatedDate} days ago. Click to update.";
            vnTagCB.DataSource = taglist;
            vnLength.Text = vnItem.Length;
            vnUserStatus.Text = complete;
            if (vnItem.ImageNSFW && !Settings.Default.ShowNSFWImages) pcbImages.Image = Resources.nsfw_image;
            else if (File.Exists(imageLoc)) pcbImages.ImageLocation = imageLoc;
            else pcbImages.Image = Resources.no_image;

        }

        private async void vnUpdateLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (vnUpdateLink.Text.Equals(Resources.vn_updated)) return;
            await _parentForm.UpdateSingleVN(Convert.ToInt32(vnID.Text), vnUpdateLink);
            var vnItem = _parentForm.UpdatingVN;
            if (vnItem == null)
            {
                vnUpdateLink.Text = Resources.svn_query_error;
                return;
            }
            Debug.Print($"Trying to update {vnItem.VNID} p3");
            SetData(vnItem);
            vnUpdateLink.Text = Resources.vn_updated;
        }

        private void vnID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (vnID.Text.Equals("")) return;
            Process.Start("http://vndb.org/v" + vnID.Text + '/');
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void MoveWindowLeftclick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
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
