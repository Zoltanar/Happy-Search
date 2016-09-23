using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Happy_Search.Properties;
using Newtonsoft.Json;

namespace Happy_Search
{
    partial class FormMain
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
                    var finishedCount = producerVNs.Count(x => x.ULStatus.Equals("Finished"));
                    var droppedCount = producerVNs.Count(x => x.ULStatus.Equals("Dropped"));
                    ListedVN[] producerVotedVNs = producerVNs.Where(x => x.Vote > 0).ToArray();
                    userDropRate = finishedCount + droppedCount != 0
                        ? (double)droppedCount / (droppedCount + finishedCount)
                        : -1;
                    userAverageVote = producerVotedVNs.Any() ? producerVotedVNs.Select(x => x.Vote).Average() : -1;
                }
                favoriteProducerList.Add(new ListedProducer(producer.Name, producer.NumberOfTitles, producer.Loaded,
                    DateTime.UtcNow, producer.ID, userAverageVote, (int)Math.Round(userDropRate * 100)));
            }
            DBConn.Open();
            DBConn.InsertFavoriteProducers(favoriteProducerList, UserID);
            DBConn.Close();
        }

        /// <summary>
        /// Bring up dialog explaining features of the 'Favorite Producers' section.
        /// </summary>
        private void Help_FavoriteProducers(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            if (path == null)
            {
                WriteError(prodReply, @"Unknown Path Error");
                return;
            }
            var helpFile = $"{Path.Combine(path, "Program Data\\Help\\favoriteproducers.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

        /// <summary>
        /// Bring up Form to search/add producers into Favorite Producers list.
        /// </summary>
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

        /// <summary>
        /// Remove producer(s) from Favorite Producers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Get new and refresh old titles from Favorite Producers.
        /// </summary>
        private async void RefreshAllFavoriteProducerTitles(object sender, EventArgs e)
        {
            if (olFavoriteProducers.Items.Count == 0)
            {
                WriteError(prodReply, "No Items in list.", true);
                return;
            }
            var vnCount = olFavoriteProducers.Objects.Cast<ListedProducer>().Sum(producer => producer.NumberOfTitles);
            var askBox =
                MessageBox.Show($@"Are you sure you wish to reload {vnCount}+ VNs?
This may take a while...",
                    Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            ReloadLists();
            _vnsAdded = 0;
            _vnsSkipped = 0;
            foreach (ListedProducer producer in olFavoriteProducers.Objects)
                await GetProducerTitles(producer, prodReply, true);
            LoadFavoriteProducerList();
            WriteText(prodReply, Resources.update_fp_titles_success + $" ({_vnsAdded} new titles)");
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
                WriteError(prodReply, Resources.no_items_selected, true);
                return;
            }
            IEnumerable<string> prodList = from ListedProducer producer in olFavoriteProducers.SelectedObjects
                                           select producer.Name;
            List_ClearOther();
            _currentList = vn => prodList.Contains(vn.Producer);
            _currentListLabel = "Favorite Producers (Selected)";
            RefreshVNList();
        }

        /// <summary>
        /// Fetch titles for producers whose titles haven't been fetched yet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LoadUnloaded(object sender, EventArgs e)
        {
            if (olFavoriteProducers.Items.Count == 0)
            {
                WriteError(prodReply, "No Items in list.", true);
                return;
            }
            ReloadLists();
            List<ListedProducer> producers =
                olFavoriteProducers.Objects.Cast<ListedProducer>().Where(item => item.Loaded.Equals("No")).ToList();
            foreach (var producer in producers)
            {
                await GetProducerTitles(producer, prodReply);
            }
            ReloadLists();
            RefreshVNList();
            LoadFavoriteProducerList();
            WriteText(prodReply, $"Loaded {producers.Count} producers.");
        }

        /// <summary>
        /// Get new titles from Favorite Producers, only updates if last update was over 2 days ago.
        /// </summary>
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
            ReloadLists();
            List<ListedProducer> producers =
                olFavoriteProducers.Objects.Cast<ListedProducer>().Where(item => item.Updated > 2).ToList();
            LogToFile($"{producers.Count} to be updated");
            _vnsAdded = 0;
            _vnsSkipped = 0;
            foreach (var producer in producers) await GetProducerTitles(producer, prodReply);
            ReloadLists();
            RefreshVNList();
            LoadFavoriteProducerList();
            WriteText(prodReply, $"Got {_vnsAdded} new titles by Favorite Producers.", true);
        }

        /// <summary>
        /// Bring up form with suggestions for favorite producers (producers not in list with over 2 finished titles.
        /// </summary>
        private void SuggestProducers(object sender, EventArgs e)
        {
            var suggestions = new Dictionary<ListedSearchedProducer, int>();
            foreach (var producer in _producerList)
            {
                var listedProducers = olFavoriteProducers.Objects as List<ListedProducer>;
                if (listedProducers?.Find(x => x.Name.Equals(producer.Name)) != null) continue;
                int finishedTitles = URTList.Count(x => x.Producer == producer.Name && x.ULStatus.Equals("Finished"));
                int urtTitles = URTList.Count(x => x.Producer == producer.Name);
                if (finishedTitles > 2) suggestions.Add(new ListedSearchedProducer(producer.Name, "No", producer.ID, finishedTitles, urtTitles), finishedTitles);
            }
            LogToFile("Finished adding suggestions");
            IOrderedEnumerable<KeyValuePair<ListedSearchedProducer, int>> sortedSuggestions = from entry in suggestions orderby entry.Value descending select entry;
            var listForForm = new List<ListedSearchedProducer>();
            foreach (KeyValuePair<ListedSearchedProducer, int> suggestion in sortedSuggestions)
            {
                LogToFile($"{suggestion.Key.Name} = Finished {suggestion.Value} titles.");
                listForForm.Add(suggestion.Key);
            }

            new ProducerSearchForm(this, listForForm).ShowDialog();
            LoadFavoriteProducerList();
        }

        /// <summary>
        /// Get titles developed/published by producer.
        /// </summary>
        /// <param name="producer">Producer whose titles should be found</param>
        /// <param name="replyLabel">Label that should receive reply</param>
        /// <param name="refreshAll">Should already known titles be refreshed as well?</param>
        private async Task GetProducerTitles(ListedProducer producer, Label replyLabel, bool refreshAll = false)
        {
            LogToFile($"Getting Titles for Producer {producer.Name}");
            string prodReleaseQuery = $"get release vn (producer={producer.ID}) {{{MaxResultsString}}}";
            var result = await TryQuery(prodReleaseQuery, Resources.upt_query_error, replyLabel, true, ignoreDateLimit: true);
            if (!result) return;
            var releaseRoot = JsonConvert.DeserializeObject<ReleasesRoot>(Conn.LastResponse.JsonPayload);
            List<ReleaseItem> releaseItems = releaseRoot.Items;
            List<int> producerVNList = releaseItems.SelectMany(item => item.VN.Select(x=>x.ID)).ToList();
            /* var producerVNList = new List<int>();
            foreach (var item in releaseItems)
            {
                producerVNList.AddRange(item.VN.Select(x=>x.ID));
            }*/
            var moreResults = releaseRoot.More;
            var pageNo = 1;
            while (moreResults)
            {
                pageNo++;
                string prodReleaseMoreQuery =
                    $"get release vn (producer={producer.ID}) {{{MaxResultsString}, \"page\":{pageNo}}}";
                var moreResult = await TryQuery(prodReleaseMoreQuery, Resources.upt_query_error, replyLabel, true, ignoreDateLimit: true);
                if (!moreResult) return;
                releaseRoot = JsonConvert.DeserializeObject<ReleasesRoot>(Conn.LastResponse.JsonPayload);
                releaseItems = releaseRoot.Items;
                producerVNList.AddRange(releaseItems.SelectMany(item => item.VN.Select(x => x.ID)));
                moreResults = releaseRoot.More;
            }
            await GetMultipleVN(producerVNList.Distinct(), replyLabel, true, refreshAll);
            DBConn.Open();
            List<ListedVN> producerTitles = DBConn.GetTitlesFromProducerID(UserID, producer.ID);
            DBConn.InsertProducer(new ListedProducer(producer.Name, producerTitles.Count, "Yes", DateTime.UtcNow,
                producer.ID));
            DBConn.Close();
            LogToFile($"Finished getting titles for Producer= {producer}, {producerTitles.Count} titles.");
        }

        /// <summary>
        /// Load Favorite Producers into ObjectListView.
        /// </summary>
        private void LoadFavoriteProducerList()
        {
            if (UserID < 1) return;
            DBConn.Open();
            List<ListedProducer> favoriteProducers = DBConn.GetFavoriteProducersForUser(UserID);
            DBConn.Close();
            foreach (var favoriteProducer in favoriteProducers)
            {
                double[] vnsWithVotes =_vnList.Where(x => x.Producer.Equals(favoriteProducer.Name) && x.Rating > 0).Select(x => x.Rating).ToArray();
                favoriteProducer.GeneralRating = vnsWithVotes.Any() ? Math.Round(vnsWithVotes.Average(), 2) : -1;
            }
            olFavoriteProducers.SetObjects(favoriteProducers);
            olFavoriteProducers.Sort(0);
        }

        private void UpdateFavoriteProducerForURTChange(string producerName)
        {
            var favoriteProducers = olFavoriteProducers.Objects as List<ListedProducer>;
            if (favoriteProducers?.Find(x => x.Name.Equals(producerName)) == null)
            {
                ReloadLists();
                RefreshVNList();
                return;
            }
                ReloadLists();
            var producer = _producerList.Find(x => x.Name == producerName);
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
            producer = new ListedProducer(producer.Name, producer.NumberOfTitles, producer.Loaded, DateTime.UtcNow, producer.ID, userAverageVote, (int)Math.Round(userDropRate * 100));
            DBConn.Open();
            DBConn.InsertFavoriteProducers(new List<ListedProducer> { producer }, UserID);
            DBConn.Close();
            UpdateUserStats();
            ReloadLists();
            RefreshVNList();
            LoadFavoriteProducerList();
        }

        /// <summary>
        /// Format row in Favorite Producers OLV.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatRowFavoriteProducers(object sender, FormatRowEventArgs e)
        {
            var listedProducer = (ListedProducer)e.Model;
            e.Item.GetSubItem(ol2UserAverageVote.Index).Text = listedProducer.UserAverageVote > 0 ? listedProducer.UserAverageVote.ToString("0.00") : "";
            e.Item.GetSubItem(ol2UserDropRate.Index).Text = listedProducer.UserDropRate < 0 ? "" : $"{listedProducer.UserDropRate}%";
            if (listedProducer.NumberOfTitles < 0) e.Item.GetSubItem(ol2ItemCount.Index).Text = "";
        }

    }
}
