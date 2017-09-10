using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Happy_Search.Properties;
using Happy_Search.Other_Forms;
using Newtonsoft.Json;
using Happy_Apps_Core;
using static Happy_Apps_Core.StaticHelpers;

namespace Happy_Search
{
    public partial class FormMain
    {
        /// <summary>
        /// Display html file explaining searching, listing and filtering section.
        /// </summary>
        private void Help_ListingAndSearching(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            Debug.Assert(path != null, nameof(path) + " != null");
            var helpFile = $"{Path.Combine(path, "Program Data\\Help\\listingandsearching.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

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
                panel3.Location = new Point(6, 6);
                panel3.Height += 246;
                toggleViewButton.Text = @"▼ Show Options ▼";
            }
            else
            {
                panel1.Visible = true;
                panel2.Visible = true;
                panel3.Location = new Point(6, 252);
                panel3.Height -= 246;
                toggleViewButton.Text = @"▲ Hide Options ▲";
            }
        }

        /// <summary>
        /// Handle selected multi-selection action.
        /// </summary>
        private async void MultiActionSelect(object sender, EventArgs e)
        {
            if (multiActionBox.SelectedIndex < 1) return;
            if (multiActionBox.SelectedIndex == 1)
            {
                multiActionBox.SelectedIndex = 0;
                return;
            }
            if (tileOLV.SelectedObjects.Count < 1)
            {
                WriteText(replyText, "No titles selected.");
                multiActionBox.SelectedIndex = 0;
                return;
            }
            var titles = tileOLV.SelectedObjects.Cast<ListedVN>().ToArray();
            switch (multiActionBox.SelectedIndex)
            {
                case 2:
                    tileOLV.SelectedObjects = null;
                    break;
                case 3:
                    string message3 = $"You've selected {titles.Length} titles.\nAre you sure you wish to remove them from local database?";
                    var messageBox3 = MessageBox.Show(message3, Resources.Confirm_Action, MessageBoxButtons.YesNo);
                    if (messageBox3 != DialogResult.Yes)
                    {
                        multiActionBox.SelectedIndex = 0;
                        return;
                    }
                    WriteWarning(replyText, "Removing titles...");
                    await Task.Run(() => RemoveTitlesFromDB(titles));
                    ReloadListsFromDb();
                    LoadVNListToGui();
                    WriteText(replyText, "Titles Removed.");
                    multiActionBox.SelectedIndex = 0;
                    return;
                case 4:
                    string message4 = $"You've selected {titles.Length} titles.\nAre you sure you wish to update tags traits and stats for them?";
                    var messageBox4 = MessageBox.Show(message4, Resources.Confirm_Action, MessageBoxButtons.YesNo);
                    if (messageBox4 != DialogResult.Yes)
                    {
                        multiActionBox.SelectedIndex = 0;
                        return;
                    }
                    if (!Conn.StartQuery(replyText, "UpdateTagsTraitsStats (MA)", true, true, false))
                    {
                        multiActionBox.SelectedIndex = 0;
                        return;
                    }
                    WriteWarning(replyText, "Updating titles...");
                    await Conn.UpdateTagsTraitsStats(titles.Select(vn => vn.VNID));
                    break;
                case 5:
                    string message5 = $"You've selected {titles.Length} titles.\nAre you sure you wish to update all data for them?";
                    var messageBox5 = MessageBox.Show(message5, Resources.Confirm_Action, MessageBoxButtons.YesNo);
                    if (messageBox5 != DialogResult.Yes)
                    {
                        multiActionBox.SelectedIndex = 0;
                        return;
                    }
                    if (!Conn.StartQuery(replyText, "Update All Data (MA)", true, true, true))
                    {
                        multiActionBox.SelectedIndex = 0;
                        return;
                    }
                    WriteWarning(replyText, "Updating titles...");
                    await Conn.GetMultipleVN(titles.Select(vn => vn.VNID).ToArray(), true);
                    break;
            }
            ChangeAPIStatus(Conn.Status);
            ReloadListsFromDb();
            LoadVNListToGui();
            WriteText(replyText, "Titles updated.");
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
                var screenItems = JsonConvert.DeserializeObject<VNItem.ScreenItem[]>(title.Screens);
                if (screenItems != null)
                {
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
            }
            LocalDatabase.BeginTransaction();
            foreach (var title in titles) LocalDatabase.RemoveVisualNovel(title.VNID);
            LocalDatabase.EndTransaction();
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
                WriteError(replyText, Resources.enter_vn_title);
                return;
            }
            if (ListByTB.Text.Length < 3)
            {
                WriteError(replyText, Resources.enter_vn_title + " (atleast 3 chars)");
                return;
            }
            var result = Conn.StartQuery(replyText, "Search_ByName", false, false, true);
            if (!result) return;
            var searchString = ListByTB.Text;
            ListByTB.Text = "";
            await Conn.SearchByNameOrAlias(searchString);
            WriteText(replyText, $"Found {Conn.TitlesAdded + Conn.TitlesSkipped} titles, {Conn.TitlesAdded}/{Conn.TitlesAdded + Conn.TitlesSkipped} added.");
            _currentList = vn =>
                vn.Title.ToLowerInvariant().Contains(searchString) ||
                vn.KanjiTitle.ToLowerInvariant().Contains(searchString) ||
                vn.Aliases.ToLowerInvariant().Contains(searchString);
            _currentListLabel = $"{searchString} (Search)";
            LoadVNListToGui();
        }

        /// <summary>
        /// Gets VNs released in the year entered by user, doesn't update VNs already in local database
        /// </summary>
        private async void Search_Year()
        {
            if (ListByTB.Text == "") //check if box is empty
            {
                WriteError(replyText, Resources.enter_year);
                return;
            }
            var userIsNumber = int.TryParse(ListByTB.Text, out int year);
            if (userIsNumber == false) //check if box has integer
            {
                WriteError(replyText, Resources.must_be_integer);
                return;
            }
            var result = Conn.StartQuery(replyText, "Search_ByYear", true, true, true);
            if (!result) return;
            var startTime = DateTime.UtcNow.ToLocalTime();
            var startTimeString = startTime.ToString("HH:mm");
            WriteText(replyText, $"Getting All VNs For year {year}.  Started at {startTimeString}");
            ListByTB.Text = "";
            _currentList = x => x.RelDate.StartsWith(ListByTB.Text);
            _currentListLabel = $"{ListByTB.Text} (Year)";
            string vnInfoQuery =
                $"get vn basic (released > \"{year - 1}\" and released <= \"{year}\") {{{MaxResultsString}}}";
            result = await Conn.TryQuery(vnInfoQuery, Resources.gyt_query_error);
            if (!result) return;
            var vnRoot = JsonConvert.DeserializeObject<ResultsRoot<VNItem>>(Conn.LastResponse.JsonPayload);
            List<VNItem> vnItems = vnRoot.Items;
            await Conn.GetMultipleVN(vnItems.Select(x => x.ID).ToArray(), false);
            var pageNo = 1;
            var moreResults = vnRoot.More;
            while (moreResults)
            {
                pageNo++;
                string vnInfoMoreQuery =
                    $"get vn basic (released > \"{year - 1}\" and released <= \"{year}\") {{{MaxResultsString}, \"page\":{pageNo}}}";
                var moreResult = await Conn.TryQuery(vnInfoMoreQuery, Resources.gyt_query_error);
                if (!moreResult) return;
                var vnMoreRoot = JsonConvert.DeserializeObject<ResultsRoot<VNItem>>(Conn.LastResponse.JsonPayload);
                List<VNItem> vnMoreItems = vnMoreRoot.Items;
                await Conn.GetMultipleVN(vnMoreItems.Select(x => x.ID).ToArray(), false);
                moreResults = vnMoreRoot.More;
            }
            var span = DateTime.UtcNow.ToLocalTime() - startTime;
            ReloadListsFromDb();
            LoadVNListToGui();
            WriteText(replyText,
                span < TimeSpan.FromMinutes(1)
                    ? $"Got VNs for {year} in <1 min. {Conn.TitlesAdded}/{Conn.TitlesAdded + Conn.TitlesSkipped} added."
                    : $"Got VNs for {year} in {span:hh\\:mm}. {Conn.TitlesAdded}/{Conn.TitlesAdded + Conn.TitlesSkipped} added.");
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
            var producerItem = LocalDatabase.ProducerList.Find(x => x.Name.Equals(producer, StringComparison.InvariantCultureIgnoreCase));
            if (producerItem == null)
            {
                var askBox2 = MessageBox.Show($@"A producer named {producer} was not found in local database.\nWould you like to search VNDB?", Resources.are_you_sure, MessageBoxButtons.YesNo);
                if (askBox2 != DialogResult.Yes) return;
                var result2 = Conn.StartQuery(replyText, "Update Producer Titles", false, false, false);
                if (!result2) return;
                var producers = await Conn.AddProducersBySearchedName(producer);
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
                    ReloadListsFromDb();
                    ChangeAPIStatus(Conn.Status);
                    return;
                }
                ChangeAPIStatus(Conn.Status);
                ReloadListsFromDb();
                producerItem = LocalDatabase.ProducerList.Find(x => x.Name.Equals(producer, StringComparison.InvariantCultureIgnoreCase));
                ListByTB.Text = producer;
            }
            var result = Conn.StartQuery(replyText, "Update Producer Titles", false, false, false);
            if (!result) return;
            await GetProducerTitles(producerItem, false);
            WriteText(replyText, $"Got new VNs for {producerItem.Name}, added {Conn.TitlesAdded} titles.");
            ReloadListsFromDb();
            LoadVNListToGui();
            ChangeAPIStatus(Conn.Status);
        }

        #endregion

        #region Listing

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
                    ListByCBQuery.Visible = false;
                    ListByUpdateButton.Enabled = true;
                    ListByGoButton.Enabled = true;
                    ListByTB.AutoCompleteMode = AutoCompleteMode.None;
                    break;
                case (int)ListBy.Producer:
                    ListByTB.Visible = true;
                    ListByCBQuery.Visible = false;
                    ListByUpdateButton.Enabled = true;
                    ListByGoButton.Enabled = true;
                    PopulateProducerSearchBox();
                    break;
                case (int)ListBy.Year:
                    ListByTB.Visible = true;
                    ListByCBQuery.Visible = false;
                    ListByUpdateButton.Enabled = true;
                    ListByGoButton.Enabled = true;
                    ListByTB.AutoCompleteMode = AutoCompleteMode.None;
                    break;
                case (int)ListBy.Group:
                    ListByTB.Visible = false;
                    ListByCBQuery.Visible = true;
                    ListByUpdateButton.Enabled = false;
                    ListByGoButton.Enabled = false;
                    PopulateGroupSearchBox();
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

        /// <summary>
        /// List by name or alias.
        /// </summary>
        private void List_Name()
        {
            if (ListByTB.Text == "") //check if box is empty
            {
                WriteError(replyText, Resources.enter_vn_title);
                return;
            }
            var searchString = ListByTB.Text.ToLowerInvariant();
            List_ClearOther(skipListBox: true);
            _currentList = vn =>
            vn.Title.ToLowerInvariant().Contains(searchString) ||
            vn.KanjiTitle.ToLowerInvariant().Contains(searchString) ||
            vn.Aliases.ToLowerInvariant().Contains(searchString);
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
                WriteError(replyText, Resources.enter_year);
                return;
            }
            if (!int.TryParse(ListByTB.Text, out int year)) //check if box has integer
            {
                WriteError(replyText, Resources.must_be_integer);
                return;
            }
            List_ClearOther(skipListBox: true);
            _currentList = vn => vn.ReleasedInYear(year);
            _currentListLabel = $"{year} (Year)";
            LoadVNListToGui();
        }

        /// <summary>
        /// Clear other listing options.
        /// </summary>
        private void List_ClearOther(bool skipListBox = false)
        {
            DontTriggerEvent = true;
            if (!skipListBox) ListByTB.Text = "";
            DontTriggerEvent = false;
        }

        /// <summary>
        /// Display VNs in user-defined group that is typed/selected in box.
        /// </summary>
        internal void List_Group(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex < 1) return;
            var groupName = ListByCBQuery.Text;
            if (groupName.Equals("(Group)")) return;
            List_ClearOther();
            _currentList = x => x.IsInGroup(groupName);
            _currentListLabel = $"{groupName} (Group)";
            LoadVNListToGui(skipComboSearch: true);
        }

        private void ListByCbEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (ListByCBQuery.Text.Equals("")) return;
            if (ListByCB.SelectedIndex == (int)ListBy.Group) List_Group(sender, e);
        }

        /// <summary>
        /// Display VNs by producer typed/selected in box.
        /// </summary>
        internal void List_Producer(string producerName = null)
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

        #region VN Context Menu

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
                WriteError(replyText, "Not Logged In");
                return;
            }
            var nitem = e.ClickedItem;
            if (nitem == null) return;
            bool success;
            if (!(tileOLV.SelectedObject is ListedVN vn)) return;
            var statusInt = -1;
            switch (nitem.OwnerItem.Text)
            {
                case "Userlist":
                    if (vn.ULStatus.ToString().Equals(nitem.Text))
                    {
                        WriteText(replyText, $"{TruncateString(vn.Title, 20)} already has that status.");
                        return;
                    }
                    statusInt = (int)(long)Enum.Parse(typeof(UserlistStatus), nitem.Text);
                    success = await ChangeVNStatus(vn, VNDatabase.ChangeType.UL, statusInt);
                    break;
                case "Wishlist":
                    if (vn.WLStatus.ToString().Equals(nitem.Text))
                    {
                        WriteText(replyText, $"{TruncateString(vn.Title, 20)} already has that status.");
                        return;
                    }
                    statusInt = (int)(long)Enum.Parse(typeof(WishlistStatus), nitem.Text);
                    success = await ChangeVNStatus(vn, VNDatabase.ChangeType.WL, statusInt);
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
                    if (Math.Abs(vn.Vote - newVoteValue) < 0.001)
                    {
                        WriteText(replyText, $"{TruncateString(vn.Title, 20)} already has that status.");
                        return;
                    }
                    success = await ChangeVNStatus(vn, VNDatabase.ChangeType.Vote, statusInt, newVoteValue);
                    break;
                default:
                    return;
            }
            if (!success) return;
            WriteText(replyText, $"{TruncateString(vn.Title, 20)} status changed.");
        }

        private void RightClickShowProducerTitles(object sender, EventArgs e)
        {
            if (!(tileOLV.SelectedObject is ListedVN vn)) return;
            List_Producer(vn.Producer);
        }

        private void RightClickAddProducer(object sender, EventArgs e)
        {
            if (!(tileOLV.SelectedObject is ListedVN vn)) return;
            ListedVN[] producerVNs = LocalDatabase.URTList.Where(x => x.Producer.Equals(vn.Producer)).ToArray();
            double userAverageVote = -1;
            double userDropRate = -1;
            if (producerVNs.Any())
            {
                var finishedCount = producerVNs.Count(x => x.ULStatus == UserlistStatus.Finished);
                var droppedCount = producerVNs.Count(x => x.ULStatus == UserlistStatus.Dropped);
                ListedVN[] producerVotedVNs = producerVNs.Where(x => x.Vote > 0).ToArray();
                userAverageVote = producerVotedVNs.Any() ? producerVotedVNs.Select(x => x.Vote).Average() : -1;
                userDropRate = finishedCount + droppedCount != 0
                    ? (double)droppedCount / (droppedCount + finishedCount)
                    : -1;
            }
            var producer = LocalDatabase.ProducerList.Find(x => x.Name == vn.Producer);
            var addProducerList = new List<ListedProducer>
            {
                new ListedProducer(vn.Producer, producerVNs.Length, DateTime.UtcNow,
                    producer.ID, producer.Language,
                    userAverageVote, (int) Math.Round(userDropRate*100))
            };
            LocalDatabase.BeginTransaction();
            LocalDatabase.InsertFavoriteProducers(addProducerList, Settings.UserID);
            LocalDatabase.EndTransaction();
            ReloadListsFromDb();
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
                WriteError(replyText, "Not Logged In");
                return;
            }
            if (!(tileOLV.SelectedObject is ListedVN vn)) return;
            VNItem.CustomItemNotes itemNotes = vn.GetCustomItemNotes();
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
                WriteError(replyText, "Not Logged In");
                return;
            }
            if (!(tileOLV.SelectedObject is ListedVN vn)) return;
            VNItem.CustomItemNotes itemNotes = vn.GetCustomItemNotes();
            var result = new ListDialogBox(itemNotes.Groups, "Add Title to Groups", $"{vn.Title} is in groups:").ShowDialog();
            if (result != DialogResult.OK) return;
            if (itemNotes.Groups.Any(group => group.Contains('\n')))
            {
                WriteError(replyText, "Group name cannot contain newline characters.");
                return;
            }
            await UpdateItemNotes("Added title to group(s).", vn.VNID, itemNotes);
        }

        #endregion

        #region List Results

        private void Help_ListResults(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            Debug.Assert(path != null, nameof(path) + " != null");
            var helpFile = $"{Path.Combine(path, "Program Data\\Help\\listresults.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

        /// <summary>
        /// Load Visual Novel Form with details of visual novel that was double clicked.
        /// </summary>
        private void VisualNovelDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.ClickCount < 2) return;
            if (ModifierKeys.HasFlag(Keys.Control)) return;
            var vnItem = (ListedVN)e.Model;
            var tabPage = new TabPage();
            VNControl vnf;
            if (Conn.ActiveQuery.Completed)
            {
                LocalDatabase.Open();
                vnItem = LocalDatabase.GetSingleVN(vnItem.VNID, Settings.UserID);
                LocalDatabase.Close();
                vnf = new VNControl(vnItem, this, tabPage, true);
            }
            else vnf = new VNControl(vnItem, this, tabPage, false);
            vnf.Dock = DockStyle.Fill;
            tabPage.Controls.Add(vnf);
            TabsControl.TabPages.Add(tabPage);
            //dont auto-switch to tab if holding alt
            if (ModifierKeys.HasFlag(Keys.Alt)) return;
            TabsControl.SelectTab(tabPage);
        }

        //format list rows, color according to userlist status, only for Details View
        private void FormatVNRow(object sender, FormatRowEventArgs e)
        {
            if (e.ListView.View != View.Details) return;
            var listedVN = (ListedVN)e.Model;
            //ULStatus takes priority over WLStatus
            var brush = GetBrushFromStatuses(listedVN);
            if (brush != null) e.Item.BackColor = brush.Color;
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(-1);
            e.Item.GetSubItem(tileColumnULAdded.Index).Text = listedVN.ULAdded != dateTimeOffset ? listedVN.ULAdded.ToShortDateString() : "";
            e.Item.GetSubItem(tileColumnWLAdded.Index).Text = listedVN.WLAdded != dateTimeOffset ? listedVN.WLAdded.ToShortDateString() : "";
            if (listedVN.ULStatus == UserlistStatus.None) e.Item.GetSubItem(tileColumnULS.Index).Text = "";
            if (listedVN.ULStatus == UserlistStatus.Playing) e.Item.GetSubItem(tileColumnULS.Index).ForeColor = ULPlayingBrush.Color;
            if (listedVN.WLStatus == WishlistStatus.None) e.Item.GetSubItem(tileColumnWLS.Index).Text = "";
            if (listedVN.Vote < 1) e.Item.GetSubItem(tileColumnVote.Index).Text = "";
            if (LocalDatabase.FavoriteProducerList.Any(x => x.Name.Equals(listedVN.Producer))) e.Item.GetSubItem(tileColumnProducer.Index).ForeColor = FavoriteProducerBrush.Color;
            e.Item.GetSubItem(tileColumnLength.Index).Text = listedVN.LengthString;
            e.Item.GetSubItem(tileColumnDate.Index).Text = listedVN.RelDate;
            e.Item.GetSubItem(tileColumnRating.Index).Text = listedVN.VoteCount > 0 ? $"{listedVN.Rating:0.00} ({listedVN.VoteCount} Votes)" : "";
            e.Item.GetSubItem(tileColumnPopularity.Index).Text = listedVN.Popularity > 0 ? listedVN.Popularity.ToString("0.00") : "";

        }

        /// <summary>
        /// Send query to VNDB to update a VN's notes and if successful, update database.
        /// </summary>
        /// <param name="replyMessage">Message to be printed if query is successful</param>
        /// <param name="vnid">ID of VN</param>
        /// <param name="itemNotes">Object containing new data to replace old</param>
        private async Task UpdateItemNotes(string replyMessage, int vnid, VNItem.CustomItemNotes itemNotes)
        {
            var result = Conn.StartQuery(replyText, "Update Item Notes", false, false, false);
            if (!result) return;
            string serializedNotes = itemNotes.Serialize();
            var query = $"set vnlist {vnid} {{\"notes\":\"{serializedNotes}\"}}";
            var apiResult = await Conn.TryQuery(query, "UIN Query Error");
            if (!apiResult) return;
            LocalDatabase.Open();
            LocalDatabase.AddNoteToVN(vnid, serializedNotes, Settings.UserID);
            LocalDatabase.Close();
            ReloadListsFromDb();
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
        private void OLV_ItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            if (tileOLV.Objects == null) return;
            var count = tileOLV.FilteredObjects.Cast<object>().Count();
            var totalcount = tileOLV.Objects.Cast<object>().Count();
            string itemCountString = count == totalcount ? $"{totalcount} items." : $"{count}/{totalcount} items.";
            resultLabel.Text = $@"List: {_currentListLabel} ({CurrentFilterLabel}) {itemCountString}";
        }

        /// <summary>
        /// Adjust tile size on OLV resize to avoid empty spaces.
        /// </summary>
        private void OLV_Resize(object sender, EventArgs e)
        {
            var width = tileOLV.Width - 24;
            var s = (int)Math.Round((double)width / 230);
            if (s == 0) return;
            tileOLV.TileSize = new Size(width / s, 300);
        }

        #endregion

        internal void SetVNList(Func<ListedVN, bool> function, string label)
        {
            tileOLV.ModelFilter = new ModelFilter(vn => function((ListedVN)vn));
            CurrentFilterLabel = label;
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            CustomFilter selectedItem = (CustomFilter)filterDropdown.SelectedItem;
            _filtersTab?.ChangeCustomFilter(this, selectedItem);
        }

        private void SetAllTitles(object sender, EventArgs e)
        {
            _currentList = x => true;
            _currentListLabel = "All Titles";
            LoadVNListToGui();
        }

        #region Classes and Enums
#pragma warning disable 1591
        /// <summary>
        /// Specifies ListBy mode.
        /// </summary>
        private enum ListBy { Name, Producer, Year, Group }
#pragma warning restore 1591
        #endregion

    }

}
