﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Happy_Search.Properties;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Happy_Search
{
    /// <summary>
    ///     Main Form of application, contains global variables.
    /// </summary>
    public partial class FormMain : Form
    {
        //constants / definables
        private const string VNImagesFolder = "vnImages\\";
        internal const string VNScreensFolder = "vnScreens\\";
        private const string DBStatsXml = "dbs.xml";
        private const string LogFile = "message.log";
        private const string TagsURL = "http://vndb.org/api/tags.json.gz";
        private const string TagsJsonGz = "tags.json.gz";
        private const string TagsJson = "tags.json";
        private const string TraitsURL = "http://vndb.org/api/traits.json.gz";
        private const string TraitsJsonGz = "traits.json.gz";
        private const string TraitsJson = "traits.json";
        private const string TagTypeAll = "checkBox";
        private const string TagTypeUrt = "mctULLabel";
        internal const string ContentTag = "cont";
        internal const string SexualTag = "ero";
        internal const string TechnicalTag = "tech";
        internal const string ClientName = "Happy Search By Zolty";
        internal const string ClientVersion = "1.3.4";
        internal const string APIVersion = "2.25";
        private const int APIMaxResults = 25;
        internal static readonly string MaxResultsString = $"\"results\":{APIMaxResults}";
        private const int LabelFadeTime = 5000; //ms for text to disappear (not actual fade)
        private const string MainXmlFile = "saved_objects.xml";
        private static readonly Color ErrorColor = Color.Red;
        internal static readonly Color NormalColor = SystemColors.ControlLightLight;
        internal static readonly Color NormalLinkColor = Color.FromArgb(0, 192, 192);
        private static readonly Color WarningColor = Color.DarkKhaki;

        internal readonly VndbConnection Conn = new VndbConnection();
        internal readonly DbHelper DBConn;
        private Func<ListedVN, bool> _currentList = x => true;
        private string _currentListLabel;
        internal bool DontTriggerEvent; //used to skip indexchanged events
        private List<ListedProducer> _producerList; //contains all producers in local database
        internal List<CharacterItem> CharacterList; //contains all producers in local database
        private List<ListedVN> _vnList; //contains all vns in local database
        private ushort _vnsAdded;
        private ushort _vnsSkipped;
        internal List<WrittenTag> PlainTags; //Contains all tags as in tags.json
        internal List<WrittenTrait> PlainTraits; //Contains all tags as in tags.json
        internal List<ListedVN> URTList; //contains all user-related vns
        internal int UserID; //id of current user

        /*credits and resources
        ObjectListView by Phillip Piper (GPLv3)from http://www.codeproject.com/Articles/16009/A-Much-Easier-to-Use-ListView
        (slightly modified) A Pretty Good Splash Screen in C# by Tom Clement (CPOL) from http://www.codeproject.com/Articles/5454/A-Pretty-Good-Splash-Screen-in-C
        (reasonably modified) VndbClient by FredTheBarber, for connection/queries to VNDB API https://github.com/FredTheBarber/VndbClient
        */

        #region Initialization

        /// <summary>
        ///     Constructor for Main Application Form.
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            SplashScreen.SplashScreen.SetStatus("Initializing Controls...");
            {
                DontTriggerEvent = true;
                ulStatusDropDown.SelectedIndex = 0;
                wlStatusDropDown.SelectedIndex = 0;
                customTagFilters.SelectedIndex = 0;
                viewPicker.SelectedIndex = 0;
                URTToggleBox.SelectedIndex = 0;
                UnreleasedToggleBox.SelectedIndex = 0;
                BlacklistToggleBox.SelectedIndex = 0;
                DontTriggerEvent = false;
                replyText.Text = "";
                userListReply.Text = "";
                resultLabel.Text = "";
                loginReply.Text = "";
                prodReply.Text = "";
                tagReply.Text = "";
                traitReply.Text = "";
                mctLoadingLabel.Text = "";
                checkBox1.Visible = false;
                checkBox2.Visible = false;
                checkBox3.Visible = false;
                checkBox4.Visible = false;
                checkBox5.Visible = false;
                checkBox6.Visible = false;
                checkBox7.Visible = false;
                checkBox8.Visible = false;
                checkBox9.Visible = false;
                checkBox10.Visible = false;
                tileOLV.ItemRenderer = new VNTileRenderer();
                File.Create(LogFile).Close();
                aboutTextBox.Text =
                    $@"{ClientName} (Version {ClientVersion}, for VNDB API {APIVersion})
VNDB API Client for filtering/organizing and finding visual novels.

Resources:
ObjectListView by Phillip Piper (GPLv3)
http://www.codeproject.com/Articles/16009/A-Much-Easier-to-Use-ListView
(slightly modified) A Pretty Good Splash Screen in C# by Tom Clement (CPOL)
http://www.codeproject.com/Articles/5454/A-Pretty-Good-Splash-Screen-in-C
(reasonably modified) VndbClient by FredTheBarber
https://github.com/FredTheBarber/VndbClient";
            }
            SplashScreen.SplashScreen.SetStatus("Loading User Settings...");
            {
                UserID = Settings.Default.UserID;
                tagTypeC.Checked = Settings.Default.TagTypeC;
                tagTypeS.Checked = Settings.Default.TagTypeS;
                tagTypeT.Checked = Settings.Default.TagTypeT;
                tagTypeC2.Checked = Settings.Default.TagTypeC;
                tagTypeS2.Checked = Settings.Default.TagTypeS;
                tagTypeT2.Checked = Settings.Default.TagTypeT;
                nsfwToggle.Checked = Settings.Default.ShowNSFWImages;
                autoUpdateURTBox.Checked = Settings.Default.AutoUpdateURT;
                yearLimitBox.Checked = Settings.Default.Limit10Years;
            }
            SplashScreen.SplashScreen.SetStatus("Loading Tag and Trait files...");
            {
                LogToFile(
                    $"Tagdump Update = {Settings.Default.DumpfilesUpdate}, days since = {DaysSince(Settings.Default.DumpfilesUpdate)}");
                if (DaysSince(Settings.Default.DumpfilesUpdate) > 2 || DaysSince(Settings.Default.DumpfilesUpdate) == -1)
                {
                    GetNewTagdump();
                    GetNewTraitdump();
                }
                else
                {
                    LoadTagdump();
                    LoadTraitdump();
                }
                var tagSource = new AutoCompleteStringCollection();
                tagSource.AddRange(PlainTags.Select(v => v.Name).ToArray());
                tagSearchBox.AutoCompleteCustomSource = tagSource;
                var traitRootIDs = PlainTraits.Select(x => x.TopmostParent).Distinct();
                var traitRoots = PlainTraits.Where(x => traitRootIDs.Contains(x.ID));
                foreach (var writtenTrait in traitRoots)
                {
                    traitRootsDropdown.Items.Add(writtenTrait.Name);
                }
            }
            SplashScreen.SplashScreen.SetStatus("Connecting to SQLite Database...");
            {
                DBConn = new DbHelper();
            }
            SplashScreen.SplashScreen.SetStatus("Loading Data from Database...");
            {
                DBConn.Open();
                _vnList = DBConn.GetAllTitles(UserID);
                _producerList = DBConn.GetAllProducers();
                CharacterList = DBConn.GetAllCharacters();
                URTList = DBConn.GetUserRelatedTitles(UserID);
                DBConn.Close();
                LoadFavoriteProducerList();
                LogToFile("VN Items= " + _vnList.Count);
                LogToFile("Producers= " + _producerList.Count);
                LogToFile("Characters= " + CharacterList.Count);
                LogToFile("UserRelated Items= " + URTList.Count);
                var producerFilterSource = new AutoCompleteStringCollection();
                producerFilterSource.AddRange(_producerList.Select(v => v.Name).ToArray());
                ProducerListBox.AutoCompleteCustomSource = producerFilterSource;
            }
            SplashScreen.SplashScreen.SetStatus("Updating User Stats...");
            {
                UpdateUserStats();
            }
            SplashScreen.SplashScreen.SetStatus("Adding VNs to Object Lists...");
            {
                tileOLV.SetObjects(_vnList);
                tileOLV.Sort(tileColumnDate, SortOrder.Descending);
                _currentListLabel = "All Titles";
            }
            SplashScreen.SplashScreen.SetStatus("Loading Custom Filters...");
            {
                var xml = File.Exists(MainXmlFile) ? XmlHelper.FromXmlFile<MainXml>(MainXmlFile) : new MainXml();
                _customTagFilters = xml.CustomTagFilters;
                foreach (var filter in _customTagFilters) customTagFilters.Items.Add(filter.Name);
                _customTraitFilters = xml.CustomTraitFilters;
                foreach (var filter in _customTraitFilters) customTraitFilters.Items.Add(filter.Name);
                DontTriggerEvent = true;
                URTToggleBox.SelectedIndex = (int)xml.XmlToggles.URTToggleSetting;
                Toggles.URTToggleSetting = (ToggleSetting)URTToggleBox.SelectedIndex;
                UnreleasedToggleBox.SelectedIndex = (int)xml.XmlToggles.UnreleasedToggleSetting;
                Toggles.UnreleasedToggleSetting = (ToggleSetting)UnreleasedToggleBox.SelectedIndex;
                BlacklistToggleBox.SelectedIndex = (int)xml.XmlToggles.BlacklistToggleSetting;
                Toggles.BlacklistToggleSetting = (ToggleSetting)BlacklistToggleBox.SelectedIndex;
                DontTriggerEvent = false;
                ApplyListFilters();
            }
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            InitAPIConnection();
            SplashScreen.SplashScreen.SetStatus("Loading DBStats...");
            {
                LogToFile(
                    $"dbstats Update = {Settings.Default.DBStatsUpdate}, days since = {DaysSince(Settings.Default.DBStatsUpdate)}");
                if (DaysSince(Settings.Default.DBStatsUpdate) > 2 || DaysSince(Settings.Default.DBStatsUpdate) == -1)
                    GetNewDBStats();
                else LoadDBStats();
            }
            SplashScreen.SplashScreen.CloseForm();
            if (!Settings.Default.AutoUpdateURT || UserID <= 0) return;
            LogToFile("Checking if URT Update is due...");
            LogToFile($"URTUpdate= {Settings.Default.URTUpdate}, days since = {DaysSince(Settings.Default.URTUpdate)}");
            if (DaysSince(Settings.Default.URTUpdate) > 2 || DaysSince(Settings.Default.URTUpdate) == -1)
            {
                LogToFile("Updating User Related Titles...");
                Task.Run(UpdateURT);
                Settings.Default.URTUpdate = DateTime.UtcNow;
                Settings.Default.Save();
            }
            else
            {
                LogToFile("Update not needed.");
            }
        }

        private void InitAPIConnection()
        {
            SplashScreen.SplashScreen.SetStatus("Logging into VNDB API...");
            {
                Conn.Open();
                if (Conn.Status == VndbConnection.APIStatus.Error)
                {
                    ChangeAPIStatus(Conn.Status);
                    return;
                }
                //login with credentials if setting is enabled and credentials exist, otherwise login without credentials
                if (Settings.Default.RememberCredentials)
                {
                    LogToFile("Attempting log in with credentials");
                    KeyValuePair<string, char[]> credentials = LoadCredentials();
                    if (credentials.Value != null)
                    {
                        APILoginWithCredentials(credentials);
                        return;
                    }
                    APILogin();
                    return;
                }
                LogToFile("Attempting log in without credentials");
                APILogin();
            }
        }

        /// <summary>
        ///     Load Login Form for user to log in.
        /// </summary>
        private void LogInDialog(object sender, EventArgs e)
        {
            DialogResult = new LoginForm(this).ShowDialog();
            if (DialogResult != DialogResult.OK) return;
            Settings.Default.UserID = UserID;
            Settings.Default.Save();
            UpdateUserStats();
            ReloadLists();
            RefreshVNList();
            LoadFavoriteProducerList();
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            Conn.Close();
        }

        #endregion

        #region Settings Area / Get Started


        /// <summary>
        ///     Update titles to include all fields in latest version of Happy Search.
        /// </summary>
        private async void UpdateTitlesToLatestVersionClick(object sender, EventArgs e)
        {
            //popularity, rating and votecount were added, check for votecount
            int[] listOfTitlesFromOldVersions = _vnList.Where(x => x.VoteCount == -1).Select(x => x.VNID).ToArray();
            var messageBox =
                MessageBox.Show(
                    $@"{listOfTitlesFromOldVersions.Length} need to be updated, if this is a large number (over 2000), it may take a while, are you sure?",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (messageBox != DialogResult.Yes) return;
            await UpdateTitlesToLatestVersion(listOfTitlesFromOldVersions);
            ReloadLists();
            RefreshVNList();
            var messageBox2 =
                MessageBox.Show(
                    @"Do you wish to get character data about all VNs?
You only need to this once and only if you used Happy Search prior to version 1.3
This will take a long time if you have a lot of titles in your local database.",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (messageBox2 == DialogResult.Yes)
            {
                await GetCharactersForMultipleVN(_vnList.Select(x => x.VNID).ToArray(), userListReply);
            }
            ReloadLists();
            RefreshVNList();
            WriteText(userListReply, $"Updated {_vnsAdded} titles to latest version.");
        }

        /// <summary>
        ///     Update tags of titles that haven't been updated in over 7 days.
        /// </summary>
        private async void UpdateTagsAndTraitsClick(object sender, EventArgs e)
        {
            int[] listOfTitlesToUpdate = _vnList.Where(x => x.UpdatedDate > 7).Select(x => x.VNID).ToArray();
            var messageBox =
                MessageBox.Show(
                    $@"{listOfTitlesToUpdate.Length} need to be updated, if this is a large number (over 1000), it may take a while, are you sure?",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (messageBox != DialogResult.Yes) return;
            await UpdateTagsAndTraits(listOfTitlesToUpdate);
            ReloadLists();
            RefreshVNList();
            WriteText(userListReply, $"Updated tags/traits of {_vnsAdded} titles.");
        }

        private void ToggleNSFWImages(object sender, EventArgs e)
        {
            Settings.Default.ShowNSFWImages = nsfwToggle.Checked;
            Settings.Default.Save();
            tileOLV.SetObjects(tileOLV.Objects);
        }

        private void ToggleAutoUpdateURT(object sender, EventArgs e)
        {
            Settings.Default.AutoUpdateURT = autoUpdateURTBox.Checked;
            Settings.Default.Save();
        }

        private void ToggleLimit10Years(object sender, EventArgs e)
        {
            Settings.Default.Limit10Years = yearLimitBox.Checked;
            Settings.Default.Save();
        }

        /// <summary>
        ///     Close All Open Visual Novel Forms (windows)
        /// </summary>
        private void CloseAllForms(object sender, EventArgs e)
        {
            for (var i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i].Name == "FormMain") continue;
                LogToFile($"Closing {Application.OpenForms[i].Name}, {i}");
                Application.OpenForms[i].Close();
            }
        }

        /// <summary>
        ///     Display html file explaining how to get started.
        /// </summary>
        private void Help_GetStarted(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            if (path == null)
            {
                WriteError(prodReply, @"Unknown Path Error");
                return;
            }
            var helpFile = $"{Path.Combine(path, "help\\getstarted.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

        #endregion

        #region Get User-Related Titles

        //Get user's user/wish/votelists from VNDB
        /// <summary>
        ///     Get user's userlist, wishlist and votelist from VNDB.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateURTButtonClick(object sender, EventArgs e)
        {
            if (UserID < 1)
            {
                WriteText(userListReply, Resources.set_userid_first);
                return;
            }
            var askBox =
                MessageBox.Show(
                    $@"This may take a while depending on how many titles are in your lists,
especially if this is the first time.
Are you sure?

You currently have {URTList
                        .Count} items in the local database, they can
be displayed by clicking the User Related Titles (URT) filter.",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            await UpdateURT();
        }

        private async Task UpdateURT()
        {
            if (UserID < 1) return;
            LogToFile($"Starting GetUserRelatedTitles for {UserID}, previously had {URTList.Count} titles.");
            ReloadLists();
            List<int> userIDList = URTList.Select(x => x.VNID).ToList();
            userIDList = await GetUserList(userIDList);
            userIDList = await GetWishList(userIDList);
            await GetVoteList(userIDList);
            await GetRemainingTitles();
            DBConn.Open();
            _vnList = DBConn.GetAllTitles(UserID);
            DBConn.Close();
            var favprolist = new List<ListedProducer>();
            foreach (ListedProducer producer in olFavoriteProducers.Objects)
            {
                ListedVN[] producerVNs = URTList.Where(x => x.Producer.Equals(producer.Name)).ToArray();
                double userDropRate = -1;
                double userAverageVote = -1;
                if (producerVNs.Any())
                {
                    var finishedCount = producerVNs.Count(x => x.ULStatus.Equals("Finished"));
                    var droppedCount = producerVNs.Count(x => x.ULStatus.Equals("Dropped"));
                    ListedVN[] producerVotedVNs = producerVNs.Where(x => x.Vote > 0).ToArray();
                    userDropRate = finishedCount + droppedCount != 0
                        ? (double)droppedCount / (droppedCount + finishedCount)
                        : -1;
                    userAverageVote = producerVotedVNs.Any() ? producerVotedVNs.Select(x => x.Vote).Average() : -1;
                }
                favprolist.Add(new ListedProducer(producer.Name, producer.NumberOfTitles, producer.Loaded,
                    DateTime.UtcNow, producer.ID, userAverageVote, (int)Math.Round(userDropRate * 100)));
            }
            DBConn.Open();
            DBConn.InsertFavoriteProducers(favprolist, UserID);
            DBConn.Close();
            ReloadLists();
            LoadFavoriteProducerList();
            RefreshVNList();
            UpdateUserStats();
            if (URTList.Count > 0) WriteText(userListReply, $"Updated URT ({_vnsAdded} added).");
            else WriteError(userListReply, Resources.no_results, true);
        }


        /// <summary>
        ///     Get user's userlist from VNDB, add titles that aren't in local db already.
        /// </summary>
        /// <param name="userIDList">list of title IDs (avoids duplicate fetching)</param>
        /// <returns>list of title IDs (avoids duplicate fetching)</returns>
        private async Task<List<int>> GetUserList(List<int> userIDList)
        {
            LogToFile("Starting GetUserList");
            string userListQuery = $"get vnlist basic (uid = {UserID} ) {{\"results\":100}}";
            //1 - fetch from VNDB using API
            var result = await TryQuery(userListQuery, Resources.gul_query_error, userListReply);
            if (!result) return userIDList;
            var ulRoot = JsonConvert.DeserializeObject<UserListRoot>(Conn.LastResponse.JsonPayload);
            if (ulRoot.Num == 0) return userIDList;
            List<UserListItem> ulList = ulRoot.Items; //make list of vns in list
            var pageNo = 1;
            var moreResults = ulRoot.More;
            while (moreResults)
            {
                pageNo++;
                string userListQuery2 = $"get vnlist basic (uid = {UserID} ) {{\"results\":100, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(userListQuery2, Resources.gul_query_error, userListReply);
                if (!moreResult) return userIDList;
                var ulMoreRoot = JsonConvert.DeserializeObject<UserListRoot>(Conn.LastResponse.JsonPayload);
                ulList.AddRange(ulMoreRoot.Items);
                moreResults = ulMoreRoot.More;
            }
            DBConn.Open();
            foreach (var item in ulList)
            {
                DBConn.UpsertUserList(UserID, item, userIDList.Contains(item.VN));
                if (!userIDList.Contains(item.VN)) userIDList.Add(item.VN);
            }
            DBConn.Close();
            return userIDList;
        }

        private async Task<List<int>> GetWishList(List<int> userIDList)
        {
            LogToFile("Starting GetWishList");
            string wishListQuery = $"get wishlist basic (uid = {UserID} ) {{\"results\":100}}";
            var result = await TryQuery(wishListQuery, Resources.gwl_query_error, userListReply);
            if (!result) return userIDList;
            var wlRoot = JsonConvert.DeserializeObject<WishListRoot>(Conn.LastResponse.JsonPayload);
            if (wlRoot.Num == 0) return userIDList;
            List<WishListItem> wlList = wlRoot.Items; //make list of vn in list
            var pageNo = 1;
            var moreResults = wlRoot.More;
            while (moreResults)
            {
                pageNo++;
                string wishListQuery2 = $"get wishlist basic (uid = {UserID} ) {{\"results\":100, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(wishListQuery2, Resources.gwl_query_error, userListReply);
                if (!moreResult) return userIDList;
                var wlMoreRoot = JsonConvert.DeserializeObject<WishListRoot>(Conn.LastResponse.JsonPayload);
                wlList.AddRange(wlMoreRoot.Items);
                moreResults = wlMoreRoot.More;
            }
            DBConn.Open();
            foreach (var item in wlList)
            {
                DBConn.UpsertWishList(UserID, item, userIDList.Contains(item.VN));
                if (!userIDList.Contains(item.VN)) userIDList.Add(item.VN);
            }
            DBConn.Close();
            return userIDList;
        }

        private async Task<List<int>> GetVoteList(List<int> userIDList)
        {
            LogToFile("Starting GetVoteList");
            string voteListQuery = $"get votelist basic (uid = {UserID} ) {{\"results\":100}}";
            var result = await TryQuery(voteListQuery, Resources.gvl_query_error, userListReply);
            if (!result) return userIDList;
            var vlRoot = JsonConvert.DeserializeObject<VoteListRoot>(Conn.LastResponse.JsonPayload);
            if (vlRoot.Num == 0) return userIDList;
            List<VoteListItem> vlList = vlRoot.Items; //make list of vn in list
            var pageNo = 1;
            var moreResults = vlRoot.More;
            while (moreResults)
            {
                pageNo++;
                string voteListQuery2 = $"get votelist basic (uid = {UserID} ) {{\"results\":100, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(voteListQuery2, Resources.gvl_query_error, userListReply);
                if (!moreResult) return userIDList;
                var vlMoreRoot = JsonConvert.DeserializeObject<VoteListRoot>(Conn.LastResponse.JsonPayload);
                vlList.AddRange(vlMoreRoot.Items);
                moreResults = vlMoreRoot.More;
            }
            DBConn.Open();
            foreach (var item in vlList)
            {
                DBConn.UpsertVoteList(UserID, item, userIDList.Contains(item.VN));
                if (!userIDList.Contains(item.VN)) userIDList.Add(item.VN);
            }
            DBConn.Close();
            return userIDList;
        }

        private async Task GetRemainingTitles()
        {
            LogToFile("Starting GetRemainingTitles");
            DBConn.Open();
            List<int> unfetchedTitles = DBConn.GetUnfetchedUserRelatedTitles(UserID);
            DBConn.Close();
            if (!unfetchedTitles.Any()) return;
            await GetMultipleVN(unfetchedTitles, userListReply,true);
            WriteText(userListReply, Resources.updated_urt, true);
            ReloadLists();
        }

        private void UpdateUserStats()
        {
            if (!URTList.Any())
            {
                ulstatsall.Text = @"None";
                ulstatsul.Text = @"-";
                ulstatswl.Text = @"-";
                ulstatsvl.Text = @"-";
                ulstatsavs.Text = @"-";
                DisplayCommonTagsURT(null, null);
                return;
            }
            var ulCount = 0;
            var wlCount = 0;
            var vlCount = 0;
            double cumulativeScore = 0;
            foreach (var item in URTList)
            {
                if (item.ULStatus.Length > 0) ulCount++;
                if (item.WLStatus.Length > 0) wlCount++;
                if (!(item.Vote > 0)) continue;
                vlCount++;
                cumulativeScore += item.Vote;
            }
            ulstatsall.Text = URTList.Count.ToString();
            ulstatsul.Text = ulCount.ToString();
            ulstatswl.Text = wlCount.ToString();
            ulstatsvl.Text = vlCount.ToString();
            ulstatsavs.Text = (cumulativeScore / vlCount).ToString("#.##");
            DisplayCommonTagsURT(null, null);
        }

        #endregion

        #region Other/General

        /// <summary>
        /// Print message to Debug and write it to log file.
        /// </summary>
        /// <param name="message">Message to be written</param>
        public static void LogToFile(string message)
        {
            Debug.Print(message);
            using (var writer = new StreamWriter(LogFile,true))
            {
                writer.WriteLine(message);
            }
        }

        /// <summary>
        ///     Loads lists from local database.
        /// </summary>
        private void ReloadLists()
        {
            DBConn.Open();
            _vnList = DBConn.GetAllTitles(UserID);
            _producerList = DBConn.GetAllProducers();
            CharacterList = DBConn.GetAllCharacters();
            URTList = DBConn.GetUserRelatedTitles(UserID);
            DBConn.Close();
        }

        /// <summary>
        ///     Writes message in a label with message text color.
        /// </summary>
        /// <param name="label">Label to which the message is written</param>
        /// <param name="message">Message to be written</param>
        /// <param name="fade">Should message disappear after a few seconds?</param>
        public static void WriteText(Label label, string message, bool fade = false)
        {
            var linkLabel = label as LinkLabel;
            if (linkLabel != null) linkLabel.LinkColor = NormalLinkColor;
            else label.ForeColor = NormalColor;
            label.Text = message;
            if (fade) FadeLabel(label);
        }

        /// <summary>
        ///     Writes message in a label with warning text color.
        /// </summary>
        /// <param name="label">Label to which the message is written</param>
        /// <param name="message">Message to be written</param>
        /// <param name="fade">Should message disappear after a few seconds?</param>
        public static void WriteWarning(Label label, string message, bool fade = false)
        {
            var linkLabel = label as LinkLabel;
            if (linkLabel != null) linkLabel.LinkColor = WarningColor;
            else label.ForeColor = WarningColor;
            label.Text = message;
            if (fade) FadeLabel(label);
        }

        /// <summary>
        ///     Writes message in a label with error text color.
        /// </summary>
        /// <param name="label">Label to which the message is written</param>
        /// <param name="message">Message to be written</param>
        /// <param name="fade">Should message disappear after a few seconds?</param>
        public static void WriteError(Label label, string message, bool fade = false)
        {
            var linkLabel = label as LinkLabel;
            if (linkLabel != null) linkLabel.LinkColor = ErrorColor;
            else label.ForeColor = ErrorColor;
            label.Text = message;
            if (fade) FadeLabel(label);
        }

        /// <summary>
        ///     Convert DateTime to UnixTimestamp.
        /// </summary>
        /// <param name="dateTime">DateTime to be converted</param>
        /// <returns>UnixTimestamp (double)</returns>
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime -
                    new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        /// <summary>
        ///     Saves a VN's cover image (unless it already exists)
        /// </summary>
        /// <param name="vn"></param>
        private static void SaveImage(VNItem vn)
        {
            if (!Directory.Exists(VNImagesFolder)) Directory.CreateDirectory(VNImagesFolder);
            if (vn.Image == null || vn.Image.Equals("")) return;
            var ext = Path.GetExtension(vn.Image);
            string imageLoc = $"{VNImagesFolder}{vn.ID}{ext}";
            if (File.Exists(imageLoc)) return;
            var requestPic = WebRequest.Create(vn.Image);
            using (var responsePic = requestPic.GetResponse())
            {
                using (var stream = responsePic.GetResponseStream())
                {
                    if (stream == null) return;
                    var webImage = Image.FromStream(stream);
                    webImage.Save(imageLoc);
                }
            }
        }

        internal static void SaveScreenshot(string imageUrl, string savedLocation)
        {
            if (!Directory.Exists(VNScreensFolder)) Directory.CreateDirectory(VNScreensFolder);
            string[] urlSplit = imageUrl.Split('/');
            if (!Directory.Exists($"{VNScreensFolder}\\{urlSplit[urlSplit.Length - 2]}"))
                Directory.CreateDirectory($"{VNScreensFolder}\\{urlSplit[urlSplit.Length - 2]}");
            if (imageUrl.Equals("")) return;
            if (File.Exists(savedLocation)) return;
            var requestPic = WebRequest.Create(imageUrl);
            using (var responsePic = requestPic.GetResponse())
            {
                using (var stream = responsePic.GetResponseStream())
                {
                    if (stream == null) return;
                    var webImage = Image.FromStream(stream);
                    webImage.Save(savedLocation);
                }
            }
        }

        /// <summary>
        ///     Delete text in label after time set in LabelFadeTime.
        /// </summary>
        /// <param name="tLabel">Label to delete text in</param>
        internal static void FadeLabel(Label tLabel)
        {
            var fadeTimer = new Timer { Interval = LabelFadeTime };
            fadeTimer.Tick += (sender, e) =>
            {
                tLabel.Text = "";
                fadeTimer.Stop();
            };
            fadeTimer.Start();
        }

        /// <summary>
        ///     Display ten most common tags in the current list. Takes time when list contains over 9000 titles.
        /// </summary>
        internal void DisplayCommonTags(object sender, EventArgs e)
        {
            //TODO use one predefined thread, so that this only occurs once at a time instead of overlapping eachother
            if (sender != null && !DontTriggerEvent)
            {
                var checkBox = (CheckBox)sender;
                DontTriggerEvent = true;
                IEnumerable<VisualNovelForm> vnForms = Application.OpenForms.OfType<VisualNovelForm>();
                switch (checkBox.Name)
                {
                    case "tagTypeC":
                        Settings.Default.TagTypeC = checkBox.Checked;
                        tagTypeC2.Checked = checkBox.Checked;
                        foreach (var vnForm in vnForms)
                        {
                            vnForm.tagTypeC.Checked = checkBox.Checked;
                            vnForm.DisplayTags(null, null);
                        }
                        break;
                    case "tagTypeS":
                        Settings.Default.TagTypeS = checkBox.Checked;
                        tagTypeS2.Checked = checkBox.Checked;
                        foreach (var vnForm in vnForms)
                        {
                            vnForm.tagTypeC.Checked = checkBox.Checked;
                            vnForm.DisplayTags(null, null);
                        }
                        break;
                    case "tagTypeT":
                        Settings.Default.TagTypeT = checkBox.Checked;
                        tagTypeT2.Checked = checkBox.Checked;
                        foreach (var vnForm in vnForms)
                        {
                            vnForm.tagTypeC.Checked = checkBox.Checked;
                            vnForm.DisplayTags(null, null);
                        }
                        break;
                }
                DontTriggerEvent = false;
                Settings.Default.Save();
                DisplayCommonTagsURT(null, null);
            }
            List<ListedVN> vnlist = tileOLV.FilteredObjects.Cast<ListedVN>().ToList();
            var vnCount = vnlist.Count;
            if (vnCount == 0)
            {
                ClearCommonTags(TagTypeAll, 10);
                return;
            }
            List<KeyValuePair<int, int>> toptentags = null;
            var bw = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            bw.DoWork += delegate
            {
                //vn list - most common tags
                var taglist = new Dictionary<int, int>();
                var vnNo = 1;
                foreach (var vn in vnlist)
                {
                    var progressPercent = (double)vnNo / vnCount * 100;
                    vnNo++;
                    try
                    {
                        bw.ReportProgress((int)Math.Floor(progressPercent));
                    }
                    catch
                    {
                        LogToFile("Closed while Updating Most Common Tags");
                        return;
                    }
                    if (!vn.Tags.Any()) continue;
                    List<TagItem> tags = StringToTags(vn.Tags);
                    foreach (var tag in tags)
                    {
                        var tagtag = PlainTags.Find(item => item.ID == tag.ID);
                        if (tagtag == null) continue;
                        if (tagtag.Cat.Equals(ContentTag) && Settings.Default.TagTypeC == false) continue;
                        if (tagtag.Cat.Equals(SexualTag) && Settings.Default.TagTypeS == false) continue;
                        if (tagtag.Cat.Equals(TechnicalTag) && Settings.Default.TagTypeT == false) continue;
                        if (_activeTagFilter.Find(x => x.ID == tagtag.ID) != null) continue;
                        if (taglist.ContainsKey(tag.ID))
                        {
                            taglist[tag.ID] = taglist[tag.ID] + 1;
                        }
                        else
                        {
                            taglist.Add(tag.ID, 1);
                        }
                    }
                }
                List<KeyValuePair<int, int>> prodlistlist = taglist.ToList();
                prodlistlist.Sort((x, y) => y.Value.CompareTo(x.Value));
                toptentags = prodlistlist.Take(10).ToList();
            };
            bw.ProgressChanged +=
                delegate (object o, ProgressChangedEventArgs args)
                {
                    mctLoadingLabel.Text = $@"{args.ProgressPercentage}% Completed";
                };
            bw.RunWorkerCompleted += delegate
            {
                if (!toptentags.Any()) return;
                var mctNo = 1;
                while (mctNo < toptentags.Count)
                {
                    var mctIndex = mctNo - 1;
                    var name = TagTypeAll + mctNo;
                    var cb = (CheckBox)Controls.Find(name, true).First();
                    var tagName = PlainTags.Find(item => item.ID == toptentags[mctIndex].Key).Name;
                    cb.Text = $@"{tagName} ({toptentags[mctIndex].Value})";
                    cb.Checked = false;
                    cb.Visible = true;
                    mctNo++;
                }
                ClearCommonTags(TagTypeAll, 10 - toptentags.Count);
                FadeLabel(mctLoadingLabel);
            };
            bw.RunWorkerAsync();
        }

        /// <summary>
        ///     Display ten most common tags in user related titles.
        /// </summary>
        internal void DisplayCommonTagsURT(object sender, EventArgs e)
        {
            if (!URTList.Any())
            {
                var labelCount = 1;
                while (labelCount <= 10)
                {
                    var name = TagTypeUrt + labelCount;
                    var mctULLabel = (Label)Controls.Find(name, true).First();
                    mctULLabel.Visible = false;
                    labelCount++;
                }
                return;
            }
            //userlist stats - most common tags
            if (sender != null && !DontTriggerEvent)
            {
                var checkBox = (CheckBox)sender;
                DontTriggerEvent = true;
                switch (checkBox.Name)
                {
                    case "tagTypeC2":
                        Settings.Default.TagTypeC = checkBox.Checked;
                        tagTypeC.Checked = checkBox.Checked;
                        break;
                    case "tagTypeS2":
                        Settings.Default.TagTypeS = checkBox.Checked;
                        tagTypeS.Checked = checkBox.Checked;
                        break;
                    case "tagTypeT2":
                        Settings.Default.TagTypeT = checkBox.Checked;
                        tagTypeT.Checked = checkBox.Checked;
                        break;
                }
                DontTriggerEvent = false;
                Settings.Default.Save();
                DisplayCommonTags(null, null);
            }

            if (tagTypeC2.Checked == false && tagTypeS2.Checked == false && tagTypeT2.Checked == false)
            {
                var labelCount = 1;
                while (labelCount <= 10)
                {
                    var name = TagTypeUrt + labelCount;
                    var mctULLabel = (Label)Controls.Find(name, true).First();
                    mctULLabel.Visible = false;
                    labelCount++;
                }
                return;
            }
            var ulTagList = new Dictionary<int, int>();
            if (URTList.Count == 0) return;
            foreach (var vn in URTList)
            {
                if (!vn.Tags.Any()) continue;
                List<TagItem> tags = StringToTags(vn.Tags);
                foreach (var tag in tags)
                {
                    var tagtag = PlainTags.Find(item => item.ID == tag.ID);
                    if (tagtag == null) continue;
                    if (tagtag.Cat.Equals(ContentTag) && tagTypeC2.Checked == false) continue;
                    if (tagtag.Cat.Equals(SexualTag) && tagTypeS2.Checked == false) continue;
                    if (tagtag.Cat.Equals(TechnicalTag) && tagTypeT2.Checked == false) continue;
                    if (ulTagList.ContainsKey(tag.ID))
                    {
                        ulTagList[tag.ID] = ulTagList[tag.ID] + 1;
                    }
                    else
                    {
                        ulTagList.Add(tag.ID, 1);
                    }
                }
            }
            List<KeyValuePair<int, int>> ulProdlistlist = ulTagList.ToList();
            if (ulProdlistlist.Count == 0) return;
            ulProdlistlist.Sort((x, y) => y.Value.CompareTo(x.Value));
            var p = 1;
            var max = Math.Min(10, ulProdlistlist.Count) + 1;
            while (p < max)
            {
                var name = TagTypeUrt + p;
                var mctULLabel = (Label)Controls.Find(name, true).First();
                var tagName = PlainTags.Find(item => item.ID == ulProdlistlist[p - 1].Key).Name;
                mctULLabel.Text = $@"{tagName} ({ulProdlistlist[p - 1].Value})";
                mctULLabel.Visible = true;
                p++;
            }
            while (max <= 10)
            {
                var name = TagTypeUrt + max;
                var mctULLabel = (Label)Controls.Find(name, true).First();
                mctULLabel.Visible = false;
                max++;
            }
        }

        /// <summary>
        ///     Clear tag checkboxes that aren't in use (when most common tags are less than 10).
        /// </summary>
        /// <param name="tagType">All Tags or URT Tags</param>
        /// <param name="number">How many checkboxes to clear</param>
        private void ClearCommonTags(string tagType, int number)
        {
            if (number == 0) return;
            var min = 11 - number;
            while (min <= 10)
            {
                var name = tagType + min;
                var mctULLabel = Controls.Find(name, true).First();
                mctULLabel.Visible = false;
                min++;
            }
        }

        /// <summary>
        ///     Check if date is in the future.
        /// </summary>
        /// <param name="date">Date to be checked</param>
        /// <returns>Whether date is in the future</returns>
        private static bool IsUnreleased(string date)
        {
            return StringToDate(date) > DateTime.UtcNow;
        }

        /// <summary>
        ///     Convert a string containing a date (in the format YYYY-MM-DD) to a DateTime.
        /// </summary>
        /// <param name="date">String to be converted</param>
        /// <returns>DateTime representing date in string</returns>
        private static DateTime StringToDate(string date)
        {
            //unreleased if date is null or doesnt have any digits (tba, n/a etc)
            if (date == null || !date.Any(char.IsDigit)) return DateTime.MaxValue;
            int[] dateArray = date.Split('-').Select(int.Parse).ToArray();
            var dtDate = new DateTime();
            var dateRegex = new Regex(@"^\d{4}-\d{2}-\d{2}$");
            if (dateRegex.IsMatch(date))
            {
                //handle possible invalid dates such as february 30
                var tryDone = false;
                var tryCount = 0;
                while (!tryDone)
                {
                    try
                    {
                        dtDate = new DateTime(dateArray[0], dateArray[1], dateArray[2] - tryCount);
                        tryDone = true;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        LogToFile(
                            $"Date: {dateArray[0]}-{dateArray[1]}-{dateArray[2] - tryCount} is invalid, trying again one day earlier");
                        tryCount++;
                    }
                }
                return dtDate;
            }
            //if date only has year-month, then if month hasn't finished = unreleased
            var monthRegex = new Regex(@"^\d{4}-\d{2}$");
            if (monthRegex.IsMatch(date))
            {
                dtDate = new DateTime(dateArray[0], dateArray[1], 28);
                return dtDate;
            }
            //if date only has year, then if year hasn't finished = unreleased
            var yearRegex = new Regex(@"^\d{4}$");
            if (yearRegex.IsMatch(date))
            {
                dtDate = new DateTime(dateArray[0], 12, 28);
                return dtDate;
            }
            return DateTime.MaxValue;
        }


        /// <summary>
        ///     Convert JSON-formatted string to list of tags.
        /// </summary>
        /// <param name="tagstring">JSON-formatted string</param>
        /// <returns>List of tags</returns>
        internal static List<TagItem> StringToTags(string tagstring)
        {
            if (tagstring.Equals("")) return new List<TagItem>();
            var curS = $"{{\"tags\":{tagstring}}}";
            var vnitem = JsonConvert.DeserializeObject<VNItem>(curS);
            return vnitem.Tags;
        }

        /// <summary>
        ///     Decompress GZip file.
        /// </summary>
        /// <param name="fileToDecompress">File to Decompress</param>
        /// <param name="outputFile">Output File</param>
        public static void GZipDecompress(string fileToDecompress, string outputFile)
        {
            using (var originalFileStream = File.OpenRead(fileToDecompress))
            {
                var newFileName = outputFile;

                using (var decompressedFileStream = File.Create(newFileName))
                {
                    using (var decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        LogToFile($@"Decompressed: {fileToDecompress}");
                    }
                }
            }
        }

        /// <summary>
        ///     Get Days passed since date of last update.
        /// </summary>
        /// <param name="updatedDate">Date of last update</param>
        /// <returns>Number of days since last update</returns>
        public static int DaysSince(DateTime updatedDate)
        {
            if (updatedDate == DateTime.MinValue) return -1;
            var days = (DateTime.UtcNow - updatedDate).Days;
            return days;
        }

        /// <summary>
        ///     Get new VNDB Stats from VNDB using API.
        /// </summary>
        private void GetNewDBStats()
        {
            if (Conn.Status != VndbConnection.APIStatus.Ready) return;
            Conn.Query("dbstats");
            serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
            if (Conn.LastResponse.Type != ResponseType.DBStats)
            {
                dbs1r.Text = Resources.dbs_unknown_error;
                return;
            }
            var dbInfo = JsonConvert.DeserializeObject<DbRoot>(Conn.LastResponse.JsonPayload);
            XmlHelper.ToXmlFile(dbInfo, DBStatsXml);
            Settings.Default.DBStatsUpdate = DateTime.UtcNow;
            Settings.Default.Save();
            LoadDBStats();
        }

        /// <summary>
        ///     Load VNDB Stats from XML file.
        /// </summary>
        private void LoadDBStats()
        {
            if (!File.Exists(DBStatsXml)) GetNewDBStats();
            var dbXml = XmlHelper.FromXmlFile<DbRoot>(DBStatsXml);
            dbs1r.Text = Convert.ToString(dbXml.Users);
            dbs2r.Text = Convert.ToString(dbXml.Threads);
            dbs3r.Text = Convert.ToString(dbXml.Tags);
            dbs4r.Text = Convert.ToString(dbXml.Releases);
            dbs5r.Text = Convert.ToString(dbXml.Producers);
            dbs6r.Text = Convert.ToString(dbXml.Chars);
            dbs7r.Text = Convert.ToString(dbXml.Posts);
            dbs8r.Text = Convert.ToString(dbXml.VN);
            dbs9r.Text = Convert.ToString(dbXml.Traits);
        }

        /// <summary>
        ///     Get new Tag dump file from VNDB.org
        /// </summary>
        private void GetNewTagdump()
        {
            if (!File.Exists(TagsJsonGz))
            {
                SplashScreen.SplashScreen.SetStatus("Downloading new Tagdump file...");
                using (var client = new WebClient())
                {
                    client.DownloadFile(TagsURL, TagsJsonGz);
                }
            }
            GZipDecompress(TagsJsonGz, TagsJson);
            File.Delete(TagsJsonGz);
            Settings.Default.DumpfilesUpdate = DateTime.UtcNow;
            Settings.Default.Save();
            LoadTagdump();
        }

        /// <summary>
        ///     Load Tags from Tag dump file.
        /// </summary>
        private void LoadTagdump()
        {
            if (!File.Exists(TagsJson))
                GetNewTagdump();
            else
                try
                {
                    PlainTags = JsonConvert.DeserializeObject<List<WrittenTag>>(File.ReadAllText(TagsJson));
                    List<ItemWithParents> baseList = PlainTags.Cast<ItemWithParents>().ToList();
                    foreach (var writtenTag in PlainTags)
                    {
                        writtenTag.SetItemChildren(baseList);
                    }
                }
                catch (JsonReaderException e)
                {
                    LogToFile(e.Message);
                    LogToFile($"{TagsJson} could not be read, deleting it and getting new one.");
                    File.Delete(TagsJson);
                    GetNewTagdump();
                }
        }

        /// <summary>
        ///     Get new Trait dump file from VNDB.org
        /// </summary>
        private void GetNewTraitdump()
        {
            if (!File.Exists(TraitsJsonGz))
            {
                SplashScreen.SplashScreen.SetStatus("Downloading new Traitdump file...");
                using (var client = new WebClient())
                {
                    client.DownloadFile(TraitsURL, TraitsJsonGz);
                }
            }
            GZipDecompress(TraitsJsonGz, TraitsJson);
            File.Delete(TraitsJsonGz);
            Settings.Default.DumpfilesUpdate = DateTime.UtcNow;
            Settings.Default.Save();
            LoadTraitdump();
        }

        /// <summary>
        ///     Load Traits from Trait dump file.
        /// </summary>
        private void LoadTraitdump()
        {
            if (!File.Exists(TraitsJson))
                GetNewTraitdump();
            else
                try
                {
                    PlainTraits = JsonConvert.DeserializeObject<List<WrittenTrait>>(File.ReadAllText(TraitsJson));
                    List<ItemWithParents> baseList = PlainTraits.Cast<ItemWithParents>().ToList();
                    foreach (var writtenTrait in PlainTraits)
                    {
                        writtenTrait.SetTopmostParent(PlainTraits);
                        writtenTrait.SetItemChildren(baseList);
                    }

                }
                catch (JsonReaderException e)
                {
                    LogToFile(e.Message);
                    LogToFile($"{TraitsJson} could not be read, deleting it and getting new one.");
                    File.Delete(TraitsJson);
                    GetNewTraitdump();
                }
        }

        /// <summary>
        ///     Save custom filters and other filter settings to XML.
        /// </summary>
        private void SaveMainXML()
        {
            XmlHelper.ToXmlFile(new MainXml(_customTagFilters, _customTraitFilters, Toggles), MainXmlFile);
        }

        /// <summary>
        ///     Refresh VN OLV.
        /// </summary>
        private void RefreshVNList()
        {
            tileOLV.SetObjects(_vnList.Where(_currentList));
        }

        /// <summary>
        ///     Save user's VNDB login credentials to Windows Registry (encrypted).
        /// </summary>
        /// <param name="username">User's username</param>
        /// <param name="password">User's password</param>
        internal static void SaveCredentials(string username, char[] password)
        {
            //prepare data
            byte[] stringBytes = Encoding.UTF8.GetBytes(password);

            var entropy = new byte[20];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }
            byte[] ciphertext = ProtectedData.Protect(stringBytes, entropy,
                DataProtectionScope.CurrentUser);

            //store data
            var key = Registry.CurrentUser.OpenSubKey($"SOFTWARE\\{ClientName}", true) ??
                      Registry.CurrentUser.CreateSubKey($"SOFTWARE\\{ClientName}", true);
            if (key == null) return;
            key.SetValue("Cred1", username);
            key.SetValue("Cred2", ciphertext);
            key.SetValue("RNG", entropy);
            key.Close();
            LogToFile("Saved Login Credentials");
        }

        /// <summary>
        ///     Load user's VNDB login credentials from Windows Registry
        /// </summary>
        /// <returns></returns>
        private static KeyValuePair<string, char[]> LoadCredentials()
        {
            //get key data
            var key = Registry.CurrentUser.OpenSubKey($"SOFTWARE\\{ClientName}");
            if (key == null) return new KeyValuePair<string, char[]>();
            var username = key.GetValue("Cred1") as string;
            var password = key.GetValue("Cred2") as byte[];
            var entropy = key.GetValue("RNG") as byte[];
            key.Close();
            if (username == null || password == null || entropy == null) return new KeyValuePair<string, char[]>();
            byte[] vv = ProtectedData.Unprotect(password, entropy, DataProtectionScope.CurrentUser);
            LogToFile("Loaded Login Credentials");
            return new KeyValuePair<string, char[]>(username, Encoding.UTF8.GetChars(vv));
        }

        private async void LogQuestion(object sender, KeyPressEventArgs e) //send a command direct to server
        {
            if (e.KeyChar != (char)Keys.Enter) return;
            await Conn.QueryAsync(questionBox.Text);
            serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
        }

        private void ClearLog(object sender, EventArgs e) //clear log
        {
            serverQ.Text = "";
            serverR.Text = "";
        }

        private static string TruncateString(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
        #endregion

        #region Press Enter On Text Boxes


        private void EnterCustomTagFilterName(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SaveCustomTagFilter(sender, e);
        }

        private void EnterCustomTraitFilterName(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SaveCustomTraitFilter(sender, e);
        }
        private void searchButton_keyPress(object sender, KeyPressEventArgs e) //press enter on search button
        {
            if (e.KeyChar != (char)Keys.Enter) return;
            e.Handled = true;
            VNSearch(sender, e);
        }

        private void yearBox_KeyDown(object sender, KeyEventArgs e) //press enter on get year titles box
        {
            if (e.KeyCode == Keys.Enter) GetYearTitles(sender, e);
        }

        #endregion

        #region Classes/Enums

        /// <summary>
        ///     Class For XML File, holding saved objects.
        /// </summary>
        [Serializable, XmlRoot("MainXml")]
        public class MainXml
        {
            /// <summary>
            ///     Empty constructor needed for XML.
            /// </summary>
            public MainXml()
            {
                CustomTagFilters = new List<CustomTagFilter>();
                CustomTraitFilters = new List<CustomTraitFilter>();
                XmlToggles = new ToggleArray();
            }

            /// <summary>
            ///     Constructor For Main XML File.
            /// </summary>
            /// <param name="customTagFilters">List of user-set custom tag filters</param>
            /// <param name="customTraitFilters">List of user-set custom trait filters</param>
            /// <param name="xmlToggleArray">Current list toggle settings</param>
            public MainXml(List<CustomTagFilter> customTagFilters, List<CustomTraitFilter> customTraitFilters, ToggleArray xmlToggleArray)
            {
                CustomTagFilters = customTagFilters;
                CustomTraitFilters = customTraitFilters;
                XmlToggles = xmlToggleArray;
            }

            /// <summary>
            ///     List of User-created custom filters.
            /// </summary>
            public List<CustomTagFilter> CustomTagFilters { get; set; }

            /// <summary>
            ///     List of User-created custom filters.
            /// </summary>
            public List<CustomTraitFilter> CustomTraitFilters { get; set; }

            /// <summary>
            ///     Current state of list filters.
            /// </summary>
            public ToggleArray XmlToggles { get; set; }
        }

        /// <summary>
        ///     Command to change VN status.
        /// </summary>
        internal enum Command
        {
            New,
            Update,
            Delete
        }

        /// <summary>
        ///     Type of VN status to be changed.
        /// </summary>
        internal enum ChangeType
        {
            UL,
            WL,
            Vote
        }

        #endregion

    }
}