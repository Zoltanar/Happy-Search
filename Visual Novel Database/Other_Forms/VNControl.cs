using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Happy_Search.Properties;
using Newtonsoft.Json;
using static Happy_Search.StaticHelpers;

namespace Happy_Search.Other_Forms
{
    /// <summary>
    /// Form for showing Visual Novel Information
    /// </summary>
    public partial class VNControl : UserControl
    {
        // ReSharper disable InconsistentNaming
        // ReSharper restore InconsistentNaming
        private readonly FormMain _parentForm;
        private const int ScreenshotPadding = 10;
        private ListedVN _displayedVN;
        private readonly bool _loadFromDb;
        /// <summary>
        /// Load VN form with specified Visual Novel.
        /// </summary>
        /// <param name="vnItem">Visual Novel to be shown</param>
        /// <param name="parentForm">Parent form</param>
        /// <param name="loadFromDb">Should anime/relations/screenshots also be loaded?</param>
        public VNControl(ListedVN vnItem, FormMain parentForm, bool loadFromDb = true)
        {
            _parentForm = parentForm;
            _loadFromDb = loadFromDb;
            InitializeComponent();
            Text = $@"{vnItem.Title} - {FormMain.ClientName}";
            tagTypeC.Checked = FormMain.Settings.ContentTags;
            tagTypeS.Checked = FormMain.Settings.SexualTags;
            tagTypeT.Checked = FormMain.Settings.TechnicalTags;
            tagTypeC.CheckedChanged += DisplayTags;
            tagTypeS.CheckedChanged += DisplayTags;
            tagTypeT.CheckedChanged += DisplayTags;
            _displayedVN = vnItem;
            Load += LoadForm;
        }

        private async void LoadForm(object sender, EventArgs eventArgs)
        {
            await SetData(_displayedVN);
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
                        FormMain.Settings.ContentTags = checkBox.Checked;
                        _parentForm.tagTypeC.Checked = checkBox.Checked;
                        _parentForm.tagTypeC2.Checked = checkBox.Checked;
                        break;
                    case "tagTypeS":
                        FormMain.Settings.SexualTags = checkBox.Checked;
                        _parentForm.tagTypeS.Checked = checkBox.Checked;
                        _parentForm.tagTypeS2.Checked = checkBox.Checked;
                        break;
                    case "tagTypeT":
                        FormMain.Settings.TechnicalTags = checkBox.Checked;
                        _parentForm.tagTypeT.Checked = checkBox.Checked;
                        _parentForm.tagTypeT2.Checked = checkBox.Checked;
                        break;
                }
                _parentForm.DontTriggerEvent = false;
                _parentForm.DisplayCommonTags(null, null);
                _parentForm.DisplayCommonTagsURT(null, null);
                FormMain.Settings.Save();
            }
            if (_displayedVN == null || _displayedVN.Tags == string.Empty) vnTagCB.DataSource = "No Tags Found";
            else
            {
                _displayedVN.SetTags(_parentForm.PlainTags);
                List<TagItem> allTags = _displayedVN.TagList;
                var visibleTags = new List<TagItem>();
                foreach (var tag in allTags)
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

        /// <summary>
        /// Replaces the displayed vn with a new vn (or updates it)
        /// </summary>
        /// <param name="vnid">ID of vn to be displayed</param>
        private async Task SetNewData(int vnid)
        {
            await GetSingleVN(vnid, vnUpdateLink);
            var root = JsonConvert.DeserializeObject<VNRoot>(_parentForm.Conn.LastResponse.JsonPayload);
            if (root.Num == 0)
            {
                SetDeletedData();
                return;
            }
            ListedVN vn = null;
            await Task.Run(() =>
            {
                _parentForm.DBConn.Open();
                vn = _parentForm.DBConn.GetSingleVN(vnid, FormMain.Settings.UserID);
                _parentForm.DBConn.Close();
            });
            SetDeletedData(); //clear before setting new data
            await SetData(vn);
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
            Text = $@"{vnItem.Title} - {FormMain.ClientName}";
            //prepare data
            _displayedVN = vnItem;
            var ext = Path.GetExtension(vnItem.ImageURL);
            var imageLoc = $"{VNImagesFolder}{vnItem.VNID}{ext}";
            DisplayTags(null, null);
            DisplayVNCharacterTraits(vnItem);
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
            if (vnItem.ImageNSFW && !FormMain.Settings.NSFWImages) pcbImages.Image = Resources.nsfw_image;
            else if (File.Exists(imageLoc)) pcbImages.ImageLocation = imageLoc;
            else pcbImages.Image = Resources.no_image;
            //relations, anime and screenshots are only fetched here but are saved to database/disk
            var loadResult = await LoadFromAPI(vnItem, update);
            switch (loadResult.Status)
            {
                case FetchStatus.Error:
                    _parentForm.DBConn.Open();
                    _parentForm.DBConn.RemoveVisualNovel(vnItem.VNID);
                    _parentForm.DBConn.Close();
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

        private async Task<APILoadStruct> LoadFromAPI(ListedVN vnItem, bool update)
        {
            var response = new APILoadStruct();
            if (!_loadFromDb)
            {
                response.Status = FetchStatus.Throttled;
                return response;
            }
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

        private void SetDeletedData()
        {
            Text = $@"{FormMain.ClientName} - (Deleted Title)";
            _displayedVN = null;
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
            var vnCharacters = vnItem.GetCharacters(_parentForm.CharacterList);
            var stringList = new List<string> { $"{vnCharacters.Length} Characters" };
            foreach (var characterItem in vnCharacters)
            {
                stringList.Add($"Character {characterItem.ID}");
                foreach (var trait in characterItem.Traits)
                {
                    stringList.Add(_parentForm.PlainTraits.Find(x => x.ID == trait.ID)?.ToString());
                }
                stringList.Add("---------------");
            }
            vnTraitsCB.DataSource = stringList;
            vnTraitsCB.SelectedIndex = 0;
        }

        private async Task<Tuple<FetchStatus, RelationsItem[]>> GetVNRelations(ListedVN vnItem, bool update)
        {
            //relations were fetched before but nothing was found
            if (vnItem.Relations.Equals("Empty") && update == false)
            {
                return new Tuple<FetchStatus, RelationsItem[]>(FetchStatus.Success, null);
            }
            //relations were fetched before and something was found
            if (!vnItem.Relations.Equals("") && update == false)
            {
                var loadedRelations = JsonConvert.DeserializeObject<RelationsItem[]>(vnItem.Relations);
                return new Tuple<FetchStatus, RelationsItem[]>(FetchStatus.Success, loadedRelations);
            }
            //relations haven't been fetched before
            if (_parentForm.Conn.Status != VndbConnection.APIStatus.Ready)
            {
                return new Tuple<FetchStatus, RelationsItem[]>(FetchStatus.Throttled, null);
            }
            await _parentForm.TryQuery($"get vn relations (id = {vnItem.VNID})", "Relations Query Error", vnUpdateLink);
            var root = JsonConvert.DeserializeObject<VNRoot>(_parentForm.Conn.LastResponse.JsonPayload);
            if (root.Num == 0)
            {
                return new Tuple<FetchStatus, RelationsItem[]>(FetchStatus.Error, null);
            }
            RelationsItem[] relations = root.Items[0].Relations;
            await Task.Run(() =>
            {
                _parentForm.DBConn.Open();
                _parentForm.DBConn.AddRelationsToVN(vnItem.VNID, relations);
                _parentForm.DBConn.Close();
            });
            return new Tuple<FetchStatus, RelationsItem[]>(FetchStatus.Success, relations);
        }

        private async Task<Tuple<FetchStatus, AnimeItem[]>> GetVNAnime(ListedVN vnItem, bool update)
        {
            //anime was fetched before but nothing was found
            if (vnItem.Anime.Equals("Empty") && update == false)
            {
                return new Tuple<FetchStatus, AnimeItem[]>(FetchStatus.Success, null);
            }
            //anime was fetched before and something was found
            if (!vnItem.Anime.Equals("") && update == false)
            {
                var loadedAnime = JsonConvert.DeserializeObject<AnimeItem[]>(vnItem.Anime);
                return new Tuple<FetchStatus, AnimeItem[]>(FetchStatus.Success, loadedAnime);
            }
            //anime hasn't been fetched before
            if (_parentForm.Conn.Status != VndbConnection.APIStatus.Ready)
            {
                return new Tuple<FetchStatus, AnimeItem[]>(FetchStatus.Throttled, null);
            }
            await _parentForm.TryQuery($"get vn anime (id = {vnItem.VNID})", "Anime Query Error", vnUpdateLink);
            var root = JsonConvert.DeserializeObject<VNRoot>(_parentForm.Conn.LastResponse.JsonPayload, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            if (root.Num == 0)
            {
                return new Tuple<FetchStatus, AnimeItem[]>(FetchStatus.Error, null);
            }
            AnimeItem[] animeItems = root.Items[0].Anime;
            await Task.Run(() =>
            {
                _parentForm.DBConn.Open();
                _parentForm.DBConn.AddAnimeToVN(vnItem.VNID, animeItems);
                _parentForm.DBConn.Close();
            });
            return new Tuple<FetchStatus, AnimeItem[]>(FetchStatus.Success, animeItems);
        }

        private async Task<Tuple<FetchStatus, ScreenItem[]>> GetVNScreenshots(ListedVN vnItem, bool update)
        {
            //screenshots were fetched before but nothing was found
            if (vnItem.Screens.Equals("Empty") && update == false) return new Tuple<FetchStatus, ScreenItem[]>(FetchStatus.Success, null);
            ScreenItem[] screens = null;
            //screenshots were fetched before and something was found
            if (!vnItem.Screens.Equals("") && update == false)
            {
                screens = JsonConvert.DeserializeObject<ScreenItem[]>(vnItem.Screens);
                return new Tuple<FetchStatus, ScreenItem[]>(FetchStatus.Success, screens);
            }
            //screenshots haven't been fetched yet
            if (_parentForm.Conn.Status != VndbConnection.APIStatus.Ready)
            {
                return new Tuple<FetchStatus, ScreenItem[]>(FetchStatus.Throttled, null);
            }
            await _parentForm.TryQuery($"get vn screens (id = {vnItem.VNID})", "Screens Query Error", vnUpdateLink);
            var root = JsonConvert.DeserializeObject<VNRoot>(_parentForm.Conn.LastResponse.JsonPayload);
            if (root.Num == 0)
            {
                return new Tuple<FetchStatus, ScreenItem[]>(FetchStatus.Error, null);
            }
            await Task.Run(() =>
            {
                screens = root.Items[0].Screens;
                _parentForm.DBConn.Open();
                _parentForm.DBConn.AddScreensToVN(vnItem.VNID, screens);
                _parentForm.DBConn.Close();
            });
            return new Tuple<FetchStatus, ScreenItem[]>(FetchStatus.Success, screens);
        }

        private void DisplayRelations(RelationsItem[] relationItems)
        {
            if (relationItems == null || !relationItems.Any())
            {
                vnRelationsCB.DataSource = new List<string> { "No relations found." };
                return;
            }
            var titleString = relationItems.Length == 1 ? "1 Relation" : $"{relationItems.Length} Relations";
            var stringList = new List<string> { titleString, "--------------" };
            IEnumerable<IGrouping<string, RelationsItem>> groups = relationItems.GroupBy(x => x.Relation);
            foreach (IGrouping<string, RelationsItem> group in groups)
            {
                //stringList.Add(RelationsItem.relationDict[group.Key]);
                stringList.AddRange(group.Select(relation => relation.Print()));
                stringList.Add("--------------");
            }
            vnRelationsCB.DataSource = stringList;
        }

        private void DisplayAnime(AnimeItem[] animeItems)
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

        private void DisplayScreenshots(ScreenItem[] screenItems)
        {
            picturePanel.Controls.Clear();
            if (screenItems == null || !screenItems.Any())
            {
                DisplayTextOnScreenshotArea("No Screenshots Found");
                return;
            }
            int imageX = 0;
            foreach (var screen in screenItems)
            {
                if (screen.Nsfw && !FormMain.Settings.NSFWImages)
                {
                    imageX += DrawNSFWImageFitToHeight(picturePanel, 400, imageX) + ScreenshotPadding;
                }
                else
                {
                    imageX += DrawImageFitToHeight(picturePanel, 400, imageX, screen) + ScreenshotPadding;
                }
            }
        }

        private void DisplayTextOnScreenshotArea(string text)
        {
            picturePanel.Controls.Add(new Label
            {
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 0),
                Size = new Size(picturePanel.Size.Width, picturePanel.Size.Height),
                Font = new Font(DefaultFont.FontFamily, DefaultFont.SizeInPoints * 1.8f)
            });
        }
        private async void vnUpdateLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (vnUpdateLink.Text.Equals(Resources.vn_updated)) return;
            if (vnUpdateLink.Text.Equals("")) return;
            var vnItem = await _parentForm.UpdateSingleVN(Convert.ToInt32(vnID.Text), vnUpdateLink);
            await SetData(vnItem, true);
        }

        private void vnID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (vnID.Text.Equals("")) return;
            Process.Start("http://vndb.org/v" + vnID.Text + '/');
        }

        private async void RelationSelected(object sender, EventArgs e)
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
                    var vn = _parentForm.DBConn.GetSingleVN(vnid, FormMain.Settings.UserID);
                    _parentForm.DBConn.Close();
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
            await SetData(_displayedVN);
        }

        /// <summary>
        /// Get data about a single visual novel.
        /// </summary>
        /// <param name="vnid">ID of VN to be retrieved.</param>
        /// <param name="replyLabel">Label where reply will be printed.</param>
        internal async Task GetSingleVN(int vnid, Label replyLabel)
        {
            var result = _parentForm.StartQuery(replyLabel, "Get Single VN");
            if (!result) return;
            string singleVNQuery = $"get vn basic,details,tags,stats (id = {vnid})";
            result = await _parentForm.TryQuery(singleVNQuery, Resources.svn_query_error, replyLabel);

            if (!result) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(_parentForm.Conn.LastResponse.JsonPayload);
            if (vnRoot.Num == 0)
            {
                //this vn has been deleted (or something along those lines)
                await Task.Run(() =>
                {
                    _parentForm.DBConn.Open();
                    _parentForm.DBConn.RemoveVisualNovel(vnid);
                    _parentForm.DBConn.Close();
                });
                return;
            }
            var vnItem = vnRoot.Items[0];
            SaveImage(vnItem);
            var relProducer = await _parentForm.GetDeveloper(vnid, Resources.svn_query_error, replyLabel);
            //TODO
            var gpResult = await _parentForm.GetProducer(relProducer, Resources.svn_query_error, replyLabel);
            if (!gpResult.Item1) return;
            await _parentForm.GetCharactersForMultipleVN(new[] { vnid }, replyLabel);
            await Task.Run(() =>
            {
                _parentForm.DBConn.Open();
                _parentForm.DBConn.UpsertSingleVN(vnItem, relProducer);
                if (gpResult.Item2 != null) _parentForm.DBConn.InsertProducer(gpResult.Item2, true);
                _parentForm.DBConn.Close();
            });
            _parentForm.ChangeAPIStatus(_parentForm.Conn.Status);
        }

        private static int DrawImageFitToHeight(Control control, int height, int locationX, ScreenItem screenItem)
        {
            string photoString = screenItem.StoredLocation();
            if (!File.Exists(photoString))
            {
                SaveScreenshot(screenItem.Image, photoString);
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
            public RelationsItem[] Relations;
            /// <summary>
            /// VN Anime
            /// </summary>
            public AnimeItem[] Anime;
            /// <summary>
            /// VN Screenshots
            /// </summary>
            public ScreenItem[] Screens;
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

        #region Window Controls
        private void CloseButton(object sender, EventArgs e)
        {
            _parentForm.tabControl1.SelectedTab.Dispose();
        }

        private void CloseByEscape(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) CloseButton(null,null);
        }
        #endregion
    }
}