using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Happy_Search.Properties;
using Microsoft.Win32;
using Newtonsoft.Json;
using Timer = System.Windows.Forms.Timer;

namespace Happy_Search
{
    /// <summary>
    ///     Main Form of application, contains global variables.
    /// </summary>
    public partial class FormMain : Form
    {
        #region File Locations

        private const string TagsURL = "http://vndb.org/api/tags.json.gz";
        private const string TraitsURL = "http://vndb.org/api/traits.json.gz";
        private const string ProjectURL = "https://github.com/Zoltanar/Happy-Search";
        private const string DefaultTraitsJson = "Program Data\\Default Files\\traits.json";
        private const string DefaultTagsJson = "Program Data\\Default Files\\tags.json";

#if DEBUG
        internal const string VNImagesFolder = "..\\Release\\Stored Data\\Saved Cover Images\\";
        internal const string VNScreensFolder = "..\\Release\\Stored Data\\Saved Screenshots\\";
        private const string DBStatsXml = "..\\Release\\Stored Data\\dbs.xml";
        private const string MainXmlFile = "..\\Release\\Stored Data\\saved_objects.xml";
        private const string LogFile = "..\\Release\\Stored Data\\message.log";
        private const string TagsJsonGz = "..\\Release\\Stored Data\\tags.json.gz";
        private const string TraitsJsonGz = "..\\Release\\Stored Data\\traits.json.gz";
        private const string TagsJson = "..\\Release\\Stored Data\\tags.json";
        private const string TraitsJson = "..\\Release\\Stored Data\\traits.json";
#else
        internal const string VNImagesFolder = "Stored Data\\Saved Cover Images\\";
        internal const string VNScreensFolder = "Stored Data\\Saved Screenshots\\";
        private const string DBStatsXml = "Stored Data\\dbs.xml";
        private const string MainXmlFile = "Stored Data\\saved_objects.xml";
        private const string LogFile = "Stored Data\\message.log";
        private const string TagsJsonGz = "Stored Data\\tags.json.gz";
        private const string TraitsJsonGz = "Stored Data\\traits.json.gz";
        private const string TagsJson = "Stored Data\\tags.json";
        private const string TraitsJson = "Stored Data\\traits.json";
#endif
        #endregion

        //constants / definables
        internal const string ClientName = "Happy Search By Zolty";
        internal const string ClientVersion = "1.3.8";
        internal const string APIVersion = "2.25";
        private const int APIMaxResults = 25;
        internal static readonly string MaxResultsString = "\"results\":" + APIMaxResults;
        private const string TagTypeAll = "checkBox";
        private const string TagTypeUrt = "mctULLabel";
        internal const string ContentTag = "cont";
        internal const string SexualTag = "ero";
        internal const string TechnicalTag = "tech";
        private const int LabelFadeTime = 5000; //ms for text to disappear (not actual fade)
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
        private List<KeyValuePair<int, int>> _toptentags;
        private byte _mctCount;

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
                otherMethodsCB.SelectedIndex = 0;
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
                Directory.CreateDirectory("Stored Data");
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
                LogToFile($"{ClientName} (Version {ClientVersion}, for VNDB API {APIVersion})");
                LogToFile($"Project at {ProjectURL}");
                LogToFile($"Start Time = {DateTime.UtcNow}");
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
                    $"Dumpfiles Update = {Settings.Default.DumpfilesUpdate}, days since = {DaysSince(Settings.Default.DumpfilesUpdate)}");
                if (DaysSince(Settings.Default.DumpfilesUpdate) > 2 || DaysSince(Settings.Default.DumpfilesUpdate) == -1)
                {
                    GetNewDumpFiles();
                }
                else
                {
                    //load dump files if they exist, otherwise load default.
                    LoadTagdump(!File.Exists(TagsJson));
                    LoadTraitdump(!File.Exists(TraitsJson));
                }
                var tagSource = new AutoCompleteStringCollection();
                tagSource.AddRange(PlainTags.Select(v => v.Name).ToArray());
                tagSearchBox.AutoCompleteCustomSource = tagSource;
                string[] traitRootNames = PlainTraits.Where(x => x.TopmostParentName == null).Select(x => x.Name).ToArray();
                traitRootsDropdown.Items.Clear();
                foreach (var rootName in traitRootNames)
                {
                    if (rootName == null) continue;
                    traitRootsDropdown.Items.Add(rootName);
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
        private async void GetOldVNStatsClick()
        {
            //popularity, rating and votecount were added, check for votecount
            int[] titlesWithoutStats = _vnList.Where(x => Math.Abs(x.Popularity) < 0.001).Select(x => x.VNID).ToArray();
            var oldCount = titlesWithoutStats.Length;
            if (oldCount == 0)
            {
                WriteWarning(userListReply, "There are no titles missing stats.", true);
                return;
            }
            var messageBox =
                MessageBox.Show(
                    $@"You only need to do this once and only if you used Happy Search prior to version 1.2.
{oldCount} need to be updated, if this is over 6000 it may take a while, are you sure?",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (messageBox != DialogResult.Yes) return;
            await GetOldVNStats(titlesWithoutStats);
            ReloadLists();
            RefreshVNList();
            LoadFavoriteProducerList();
            WriteText(userListReply, $"Got stats for {_vnsAdded} titles.");
        }

        /// <summary>
        ///     Update tags of titles that haven't been updated in over 7 days.
        /// </summary>
        private async void UpdateTitleDataClick(object sender, EventArgs e)
        {
            int[] listOfTitlesToUpdate;
            if (Settings.Default.Limit10Years)
            {
                //limit to titles release in last 10 years but include all favorite producers' titles
                DBConn.Open();
                IEnumerable<string> favProList = DBConn.GetFavoriteProducersForUser(UserID).Select(x => x.Name);
                DBConn.Close();
                int[] limitedTitlesToUpdate = _vnList.Where(x => x.UpdatedDate > 7 && x.DateForSorting > DateTime.UtcNow.AddYears(-10)).Select(x => x.VNID).ToArray();
                int[] favProTitles = _vnList.Where(x => favProList.Contains(x.Producer)).Select(x => x.VNID).ToArray();
                listOfTitlesToUpdate = limitedTitlesToUpdate.Concat(favProTitles).Distinct().ToArray();
            }
            else listOfTitlesToUpdate = _vnList.Where(x => x.UpdatedDate > 7).Select(x => x.VNID).ToArray();
            var messageBox =
                MessageBox.Show(
                    $@"{listOfTitlesToUpdate.Length} need to be updated, if this is a large number (over 1000), it may take a while, are you sure?",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (messageBox != DialogResult.Yes) return;
            await UpdateTitleData(listOfTitlesToUpdate);
            ReloadLists();
            RefreshVNList();
            WriteText(userListReply, $"Updated data on {_vnsAdded} titles.");
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
            var helpFile = $"{Path.Combine(path, "Program Data\\Help\\getstarted.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

        private void OtherMethodChosen(object sender, EventArgs e)
        {
            switch (otherMethodsCB.SelectedIndex)
            {
                case 1:
                    break;
                case 2:
                    GetOldVNStatsClick();
                    break;
                case 3:
                    GetAllCharacterData();
                    break;
                case 4:
                    GetAllMissingImages();
                    break;
                default:
                    return;
            }

            otherMethodsCB.SelectedIndex = 0;
        }

        private async void GetAllMissingImages()
        {
            IEnumerable<ListedVN> vnsWithImages = _vnList.Where(x => !x.ImageURL.Equals(""));
            ListedVN[] vnsMissingImages = (from vn in vnsWithImages
                                           let photoFile = string.Format($"{VNImagesFolder}{vn.VNID}{Path.GetExtension(vn.ImageURL)}")
                                           where !File.Exists(photoFile)
                                           select vn).ToArray();
            const int averageImageSizeBytes = 37 * 1024;
            var missingCount = vnsMissingImages.Length;
            var estimatedSizeString = BytesToString(missingCount * averageImageSizeBytes);
            var doubleEstimatedSizeString = BytesToString(missingCount * averageImageSizeBytes * 2);
            if (missingCount == 0)
            {
                WriteWarning(userListReply, "There are no titles missing their cover image.", true);
                return;
            }
            var messageBox =
                   MessageBox.Show(
                       $@"There are {missingCount} titles in your local database missing their cover image.
Do you wish to download all missing cover images?
This can useful if you modify or replace your local database file without modifying your saved cover images folder.
The total download size is estimated to be {estimatedSizeString} ~ {doubleEstimatedSizeString}.",
                       Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (messageBox != DialogResult.Yes) return;
            foreach (var vn in vnsMissingImages)
            {
                await SaveImageAsync(vn);
            }
            WriteText(userListReply, "Finished getting missing cover images.");
        }

        private async void GetAllCharacterData()
        {
            ReloadLists();
            var messageBox2 =
                   MessageBox.Show(
                       @"Do you wish to get character data about all VNs?
You only need to this once and only if you used Happy Search prior to version 1.3
This will take a long time if you have a lot of titles in your local database.",
                       Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (messageBox2 != DialogResult.Yes) return;
            await GetCharactersForMultipleVN(_vnList.Select(x => x.VNID).ToArray(), userListReply);
            ReloadLists();
            RefreshVNList();
            WriteText(userListReply, "Finished getting characters for all titles.");
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
            if (Conn.Status != VndbConnection.APIStatus.Ready)
            {
                WriteError(userListReply, "API Connection isn't ready.");
                return;
            }
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
            SetFavoriteProducersData();
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
                DBConn.UpsertUserList(UserID, item);
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
                DBConn.UpsertWishList(UserID, item);
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
                DBConn.UpsertVoteList(UserID, item);
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
            await GetMultipleVN(unfetchedTitles, userListReply, true);
            WriteText(userListReply, Resources.updated_urt);
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
        /// Convert number of bytes to human-readable formatted string, rounded to 1 decimal place. (e.g 79.4KB)
        /// </summary>
        /// <param name="byteCount">Number of bytes</param>
        /// <returns>Formatted string</returns>
        public static string BytesToString(int byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB" }; //int.MaxValue is in gigabyte range.
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num) + suf[place];
        }

        /// <summary>
        /// Print message to Debug and write it to log file.
        /// </summary>
        /// <param name="message">Message to be written</param>
        public static void LogToFile(string message)
        {
            Debug.Print(message);
            using (var writer = new StreamWriter(LogFile, true))
            {
                writer.WriteLine(message);
            }
        }
        /// <summary>
        /// Print exception to Debug and write it to log file.
        /// </summary>
        /// <param name="exception">Exception to be written to file</param>
        public static void LogToFile(Exception exception)
        {
            Debug.Print(exception.Message);
            Debug.Print(exception.StackTrace);
            using (var writer = new StreamWriter(LogFile, true))
            {
                writer.WriteLine(exception.Message);
                writer.WriteLine(exception.StackTrace);
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
        /// <param name="disableFade"></param>
        public static void WriteText(Label label, string message, bool disableFade = false)
        {
            var linkLabel = label as LinkLabel;
            if (linkLabel != null) linkLabel.LinkColor = NormalLinkColor;
            else label.ForeColor = NormalColor;
            label.Text = message;
            if (disableFade) return;
            FadeLabel(label);
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
            string imageLocation = $"{VNImagesFolder}{vn.ID}{Path.GetExtension(vn.Image)}";
            if (File.Exists(imageLocation)) return;
            LogToFile($"Start downloading cover image for {vn}");
            try
            {
                var requestPic = WebRequest.Create(vn.Image);
                using (var responsePic = requestPic.GetResponse())
                {
                    using (var stream = responsePic.GetResponseStream())
                    {
                        if (stream == null) return;
                        var webImage = Image.FromStream(stream);
                        webImage.Save(imageLocation);
                    }
                }
            }
            catch (Exception ex) when (ex is NotSupportedException || ex is ArgumentNullException || ex is SecurityException || ex is UriFormatException)
            {
                LogToFile(ex);
            }
        }


        /// <summary>
        ///     Saves a title's cover image (unless it already exists)
        /// </summary>
        /// <param name="vn">Title</param>
        private static async Task SaveImageAsync(ListedVN vn)
        {
            if (!Directory.Exists(VNImagesFolder)) Directory.CreateDirectory(VNImagesFolder);
            if (vn.ImageURL == null || vn.ImageURL.Equals("")) return;
            string imageLocation = $"{VNImagesFolder}{vn.VNID}{Path.GetExtension(vn.ImageURL)}";
            if (File.Exists(imageLocation)) return;
            LogToFile($"Start downloading cover image for {vn}");
            try
            {
                var requestPic = WebRequest.Create(vn.ImageURL);
                using (var responsePic = await requestPic.GetResponseAsync())
                {
                    using (var stream = responsePic.GetResponseStream())
                    {
                        if (stream == null) return;
                        var webImage = Image.FromStream(stream);
                        webImage.Save(imageLocation);
                    }
                }
            }
            catch (Exception ex) when (ex is NotSupportedException || ex is ArgumentNullException || ex is SecurityException || ex is UriFormatException)
            {
                LogToFile(ex);
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
        public static DateTime StringToDate(string date)
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

        private void GetNewDumpFiles()
        {
            const int maxTries = 5;
            int tries = 0;
            bool complete = false;
            //tagdump section
            while (!complete && tries < maxTries)
            {
                if (!File.Exists(TagsJsonGz))
                {
                    SplashScreen.SplashScreen.SetStatus("Downloading new Tagdump file...");
                    tries++;
                    try
                    {
                        using (var client = new WebClient())
                        {
                            client.DownloadFile(TagsURL, TagsJsonGz);
                        }


                        GZipDecompress(TagsJsonGz, TagsJson);
                        File.Delete(TagsJsonGz);
                        complete = true;
                    }
                    catch (Exception e)
                    {
                        LogToFile(e);
                    }
                }
            }
            //load default file if new one couldnt be received or for some reason doesn't exist.
            if (!complete || !File.Exists(TagsJson)) LoadTagdump(true);
            else LoadTagdump();
            //traitdump section
            tries = 0;
            complete = false;
            while (!complete && tries < maxTries)
            {
                if (!File.Exists(TraitsJsonGz))
                {
                    SplashScreen.SplashScreen.SetStatus("Downloading new Traitdump file...");
                    tries++;
                    try
                    {
                        using (var client = new WebClient())
                        {
                            client.DownloadFile(TraitsURL, TraitsJsonGz);
                        }
                        GZipDecompress(TraitsJsonGz, TraitsJson);
                        File.Delete(TraitsJsonGz);
                        complete = true;
                    }
                    catch (Exception e)
                    {
                        LogToFile(e);
                    }
                }
            }
            //load default file if new one couldnt be received or for some reason doesn't exist.
            if (!complete || !File.Exists(TraitsJson)) LoadTraitdump(true);
            else
            {
                Settings.Default.DumpfilesUpdate = DateTime.UtcNow;
                Settings.Default.Save();
                LoadTraitdump();
            }
        }


        /// <summary>
        ///     Load Tags from Tag dump file.
        /// </summary>
        /// <param name="loadDefault">Load default file?</param>
        private void LoadTagdump(bool loadDefault = false)
        {
            var fileToLoad = loadDefault ? DefaultTagsJson : TagsJson;
            LogToFile($"Attempting to load {fileToLoad}");
            try
            {
                PlainTags = JsonConvert.DeserializeObject<List<WrittenTag>>(File.ReadAllText(fileToLoad));
                List<ItemWithParents> baseList = PlainTags.Cast<ItemWithParents>().ToList();
                foreach (var writtenTag in PlainTags)
                {
                    writtenTag.SetItemChildren(baseList);
                }
            }
            catch (JsonReaderException e)
            {
                if (fileToLoad.Equals(DefaultTagsJson))
                {
                    //Should never happen.
                    LogToFile($"Failed to read default tags.json file, please download a new one from {TagsURL} uncompress it and paste it in {DefaultTagsJson}.");
                    PlainTags = new List<WrittenTag>();
                    return;
                }
                LogToFile(e);
                LogToFile($"{TagsJson} could not be read, deleting it and loading default tagdump.");
                File.Delete(TagsJson);
                LoadTagdump(true);
            }
        }

        /// <summary>
        ///     Load Traits from Trait dump file.
        /// </summary>
        private void LoadTraitdump(bool loadDefault = false)
        {
            var fileToLoad = loadDefault ? DefaultTraitsJson : TraitsJson;
            LogToFile($"Attempting to load {fileToLoad}");
            try
            {
                PlainTraits = JsonConvert.DeserializeObject<List<WrittenTrait>>(File.ReadAllText(fileToLoad));
                List<ItemWithParents> baseList = PlainTraits.Cast<ItemWithParents>().ToList();
                foreach (var writtenTrait in PlainTraits)
                {
                    writtenTrait.SetTopmostParent(PlainTraits);
                    writtenTrait.SetItemChildren(baseList);
                }
            }
            catch (JsonReaderException e)
            {
                if (fileToLoad.Equals(DefaultTraitsJson))
                {
                    //Should never happen.
                    LogToFile($"Failed to read default traits.json file, please download a new one from {TraitsURL} uncompress it and paste it in {DefaultTraitsJson}.");
                    PlainTraits = new List<WrittenTrait>();
                    return;
                }
                LogToFile(e);
                LogToFile($"{TraitsJson} could not be read, deleting it and loading default traitdump.");
                File.Delete(TraitsJson);
                LoadTraitdump(true);
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

        private class IdentifiableBackgroundWorker : BackgroundWorker
        {
            public int ID { get; set; }

        }

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