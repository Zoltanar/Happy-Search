using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Happy_Search.Other_Forms;
using Happy_Search.Properties;
//using Happy_Search.Properties;
using Microsoft.Win32;
using Newtonsoft.Json;
using Octokit;
using static Happy_Search.StaticHelpers;
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
        //constants / definables
#pragma warning disable 1591
        public const string ClientName = "Happy Search";
        public const string ClientVersion = "1.4.6";
        private const string APIVersion = "2.25";
        private const int APIMaxResults = 25;
        public static readonly string MaxResultsString = "\"results\":" + APIMaxResults;
        private const string TagTypeAll = "checkBox";
        private const string TagTypeUrt = "mctULLabel";
        public static readonly Color ErrorColor = Color.Red;
        public static readonly Color NormalColor = SystemColors.ControlLightLight;
        public static readonly Color NormalLinkColor = Color.FromArgb(0, 192, 192);
        public static readonly Color WarningColor = Color.DarkKhaki;
#pragma warning restore 1591

        internal readonly VndbConnection Conn = new VndbConnection();
        internal readonly DbHelper DBConn;
        private Func<ListedVN, bool> _currentList = x => true;
        private string _currentListLabel;
        internal bool DontTriggerEvent; //used to skip indexchanged events
        internal List<ListedProducer> ProducerList; //contains all producers in local database
        internal List<CharacterItem> CharacterList; //contains all producers in local database
        internal List<ListedProducer> FavoriteProducerList; //contains all favorite producers for logged in user
        private List<ListedVN> _vnList; //contains all vns in local database
        private ushort _vnsAdded;
        private ushort _vnsSkipped;
        internal List<WrittenTag> PlainTags; //Contains all tags as in tags.json
        internal List<WrittenTrait> PlainTraits; //Contains all tags as in tags.json
        internal List<ListedVN> URTList; //contains all user-related vns
        private List<KeyValuePair<int, int>> _toptentags;
        private byte _mctCount;
        private bool _wideView;
        internal static UserSettings Settings;

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
            SplashScreen.SetStatus("Initializing Controls...");
            {
                DontTriggerEvent = true;
                ulStatusDropDown.SelectedIndex = 0;
                wlStatusDropDown.SelectedIndex = 0;
                customTagFilters.SelectedIndex = 0;
                customTraitFilters.SelectedIndex = 0;
                viewPicker.SelectedIndex = 0;
                URTToggleBox.SelectedIndex = 0;
                UnreleasedToggleBox.SelectedIndex = 0;
                BlacklistToggleBox.SelectedIndex = 0;
                otherMethodsCB.SelectedIndex = 0;
                ListByCB.SelectedIndex = 0;
                multiActionBox.SelectedIndex = 0;
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
                advancedCheckBox.Checked = args.Contains("-am") || args.Contains("-debug");
                tileOLV.ItemRenderer = new VNTileRenderer();
#if DEBUG
                Directory.CreateDirectory("..\\Release\\Stored Data");
#else
                Directory.CreateDirectory("Stored Data");
#endif
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
            SplashScreen.SetStatus("Loading User Settings...");
            {
                Settings = UserSettings.Load();
                tagTypeC.Checked = Settings.ContentTags;
                tagTypeS.Checked = Settings.SexualTags;
                tagTypeT.Checked = Settings.TechnicalTags;
                tagTypeC2.Checked = Settings.ContentTags;
                tagTypeS2.Checked = Settings.SexualTags;
                tagTypeT2.Checked = Settings.TechnicalTags;
                nsfwToggle.Checked = Settings.NSFWImages;
                autoUpdateURTBox.Checked = Settings.AutoUpdate;
                yearLimitBox.Checked = Settings.DecadeLimit;
            }
            SplashScreen.SetStatus("Loading Tag and Trait files...");
            {
                LogToFile(
                    $"Dumpfiles Update = {Settings.DumpfileDate}, days since = {DaysSince(Settings.DumpfileDate)}");
                if (DaysSince(Settings.DumpfileDate) > 2 || DaysSince(Settings.DumpfileDate) == -1)
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
                string[] traitRootNames =
                    PlainTraits.Where(x => x.TopmostParentName == null).Select(x => x.Name).ToArray();
                traitRootsDropdown.Items.Clear();
                foreach (var rootName in traitRootNames)
                {
                    if (rootName == null) continue;
                    traitRootsDropdown.Items.Add(rootName);
                }
            }
            SplashScreen.SetStatus("Connecting to SQLite Database...");
            {
                DBConn = new DbHelper(args.Contains("-dl") || args.Contains("-debug"));
            }
            SplashScreen.SetStatus("Loading Data from Database...");
            {
                DBConn.Open();
                _vnList = DBConn.GetAllTitles(Settings.UserID);
                ProducerList = DBConn.GetAllProducers();
                CharacterList = DBConn.GetAllCharacters();
                URTList = DBConn.GetUserRelatedTitles(Settings.UserID);
                DBConn.Close();
                LoadFPListToGui();
                LogToFile("VN Items= " + _vnList.Count);
                LogToFile("Producers= " + ProducerList.Count);
                LogToFile("Characters= " + CharacterList.Count);
                LogToFile("UserRelated Items= " + URTList.Count);
                PopulateProducerSearchBox();
                PopulateGroupSearchBox();
            }
            SplashScreen.SetStatus("Updating User Stats...");
            {
                UpdateUserStats();
            }
            SplashScreen.SetStatus("Adding VNs to Object Lists...");
            {
                tileOLV.SetObjects(_vnList);
                tileOLV.Sort(tileColumnDate, SortOrder.Descending);
                _currentListLabel = "All Titles";
            }
            SplashScreen.SetStatus("Loading Custom Filters...");
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
            //this seems to stop DisconnectedContext errors.
            /*{
                LogToFile("Start waiting 1500 ms");
                Thread.Sleep(1500);
                LogToFile("Done waiting 1500 ms");
            }*/
            SplashScreen.CloseForm();
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }


        private async void OnLoadRoutines(object sender, EventArgs e)
        {
            //client update
            var args = Environment.GetCommandLineArgs();
            if (!args.Contains("-debug") && !args.Contains("-sc"))
            {
                await ClientUpdateAsync();
            }

            InitAPIConnection();


            //dbstats update
            LogToFile($"dbstats Update = {Settings.StatsDate}, days since = {DaysSince(Settings.StatsDate)}");
            if (DaysSince(Settings.StatsDate) > 2 || DaysSince(Settings.StatsDate) == -1) GetNewDBStats();
            else LoadDBStats();

            //urt update
            if (Settings.UserID > 0 && (Settings.AutoUpdate || args.Contains("-flu")))
            {
                await URTUpdateAsync();
            }

            traitRootsDropdown.SelectedIndex = 0;
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
            CurrentFeatureName = "Open";
            if (Conn.Status == VndbConnection.APIStatus.Error)
            {
                ChangeAPIStatus(Conn.Status);
                return;
            }
            //login with password if setting is enabled and stored password exists, otherwise login without password
            if (Settings.RememberPassword)
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
                Settings.Save();
                SetLoginText();
                UpdateUserStats();
                await ReloadListsFromDbAsync();
                LoadVNListToGui();
                LoadFPListToGui();
                return;
            }
            if (dialogResult != DialogResult.Yes) return; //do nothing
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
                loginReply.ForeColor = Color.Red;
                string user = Settings.Username.Equals("") ? Settings.UserID.ToString() : $"{Settings.Username}({Settings.UserID})";
                loginReply.Text = $"Connection error, showing data for {user}.";
                return;
            }
            switch (Conn.LogIn)
            {
                case VndbConnection.LogInStatus.YesWithPassword:
                    loginReply.ForeColor = Color.LightGreen;
                    loginReply.Text = $"Logged in as {Settings.Username}({Settings.UserID}).";
                    return;
                case VndbConnection.LogInStatus.Yes:
                    loginReply.ForeColor = Color.LightGreen;
                    if (Settings.UserID < 1)
                    {
                        loginReply.Text = "Connected.";
                        return;
                    }
                    loginReply.Text = !Settings.Username.Equals("")
                        ? $"Connected as {Settings.Username}({Settings.UserID})."
                        : $"Connected as {Settings.UserID}.";
                    return;
                case VndbConnection.LogInStatus.No:
                    loginReply.ForeColor = Color.Red;
                    loginReply.Text = "Not logged in.";
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
        ///     Update titles to include all fields in latest version of Happy Search.
        /// </summary>
        private async Task GetOldVNStatsClick()
        {
            //popularity, rating and votecount were added, check for votecount
            int[] titlesWithoutStats = _vnList.Where(x => Math.Abs(x.Popularity) < 0.001).Select(x => x.VNID).ToArray();
            var oldCount = titlesWithoutStats.Length;
            if (oldCount == 0)
            {
                WriteWarning(userListReply, "There are no titles missing stats.");
                return;
            }
            var messageBox =
                MessageBox.Show(
                    $@"You only need to do this once and only if you used Happy Search prior to version 1.2.
{oldCount} need to be updated, if this is over 6000 it may take a while, are you sure?",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (messageBox != DialogResult.Yes) return;
            var result = StartQuery(userListReply, "Get Old VN Stats");
            if (!result) return;
            await GetOldVNStats(titlesWithoutStats);
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            LoadFPListToGui();
            WriteText(userListReply, $"Got stats for {_vnsAdded} titles.");
            ChangeAPIStatus(Conn.Status);
        }

        /// <summary>
        ///     Update tags/traits/stats of titles that haven't been updated in over 7 days.
        /// </summary>
        private async void UpdateTitleDataClick(object sender, EventArgs e)
        {
            //limit to titles release in last 10 years but include all favorite producers' titles
            DBConn.Open();
            IEnumerable<string> favProList = DBConn.GetFavoriteProducersForUser(Settings.UserID).Select(x => x.Name);
            DBConn.Close();
            var tier1Titles =
                _vnList.Where(x => x.UpdatedDate > 7 && x.ReleasedBetweenNowAnd(DateTime.UtcNow.AddMonths(-6)))
                    .ToArray();
            var tier2Titles =
                _vnList.Where(
                    x =>
                        x.UpdatedDate > 14 &&
                        x.ReleasedBetween(DateTime.UtcNow.AddYears(-1), DateTime.UtcNow.AddMonths(-6))).ToArray();
            var tier3Titles =
                _vnList.Where(
                    x =>
                        x.UpdatedDate > 28 &&
                        x.ReleasedBetween(DateTime.UtcNow.AddYears(-2), DateTime.UtcNow.AddYears(-1))).ToArray();
            var tier4Titles =
                _vnList.Where(
                    x =>
                        x.UpdatedDate > 56 &&
                        x.ReleasedBetween(DateTime.UtcNow.AddYears(-10), DateTime.UtcNow.AddYears(-2))).ToArray();
            var titlesToUpdate =
                tier1Titles.Concat(tier2Titles).Concat(tier3Titles).Concat(tier4Titles).Select(x => x.VNID);
            //update title data - put vns in tiers by date of release eg[under 6 months old = 7 days] [6 months to a year = 14 days][1-2 year = 28 days][2+ years = 56 days]
            int[] favProTitles =
                _vnList.Where(x => x.UpdatedDate > 7 && favProList.Contains(x.Producer)).Select(x => x.VNID).ToArray();
            int[] listOfTitlesToUpdate = titlesToUpdate.Concat(favProTitles).Distinct().ToArray();
            var messageBox =
                MessageBox.Show(
                    $@"{listOfTitlesToUpdate.Length} need to be updated, if this is a large number (over 1000), it may take a while, are you sure?
{tier1Titles.Length} Titles released in last 6 months.
{tier2Titles.Length} Titles released 6 months - 1 year ago.
{tier3Titles.Length} Titles released 1 year - 2 years ago.
{tier4Titles.Length} Titles released 2+ years ago.
{favProTitles.Length} Titles by Favorite Producers.
",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (messageBox != DialogResult.Yes) return;
            var result = StartQuery(userListReply, "Update Title Data");
            if (!result) return;
            await UpdateTitleData(listOfTitlesToUpdate);
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            WriteText(userListReply, $"Updated data on {_vnsAdded} titles.");
            ChangeAPIStatus(Conn.Status);
        }


        private void ToggleNSFWImages(object sender, EventArgs e)
        {
            Settings.NSFWImages = nsfwToggle.Checked;
            Settings.Save();
            tileOLV.SetObjects(tileOLV.Objects);
        }

        private void ToggleAutoUpdateURT(object sender, EventArgs e)
        {
            Settings.AutoUpdate = autoUpdateURTBox.Checked;
            Settings.Save();
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
            var tabpages = tabControl1.TabPages;
            while(tabpages.Count > 2) tabpages.RemoveAt(2);
        }

        private void CloseTabMiddleClick(object sender, MouseEventArgs e)
        {
            var tabControl = sender as TabControl;
            Debug.Assert(tabControl != null, "tabControl != null");
            var tabs = tabControl.TabPages;

            if (e.Button == MouseButtons.Middle)
            {
                var tab = tabs.Cast<TabPage>()
                    .Where((t, i) => tabControl.GetTabRect(i).Contains(e.Location))
                    .First();
                if(tab.TabIndex > 2) tabs.Remove(tab);
            }
        }

        /// <summary>
        ///     Display html file explaining how to get started.
        /// </summary>
        private void Help_GetStarted(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
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
                    await GetOldVNStatsClick();
                    break;
                case 3:
                    await GetAllCharacterData();
                    break;
                case 4:
                    await GetAllMissingImages();
                    break;
                case 5:
                    await UpdateAllTitlesSkipLimit();
                    break;
                case 6:
                    await GetAllAliasesLanguages();
                    break;
                default:
                    return;
            }
            otherMethodsCB.SelectedIndex = 0;
        }

        /// <summary>
        /// Update title data of all titles regardless of release date/last update date.
        /// </summary>
        private async Task UpdateAllTitlesSkipLimit()
        {
            var messageBox = MessageBox.Show(
                "This will update title data for all titles, regardless of their release date and date of last update.\n" +
                "It will take a long time if you have over 1000 titles.\n" +
                "Are you sure?",
                "Are you sure?", MessageBoxButtons.YesNo);
            if (messageBox != DialogResult.Yes) return;
            var result = StartQuery(userListReply, "Update Title Data (All)");
            if (!result) return;
            await UpdateTitleData(_vnList.Select(t => t.VNID));
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            WriteText(userListReply, "Updated data on all titles.");
            ChangeAPIStatus(Conn.Status);
        }

        private async Task GetAllMissingImages()
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

        private async Task GetAllCharacterData()
        {
            var messageBox2 =
                MessageBox.Show(
                    @"Do you wish to get character data about all VNs?
You only need to this once and only if you used Happy Search prior to version 1.3
This will take a long time if you have a lot of titles in your local database.",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (messageBox2 != DialogResult.Yes) return;
            var result = StartQuery(userListReply, "Get All Char Data");
            if (!result) return;
            await GetCharactersForMultipleVN(_vnList.Select(x => x.VNID).ToArray(), userListReply);
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            WriteText(userListReply, "Finished getting characters for all titles.");
            ChangeAPIStatus(Conn.Status);
        }


        private async Task GetAllAliasesLanguages()
        {
            var messageBox2 =
                MessageBox.Show(
                    @"Do you wish to get language data about all VNs and Producers?
You only need to this once and only if you used Happy Search prior to version 1.4.6
This will take a long time if you have a lot of titles in your local database.",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (messageBox2 != DialogResult.Yes) return;
            var result = StartQuery(userListReply, "Get All Languages");
            if (!result) return;
            await GetLanguagesForMultipleVN(_vnList.Where(x=>x.Languages == null).Select(x => x.VNID).ToArray(), userListReply);
            await GetLanguagesForProducers(ProducerList.Select(x => x.ID).ToArray(), userListReply);
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            WriteText(userListReply, "Finished getting language data.");
            ChangeAPIStatus(Conn.Status);
        }

        #endregion

        #region Get User-Related Titles

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

You currently have {URTList
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
            var result = StartQuery(userListReply, featureName);
            if (!result) return;
            LogToFile($"Starting GetUserRelatedTitles for {Settings.UserID}, previously had {URTList.Count} titles.");
            List<int> userIDList = URTList.Select(x => x.VNID).ToList();
            userIDList = await GetUserList(userIDList);
            //
            if (userIDList.Contains(0))
            {
                LogToFile($"VN of ID 0 found in {featureName}, GetUserList for {Settings.UserID}");
            }
            //
            userIDList = await GetWishList(userIDList);
            if (userIDList.Contains(0))
            {
                LogToFile($"VN of ID 0 found in {featureName}, GetWishList for {Settings.UserID}");
            }
            await GetVoteList(userIDList);
            if (userIDList.Contains(0))
            {
                LogToFile($"VN of ID 0 found in {featureName}, GetVoteList for {Settings.UserID}");
            }
            await GetRemainingTitles();
            DBConn.Open();
            _vnList = DBConn.GetAllTitles(Settings.UserID);
            DBConn.Close();
            SetFavoriteProducersData();
            await ReloadListsFromDbAsync();
            LoadFPListToGui();
            LoadVNListToGui();
            UpdateUserStats();
            if (URTList.Count > 0) WriteText(userListReply, $"Updated URT ({_vnsAdded} added).");
            else WriteError(userListReply, Resources.no_results);
            ChangeAPIStatus(Conn.Status);
        }


        /// <summary>
        ///     Get user's userlist from VNDB, add titles that aren't in local db already.
        /// </summary>
        /// <param name="userIDList">list of title IDs (avoids duplicate fetching)</param>
        /// <returns>list of title IDs (avoids duplicate fetching)</returns>
        private async Task<List<int>> GetUserList(List<int> userIDList)
        {
            LogToFile("Starting GetUserList");
            string userListQuery = $"get vnlist basic (uid = {Settings.UserID} ) {{\"results\":100}}";
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
                string userListQuery2 = $"get vnlist basic (uid = {Settings.UserID} ) {{\"results\":100, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(userListQuery2, Resources.gul_query_error, userListReply);
                if (!moreResult) return userIDList;
                var ulMoreRoot = JsonConvert.DeserializeObject<UserListRoot>(Conn.LastResponse.JsonPayload);
                ulList.AddRange(ulMoreRoot.Items);
                moreResults = ulMoreRoot.More;
            }
            DBConn.BeginTransaction();
            foreach (var item in ulList)
            {
                DBConn.UpsertUserList(Settings.UserID, item);
                if (!userIDList.Contains(item.VN)) userIDList.Add(item.VN);
            }
            DBConn.EndTransaction();
            return userIDList;
        }

        private async Task<List<int>> GetWishList(List<int> userIDList)
        {
            LogToFile("Starting GetWishList");
            string wishListQuery = $"get wishlist basic (uid = {Settings.UserID} ) {{\"results\":100}}";
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
                string wishListQuery2 = $"get wishlist basic (uid = {Settings.UserID} ) {{\"results\":100, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(wishListQuery2, Resources.gwl_query_error, userListReply);
                if (!moreResult) return userIDList;
                var wlMoreRoot = JsonConvert.DeserializeObject<WishListRoot>(Conn.LastResponse.JsonPayload);
                wlList.AddRange(wlMoreRoot.Items);
                moreResults = wlMoreRoot.More;
            }
            await Task.Run(() =>
            {
                DBConn.BeginTransaction();
                foreach (var item in wlList)
                {
                    DBConn.UpsertWishList(Settings.UserID, item);
                    if (!userIDList.Contains(item.VN)) userIDList.Add(item.VN);
                }
                DBConn.EndTransaction();
            });
            return userIDList;
        }

        private async Task<List<int>> GetVoteList(List<int> userIDList)
        {
            LogToFile("Starting GetVoteList");
            string voteListQuery = $"get votelist basic (uid = {Settings.UserID} ) {{\"results\":100}}";
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
                string voteListQuery2 = $"get votelist basic (uid = {Settings.UserID} ) {{\"results\":100, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(voteListQuery2, Resources.gvl_query_error, userListReply);
                if (!moreResult) return userIDList;
                var vlMoreRoot = JsonConvert.DeserializeObject<VoteListRoot>(Conn.LastResponse.JsonPayload);
                vlList.AddRange(vlMoreRoot.Items);
                moreResults = vlMoreRoot.More;
            }
            await Task.Run(() =>
            {
                DBConn.BeginTransaction();
                foreach (var item in vlList)
                {
                    DBConn.UpsertVoteList(Settings.UserID, item);
                    if (!userIDList.Contains(item.VN)) userIDList.Add(item.VN);
                }
                DBConn.EndTransaction();
            });
            return userIDList;
        }

        private async Task GetRemainingTitles()
        {
            List<int> unfetchedTitles = null;
            await Task.Run(() =>
            {
                LogToFile("Starting GetRemainingTitles");
                DBConn.Open();
                unfetchedTitles = DBConn.GetUnfetchedUserRelatedTitles(Settings.UserID);
                DBConn.Close();
            });
            if (unfetchedTitles == null || !unfetchedTitles.Any()) return;
            await GetMultipleVN(unfetchedTitles, userListReply, true);
            await ReloadListsFromDbAsync();
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


        internal static bool AdvancedMode; //when true, print all api queries and responses to information tab.

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
            return FavoriteProducerList.Exists(fp => fp.Name.Equals(vn.Producer));
        }

        /// <summary>
        ///     Loads lists from local database.
        /// </summary>
        internal async Task ReloadListsFromDbAsync()
        {
            await Task.Run(() =>
            {
                DBConn.Open();
                _vnList = DBConn.GetAllTitles(Settings.UserID);
                ProducerList = DBConn.GetAllProducers();
                CharacterList = DBConn.GetAllCharacters();
                URTList = DBConn.GetUserRelatedTitles(Settings.UserID);
                DBConn.Close();
            });
        }

        /// <summary>
        /// Populates group search box with group data from titles.
        /// </summary>
        private void PopulateGroupSearchBox()
        {
            var groupFilterSource = new AutoCompleteStringCollection { "(Group)" };
            groupFilterSource.AddRange(_vnList.SelectMany(vn => vn.GetCustomItemNotes().Groups).Distinct().ToArray());
            ListByCBQuery.AutoCompleteCustomSource = groupFilterSource;
            ListByCBQuery.DataSource = groupFilterSource;
        }


        private void PopulateLangSearchBox()
        {
            var groupFilterSource = new AutoCompleteStringCollection { "(Language)" };
            var titlesWithLanguages = _vnList.Where(vn => vn.Languages != null);
            var languages = titlesWithLanguages.SelectMany(vn => vn.Languages.All);
            groupFilterSource.AddRange(languages.Distinct().ToArray());
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
            producerFilterSource.AddRange(ProducerList.Select(v => v.Name).ToArray());
            ListByTB.AutoCompleteCustomSource = producerFilterSource;
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
                        Settings.ContentTags = checkBox.Checked;
                        tagTypeC.Checked = checkBox.Checked;
                        break;
                    case "tagTypeS2":
                        Settings.SexualTags = checkBox.Checked;
                        tagTypeS.Checked = checkBox.Checked;
                        break;
                    case "tagTypeT2":
                        Settings.TechnicalTags = checkBox.Checked;
                        tagTypeT.Checked = checkBox.Checked;
                        break;
                }
                DontTriggerEvent = false;
                Settings.Save();
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
        ///     Get new VNDB Stats from VNDB using API.
        /// </summary>
        private void GetNewDBStats()
        {
            if (Conn.Status != VndbConnection.APIStatus.Ready) return;
            Conn.Query("dbstats");
            if (AdvancedMode)
            {
                serverQ.Text += @"dbstats" + Environment.NewLine;
                serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
            }
            if (Conn.LastResponse.Type != ResponseType.DBStats)
            {
                dbs1r.Text = Resources.dbs_unknown_error;
                return;
            }
            var dbInfo = JsonConvert.DeserializeObject<DbRoot>(Conn.LastResponse.JsonPayload);
            XmlHelper.ToXmlFile(dbInfo, DBStatsXml);
            Settings.StatsDate = DateTime.UtcNow;
            Settings.Save();
            LoadDBStats();
        }

        /// <summary>
        ///     Load VNDB Stats from XML file.
        /// </summary>
        private void LoadDBStats()
        {
            if (!File.Exists(DBStatsXml))
            {
                GetNewDBStats();
            }
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
                    SplashScreen.SetStatus("Downloading new Tagdump file...");
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
                    SplashScreen.SetStatus("Downloading new Traitdump file...");
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
                Settings.DumpfileDate = DateTime.UtcNow;
                Settings.Save();
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
        ///     Refresh VN OLV and repopulate group and producer search boxes.
        /// </summary>
        internal void LoadVNListToGui(bool skipComboSearch = false)
        {
            tileOLV.SetObjects(_vnList.Where(_currentList));
            if (!skipComboSearch)
            {
                PopulateGroupSearchBox();
                PopulateLangSearchBox();
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
            serverQ.Text = "";
            serverR.Text = "";
        }


        private void VNToolTip(object sender, BrightIdeasSoftware.ToolTipShowingEventArgs e)
        {
            var vn = (ListedVN)e.Model;
            var notes = vn.GetCustomItemNotes();
            var characterCount = vn.GetCharacters(CharacterList).Length;
            vn.SetTags(PlainTags);
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


        private void EnterCustomTagFilterName(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SaveCustomTagFilter(sender, e);
        }

        private void EnterCustomTraitFilterName(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SaveCustomTraitFilter(sender, e);
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