using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using BrightIdeasSoftware;
using Happy_Search.Properties;
using Newtonsoft.Json;

namespace Happy_Search
{
    partial class FormMain
    {
        private static readonly ToggleArray Toggles = new ToggleArray();

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

        #region Searching

        /// <summary>
        /// Searches for VNs from VNDB, adds them if they are not in local database.
        /// </summary>
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
            _currentListLabel = $"{searchBox.Text} (Search)";
            ReloadLists();
            RefreshVNList();
        }

        /// <summary>
        /// Gets VNs released in the year entered by user, doesn't update VNs already in local database
        /// </summary>
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
            _currentListLabel = $"{yearBox.Text} (Year)";
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

        #region Listing

        /// <summary>
        /// Display all Visual Novels in local database.
        /// </summary>
        private void List_All(object sender, EventArgs e)
        {
            List_ClearOther();
            _currentList = x => true;
            _currentListLabel = "All Titles";
            RefreshVNList();
        }

        /// <summary>
        /// Clear other listing options.
        /// </summary>
        private void List_ClearOther()
        {
            DontTriggerEvent = true;
            ulStatusDropDown.SelectedIndex = 0;
            wlStatusDropDown.SelectedIndex = 0;
            ProducerListBox.Text = "";
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
            RefreshVNList();
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
            RefreshVNList();
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
            RefreshVNList();
        }

        /// <summary>
        /// Display VNs by producer typed/selected in box.
        /// </summary>
        private void List_Producer(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            var producerName = ProducerListBox.Text;
            if (!producerName.Any()) return;
            List_ClearOther();
            _currentList = x => x.Producer.Equals(producerName, StringComparison.InvariantCultureIgnoreCase);
            _currentListLabel = $"{producerName} (Producer)";
            ProducerListBox.Text = producerName;
            RefreshVNList();
        }

        /// <summary>
        /// Get new VNs from VNDB that match selected producer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateProducerTitles(object sender, EventArgs e)
        {
            var producer = ProducerListBox.Text;
            if (producer.Equals("")) return;
            var askBox = MessageBox.Show(Resources.update_custom_filter + '\n' + Resources.are_you_sure, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            var producerItem = _producerList.Find(x => x.Name.Equals(producer, StringComparison.InvariantCultureIgnoreCase));
            if (producerItem == null)
            {
                //TODO
                WriteError(replyText, "NYI (Producer not in local db)", true);
                return;
            }
            ReloadLists();
            _vnsAdded = 0;
            _vnsSkipped = 0;
            await GetProducerTitles(producerItem, replyText);
            WriteText(replyText, $"Got new VNs for {producerItem.Name}, added {_vnsAdded} titles.");
            ReloadLists();
            RefreshVNList();
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
                            return x => !IsUnreleased(x.RelDate);
                        case ToggleSetting.Only:
                            return x => IsUnreleased(x.RelDate);
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
            Func<ListedVN, bool>[] funcArray = { GetFunc(ToggleFilter.URT), GetFunc(ToggleFilter.Unreleased), GetFunc(ToggleFilter.Blacklisted), VNMatchesTagFilter, _traitFunction };
            tileOLV.ModelFilter = new ModelFilter(vn => funcArray.Select(filter => filter((ListedVN)vn)).All(valid => valid));
            objectList_ItemsChanged(null, null);
            SaveMainXML();
        }

        #endregion


        #region List Results

        private void Help_ListResults(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            if (path == null)
            {
                WriteError(replyText, @"Unknown Path Error");
                return;
            }
            var helpFile = $"{Path.Combine(path, "help\\listresults.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
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
        /// Handle VN status change via context menu.
        /// </summary>
        private async void RightClickChangeVNStatus(object sender, ToolStripItemClickedEventArgs e)
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

        private void RightClickShowProducerTitles(object sender, EventArgs e)
        {
            var vn = tileOLV.SelectedObject as ListedVN;
            if (vn == null) return;
            ProducerListBox.Text = vn.Producer;
            List_Producer(null, new KeyEventArgs(Keys.Enter));
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
            resultLabel.Text = $"List: {_currentListLabel} {itemCountString}";
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
            OnlyUnplayed
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
