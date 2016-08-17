using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Happy_Search.Properties;
using JR.Utils.GUI.Forms;
using Newtonsoft.Json;

namespace Happy_Search
{
    partial class FormMain
    {
        /// <summary>
        /// Bring up dialog explaining features of the 'Favorite Producers' section.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_FavoriteProducers(object sender, EventArgs e)
        {
            var fpHelpFile = $"{Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),"help\\favoriteproducers.html")}";

        new HtmlForm($"file:///{fpHelpFile}").Show();
        }

        /// <summary>
        /// Load Favorite Producers into ObjectListView.
        /// </summary>
        private void LoadFavoriteProducerList()
        {
            if (UserID < 1) return;
            DBConn.Open();
            olFavoriteProducers.SetObjects(DBConn.GetFavoriteProducersForUser(UserID));
            DBConn.Close();
            olFavoriteProducers.Sort(0);
        }

        /// <summary>
        /// Bring up Form to search/add producers into Favorite Producers list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// Get titles developed/published by producer.
        /// </summary>
        /// <param name="producer">Producer whose titles should be found</param>
        /// <param name="replyLabel">Label that should receive reply</param>
        /// <param name="updateAll">Should already known titles be updated as well?</param>
        /// <returns></returns>
        private async Task GetProducerTitles(ListedProducer producer, Label replyLabel, bool updateAll = false)
        {
            int[] vnIDList = _vnList.Select(x => x.VNID).ToArray();
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
                if (vnIDList.Contains(vnid)) _vnsSkipped++;
                else _vnsAdded++;
                if (!updateAll && vnIDList.Contains(vnid)) continue;
                await UpdateProducerVN(vnid, producer.ID);
            }
            DBConn.Open();
            DBConn.InsertProducer(new ListedProducer(producer.Name, producerIDList.Count, "Yes", DateTime.UtcNow,
                producer.ID));
            DBConn.Close();
            Debug.Print($"Finished getting titles for ProducerID= {producer}");
        }

        /// <summary>
        /// Update a producer's title.
        /// </summary>
        /// <param name="vnid">ID of title</param>
        /// <param name="producerid">ID of producer</param>
        /// <returns></returns>
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
        /// Get new and update old titles from Favorite Producers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            _vnsAdded = 0;
            _vnsSkipped = 0;
            foreach (ListedProducer producer in olFavoriteProducers.Objects)
                await GetProducerTitles(producer, prodReply, true);
            SetOLV();
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
            _currentList = vn => prodList.Contains(vn.Producer);
            RefreshList();
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
            List<ListedProducer> producers =
                olFavoriteProducers.Objects.Cast<ListedProducer>().Where(item => item.Loaded.Equals("No")).ToList();
            foreach (var producer in producers)
            {
                await GetProducerTitles(producer, prodReply);
            }
            SetOLV();
            WriteText(prodReply, $"Loaded {producers.Count} producers.");
        }

        /// <summary>
        /// Get new titles from Favorite Producers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Format row in Favorite Producers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatRowFavoriteProducers(object sender, FormatRowEventArgs e)
        {
            var listedProducer = (ListedProducer)e.Model;
            if (listedProducer.UserAverageVote < 1) e.Item.GetSubItem(2).Text = "";
            if (listedProducer.UserDropRate < 0) e.Item.GetSubItem(3).Text = "";
            if (listedProducer.NumberOfTitles == -1) e.Item.GetSubItem(1).Text = "";
        }
        
    }
}
