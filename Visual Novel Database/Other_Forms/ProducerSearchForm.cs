using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Happy_Search.Properties;
using Newtonsoft.Json;
using Happy_Apps_Core;
using static Happy_Apps_Core.StaticHelpers;

namespace Happy_Search.Other_Forms
{
    /// <summary>
    /// Form to search and add producers to user's favorite producers list.
    /// </summary>
    public partial class ProducerSearchForm : Form
    {
        private readonly FormMain _parentForm;

        /// <summary>
        /// Form to search and add producers to user's favorite producers list.
        /// </summary>
        public ProducerSearchForm(FormMain parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
            prodSearchReply.Text = "";
        }
        
        /// <summary>
        /// Show suggested favorite producers (producers not in list with over 2 finished titles).
        /// </summary>
        private void SuggestProducers(object sender, EventArgs e)
        {
            var suggestions = new List<ListedSearchedProducer>();
            foreach (var producer in LocalDatabase.ProducerList)
            {
                if (LocalDatabase.FavoriteProducerList.Find(x => x.Name.Equals(producer.Name)) != null) continue;
                int finishedTitles = LocalDatabase.URTList.Count(x => x.Producer == producer.Name && x.ULStatus == UserlistStatus.Finished);
                int urtTitles = LocalDatabase.URTList.Count(x => x.Producer == producer.Name);
                if (finishedTitles >= 2) suggestions.Add(new ListedSearchedProducer(producer.Name, "No", producer.ID, producer.Language, finishedTitles, urtTitles));
            }
            if (!suggestions.Any())
            {
                prodSearchReply.Text = @"No producers to be suggested.";
                return;
            }
            olProdSearch.SetObjects(suggestions);
            olProdSearch.Sort(olProdSearch.GetColumn(ol3Finished.Index), SortOrder.Descending);
            prodSearchReply.Text = $@"{suggestions.Count} suggested producers.";
        }

        /// <summary>
        /// Activates SearchProducers by button click.
        /// </summary>
        private async void SearchProducersClick(object sender, EventArgs e)
        {
            await SearchProducers();
        }
        
        /// <summary>
        /// Searches VNDB for producers by name.
        /// </summary>
        internal async Task SearchProducers()
        {
            if (producerSearchBox.Text.Equals("")) //check if box is empty
            {
                WriteError(prodSearchReply, Resources.enter_producer_name);
                return;
            }
            var result = Conn.StartQuery(prodSearchReply, "Producer Search",false,true,false);
            if (!result) return;
            var producerName = producerSearchBox.Text;
            string prodSearchQuery = $"get producer basic (search~\"{producerName}\") {{{MaxResultsString}}}";
            result = await Conn.TryQuery(prodSearchQuery, Resources.ps_query_error);
            if (!result) return;
            var prodRoot = JsonConvert.DeserializeObject<ResultsRoot<ProducerItem>>(Conn.LastResponse.JsonPayload);
            List<ProducerItem> prodItems = prodRoot.Items;
            List<ListedSearchedProducer> searchedProducers = prodItems.Select(NewListedSearchedProducer).ToList();
            var moreResults = prodRoot.More;
            var pageNo = 1;
            while (moreResults)
            {
                pageNo++;
                string prodSearchMoreQuery =
                    $"get producer basic (search~\"{producerName}\") {{{MaxResultsString}, \"page\":{pageNo}}}";
                var moreResult =
                    await Conn.TryQuery(prodSearchMoreQuery, Resources.ps_query_error);
                if (!moreResult) return;
                var prodMoreRoot =
                    JsonConvert.DeserializeObject<ResultsRoot<ProducerItem>>(Conn.LastResponse.JsonPayload);
                List<ProducerItem> prodMoreItems = prodMoreRoot.Items;
                searchedProducers.AddRange(prodMoreItems.Select(NewListedSearchedProducer));
                moreResults = prodMoreRoot.More;
            }
            olProdSearch.SetObjects(searchedProducers);
            olProdSearch.Sort(olProdSearch.GetColumn(0), SortOrder.Ascending);
            LocalDatabase.BeginTransaction();
            foreach (var producer in searchedProducers)
            {
                if (LocalDatabase.FavoriteProducerList.Find(x => x.Name.Equals(producer.Name)) != null) continue;
                LocalDatabase.InsertProducer((ListedProducer) producer, true);
            }
            LocalDatabase.EndTransaction();
            _parentForm.ChangeAPIStatus(Conn.Status);
            prodSearchReply.Text = $@"{searchedProducers.Count} producers found.";
        }
        
        /// <summary>
        /// Activates AddProducersButtonClick by double mouse click.
        /// </summary>
        private void AddProducerByDoubleClick(object sender, MouseEventArgs e)
        {
            AddProducersButtonClick(null, null);
        }
        
        /// <summary>
        /// Activates SearchProducers by enter key.
        /// </summary>
        private async void SearchProducersEnterKey(object sender, KeyEventArgs e) //press enter on producer search
        {
            if (e.KeyCode == Keys.Enter)
            {
                await SearchProducers();
            }
        }

        /// <summary>
        /// Adds selected producers to user's favorite producers list.
        /// </summary>
        private void AddProducersButtonClick(object sender, EventArgs e)
        {
            if (olProdSearch.SelectedObjects.Count == 0)
            {
                prodSearchReply.ForeColor = Color.Red;
                prodSearchReply.Text = Resources.no_items_selected;
                FadeLabel(prodSearchReply);
                return;
            }
            var addProducerList = new List<ListedProducer>();
            var i = -1;
            foreach (ListedSearchedProducer producer in olProdSearch.SelectedObjects)
            {
                i++;
                if (producer.InList.Equals("Yes")) continue;
                olProdSearch.SelectedItems[i].SubItems[1].Text = @"Yes";
                producer.InList = @"Yes";
                ListedVN[] producerVNs = LocalDatabase.URTList.Where(x => x.Producer.Equals(producer.Name)).ToArray();
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
                addProducerList.Add(new ListedProducer(producer.Name, -1, DateTime.UtcNow, producer.ID,
                    producer.Language, userAverageVote, (int)Math.Round(userDropRate * 100)));
            }
            LocalDatabase.BeginTransaction();
            LocalDatabase.InsertFavoriteProducers(addProducerList, Settings.UserID);
            LocalDatabase.EndTransaction();
            LocalDatabase.FavoriteProducerList.AddRange(addProducerList);
            prodSearchReply.Text = $@"{addProducerList.Count} added.";
        }


        private ListedSearchedProducer NewListedSearchedProducer(ProducerItem producer)
        {
            string inList = LocalDatabase.FavoriteProducerList.Find(x => x.Name.Equals(producer.Name)) != null ? "Yes" : "No";
            int finished = LocalDatabase.URTList.Count(x => x.Producer == producer.Name && x.ULStatus == UserlistStatus.Finished);
            int urtTitles = LocalDatabase.URTList.Count(x => x.Producer == producer.Name);
            return new ListedSearchedProducer(producer.Name, inList, producer.ID, producer.Language, finished, urtTitles);
        }
    }
}