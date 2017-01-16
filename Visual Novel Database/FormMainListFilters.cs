using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using BrightIdeasSoftware;
using Happy_Search.Properties;
using Happy_Search.Other_Forms;
using Newtonsoft.Json;
using static Happy_Search.StaticHelpers;

namespace Happy_Search
{
    [SuppressMessage("ReSharper", "LocalizableElement")]
    public partial class FormMain
    {
        private static readonly ToggleArray Toggles = new ToggleArray();

        /// <summary>
        /// Display html file explaining searching, listing and filtering section.
        /// </summary>
        private void Help_SearchingAndFiltering(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            var helpFile = $"{Path.Combine(path, "Program Data\\Help\\searchingandfiltering.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

        #region Searching

        /// <summary>
        /// Run selected Search function.
        /// </summary>
        private void ListByUpdate(object sender, EventArgs e)
        {
            switch (ListByCB.SelectedIndex)
            {
                case (int)ListBy.Name:
                    Search_Name();
                    return;
                case (int)ListBy.Producer:
                    Search_Producer();
                    return;
                case (int)ListBy.Year:
                    Search_Year();
                    return;
            }
        }

        /// <summary>
        /// Searches for VNs from VNDB, adds them if they are not in local database.
        /// </summary>
        private async void Search_Name() //Fetch information from 'VNDB.org'
        {
            if (ListByTB.Text == "") //check if box is empty
            {
                WriteError(replyText, Resources.enter_vn_title, true);
                return;
            }
            if (ListByTB.Text.Length < 3)
            {
                WriteError(replyText, Resources.enter_vn_title + " (atleast 3 chars)", true);
                return;
            }
            var result = StartQuery(replyText, "Search_ByName");
            if (!result) return;
            var searchString = ListByTB.Text;
            ListByTB.Text = "";
            _vnsAdded = 0;
            _vnsSkipped = 0;
            string vnSearchQuery = $"get vn basic (search ~ \"{searchString}\") {{{MaxResultsString}}}";
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
                vnSearchQuery = $"get vn basic (search ~ \"{searchString}\") {{{MaxResultsString}, \"page\":{pageNo}}}";
                queryResult = await TryQuery(vnSearchQuery, Resources.vn_query_error, replyText, ignoreDateLimit: true);
                if (!queryResult) return;
                vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                vnItems.AddRange(vnRoot.Items);
                moreResults = vnRoot.More;
            }
            await GetMultipleVN(vnItems.Select(x => x.ID), replyText, true);
            WriteText(replyText, $"Found {_vnsAdded + _vnsSkipped} titles, {_vnsAdded}/{_vnsAdded + _vnsSkipped} added.");
            IEnumerable<int> idList = vnItems.Select(x => x.ID);
            _currentList = x => idList.Contains(x.VNID);
            _currentListLabel = $"{searchString} (Search)";
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            ChangeAPIStatus(Conn.Status);
        }

        /// <summary>
        /// Gets VNs released in the year entered by user, doesn't update VNs already in local database
        /// </summary>
        private async void Search_Year()
        {
            if (ListByTB.Text == "") //check if box is empty
            {
                WriteError(replyText, Resources.enter_year, true);
                return;
            }
            int year;
            var userIsNumber = int.TryParse(ListByTB.Text, out year);
            if (userIsNumber == false) //check if box has integer
            {
                WriteError(replyText, Resources.must_be_integer, true);
                return;
            }
            var result = StartQuery(replyText, "Search_ByYear");
            if (!result) return;
            var startTime = DateTime.UtcNow.ToLocalTime();
            var startTimeString = startTime.ToString("HH:mm");
            WriteText(replyText, $"Getting All VNs For year {year}.  Started at {startTimeString}");
            ListByTB.Text = "";
            _currentList = x => x.RelDate.StartsWith(ListByTB.Text);
            _currentListLabel = $"{ListByTB.Text} (Year)";
            _vnsAdded = 0;
            _vnsSkipped = 0;
            string vnInfoQuery =
                $"get vn basic (released > \"{year - 1}\" and released <= \"{year}\") {{{MaxResultsString}}}";
            result = await TryQuery(vnInfoQuery, Resources.gyt_query_error, replyText, true, true, true);
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
                    $"get vn basic (released > \"{year - 1}\" and released <= \"{year}\") {{{MaxResultsString}, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(vnInfoMoreQuery, Resources.gyt_query_error, replyText, true, true, true);
                if (!moreResult) return;
                var vnMoreRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                List<VNItem> vnMoreItems = vnMoreRoot.Items;
                await GetMultipleVN(vnMoreItems.Select(x => x.ID).ToList(), replyText, true);
                moreResults = vnMoreRoot.More;
            }
            var span = DateTime.UtcNow.ToLocalTime() - startTime;
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            WriteText(replyText,
                span < TimeSpan.FromMinutes(1)
                    ? $"Got VNs for {year} in <1 min. {_vnsAdded}/{_vnsAdded + _vnsSkipped} added."
                    : $"Got VNs for {year} in {span:HH:mm}. {_vnsAdded}/{_vnsAdded + _vnsSkipped} added.", true);
            ChangeAPIStatus(Conn.Status);
        }

        /// <summary>
        /// Get new VNs from VNDB that match selected producer.
        /// </summary>
        private async void Search_Producer()
        {
            var producer = ListByTB.Text;
            if (producer.Equals(""))
            {
                WriteError(replyText, "Enter producer name.");
                return;
            }
            var askBox = MessageBox.Show(Resources.update_custom_filter, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            var producerItem = ProducerList.Find(x => x.Name.Equals(producer, StringComparison.InvariantCultureIgnoreCase));
            if (producerItem == null)
            {
                var askBox2 = MessageBox.Show($"A producer named {producer} was not found in local database.\nWould you like to search VNDB?", Resources.are_you_sure, MessageBoxButtons.YesNo);
                if (askBox2 != DialogResult.Yes) return;
                var result2 = StartQuery(replyText, "Update Producer Titles");
                if (!result2) return;
                var producers = await AddProducersBySearchedName(producer, replyText);
                if (producers == null) return;
                if (producers.Count == 0)
                {
                    WriteError(replyText, $"{producer} was not found.");
                    ChangeAPIStatus(Conn.Status);
                    return;
                }
                if (!producers.Exists(x => x.Name.Equals(producer)))
                {
                    WriteError(replyText, $"{producer} wasn't found but {producers.Count} other producers were added.");
                    await ReloadListsFromDbAsync();
                    ChangeAPIStatus(Conn.Status);
                    return;
                }
                ChangeAPIStatus(Conn.Status);
                await ReloadListsFromDbAsync();
                producerItem = ProducerList.Find(x => x.Name.Equals(producer, StringComparison.InvariantCultureIgnoreCase));
                ListByTB.Text = producer;
            }
            var result = StartQuery(replyText, "Update Producer Titles");
            if (!result) return;
            _vnsAdded = 0;
            _vnsSkipped = 0;
            await GetProducerTitles(producerItem, replyText);
            WriteText(replyText, $"Got new VNs for {producerItem.Name}, added {_vnsAdded} titles.");
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            ChangeAPIStatus(Conn.Status);
        }

        #endregion

        #region Listing

        /// <summary>
        /// Toggle between wide view (see more results) and normal view
        /// </summary>
        private void ToggleWideView(object sender, EventArgs e)
        {
            _wideView = !_wideView;
            if (_wideView)
            {
                panel1.Visible = false;
                panel2.Visible = false;
                tabControl2.Visible = false;
                panel3.Location = new Point(6, 6);
                tileOLV.Location = new Point(6, 65);
                tileOLV.Height += 300;
                toggleViewButton.Text = "▼ Show Options ▼";
            }
            else
            {
                panel1.Visible = true;
                panel2.Visible = true;
                tabControl2.Visible = true;
                panel3.Location = new Point(6, 306);
                tileOLV.Location = new Point(6, 365);
                tileOLV.Height -= 300;
                toggleViewButton.Text = "▲ Hide Options ▲";
            }
        }

        /// <summary>
        /// Handle selected multi-selection action.
        /// </summary>
        private async void MultiActionSelect(object sender, EventArgs e)
        {
            if (multiActionBox.SelectedIndex < 1) return;
            if (tileOLV.SelectedObjects.Count < 1)
            {
                WriteText(replyText, "No titles selected.");
                multiActionBox.SelectedIndex = 0;
                return;
            }
            var titles = tileOLV.SelectedObjects.Cast<ListedVN>().ToArray();
            switch (multiActionBox.SelectedIndex)
            {
                case 1:
                    tileOLV.SelectedObjects = null;
                    break;
                case 2:
                    string message = $"You've selected {titles.Length} titles.\nAre you sure you wish to remove them from local database?";
                    var messageBox = MessageBox.Show(message, "Confirm Action", MessageBoxButtons.YesNo);
                    if (messageBox != DialogResult.Yes)
                    {
                        multiActionBox.SelectedIndex = 0;
                        return;
                    }
                    WriteWarning(replyText, "Removing titles...");
                    await Task.Run(() => RemoveTitlesFromDB(titles));
                    await ReloadListsFromDbAsync();
                    LoadVNListToGui();
                    WriteText(replyText, "Titles Removed.");
                    break;
            }
            multiActionBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Remove titles and associated images from local database.
        /// </summary>
        /// <param name="titles">Titles to be removed</param>
        private void RemoveTitlesFromDB(ListedVN[] titles)
        {
            foreach (var title in titles)
            {
                var screenItems = JsonConvert.DeserializeObject<ScreenItem[]>(title.Screens);
                foreach (var screen in screenItems)
                {
                    try
                    {
                        File.Delete(screen.StoredLocation());
                    }
                    catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException) { }
                }
                try
                {
                    File.Delete($"{VNImagesFolder}\\{title.VNID}{Path.GetExtension(title.ImageURL)}");
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException) { }
            }
            DBConn.BeginTransaction();
            foreach (var title in titles) DBConn.RemoveVisualNovel(title.VNID);
            DBConn.EndTransaction();
        }

        /// <summary>
        /// Change ListBy TextBox in accordance to selected function.
        /// </summary>
        private void ChangeListBy(object sender, EventArgs e)
        {
            ListByTB.Text = "";
            switch (ListByCB.SelectedIndex)
            {
                case (int)ListBy.Name:
                    ListByTB.Visible = true;
                    groupListBox.Visible = false;
                    ListByUpdateButton.Enabled = true;
                    ListByGoButton.Enabled = true;
                    ListByTB.AutoCompleteMode = AutoCompleteMode.None;
                    break;
                case (int)ListBy.Producer:
                    ListByTB.Visible = true;
                    groupListBox.Visible = false;
                    ListByUpdateButton.Enabled = true;
                    ListByGoButton.Enabled = true;
                    PopulateProducerSearchBox();
                    break;
                case (int)ListBy.Year:
                    ListByTB.Visible = true;
                    groupListBox.Visible = false;
                    ListByUpdateButton.Enabled = true;
                    ListByGoButton.Enabled = true;
                    ListByTB.AutoCompleteMode = AutoCompleteMode.None;
                    break;
                case (int)ListBy.Group:
                    ListByTB.Visible = false;
                    groupListBox.Visible = true;
                    ListByUpdateButton.Enabled = false;
                    ListByGoButton.Enabled = false;
                    return;

            }
        }

        /// <summary>
        /// Only allow Digits in input for List By Year.
        /// </summary>
        private void ListByTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ListByCB.SelectedIndex == (int)ListBy.Year)
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        /// <summary>
        /// Enter Key on ListBy TextBox.
        /// </summary>
        private void ListByTB_KeyUp(object sender, KeyEventArgs e)
        {
            switch (ListByCB.SelectedIndex)
            {
                case (int)ListBy.Name:
                    if (e.KeyCode == Keys.Enter) List_Name();
                    return;
                case (int)ListBy.Producer:
                    if (e.KeyCode == Keys.Enter) List_Producer();
                    return;
                case (int)ListBy.Year:
                    if (e.KeyCode == Keys.Enter) List_Year();
                    return;
                case (int)ListBy.Group:
                    if (e.KeyCode == Keys.Enter) List_Group(null, null);
                    return;
            }
        }

        /// <summary>
        /// Run selected List function.
        /// </summary>
        private void ListByGo(object sender, EventArgs e)
        {
            switch (ListByCB.SelectedIndex)
            {
                case (int)ListBy.Name:
                    List_Name();
                    return;
                case (int)ListBy.Producer:
                    List_Producer();
                    return;
                case (int)ListBy.Year:
                    List_Year();
                    return;
            }
        }

        private void List_Name()
        {
            if (ListByTB.Text == "") //check if box is empty
            {
                WriteError(replyText, Resources.enter_vn_title, true);
                return;
            }
            if (ListByTB.Text.Length < 3)
            {
                WriteError(replyText, Resources.enter_vn_title + " (atleast 3 chars)", true);
                return;
            }
            var searchString = ListByTB.Text.ToLowerInvariant();
            List_ClearOther(skipListBox: true);
            _currentList = vn => vn.Title.ToLowerInvariant().Contains(searchString) || vn.KanjiTitle.ToLowerInvariant().Contains(searchString);
            _currentListLabel = $"{searchString} (Search)";
            LoadVNListToGui();
        }

        /// <summary>
        /// List titles released in specified year.
        /// </summary>
        private void List_Year()
        {
            if (ListByTB.Text == "") //check if box is empty
            {
                WriteError(replyText, Resources.enter_year, true);
                return;
            }
            int year;
            var userIsNumber = int.TryParse(ListByTB.Text, out year);
            if (userIsNumber == false) //check if box has integer
            {
                WriteError(replyText, Resources.must_be_integer, true);
                return;
            }
            List_ClearOther(skipListBox: true);
            _currentList = vn => vn.ReleasedInYear(year);
            _currentListLabel = $"{year} (Year)";
            LoadVNListToGui();
        }

        /// <summary>
        /// Display all Visual Novels in local database.
        /// </summary>
        private void List_All(object sender, EventArgs e)
        {
            List_ClearOther();
            _currentList = x => true;
            _currentListLabel = "All Titles";
            LoadVNListToGui();
        }

        /// <summary>
        /// Clear other listing options.
        /// </summary>
        private void List_ClearOther(bool skipListBox = false)
        {
            DontTriggerEvent = true;
            ulStatusDropDown.SelectedIndex = 0;
            wlStatusDropDown.SelectedIndex = 0;
            if (!skipListBox) ListByTB.Text = "";
            DontTriggerEvent = false;
        }

        /// <summary>
        /// Display VNs by producers in Favorite Producers list.
        /// </summary>
        private void List_FavoriteProducers(object sender, EventArgs e)
        {
            if (DontTriggerEvent) return;
            List_ClearOther();
            if (olFavoriteProducers.Items.Count == 0)
            {
                WriteError(replyText, "No Favorite Producers.", true);
                return;
            }
            IEnumerable<string> prodList = from ListedProducer producer in olFavoriteProducers.Objects
                                           select producer.Name;
            _currentList = vn => prodList.Contains(vn.Producer);
            _currentListLabel = "Favorite Producers";
            LoadVNListToGui();
        }

        /// <summary>
        /// Display VNs with selected Userlist status.
        /// </summary>
        private void List_ULStatus(object sender, EventArgs e)
        {
            if (DontTriggerEvent) return;
            var dropdownlist = (ComboBox)sender;
            switch (dropdownlist.SelectedIndex)
            {
                case 0:
                    List_All(null, null);
                    return;
                case 1:
                    dropdownlist.SelectedIndex = 0;
                    return;
                case 2:
                    _currentList = x => !x.ULStatus.Equals("");
                    _currentListLabel = "Userlist Titles";
                    break;
                case 3:
                    _currentList = x => x.ULStatus.Equals("Unknown");
                    _currentListLabel = "UL Unknown";
                    break;
                case 4:
                    _currentList = x => x.ULStatus.Equals("Playing");
                    _currentListLabel = "UL Playing";
                    break;
                case 5:
                    _currentList = x => x.ULStatus.Equals("Finished");
                    _currentListLabel = "UL Finished";
                    break;
                case 6:
                    _currentList = x => x.ULStatus.Equals("Stalled");
                    _currentListLabel = "UL Stalled";
                    break;
                case 7:
                    _currentList = x => x.ULStatus.Equals("Dropped");
                    _currentListLabel = "UL Dropped";
                    break;
            }
            var value = dropdownlist.SelectedIndex;
            List_ClearOther();
            DontTriggerEvent = true;
            ulStatusDropDown.SelectedIndex = value;
            DontTriggerEvent = false;
            LoadVNListToGui();
        }

        /// <summary>
        /// Display VNs with selected Wishlist status.
        /// </summary>
        private void List_WLStatus(object sender, EventArgs e)
        {
            if (DontTriggerEvent) return;
            var dropdownlist = (ComboBox)sender;
            switch (dropdownlist.SelectedIndex)
            {
                case 0:
                    List_All(null, null);
                    return;
                case 1:
                    dropdownlist.SelectedIndex = 0;
                    return;
                case 2:
                    _currentList = x => !x.WLStatus.Equals("");
                    _currentListLabel = "Wishlist Titles";
                    break;
                case 3:
                    _currentList = x => x.WLStatus.Equals("High");
                    _currentListLabel = "WL High";
                    break;
                case 4:
                    _currentList = x => x.WLStatus.Equals("Medium");
                    _currentListLabel = "WL Medium";
                    break;
                case 5:
                    _currentList = x => x.WLStatus.Equals("Low");
                    _currentListLabel = "WL Low";
                    break;
                case 6:
                    _currentList = x => x.WLStatus.Equals("Blacklist");
                    _currentListLabel = "WL Blacklist";
                    break;
            }
            var value = dropdownlist.SelectedIndex;
            List_ClearOther();
            DontTriggerEvent = true;
            wlStatusDropDown.SelectedIndex = value;
            DontTriggerEvent = false;
            LoadVNListToGui();
        }

        /// <summary>
        /// Display VNs in user-defined group that is typed/selected in box.
        /// </summary>
        private void List_Group(object sender, EventArgs e)
        {
            var groupName = groupListBox.Text;
            if (groupName.Equals("(Group)")) return;
            List_ClearOther();
            _currentList = x => x.IsInGroup(groupName);
            _currentListLabel = $"{groupName} (Group)";
            LoadVNListToGui(skipGroupSearch: true);
        }

        private void List_GroupEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (groupListBox.Text.Equals("")) return;
            List_Group(null, null);
        }

        /// <summary>
        /// Display VNs by producer typed/selected in box.
        /// </summary>
        private void List_Producer(string producerName = null)
        {
            producerName = producerName ?? ListByTB.Text;
            if (producerName.Equals(""))
            {
                WriteError(replyText, "Enter producer name.");
                return;
            }
            List_ClearOther(skipListBox: true);
            _currentList = x => x.Producer.Equals(producerName, StringComparison.InvariantCultureIgnoreCase);
            _currentListLabel = $"{producerName} (Producer)";
            LoadVNListToGui();
        }

        #endregion

        #region Filtering

        /// <summary>
        /// Filter titles by URT status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter_URT(object sender, EventArgs e)
        {
            if (DontTriggerEvent) return;
            Toggles.URTToggleSetting = (ToggleSetting)URTToggleBox.SelectedIndex;
            ApplyListFilters();
        }

        /// <summary>
        /// Filter titles by released status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter_Unreleased(object sender, EventArgs e)
        {
            if (DontTriggerEvent) return;
            Toggles.UnreleasedToggleSetting = (ToggleSetting)UnreleasedToggleBox.SelectedIndex;
            ApplyListFilters();
        }

        /// <summary>
        /// Filter titles by blacklist status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter_Blacklist(object sender, EventArgs e)
        {
            if (DontTriggerEvent) return;
            Toggles.BlacklistToggleSetting = (ToggleSetting)BlacklistToggleBox.SelectedIndex;
            ApplyListFilters();
        }

        /// <summary>
        /// Get function for the specified filter.
        /// </summary>
        /// <param name="toggle">Which filter to get function from</param>
        /// <returns>Function for specified filter</returns>
        public Func<ListedVN, bool> GetFunc(ToggleFilter toggle)
        {
            var function = new Func<ListedVN, bool>(x => true);
            switch (toggle)
            {
                case ToggleFilter.URT:
                    switch (Toggles.URTToggleSetting)
                    {
                        case ToggleSetting.Show:
                            return function;
                        case ToggleSetting.Hide:
                            return x => URTList.Find(y => y.VNID == x.VNID) == null;
                        case ToggleSetting.Only:
                            return x => URTList.Find(y => y.VNID == x.VNID) != null;
                        case ToggleSetting.OnlyUnplayed:
                            return x => !x.ULStatus.Equals("Finished") && !x.ULStatus.Equals("Dropped");
                        default: return function;
                    }
                case ToggleFilter.Unreleased:
                    switch (Toggles.UnreleasedToggleSetting)
                    {
                        case ToggleSetting.Show:
                            return function;
                        case ToggleSetting.Hide:
                            return x => !DateIsUnreleased(x.RelDate);
                        case ToggleSetting.Only:
                            return x => DateIsUnreleased(x.RelDate);
                        case ToggleSetting.HideNoReleaseDate:
                            return x => x.DateForSorting != DateTime.MaxValue;
                        default: return function;
                    }
                case ToggleFilter.Blacklisted:
                    switch (Toggles.BlacklistToggleSetting)
                    {
                        case ToggleSetting.Show:
                            return function;
                        case ToggleSetting.Hide:
                            return x => !x.WLStatus.Equals("Blacklist");
                        case ToggleSetting.Only:
                            return x => x.WLStatus.Equals("Blacklist");
                        default: return function;
                    }
                default:
                    return function;
            }
        }

        /// <summary>
        /// Apply toggle filters to list.
        /// </summary>
        private void ApplyListFilters()
        {
            tagSignaler.BackColor = _activeTagFilter.Any() ? SignalerActive : SignalerDefault;
            traitSignaler.BackColor = _activeTraitFilter.Any() ? SignalerActive : SignalerDefault;
            Func<ListedVN, bool>[] funcArray;
            //do OR for tag/trait filters
            if (ToggleFiltersModeButton.Checked)
            {
                //if both are active
                if (_activeTagFilter.Any() && _activeTraitFilter.Any())
                {
                    funcArray = new[] { GetFunc(ToggleFilter.URT), GetFunc(ToggleFilter.Unreleased), GetFunc(ToggleFilter.Blacklisted),
                    vn => VNMatchesTagFilter(vn) || _traitFunction(vn) };

                }
                //if only tagfilter is active
                else if (_activeTagFilter.Any() && !_activeTraitFilter.Any())
                {
                    funcArray = new[] { GetFunc(ToggleFilter.URT), GetFunc(ToggleFilter.Unreleased), GetFunc(ToggleFilter.Blacklisted),
                    VNMatchesTagFilter };
                }
                //if only traitfilter is active
                else if (!_activeTagFilter.Any() && _activeTraitFilter.Any())
                {
                    funcArray = new[]
                    {
                        GetFunc(ToggleFilter.URT), GetFunc(ToggleFilter.Unreleased), GetFunc(ToggleFilter.Blacklisted),
                        _traitFunction
                    };
                }
                //if none are active
                else
                {
                    funcArray = new[] { GetFunc(ToggleFilter.URT), GetFunc(ToggleFilter.Unreleased), GetFunc(ToggleFilter.Blacklisted) };
                }
            }
            else
            {
                funcArray = new[] { GetFunc(ToggleFilter.URT), GetFunc(ToggleFilter.Unreleased), GetFunc(ToggleFilter.Blacklisted), VNMatchesTagFilter, _traitFunction };
            }
            tileOLV.ModelFilter = new ModelFilter(vn => funcArray.Select(filter => filter((ListedVN)vn)).All(valid => valid));
            objectList_ItemsChanged(null, null);
            SaveMainXML();
        }

        private void ToggleFiltersMode(object sender, EventArgs e)
        {
            if (ToggleFiltersModeButton.Checked)
            {
                ToggleFiltersModeButton.BackColor = Color.LightGreen;
                ToggleFiltersModeButton.ForeColor = Color.Black;
                ToggleFiltersModeButton.Text = @"Or";
            }
            else
            {
                ToggleFiltersModeButton.BackColor = Color.Black;
                ToggleFiltersModeButton.ForeColor = Color.White;
                ToggleFiltersModeButton.Text = @"And";
            }
            //only reapply filters if both are active (if only one is active, the result would be the same)
            if (_activeTagFilter.Any() && _activeTraitFilter.Any())
            {
                ApplyListFilters();
            }
        }
        #endregion

        #region List Results

        private void Help_ListResults(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            var helpFile = $"{Path.Combine(path, "Program Data\\Help\\listresults.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

        /// <summary>
        /// Load Visual Novel Form with details of visual novel that was left clicked.
        /// </summary>
        private void VisualNovelLeftClick(object sender, CellClickEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control)) return;
            var listView = (ObjectListView)sender;
            if (listView.SelectedIndices.Count <= 0) return;
            var vnItem = (ListedVN)listView.SelectedObjects[0];
            VNControl vnf;
            if (CurrentFeatureName.Equals(""))
            {
                DBConn.Open();
                vnItem = DBConn.GetSingleVN(vnItem.VNID, Settings.UserID);
                DBConn.Close();
                vnf = new VNControl(vnItem, this);
            }
            else
            {
                vnf = new VNControl(vnItem, this, false);
            }
            var tabPage = new TabPage("VN - " + vnItem.Title);
            tabPage.Controls.Add(vnf);
            tabControl1.TabPages.Add(tabPage);
        }

        //format list rows, color according to userlist status, only for Details View
        private void FormatVNRow(object sender, FormatRowEventArgs e)
        {
            if (e.ListView.View != View.Details) return;
            var listedVN = (ListedVN)e.Model;
            //ULStatus takes priority over WLStatus
            switch (listedVN.WLStatus)
            {
                case "High":
                    e.Item.BackColor = WLHighBrush.Color;
                    break;
                case "Medium":
                    e.Item.BackColor = WLMediumBrush.Color;
                    break;
                case "Low":
                    e.Item.BackColor = WLLowBrush.Color;
                    break;
            }
            switch (listedVN.ULStatus)
            {
                case "Finished":
                    e.Item.BackColor = ULFinishedBrush.Color;
                    break;
                case "Stalled":
                    e.Item.BackColor = ULStalledBrush.Color;
                    break;
                case "Dropped":
                    e.Item.BackColor = ULDroppedBrush.Color;
                    break;
                case "Unknown":
                    e.Item.BackColor = ULUnknownBrush.Color;
                    break;
            }
            if (listedVN.ULStatus.Equals("Playing")) e.Item.GetSubItem(tileColumnULS.Index).ForeColor = ULPlayingBrush.Color;
            if (FavoriteProducerList.Any() && FavoriteProducerList.Exists(x => x.Name == listedVN.Producer))
            {
                //e.Item.GetSubItem(tileColumnProducer.Index).ForeColor = FavoriteProducerBrush.Color;
                e.Item.GetSubItem(tileColumnProducer.Index).ForeColor = Color.Blue;
            }
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(-1);
            if (listedVN.ULAdded == dateTimeOffset) e.Item.GetSubItem(tileColumnULAdded.Index).Text = "";
            if (listedVN.WLAdded == dateTimeOffset) e.Item.GetSubItem(tileColumnWLAdded.Index).Text = "";
            if (listedVN.Vote < 1) e.Item.GetSubItem(tileColumnVote.Index).Text = "";
            e.Item.GetSubItem(tileColumnDate.Index).Text = listedVN.RelDate;
            e.Item.GetSubItem(tileColumnRating.Index).Text = listedVN.VoteCount > 0 ? $"{listedVN.Rating:0.00} ({listedVN.VoteCount} Votes)" : "";
            e.Item.GetSubItem(tileColumnPopularity.Index).Text = listedVN.Popularity > 0 ? listedVN.Popularity.ToString("0.00") : "";
        }

        //format individual cell (only for details view)
        private void FormatVNCell(object sender, FormatCellEventArgs e)
        {
            if (tileOLV.View != View.Details) return;
            ListedVN vn = e.Model as ListedVN;
            if (vn == null) return;
            if (e.ColumnIndex == tileColumnULS.Index)
            {
                if (vn.ULStatus.Equals("Playing")) e.SubItem.ForeColor = ULPlayingBrush.Color;
            }
            else if (e.ColumnIndex == tileColumnProducer.Index)
            {
                if (FavoriteProducerList.Exists(x => x.Name.Equals(vn.Producer))) e.SubItem.ForeColor = FavoriteProducerBrush.Color;
            }

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
            if (!vn.ULStatus.Equals(""))
            {
                addChangeVNNoteToolStripMenuItem.Enabled = true;
                addChangeVNGroupsToolStripMenuItem.Enabled = true;
            }
            var producers = olFavoriteProducers.Objects as List<ListedProducer>;
            if (producers?.Find(x => x.Name == vn.Producer) != null)
            {
                addProducerToFavoritesToolStripMenuItem.Enabled = false;
                addProducerToFavoritesToolStripMenuItem.ToolTipText = @"Already in list.";
            }
            return ContextMenuVN;
        }

        /// <summary>
        /// Handle VN status change via context menu.
        /// </summary>
        private async void RightClickChangeVNStatus(object sender, ToolStripItemClickedEventArgs e)
        {
            if (Conn.LogIn != VndbConnection.LogInStatus.YesWithPassword)
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
                    if (vn.ULStatus.Equals(nitem.Text))
                    {
                        WriteText(replyText, $"{TruncateString(vn.Title, 20)} already has that status.");
                        return;
                    }
                    statusInt = Array.IndexOf(ListedVN.StatusUL, nitem.Text);
                    success = await ChangeVNStatus(vn, ChangeType.UL, statusInt);
                    break;
                case "Wishlist":
                    if (vn.WLStatus.Equals(nitem.Text))
                    {
                        WriteText(replyText, $"{TruncateString(vn.Title, 20)} already has that status.");
                        return;
                    }
                    statusInt = Array.IndexOf(ListedVN.PriorityWL, nitem.Text);
                    success = await ChangeVNStatus(vn, ChangeType.WL, statusInt);
                    break;
                case "Vote":
                    double newVoteValue = -1;
                    switch (nitem.Text)
                    {
                        case "(None)":
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
                    if (Math.Abs(vn.Vote - newVoteValue) < 0.001)
                    {
                        WriteText(replyText, $"{TruncateString(vn.Title, 20)} already has that status.");
                        return;
                    }
                    success = await ChangeVNStatus(vn, ChangeType.Vote, statusInt, newVoteValue);
                    break;
                default:
                    return;
            }
            if (!success) return;
            WriteText(replyText, $"{TruncateString(vn.Title, 20)} status changed.");
        }

        private void RightClickShowProducerTitles(object sender, EventArgs e)
        {
            var vn = tileOLV.SelectedObject as ListedVN;
            if (vn == null) return;
            List_Producer(vn.Producer);
        }

        private async void RightClickAddProducer(object sender, EventArgs e)
        {
            var vn = tileOLV.SelectedObject as ListedVN;
            if (vn == null) return;
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
                new ListedProducer(vn.Producer, producerVNs.Length, DateTime.UtcNow,
                    ProducerList.Find(x => x.Name == vn.Producer).ID,
                    userAverageVote, (int) Math.Round(userDropRate*100))
            };
            DBConn.BeginTransaction();
            DBConn.InsertFavoriteProducers(addProducerList, Settings.UserID);
            DBConn.EndTransaction();
            await ReloadListsFromDbAsync();
            LoadFPListToGui();
            WriteText(replyText, $"{vn.Producer} added to list.");
        }

        /// <summary>
        /// Add note to title in user's vnlist.
        /// </summary>
        private async void RightClickAddNote(object sender, EventArgs e)
        {
            if (Conn.LogIn != VndbConnection.LogInStatus.YesWithPassword)
            {
                WriteError(replyText, "Not Logged In", true);
                return;
            }
            var vn = tileOLV.SelectedObject as ListedVN;
            if (vn == null) return;
            CustomItemNotes itemNotes = vn.GetCustomItemNotes();
            StringBuilder notesSb = new StringBuilder(itemNotes.Notes);
            var result = new InputDialogBox(notesSb, "Add Note to Title", "Enter Note:").ShowDialog();
            if (result != DialogResult.OK) return;
            if (notesSb.ToString().Contains('\n'))
            {
                WriteError(replyText, "Note cannot contain newline characters.");
                return;
            }
            itemNotes.Notes = notesSb.ToString();
            await UpdateItemNotes("Added note to title", vn.VNID, itemNotes);

        }

        /// <summary>
        /// Add title in user's vnlist to a user-defined group.
        /// </summary>
        private async void RightClickAddGroup(object sender, EventArgs e)
        {
            if (Conn.LogIn != VndbConnection.LogInStatus.YesWithPassword)
            {
                WriteError(replyText, "Not Logged In", true);
                return;
            }
            var vn = tileOLV.SelectedObject as ListedVN;
            if (vn == null) return;
            CustomItemNotes itemNotes = vn.GetCustomItemNotes();
            var result = new ListDialogBox(itemNotes.Groups, "Add Title to Groups", $"{vn.Title} is in groups:").ShowDialog();
            if (result != DialogResult.OK) return;
            if (itemNotes.Groups.Any(group => group.Contains('\n')))
            {
                WriteError(replyText, "Group name cannot contain newline characters.");
                return;
            }
            await UpdateItemNotes("Added title to group(s).", vn.VNID, itemNotes);
        }

        /// <summary>
        /// Send query to VNDB to update a VN's notes and if successful, update database.
        /// </summary>
        /// <param name="replyMessage">Message to be printed if query is successful</param>
        /// <param name="vnid">ID of VN</param>
        /// <param name="itemNotes">Object containing new data to replace old</param>
        private async Task UpdateItemNotes(string replyMessage, int vnid, CustomItemNotes itemNotes)
        {
            var result = StartQuery(replyText, "Update Item Notes");
            if (!result) return;
            string serializedNotes = itemNotes.Serialize();
            var query = $"set vnlist {vnid} {{\"notes\":\"{serializedNotes}\"}}";
            var apiResult = await TryQuery(query, "UIN Query Error", replyText);
            if (!apiResult) return;
            DBConn.Open();
            DBConn.AddNoteToVN(vnid, serializedNotes, Settings.UserID);
            DBConn.Close();
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            WriteText(replyText, replyMessage);
            ChangeAPIStatus(Conn.Status);
        }

        /// <summary>
        /// Change view of Visual Novel ObjectListView.
        /// </summary>
        private void OLVChangeView(object sender, EventArgs e)
        {
            if (DontTriggerEvent) return;
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
        /// Update result label when items in Visual Novel OLV are changed.
        /// </summary>
        private void objectList_ItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            if (tileOLV.FilteredObjects == null) return;
            var count = tileOLV.FilteredObjects.Cast<object>().Count();
            var totalcount = tileOLV.Objects.Cast<object>().Count();
            string itemCountString = tileOLV.ModelFilter != null
                ? $"{count}/{totalcount} items."
                : $"{tileOLV.Items.Count} items.";
            resultLabel.Text = $@"List: {_currentListLabel} {itemCountString}";
            DisplayCommonTags(null, null);
        }

        /// <summary>
        /// Adjust tile size on OLV resize to avoid empty spaces.
        /// </summary>
        private void tileOLV_Resize(object sender, EventArgs e)
        {
            var width = tileOLV.Width - 24;
            var s = (int)Math.Round((double)width / 230);
            if (s == 0) return;
            tileOLV.TileSize = new Size(width / s, 300);
        }

        #endregion

        #region Classes and Enums
#pragma warning disable 1591
        /// <summary>
        /// Specifies ListBy mode.
        /// </summary>
        private enum ListBy { Name, Producer, Year, Group }

        /// <summary>
        /// Specifies toggle filter
        /// </summary>
        public enum ToggleFilter
        {
            URT,
            Unreleased,
            Blacklisted
        }

        /// <summary>
        /// Specifies toggle setting
        /// </summary>
        public enum ToggleSetting
        {
            Show,
            Hide,
            Only,
            OnlyUnplayed = 3,
            HideNoReleaseDate = 3
        }

        /// <summary>
        /// Class holding toggle filter settings.
        /// </summary>
        [Serializable, XmlRoot("ToggleArray")]
        public class ToggleArray
        {
            /// <summary>
            /// Empty constructor needed for XML
            /// </summary>
            public ToggleArray()
            {
                URTToggleSetting = 0;
                UnreleasedToggleSetting = 0;
                BlacklistToggleSetting = 0;
            }
            public ToggleSetting URTToggleSetting { get; set; }
            public ToggleSetting UnreleasedToggleSetting { get; set; }
            public ToggleSetting BlacklistToggleSetting { get; set; }

        }
#pragma warning restore 1591
        #endregion
    }
}
