using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Happy_Search.Properties;
using Happy_Search.Other_Forms;
using Newtonsoft.Json;
using static Happy_Search.StaticHelpers;

namespace Happy_Search
{
    public partial class FormMain
    {
        /// <summary>
        /// Sets favorite producer data from vn statistics.
        /// Saves data back to database.
        /// </summary>
        private void SetFavoriteProducersData()
        {
            var favoriteProducerList = new List<ListedProducer>();
            foreach (ListedProducer producer in olFavoriteProducers.Objects)
            {
                ListedVN[] producerVNs = URTList.Where(x => x.Producer.Equals(producer.Name)).ToArray();
                double userDropRate = -1;
                double userAverageVote = -1;
                if (producerVNs.Any())
                {
                    var finishedCount = producerVNs.Count(x => x.ULStatus == UserlistStatus.Finished);
                    var droppedCount = producerVNs.Count(x => x.ULStatus == UserlistStatus.Dropped);
                    ListedVN[] producerVotedVNs = producerVNs.Where(x => x.Vote > 0).ToArray();
                    userDropRate = finishedCount + droppedCount != 0
                        ? (double)droppedCount / (droppedCount + finishedCount)
                        : -1;
                    userAverageVote = producerVotedVNs.Any() ? producerVotedVNs.Select(x => x.Vote).Average() : -1;
                }
                favoriteProducerList.Add(new ListedProducer(producer.Name, producer.NumberOfTitles,
                    DateTime.UtcNow, producer.ID, producer.Language, userAverageVote, (int)Math.Round(userDropRate * 100)));
            }
            DBConn.BeginTransaction();
            DBConn.InsertFavoriteProducers(favoriteProducerList, Settings.UserID);
            DBConn.EndTransaction();
        }

        /// <summary>
        /// Bring up dialog explaining features of the 'Favorite Producers' section.
        /// </summary>
        private void Help_FavoriteProducers(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            Debug.Assert(path != null, "path != null");
            var helpFile = $"{Path.Combine(path, "Program Data\\Help\\favoriteproducers.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

        /// <summary>
        /// Bring up Form to search/add producers into Favorite Producers list.
        /// </summary>
        private void AddProducers(object sender, EventArgs e)
        {
            if (Settings.UserID < 1)
            {
                WriteText(prodReply, Resources.set_userid_first);
                return;
            }
            new ProducerSearchForm(this).ShowDialog();
            LoadFPListToGui();
        }

        /// <summary>
        /// Remove producer(s) from Favorite Producers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveProducers(object sender, EventArgs e)
        {
            if (olFavoriteProducers.SelectedObjects.Count == 0)
            {
                WriteError(prodReply, Resources.no_items_selected);
                return;
            }
            DBConn.BeginTransaction();
            foreach (ListedProducer item in olFavoriteProducers.SelectedObjects)
            {
                DBConn.RemoveFavoriteProducer(item.ID, Settings.UserID);
            }
            DBConn.EndTransaction();
            LoadFPListToGui();
        }

        /// <summary>
        /// Get new and refresh old titles from Favorite Producers.
        /// </summary>
        private async void RefreshAllFavoriteProducerTitles(object sender, EventArgs e)
        {
            if (olFavoriteProducers.Items.Count == 0)
            {
                WriteError(prodReply, "No Items in list.");
                return;
            }
            var vnCount = olFavoriteProducers.Objects.Cast<ListedProducer>().Sum(producer => producer.NumberOfTitles);
            var askBox =
                MessageBox.Show($@"Are you sure you wish to reload {vnCount}+ VNs?
This may take a while...",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            var result = StartQuery(prodReply, "Refresh Favorite Producer Titles", true, true, true);
            if (!result) return;
            foreach (ListedProducer producer in olFavoriteProducers.Objects) await GetProducerTitles(producer,true);
            await ReloadListsFromDbAsync();
            LoadFPListToGui();
            WriteText(prodReply, Resources.update_fp_titles_success + $" ({TitlesAdded} new titles)");
            ChangeAPIStatus(Conn.Status);
        }

        /// <summary>
        /// Display titles by producers selected in Favorite Producers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowSelectedProducerVNs(object sender, EventArgs e)
        {
            if (olFavoriteProducers.SelectedItems.Count == 0)
            {
                WriteError(prodReply, Resources.no_items_selected);
                return;
            }
            IEnumerable<string> prodList = from ListedProducer producer in olFavoriteProducers.SelectedObjects
                                           select producer.Name;
            List_ClearOther();
            _currentList = vn => prodList.Contains(vn.Producer);
            _currentListLabel = "Favorite Producers (Selected)";
            LoadVNListToGui();
        }

        /// <summary>
        /// Get new titles from Favorite Producers, only updates if last update was over 2 days ago.
        /// </summary>
        private async void GetNewFavoriteProducerTitles(object sender, EventArgs e)
        {
            if (olFavoriteProducers.Items.Count == 0)
            {
                WriteWarning(prodReply, "You have no favorite producers.");
                return;
            }
            var askBox =
                MessageBox.Show(Resources.get_new_fp_titles_confirm, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            List<ListedProducer> producers =
                olFavoriteProducers.Objects.Cast<ListedProducer>().Where(item => item.Updated > 2 || item.Updated == -1).ToList();
            if (producers.Count == 0)
            {
                WriteWarning(prodReply, "No producers require an update.");
                return;
            }
            var result = StartQuery(prodReply, "Get New FP Titles",true,true,true);
            if (!result) return;
            LogToFile($"{producers.Count} to be updated");
            foreach (var producer in producers) await GetProducerTitles(producer,false);
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            LoadFPListToGui();
            WriteText(prodReply, $"Got {TitlesAdded} new titles by Favorite Producers.");
            ChangeAPIStatus(Conn.Status);
        }


        /// <summary>
        /// Get titles developed/published by producer.
        /// </summary>
        /// <param name="producer">Producer whose titles should be found</param>
        /// <param name="updateAll">Should already known fetched titles be updated as well?</param>
        private async Task GetProducerTitles(ListedProducer producer, bool updateAll)
        {
            LogToFile($"Getting Titles for Producer {producer.Name}");
            string prodReleaseQuery = $"get release vn (producer={producer.ID}) {{{MaxResultsString}}}";
            var result = await TryQuery(prodReleaseQuery, Resources.upt_query_error);
            if (!result) return;
            var releaseRoot = JsonConvert.DeserializeObject<ReleasesRoot>(Conn.LastResponse.JsonPayload);
            List<ReleaseItem> releaseItems = releaseRoot.Items;
            List<int> producerVNList = releaseItems.SelectMany(item => item.VN.Select(x => x.ID)).ToList();
            var moreResults = releaseRoot.More;
            var pageNo = 1;
            while (moreResults)
            {
                pageNo++;
                string prodReleaseMoreQuery =
                    $"get release vn (producer={producer.ID}) {{{MaxResultsString}, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(prodReleaseMoreQuery, Resources.upt_query_error);
                if (!moreResult) return;
                releaseRoot = JsonConvert.DeserializeObject<ReleasesRoot>(Conn.LastResponse.JsonPayload);
                releaseItems = releaseRoot.Items;
                producerVNList.AddRange(releaseItems.SelectMany(item => item.VN.Select(x => x.ID)));
                moreResults = releaseRoot.More;
            }
            await GetMultipleVN(producerVNList.Distinct(), updateAll);
            DBConn.Open();
            List<ListedVN> producerTitles = DBConn.GetTitlesFromProducerID(Settings.UserID, producer.ID);
            DBConn.InsertProducer(new ListedProducer(producer.Name, producerTitles.Count, DateTime.UtcNow,
                producer.ID, producer.Language));
            DBConn.Close();
            LogToFile($"Finished getting titles for Producer= {producer}, {producerTitles.Count} titles.");
        }

        /// <summary>
        /// Load Favorite Producers into ObjectListView.
        /// Gets data from local database.
        /// </summary>
        internal void LoadFPListToGui()
        {
            if (Settings.UserID < 1) return;
            DBConn.Open();
            FavoriteProducerList = DBConn.GetFavoriteProducersForUser(Settings.UserID);
            DBConn.Close();
            foreach (var favoriteProducer in FavoriteProducerList)
            {
                double[] vnsWithVotes = VNList.Where(x => x.Producer.Equals(favoriteProducer.Name) && x.Rating > 0).Select(x => x.Rating).ToArray();
                favoriteProducer.GeneralRating = vnsWithVotes.Any() ? Math.Round(vnsWithVotes.Average(), 2) : -1;
            }
            olFavoriteProducers.SetObjects(FavoriteProducerList);
            olFavoriteProducers.Sort(ol2Name.Index);
        }

        /// <summary>
        /// Update Favorite Producer stats for changes in URT list.
        /// </summary>
        /// <param name="producerName">Name of producer to be updated</param>
        private async Task UpdateFavoriteProducerForURTChange(string producerName)
        {
            var favoriteProducers = olFavoriteProducers.Objects as List<ListedProducer>;
            if (favoriteProducers?.Find(x => x.Name.Equals(producerName)) == null)
            {
                await ReloadListsFromDbAsync();
                LoadVNListToGui();
                return;
            }
            var producer = ProducerList.Find(x => x.Name == producerName);
            ListedVN[] producerVNs = URTList.Where(x => x.Producer.Equals(producer.Name)).ToArray();
            double userDropRate = -1;
            double userAverageVote = -1;
            if (producerVNs.Any())
            {
                var finishedCount = producerVNs.Count(x => x.ULStatus == UserlistStatus.Finished);
                var droppedCount = producerVNs.Count(x => x.ULStatus == UserlistStatus.Dropped);
                ListedVN[] producerVotedVNs = producerVNs.Where(x => x.Vote > 0).ToArray();
                userDropRate = finishedCount + droppedCount != 0 ? (double)droppedCount / (droppedCount + finishedCount) : -1;
                userAverageVote = producerVotedVNs.Any() ? producerVotedVNs.Select(x => x.Vote).Average() : -1;
            }
            producer = new ListedProducer(producer.Name, producer.NumberOfTitles, DateTime.UtcNow, producer.ID, producer.Language, userAverageVote, (int)Math.Round(userDropRate * 100));
            DBConn.BeginTransaction();
            DBConn.InsertFavoriteProducers(new List<ListedProducer> { producer }, Settings.UserID);
            DBConn.EndTransaction();
            UpdateUserStats();
            await ReloadListsFromDbAsync();
            LoadVNListToGui();
            LoadFPListToGui();
        }

        /// <summary>
        /// Format row in Favorite Producers OLV.
        /// </summary>
        private void FormatRowFavoriteProducers(object sender, FormatRowEventArgs e)
        {
            var listedProducer = (ListedProducer)e.Model;
            e.Item.GetSubItem(ol2UserAverageVote.Index).Text = listedProducer.UserAverageVote > 0 ? listedProducer.UserAverageVote.ToString("0.00") : "";
            e.Item.GetSubItem(ol2UserDropRate.Index).Text = listedProducer.UserDropRate > -1 ? $"{listedProducer.UserDropRate}%" : "";
            if (listedProducer.NumberOfTitles < 0) e.Item.GetSubItem(ol2ItemCount.Index).Text = "";
            if (listedProducer.GeneralRating < 0) e.Item.GetSubItem(ol2GeneralRating.Index).Text = "";
            if (listedProducer.Updated == -1)
            {
                e.Item.GetSubItem(ol2Updated.Index).Text = @"Never";
                e.Item.GetSubItem(ol2GeneralRating.Index).Text = @"Never";
                if (listedProducer.NumberOfTitles < 0) e.Item.GetSubItem(ol2ItemCount.Index).Text = "";
            }
        }

        private void FavoriteProducerDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.ClickCount < 2) return;
            var producer = (ListedProducer)e.Model;
            List_Producer(producer.Name);
        }
    }
}
