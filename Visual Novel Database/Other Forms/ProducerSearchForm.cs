using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Happy_Search.Properties;
using Newtonsoft.Json;

namespace Happy_Search
{
    /// <summary>
    /// Form to search and add producers to user's favorite producers list.
    /// </summary>
    public partial class ProducerSearchForm : Form
    {
        private readonly FormMain _parentForm;
        private readonly List<ListedProducer> _producerList;

        /// <summary>
        /// Form to search and add producers to user's favorite producers list.
        /// </summary>
        public ProducerSearchForm(FormMain parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
            prodSearchReply.Text = "";
            _parentForm.DBConn.Open();
            _producerList = _parentForm.DBConn.GetFavoriteProducersForUser(_parentForm.UserID);
            _parentForm.DBConn.Close();
        }

        /// <summary>
        /// Load ProducerSearchForm already populated by producers.
        /// </summary>
        /// <param name="parentForm">Parent Form</param>
        /// <param name="producers">List of producers to be shown in OLV.</param>
        public ProducerSearchForm(FormMain parentForm, List<ListedSearchedProducer> producers)
        {
            InitializeComponent();
            _parentForm = parentForm;
            prodSearchReply.Text = "";
            olProdSearch.SetObjects(producers);
            _parentForm.DBConn.Open();
            _producerList = _parentForm.DBConn.GetFavoriteProducersForUser(_parentForm.UserID);
            _parentForm.DBConn.Close();
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
            if (producerSearchBox.Text == "") //check if box is empty
            {
                FormMain.WriteError(prodSearchReply, Resources.enter_producer_name, true);
                return;
            }
            var producerName = producerSearchBox.Text;
            string prodSearchQuery = $"get producer basic (search~\"{producerName}\") {{{FormMain.MaxResultsString}}}";
            var result = await _parentForm.TryQuery(prodSearchQuery, Resources.ps_query_error, prodSearchReply);
            if (!result) return;
            var prodRoot = JsonConvert.DeserializeObject<ProducersRoot>(_parentForm.Conn.LastResponse.JsonPayload);
            List<ProducerItem> prodItems = prodRoot.Items;
            List<ListedSearchedProducer> searchedProducers = prodItems.Select(NewListedSearchedProducer).ToList();
            var moreResults = prodRoot.More;
            var pageNo = 1;
            while (moreResults)
            {
                pageNo++;
                string prodSearchMoreQuery =
                    $"get producer basic (search~\"{producerName}\") {{{FormMain.MaxResultsString}, \"page\":{pageNo}}}";
                var moreResult =
                    await _parentForm.TryQuery(prodSearchMoreQuery, Resources.ps_query_error, prodSearchReply);
                if (!moreResult) return;
                var prodMoreRoot =
                    JsonConvert.DeserializeObject<ProducersRoot>(_parentForm.Conn.LastResponse.JsonPayload);
                List<ProducerItem> prodMoreItems = prodMoreRoot.Items;
                searchedProducers.AddRange(prodMoreItems.Select(NewListedSearchedProducer));
                moreResults = prodMoreRoot.More;
            }
            olProdSearch.SetObjects(searchedProducers);
            olProdSearch.Sort(olProdSearch.GetColumn(0), SortOrder.Ascending);
            _parentForm.DBConn.Open();
            foreach (var producer in searchedProducers)
            {
                if (_producerList.Find(x => x.Name.Equals(producer.Name)) != null) continue;
                _parentForm.DBConn.InsertProducer((ListedProducer)producer);
            }
            _parentForm.DBConn.Close();
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
                FormMain.FadeLabel(prodSearchReply);
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
                ListedVN[] producerVNs = _parentForm.URTList.Where(x => x.Producer.Equals(producer.Name)).ToArray();
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
                addProducerList.Add(new ListedProducer(producer.Name, -1, "No", DateTime.UtcNow, producer.ID,
                    userAverageVote, (int)Math.Round(userDropRate * 100)));
            }
            _parentForm.DBConn.Open();
            _parentForm.DBConn.InsertFavoriteProducers(addProducerList, _parentForm.UserID);
            _parentForm.DBConn.Close();
            _producerList.AddRange(addProducerList);
            prodSearchReply.Text = $@"{addProducerList.Count} added.";
        }


        private ListedSearchedProducer NewListedSearchedProducer(ProducerItem producer)
        {
            string inList = _producerList.Find(x => x.Name.Equals(producer.Name)) != null ? "Yes" : "No";
            int finished = _parentForm.URTList.Count(x => x.Producer == producer.Name && x.ULStatus.Equals("Finished"));
            int urtTitles = _parentForm.URTList.Count(x => x.Producer == producer.Name);
            return new ListedSearchedProducer(producer.Name, inList, producer.ID, finished, urtTitles);
        }
    }
}