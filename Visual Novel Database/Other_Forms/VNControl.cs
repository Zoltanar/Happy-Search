using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Happy_Search.Properties;
using Newtonsoft.Json;
using Happy_Apps_Core;
using static Happy_Apps_Core.StaticHelpers;
namespace Happy_Search.Other_Forms
{
    /// <summary>
    /// Form for showing Visual Novel Information
    /// </summary>
    public partial class VNControl : UserControl
    {
        private readonly FormMain _parentForm;
        private const int ScreenshotPadding = 10;
        private ListedVN _displayedVN;
        private ListedProducer _producer;
        private readonly bool _loadFromDb;
        private VNItem.ScreenItem[] _screens;
        private readonly TabPage _tabPage;
        private readonly PictureBox[] _flagBoxes;
        private bool _working;
        private bool Working
        {
            get => _working;
            set
            {
                vnReplyText.Text = value ? "Working..." : "";
                _working = value;
            }
        }

        /// <summary>
        /// Load VN form with specified Visual Novel.
        /// </summary>
        /// <param name="vnItem">Visual Novel to be shown</param>
        /// <param name="parentForm">Parent form</param>
        /// <param name="tabPage">Tab which holds this control</param>
        /// <param name="loadFromDb">Should anime/relations/screenshots also be loaded?</param>
        public VNControl(ListedVN vnItem, FormMain parentForm, TabPage tabPage, bool loadFromDb)
        {
            _parentForm = parentForm;
            _loadFromDb = loadFromDb;
            InitializeComponent();
            vnReplyText.Text = "";
            _tabPage = tabPage;
            _tabPage.Text = TruncateString($@"{vnItem.Title}", 25);
            tagTypeC.Checked = FormMain.GuiSettings.ContentTags;
            tagTypeS.Checked = FormMain.GuiSettings.SexualTags;
            tagTypeT.Checked = FormMain.GuiSettings.TechnicalTags;
            tagTypeC.CheckedChanged += DisplayTags;
            tagTypeS.CheckedChanged += DisplayTags;
            tagTypeT.CheckedChanged += DisplayTags;
            picturePanel.MouseWheel += ScrollScreens;
            _displayedVN = vnItem;
            _flagBoxes = new[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6 };
            Load += LoadForm;
        }

        private async void LoadForm(object sender, EventArgs eventArgs)
        {
            await SetData(_displayedVN);
        }

        private void DisplayTags(object sender, EventArgs e)
        {
            if (sender != null && !DontTriggerEvent)
            {

                var checkBox = (CheckBox)sender;
                DontTriggerEvent = true;
                switch (checkBox.Name)
                {
                    case "tagTypeC":
                        FormMain.GuiSettings.ContentTags = checkBox.Checked;
                        _parentForm.tagTypeC2.Checked = checkBox.Checked;
                        break;
                    case "tagTypeS":
                        FormMain.GuiSettings.SexualTags = checkBox.Checked;
                        _parentForm.tagTypeS2.Checked = checkBox.Checked;
                        break;
                    case "tagTypeT":
                        FormMain.GuiSettings.TechnicalTags = checkBox.Checked;
                        _parentForm.tagTypeT2.Checked = checkBox.Checked;
                        break;
                }
                DontTriggerEvent = false;
                _parentForm.DisplayCommonTagsURT();
                FormMain.GuiSettings.Save();
            }
            if (_displayedVN == null || !_displayedVN.TagList.Any()) vnTagCB.DataSource = new[] { "No Tags Found" };
            else
            {
                var visibleTags = new List<VNItem.TagItem>();
                foreach (var tag in _displayedVN.TagList)
                {
                    switch (tag.Category)
                    {
                        case TagCategory.Content:
                            if (!tagTypeC.Checked) continue;
                            break;
                        case TagCategory.Sexual:
                            if (!tagTypeS.Checked) continue;
                            break;
                        case TagCategory.Technical:
                            if (!tagTypeT.Checked) continue;
                            break;
                    }
                    visibleTags.Add(tag);
                }
                List<string> stringList = visibleTags.Select(x => x.Print(DumpFiles.PlainTags)).ToList();
                stringList.Sort();
                vnTagCB.DataSource = stringList;
            }
        }

        /// <summary>
        /// Replaces the displayed vn with a new vn or refreshes it
        /// </summary>
        /// <param name="vnid">ID of vn to be displayed</param>
        /// <param name="updateAPIStruct">Whether to update anime/relations/screenshots</param>
        private async Task SetNewData(int vnid, bool updateAPIStruct = false)
        {
            ListedVN vn = null;
            await Task.Run(() =>
            {
                LocalDatabase.Open();
                vn = LocalDatabase.GetSingleVN(vnid, Settings.UserID);
                LocalDatabase.Close();
            });
            SetDeletedData(); //clear before setting new data
            await SetData(vn, updateAPIStruct);
        }

        /// <summary>
        /// Display data on specified VN.
        /// </summary>
        /// <param name="vnItem">VN to be displayed</param>
        /// <param name="update">Whether to refetch anime, relations and screenshots</param>
        private async Task SetData(ListedVN vnItem, bool update = false)
        {
            if (vnItem == null || vnItem.VNID <= 0)
            {
                SetDeletedData();
                return;
            }
            _tabPage.Text = TruncateString($@"{vnItem.Title}", 25);
            //prepare data
            _displayedVN = vnItem;
            _producer = LocalDatabase.ProducerList.Find(p => p.Name == _displayedVN.Producer);
            DisplayTags(null, null);
            DisplayVNCharacterTraits(vnItem);
            //set data
            vnName.Text = vnItem.Title;
            vnID.Text = vnItem.VNID.ToString();
            vnKanjiName.Text = vnItem.KanjiTitle;
            producerLabel.Text = vnItem.Producer;
            producerFlag.SetFlagImage(_producer?.Language);
            if (vnItem.Languages != null)
            {
                int boxIndex = 0;
                foreach (var language in vnItem.Languages.All)
                {
                    if (boxIndex > 5) break;
                    _flagBoxes[boxIndex].SetFlagImage(language);
                    boxIndex++;
                }
            }
            if (LocalDatabase.FavoriteProducerList.Exists(fp => fp.Name.Equals(vnItem.Producer)))
            {
                producerLabel.LinkColor = FavoriteProducerBrush.Color;
                producerLabel.ActiveLinkColor = FavoriteProducerBrush.Color;
                producerLabel.VisitedLinkColor = FavoriteProducerBrush.Color;
            }
            vnDate.Text = vnItem.RelDate;
            vnDesc.Text = vnItem.Description;
            vnRating.Text = vnItem.RatingAndVoteCount();
            vnPopularity.Text = $@"Popularity: {vnItem.Popularity:0.00}";
            vnLength.Text = vnItem.Length.GetDescription();
            vnUserStatus.Text = vnItem.UserRelatedStatus();
            var notes = vnItem.GetCustomItemNotes();
            vnNotes.Text = notes.Notes.Length > 0 ? $"Notes: {notes.Notes}" : "No notes.";
            vnUpdate.Text = $@"Days since last updates (Tags/Traits/Stats and full):  {vnItem.UpdatedDate}/{vnItem.DateFullyUpdated}";
            if (update) WriteText(vnReplyText, "Just updated.");
            DisplayGroups(notes);
            SetCoverImage(vnItem);
            //relations, anime and screenshots are only fetched here but are saved to database/disk
            DisplayTextOnScreenshotArea("Getting data from VNDB...");
            while (LocalDatabase.IsBusy()) await Task.Delay(25);
            var loadResult = await LoadFromAPI(vnItem, update);
            switch (loadResult.Status)
            {
                case FetchStatus.Error:
                    LocalDatabase.Open();
                    LocalDatabase.RemoveVisualNovel(vnItem.VNID);
                    LocalDatabase.Close();
                    SetDeletedData();
                    break;
                case FetchStatus.Throttled:
                    vnRelationsCB.DataSource = new List<string> { "Relations cannot be fetched until API connection is ready." };
                    vnAnimeCB.DataSource = new List<string> { "Anime cannot be fetched until API connection is ready." };
                    picturePanel.Controls.Clear();
                    DisplayTextOnScreenshotArea("Screenshots cannot be fetched until API connection is ready");
                    break;
                case FetchStatus.Success:
                    DisplayRelations(loadResult.Relations);
                    DisplayAnime(loadResult.Anime);
                    DisplayScreenshots(loadResult.Screens);
                    break;
            }
        }

        private void SetCoverImage(ListedVN vnItem)
        {
            var imageLoc = vnItem.GetImageLocation();
            if (vnItem.ImageNSFW && !FormMain.GuiSettings.NSFWImages) pcbImages.Image = Resources.nsfw_image;
            else if (File.Exists(imageLoc))
            {
                Image coverImage;
                using (var ms = new MemoryStream(File.ReadAllBytes(imageLoc))) coverImage = Image.FromStream(ms);
                pcbImages.Image = coverImage;
            }
            else pcbImages.Image = Resources.no_image;
        }

        private void DisplayGroups(VNItem.CustomItemNotes notes)
        {
            int groupCount = notes.Groups.Count;
            if (groupCount <= 0)
            {
                vnGroups.Items.Add("No Groups");
                vnGroups.SelectedIndex = 0;
                return;
            }
            vnGroups.Items.Add(groupCount == 1 ? "1 Group" : $"{groupCount} Groups");
            vnGroups.Items.Add("----------");
            foreach (var groupString in notes.Groups)
            {

                vnGroups.Items.Add(groupString);
            }
            vnGroups.SelectedIndex = 0;
        }

        private async Task<APILoadStruct> LoadFromAPI(ListedVN vnItem, bool update)
        {
            var response = new APILoadStruct();
            if (!_loadFromDb)
            {
                response.Status = FetchStatus.Throttled;
                return response;
            }
            var result = Conn.StartQuery(vnReplyText, "LoadVNInfo", false, false, true);
            if (!result)
            {
                response.Status = FetchStatus.Error;
                return response;
            }
            try
            {
                var relationsResponse = await GetVNRelations(vnItem, update);
                response.Status = relationsResponse.Item1;
                if (response.Status == FetchStatus.Error || response.Status == FetchStatus.Throttled) return response;
                response.Relations = relationsResponse.Item2;
                var animeResponse = await GetVNAnime(vnItem, update);
                response.Status = animeResponse.Item1;
                if (response.Status == FetchStatus.Error || response.Status == FetchStatus.Throttled) return response;
                response.Anime = animeResponse.Item2;
                var screenshotsResponse = await GetVNScreenshots(vnItem, update);
                response.Status = screenshotsResponse.Item1;
                if (response.Status == FetchStatus.Error || response.Status == FetchStatus.Throttled) return response;
                response.Screens = screenshotsResponse.Item2;
                return response;
            }
            finally
            {
                _parentForm.ChangeAPIStatus(Conn.Status);
            }
        }

        private void SetDeletedData()
        {
            Text = $@"{ClientName} - (Deleted Title)";
            _displayedVN = null;
            vnName.Text = @"This VN was deleted.";
            vnKanjiName.Text = "";
            producerLabel.Text = "";
            vnDate.Text = "";
            vnUserStatus.Text = "";
            vnLength.Text = "";
            vnID.Text = "";
            vnReplyText.Text = "";
            vnRating.Text = "";
            vnPopularity.Text = "";
            vnNotes.Text = "";
            vnUpdate.Text = "";
            vnGroups.Items.Clear();
            vnTagCB.DataSource = null;
            vnRelationsCB.DataSource = null;
            vnAnimeCB.DataSource = null;
            picturePanel.Controls.Clear();
            pcbImages.Image = Resources.no_image;
        }

        private void DisplayVNCharacterTraits(ListedVN vnItem)
        {
            var vnCharacters = vnItem.GetCharacters(LocalDatabase.CharacterList);
            var stringList = new List<string> { $"{vnCharacters.Length} Characters" };
            foreach (var characterItem in vnCharacters)
            {
                stringList.Add($"Character {characterItem.ID}");
                foreach (var trait in characterItem.Traits)
                {
                    stringList.Add(DumpFiles.PlainTraits.Find(x => x.ID == trait.ID)?.ToString());
                }
                stringList.Add("---------------");
            }
            vnTraitsCB.DataSource = stringList;
            vnTraitsCB.SelectedIndex = 0;
        }

        private async Task<(FetchStatus, VNItem.RelationsItem[])> GetVNRelations(ListedVN vnItem, bool update)
        {
            //relations were fetched before but nothing was found
            if (vnItem.Relations.Equals("Empty") && update == false)
            {
                return (FetchStatus.Success, null);
            }
            //relations were fetched before and something was found
            if (!vnItem.Relations.Equals("") && update == false)
            {
                var loadedRelations = JsonConvert.DeserializeObject<VNItem.RelationsItem[]>(vnItem.Relations);
                return (FetchStatus.Success, loadedRelations);
            }
            //relations haven't been fetched before
            if (Conn.Status != VndbConnection.APIStatus.Ready)
            {
                return (FetchStatus.Throttled, null);
            }
            await Conn.TryQuery($"get vn relations (id = {vnItem.VNID})", "Relations Query Error");
            var root = JsonConvert.DeserializeObject<ResultsRoot<VNItem>>(Conn.LastResponse.JsonPayload);
            if (root.Num == 0)
            {
                return (FetchStatus.Error, null);
            }
            VNItem.RelationsItem[] relations = root.Items[0].Relations;
            await Task.Run(() =>
            {
                LocalDatabase.Open();
                LocalDatabase.AddRelationsToVN(vnItem.VNID, relations);
                LocalDatabase.Close();
            });
            return (FetchStatus.Success, relations);
        }

        private async Task<(FetchStatus, VNItem.AnimeItem[])> GetVNAnime(ListedVN vnItem, bool update)
        {
            //anime was fetched before but nothing was found
            if (vnItem.Anime.Equals("Empty") && update == false)
            {
                return (FetchStatus.Success, null);
            }
            //anime was fetched before and something was found
            if (!vnItem.Anime.Equals("") && update == false)
            {
                var loadedAnime = JsonConvert.DeserializeObject<VNItem.AnimeItem[]>(vnItem.Anime);
                return (FetchStatus.Success, loadedAnime);
            }
            //anime hasn't been fetched before
            if (Conn.Status != VndbConnection.APIStatus.Ready)
            {
                return (FetchStatus.Throttled, null);
            }
            await Conn.TryQuery($"get vn anime (id = {vnItem.VNID})", "Anime Query Error");
            var root = JsonConvert.DeserializeObject<ResultsRoot<VNItem>>(Conn.LastResponse.JsonPayload, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            if (root.Num == 0)
            {
                return (FetchStatus.Error, null);
            }
            VNItem.AnimeItem[] animeItems = root.Items[0].Anime;
            await Task.Run(() =>
            {
                LocalDatabase.Open();
                LocalDatabase.AddAnimeToVN(vnItem.VNID, animeItems);
                LocalDatabase.Close();
            });
            return (FetchStatus.Success, animeItems);
        }

        private async Task<(FetchStatus, VNItem.ScreenItem[])> GetVNScreenshots(ListedVN vnItem, bool update)
        {
            //screenshots were fetched before but nothing was found
            if (vnItem.Screens.Equals("Empty") && update == false) return (FetchStatus.Success, null);
            VNItem.ScreenItem[] screens = null;
            //screenshots were fetched before and something was found
            if (!vnItem.Screens.Equals("") && update == false)
            {
                screens = JsonConvert.DeserializeObject<VNItem.ScreenItem[]>(vnItem.Screens);
                _screens = screens;
                foreach (var screenItem in screens)
                {
                    var screenLocation = screenItem.StoredLocation();
                    if (!File.Exists(screenLocation))
                    {
                        SaveScreenshot(screenItem.Image, screenLocation);
                    }
                }
                return (FetchStatus.Success, screens);
            }
            //screenshots haven't been fetched yet
            if (Conn.Status != VndbConnection.APIStatus.Ready)
            {
                return (FetchStatus.Throttled, null);
            }
            await Conn.TryQuery($"get vn screens (id = {vnItem.VNID})", "Screens Query Error");
            var root = JsonConvert.DeserializeObject<ResultsRoot<VNItem>>(Conn.LastResponse.JsonPayload);
            if (root.Num == 0)
            {
                return (FetchStatus.Error, null);
            }
            await Task.Run(() =>
            {
                screens = root.Items[0].Screens;
                LocalDatabase.Open();
                LocalDatabase.AddScreensToVN(vnItem.VNID, screens);
                LocalDatabase.Close();
            });
            _screens = screens;
            return (FetchStatus.Success, screens);
        }

        private void DisplayRelations(VNItem.RelationsItem[] relationItems)
        {
            if (relationItems == null || !relationItems.Any())
            {
                vnRelationsCB.DataSource = new List<string> { "No relations found." };
                return;
            }
            var titleString = relationItems.Length == 1 ? "1 Relation" : $"{relationItems.Length} Relations";
            var stringList = new List<string> { titleString, "--------------" };
            IEnumerable<IGrouping<string, VNItem.RelationsItem>> groups = relationItems.GroupBy(x => x.Relation);
            foreach (IGrouping<string, VNItem.RelationsItem> group in groups)
            {
                stringList.AddRange(group.Select(relation => relation.Print()));
                stringList.Add("--------------");
            }
            vnRelationsCB.DataSource = stringList;
        }

        private void DisplayAnime(VNItem.AnimeItem[] animeItems)
        {
            if (animeItems == null || !animeItems.Any())
            {
                vnAnimeCB.DataSource = new List<string> { "No anime found." };
                return;
            }
            var titleString = $"{animeItems.Length} Anime";
            var stringList = new List<string> { titleString, "--------------" };
            stringList.AddRange(animeItems.Select(x => x.Print()));
            vnAnimeCB.DataSource = stringList;
        }

        private void DisplayScreenshots(VNItem.ScreenItem[] screenItems)
        {
            picturePanel.Controls.Clear();
            var dist = picturePanel.Size.Width;
            if (screenItems == null || !screenItems.Any())
            {
                DisplayTextOnScreenshotArea("No Screenshots Found");
                _selectedScreen = -1;
                return;
            }
            int imageX = 0;
            foreach (var screen in screenItems)
            {
                if (screen.Nsfw && !FormMain.GuiSettings.NSFWImages)
                {
                    imageX += DrawNSFWImageFitToHeight(picturePanel, 400, imageX) + ScreenshotPadding;
                }
                else
                {
                    imageX += DrawImageFitToWidth(picturePanel, dist, imageX, screen) + ScreenshotPadding;
                }
            }
            _selectedScreen = 1;
        }

        private void DisplayTextOnScreenshotArea(string text)
        {
            picturePanel.Controls.Clear();
            picturePanel.Controls.Add(new Label
            {
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 0),
                Size = new Size(picturePanel.Size.Width, picturePanel.Size.Height),
                Font = new Font(DefaultFont.FontFamily, DefaultFont.SizeInPoints * 1.8f),
                ForeColor = Color.White
            });
        }

        private async void UpdateVN(object sender, EventArgs e)
        {
            if (vnReplyText.Text.Equals("Updating...")) return;
            WriteWarning(vnReplyText, "Updating...");
            Conn.StartQuery(vnReplyText, "Update VN", false, true, true);
            try
            {
                await Conn.GetMultipleVN(new[] { _displayedVN.VNID }, true);
            }
            finally
            {
                _parentForm.ChangeAPIStatus(Conn.Status);
            }
            await SetNewData(_displayedVN.VNID, true);
        }

        private void OpenVndbPage(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (vnID.Text.Equals("")) return;
            Process.Start("http://vndb.org/v" + vnID.Text + '/');
        }

        private async void RelationSelected(object sender, EventArgs e)
        {
            ComboBox dropdownlist = (ComboBox)sender;
            if (dropdownlist.SelectedIndex < 0) return;
            if (dropdownlist.Text.StartsWith("------"))
            {
                dropdownlist.SelectedIndex = 0;
                return;
            }
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
                    LocalDatabase.Open();
                    var vn = LocalDatabase.GetSingleVN(vnid, Settings.UserID);
                    LocalDatabase.Close();
                    if (vn == null) await SetNewData(vnid);
                    else
                    {
                        SetDeletedData(); //clear before setting new data
                        await SetData(vn);
                    }
                    return;
            }
        }

        /// <summary>
        /// Refresh data on currently displayed title.
        /// </summary>
        private async void RefreshData(object sender, EventArgs e)
        {
            if (_displayedVN == null) return;
            await SetNewData(_displayedVN.VNID);
        }

        /// <summary>
        /// DrawImage to fit a space with maxWdith, all image is shown but to stay in ratio, there can be space left below or to the right.
        /// </summary>
        private static int DrawImageFitToWidth(Control control, int maxWidth, int locationX, VNItem.ScreenItem screenItem)
        {
            int maxHeight = control.Height - 20;
            string photoString = screenItem.StoredLocation();
            if (!File.Exists(photoString))
            {
                SaveScreenshot(screenItem.Image, photoString);
            }
            var photo = Image.FromFile(photoString);
            double ratioH = (double)maxHeight / screenItem.Height;
            double ratioW = (double)maxWidth / screenItem.Width;
            double multiRatio = Math.Min(ratioH, ratioW);
            var newHeight = (int)(screenItem.Height * multiRatio);
            var newWidth = (int)(screenItem.Width * multiRatio);
            var pictureBox = new PictureBox
            {
                BackgroundImage = photo,
                Size = new Size(newWidth, newHeight),
                Location = new Point(locationX, 0),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            control.Controls.Add(pictureBox);
            return newWidth;
        }

        private static int DrawNSFWImageFitToHeight(Control control, int height, int locationX)
        {
            var nsfwImage = Resources.nsfw_image;
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

        #region TabPage Controls
        private void CloseButton(object sender, EventArgs e)
        {
            _parentForm.TabsControl.SelectedTab.Dispose();
        }

        private void CloseByEscape(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) CloseButton(null, null);
        }

        private void OnResize(object sender, EventArgs e)
        {
            DisplayScreenshots(_screens);
        }

        private int _selectedScreen;

        /// <summary>
        /// Scroll through screenshots with mousewheel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollScreens(object sender, MouseEventArgs e)
        {
            var pictures = picturePanel.Controls.Count;
            if (e.Delta < 0)
            {
                if (pictures <= _selectedScreen + 1 || _selectedScreen == -1) return;
                picturePanel.ScrollControlIntoView(picturePanel.Controls[_selectedScreen + 1]);
                _selectedScreen++;
            }
            else if (e.Delta > 0)
            {
                if (_selectedScreen <= 0) return;
                picturePanel.ScrollControlIntoView(picturePanel.Controls[_selectedScreen - 1]);
                _selectedScreen--;
            }
        }
        #endregion

        private void DisplayProducerTitles(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _parentForm.TabsControl.SelectTab(0);
            _parentForm.List_Producer(_displayedVN.Producer);
        }

        /// <summary>
        /// Prepare and display context menu for visual novel.
        /// </summary>
        private ContextMenuStrip VNControlContextMenu(ListedVN vn)
        {
            //clearing previous
            foreach (ToolStripMenuItem item in userlistToolStripMenuItem.DropDownItems) item.Checked = false;
            foreach (ToolStripMenuItem item in wishlistToolStripMenuItem.DropDownItems) item.Checked = false;
            foreach (ToolStripMenuItem item in voteToolStripMenuItem.DropDownItems) item.Checked = false;
            userlistToolStripMenuItem.Checked = false;
            wishlistToolStripMenuItem.Checked = false;
            voteToolStripMenuItem.Checked = false;

            //set new
            userlistToolStripMenuItem.Checked = vn.ULStatus > UserlistStatus.None;
            wishlistToolStripMenuItem.Checked = vn.WLStatus > WishlistStatus.None;
            voteToolStripMenuItem.Checked = vn.Vote > 0;
            ((ToolStripMenuItem)userlistToolStripMenuItem.DropDownItems[(int)vn.ULStatus + 1]).Checked = true;
            ((ToolStripMenuItem)wishlistToolStripMenuItem.DropDownItems[(int)vn.WLStatus + 1]).Checked = true;
            if (vn.Vote > 0)
            {
                var vote = (int)Math.Floor(vn.Vote);
                ((ToolStripMenuItem)voteToolStripMenuItem.DropDownItems[vote]).Checked = true;
            }
            else
                ((ToolStripMenuItem)voteToolStripMenuItem.DropDownItems[0]).Checked = true;
            if (vn.ULStatus > UserlistStatus.None)
            {
                addChangeVNNoteToolStripMenuItem.Enabled = true;
                addChangeVNGroupsToolStripMenuItem.Enabled = true;
            }
            if (VNIsByFavoriteProducer(vn))
            {
                addProducerToFavoritesToolStripMenuItem.Enabled = false;
                addProducerToFavoritesToolStripMenuItem.ToolTipText = @"Already in list.";
            }
            return ContextMenuVNControl;
        }

        private async void ChangeVNStatus(object sender, ToolStripItemClickedEventArgs e)
        {

            if (Conn.LogIn != VndbConnection.LogInStatus.YesWithPassword)
            {
                WriteError(vnReplyText, "Not Logged In");
                return;
            }
            var nitem = e.ClickedItem;
            if (nitem == null) return;
            bool success;
            if (_displayedVN == null || Working) return;
            var statusInt = -1;
            switch (nitem.OwnerItem.Text)
            {
                case "Userlist":
                    if (_displayedVN.ULStatus.ToString().Equals(nitem.Text))
                    {
                        WriteText(vnReplyText, $"{TruncateString(_displayedVN.Title, 20)} already has that status.");
                        return;
                    }
                    Working = true;
                    statusInt = (int)(long)Enum.Parse(typeof(UserlistStatus), nitem.Text);
                    success = await _parentForm.ChangeVNStatus(_displayedVN, VNDatabase.ChangeType.UL, statusInt);
                    break;
                case "Wishlist":
                    if (_displayedVN.WLStatus.ToString().Equals(nitem.Text))
                    {
                        WriteText(vnReplyText, $"{TruncateString(_displayedVN.Title, 20)} already has that status.");
                        return;
                    }
                    Working = true;
                    statusInt = (int)(long)Enum.Parse(typeof(WishlistStatus), nitem.Text);
                    success = await _parentForm.ChangeVNStatus(_displayedVN, VNDatabase.ChangeType.WL, statusInt);
                    break;
                case "Vote":
                    double newVoteValue = -1;
                    switch (nitem.Text)
                    {
                        case "None":
                            break;
                        case "Precise Number":
                            StringBuilder input = new StringBuilder();
                            var voteBox = new InputDialogBox(input, "Precise Vote", "Enter vote value:", preciseVote: true).ShowDialog();
                            if (voteBox != DialogResult.OK) return;
                            newVoteValue = double.Parse(input.ToString());
                            statusInt = 1;
                            break;
                        default:
                            newVoteValue = Convert.ToInt32(nitem.Text);
                            statusInt = 1;
                            break;
                    }
                    if (Math.Abs(_displayedVN.Vote - newVoteValue) < 0.001)
                    {
                        WriteText(vnReplyText, $"{TruncateString(_displayedVN.Title, 20)} already has that status.");
                        return;
                    }
                    Working = true;
                    success = await _parentForm.ChangeVNStatus(_displayedVN, VNDatabase.ChangeType.Vote, statusInt, newVoteValue);
                    Working = false;
                    break;
                default:
                    return;
            }
            if (!success)
            {
                Working = false;
                return;
            }
            WriteText(vnReplyText, $"{TruncateString(_displayedVN.Title, 20)} status changed.");
            await SetNewData(_displayedVN.VNID);
            Working = false;
        }

        private async void AddProducer(object sender, EventArgs e)
        {
            if (_displayedVN == null || Working) return;
            Working = true;
            ListedVN[] producerVNs = LocalDatabase.URTList.Where(x => x.Producer.Equals(_displayedVN.Producer)).ToArray();
            double userAverageVote = -1;
            double userDropRate = -1;
            if (producerVNs.Any())
            {
                var finishedCount = producerVNs.Count(x => x.ULStatus == UserlistStatus.Finished);
                var droppedCount = producerVNs.Count(x => x.ULStatus == UserlistStatus.Dropped);
                ListedVN[] producerVotedVNs = producerVNs.Where(x => x.Vote > 0).ToArray();
                userAverageVote = producerVotedVNs.Any() ? producerVotedVNs.Select(x => x.Vote).Average() : -1;
                userDropRate = finishedCount + droppedCount != 0 ?
                    (double)droppedCount / (droppedCount + finishedCount) : -1;
            }
            var producer = LocalDatabase.ProducerList.Find(x => x.Name == _displayedVN.Producer);
            var addProducerList = new List<ListedProducer>
            {
                new ListedProducer(_displayedVN.Producer, producerVNs.Length, DateTime.UtcNow,
                    producer.ID, producer.Language,
                    userAverageVote, (int) Math.Round(userDropRate*100))
            };
            LocalDatabase.BeginTransaction();
            LocalDatabase.InsertFavoriteProducers(addProducerList, Settings.UserID);
            LocalDatabase.EndTransaction();
            ReloadListsFromDb();
            _parentForm.LoadFPListToGui();
            WriteText(vnReplyText, $"{_displayedVN.Producer} added to list.");
            await SetNewData(_displayedVN.VNID);

            Working = false;
        }

        private void OptionsMenu(object sender, EventArgs e)
        {
            if (Working)
            {
                vnReplyText.Text = @"Please wait...";
                return;
            }
            if (_displayedVN == null) return;
            VNControlContextMenu(_displayedVN).Show(Cursor.Position.X, Cursor.Position.Y + statusChangeButton.Height);
        }

        private void ShowProducerTitles(object sender, EventArgs e)
        {
            DisplayProducerTitles(null, null);
        }


        /// <summary>
        /// Add note to title in user's vnlist.
        /// </summary>
        private async void AddNote(object sender, EventArgs e)
        {
            if (Working) return;
            if (Conn.LogIn != VndbConnection.LogInStatus.YesWithPassword)
            {
                WriteError(vnReplyText, "Not Logged In");
                return;
            }
            if (_displayedVN == null) return;
            VNItem.CustomItemNotes itemNotes = _displayedVN.GetCustomItemNotes();
            StringBuilder notesSb = new StringBuilder(itemNotes.Notes);
            var result = new InputDialogBox(notesSb, "Add Note to Title", "Enter Note:").ShowDialog();
            if (result != DialogResult.OK) return;
            if (notesSb.ToString().Contains('\n'))
            {
                WriteError(vnReplyText, "Note cannot contain newline characters.");
                return;
            }
            itemNotes.Notes = notesSb.ToString();
            await UpdateItemNotes("Added note to title", _displayedVN.VNID, itemNotes);

        }

        /// <summary>
        /// Add title in user's vnlist to a user-defined group.
        /// </summary>
        private async void AddGroup(object sender, EventArgs e)
        {
            if (Working) return;
            if (Conn.LogIn != VndbConnection.LogInStatus.YesWithPassword)
            {
                WriteError(vnReplyText, "Not Logged In");
                return;
            }
            if (_displayedVN == null) return;
            VNItem.CustomItemNotes itemNotes = _displayedVN.GetCustomItemNotes();
            var result = new ListDialogBox(itemNotes.Groups, "Add Title to Groups", $"{_displayedVN.Title} is in groups:").ShowDialog();
            if (result != DialogResult.OK) return;
            if (itemNotes.Groups.Any(group => group.Contains('\n')))
            {
                WriteError(vnReplyText, "Group name cannot contain newline characters.");
                return;
            }
            await UpdateItemNotes("Added title to group(s).", _displayedVN.VNID, itemNotes);
        }

        /// <summary>
        /// Send query to VNDB to update a VN's notes and if successful, update database.
        /// </summary>
        /// <param name="replyMessage">Message to be printed if query is successful</param>
        /// <param name="vnid">ID of VN</param>
        /// <param name="itemNotes">Object containing new data to replace old</param>
        private async Task UpdateItemNotes(string replyMessage, int vnid, VNItem.CustomItemNotes itemNotes)
        {
            if (Working) return;
            Working = true;
            var result = Conn.StartQuery(vnReplyText, "Update Item Notes", false, false, true);
            if (!result) return;
            string serializedNotes = itemNotes.Serialize();
            var query = $"set vnlist {vnid} {{\"notes\":\"{serializedNotes}\"}}";
            var apiResult = await Conn.TryQuery(query, "UIN Query Error");
            if (!apiResult) return;
            LocalDatabase.Open();
            LocalDatabase.AddNoteToVN(vnid, serializedNotes, Settings.UserID);
            LocalDatabase.Close();
            ReloadListsFromDb();
            _parentForm.LoadVNListToGui();
            WriteText(vnReplyText, replyMessage);
            _parentForm.ChangeAPIStatus(Conn.Status);
            await SetNewData(_displayedVN.VNID);
            Working = false;
        }

        private void VnGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (vnGroups.SelectedIndex)
            {
                case 0:
                    return;
                case 1:
                    vnGroups.SelectedIndex = 0;
                    return;
                default:
                    _parentForm.TabsControl.SelectTab(0);
                    _parentForm.ListByCBQuery.Text = (string)vnGroups.SelectedItem;
                    _parentForm.List_Group(null, null);
                    return;
            }
        }

        /// <summary>
        /// Class holding VN information fetched via API on VN Form Load.
        /// </summary>
        private class APILoadStruct
        {
            /// <summary>
            /// Status of info fetch (error, success, throttled)
            /// </summary>
            public FetchStatus Status;
            /// <summary>
            /// VN Relations
            /// </summary>
            public VNItem.RelationsItem[] Relations;
            /// <summary>
            /// VN Anime
            /// </summary>
            public VNItem.AnimeItem[] Anime;
            /// <summary>
            /// VN Screenshots
            /// </summary>
            public VNItem.ScreenItem[] Screens;
        }

        /// <summary>
        /// Status of info fetch (error, success, throttled)
        /// </summary>
        private enum FetchStatus
        {
            /// <summary>
            /// There was an error in fetching (probably a deleted VN)
            /// </summary>
            Error = 0,
            /// <summary>
            /// Fetch was successful
            /// </summary>
            Success = 1,
            /// <summary>
            /// Fetch could not be done because connection was not ready
            /// </summary>
            Throttled = 2
        }
    }
}