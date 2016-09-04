using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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
using BrightIdeasSoftware;
using Happy_Search.Properties;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Happy_Search
{
    /// <summary>
    /// Main Form of application, contains global variables.
    /// </summary>
    public partial class FormMain : Form
    {
        //constants / definables
        private const string VNImagesFolder = "vnImages\\";
        private const string DBStatsXml = "dbs.xml";
        private const string TagsJsonGz = "tags.json.gz";
        private const string TagsJson = "tags.json";
        private const string TagTypeAll = "checkBox";
        private const string TagTypeUrt = "mctULLabel";
        private const string FilterLabel = "filterLabel";
        internal const string ClientName = "Happy Search By Zolty";
        internal const string ClientVersion = "1.1";
        internal const string APIVersion = "2.25";
        private const string APIMaxResults = "\"results\":25";
        private const int LabelFadeTime = 5000; //ms for text to disappear (not actual fade)
        private const string TagsURL = "http://vndb.org/api/tags.json.gz";
        private const string MainXmlFile = "saved_objects.xml";
        private static readonly Color ErrorColor = Color.Red;
        internal static readonly Color NormalColor = SystemColors.ControlLightLight;
        internal static readonly Color NormalLinkColor = Color.FromArgb(0, 192, 192);
        private static readonly Color WarningColor = Color.DarkKhaki;

        private List<ListedVN> _vnList; //contains all vns in local database
        private List<ListedProducer> _producerList; //contains all producers in local database
        private List<TagFilter> _activeFilter = new List<TagFilter>();
        private readonly List<ComplexFilter> _customFilters;
        internal readonly VndbConnection Conn = new VndbConnection();
        internal readonly DbHelper DBConn;
        private ushort _vnsAdded;
        private ushort _vnsSkipped;
        private Func<ListedVN, bool> _currentList = x => true;
        private bool _dontTriggerEvent; //used to skip indexchanged events
        internal List<WrittenTag> PlainTags; //Contains all tags as in tags.json
        internal int UserID; //id of current user
        internal List<ListedVN> URTList; //contains all user-related vns

        /*credits and resources
        ObjectListView by Phillip Piper (GPLv3)from http://www.codeproject.com/Articles/16009/A-Much-Easier-to-Use-ListView
        (slightly modified) A Pretty Good Splash Screen in C# by Tom Clement (CPOL) from http://www.codeproject.com/Articles/5454/A-Pretty-Good-Splash-Screen-in-C
        (reasonably modified) VndbClient by FredTheBarber, for connection/queries to VNDB API https://github.com/FredTheBarber/VndbClient
        */

        #region Initialization

        /// <summary>
        /// Constructor for Main Application Form.
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            SplashScreen.SplashScreen.SetStatus("Initializing Controls...");
            {
                _dontTriggerEvent = true;
                ulStatusDropDown.SelectedIndex = 0;
                wlStatusDropDown.SelectedIndex = 0;
                customFilters.SelectedIndex = 0;
                viewPicker.SelectedIndex = 0;
                URTToggleBox.SelectedIndex = 0;
                UnreleasedToggleBox.SelectedIndex = 0;
                BlacklistToggleBox.SelectedIndex = 0;
                _dontTriggerEvent = false;
                replyText.Text = "";
                userListReply.Text = "";
                resultLabel.Text = "";
                loginReply.Text = "";
                prodReply.Text = "";
                filterReply.Text = "";
                mctLoadingLabel.Text = "";
                currentListLabel.Text = "";
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
                tagTypeC2.Checked = Settings.Default.TagTypeC2;
                tagTypeS2.Checked = Settings.Default.TagTypeS2;
                tagTypeT2.Checked = Settings.Default.TagTypeT2;
                nsfwToggle.Checked = Settings.Default.ShowNSFWImages;
                autoUpdateURTBox.Checked = Settings.Default.AutoUpdateURT;
                yearLimitBox.Checked = Settings.Default.Limit10Years;
            }
            SplashScreen.SplashScreen.SetStatus("Loading Tagdump...");
            {
                Debug.Print(
                    $"Tagdump Update = {Settings.Default.TagdumpUpdate}, days since = {DaysSince(Settings.Default.TagdumpUpdate)}");
                if (DaysSince(Settings.Default.TagdumpUpdate) > 2 || DaysSince(Settings.Default.TagdumpUpdate) == -1)
                    GetNewTagdump();
                else LoadTagdump();
                var acSource = new AutoCompleteStringCollection();
                acSource.AddRange(PlainTags.Select(v => v.Name).ToArray());
                tagSearchBox.AutoCompleteCustomSource = acSource;
            }
            SplashScreen.SplashScreen.SetStatus("Connecting to SQLite Database...");
            {
                DBConn = new DbHelper();
            }
            SplashScreen.SplashScreen.SetStatus("Loading Data from Database...");
            {
                LoadFavoriteProducerList();
                DBConn.Open();
                _vnList = DBConn.GetAllTitles(UserID);
                _producerList = DBConn.GetAllProducers();
                URTList = DBConn.GetUserRelatedTitles(UserID);
                DBConn.Close();
                Debug.Print("VN Items= " + _vnList.Count);
                Debug.Print("Producers= " + _producerList.Count);
                Debug.Print("UserRelated Items= " + URTList.Count);
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
                currentListLabel.Text = @"List: " + @"All Titles";
            }
            SplashScreen.SplashScreen.SetStatus("Loading Custom Filters...");
            {
                MainXml xml = File.Exists(MainXmlFile) ? XmlHelper.FromXmlFile<MainXml>(MainXmlFile) : new MainXml();
                _customFilters = xml.ComplexFilters;
                foreach (var filter in _customFilters) customFilters.Items.Add(filter.Name);
                _dontTriggerEvent = true;
                URTToggleBox.SelectedIndex = (int)xml.XmlToggles.URTToggleSetting;
                Toggles.URTToggleSetting = (ToggleSetting)URTToggleBox.SelectedIndex;
                UnreleasedToggleBox.SelectedIndex = (int)xml.XmlToggles.UnreleasedToggleSetting;
                Toggles.UnreleasedToggleSetting = (ToggleSetting)UnreleasedToggleBox.SelectedIndex;
                BlacklistToggleBox.SelectedIndex = (int)xml.XmlToggles.BlacklistToggleSetting;
                Toggles.BlacklistToggleSetting = (ToggleSetting)BlacklistToggleBox.SelectedIndex;
                _dontTriggerEvent = false;
                ApplyToggleFilters();
            }
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            InitAPIConnection();

            SplashScreen.SplashScreen.SetStatus("Loading DBStats...");
            {
                Debug.Print(
                    $"dbstats Update = {Settings.Default.DBStatsUpdate}, days since = {DaysSince(Settings.Default.DBStatsUpdate)}");
                if (DaysSince(Settings.Default.DBStatsUpdate) > 2 || DaysSince(Settings.Default.DBStatsUpdate) == -1)
                    GetNewDBStats();
                else LoadDBStats();
            }
            SplashScreen.SplashScreen.CloseForm();
            Debug.Print("Updating User Related Titles...");
            if (Settings.Default.AutoUpdateURT && UserID > 0)
            {
                Debug.Print($"URTUpdate= {Settings.Default.URTUpdate}");
                if (DaysSince(Settings.Default.URTUpdate) > 2 || DaysSince(Settings.Default.URTUpdate) == -1)
                {
                    Task.Run(UpdateURT);
                    Settings.Default.URTUpdate = DateTime.UtcNow;
                    Settings.Default.Save();
                }
                else
                {
                    Debug.Print("Update not needed.");
                }
            }
        }

        private void ShowProducerTitles(object sender, EventArgs e)
        {
            var vn = tileOLV.SelectedObject as ListedVN;
            if (vn == null) return;
            ProducerListBox.Text = vn.Producer;
            Filter_Producer(null, new KeyEventArgs(Keys.Enter));
        }


        private void RightClickAddProducer(object sender, EventArgs e)
        {
            var vn = tileOLV.SelectedObject as ListedVN;
            if (vn == null) return;
            var producers = olFavoriteProducers.Objects as List<ListedProducer>;
            if (producers?.Find(x => x.Name == vn.Producer) != null)
            {
                WriteText(replyText, "Already in list.", true);
                return;
            }
            ListedVN[] producerVNs = URTList.Where(x => x.Producer.Equals(vn.Producer)).ToArray();
            double userAverageVote = -1;
            double userDropRate = -1;
            if (producerVNs.Any())
            {
                var finishedCount = producerVNs.Count(x => x.ULStatus.Equals("Finished"));
                var droppedCount = producerVNs.Count(x => x.ULStatus.Equals("Dropped"));
                ListedVN[] producerVotedVNs = producerVNs.Where(x => x.Vote > 0).ToArray();
                userAverageVote = producerVotedVNs.Any() ? producerVotedVNs.Select(x => x.Vote).Average() : -1;
                userDropRate = finishedCount + droppedCount != 0
                    ? (double)droppedCount / (droppedCount + finishedCount)
                    : -1;
            }
            var addProducerList = new List<ListedProducer>
            {
                new ListedProducer(vn.Producer, producerVNs.Length, "No", DateTime.UtcNow,
                    _producerList.Find(x => x.Name == vn.Producer).ID,
                    userAverageVote, (int) Math.Round(userDropRate*100))
            };
            DBConn.Open();
            DBConn.InsertFavoriteProducers(addProducerList, UserID);
            DBConn.Close();
            ReloadLists();
            LoadFavoriteProducerList();
            WriteText(replyText, $"{vn.Producer} added to list.", true);
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
                DisplayCommonTagsULStats(null, null);
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
            DisplayCommonTagsULStats(null, null);
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            Conn.Close();
        }

        private void InitAPIConnection()
        {
            SplashScreen.SplashScreen.SetStatus("Logging into VNDB API...");
            {
                Conn.Open();
                //login with credentials if setting is enabled and credentials exist, otherwise login without credentials
                if (Settings.Default.RememberCredentials)
                {
                    KeyValuePair<string, char[]> credentials = LoadCredentials();
                    if (credentials.Value != null)
                    {
                        APILoginWithCredentials(credentials);
                        return;
                    }
                    APILogin();
                    return;
                }
                APILogin();
            }
        }

        /// <summary>
        /// Load Login Form for user to log in.
        /// </summary>
        private void LogInDialog(object sender, EventArgs e)
        {
            DialogResult = new LoginForm(this).ShowDialog();
            Debug.Print(DialogResult.ToString());
            if (DialogResult != DialogResult.OK) return;
            Settings.Default.UserID = UserID;
            Settings.Default.Save();
            UpdateUserStats();
            ReloadLists();
            RefreshVNList();
            LoadFavoriteProducerList();
        }

        #endregion

        #region Get User-Related Titles

        //Get user's user/wish/votelists from VNDB
        /// <summary>
        /// Get user's userlist, wishlist and votelist from VNDB.
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

You currently have {URTList.Count} items in the local database, they can
be displayed by clicking the User Related Titles (URT) filter.",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            await UpdateURT();
        }

        private async Task UpdateURT()
        {
            if (UserID < 1) return;
            Debug.Print($"Starting GetUserRelatedTitles for {UserID}, previously had {URTList.Count} titles.");
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
            tileOLV.Sort(tileColumnDate, SortOrder.Descending);
            ReloadLists();
            RefreshVNList();
            UpdateUserStats();
            if (URTList.Count > 0) WriteText(userListReply, $"Updated URT ({_vnsAdded} added).");
            else WriteError(userListReply, Resources.no_results);
        }

        /// <summary>
        /// Get user's userlist from VNDB, add titles that aren't in local db already.
        /// </summary>
        /// <param name="userIDList">list of title IDs (avoids duplicate fetching)</param>
        /// <returns>list of title IDs (avoids duplicate fetching)</returns>
        private async Task<List<int>> GetUserList(List<int> userIDList)
        {
            Debug.Print("Starting GetUserList");
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
            Debug.Print("Starting GetWishList");
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
            Debug.Print("Starting GetVoteList");
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
            Debug.Print("Starting GetRemainingTitles");
            DBConn.Open();
            List<int> unfetchedTitles = DBConn.GetUnfetchedUserRelatedTitles(UserID);
            DBConn.Close();
            if (!unfetchedTitles.Any()) return;
            await GetMultipleVN(unfetchedTitles, userListReply);
            WriteText(userListReply, Resources.updated_urt);
            ReloadLists();
        }

        #endregion

        #region Search Visual Novels

        /// <summary>
        /// Searches for VNs from VNDB, adds them if they are not in local database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void VNSearch(object sender, EventArgs e) //Fetch information from 'VNDB.org'
        {
            if (searchBox.Text == "") //check if box is empty
            {
                WriteError(replyText, Resources.enter_vn_title, true);
                return;
            }
            if (searchBox.Text.Length < 3)
            {
                WriteError(replyText, Resources.enter_vn_title + " (atleast 3 chars)", true);
                return;
            }
            _vnsAdded = 0;
            _vnsSkipped = 0;
            string vnSearchQuery = $"get vn basic (search ~ \"{searchBox.Text}\") {{{APIMaxResults}}}";
            var queryResult = await TryQuery(vnSearchQuery, Resources.vn_query_error, replyText, ignoreDateLimit: true);
            if (!queryResult) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            List<VNItem> vnItems = vnRoot.Items;
            //await GetMultipleVN(vnItems.Select(x => x.ID), replyText);
            var pageNo = 1;
            var moreResults = vnRoot.More;
            while (moreResults)
            {
                pageNo++;
                vnSearchQuery = $"get vn basic (search ~ \"{searchBox.Text}\") {{{APIMaxResults}, \"page\":{pageNo}}}";
                queryResult = await TryQuery(vnSearchQuery, Resources.vn_query_error, replyText, ignoreDateLimit: true);
                if (!queryResult) return;
                vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                vnItems.AddRange(vnRoot.Items);
                moreResults = vnRoot.More;
            }
            await GetMultipleVN(vnItems.Select(x => x.ID), replyText, true);
            WriteText(replyText, $"Found {_vnsAdded + _vnsSkipped} VNs for, {_vnsAdded} added, {_vnsSkipped} skipped.");
            IEnumerable<int> idList = vnItems.Select(x => x.ID);
            _currentList = x => idList.Contains(x.VNID);
            currentListLabel.Text = @"List: " + $"{searchBox.Text} (Search)";
            RefreshVNList();
        }

        /// <summary>
        /// Gets VNs released in the year entered by user, doesn't update VNs already in local database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GetYearTitles(object sender, EventArgs e)
        {
            if (yearBox.Text == "") //check if box is empty
            {
                WriteError(replyText, Resources.enter_year, true);
                return;
            }
            int year;
            var userIsNumber = int.TryParse(yearBox.Text, out year);
            if (userIsNumber == false) //check if box has integer
            {
                WriteError(replyText, Resources.must_be_integer, true);
                return;
            }
            var startTime = DateTime.UtcNow.ToLocalTime().ToString("HH:mm");
            WriteText(replyText, $"Getting All VNs For year {year}.  Started at {startTime}");
            ReloadLists();
            _currentList = x => x.RelDate.StartsWith(yearBox.Text);
            currentListLabel.Text = @"List: " + $"{yearBox.Text} (Year)";
            _vnsAdded = 0;
            _vnsSkipped = 0;
            string vnInfoQuery =
                $"get vn basic (released > \"{year - 1}\" and released <= \"{year}\") {{{APIMaxResults}}}";
            var result = await TryQuery(vnInfoQuery, Resources.gyt_query_error, replyText, true, true, true);
            if (!result) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            List<VNItem> vnItems = vnRoot.Items;
            await GetMultipleVN(vnItems.Select(x => x.ID).ToList(), replyText, true);
            var pageNo = 1;
            var moreResults = vnRoot.More;
            while (moreResults)
            {
                pageNo++;
                string vnInfoMoreQuery =
                    $"get vn basic (released > \"{year - 1}\" and released <= \"{year}\") {{{APIMaxResults}, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(vnInfoMoreQuery, Resources.gyt_query_error, replyText, true, true, true);
                if (!moreResult) return;
                var vnMoreRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                List<VNItem> vnMoreItems = vnMoreRoot.Items;
                await GetMultipleVN(vnMoreItems.Select(x => x.ID).ToList(), replyText, true);
                moreResults = vnMoreRoot.More;
            }
            var endTime = DateTime.UtcNow.ToLocalTime().ToString("HH:mm");
            WriteText(replyText,
                $"Got all VNs for {year}.  Time:{startTime}-{endTime}  {_vnsAdded} added, {_vnsSkipped} skipped.");
            RefreshVNList();
        }

        #endregion

        #region User-Settings

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

        #endregion

        #region Log Tab

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

        #endregion

        #region Other/General

        /// <summary>
        /// Handle VN status change via context menu.
        /// </summary>
        private async void HandleContextMenuVNStatusChange(object sender, ToolStripItemClickedEventArgs e)
        {
            if (Conn.LogIn != VndbConnection.LogInStatus.YesWithCredentials)
            {
                WriteError(replyText, "Not Logged In", true);
                return;
            }
            var nitem = e.ClickedItem;
            if (nitem == null) return;
            bool success;
            var vn = tileOLV.SelectedObject as ListedVN;
            if (vn == null) return;
            var statusInt = -1;
            switch (nitem.OwnerItem.Text)
            {
                case "Userlist":
                    statusInt = Array.IndexOf(ListedVN.StatusUL, nitem.Text);
                    success = await ChangeVNStatus(vn, ChangeType.UL, statusInt);
                    break;
                case "Wishlist":
                    statusInt = Array.IndexOf(ListedVN.PriorityWL, nitem.Text);
                    success = await ChangeVNStatus(vn, ChangeType.WL, statusInt);
                    break;
                case "Vote":
                    if (!nitem.Text.Equals("(None)")) statusInt = Convert.ToInt32(nitem.Text);
                    success = await ChangeVNStatus(vn, ChangeType.Vote, statusInt);
                    break;
                default:
                    success = false;
                    break;
            }
            if (!success) return;
            ReloadLists();
            RefreshVNList();
            WriteText(replyText, $"ID={vn.VNID}, status changed.", true);
        }

        /// <summary>
        /// Display html file explaining how to get started.
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

        /// <summary>
        /// Display html file explaining searching, listing and filtering section.
        /// </summary>
        private void Help_SearchingAndFiltering(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            if (path == null)
            {
                WriteError(replyText, @"Unknown Path Error");
                return;
            }
            var helpFile = $"{Path.Combine(path, "help\\searchingandfiltering.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

        /// <summary>
        /// Close All Open Visual Novel Forms (windows)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseAllForms(object sender, EventArgs e)
        {
            for (var i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i].Name == "FormMain") continue;
                Debug.Print($"Closing {Application.OpenForms[i].Name}, {i}");
                Application.OpenForms[i].Close();
            }
        }

        /// <summary>
        /// Loads lists from local database.
        /// </summary>
        private void ReloadLists()
        {
            DBConn.Open();
            _vnList = DBConn.GetAllTitles(UserID);
            _producerList = DBConn.GetAllProducers();
            URTList = DBConn.GetUserRelatedTitles(UserID);
            DBConn.Close();
        }

        /// <summary>
        /// Writes message in a label with message text color.
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
        /// Writes message in a label with warning text color.
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
        /// Writes message in a label with error text color.
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
        /// Change view of Visual Novel ObjectListView.
        /// </summary>
        private void OLVChangeView(object sender, EventArgs e)
        {
            if (_dontTriggerEvent) return;
            var cb = (ComboBox)sender;
            switch (cb.SelectedIndex)
            {
                case 0:
                    tileOLV.View = View.Tile;
                    break;
                case 1:
                    tileOLV.View = View.Details;
                    break;
            }
        }

        /// <summary>
        /// Convert DateTime to UnixTimestamp.
        /// </summary>
        /// <param name="dateTime">DateTime to be converted</param>
        /// <returns>UnixTimestamp (double)</returns>
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime -
                    new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        /// <summary>
        /// Display Context Menu on right clicking a visual novel.
        /// </summary>
        private void ShowContextMenu(object sender, CellRightClickEventArgs e)
        {
            if (e == null) return;
            e.MenuStrip = VNContextMenu(e.Model);
        }

        /// <summary>
        /// Prepare and display context menu for visual novel.
        /// </summary>
        /// <param name="model">The object that was selected.</param>
        /// <returns>Context Menu for Visual Novel</returns>
        private ContextMenuStrip VNContextMenu(object model)
        {
            //clearing previous
            foreach (ToolStripMenuItem item in userlistToolStripMenuItem.DropDownItems) item.Checked = false;
            foreach (ToolStripMenuItem item in wishlistToolStripMenuItem.DropDownItems) item.Checked = false;
            foreach (ToolStripMenuItem item in voteToolStripMenuItem.DropDownItems) item.Checked = false;
            userlistToolStripMenuItem.Checked = false;
            wishlistToolStripMenuItem.Checked = false;
            voteToolStripMenuItem.Checked = false;

            //set new
            var vn = (ListedVN)model;
            userlistToolStripMenuItem.Checked = !vn.ULStatus.Equals("");
            wishlistToolStripMenuItem.Checked = !vn.WLStatus.Equals("");
            voteToolStripMenuItem.Checked = vn.Vote > 0;
            switch (vn.ULStatus)
            {
                case "":
                    ((ToolStripMenuItem)userlistToolStripMenuItem.DropDownItems[0]).Checked = true;
                    break;
                case "Unknown":
                    ((ToolStripMenuItem)userlistToolStripMenuItem.DropDownItems[1]).Checked = true;
                    break;
                case "Playing":
                    ((ToolStripMenuItem)userlistToolStripMenuItem.DropDownItems[2]).Checked = true;
                    break;
                case "Finished":
                    ((ToolStripMenuItem)userlistToolStripMenuItem.DropDownItems[3]).Checked = true;
                    break;
                case "Stalled":
                    ((ToolStripMenuItem)userlistToolStripMenuItem.DropDownItems[4]).Checked = true;
                    break;
                case "Dropped":
                    ((ToolStripMenuItem)userlistToolStripMenuItem.DropDownItems[5]).Checked = true;
                    break;
            }
            switch (vn.WLStatus)
            {
                case "":
                    ((ToolStripMenuItem)wishlistToolStripMenuItem.DropDownItems[0]).Checked = true;
                    break;
                case "High":
                    ((ToolStripMenuItem)wishlistToolStripMenuItem.DropDownItems[1]).Checked = true;
                    break;
                case "Medium":
                    ((ToolStripMenuItem)wishlistToolStripMenuItem.DropDownItems[2]).Checked = true;
                    break;
                case "Low":
                    ((ToolStripMenuItem)wishlistToolStripMenuItem.DropDownItems[3]).Checked = true;
                    break;
                case "Blacklist":
                    ((ToolStripMenuItem)wishlistToolStripMenuItem.DropDownItems[4]).Checked = true;
                    break;
            }
            if (vn.Vote > 0)
            {
                var vote = (int)Math.Floor(vn.Vote);
                ((ToolStripMenuItem)voteToolStripMenuItem.DropDownItems[vote]).Checked = true;
            }
            else
                ((ToolStripMenuItem)voteToolStripMenuItem.DropDownItems[0]).Checked = true;

            return ContextMenuVN;
        }

        /// <summary>
        /// Whether Visual Novel matches list of active tag filters.
        /// </summary>
        /// <param name="vn">Visual Novel to be checked</param>
        /// <returns>Whether it matches</returns>
        private bool VNMatchesFilter(ListedVN vn)
        {
            int[] vnTags = StringToTags(vn.Tags).Select(x => x.ID).ToArray();
            //for each tag in list of active tag filters, add 1 to counter if vn has a tag that is either the specified tag or a subtag
            //if counter is the same as the amount of tags in active tag filters, it means it matched all of them, thus, it matches the whole filter.
            /*var filtersMatched = 0;
            foreach (var filter in _activeFilter)
            {
                if (vnTags.Any(vntag => filter.AllIDs.Contains(vntag))) filtersMatched++;
            }*/
            var filtersMatched = _activeFilter.Count(filter => vnTags.Any(vntag => filter.AllIDs.Contains(vntag)));
            return filtersMatched == _activeFilter.Count;
        }

        /// <summary>
        /// Whether Visual Novel contains a single tag (or a subtag).
        /// </summary>
        /// <param name="vn">Visual Novel to be checked</param>
        /// <param name="tag">Tag to be checked</param>
        /// <returns>Whether it contains the tag or a subtag.</returns>
        private static bool VNMatchesSingleTag(ListedVN vn, TagFilter tag)
        {
            int[] vnTags = StringToTags(vn.Tags).Select(x => x.ID).ToArray();
            return vnTags.Any(vntag => tag.AllIDs.Contains(vntag));
        }

        /// <summary>
        /// Saves a VN's cover image (unless it already exists)
        /// </summary>
        /// <param name="vn"></param>
        private static void SaveImage(VNItem vn)
        {
            if (!Directory.Exists(VNImagesFolder)) Directory.CreateDirectory(VNImagesFolder);
            if (vn.Image == null || vn.Image.Equals("")) return;
            var ext = Path.GetExtension(vn.Image);
            string imageLoc = $"vnImages\\{vn.ID}{ext}";
            if (File.Exists(imageLoc)) return;
            var requestPic = WebRequest.Create(vn.Image);
            var responsePic = requestPic.GetResponse();
            var stream = responsePic.GetResponseStream();
            if (stream == null) return;
            var webImage = Image.FromStream(stream);
            webImage.Save(imageLoc);
        }

        /// <summary>
        /// Delete text in label after time set in LabelFadeTime.
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

        //format list rows, color according to userlist status
        private void FormatRow(object sender, FormatRowEventArgs e)
        {
            if (e.ListView.View != View.Details) return;
            var listedVN = (ListedVN)e.Model;
            //ULStatus takes priority over WLStatus
            switch (listedVN.WLStatus)
            {
                case "High":
                    e.Item.BackColor = Color.DeepPink;
                    break;
                case "Medium":
                    e.Item.BackColor = Color.Pink;
                    break;
                case "Low":
                    e.Item.BackColor = Color.LightPink;
                    break;
            }
            switch (listedVN.ULStatus)
            {
                case "Finished":
                    e.Item.BackColor = Color.LightGreen;
                    break;
                case "Stalled":
                    e.Item.BackColor = Color.LightYellow;
                    break;
                case "Dropped":
                    e.Item.BackColor = Color.DarkOrange;
                    break;
                case "Unknown":
                    e.Item.BackColor = Color.Gray;
                    break;
            }
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(-1);
            if (listedVN.ULAdded == dateTimeOffset) e.Item.GetSubItem(4).Text = "";
            if (listedVN.WLAdded == dateTimeOffset) e.Item.GetSubItem(7).Text = "";
            if (listedVN.Vote < 1) e.Item.GetSubItem(8).Text = "";
            e.Item.GetSubItem(9).Text = listedVN.VoteCount > 0 ? $"{listedVN.Rating.ToString("0.00")} ({listedVN.VoteCount} Votes)" : "";
            e.Item.GetSubItem(10).Text = listedVN.Popularity > 0 ? listedVN.Popularity.ToString("0.00") : "";
        }

        /// <summary>
        /// Display ten most common tags in the current list. Takes time when list contains over 9000 titles.
        /// </summary>
        private void DisplayCommonTags(object sender, EventArgs e)
        {
            //TODO use one predefined thread, so that this only occurs once at a time instead of overlapping eachother
            if (sender != null)
            {
                var checkBox = (CheckBox)sender;
                switch (checkBox.Name)
                {
                    case "tagTypeC":
                        Settings.Default.TagTypeC = checkBox.Checked;
                        break;
                    case "tagTypeS":
                        Settings.Default.TagTypeS = checkBox.Checked;
                        break;
                    case "tagTypeT":
                        Settings.Default.TagTypeT = checkBox.Checked;
                        break;
                }
                Settings.Default.Save();
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
                        Debug.Print("Closed while Updating Most Common Tags");
                        return;
                    }
                    if (!vn.Tags.Any()) continue;
                    List<TagItem> tags = StringToTags(vn.Tags);
                    foreach (var tag in tags)
                    {
                        var tagtag = PlainTags.Find(item => item.ID == tag.ID);
                        if (tagtag == null) continue;
                        if (tagtag.Cat.Equals("cont") && Settings.Default.TagTypeC == false) continue;
                        if (tagtag.Cat.Equals("ero") && Settings.Default.TagTypeS == false) continue;
                        if (tagtag.Cat.Equals("tech") && Settings.Default.TagTypeT == false) continue;
                        if (_activeFilter.Find(x => x.ID == tagtag.ID) != null) continue;
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
                    mctLoadingLabel.Text = $"{args.ProgressPercentage}% Completed";
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
                    cb.Text = $"{tagName} ({toptentags[mctIndex].Value})";
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
        /// Display ten most common tags in user related titles.
        /// </summary>
        private void DisplayCommonTagsULStats(object sender, EventArgs e)
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
            if (sender != null)
            {
                var checkBox = (CheckBox)sender;
                switch (checkBox.Name)
                {
                    case "tagTypeC2":
                        Settings.Default.TagTypeC2 = checkBox.Checked;
                        break;
                    case "tagTypeS2":
                        Settings.Default.TagTypeS2 = checkBox.Checked;
                        break;
                    case "tagTypeT2":
                        Settings.Default.TagTypeT2 = checkBox.Checked;
                        break;
                }
                Settings.Default.Save();
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
                    if (tagtag.Cat.Equals("cont") && tagTypeC2.Checked == false) continue;
                    if (tagtag.Cat.Equals("ero") && tagTypeS2.Checked == false) continue;
                    if (tagtag.Cat.Equals("tech") && tagTypeT2.Checked == false) continue;
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
                mctULLabel.Text = $"{tagName} ({ulProdlistlist[p - 1].Value})";
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
        /// Clear tag checkboxes that aren't in use (when most common tags are less than 10).
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
        /// Check if date is in the future.
        /// </summary>
        /// <param name="date">Date to be checked</param>
        /// <returns>Whether date is in the future</returns>
        private static bool IsUnreleased(string date)
        {
            return StringToDate(date) > DateTime.UtcNow;
        }

        /// <summary>
        /// Convert a string containing a date (in the format YYYY-MM-DD) to a DateTime.
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
                        Debug.Print(
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
        /// Convert list of tags to JSON-formatted string.
        /// </summary>
        /// <param name="tags">List of tags</param>
        /// <returns>JSON-formatted string</returns>
        internal static string TagsToString(List<TagItem> tags)
        {
            //var tagstring = string.Join(",", tags.Select(v => string.Join(",", v.ToArray())).Select(vstring => '[' + vstring + ']').ToArray());
            return '[' + string.Join(",", tags.Select(v => v.ToString())) + ']';
        }

        /// <summary>
        /// Convert JSON-formatted string to list of tags.
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
        /// Load Visual Novel Form with details of visual novel that was left clicked.
        /// </summary>
        private void VisualNovelLeftClick(object sender, CellClickEventArgs e)
        {
            var listView = (ObjectListView)sender;
            if (listView.SelectedIndices.Count <= 0) return;
            var vnItem = (ListedVN)listView.SelectedObjects[0];
            DBConn.Open();
            vnItem = DBConn.GetSingleVN(vnItem.VNID, UserID);
            DBConn.Close();
            var vnf = new VisualNovelForm(vnItem, this);
            vnf.Show(this);
        }

        /// <summary>
        /// Update result label when items in Visual Novel OLV are changed.
        /// </summary>
        private void objectList_ItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            if (tileOLV.FilteredObjects == null) return;
            var count = tileOLV.FilteredObjects.Cast<object>().Count();
            var totalcount = tileOLV.Objects.Cast<object>().Count();
            resultLabel.Text = tileOLV.ModelFilter != null
                ? $"{count}/{totalcount} items."
                : $"{tileOLV.Items.Count} items.";
            DisplayCommonTags(null, null);
        }

        /// <summary>
        /// Decompress GZip file.
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
                        Console.WriteLine($"Decompressed: {fileToDecompress}");
                    }
                }
            }
        }

        /// <summary>
        /// Get Days passed since date of last update.
        /// </summary>
        /// <param name="updatedDate">Date of last update</param>
        /// <returns>Number of days since last update</returns>
        public static int DaysSince(DateTime updatedDate)
        {
            if (updatedDate == DateTime.MinValue) return -1;
            var days = (DateTime.Today - updatedDate).Days;
            return days;
        }

        /// <summary>
        /// Get new VNDB Stats from VNDB using API.
        /// </summary>
        private void GetNewDBStats()
        {
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
        /// Load VNDB Stats from XML file.
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
        /// Get new Tag dump file from VNDB.org
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
            Settings.Default.TagdumpUpdate = DateTime.UtcNow;
            Settings.Default.Save();
            LoadTagdump();
        }

        /// <summary>
        /// Load Tags from Tag dump file.
        /// </summary>
        private void LoadTagdump()
        {
            if (!File.Exists(TagsJson))
                GetNewTagdump();
            else
                try
                {
                    PlainTags = JsonConvert.DeserializeObject<List<WrittenTag>>(File.ReadAllText(TagsJson));
                }
                catch (JsonReaderException e)
                {
                    Debug.Print(e.Message);
                    Debug.Print($"{TagsJson} could not be read, deleting it and getting new one.");
                    File.Delete(TagsJson);
                    GetNewTagdump();
                }
        }
        
        /// <summary>
        /// Adjust tile size on OLV resize to avoid empty spaces.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tileOLV_Resize(object sender, EventArgs e)
        {
            var width = tileOLV.Width - 24;
            var s = (int)Math.Round((double)width / 230);
            if (s == 0) return;
            tileOLV.TileSize = new Size(width / s, 300);
        }

        /// <summary>
        /// Save custom filters and other filter settings to XML.
        /// </summary>
        private void SaveMainXML()
        {
            XmlHelper.ToXmlFile(new MainXml(_customFilters, Toggles), MainXmlFile);
        }

        /// <summary>
        /// Refresh VN OLV.
        /// </summary>
        private void RefreshVNList()
        {
            tileOLV.SetObjects(_vnList.Where(_currentList));
        }

        /// <summary>
        /// Save user's VNDB login credentials to Windows Registry (encrypted).
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
            Debug.Print("Saved Login Credentials");
        }

        /// <summary>
        /// Load user's VNDB login credentials from Windows Registry
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
            Debug.Print("Loaded Login Credentials");
            return new KeyValuePair<string, char[]>(username, Encoding.UTF8.GetChars(vv));
        }

        #endregion

        #region Press Enter On Text Boxes

        private void tagSearchBox_KeyDown(object sender, KeyEventArgs e) //press enter on tag search
        {
            if (e.KeyCode != Keys.Enter) return;
            if (tagSearchBox.Text == "") //check if box is empty
            {
                replyText.ForeColor = Color.Red;
                replyText.Text = Resources.enter_user_id;
                return;
            }
            var tagName = tagSearchBox.Text;
            AddFilterTag(tagName);
            var s = (Control)sender;
            s.Text = "";
        }

        private void filterNameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SaveCustomFilter(sender, e);
        }

        private void searchButton_keyPress(object sender, KeyPressEventArgs e) //press enter on search button
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                VNSearch(sender, e);
            }
        }

        private void yearBox_KeyDown(object sender, KeyEventArgs e) //press enter on get year titles box
        {
            if (e.KeyCode == Keys.Enter) GetYearTitles(sender, e);
        }

        #endregion

        #region Classes/Enums
        /// <summary>
        /// Holds details of user-created custom filter
        /// </summary>
        [Serializable, XmlRoot("ComplexFilter")]
        public class ComplexFilter
        {
            /// <summary>
            /// Constructor for ComplexFilter (Custom Filter).
            /// </summary>
            /// <param name="name">User-set name of filter</param>
            /// <param name="filters">List of Tags in filter</param>
            public ComplexFilter(string name, List<TagFilter> filters)
            {
                Name = name;
                Filters = filters;
            }

            /// <summary>
            /// Empty Constructor needed for XML.
            /// </summary>
            public ComplexFilter()
            {
            }

            /// <summary>
            /// User-set name of custom filter
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// List of tags in custom filter
            /// </summary>
            public List<TagFilter> Filters { get; set; }
            /// <summary>
            /// Date of last update to custom filter
            /// </summary>
            public DateTime Updated { get; set; }
        }

        /// <summary>
        /// Holds details of a VNDB Tag and its subtags
        /// </summary>
        [Serializable, XmlRoot("TagFilter")]
        public class TagFilter
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="name"></param>
            /// <param name="titles"></param>
            /// <param name="children"></param>
            public TagFilter(int id, string name, int titles, int[] children)
            {
                ID = id;
                Name = name;
                Titles = titles;
                Children = children;
                AllIDs = children.Union(new[] { id }).ToArray();
            }

            /// <summary>
            /// Empty Constructor needed for XML.
            /// </summary>
            public TagFilter()
            {
            }

            /// <summary>
            /// ID of tag.
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// Name of tag.
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Number of titles with tag.
            /// </summary>
            public int Titles { get; set; }
            /// <summary>
            /// Subtag IDs of tag.
            /// </summary>
            public int[] Children { get; set; }
            /// <summary>
            /// Tag ID and subtag IDs
            /// </summary>
            public int[] AllIDs { get; set; }


            /// <summary>
            /// Check if given tag is a child tag of TagFilter
            /// </summary>
            /// <param name="tag">Tag to be checked</param>
            /// <returns>Whether tag is child of TagFilter</returns>
            public bool HasChild(int tag)
            {
                return Children.Contains(tag);
            }


            /// <summary>Returns a string that represents the current object.</summary>
            /// <returns>A string that represents the current object.</returns>
            /// <filterpriority>2</filterpriority>
            public override string ToString()
            {
                return $"{ID} - {Name}";
            }
        }

        /// <summary>
        /// Class for drawing individual tiles in ObjectListView
        /// </summary>
        public class VNTileRenderer : AbstractRenderer
        {
            internal Pen BorderPen = new Pen(Color.FromArgb(0x33, 0x33, 0x33));
            internal Brush HeaderBackBrush = new SolidBrush(Color.FromArgb(0x33, 0x33, 0x33));
            internal Brush HeaderTextBrush = Brushes.AliceBlue;

            /// <summary>
            /// Render the whole item within an ObjectListView. This is only used in non-Details views.
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

                if (e.Item.Selected)
                {
                    BorderPen = Pens.Blue;
                    HeaderBackBrush = new SolidBrush(olv.BackColor);
                }
                else
                {
                    BorderPen = new Pen(Color.FromArgb(0x33, 0x33, 0x33));
                    HeaderBackBrush = new SolidBrush(Color.FromArgb(0x33, 0x33, 0x33));
                }
                DrawVNTile(g, itemBounds, rowObject, olv, (OLVListItem)e.Item);

                // Finally render the buffered graphics
                buffered.Render();
                buffered.Dispose();

                // Return true to say that we've handled the drawing
                return true;
            }

            /// <summary>
            /// Draw Visual Novel Tile, displayed in Visual Novels Object List View.
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
                Brush backBrush = Brushes.LightBlue;
                //tile size 230,375
                //image 230-spacing, 300-spacing
                const int spacing = 8;
                var font = new Font("Microsoft Sans Serif", 8.25f);
                var boldFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
                var fmt = new StringFormat(StringFormatFlags.NoWrap)
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
                var size = g.MeasureString("Wj", font, itemBounds.Width, fmt);
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
                g.DrawPath(BorderPen, path);
                g.Clip = new Region(itemBounds);
                // Draw the photo
                var photoRect = itemBounds;
                photoRect.Inflate(-spacing, -spacing);
                if (vn == null) return;
                var id = vn.VNID.ToString();
                var imageUrl = vn.ImageURL;
                var ext = Path.GetExtension(imageUrl);
                photoRect.Height = (int)(itemBounds.Height - 3 * size.Height - spacing * 2);
                var rectratio = (double)photoRect.Width / photoRect.Height;
                var photoFile = string.Format($"vnImages\\{id}{ext}");
                if (vn.ImageNSFW && !Settings.Default.ShowNSFWImages) g.DrawImage(Resources.nsfw_image, photoRect);
                else if (File.Exists(photoFile))
                {
                    var photo = Image.FromFile(photoFile);
                    var photoratio = (double)photo.Width / photo.Height;
                    //zoom in image to occupy whole area
                    //Alternately show whole image but do not occupy whole area
                    if (photoratio > rectratio) //if image is wider
                    {
                        var shrinkratio = (double)photo.Width / photoRect.Width;
                        var newWidth = photoRect.Width;
                        var newHeight = (int)Math.Floor(photo.Height / shrinkratio);
                        var newX = photoRect.X;
                        var hny = (double)newHeight / 2;
                        var hph = (double)photoRect.Height / 2;
                        var newY = photoRect.Y + (int)Math.Floor(hph) - (int)Math.Floor(hny);
                        var newPhotoRect = new Rectangle(newX, newY, newWidth, newHeight);
                        g.DrawImage(photo, newPhotoRect);
                    }
                    else //if image is taller
                    {
                        var shrinkratio = (double)photo.Height / photoRect.Height;
                        var newWidth = (int)Math.Floor(photo.Width / shrinkratio);
                        var newHeight = photoRect.Height;
                        var hnx = (double)newWidth / 2;
                        var hpw = (double)photoRect.Width / 2;
                        var newX = photoRect.X + (int)Math.Floor(hpw) - (int)Math.Floor(hnx);
                        var newY = photoRect.Y;
                        var newPhotoRect = new Rectangle(newX, newY, newWidth, newHeight);
                        g.DrawImage(photo, newPhotoRect);
                    }
                }
                else g.DrawImage(Resources.no_image, photoRect);
                // Now draw the text portion
                RectangleF textBoxRect = photoRect;
                textBoxRect.Y += photoRect.Height + spacing;
                textBoxRect.Width = itemBounds.Right - textBoxRect.X - spacing;
                // Draw the other bits of information
                textBoxRect.Height = size.Height;
                fmt.Alignment = StringAlignment.Near;
                g.DrawString(vn.Title, boldFont, textBrush, textBoxRect, fmt);
                textBoxRect.Y += size.Height;
                g.DrawString(vn.RelDate, font, textBrush, textBoxRect, fmtFar);
                var producerBrush = textBrush;
                List<ListedProducer> producers = Application.OpenForms.OfType<FormMain>().Select(main => main.olFavoriteProducers.Objects as List<ListedProducer>).FirstOrDefault();
                if (producers != null && producers.Exists(x => x.Name == vn.Producer)) producerBrush = Brushes.GreenYellow;
                g.DrawString(vn.Producer, font, producerBrush, textBoxRect, fmt);
                textBoxRect.Y += size.Height;
                string[] parts = { "", "", "" };
                if (!vn.ULStatus.Equals(""))
                {
                    parts[0] = "Userlist: ";
                    parts[1] = vn.ULStatus;
                }
                else if (!vn.WLStatus.Equals(""))
                {
                    parts[0] = "Wishlist: ";
                    parts[1] = vn.WLStatus;
                }
                if (Convert.ToInt32(vn.Vote) > 0)
                    parts[2] = $" (Vote: {vn.Vote})";
                var complete = string.Join(" ", parts);
                g.DrawString(complete, font, textBrush, textBoxRect, fmt);
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

            /// <summary>
            /// Scales an Image to fit the given dimensions.
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

        /// <summary>
        /// Class For XML File, holding saved objects.
        /// </summary>
        [Serializable, XmlRoot("MainXml")]
        public class MainXml
        {
            /// <summary>
            /// List of User-created custom filters.
            /// </summary>
            public List<ComplexFilter> ComplexFilters { get; set; }
            /// <summary>
            /// Current state of list filters.
            /// </summary>
            public ToggleArray XmlToggles { get; set; }

            /// <summary>
            /// Empty constructor needed for XML.
            /// </summary>
            public MainXml()
            {
                ComplexFilters = new List<ComplexFilter>();
                XmlToggles = new ToggleArray();
            }

            /// <summary>
            /// Constructor For Main XML File.
            /// </summary>
            /// <param name="customFilters">List of user-set custom filters</param>
            /// <param name="xmlToggleArray">Current list toggle settings</param>
            public MainXml(List<ComplexFilter> customFilters, ToggleArray xmlToggleArray)
            {
                ComplexFilters = customFilters;
                XmlToggles = xmlToggleArray;
            }
        }

        /// <summary>
        /// Command to change VN status.
        /// </summary>
        internal enum Command
        {
            New,
            Update,
            Delete
        }

        /// <summary>
        /// Type of VN status to be changed.
        /// </summary>
        internal enum ChangeType
        {
            UL,
            WL,
            Vote
        }


        #endregion

        /// <summary>
        /// Update titles to include all fields in latest version of Happy Search.
        /// </summary>
        private async void UpdateTitlesToLatestVersionClick(object sender, EventArgs e)
        {
            //popularity, rating and votecount were added, check for votecount
            int[] listOfTitlesFromOldVersions = _vnList.Where(x => x.VoteCount == -1).Select(x => x.VNID).ToArray();
            var messageBox = 
                MessageBox.Show($"{listOfTitlesFromOldVersions.Length} need to be updated, if this is a large number (over 2000), it may take a while, are you sure?",
                Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (messageBox != DialogResult.Yes) return;
            await UpdateTitlesToLatestVersion(listOfTitlesFromOldVersions);
            ReloadLists();
            RefreshVNList();
            WriteText(userListReply,$"Updated {_vnsAdded} to latest version.");
        }

        /// <summary>
        /// Update tags of titles that haven't been updated in over 7 days.
        /// </summary>
        private async void UpdateTitleTagsClick(object sender, EventArgs e)
        {
            int[] listOfTitlesToUpdate = _vnList.Where(x => x.UpdatedDate > -1).Select(x=> x.VNID).ToArray();
            var messageBox = MessageBox.Show($"{listOfTitlesToUpdate.Length} need to be updated, if this is a large number (over 2000), it may take a while, are you sure?",
                Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (messageBox != DialogResult.Yes) return;
            await UpdateTitleTags(listOfTitlesToUpdate);
            ReloadLists();
            RefreshVNList();
            WriteText(userListReply, $"Updated tags of {_vnsAdded} titles.");
        }
    }
}