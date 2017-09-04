using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Happy_Search.Other_Forms;
using Happy_Search.Properties;
using Microsoft.Win32;
using Newtonsoft.Json;
using Octokit;
using Happy_Apps_Core;
using static Happy_Apps_Core.StaticHelpers;
using Application = System.Windows.Forms.Application;
using Label = System.Windows.Forms.Label;

// ReSharper disable LocalizableElement

namespace Happy_Search
{
    /// <summary>
    ///     Main Form of application, contains global variables.
    /// </summary>
    public partial class FormMain : Form
    {
        internal readonly VndbConnection Conn = new VndbConnection();
        internal readonly VNDatabase LocalDatabase;
        private Func<ListedVN, bool> _currentList = x => true;
        private string _currentListLabel;



        private bool _wideView;
        internal static GuiSettings GuiSettings;
        internal string LoginString;

        /// <summary>
        /// Holds information on state of filters for VN list.
        /// </summary>
        internal FiltersTab FiltersTab;

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
            string[] args = Environment.GetCommandLineArgs();
            InitializeControls();
            LoadUserSettings();
            SplashScreen.SetStatus("Loading Tag and Trait dump files...");
            LogToFile($"Dumpfiles Update = {Settings.DumpfileDate}, days since = {DaysSince(Settings.DumpfileDate)}");
            DumpFiles.Load();
            SplashScreen.SetStatus("Connecting to SQLite Database...");
            LocalDatabase = new VNDatabase(dbLog:args.Contains("-dl") || args.Contains("-debug"));
            LoadDataFromDatabase();
            SplashScreen.SetStatus("Updating User Stats...");
            UpdateUserStats();
            SplashScreen.SetStatus("Adding VNs to Object Lists...");
            _currentListLabel = "All Titles";
            CurrentFilterLabel = "No Filters";
            SplashScreen.CloseForm();
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

            void InitializeControls()
            {
                SplashScreen.SetStatus("Initializing Controls...");
                DontTriggerEvent = true;
                viewPicker.SelectedIndex = 0;
                otherMethodsCB.SelectedIndex = 0;
                ListByCB.SelectedIndex = 0;
                multiActionBox.SelectedIndex = 0;
                DontTriggerEvent = false;
                replyText.Text = "";
                userListReply.Text = "";
                resultLabel.Text = "";
                LoginString = "";
                prodReply.Text = "";
                advancedCheckBox.Checked = args.Contains("-am") || args.Contains("-debug");
                tileOLV.ItemRenderer = new VNTileRenderer();
                Directory.CreateDirectory(StoredDataFolder);
                File.Create(LogFile).Close();
                aboutTextBox.Text = $@"{ClientName} (Version {ClientVersion}, for VNDB API {APIVersion})
VNDB API Client for filtering/organizing and finding visual novels.
Created by Zolty, visit the project at {ProjectURL}

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
            void LoadUserSettings()
            {
                SplashScreen.SetStatus("Loading User Settings...");
                GuiSettings = GuiSettings.Load();
                tagTypeC2.Checked = GuiSettings.ContentTags;
                tagTypeS2.Checked = GuiSettings.SexualTags;
                tagTypeT2.Checked = GuiSettings.TechnicalTags;
                nsfwToggle.Checked = GuiSettings.NSFWImages;
                autoUpdateURTBox.Checked = GuiSettings.AutoUpdate;
                yearLimitBox.Checked = Settings.DecadeLimit;
            }
            void LoadDataFromDatabase()
            {
                SplashScreen.SetStatus("Loading Data from Database...");
                LocalDatabase.Open();
                LocalDatabase.GetAllTitles(Settings.UserID);
                LocalDatabase.GetAllProducers();
                LocalDatabase.GetAllCharacters();
                LocalDatabase.GetUserRelatedTitles(Settings.UserID);
                LocalDatabase.Close();
                LoadFPListToGui();
                LogToFile("VN Items= " + LocalDatabase.VNList.Count);
                LogToFile("Producers= " + LocalDatabase.ProducerList.Count);
                LogToFile("Characters= " + LocalDatabase.CharacterList.Count);
                LogToFile("UserRelated Items= " + LocalDatabase.URTList.Count);
                PopulateProducerSearchBox();
                PopulateGroupSearchBox();
            }
        }


        private async void OnLoadRoutines(object sender, EventArgs e)
        {
            //client update
            var args = Environment.GetCommandLineArgs();
            if (!args.Contains("-debug") && !args.Contains("-sc"))
            { await ClientUpdateAsync(); }
            InitAPIConnection();
            //dbstats update
            LogToFile($"dbstats Update = {Settings.StatsDate}, days since = {DaysSince(Settings.StatsDate)}");
            if (DaysSince(Settings.StatsDate) > 2 || DaysSince(Settings.StatsDate) == -1) GetNewDBStats();
            else LoadDBStats();
            //urt update
            if (Settings.UserID > 0 && (GuiSettings.AutoUpdate || args.Contains("-flu")))
            { await URTUpdateAsync(); }
            FiltersTab = new FiltersTab(this);
            filtersTab.Controls.Add(FiltersTab);
            FiltersTab.Dock = DockStyle.Fill;
            LoadVNListToGui();
            tileOLV.Sort(tileColumnDate, SortOrder.Descending);
        }

        /// <summary>
        /// Check if URT update is due and if so, execute it
        /// </summary>
        private async Task URTUpdateAsync()
        {
            LogToFile("Checking if URT Update is due...");
            LogToFile(
                $"URTUpdate= {Settings.URTDate}, days since = {DaysSince(Settings.URTDate)}");
            var args = Environment.GetCommandLineArgs();
            if (DaysSince(Settings.URTDate) > 2 || DaysSince(Settings.URTDate) == -1 || args.Contains("-flu"))
            {
                LogToFile("Updating User Related Titles...");
                await UpdateURT("Auto-Update URT");
                Settings.URTDate = DateTime.UtcNow;
                Settings.Save();
            }
            else
            {
                LogToFile("Update not needed.");
            }
        }

        /// <summary>
        /// Checks if there is a new release out and if so, ask user if they want to visit releases page.
        /// </summary>
        private async Task ClientUpdateAsync()
        {
            var client = new GitHubClient(new ProductHeaderValue("Happy-Search"));
            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("Zoltanar", "Happy-Search");
            var latest = releases.First(r => !r.Prerelease);
            LogToFile($"The latest non-test release is tagged at {latest.TagName} and is named {latest.Name}");
            if (!latest.TagName.Equals(ClientVersion))
            {
                var messageBoxResult = MessageBox.Show($"Latest non-test release is {latest.TagName}, you are running {ClientVersion}\nDo you wish to visit the releases page?",
                    @"Update Client?", MessageBoxButtons.YesNo);
                if (messageBoxResult == DialogResult.Yes) Process.Start($"{ProjectURL}/releases");
            }
        }

        /// <summary>
        /// Initialize API Connection.
        /// </summary>
        private void InitAPIConnection()
        {
            Conn.Open();
            Conn.ActiveQuery = new ApiQuery(true, replyText);
            if (Conn.Status == VndbConnection.APIStatus.Error)
            {
                ChangeAPIStatus(Conn.Status);
                return;
            }
            //login with password if setting is enabled and stored password exists, otherwise login without password
            if (GuiSettings.RememberPassword)
            {
                LogToFile("Attempting log in with password.");
                char[] password = LoadPassword();
                if (password != null)
                {
                    APILoginWithPassword(password);
                    return;
                }
                APILogin();
                return;
            }
            LogToFile("Attempting log in without password.");
            APILogin();
        }

        /// <summary>
        ///     Load Login Form for user to log in.
        /// </summary>
        private async void LogInDialog(object sender, EventArgs e)
        {
            var dialogResult = new LoginForm(this).ShowDialog();
            ChangeAPIStatus(Conn.Status);
            if (dialogResult == DialogResult.OK) //change id/username only (no need to login with password)
            {
                //relogin without credentials to clear data
                switch (Conn.LogIn)
                {
                    case VndbConnection.LogInStatus.No:
                    case VndbConnection.LogInStatus.YesWithPassword:
                        Conn.Login(ClientName, ClientVersion);
                        break;
                }
                //get username from id if none was entered
                if (Settings.Username.Equals(""))
                {
                    Settings.Username = await GetUsernameFromID(Settings.UserID);
                }
                if (Settings.UserID < 1)
                {
                    Settings.UserID = await GetIDFromUsername(Settings.Username);
                }
                ChangeAPIStatus(Conn.Status);
                GuiSettings.Save();
                Settings.Save();
                SetLoginText();
                UpdateUserStats();
                await ReloadListsFromDbAsync();
                LoadVNListToGui();
                LoadFPListToGui();
                return;
            }
            if (dialogResult != DialogResult.Yes) return; //do nothing
            GuiSettings.Save();
            Settings.Save();
            SetLoginText();
            UpdateUserStats();
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            LoadFPListToGui();
        }

        /// <summary>
        /// Set text for Login Status.
        /// </summary>
        private void SetLoginText()
        {
            if (Conn.Status != VndbConnection.APIStatus.Ready)
            {
                string user = Settings.Username.Equals("") ? Settings.UserID.ToString() : $"{Settings.Username}({Settings.UserID})";
                LoginString = $"Connection error, showing data for {user}.";
                ChangeAPIStatus(Conn.Status);
                return;
            }
            switch (Conn.LogIn)
            {
                case VndbConnection.LogInStatus.YesWithPassword:
                    LoginString = $"Logged in as {Settings.Username}({Settings.UserID}).";
                    ChangeAPIStatus(Conn.Status);
                    return;
                case VndbConnection.LogInStatus.Yes:
                    if (Settings.UserID < 1)
                    {
                        LoginString = "Connected.";
                        ChangeAPIStatus(Conn.Status);
                        return;
                    }
                    LoginString = !Settings.Username.Equals("")
                        ? $"Connected as {Settings.Username}({Settings.UserID})."
                        : $"Connected as {Settings.UserID}.";
                    ChangeAPIStatus(Conn.Status);
                    return;
                case VndbConnection.LogInStatus.No:
                    LoginString = "Not logged in.";
                    ChangeAPIStatus(Conn.Status);
                    return;
            }
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            Conn.Close();
        }

        #endregion

        #region Settings Area / Get Started

        /// <summary>
        ///     Update tags/traits/stats of titles that haven't been updated in over 7 days.
        /// </summary>
        private async void UpdateTagsTraitsStatsClick(object sender, EventArgs e)
        {
            //limit to titles release in last 10 years but include all favorite producers' titles
            LocalDatabase.Open();
            LocalDatabase.GetFavoriteProducersForUser(Settings.UserID);
            LocalDatabase.Close();
            var tieredVns = new TieredVNs(LocalDatabase.VNList, LocalDatabase.FavoriteProducerList.Select(x=>x.Name), false);
            var messageBox =
                MessageBox.Show(tieredVns.MessageString, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (messageBox != DialogResult.Yes) return;
            var result = Conn.StartQuery(userListReply, "Update Tags/Traits/Stats", true, true, false);
            if (!result) return;
            await UpdateTagsTraitsStats(tieredVns.AllVns);
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            WriteText(userListReply, $"Updated tags, traits and stats on {Conn.TitlesAdded} titles.");
            ChangeAPIStatus(Conn.Status);
        }

        private async void UpdateAllDataClick(object sender, EventArgs e)
        {
            //limit to titles release in last 10 years but include all favorite producers' titles
            LocalDatabase.Open();
            LocalDatabase.GetFavoriteProducersForUser(Settings.UserID);
            LocalDatabase.Close();
            var tieredVns = new TieredVNs(LocalDatabase.VNList, LocalDatabase.FavoriteProducerList.Select(x=>x.Name), true);
            var messageBox =
                MessageBox.Show(tieredVns.MessageString, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (messageBox != DialogResult.Yes) return;
            var result = Conn.StartQuery(userListReply, "Update All Data", true, true, true);
            if (!result) return;
            await GetMultipleVN(tieredVns.AllVns, true);
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            WriteText(userListReply, $"Updated data on {Conn.TitlesAdded} titles.");
            ChangeAPIStatus(Conn.Status);
        }

        private struct TieredVNs
        {
            /*
            /// <summary>
            /// Updated over 7 days ago, released under 6 months ago
            /// </summary>
            public int Tier1Count { get; }

            /// <summary>
            /// Updated over 14 days ago, released 6 months to 1 year ago
            /// </summary>
            public int Tier2Count { get; }

            /// <summary>
            /// Updated over 28 days ago, released 1 to 2 years ago
            /// </summary>
            public int Tier3Count { get; }

            /// <summary>
            /// Updated over 56 days ago, released 2 to 10 years ago
            /// </summary>
            public int Tier4Count { get; }

            /// <summary>
            /// Updated over 7 days ago, released by favorite producers
            /// </summary>
            public int FPTitleCount { get; }
                        
            public int AllVnsCount => AllVns.Length;
            */

            public int[] AllVns { get; }

            public string MessageString { get; }

            /// <summary>
            /// Creates a tiered list of vns that need an update.
            /// </summary>
            /// <param name="allTitles"></param>
            /// <param name="favoriteProducers"></param>
            /// <param name="fullyUpdate">If true, gets list for vns that need full update, else, gets list of vns that need tags/stats/traits update</param>
            public TieredVNs(IReadOnlyCollection<ListedVN> allTitles, IEnumerable<string> favoriteProducers, bool fullyUpdate)
            {
                var tier1 = allTitles.Where(x => x.LastUpdatedOverDaysAgo(7, fullyUpdate) &&
                              x.DateForSorting >= DateTime.UtcNow.AddMonths(-6)).Select(t => t.VNID).ToArray();
                var tier1Count = tier1.Length;
                var tier2 = allTitles.Where(x => x.LastUpdatedOverDaysAgo(14, fullyUpdate) &&
                             x.ReleasedBetween(DateTime.UtcNow.AddYears(-1), DateTime.UtcNow.AddMonths(-6))).Select(t => t.VNID).ToArray();
                var tier2Count = tier2.Length;
                var tier3 = allTitles.Where(x => x.LastUpdatedOverDaysAgo(28, fullyUpdate) &&
                             x.ReleasedBetween(DateTime.UtcNow.AddYears(-2), DateTime.UtcNow.AddYears(-1))).Select(t => t.VNID).ToArray();
                var tier3Count = tier3.Length;
                var tier4 = allTitles.Where(x => x.LastUpdatedOverDaysAgo(56, fullyUpdate) &&
                             x.ReleasedBetween(DateTime.UtcNow.AddYears(-10), DateTime.UtcNow.AddYears(-2))).Select(t => t.VNID).ToArray();
                var tier4Count = tier4.Length;
                var tier5 = allTitles.Where(x => x.LastUpdatedOverDaysAgo(56, fullyUpdate) &&
                             x.ReleasedBetween(DateTime.MinValue, DateTime.UtcNow.AddYears(-10))).Select(t => t.VNID).ToArray();
                var tier5Count = tier5.Length;
                var fpTitles = allTitles.Where(x => x.LastUpdatedOverDaysAgo(7, fullyUpdate) &&
                             favoriteProducers.Contains(x.Producer)).Select(t => t.VNID).ToArray();
                var fpTitleCount = fpTitles.Length;
                AllVns = tier1.Concat(tier2).Concat(tier3).Concat(tier4).Concat(tier5).Concat(fpTitles).Distinct().ToArray();
                MessageString =
                    $@"{AllVns.Length} need to be updated, if this is a large number (over 1000), it may take a while, are you sure?
{tier1Count} Titles released in last 6 months.
{tier2Count} Titles released 6 months - 1 year ago.
{tier3Count} Titles released 1 year - 2 years ago.
{tier4Count} Titles released 2-10 years ago.
{tier5Count} Titles released 10+ years ago but never updated.
{fpTitleCount} Titles by Favorite Producers.";
            }
        }



        private void ToggleNSFWImages(object sender, EventArgs e)
        {
            GuiSettings.NSFWImages = nsfwToggle.Checked;
            GuiSettings.Save();
            tileOLV.SetObjects(tileOLV.Objects);
        }

        private void ToggleAutoUpdateURT(object sender, EventArgs e)
        {
            GuiSettings.AutoUpdate = autoUpdateURTBox.Checked;
            GuiSettings.Save();
        }

        private void ToggleLimit10Years(object sender, EventArgs e)
        {
            Settings.DecadeLimit = yearLimitBox.Checked;
            Settings.Save();
        }

        /// <summary>
        ///     Close all VN tabs.
        /// </summary>
        private void CloseVNTabs(object sender, EventArgs e)
        {
            var tabpages = TabsControl.TabPages;
            while (tabpages.Count > 2) tabpages.RemoveAt(2);
        }

        private void CloseTabMiddleClick(object sender, MouseEventArgs e)
        {
            var tabControl = sender as TabControl;
            Debug.Assert(tabControl != null, "tabControl != null");
            var tabs = tabControl.TabPages;

            if (e.Button != MouseButtons.Middle) return;
            var tab = tabs.Cast<TabPage>()
                .Where((t, i) => tabControl.GetTabRect(i).Contains(e.Location))
                .First();
            if (tab.TabIndex > 3) tabs.Remove(tab);
        }

        /// <summary>
        ///     Display html file explaining how to get started.
        /// </summary>
        private void Help_GetStarted(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            Debug.Assert(path != null, "Path.GetDirectoryName(Application.ExecutablePath) != null");
            var helpFile = $"{Path.Combine(path, "Program Data\\Help\\getstarted.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

        private async void OtherMethodChosen(object sender, EventArgs e)
        {
            switch (otherMethodsCB.SelectedIndex)
            {
                case 1:
                    break;
                case 2:
                    await GetAllMissingImages();
                    break;
                case 3:
                    await UpdateTagsTraitsStatsSkipLimit();
                    break;
                case 4:
                    await UpdateAllDataSkipLimit();
                    break;
                case 5:
                    await GetProducerLanguages();
                    break;
                default:
                    return;
            }
            otherMethodsCB.SelectedIndex = 0;
        }

        private async Task GetProducerLanguages()
        {
            var messageBox = MessageBox.Show(
                "This will get language info on all producers, only required once when upgrading from versions before 1.4.7" + Environment.NewLine +
                "It could take a while, Are you sure?",
                "Are you sure?", MessageBoxButtons.YesNo);
            if (messageBox != DialogResult.Yes) return;
            var result = Conn.StartQuery(userListReply, "Get Producer Languages", true, true, true);
            if (!result) return;
            await GetLanguagesForProducers(LocalDatabase.ProducerList.Select(t => t.ID).ToArray());
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            WriteText(userListReply, "Got producer languages.");
            ChangeAPIStatus(Conn.Status);
        }

        private async Task UpdateAllDataSkipLimit()
        {
            var messageBox = MessageBox.Show(
                "This will update data for all titles, regardless of their release date and date of last update.\n" +
                "It will take a long time if you have over 1000 titles.\n" +
                "Are you sure?",
                "Are you sure?", MessageBoxButtons.YesNo);
            if (messageBox != DialogResult.Yes) return;
            var result = Conn.StartQuery(userListReply, "Update All Data (All)", true, true, true);
            if (!result) return;
            await GetMultipleVN(LocalDatabase.VNList.Select(t => t.VNID).ToArray(), true);
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            WriteText(userListReply, "Updated data on all titles.");
            ChangeAPIStatus(Conn.Status);
        }




        /// <summary>
        /// Update title data of all titles regardless of release date/last update date.
        /// </summary>
        private async Task UpdateTagsTraitsStatsSkipLimit()
        {
            var messageBox = MessageBox.Show(
                "This will update tags, traits and stats for all titles, regardless of their release date and date of last update.\n" +
                "It will take a long time if you have over 1000 titles.\n" +
                "Are you sure?",
                "Are you sure?", MessageBoxButtons.YesNo);
            if (messageBox != DialogResult.Yes) return;
            var result = Conn.StartQuery(userListReply, "Update Tags/Traits/Stats (All)", true, true, true);
            if (!result) return;
            await UpdateTagsTraitsStats(LocalDatabase.VNList.Select(t => t.VNID));
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            WriteText(userListReply, "Updated tags/traits/stats on all titles.");
            ChangeAPIStatus(Conn.Status);
        }

        private async Task GetAllMissingImages()
        {
            IEnumerable<ListedVN> vnsWithImages = LocalDatabase.VNList.Where(x => !x.ImageURL.Equals(""));
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
                WriteWarning(userListReply, "There are no titles missing their cover image.");
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

        #endregion

        #region Get User-Related Titles

        private int _vnidToDebug = 20367;

        //Get user's user/wish/votelists from VNDB
        /// <summary>
        ///     Get user's userlist, wishlist and votelist from VNDB.
        /// </summary>
        private async void UpdateURTButtonClick(object sender, EventArgs e)
        {
            if (Settings.UserID < 1)
            {
                WriteText(userListReply, Resources.set_userid_first);
                return;
            }
            var askBox =
                MessageBox.Show(
                    $@"This may take a while depending on how many titles are in your lists,
especially if this is the first time.
Are you sure?

You currently have {LocalDatabase.URTList
                        .Count} items in the local database, they can
be displayed by clicking the User Related Titles (URT) filter.",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            await UpdateURT("Update URT");
        }

        /// <summary>
        /// Update logged in user's list of user-related titles.
        /// </summary>
        /// <param name="featureName">User-read name of function that called this function.</param>
        private async Task UpdateURT(string featureName)
        {
            if (Settings.UserID < 1) return;
            var result = Conn.StartQuery(userListReply, featureName, true, true, true);
            if (!result) return;
            LogToFile($"Starting GetUserRelatedTitles for {Settings.UserID}, previously had {LocalDatabase.URTList.Count} titles.");
            //clone list to make sure it doesnt keep command status.
            List<VNDatabase.UrtListItem> localURTList = LocalDatabase.URTList.Select(VNDatabase.UrtListItem.FromVN).ToList();
            await GetUserList(localURTList);
            await GetWishList(localURTList);
            await GetVoteList(localURTList);
            LocalDatabase.BeginTransaction();
            LocalDatabase.UpdateURTTitles(Settings.UserID, localURTList);
            LocalDatabase.EndTransaction();
            await GetRemainingTitles();
            LocalDatabase.Open();
            LocalDatabase.GetAllTitles(Settings.UserID);
            LocalDatabase.Close();
            SetFavoriteProducersData();
            await ReloadListsFromDbAsync();
            LoadFPListToGui();
            LoadVNListToGui();
            UpdateUserStats();
            WriteText(userListReply, $"Updated URT ({Conn.TitlesAdded} added).");
            ChangeAPIStatus(Conn.Status);
        }


        /// <summary>
        ///     Get user's userlist from VNDB, add titles that aren't in local db already.
        /// </summary>
        /// <param name="urtList">list of title IDs (avoids duplicate fetching)</param>
        /// <returns>list of title IDs (avoids duplicate fetching)</returns>
        private async Task GetUserList(List<VNDatabase.UrtListItem> urtList)
        {
            LogToFile("Starting GetUserList");
            string userListQuery = $"get vnlist basic (uid = {Settings.UserID} ) {{\"results\":100}}";
            //1 - fetch from VNDB using API
            var result = await TryQuery(userListQuery, Resources.gul_query_error);
            if (!result) return;
            var ulRoot = JsonConvert.DeserializeObject<UserListRoot>(Conn.LastResponse.JsonPayload);
            if (ulRoot.Num == 0) return;
            List<UserListItem> ulList = ulRoot.Items; //make list of vns in list
            var pageNo = 1;
            var moreResults = ulRoot.More;
            while (moreResults)
            {
                pageNo++;
                string userListQuery2 = $"get vnlist basic (uid = {Settings.UserID} ) {{\"results\":100, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(userListQuery2, Resources.gul_query_error);
                if (!moreResult) return;
                var ulMoreRoot = JsonConvert.DeserializeObject<UserListRoot>(Conn.LastResponse.JsonPayload);
                ulList.AddRange(ulMoreRoot.Items);
                moreResults = ulMoreRoot.More;
            }
            foreach (var item in ulList)
            {
                if (item.VN == _vnidToDebug) { }
                var itemInlist = urtList.FirstOrDefault(vn => vn.ID == item.VN);
                //add if it doesn't exist
                if (itemInlist == null) urtList.Add(new VNDatabase.UrtListItem(item));
                //update if it already exists
                else itemInlist.Update(item);
            }
        }

        private async Task GetWishList(List<VNDatabase.UrtListItem> urtList)
        {
            LogToFile("Starting GetWishList");
            string wishListQuery = $"get wishlist basic (uid = {Settings.UserID} ) {{\"results\":100}}";
            var result = await TryQuery(wishListQuery, Resources.gwl_query_error);
            if (!result) return;
            var wlRoot = JsonConvert.DeserializeObject<WishListRoot>(Conn.LastResponse.JsonPayload);
            if (wlRoot.Num == 0) return;
            List<WishListItem> wlList = wlRoot.Items; //make list of vn in list
            var pageNo = 1;
            var moreResults = wlRoot.More;
            while (moreResults)
            {
                pageNo++;
                string wishListQuery2 = $"get wishlist basic (uid = {Settings.UserID} ) {{\"results\":100, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(wishListQuery2, Resources.gwl_query_error);
                if (!moreResult) return;
                var wlMoreRoot = JsonConvert.DeserializeObject<WishListRoot>(Conn.LastResponse.JsonPayload);
                wlList.AddRange(wlMoreRoot.Items);
                moreResults = wlMoreRoot.More;
            }
            foreach (var item in wlList)
            {
                if (item.VN == _vnidToDebug) { }
                var itemInlist = urtList.FirstOrDefault(vn => vn.ID == item.VN);
                //add if it doesn't exist
                if (itemInlist == null) urtList.Add(new VNDatabase.UrtListItem(item));
                //update if it already exists
                else itemInlist.Update(item);
            }
        }

        private async Task GetVoteList(List<VNDatabase.UrtListItem> urtList)
        {
            LogToFile("Starting GetVoteList");
            string voteListQuery = $"get votelist basic (uid = {Settings.UserID} ) {{\"results\":100}}";
            var result = await TryQuery(voteListQuery, Resources.gvl_query_error);
            if (!result) return;
            var vlRoot = JsonConvert.DeserializeObject<VoteListRoot>(Conn.LastResponse.JsonPayload);
            if (vlRoot.Num == 0) return;
            List<VoteListItem> vlList = vlRoot.Items; //make list of vn in list
            var pageNo = 1;
            var moreResults = vlRoot.More;
            while (moreResults)
            {
                pageNo++;
                string voteListQuery2 = $"get votelist basic (uid = {Settings.UserID} ) {{\"results\":100, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(voteListQuery2, Resources.gvl_query_error);
                if (!moreResult) return;
                var vlMoreRoot = JsonConvert.DeserializeObject<VoteListRoot>(Conn.LastResponse.JsonPayload);
                vlList.AddRange(vlMoreRoot.Items);
                moreResults = vlMoreRoot.More;
            }
            foreach (var item in vlList)
            {
                if (item.VN == _vnidToDebug) { }
                var itemInlist = urtList.FirstOrDefault(vn => vn.ID == item.VN);
                //add if it doesn't exist
                if (itemInlist == null) urtList.Add(new VNDatabase.UrtListItem(item));
                //update if it already exists
                else itemInlist.Update(item);
            }
        }

        private async Task GetRemainingTitles()
        {
            List<int> unfetchedTitles = null;
            await Task.Run(() =>
            {
                LogToFile("Starting GetRemainingTitles");
                LocalDatabase.Open();
                unfetchedTitles = LocalDatabase.GetUnfetchedUserRelatedTitles(Settings.UserID);
                LocalDatabase.Close();
            });
            if (unfetchedTitles == null || !unfetchedTitles.Any()) return;
            await GetMultipleVN(unfetchedTitles.ToArray(), false);
            await ReloadListsFromDbAsync();
        }

        private void UpdateUserStats()
        {
            if (!LocalDatabase.URTList.Any())
            {
                ulstatsall.Text = @"None";
                ulstatsul.Text = @"-";
                ulstatswl.Text = @"-";
                ulstatsvl.Text = @"-";
                ulstatsavs.Text = @"-";
                DisplayCommonTagsURT();
                return;
            }
            var ulCount = 0;
            var wlCount = 0;
            var vlCount = 0;
            double cumulativeScore = 0;
            foreach (var item in LocalDatabase.URTList)
            {
                if (item.ULStatus > UserlistStatus.None) ulCount++;
                if (item.WLStatus > WishlistStatus.None) wlCount++;
                if (!(item.Vote > 0)) continue;
                vlCount++;
                cumulativeScore += item.Vote;
            }
            ulstatsall.Text = LocalDatabase.URTList.Count.ToString();
            ulstatsul.Text = ulCount.ToString();
            ulstatswl.Text = wlCount.ToString();
            ulstatsvl.Text = vlCount.ToString();
            ulstatsavs.Text = (cumulativeScore / vlCount).ToString("#.##");
            DisplayCommonTagsURT();
        }

        #endregion

        #region Other/General


        internal static bool AdvancedMode; //when true, print all api queries and responses to information tab.
        internal string CurrentFilterLabel;

        private void ToggleAdvancedMode(object sender, EventArgs e)
        {
            AdvancedMode = advancedCheckBox.Checked;
            questionBox.Enabled = AdvancedMode;
            serverQ.Enabled = AdvancedMode;
            serverR.Enabled = AdvancedMode;
            sendQueryButton.Enabled = AdvancedMode;
            clearLogButton.Enabled = AdvancedMode;
            if (AdvancedMode)
            {
                questionBox.Text = "";
                serverQ.Text = "";
                serverR.Text = "";
            }
            else
            {
                questionBox.Text = @"(Advanced Mode Disabled)";
                serverQ.Text = @"(Advanced Mode Disabled)";
                serverR.Text = @"(Advanced Mode Disabled)";
            }
        }

        internal bool VNIsByFavoriteProducer(ListedVN vn)
        {
            return LocalDatabase.FavoriteProducerList.Exists(fp => fp.Name.Equals(vn.Producer));
        }

        /// <summary>
        ///     Loads lists from local database.
        /// </summary>
        internal async Task ReloadListsFromDbAsync()
        {
            await Task.Run(() =>
            {
                LocalDatabase.Open();
                LocalDatabase.GetAllTitles(Settings.UserID);
                LocalDatabase.GetAllProducers();
                LocalDatabase.GetAllCharacters();
                LocalDatabase.GetUserRelatedTitles(Settings.UserID);
                LocalDatabase.Close();
                FiltersTab?.PopulateLanguages(false);
            });
        }

        /// <summary>
        /// Populates group search box with group data from titles.
        /// </summary>
        private void PopulateGroupSearchBox()
        {
            var groupFilterSource = new AutoCompleteStringCollection { "(Group)" };
            groupFilterSource.AddRange(LocalDatabase.VNList.SelectMany(vn => vn.GetCustomItemNotes().Groups).Distinct().ToArray());
            ListByCBQuery.AutoCompleteCustomSource = groupFilterSource;
            ListByCBQuery.DataSource = groupFilterSource;
        }

        /// <summary>
        /// Populates producer search box with data from producer list.
        /// </summary>
        private void PopulateProducerSearchBox()
        {
            if (ListByCB.SelectedIndex != (int)ListBy.Producer) return;
            ListByTB.AutoCompleteSource = AutoCompleteSource.CustomSource;
            ListByTB.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            var producerFilterSource = new AutoCompleteStringCollection();
            producerFilterSource.AddRange(LocalDatabase.ProducerList.Select(v => v.Name).ToArray());
            ListByTB.AutoCompleteCustomSource = producerFilterSource;
        }


        /// <summary>
        ///     Display ten most common tags in user related titles.
        /// </summary>
        internal void DisplayCommonTagsURTClick(object sender, EventArgs e)
        {
            if (sender == null || DontTriggerEvent) return;
            var checkBox = (CheckBox)sender;
            DontTriggerEvent = true;
            switch (checkBox.Name)
            {
                case "tagTypeC2":
                    GuiSettings.ContentTags = checkBox.Checked;
                    break;
                case "tagTypeS2":
                    GuiSettings.SexualTags = checkBox.Checked;
                    break;
                case "tagTypeT2":
                    GuiSettings.TechnicalTags = checkBox.Checked;
                    break;
            }
            DontTriggerEvent = false;
            GuiSettings.Save();
            DisplayCommonTagsURT();
        }

        internal void DisplayCommonTagsURT()
        {
            if (!LocalDatabase.URTList.Any())
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
            if (LocalDatabase.URTList.Count == 0) return;
            foreach (var vn in LocalDatabase.URTList)
            {
                if (!vn.TagList.Any()) continue;
                foreach (var tag in vn.TagList)
                {
                    var tagtag = DumpFiles.PlainTags.Find(item => item.ID == tag.ID);
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
                var tagName = DumpFiles.PlainTags.Find(item => item.ID == ulProdlistlist[p - 1].Key).Name;
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
        ///     Get new VNDB Stats from VNDB using API.
        /// </summary>
        private void GetNewDBStats()
        {
            if (Conn.Status != VndbConnection.APIStatus.Ready) return;
            Conn.Query("dbstats");
            HandleAdvancedMode("dbstats");
            if (Conn.LastResponse.Type != ResponseType.DBStats)
            {
                dbs1r.Text = Resources.dbs_unknown_error;
                return;
            }
            var dbInfo = JsonConvert.DeserializeObject<DbRoot>(Conn.LastResponse.JsonPayload);
            SaveObjectToJsonFile(dbInfo, DBStatsJson);
            Settings.StatsDate = DateTime.UtcNow;
            Settings.Save();
            LoadDBStats();
        }

        /// <summary>
        ///     Load VNDB Stats from XML file.
        /// </summary>
        private void LoadDBStats()
        {
            if (!File.Exists(DBStatsJson))
            {
                GetNewDBStats();
            }
            var dbXml = LoadObjectFromJsonFile<DbRoot>(DBStatsJson);
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

        private void OnFiltersLeave(object sender, EventArgs e)
        {
            FiltersTab?.LeftFiltersTab();
        }

        /// <summary>
        ///     Refresh VN OLV and repopulate group and producer search boxes.
        /// </summary>
        internal void LoadVNListToGui(bool skipComboSearch = false)
        {
            var watch = Stopwatch.StartNew();
            tileOLV.SetObjects(LocalDatabase.VNList.Where(_currentList));
            Debug.WriteLine($"Took {watch.ElapsedMilliseconds}ms to set VN objects.");
            if (!skipComboSearch)
            {
                PopulateGroupSearchBox();
            }
            PopulateProducerSearchBox();
        }

        /// <summary>
        ///     Save user's VNDB login password to Windows Registry (encrypted).
        /// </summary>
        /// <param name="password">User's password</param>
        internal static void SaveCredentials(char[] password)
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
            key.SetValue("Data1", ciphertext);
            key.SetValue("Data2", entropy);
            key.Close();
            LogToFile("Saved Login Password");
        }

        /// <summary>
        ///     Load user's VNDB login credentials from Windows Registry
        /// </summary>
        /// <returns></returns>
        private static char[] LoadPassword()
        {
            //get key data
            var key = Registry.CurrentUser.OpenSubKey($"SOFTWARE\\{ClientName}");
            if (key == null) return null;
            var password = key.GetValue("Data1") as byte[];
            var entropy = key.GetValue("Data2") as byte[];
            key.Close();
            if (password == null || entropy == null) return null;
            byte[] vv = ProtectedData.Unprotect(password, entropy, DataProtectionScope.CurrentUser);
            LogToFile("Loaded Login Password");
            return Encoding.UTF8.GetChars(vv);
        }

        private async void LogQuestion(object sender, EventArgs e) //send a command direct to server
        {
            if (questionBox.Text.Equals("")) return;
            var query = questionBox.Text;
            questionBox.Text = "";
            await Conn.QueryAsync(query);
            serverQ.Text += query + Environment.NewLine;
            serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
        }

        private void ClearLog(object sender, EventArgs e) //clear log
        {
            if (serverQ.InvokeRequired)
                serverQ.Invoke(new MethodInvoker(() => serverQ.Text = ""));
            else
                serverQ.Text = "";
            if (serverR.InvokeRequired)
                serverR.Invoke(new MethodInvoker(() => serverR.Text = ""));
            else
                serverR.Text = "";
        }


        private void VNToolTip(object sender, BrightIdeasSoftware.ToolTipShowingEventArgs e)
        {
            var vn = (ListedVN)e.Model;
            var notes = vn.GetCustomItemNotes();
            var characterCount = vn.GetCharacters(LocalDatabase.CharacterList).Length;
            var toolTipLines = new List<string>
            {
                $"{TruncateString(vn.Title, 50)}",
                $"{TruncateString($"by {vn.Producer}", 50)}"
            };
            if (vn.TagList.Any())
            {
                toolTipLines.Add($"Tags: {vn.TagList.GetTagCountByCat(TagCategory.Content)} Content," +
                                 $" {vn.TagList.GetTagCountByCat(TagCategory.Sexual)} Sexual," +
                                 $" {vn.TagList.GetTagCountByCat(TagCategory.Technical)} Technical");
            }
            else toolTipLines.Add("No Tags Found.");
            if (characterCount > 0) toolTipLines.Add($"Characters: {characterCount}.");
            if (notes != null && !notes.Notes.Equals("")) toolTipLines.Add($"{TruncateString($"Notes: {notes.Notes}", 50)}");
            if (notes != null && notes.Groups.Count != 0) toolTipLines.Add($"{TruncateString($"Groups: {string.Join(", ", notes.Groups)}", 50)}");
            e.Text = string.Join("\n", toolTipLines);
        }
        #endregion

        #region Press Enter On Text Boxes




        #endregion

        #region Classes/Enums


        #endregion


        private void RightClickSeeOnWebsite(object sender, EventArgs e)
        {
            if (!(tileOLV.SelectedObject is ListedVN vn)) return;
            Process.Start($"http://vndb.org/v{vn.VNID}/");
        }
    }


}