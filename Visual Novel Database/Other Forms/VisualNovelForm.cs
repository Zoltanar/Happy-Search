using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
        private ListedVN displayedVN;

        /// <summary>
        /// Load VN form with specified Visual Novel.
        /// </summary>
        /// <param name="vnItem">Visual Novel to be shown</param>
        /// <param name="parentForm">Parent form</param>
        public VisualNovelForm(ListedVN vnItem, FormMain parentForm)
        {
            _parentForm = parentForm;
            InitializeComponent();
            tagTypeC.Checked = Settings.Default.TagTypeC;
            tagTypeS.Checked = Settings.Default.TagTypeS;
            tagTypeT.Checked = Settings.Default.TagTypeT;
            tagTypeC.CheckedChanged += DisplayTags;
            tagTypeS.CheckedChanged += DisplayTags;
            tagTypeT.CheckedChanged += DisplayTags;
            SetData(vnItem);
        }

        internal void DisplayTags(object sender, EventArgs e)
        {
            if (sender != null && !_parentForm.DontTriggerEvent)
            {

                var checkBox = (CheckBox)sender;
                _parentForm.DontTriggerEvent = true;
                switch (checkBox.Name)
                {
                    case "tagTypeC":
                        Settings.Default.TagTypeC = checkBox.Checked;
                        _parentForm.tagTypeC.Checked = checkBox.Checked;
                        _parentForm.tagTypeC2.Checked = checkBox.Checked;
                        break;
                    case "tagTypeS":
                        Settings.Default.TagTypeS = checkBox.Checked;
                        _parentForm.tagTypeS.Checked = checkBox.Checked;
                        _parentForm.tagTypeS2.Checked = checkBox.Checked;
                        break;
                    case "tagTypeT":
                        Settings.Default.TagTypeT = checkBox.Checked;
                        _parentForm.tagTypeT.Checked = checkBox.Checked;
                        _parentForm.tagTypeT2.Checked = checkBox.Checked;
                        break;
                }
                _parentForm.DontTriggerEvent = false;
                _parentForm.DisplayCommonTags(null, null);
                _parentForm.DisplayCommonTagsURT(null, null);
                Settings.Default.Save();
            }
            if (displayedVN == null || displayedVN.Tags == string.Empty) vnTagCB.DataSource = "No Tags Found";
            else
            {
                List<TagItem> allTags = FormMain.StringToTags(displayedVN.Tags);
                var visibleTags = new List<TagItem>();
                foreach (var tag in allTags)
                {
                    var cat = tag.GetCategory(_parentForm.PlainTags);
                    switch (cat)
                    {
                        case FormMain.ContentTag:
                            if (!tagTypeC.Checked) continue;
                            break;
                        case FormMain.SexualTag:
                            if (!tagTypeS.Checked) continue;
                            break;
                        case FormMain.TechnicalTag:
                            if (!tagTypeT.Checked) continue;
                            break;
                    }
                    visibleTags.Add(tag);
                }
                List<string> stringList = visibleTags.Select(x => x.Print(_parentForm.PlainTags)).ToList();
                stringList.Sort();
                vnTagCB.DataSource = stringList;
            }
        }

        /// <returns>The text associated with this control.</returns>
        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private async void SetNewData(int vnid)
        {
            await _parentForm.GetSingleVN(vnid, vnUpdateLink, true);
            var root = JsonConvert.DeserializeObject<VNRoot>(_parentForm.Conn.LastResponse.JsonPayload);
            if (root.Num == 0)
            {
                SetDeletedData();
                return;
            }
            _parentForm.DBConn.Open();
            var vn = _parentForm.DBConn.GetSingleVN(vnid, _parentForm.UserID);
            _parentForm.DBConn.Close();
            SetDeletedData(); //clear before setting new data
            SetData(vn);
        }

        private async void SetData(ListedVN vnItem)
        {
            Text = $@"{FormMain.ClientName} - {vnItem.Title}";
            //prepare data
            displayedVN = vnItem;
            var ext = Path.GetExtension(vnItem.ImageURL);
            var imageLoc = $"vnImages\\{vnItem.VNID}{ext}";
            DisplayTags(null, null);
            DisplayVNCharacterTraits(vnItem);
            //relations, anime and screenshots are only fetched here but are saved to database/disk
            var taskResult = await GetVNRelations(vnItem);
            if (!taskResult) return;
            taskResult = await GetVNAnime(vnItem);
            if (!taskResult) return;
            taskResult = await GetVNScreenshots(vnItem);
            if (!taskResult) return;
            //set data
            vnName.Text = vnItem.Title;
            vnID.Text = vnItem.VNID.ToString();
            vnKanjiName.Text = vnItem.KanjiTitle;
            vnProducer.Text = vnItem.Producer;
            vnDate.Text = vnItem.RelDate;
            vnDesc.Text = vnItem.Description;
            vnRating.Text = vnItem.RatingAndVoteCount();
            vnPopularity.Text = $@"Popularity: {vnItem.Popularity:0.00}";
            vnLength.Text = vnItem.Length;
            vnUserStatus.Text = vnItem.UserRelatedStatus();
            vnUpdateLink.Text = $@"Updated {vnItem.UpdatedDate} days ago. Click to update.";
            if (vnItem.ImageNSFW && !Settings.Default.ShowNSFWImages) pcbImages.Image = Resources.nsfw_image;
            else if (File.Exists(imageLoc)) pcbImages.ImageLocation = imageLoc;
            else pcbImages.Image = Resources.no_image;
        }

        private void SetDeletedData()
        {
            Text = $@"{FormMain.ClientName} - (Deleted Title)";
            displayedVN = null;
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
            vnTagCB.DataSource = null;
            vnRelationsCB.DataSource = null;
            vnAnimeCB.DataSource = null;
            picturePanel.Controls.Clear();
            pcbImages.Image = Resources.no_image;
        }

        private void DisplayVNCharacterTraits(ListedVN vnItem)
        {
            var vnCharacters = _parentForm.CharacterList.Where(x => x.CharacterIsInVN(vnItem.VNID)).ToArray();
            var stringList = new List<string> { $"{vnCharacters.Length} Characters" };
            foreach (var characterItem in vnCharacters)
            {
                stringList.Add($"Character {characterItem.ID}");
                foreach (var trait in characterItem.Traits)
                {
                    stringList.Add(_parentForm.PlainTraits.Find(x => x.ID == trait.ID)?.Print());
                }
                stringList.Add("---------------");
            }
            vnTraitsCB.DataSource = stringList;
        }

        private async Task<bool> GetVNAnime(ListedVN vnItem)
        {
            //anime was fetched before but nothing was found
            if (vnItem.Anime.Equals("Empty"))
            {
                vnAnimeCB.DataSource = new List<string> { "No Anime found." };
                return true;
            }
            //anime was fetched before and something was found
            if (!vnItem.Anime.Equals(""))
            {
                var loadedAnime = JsonConvert.DeserializeObject<List<AnimeItem>>(vnItem.Anime);
                var animeList = new List<string> { $"{loadedAnime.Count} Anime", "--------------" };
                animeList.AddRange(loadedAnime.Select(anime => anime.Print()));
                vnAnimeCB.DataSource = animeList;
                return true;
            }
            //anime hasn't been fetched before
            if (_parentForm.Conn.Status != VndbConnection.APIStatus.Ready)
            {
                vnAnimeCB.DataSource = new List<string>
                {
                    "Anime cannot be fetched until API connection is ready."
                };
                return true;
            }
            await _parentForm.TryQuery($"get vn anime (id = {vnItem.VNID})", "Anime Query Error", vnUpdateLink);
            var root = JsonConvert.DeserializeObject<VNRoot>(_parentForm.Conn.LastResponse.JsonPayload, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            if (root.Num == 0)
            {
                _parentForm.DBConn.Open();
                _parentForm.DBConn.RemoveVisualNovel(vnItem.VNID);
                _parentForm.DBConn.Close();
                SetDeletedData();
                return false;
            }
            List<AnimeItem> animeItems = root.Items[0].Anime;
            _parentForm.DBConn.Open();
            _parentForm.DBConn.AddAnimeToVN(vnItem.VNID, animeItems);
            _parentForm.DBConn.Close();
            int animeCount = animeItems.Count;
            if (animeCount > 0)
            {
                var animeList = new List<string> { $"{animeCount} Anime", "--------------" };
                animeList.AddRange(animeItems.Select(anime => anime.Print()));
                vnAnimeCB.DataSource = animeList;
                return true;
            }
            vnAnimeCB.DataSource = new List<string> { "No Anime found." };
            return true;
        }

        private async Task<bool> GetVNScreenshots(ListedVN vnItem)
        {
            //screenshots were fetched before but nothing was found
            if (vnItem.Screens.Equals("Empty")) return true;
            List<ScreenItem> screens;
            //screenshots were fetched before and something was found
            if (!vnItem.Screens.Equals(""))
            {
                screens = JsonConvert.DeserializeObject<List<ScreenItem>>(vnItem.Screens);
            }
            //screenshots haven't been fetched yet
            else
            {
                if (_parentForm.Conn.Status != VndbConnection.APIStatus.Ready)
                {
                    picturePanel.Controls.Add(new Label
                    {
                        Text = @"Screenshots can't be fetched until API connection is ready.",
                        TextAlign = ContentAlignment.MiddleCenter,
                        Location = new Point(0, 0),
                        Size = new Size(picturePanel.Size.Width, picturePanel.Size.Height),
                        Font = new Font(DefaultFont.FontFamily, DefaultFont.SizeInPoints * 1.8f)
                    });
                    return true;
                }
                await _parentForm.TryQuery($"get vn screens (id = {vnItem.VNID})", "Screens Query Error", vnUpdateLink);
                var root = JsonConvert.DeserializeObject<VNRoot>(_parentForm.Conn.LastResponse.JsonPayload);
                if (root.Num == 0)
                {
                    _parentForm.DBConn.Open();
                    _parentForm.DBConn.RemoveVisualNovel(vnItem.VNID);
                    _parentForm.DBConn.Close();
                    SetDeletedData();
                    return false;
                }
                screens = root.Items[0].Screens;
                _parentForm.DBConn.Open();
                _parentForm.DBConn.AddScreensToVN(vnItem.VNID, screens);
                _parentForm.DBConn.Close();
            }
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
            return true;
        }

        private async Task<bool> GetVNRelations(ListedVN vnItem)
        {
            //relations were fetched before but nothing was found
            if (vnItem.Relations.Equals("Empty"))
            {
                vnRelationsCB.DataSource = new List<string> { "No relations found." };
                return true;
            }
            //relations were fetched before and something was found
            if (!vnItem.Relations.Equals(""))
            {
                var loadedRelations = JsonConvert.DeserializeObject<List<RelationsItem>>(vnItem.Relations);
                DisplayRelations(loadedRelations);
                return true;
            }
            //relations haven't been fetched before
            if (_parentForm.Conn.Status != VndbConnection.APIStatus.Ready)
            {
                vnRelationsCB.DataSource = new List<string>
                {
                    "Relations cannot be fetched unttil API connection is ready."
                };
                return true;
            }
            await _parentForm.TryQuery($"get vn relations (id = {vnItem.VNID})", "Relations Query Error", vnUpdateLink);
            var root = JsonConvert.DeserializeObject<VNRoot>(_parentForm.Conn.LastResponse.JsonPayload);
            if (root.Num == 0)
            {
                _parentForm.DBConn.Open();
                _parentForm.DBConn.RemoveVisualNovel(vnItem.VNID);
                _parentForm.DBConn.Close();
                SetDeletedData();
                return false;
            }
            List<RelationsItem> relations = root.Items[0].Relations;
            _parentForm.DBConn.Open();
            _parentForm.DBConn.AddRelationsToVN(vnItem.VNID, relations);
            _parentForm.DBConn.Close();
            DisplayRelations(relations);
            return true;
        }

        private void DisplayRelations(List<RelationsItem> relationList)
        {
            var relationCount = relationList.Count;
            if (relationCount == 0)
            {

                vnRelationsCB.DataSource = new List<string> { "No relations found." };
                return;
            }
            var titleString = relationList.Count == 1 ? "1 Relation" : $"{relationList.Count} Relations";
            var stringList = new List<string> { titleString, "--------------" };
            IEnumerable<IGrouping<string, RelationsItem>> groups = relationList.GroupBy(x => x.Relation);
            foreach (IGrouping<string, RelationsItem> group in groups)
            {
                //stringList.Add(RelationsItem.relationDict[group.Key]);
                stringList.AddRange(group.Select(relation => relation.Print()));
                stringList.Add("--------------");
            }
            vnRelationsCB.DataSource = stringList;
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
            if (dropdownlist.SelectedIndex < 0) return;
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
                    if (vn == null) SetNewData(vnid);
                    else
                    {
                        SetDeletedData(); //clear before setting new data
                        SetData(vn);
                    }
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