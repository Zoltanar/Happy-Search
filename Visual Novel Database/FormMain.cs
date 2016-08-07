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
    public partial class FormMain : Form
    {
        private const string VNImagesFolder = "vnImages\\";
        private const string DBStatsXml = "dbs.xml";
        private const string TagsJsonGz = "tags.json.gz";
        private const string TagsJson = "tags.json";
        private const string TagTypeAll = "checkBox";
        private const string TagTypeUrt = "mctULLabel";
        private const string FilterLabel = "filterLabel";
        internal const string ClientName = "Happy Search By Zolty";
        internal const string ClientVersion = "1.00";
        internal const string APIVersion = "2.25";
        private const int LabelFadeTime = 5000; //ms for text to disappear (not actual fade)
        private const string TagsURL = "http://vndb.org/api/tags.json.gz";
        private const string CustomfiltersXml = "customfilters.xml";
        private static List<ListedVN> _vnList;
        private static List<ListedProducer> _producerList;
        private static List<int> _filterIDList = new List<int>();
        private static List<TagFilter> _activeFilter = new List<TagFilter>();
        private static List<ComplexFilter> _customFilters;
        private static readonly Color ErrorColor = Color.Red;
        internal static readonly Color NormalColor = SystemColors.ControlLightLight;
        internal static readonly Color NormalLinkColor = Color.FromArgb(0, 192, 192);
        private static readonly Color WarningColor = Color.YellowGreen;
        private readonly Func<ListedVN, bool>[] _filters = {x => true, x => true, x => true};
        internal readonly VndbConnection Conn = new VndbConnection();
        internal readonly DbHelper DBConn;
        private int _added;
        private Func<ListedVN, bool> _currentList = x => true;
        private bool _fullyLoaded;
        private IEnumerable<int> _producerIDList;
        private int _skipped;
        private List<int> _useridList;
        private IEnumerable<int> _vnIDList;
        internal List<WrittenTag> PlainTags; //Contains all plain_tags, meaning the loaded tagdump.json
        internal ListedVN UpdatingVN;
        internal int UserID;
        internal List<ListedVN> UserList;

        private void VNSearchButt(object sender, EventArgs e) //Fetch information from 'VNDB.org'
        {
            //TODO Whole method
            WriteError(replyText, "Not yet implemented", true);
        }

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
            WriteText(replyText, $"Getting All VNs For year {year}\nStarted at {startTime}");
            ReloadLists();
            _currentList = x => x.RelDate.StartsWith(yearBox.Text);
            _added = 0;
            _skipped = 0;
            string vnInfoQuery =
                $"get vn basic (released > \"{year - 1}\" and released <= \"{year}\") {{\"results\":25}}";
            var result = await TryQuery(vnInfoQuery, Resources.gyt_query_error, replyText, true, true);
            if (!result) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            List<VNItem> vnItems = vnRoot.Items;
            foreach (var vnid in vnItems.Select(x => x.ID)) await GetSingleVN(vnid, replyText, false, true, true);
            var pageNo = 1;
            var moreResults = vnRoot.More;
            while (moreResults)
            {
                pageNo++;
                string vnInfoMoreQuery =
                    $"get vn basic (released > \"{year - 1}\" and released <= \"{year}\") {{\"results\":25, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(vnInfoMoreQuery, Resources.gyt_query_error, replyText, true, true);
                if (!moreResult) return;
                var vnMoreRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                List<VNItem> vnMoreItems = vnMoreRoot.Items;
                foreach (var vnid in vnMoreItems.Select(x => x.ID))
                    await GetSingleVN(vnid, replyText, false, true, true);
                moreResults = vnMoreRoot.More;
            }
            var endTime = DateTime.UtcNow.ToLocalTime().ToString("HH:mm");
            WriteText(replyText,
                $"Got all VNs for {year}\nTime:{startTime}-{endTime}\n{_added} added, {_skipped} skipped.");
            RefreshList();
        }

        private void Test(object sender, EventArgs e)
        {
        }

        private async void HandleContextItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
            if (success) SetOLV();
            WriteText(replyText, success ? Resources.item_changed : "Unknown Error");
        }

        private void LogInDialog(object sender, EventArgs e)
        {
            DialogResult = new LoginForm(this).ShowDialog();
            Debug.Print(DialogResult.ToString());
            if (DialogResult != DialogResult.OK) return;
            Settings.Default.UserID = UserID;
            Settings.Default.Save();
            UpdateUserStats();
            SetOLV();
        }

        /*credits and resources
        ObjectListView by Phillip Piper (GPLv3)from http://www.codeproject.com/Articles/16009/A-Much-Easier-to-Use-ListView
        (slightly modified) A Pretty Good Splash Screen in C# by Tom Clement (CPOL) from http://www.codeproject.com/Articles/5454/A-Pretty-Good-Splash-Screen-in-C
        (reasonably modified) VndbClient by FredTheBarber, for connection/queries to VNDB API https://github.com/FredTheBarber/VndbClient
        */

        #region Initialization

        public FormMain()
        {
            InitializeComponent();
            SplashScreen.SplashScreen.SetStatus("Initializing Controls...");
            {
                ULStatusDropDown.SelectedIndex = 0;
                customFilters.SelectedIndex = 0;
                viewPicker.SelectedIndex = 0;
                ULStatusDropDown.SelectedIndexChanged += Filter_ULStatus;
                customFilters.SelectedIndexChanged += Filter_Custom;
                replyText.Text = "";
                userListReply.Text = "";
                resultLabel.Text = "";
                loginReply.Text = "";
                prodReply.Text = "";
                filterReply.Text = "";
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
                UserList = DBConn.GetUserRelatedTitles(UserID);
                DBConn.Close();
                Debug.Print("VN Items= " + _vnList.Count);
                Debug.Print("Producers= " + _producerList.Count);
                Debug.Print("UserRelated Items= " + UserList.Count);
                _vnIDList = _vnList.Select(x => x.VNID);
                _producerIDList = _producerList.Select(x => x.ID);
                var producerFilterSource = new AutoCompleteStringCollection();
                producerFilterSource.AddRange(_producerList.Select(v => v.Name).ToArray());
                ProducerFilterBox.AutoCompleteCustomSource = producerFilterSource;
            }
            SplashScreen.SplashScreen.SetStatus("Updating User Stats...");
            {
                UpdateUserStats();
            }
            SplashScreen.SplashScreen.SetStatus("Adding VNs to Object Lists...");
            {
                tileOLV.SetObjects(_vnList);
                tileOLV.Sort(tileColumnDate, SortOrder.Descending);
            }
            SplashScreen.SplashScreen.SetStatus("Loading Custom Filters...");
            {
                _customFilters = File.Exists(CustomfiltersXml)
                    ? XmlHelper.FromXmlFile<List<ComplexFilter>>(CustomfiltersXml)
                    : new List<ComplexFilter>();
                foreach (var filter in _customFilters) customFilters.Items.Add(filter.Name);
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
                Console.WriteLine($"URTUpdate= {Settings.Default.URTUpdate}");
                if (DaysSince(Settings.Default.URTUpdate) > 2 || DaysSince(Settings.Default.URTUpdate) == -1)
                {
                    Task.Run(UpdateURT);
                    Settings.Default.URTUpdate = DateTime.UtcNow;
                    Settings.Default.Save();
                }
                else
                {
                    Debug.Print("Update not needed.");
                    Console.WriteLine(@"Update not needed.");
                }
            }
        }

        private void UpdateUserStats()
        {
            if (!UserList.Any())
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
            foreach (var item in UserList)
            {
                if (item.ULStatus.Length > 0) ulCount++;
                if (item.WLStatus.Length > 0) wlCount++;
                if (!(item.Vote > 0)) continue;
                vlCount++;
                cumulativeScore += item.Vote;
            }
            ulstatsall.Text = UserList.Count.ToString();
            ulstatsul.Text = ulCount.ToString();
            ulstatswl.Text = wlCount.ToString();
            ulstatsvl.Text = vlCount.ToString();
            ulstatsavs.Text = (cumulativeScore/vlCount).ToString("#.##");
            DisplayCommonTagsULStats(null, null);
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            Conn.Close();
        }

        internal void APILogin()
        {
            Conn.Login(ClientName, ClientVersion);
            switch (Conn.LastResponse.Type)
            {
                case ResponseType.Ok:
                    loginReply.ForeColor = Color.LightGreen;
                    loginReply.Text = UserID > 0 ? $"Connected with ID {UserID}." : "Connected without ID.";
                    Console.WriteLine(loginReply.Text);
                    ChangeAPIStatus(Conn.Status);
                    return;
                case ResponseType.Error:
                    if (Conn.LastResponse.Error.ID.Equals("loggedin"))
                    {
                        //should never happen
                        loginReply.ForeColor = Color.LightGreen;
                        loginReply.Text = Resources.already_logged_in;
                        break;
                    }
                    loginReply.ForeColor = Color.Red;
                    loginReply.Text = Resources.connection_failed;
                    break;
                default:
                    loginReply.ForeColor = Color.Red;
                    loginReply.Text = Resources.login_unknown_error;
                    break;
            }
            serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
            ChangeAPIStatus(Conn.Status);
        }

        internal void APILoginWithCredentials(KeyValuePair<string, char[]> credentials)
        {
            Conn.Login(ClientName, ClientVersion, credentials.Key, credentials.Value);
            switch (Conn.LastResponse.Type)
            {
                case ResponseType.Ok:
                    ChangeAPIStatus(Conn.Status);
                    loginReply.ForeColor = Color.LightGreen;
                    loginReply.Text = $"Logged in as {credentials.Key}.";
                    return;
                case ResponseType.Error:
                    if (Conn.LastResponse.Error.ID.Equals("loggedin"))
                    {
                        //should never happen
                        loginReply.ForeColor = Color.LightGreen;
                        loginReply.Text = Resources.already_logged_in;
                        break;
                    }
                    if (Conn.LastResponse.Error.ID.Equals("auth"))
                    {
                        loginReply.ForeColor = Color.Red;
                        loginReply.Text = Conn.LastResponse.Error.Msg;
                        break;
                    }
                    loginReply.ForeColor = Color.Red;
                    loginReply.Text = Resources.connection_failed;
                    break;
                default:
                    loginReply.ForeColor = Color.Red;
                    loginReply.Text = Resources.login_unknown_error;
                    break;
            }
            serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
            ChangeAPIStatus(Conn.Status);
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

        #endregion

        #region Get User-Related Titles

        //Get user's user/wish/votelists from VNDB
        private async void UpdateURTButtonClick(object sender, EventArgs e)
        {
            if (UserID < 1)
            {
                WriteText(userListReply, Resources.set_userid_first);
                return;
            }
            DBConn.Open();
            _useridList = DBConn.GetUserRelatedIDs(UserID);
            DBConn.Close();
            var askBox =
                MessageBox.Show(
                    $@"This may take a while depending on how many titles are in your lists.
Are you sure?

You currently have {_useridList
                        .Count} items in the local database, they can
be displayed by clicking the User Related Titles (URT) filter below.",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            await UpdateURT();
            WriteText(userListReply, Resources.gurt_success);
        }

        private async Task UpdateURT()
        {
            if (UserID < 1) return;
            DBConn.Open();
            _useridList = DBConn.GetUserRelatedIDs(UserID);
            DBConn.Close();
            Debug.Print($"Starting GetUserRelatedTitles for {UserID}, previously had {_useridList.Count} titles.");
            await GetUserList();
            await GetWishList();
            await GetVoteList();
            await GetRemainingTitles();
            DBConn.Open();
            _vnList = DBConn.GetAllTitles(UserID);
            DBConn.Close();
            RefreshList();
            var favprolist = new List<ListedProducer>();
            foreach (ListedProducer producer in olFavoriteProducers.Objects)
            {
                ListedVN[] producerVNs = UserList.Where(x => x.Producer.Equals(producer.Name)).ToArray();
                double userDropRate = -1;
                double userAverageVote = -1;
                if (producerVNs.Any())
                {
                    var finishedCount = producerVNs.Count(x => x.ULStatus.Equals("Finished"));
                    var droppedCount = producerVNs.Count(x => x.ULStatus.Equals("Dropped"));
                    ListedVN[] producerVotedVNs = producerVNs.Where(x => x.Vote > 0).ToArray();
                    userDropRate = finishedCount + droppedCount != 0
                        ? (double) droppedCount/(droppedCount + finishedCount)
                        : -1;
                    userAverageVote = producerVotedVNs.Any() ? producerVotedVNs.Select(x => x.Vote).Average() : -1;
                }
                favprolist.Add(new ListedProducer(producer.Name, producer.NumberOfTitles, producer.Loaded,
                    DateTime.UtcNow, producer.ID, userAverageVote, (int) Math.Round(userDropRate*100)));
            }
            DBConn.Open();
            DBConn.InsertFavoriteProducers(favprolist, UserID);
            DBConn.Close();
            tileOLV.Sort(tileColumnDate, SortOrder.Descending);
            UpdateUserStats();
            var ulCount = UserList.Count;
            if (ulCount > 0) WriteText(userListReply, $"Found {ulCount} titles.");
            else WriteError(userListReply, Resources.no_results);
        }

        private async Task GetUserList()
        {
            Debug.Print("Starting GetUserList");
            DBConn.Open();
            _useridList = DBConn.GetUserRelatedIDs(UserID);
            DBConn.Close();
            string userListQuery = $"get vnlist basic (uid = {UserID} ) {{\"results\":100}}";
            //1 - fetch from VNDB using API
            var result = await TryQuery(userListQuery, Resources.gul_query_error, userListReply);
            if (!result) return;
            var ulRoot = JsonConvert.DeserializeObject<UserListRoot>(Conn.LastResponse.JsonPayload);
            if (ulRoot.Num == 0) return;
            List<UserListItem> ulList = ulRoot.Items; //make list of vns in list
            var pageNo = 1;
            var moreResults = ulRoot.More;
            while (moreResults)
            {
                pageNo++;
                string userListQuery2 = $"get vnlist basic (uid = {UserID} ) {{\"results\":100, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(userListQuery2, Resources.gul_query_error, userListReply);
                if (!moreResult) return;
                var ulMoreRoot = JsonConvert.DeserializeObject<UserListRoot>(Conn.LastResponse.JsonPayload);
                ulList.AddRange(ulMoreRoot.Items);
                moreResults = ulMoreRoot.More;
            }
            DBConn.Open();
            foreach (var item in ulList)
            {
                DBConn.UpsertUserList(UserID, item, _useridList.Contains(item.VN));
                if (!_useridList.Contains(item.VN)) _useridList.Add(item.VN);
            }
            DBConn.Close();
        }

        private async Task GetWishList()
        {
            Debug.Print("Starting GetWishList");
            DBConn.Open();
            _useridList = DBConn.GetUserRelatedIDs(UserID);
            DBConn.Close();
            string wishListQuery = $"get wishlist basic (uid = {UserID} ) {{\"results\":100}}";
            var result = await TryQuery(wishListQuery, Resources.gwl_query_error, userListReply);
            if (!result) return;
            var wlRoot = JsonConvert.DeserializeObject<WishListRoot>(Conn.LastResponse.JsonPayload);
            if (wlRoot.Num == 0) return;
            List<WishListItem> wlList = wlRoot.Items; //make list of vn in list
            var pageNo = 1;
            var moreResults = wlRoot.More;
            while (moreResults)
            {
                pageNo++;
                string wishListQuery2 = $"get wishlist basic (uid = {UserID} ) {{\"results\":100, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(wishListQuery2, Resources.gwl_query_error, userListReply);
                if (!moreResult) return;
                var wlMoreRoot = JsonConvert.DeserializeObject<WishListRoot>(Conn.LastResponse.JsonPayload);
                wlList.AddRange(wlMoreRoot.Items);
                moreResults = wlMoreRoot.More;
            }
            DBConn.Open();
            foreach (var item in wlList)
            {
                DBConn.UpsertWishList(UserID, item, _useridList.Contains(item.VN));
                if (!_useridList.Contains(item.VN)) _useridList.Add(item.VN);
            }
            DBConn.Close();
        }

        private async Task GetVoteList()
        {
            Debug.Print("Starting GetVoteList");
            DBConn.Open();
            _useridList = DBConn.GetUserRelatedIDs(UserID);
            DBConn.Close();
            string voteListQuery = $"get votelist basic (uid = {UserID} ) {{\"results\":100}}";
            var result = await TryQuery(voteListQuery, Resources.gvl_query_error, userListReply);
            if (!result) return;
            var vlRoot = JsonConvert.DeserializeObject<VoteListRoot>(Conn.LastResponse.JsonPayload);
            if (vlRoot.Num == 0) return;
            List<VoteListItem> vlList = vlRoot.Items; //make list of vn in list
            var pageNo = 1;
            var moreResults = vlRoot.More;
            while (moreResults)
            {
                pageNo++;
                string voteListQuery2 = $"get votelist basic (uid = {UserID} ) {{\"results\":100, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(voteListQuery2, Resources.gvl_query_error, userListReply);
                if (!moreResult) return;
                var vlMoreRoot = JsonConvert.DeserializeObject<VoteListRoot>(Conn.LastResponse.JsonPayload);
                vlList.AddRange(vlMoreRoot.Items);
                moreResults = vlMoreRoot.More;
            }
            DBConn.Open();
            foreach (var item in vlList)
            {
                DBConn.UpsertVoteList(UserID, item, _useridList.Contains(item.VN));
                if (!_useridList.Contains(item.VN)) _useridList.Add(item.VN);
            }
            DBConn.Close();
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

        #region Favorite Producers

        private void LoadFavoriteProducerList()
        {
            if (UserID < 1) return;
            DBConn.Open();
            olFavoriteProducers.SetObjects(DBConn.GetFavoriteProducersForUser(UserID));
            DBConn.Close();
            olFavoriteProducers.Sort(0);
        }

        private void AddProducers(object sender, EventArgs e)
        {
            if (UserID < 1)
            {
                WriteText(prodReply, Resources.set_userid_first);
                return;
            }
            new ProducerSearchForm(this).ShowDialog();
            LoadFavoriteProducerList();
        }

        private async Task GetProducerTitles(ListedProducer producer, Label replyLabel, bool updateAll = false)
        {
            Debug.Print($"Getting Titles for Producer {producer.Name}");
            string prodReleaseQuery = $"get release vn (producer={producer.ID}) {{\"results\":25}}";
            var result = await TryQuery(prodReleaseQuery, Resources.upt_query_error, replyLabel);
            if (!result) return;
            var releaseRoot = JsonConvert.DeserializeObject<ReleasesRoot>(Conn.LastResponse.JsonPayload);
            List<ReleaseItem> releaseItems = releaseRoot.Items;
            var vnList = new List<VNItem>();
            foreach (var item in releaseItems) vnList.AddRange(item.VN);
            var moreResults = releaseRoot.More;
            var pageNo = 1;
            while (moreResults)
            {
                pageNo++;
                string prodReleaseMoreQuery =
                    $"get release vn (producer={producer.ID}) {{\"results\":25, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(prodReleaseMoreQuery, Resources.upt_query_error, replyLabel);
                if (!moreResult) return;
                var releaseMoreRoot = JsonConvert.DeserializeObject<ReleasesRoot>(Conn.LastResponse.JsonPayload);
                List<ReleaseItem> releaseMoreItems = releaseMoreRoot.Items;
                foreach (var item in releaseMoreItems) vnList.AddRange(item.VN);
                moreResults = releaseMoreRoot.More;
            }
            List<int> producerIDList = vnList.Distinct().Select(x => x.ID).ToList();
            Debug.Print($"Found {producerIDList.Count} titles.");
            foreach (var vnid in producerIDList)
            {
                if (_vnIDList.Contains(vnid)) _skipped++;
                else _added++;
                if (!updateAll && _vnIDList.Contains(vnid)) continue;
                await UpdateProducerVN(vnid, producer.ID);
            }
            DBConn.Open();
            DBConn.InsertProducer(new ListedProducer(producer.Name, producerIDList.Count, "Yes", DateTime.UtcNow,
                producer.ID));
            DBConn.Close();
            Debug.Print($"Finished getting titles for ProducerID= {producer}");
        }

        internal async Task UpdateProducerVN(int vnid, int producerid)
        {
            ReloadLists();
            string singleVNQuery = $"get vn basic,details,tags (id = {vnid})";
            var result = await TryQuery(singleVNQuery, Resources.svn_query_error, prodReply);
            if (!result) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            if (vnRoot.Num == 0) return;
            var vnItem = vnRoot.Items[0];
            SaveImage(vnItem);
            DBConn.Open();
            DBConn.UpsertSingleVN(vnItem, producerid, false);
            DBConn.Close();
        }

        private void RemoveProducers(object sender, EventArgs e)
        {
            if (olFavoriteProducers.SelectedObjects.Count == 0)
            {
                WriteError(prodReply, Resources.no_items_selected, true);
                return;
            }
            DBConn.Open();
            foreach (ListedProducer item in olFavoriteProducers.SelectedObjects)
            {
                DBConn.RemoveFavoriteProducer(item.ID, UserID);
            }
            DBConn.Close();
            LoadFavoriteProducerList();
        }

        private async void UpdateAllFavoriteProducerTitles(object sender, EventArgs e)
        {
            if (olFavoriteProducers.Items.Count == 0)
            {
                WriteError(prodReply, "No Items in list.", true);
                return;
            }
            var vnCount = olFavoriteProducers.Objects.Cast<ListedProducer>().Sum(producer => producer.NumberOfTitles);
            var askBox =
                MessageBox.Show($"Are you sure you wish to reload {vnCount}+ VNs?\nThis may take a while...",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            _added = 0;
            _skipped = 0;
            foreach (ListedProducer producer in olFavoriteProducers.Objects)
                await GetProducerTitles(producer, prodReply, true);
            SetOLV();
            WriteText(prodReply, Resources.update_fp_titles_success + $" ({_added} new titles)");
        }

        private void ShowSelectedProducerVNs(object sender, EventArgs e)
        {
            if (olFavoriteProducers.SelectedItems.Count == 0)
            {
                WriteError(prodReply, Resources.no_items_selected, true);
                return;
            }
            IEnumerable<string> prodList = from ListedProducer producer in olFavoriteProducers.SelectedObjects
                select producer.Name;
            _currentList = vn => prodList.Contains(vn.Producer);
            RefreshList();
        }

        private async void LoadUnloaded(object sender, EventArgs e)
        {
            if (olFavoriteProducers.Items.Count == 0)
            {
                WriteError(prodReply, "No Items in list.", true);
                return;
            }
            List<ListedProducer> producers =
                olFavoriteProducers.Objects.Cast<ListedProducer>().Where(item => item.Loaded.Equals("No")).ToList();
            foreach (var producer in producers)
            {
                await GetProducerTitles(producer, prodReply);
            }
            SetOLV();
            WriteText(prodReply, $"Loaded {producers.Count} producers.");
        }

        private async void GetNewFavoriteProducerTitles(object sender, EventArgs e)
        {
            if (olFavoriteProducers.Items.Count == 0)
            {
                WriteError(prodReply, "No Items in list.", true);
                return;
            }
            var askBox =
                MessageBox.Show(Resources.get_new_fp_titles_confirm, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            List<ListedProducer> producers =
                olFavoriteProducers.Objects.Cast<ListedProducer>().Where(item => item.Updated > 2).ToList();
            Debug.Print($"{producers.Count} to be updated");
            foreach (var producer in producers) await GetProducerTitles(producer, prodReply);
            SetOLV();
            WriteText(prodReply, Resources.get_new_fp_titles_success);
        }

        private void FormatRowFavoriteProducers(object sender, FormatRowEventArgs e)
        {
            var listedProducer = (ListedProducer) e.Model;
            if (listedProducer.UserAverageVote < 1) e.Item.GetSubItem(2).Text = "";
            if (listedProducer.UserDropRate < 0) e.Item.GetSubItem(3).Text = "";
            if (listedProducer.NumberOfTitles == -1) e.Item.GetSubItem(1).Text = "";
        }

        #endregion

        #region API Methods

        internal async Task<bool> TryQuery(string query, string errorMessage, Label label,
            bool additionalMessage = false, bool refreshList = false)
        {
            if (Conn.Status != VndbConnection.APIStatus.Ready)
            {
                WriteError(label, "API Connection isn't ready.");
                return false;
            }
            //change status to busy until it is solved, if error is returned then status is changed to throttled or ready if the error isn't throttling error
            //if response type is unknown then status is changed to error because it's probably a connection loss and will require reconnecting.
            ChangeAPIStatus(VndbConnection.APIStatus.Busy);
            if (Settings.Default.Limit10Years && query.StartsWith("get vn ") && !query.Contains("id = "))
            {
                query = Regex.Replace(query, "\\)", $" and released > \"{DateTime.UtcNow.Year - 10}\")");
            }
            Debug.Print(query);
            await Conn.QueryAsync(query); //request detailed information
            serverQ.Text += query + Environment.NewLine;
            serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
            if (Conn.LastResponse.Type == ResponseType.Unknown)
            {
                ChangeAPIStatus(VndbConnection.APIStatus.Error);
                return false;
            }
            while (Conn.LastResponse.Type == ResponseType.Error)
            {
                if (!Conn.LastResponse.Error.ID.Equals("throttled"))
                {
                    WriteError(label, errorMessage);
                    ChangeAPIStatus(Conn.Status);
                    return false;
                }
                var waitS = Conn.LastResponse.Error.Minwait*30;
                var minWait = Math.Min(waitS, Conn.LastResponse.Error.Fullwait);
                string normalWarning = $"Throttled for {Math.Floor(minWait)} secs.";
                string additionalWarning = $" Added {_added} and skipped {_skipped} so far...";
                var fullThrottleMessage = additionalMessage ? normalWarning + additionalWarning : normalWarning;
                WriteWarning(label, fullThrottleMessage);
                ChangeAPIStatus(VndbConnection.APIStatus.Throttled);
                var waitMS = minWait*1000;
                var wait = Convert.ToInt32(waitMS);
                Debug.Print($"{DateTime.UtcNow} - {fullThrottleMessage}");
                if (refreshList) tileOLV.SetObjects(_vnList.Where(_currentList));
                await Task.Delay(wait);
                ClearLog(null, null);
                await Conn.QueryAsync(query); //request detailed information
                serverQ.Text += query + Environment.NewLine;
                serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
            }
            ChangeAPIStatus(VndbConnection.APIStatus.Ready);
            return true;
        }

        internal async Task UpdateSingleVN(int vnid, LinkLabel updateLink)
        {
            ReloadLists();
            string singleVNQuery = $"get vn basic,details,tags (id = {vnid})";
            var result = await TryQuery(singleVNQuery, Resources.svn_query_error, updateLink);
            if (!result) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            var vnItem = vnRoot.Items[0];
            var relProducer = -1;
            SaveImage(vnItem);
            //fetch developer from releases
            string relInfoQuery = $"get release producers (vn =\"{vnid}\")";
            var releaseResult = await TryQuery(relInfoQuery, Resources.svnr_query_error, updateLink);
            if (!releaseResult) return;
            var relInfo = JsonConvert.DeserializeObject<ReleasesRoot>(Conn.LastResponse.JsonPayload);
            List<ReleaseItem> relItem = relInfo.Items;
            if (relItem.Count != 0)
            {
                foreach (var item in relItem)
                {
                    relProducer = item.Producers.Find(x => x.Developer)?.ID ?? -1;
                    if (relProducer > 0) break;
                }
            }
            if (relProducer != -1 && !_producerIDList.Contains(relProducer))
            {
                //query api
                string producerQuery = $"get producer basic (id={relProducer})";
                var producerResult = await TryQuery(producerQuery, Resources.sp_query_error, updateLink);
                if (!producerResult) return;
                var root = JsonConvert.DeserializeObject<ProducersRoot>(Conn.LastResponse.JsonPayload);
                List<ProducerItem> producers = root.Items;
                DBConn.Open();
                foreach (var producer in producers)
                {
                    if (_producerIDList.Contains(producer.ID)) continue;

                    DBConn.InsertProducer(new ListedProducer(producer.Name, -1, "No", DateTime.UtcNow, producer.ID));
                }
                DBConn.Close();
            }
            DBConn.Open();
            DBConn.UpsertSingleVN(vnItem, relProducer, false);
            DBConn.Close();
            WriteText(updateLink, Resources.vn_updated);
            DBConn.Open();
            UpdatingVN = DBConn.GetSingleVN(vnid, UserID);
            DBConn.Close();
        }

        internal async Task GetSingleVN(int vnid, Label replyLabel, bool forceUpdate = false,
            bool additionalMessage = false, bool refreshList = false)
        {
            if (_vnIDList.Contains(vnid) && forceUpdate == false)
            {
                _skipped++;
                return;
            }
            //fetch visual novel information
            string singleVNQuery = $"get vn basic,details,tags (id = {vnid})";
            var result =
                await TryQuery(singleVNQuery, Resources.svn_query_error, replyLabel, additionalMessage, refreshList);
            if (!result) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            var vnItem = vnRoot.Items[0];
            var relProducer = -1;
            SaveImage(vnItem);
            //fetch developer from releases
            string relInfoQuery = $"get release producers (vn =\"{vnid}\")";
            var releaseResult =
                await TryQuery(relInfoQuery, Resources.gsvn_query_error, replyLabel, additionalMessage, refreshList);
            if (!releaseResult) return;
            var relInfo = JsonConvert.DeserializeObject<ReleasesRoot>(Conn.LastResponse.JsonPayload);
            List<ReleaseItem> relItem = relInfo.Items;
            if (relItem.Any())
            {
                foreach (var item in relItem)
                {
                    relProducer = item.Producers.Find(x => x.Developer)?.ID ?? -1;
                    if (relProducer > 0) break;
                }
            }
            //get producer information if not already present
            if (relProducer != -1 && !_producerIDList.Contains(relProducer))
            {
                string producerQuery = $"get producer basic (id={relProducer})";
                var producerResult =
                    await TryQuery(producerQuery, Resources.sp_query_error, replyLabel, additionalMessage, refreshList);
                if (!producerResult) return;
                var root = JsonConvert.DeserializeObject<ProducersRoot>(Conn.LastResponse.JsonPayload);
                List<ProducerItem> producers = root.Items;
                DBConn.Open();
                //insert all producers that weren't already present
                foreach (var producer in producers)
                {
                    if (_producerIDList.Contains(producer.ID)) continue;
                    DBConn.InsertProducer(new ListedProducer(producer.Name, -1, "No", DateTime.UtcNow, producer.ID));
                }
                DBConn.Close();
            }
            DBConn.Open();
            DBConn.UpsertSingleVN(vnItem, relProducer, false);
            DBConn.Close();
            _added++;
        }

        internal async Task GetMultipleVN(List<int> vnIDs, Label replyLabel, bool refreshList = false)
        {
            ReloadLists();
            foreach (var id in vnIDs)
            {
                if (_vnIDList.Contains(id))
                {
                    _skipped++;
                    continue;
                }
                string singleVNQuery = $"get vn basic,details,tags (id = {id})";
                var result = await TryQuery(singleVNQuery, Resources.svn_query_error, replyLabel, true, refreshList);
                if (!result) continue;
                var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                if (vnRoot.Num == 0) continue;
                var vnItem = vnRoot.Items[0];
                var relProducer = -1;
                SaveImage(vnItem);
                //fetch developer from releases
                string relInfoQuery = $"get release producers (vn =\"{id}\")";
                var releaseResult =
                    await TryQuery(relInfoQuery, Resources.svnr_query_error, replyLabel, true, refreshList);
                if (!releaseResult) continue;
                var relInfo = JsonConvert.DeserializeObject<ReleasesRoot>(Conn.LastResponse.JsonPayload);
                List<ReleaseItem> relItem = relInfo.Items;
                if (relItem.Any())
                {
                    foreach (var item in relItem)
                    {
                        relProducer = item.Producers.Find(x => x.Developer)?.ID ?? -1;
                        if (relProducer > 0) break;
                    }
                }
                if (relProducer != -1 && !_producerIDList.Contains(relProducer))
                {
                    //query api
                    string producerQuery = $"get producer basic (id={relProducer})";
                    var producerResult =
                        await TryQuery(producerQuery, Resources.sp_query_error, replyLabel, true, refreshList);
                    if (!producerResult) return;
                    var root = JsonConvert.DeserializeObject<ProducersRoot>(Conn.LastResponse.JsonPayload);
                    List<ProducerItem> producers = root.Items;
                    DBConn.Open();
                    foreach (var producer in producers)
                    {
                        if (_producerIDList.Contains(producer.ID)) continue;
                        DBConn.InsertProducer(new ListedProducer(producer.Name, -1, "No", DateTime.UtcNow, producer.ID));
                    }
                    DBConn.Close();
                }
                _added++;
                DBConn.Open();
                DBConn.UpsertSingleVN(vnItem, relProducer, false);
                DBConn.Close();
            }
        }

        private void ChangeAPIStatus(VndbConnection.APIStatus apiStatus)
        {
            switch (apiStatus)
            {
                case VndbConnection.APIStatus.Ready:
                    statusLabel.Text = @"Ready";
                    statusLabel.BackColor = Color.LightGreen;
                    break;
                case VndbConnection.APIStatus.Busy:
                    statusLabel.Text = @"Busy";
                    statusLabel.BackColor = Color.Khaki;
                    break;
                case VndbConnection.APIStatus.Throttled:
                    statusLabel.Text = @"Throttled";
                    statusLabel.BackColor = Color.Khaki;
                    break;
                case VndbConnection.APIStatus.Error:
                    statusLabel.Text = @"Error";
                    statusLabel.BackColor = Color.Red;
                    Conn.Close();
                    break;
            }
        }

        private async Task<bool> ChangeVNStatus(ListedVN vn, ChangeType type, int statusInt)
        {
            var hasULStatus = vn.ULStatus != null && !vn.ULStatus.Equals("");
            var hasWLStatus = vn.WLStatus != null && !vn.WLStatus.Equals("");
            var hasVote = vn.Vote > 0;
            string queryString;
            bool result;
            switch (type)
            {
                case ChangeType.UL:
                    queryString = statusInt == -1
                        ? $"set vnlist {vn.VNID}"
                        : $"set vnlist {vn.VNID} {{\"status\":{statusInt}}}";
                    result = await TryQuery(queryString, Resources.cvns_query_error, replyText);
                    if (!result) return false;
                    DBConn.Open();
                    if (hasWLStatus || hasVote)
                        DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.UL, statusInt, Command.Update);
                    else if (statusInt == -1)
                        DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.UL, statusInt, Command.Delete);
                    else DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.UL, statusInt, Command.New);
                    break;
                case ChangeType.WL:
                    queryString = statusInt == -1
                        ? $"set wishlist {vn.VNID}"
                        : $"set wishlist {vn.VNID} {{\"priority\":{statusInt}}}";
                    result = await TryQuery(queryString, Resources.cvns_query_error, replyText);
                    if (!result) return false;
                    DBConn.Open();
                    if (hasULStatus || hasVote)
                        DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.WL, statusInt, Command.Update);
                    else if (statusInt == -1)
                        DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.WL, statusInt, Command.Delete);
                    else DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.WL, statusInt, Command.New);
                    break;
                case ChangeType.Vote:
                    queryString = statusInt == -1
                        ? $"set votelist {vn.VNID}"
                        : $"set votelist {vn.VNID} {{\"vote\":{statusInt*10}}}";
                    result = await TryQuery(queryString, Resources.cvns_query_error, replyText);
                    if (!result) return false;
                    DBConn.Open();
                    if (hasULStatus || hasWLStatus)
                        DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.Vote, statusInt, Command.Update);
                    else if (statusInt == -1)
                        DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.Vote, statusInt, Command.Delete);
                    else DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.Vote, statusInt, Command.New);
                    break;
            }
            DBConn.Close();
            return true;
        }

        #endregion

        #region Tag Filtering

        private void TagFilterAdded(object sender, EventArgs e)
        {
            var checkbox = (CheckBox) sender;
            //Filter Added
            if (!checkbox.Checked) return;
            var tagName = checkbox.Text.Split('(').First().Trim();
            /*var sTag = PlainTags.Find(item => item.Name.Equals(tagName));
                var wholelist = tileOLV.Objects;
                List<ListedVN> filteredlist = (from ListedVN item in wholelist
                                               let tags = StringToTags(item.Tags)
                                               where sTag != null && tags.Any(tag => tag.ID == sTag.ID)
                                               select item).ToList();
                AddFilterTag(tagName);
                tileOLV.SetObjects(filteredlist);*/
            //

            AddFilterTag(tagName);
        }

        private void SaveCustomFilter(object sender, EventArgs e)
        {
            if (filterNameBox.Text.Length == 0)
            {
                WriteText(filterReply, "Enter name of filter.");
                return;
            }
            //
            customFilters.Items.Add(filterNameBox.Text);
            _customFilters.Add(new ComplexFilter(filterNameBox.Text, _activeFilter));
            filterNameBox.Text = "";
            //serialize custom filters
            SaveCustomFiltersXML();
            WriteText(filterReply, Resources.filter_saved);
            customFilters.SelectedIndex = customFilters.Items.Count - 1;
        }

        private void ClearFilter(object sender, EventArgs e)
        {
            DisplayFilterTags(true);
            customFilters.SelectedIndex = 0;
            WriteText(filterReply, Resources.filter_cleared);
        }

        private void AddFilterTag(string tagName)
        {
            var writtenTag =
                PlainTags.Find(item => item.Name.Equals(tagName, StringComparison.InvariantCultureIgnoreCase));
            if (writtenTag == null)
            {
                WriteError(filterReply, $"Tag {tagName} not found");
                return;
            }
            foreach (var filter in _activeFilter)
            {
                if (!filter.HasChild(writtenTag.ID)) continue;
                foreach (var item in filter.Children) Debug.Write($"item={item}+");
                Debug.Print("");
                _activeFilter.Remove(filter);
                Debug.Print($"{writtenTag.Name} Tag Displaced {filter.Name} Tag");
                break;
            }
            filterReply.Text = $"Tag {writtenTag.Name} found, VNs={writtenTag.VNs}";
            //save tag as tag and children
            _filterIDList.Clear();
            var parents = new List<int>();
            Debug.Print($"Getting Child Tags for {tagName}");
            var debugCount = 0;
            var difference = 1;
            IEnumerable<WrittenTag> list2 = PlainTags.Where(x => x.Parents.Contains(writtenTag.ID));
            while (difference > 0)
            {
                var initial = _filterIDList.Count;
                _filterIDList = _filterIDList.Union(list2.Select(x => x.ID)).ToList();
                difference = _filterIDList.Count - initial;
                list2 = PlainTags.Where(tag => _filterIDList.Intersect(tag.Parents).Any());
                Debug.Print($"Found {difference} new child tags, round {debugCount}");
                debugCount++;
            }
            parents.AddRange(_filterIDList);
            var newFilter = new TagFilter(writtenTag.ID, writtenTag.Name, -1, parents);
            var notNeeded = false;
            var count = _vnList.Count(vn => VNMatchesSingleTag(vn, newFilter));
            newFilter.Titles = count;
            WriteText(filterReply, $"Tag {tagName} has {count} VNs in local database.");
            foreach (var filter in _activeFilter)
            {
                if (!newFilter.HasChild(filter.ID)) continue;
                Debug.Print($"{writtenTag.Name} not necessary, {filter.Name} is already included");
                notNeeded = true;
                break;
            }
            if (notNeeded) return;
            _activeFilter.Add(newFilter);
            DisplayFilterTags();
            _currentList = VNMatchesFilter;
            RefreshList();
        }

        private void DisplayFilterTags(bool clear = false)
        {
            //clear old labels
            var oldCount = 0;
            var oldLabel = (CheckBox) Controls.Find(FilterLabel + 0, true).FirstOrDefault();
            while (oldLabel != null)
            {
                oldLabel.Dispose();
                oldCount++;
                oldLabel = (CheckBox) Controls.Find(FilterLabel + oldCount, true).FirstOrDefault();
            }
            if (clear)
            {
                _activeFilter = new List<TagFilter>();
                return;
            }
            //add labels
            var count = 0;
            foreach (var filter in _activeFilter)
            {
                var filterLabel = new CheckBox
                {
                    AutoSize = true,
                    Location = new Point(264, 44 + count*22),
                    Name = FilterLabel + count,
                    Size = new Size(35, 13),
                    Text = $"{filter.Name} (Total: {filter.Titles})",
                    MaximumSize = new Size(200, 17),
                    Checked = true
                };

                filterLabel.CheckedChanged += TagFilterChanged;
                count++;
                tagFilteringBox.Controls.Add(filterLabel);
            }
        }

        private void TagFilterChanged(object sender, EventArgs e)
        {
            if (tileOLV.Items.Count == 0) return;
            var checkbox = (CheckBox) sender;
            //Filter Added
            if (checkbox.Checked)
            {
                var tagName = checkbox.Text.Split('(').First().Trim();
                AddFilterTag(tagName);
            }
            //Filter Removed
            else
            {
                var filterNo = Convert.ToInt32(checkbox.Name.Remove(0, 11));
                Debug.Print($"number of filter= {filterNo}");
                Debug.Print($"tags in active filter= {_activeFilter.Count} before");
                _activeFilter.RemoveAt(filterNo);
                Debug.Print($"tags in active filter= {_activeFilter.Count} after");
                DisplayFilterTags();
                if (_activeFilter.Any()) tileOLV.SetObjects(_vnList.Where(VNMatchesFilter));
                else Filter_All(null, null);
            }
        }

        private async void UpdateResults(object sender, EventArgs e)
        {
            if (Conn.Status != VndbConnection.APIStatus.Ready)
            {
                WriteWarning(filterReply, "Connection busy with previous request...", true);
                return;
            }
            var message = Resources.update_custom_filter;
            var askBox = MessageBox.Show(message, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            await UpdateFilterResults(filterReply);
        }

        private async Task UpdateFilterResults(Label replyLabel)
        {
            ReloadLists();
            _added = 0;
            _skipped = 0;
            IEnumerable<string> betterTags = _activeFilter.Select(x => x.ID).Select(s => $"tags = {s}");
            var tags = string.Join(" and ", betterTags);
            string tagQuery = $"get vn basic ({tags}) {{\"results\":25}}";
            var result = await TryQuery(tagQuery, "UCF Query Error", replyLabel, true, true);
            if (!result) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            if (vnRoot.Num == 0) return;
            List<VNItem> vnItems = vnRoot.Items;
            await GetMultipleVN(vnItems.Select(x => x.ID).ToList(), replyLabel, true);
            var pageNo = 1;
            var moreResults = vnRoot.More;
            while (moreResults)
            {
                pageNo++;
                string moreTagQuery = $"get vn basic ({tags}) {{\"results\":25, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(moreTagQuery, "UCFM Query Error", replyLabel, true, true);
                if (!moreResult) return;
                var moreVNRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                if (vnRoot.Num == 0) break;
                List<VNItem> moreVNItems = moreVNRoot.Items;
                await GetMultipleVN(moreVNItems.Select(x => x.ID).ToList(), replyLabel, true);
                moreResults = moreVNRoot.More;
            }
            ReloadLists();
            tileOLV.SetObjects(_vnList.Where(VNMatchesFilter));
            WriteText(replyLabel, $"Update complete, added {_added} and skipped {_skipped} titles.");
        }

        #endregion

        #region Log Tab

        private async void LogQuestion(object sender, KeyPressEventArgs e) //send a command direct to server
        {
            if (e.KeyChar != (char) Keys.Enter) return;
            e.Handled = true;
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

        private void CloseAllForms(object sender, EventArgs e)
        {
            for (var i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i].Name == "FormMain") continue;
                Debug.Print($"Closing {Application.OpenForms[i].Name}, {i}");
                Application.OpenForms[i].Close();
            }
        }

        private void ReloadLists()
        {
            DBConn.Open();
            _vnList = DBConn.GetAllTitles(UserID);
            _producerList = DBConn.GetAllProducers();
            UserList = DBConn.GetUserRelatedTitles(UserID);
            DBConn.Close();
            _vnIDList = _vnList.Select(x => x.VNID);
            _producerIDList = _producerList.Select(x => x.ID);
            LoadFavoriteProducerList();
        }

        private void SetOLV()
        {
            ReloadLists();
            //VN List
            tileOLV.SetObjects(_vnList.Where(_currentList));
        }

        private void FormMain_Enter(object sender, EventArgs e)
        {
            if (_fullyLoaded) return;
            _fullyLoaded = true;
            Activate();
        }

        internal static void WriteText(Label label, string message)
        {
            var linkLabel = label as LinkLabel;
            if (linkLabel != null) linkLabel.LinkColor = NormalLinkColor;
            else label.ForeColor = NormalColor;
            label.Text = message;
        }

        internal static void WriteWarning(Label label, string message, bool fade = false)
        {
            var linkLabel = label as LinkLabel;
            if (linkLabel != null) linkLabel.LinkColor = WarningColor;
            else label.ForeColor = WarningColor;
            label.Text = message;
            if (fade) FadeLabel(label);
        }

        internal static void WriteError(Label label, string message, bool fade = false)
        {
            var linkLabel = label as LinkLabel;
            if (linkLabel != null) linkLabel.LinkColor = ErrorColor;
            else label.ForeColor = ErrorColor;
            label.Text = message;
            if (fade) FadeLabel(label);
        }

        private void OLVChangeView(object sender, EventArgs e)
        {
            var cb = (ComboBox) sender;
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

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime -
                    new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        private void ShowContextMenu(object sender, CellRightClickEventArgs e)
        {
            if (e == null) return;
            e.MenuStrip = VNContextMenu(e.Model);
        }

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
            var vn = (ListedVN) model;
            userlistToolStripMenuItem.Checked = !vn.ULStatus.Equals("");
            wishlistToolStripMenuItem.Checked = !vn.WLStatus.Equals("");
            voteToolStripMenuItem.Checked = vn.Vote > 0;
            switch (vn.ULStatus)
            {
                case "":
                    ((ToolStripMenuItem) userlistToolStripMenuItem.DropDownItems[0]).Checked = true;
                    break;
                case "Unknown":
                    ((ToolStripMenuItem) userlistToolStripMenuItem.DropDownItems[1]).Checked = true;
                    break;
                case "Playing":
                    ((ToolStripMenuItem) userlistToolStripMenuItem.DropDownItems[2]).Checked = true;
                    break;
                case "Finished":
                    ((ToolStripMenuItem) userlistToolStripMenuItem.DropDownItems[3]).Checked = true;
                    break;
                case "Stalled":
                    ((ToolStripMenuItem) userlistToolStripMenuItem.DropDownItems[4]).Checked = true;
                    break;
                case "Dropped":
                    ((ToolStripMenuItem) userlistToolStripMenuItem.DropDownItems[5]).Checked = true;
                    break;
            }
            switch (vn.WLStatus)
            {
                case "":
                    ((ToolStripMenuItem) wishlistToolStripMenuItem.DropDownItems[0]).Checked = true;
                    break;
                case "High":
                    ((ToolStripMenuItem) wishlistToolStripMenuItem.DropDownItems[1]).Checked = true;
                    break;
                case "Medium":
                    ((ToolStripMenuItem) wishlistToolStripMenuItem.DropDownItems[2]).Checked = true;
                    break;
                case "Low":
                    ((ToolStripMenuItem) wishlistToolStripMenuItem.DropDownItems[3]).Checked = true;
                    break;
                case "Blacklist":
                    ((ToolStripMenuItem) wishlistToolStripMenuItem.DropDownItems[4]).Checked = true;
                    break;
            }
            if (vn.Vote > 0)
            {
                var vote = (int) Math.Floor(vn.Vote);
                ((ToolStripMenuItem) voteToolStripMenuItem.DropDownItems[vote]).Checked = true;
            }
            else
                ((ToolStripMenuItem) voteToolStripMenuItem.DropDownItems[0]).Checked = true;

            return ContextMenuVN;
        }

        private static bool VNMatchesFilter(ListedVN vn)
        {
            int[] vnTags = StringToTags(vn.Tags).Select(x => x.ID).ToArray();
            var filtersMatched = 0;
            foreach (var filter in _activeFilter)
            {
                if (vnTags.Any(vntag => filter.AllIDs.Contains(vntag))) filtersMatched++;
            }
            return filtersMatched == _activeFilter.Count;
        }

        private static bool VNMatchesSingleTag(ListedVN vn, TagFilter tag)
        {
            int[] vnTags = StringToTags(vn.Tags).Select(x => x.ID).ToArray();
            return vnTags.Any(vntag => tag.AllIDs.Contains(vntag));
        }

        private void SaveImage(VNItem vn)
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
            vnImages.Images.Add(vn.ID.ToString(), Image.FromFile(imageLoc));
        }

        internal static void FadeLabel(Label tLabel) //start timer to make label invisible
        {
            var fadeTimer = new Timer {Interval = LabelFadeTime};
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
            var listedVN = (ListedVN) e.Model;
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
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(-1);
            if (listedVN.ULAdded == dateTimeOffset) e.Item.GetSubItem(4).Text = "";
            if (listedVN.WLAdded == dateTimeOffset) e.Item.GetSubItem(7).Text = "";
            if (listedVN.VoteAdded == dateTimeOffset) e.Item.GetSubItem(9).Text = "";
            if (listedVN.Vote < 1) e.Item.GetSubItem(8).Text = "";
        }

        private void DisplayCommonTagsULStats(object sender, EventArgs e)
        {
            if (!UserList.Any())
            {
                var labelCount = 1;
                while (labelCount <= 10)
                {
                    var name = TagTypeUrt + labelCount;
                    var mctULLabel = (Label) Controls.Find(name, true).First();
                    mctULLabel.Visible = false;
                    labelCount++;
                }
                return;
            }
            //userlist stats - most common tags
            if (sender != null)
            {
                var checkBox = (CheckBox) sender;
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
                    var mctULLabel = (Label) Controls.Find(name, true).First();
                    mctULLabel.Visible = false;
                    labelCount++;
                }
                return;
            }
            var ulTagList = new Dictionary<int, int>();
            if (UserList.Count == 0) return;
            foreach (var vn in UserList)
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
                var mctULLabel = (Label) Controls.Find(name, true).First();
                var tagName = PlainTags.Find(item => item.ID == ulProdlistlist[p - 1].Key).Name;
                mctULLabel.Text = $"{tagName} ({ulProdlistlist[p - 1].Value})";
                mctULLabel.Visible = true;
                p++;
            }
            while (max <= 10)
            {
                var name = TagTypeUrt + max;
                var mctULLabel = (Label) Controls.Find(name, true).First();
                mctULLabel.Visible = false;
                max++;
            }
        }

        private void DisplayCommonTags(object sender, EventArgs e)
        {
            if (sender != null)
            {
                var checkBox = (CheckBox) sender;
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
            var bw = new BackgroundWorker {WorkerReportsProgress = true, WorkerSupportsCancellation = true};
            bw.DoWork += delegate
            {
                //vn list - most common tags
                var taglist = new Dictionary<int, int>();
                var vnNo = 1;
                foreach (var vn in vnlist)
                {
                    var progressPercent = (double) vnNo/vnCount*100;
                    vnNo++;
                    try
                    {
                        bw.ReportProgress((int) Math.Floor(progressPercent));
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
                delegate(object o, ProgressChangedEventArgs args)
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
                    var cb = (CheckBox) Controls.Find(name, true).First();
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

        private static bool CheckUnreleased(string date)
        {
            if (!date.Contains('-')) return true;
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
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Debug.Print(
                            $"Date: {dateArray[0]}-{dateArray[1]}-{dateArray[2] - tryCount} is invalid, trying again one day earlier");
                        tryCount++;
                    }
                    finally
                    {
                        tryDone = true;
                    }
                }
                return dtDate > DateTime.UtcNow;
            }
            //if date only has year-month, then if month hasn't finished = unreleased
            var monthRegex = new Regex(@"^\d{4}-\d{2}$");
            if (monthRegex.IsMatch(date))
            {
                dtDate = new DateTime(dateArray[0], dateArray[1], 28);
                return dtDate > DateTime.UtcNow;
            }
            //if date only has year, then if year hasn't finished = unreleased
            var yearRegex = new Regex(@"^\d{4}$");
            if (yearRegex.IsMatch(date))
            {
                dtDate = new DateTime(dateArray[0], 12, 28);
                return dtDate > DateTime.UtcNow;
            }
            return false;
        }

        internal static string TagsToString(List<TagItem> tags)
        {
            //var tagstring = string.Join(",", tags.Select(v => string.Join(",", v.ToArray())).Select(vstring => '[' + vstring + ']').ToArray());
            return '[' + string.Join(",", tags.Select(v => v.ToString())) + ']';
        }

        internal static List<TagItem> StringToTags(string tagstring)
        {
            if (tagstring.Equals("")) return new List<TagItem>();
            var curS = $"{{\"tags\":{tagstring}}}";
            var vnitem = JsonConvert.DeserializeObject<VNItem>(curS);
            return vnitem.Tags;
        }

        private void ObjectList_SelectedIndexChanged(object sender, CellClickEventArgs e) //display info on selected VN
        {
            var listView = (ObjectListView) sender;
            if (listView.SelectedIndices.Count <= 0) return;
            var vnItem = (ListedVN) listView.SelectedObjects[0];
            DBConn.Open();
            vnItem = DBConn.GetSingleVN(vnItem.VNID, UserID);
            DBConn.Close();
            var vnf = new VisualNovelForm(vnItem, this);
            vnf.Show(this);
        }

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

        public static void Decompress(string fileToDecompress, string outputFile)
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

        public static int DaysSince(DateTime updatedDate)
        {
            if (updatedDate == DateTime.MinValue) return -1;
            var days = (DateTime.Today - updatedDate).Days;
            return days;
        }

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
            Decompress(TagsJsonGz, TagsJson);
            File.Delete(TagsJsonGz);
            Settings.Default.TagdumpUpdate = DateTime.UtcNow;
            Settings.Default.Save();
            LoadTagdump();
        }

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


        private void tileOLV_Resize(object sender, EventArgs e)
        {
            var width = tileOLV.Width - 24;
            var s = (int) Math.Round((double) width/230);
            if (s == 0) return;
            tileOLV.TileSize = new Size(width/s, 300);
        }

        private static void SaveCustomFiltersXML()
        {
            XmlHelper.ToXmlFile(_customFilters, CustomfiltersXml);
        }

        private void RefreshList()
        {
            ReloadLists();
            tileOLV.SetObjects(_vnList.Where(_currentList));
        }

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
            var s = (Control) sender;
            s.Text = "";
        }

        private void filterNameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SaveCustomFilter(sender, e);
        }

        private void searchButton_keyPress(object sender, KeyPressEventArgs e) //press enter on search button
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                e.Handled = true;
                VNSearchButt(sender, e);
            }
        }

        private void yearBox_KeyDown(object sender, KeyEventArgs e) //press enter on get year titles box
        {
            if (e.KeyCode == Keys.Enter) GetYearTitles(sender, e);
        }

        #endregion

        #region Quick Filters

        private void Filter_All(object sender, EventArgs e)
        {
            tileOLV.ModelFilter = null;
            _currentList = x => true;
            RefreshList();
        }

        private void Filter_FavoriteProducers(object sender, EventArgs e)
        {
            if (olFavoriteProducers.Items.Count == 0)
            {
                WriteError(replyText, "No Favorite Producers.", true);
                return;
            }
            IEnumerable<string> prodList = from ListedProducer producer in olFavoriteProducers.Objects
                select producer.Name;
            _currentList = vn => prodList.Contains(vn.Producer);
            RefreshList();
        }

        private void Filter_Wishlist(object sender, EventArgs e)
        {
            _currentList = x => !x.WLStatus.Equals("");
            RefreshList();
        }

        private void Filter_ULStatus(object sender, EventArgs e)
        {
            var dropdownlist = (ComboBox) sender;
            switch (dropdownlist.SelectedIndex)
            {
                case 0:
                    Filter_All(null, null);
                    return;
                case 1:
                    dropdownlist.SelectedIndex = 0;
                    Filter_All(null, null);
                    return;
                case 2:
                    _currentList = x => x.ULStatus.Equals("Unknown");
                    break;
                case 3:
                    _currentList = x => x.ULStatus.Equals("Playing");
                    break;
                case 4:
                    _currentList = x => x.ULStatus.Equals("Finished");
                    break;
                case 5:
                    _currentList = x => x.ULStatus.Equals("Dropped");
                    break;
            }
            RefreshList();
        }

        private void Filter_Custom(object sender, EventArgs e)
        {
            var dropdownlist = (ComboBox) sender;
            switch (dropdownlist.SelectedIndex)
            {
                case 0:
                    deleteCustomFilterButton.Enabled = false;
                    updateCustomFilterButton.Enabled = false;
                    Filter_All(null, null);
                    return;
                case 1:
                    deleteCustomFilterButton.Enabled = false;
                    updateCustomFilterButton.Enabled = false;
                    dropdownlist.SelectedIndex = 0;
                    Filter_All(null, null);
                    return;
                default:
                    deleteCustomFilterButton.Enabled = true;
                    updateCustomFilterButton.Enabled = true;
                    _activeFilter = _customFilters[dropdownlist.SelectedIndex - 2]?.Filters;
                    DisplayFilterTags();
                    _currentList = VNMatchesFilter;
                    break;
            }
            RefreshList();
        }

        private void Filter_Producer(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (!ProducerFilterBox.Text.Any()) return;
            _currentList = x => x.Producer.Equals(ProducerFilterBox.Text);
            RefreshList();
        }

        private async void UpdateCustomFilter(object sender, EventArgs e)
        {
            if (Conn.Status != VndbConnection.APIStatus.Ready)
            {
                WriteWarning(replyText, "Connection busy with previous request...", true);
                return;
            }
            var selectedFilter = _customFilters[customFilters.SelectedIndex - 2];
            var message = selectedFilter.Updated != DateTime.MinValue
                ? $"This filter was last updated {DaysSince(selectedFilter.Updated)} days ago.\n{Resources.update_custom_filter}"
                : Resources.update_custom_filter;
            var askBox = MessageBox.Show(message, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            await UpdateFilterResults(replyText);
            _customFilters[customFilters.SelectedIndex - 2].Updated = DateTime.UtcNow;
            SaveCustomFiltersXML();
        }

        private void DeleteCustomFilter(object sender, EventArgs e)
        {
            var selectedFilter = customFilters.SelectedIndex;
            customFilters.Items.RemoveAt(selectedFilter);
            _customFilters.RemoveAt(selectedFilter - 2);
            SaveCustomFiltersXML();
            replyText.Text = Resources.filter_deleted;
            FadeLabel(replyText);
            customFilters.SelectedIndex = 0;
        }

        private void BlacklistToggle(object sender, EventArgs e)
        {
            Func<ListedVN, bool> function = x => true;
            switch (BlacklistToggleBox.CheckState)
            {
                case CheckState.Unchecked:
                    BlacklistToggleBox.Text = @"Hide Blacklisted";
                    function = x => !x.WLStatus.Equals("Blacklist");
                    break;
                case CheckState.Indeterminate:
                    BlacklistToggleBox.Text = @"Show Blacklisted";
                    break;
                case CheckState.Checked:
                    BlacklistToggleBox.Text = @"Only Blacklisted";
                    function = x => x.WLStatus.Equals("Blacklist");
                    break;
            }
            ApplyToggleFilters(ToggleFilter.Blacklisted, function);
        }

        private void UnreleasedToggle(object sender, EventArgs e)
        {
            Func<ListedVN, bool> function = x => true;
            switch (UnreleasedToggleBox.CheckState)
            {
                case CheckState.Unchecked:
                    UnreleasedToggleBox.Text = @"Hide Unreleased";
                    function = x => !CheckUnreleased(x.RelDate);
                    break;
                case CheckState.Indeterminate:
                    UnreleasedToggleBox.Text = @"Show Unreleased";
                    break;
                case CheckState.Checked:
                    UnreleasedToggleBox.Text = @"Only Unreleased";
                    function = x => CheckUnreleased(x.RelDate);
                    break;
            }
            ApplyToggleFilters(ToggleFilter.Unreleased, function);
        }

        private void URTToggle(object sender, EventArgs e)
        {
            Func<ListedVN, bool> function = x => true;
            switch (URTToggleBox.CheckState)
            {
                case CheckState.Unchecked:
                    URTToggleBox.Text = @"Hide URT";
                    function = x => UserList.Find(y => y.VNID == x.VNID) == null;
                    break;
                case CheckState.Indeterminate:
                    URTToggleBox.Text = @"Show URT";
                    break;
                case CheckState.Checked:
                    URTToggleBox.Text = @"Only URT";
                    function = x => UserList.Find(y => y.VNID == x.VNID) != null;
                    break;
            }
            ApplyToggleFilters(ToggleFilter.URT, function);
        }

        private async void UpdateProducerTitles(object sender, EventArgs e)
        {
            var producer = ProducerFilterBox.Text;
            if (producer.Equals("")) return;
            var producerItem = _producerList.Find(x => x.Name.Equals(producer));
            if (producerItem == null)
            {
                WriteError(replyText, "NYI (Producer not in local db)", true);
                return;
            }
            _added = 0;
            _skipped = 0;
            await GetProducerTitles(producerItem, replyText, true);
            WriteText(replyText, $"Got all VNs for {producerItem.Name} ({_added + _skipped} titles)");
            RefreshList();
        }

        private void ApplyToggleFilters(ToggleFilter toggleFilter, Func<ListedVN, bool> function)
        {
            _filters[(int) toggleFilter] = function;
            /*
            //clear filter list
            _filters.Clear();
            //add each enabled filter to list
            if (unreleasedFilter.Checked) _filters.Add(x => CheckUnreleased(x.RelDate));
            if (releasedFilter.Checked) _filters.Add(x => !CheckUnreleased(x.RelDate));
            if (urtFilter.Checked) _filters.Add(x => UserList.Find(y => y.VNID == x.VNID) != null);
            if (noURTFilter.Checked) _filters.Add(x => UserList.Find(y => y.VNID == x.VNID) == null);
            if (_blacklistToggle != null) _filters.Add(_blacklistToggle);*/
            tileOLV.ModelFilter = _filters.Any()
                ? new ModelFilter(vn => _filters.Select(filter => filter((ListedVN) vn)).All(valid => valid))
                : null;
            objectList_ItemsChanged(null, null);
        }

        #endregion

        #region Classes/Enums

        [Serializable, XmlRoot("ComplexFilter")]
        public class ComplexFilter
        {
            public ComplexFilter(string name, List<TagFilter> filters)
            {
                Name = name;
                Filters = filters;
            }

            public ComplexFilter()
            {
            }

            public string Name { get; set; }
            public List<TagFilter> Filters { get; set; }
            public DateTime Updated { get; set; }
        }

        [Serializable, XmlRoot("TagFilter")]
        public class TagFilter
        {
            public TagFilter(int id, string name, int titles, List<int> children)
            {
                ID = id;
                Name = name;
                Titles = titles;
                Children = children;
                AllIDs = children;
                AllIDs.Add(id);
            }

            public TagFilter()
            {
            }

            public int ID { get; set; }
            public string Name { get; set; }
            public int Titles { get; set; }
            public List<int> Children { get; set; }
            public List<int> AllIDs { get; set; }


            public bool HasChild(int tag)
            {
                return Children.Contains(tag);
            }
        }

        public class VNTileRenderer : AbstractRenderer
        {
            internal Brush BackBrush = Brushes.LightPink;

            internal Pen BorderPen = new Pen(Color.FromArgb(0x33, 0x33, 0x33));
            internal Brush HeaderBackBrush = new SolidBrush(Color.FromArgb(0x33, 0x33, 0x33));
            internal Brush HeaderTextBrush = Brushes.AliceBlue;
            internal Brush TextBrush = new SolidBrush(Color.FromArgb(0x22, 0x22, 0x22));

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
                DrawVNTile(g, itemBounds, rowObject, olv, (OLVListItem) e.Item);

                // Finally render the buffered graphics
                buffered.Render();
                buffered.Dispose();

                // Return true to say that we've handled the drawing
                return true;
            }

            public void DrawVNTile(Graphics g, Rectangle itemBounds, object rowObject, ObjectListView olv,
                OLVListItem item)
            {
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
                var size = g.MeasureString("Wj", font, itemBounds.Width, fmt);
                // Allow a border around the card
                itemBounds.Inflate(-2, -2);

                // Draw card background
                const int rounding = 20;
                var path = GetRoundedRect(itemBounds, rounding);
                g.FillPath(BackBrush, path);
                g.DrawPath(BorderPen, path);

                g.Clip = new Region(itemBounds);

                // Draw the photo
                var photoRect = itemBounds;
                photoRect.Inflate(-spacing, -spacing);
                var vn = rowObject as ListedVN;
                if (vn != null)
                {
                    var id = vn.VNID.ToString();
                    var imageUrl = vn.ImageURL;
                    var ext = Path.GetExtension(imageUrl);
                    photoRect.Height = (int) (itemBounds.Height - 3*size.Height - spacing*2);
                    var rectratio = (double) photoRect.Width/photoRect.Height;
                    var photoFile = string.Format($"vnImages\\{id}{ext}");
                    if (vn.ImageNSFW && !Settings.Default.ShowNSFWImages) g.DrawImage(Resources.nsfw_image, photoRect);
                    else if (File.Exists(photoFile))
                    {
                        var photo = Image.FromFile(photoFile);
                        var photoratio = (double) photo.Width/photo.Height;
                        //zoom in image to occupy whole area
                        //Alternately show whole image but do not occupy whole area
                        if (photoratio > rectratio) //if image is wider
                        {
                            var shrinkratio = (double) photo.Width/photoRect.Width;
                            var newWidth = photoRect.Width;
                            var newHeight = (int) Math.Floor(photo.Height/shrinkratio);
                            var newX = photoRect.X;
                            var hny = (double) newHeight/2;
                            var hph = (double) photoRect.Height/2;
                            var newY = photoRect.Y + (int) Math.Floor(hph) - (int) Math.Floor(hny);
                            var newPhotoRect = new Rectangle(newX, newY, newWidth, newHeight);
                            g.DrawImage(photo, newPhotoRect);
                        }
                        else //if image is taller
                        {
                            var shrinkratio = (double) photo.Height/photoRect.Height;
                            var newWidth = (int) Math.Floor(photo.Width/shrinkratio);
                            var newHeight = photoRect.Height;
                            var hnx = (double) newWidth/2;
                            var hpw = (double) photoRect.Width/2;
                            var newX = photoRect.X + (int) Math.Floor(hpw) - (int) Math.Floor(hnx);
                            var newY = photoRect.Y;
                            var newPhotoRect = new Rectangle(newX, newY, newWidth, newHeight);
                            g.DrawImage(photo, newPhotoRect);
                        }
                    }
                    else g.DrawImage(Resources.no_image, photoRect);
                }

                // Now draw the text portion
                RectangleF textBoxRect = photoRect;
                textBoxRect.Y += photoRect.Height + spacing;
                textBoxRect.Width = itemBounds.Right - textBoxRect.X - spacing;


                // Draw the other bits of information
                textBoxRect.Height = size.Height;
                fmt.Alignment = StringAlignment.Near;
                //
                var title = olv.AllColumns.Find(x => x.AspectName.Equals("Title"));
                var producer = olv.AllColumns.Find(x => x.AspectName.Equals("Producer"));
                var ulStatus = olv.AllColumns.Find(x => x.AspectName.Equals("ULStatus"));
                var wlStatus = olv.AllColumns.Find(x => x.AspectName.Equals("WLStatus"));
                var vote = olv.AllColumns.Find(x => x.AspectName.Equals("Vote"));
                g.DrawString(title.GetStringValue(rowObject), boldFont, TextBrush, textBoxRect, fmt);
                textBoxRect.Y += size.Height;
                g.DrawString(producer.GetStringValue(rowObject), font, TextBrush, textBoxRect, fmt);
                textBoxRect.Y += size.Height;
                string[] parts = {"", "", ""};
                if (!ulStatus.GetStringValue(rowObject).Equals(""))
                {
                    parts[0] = "Userlist: ";
                    parts[1] = ulStatus.GetStringValue(rowObject);
                }
                else if (!wlStatus.GetStringValue(rowObject).Equals(""))
                {
                    parts[0] = "Wishlist: ";
                    parts[1] = wlStatus.GetStringValue(rowObject);
                }
                if (Convert.ToInt32(vote.GetStringValue(rowObject)) > 0)
                    parts[2] = $" (Vote: {vote.GetStringValue(rowObject)})";
                var complete = string.Join(" ", parts);
                g.DrawString(complete, font, TextBrush, textBoxRect, fmt);
            }

            private GraphicsPath GetRoundedRect(RectangleF rect, float diameter)
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

            public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
            {
                var ratioX = (double) maxWidth/image.Width;
                var ratioY = (double) maxHeight/image.Height;
                var ratio = Math.Min(ratioX, ratioY);

                var newWidth = (int) (image.Width*ratio);
                var newHeight = (int) (image.Height*ratio);

                var newImage = new Bitmap(newWidth, newHeight);

                using (var graphics = Graphics.FromImage(newImage))
                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);

                return newImage;
            }
        }

        private enum ToggleFilter
        {
            URT,
            Unreleased,
            Blacklisted
        }

        internal enum Command
        {
            New,
            Update,
            Delete
        }

        internal enum ChangeType
        {
            UL,
            WL,
            Vote
        }

        #endregion
    }
}