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
    public partial class ProducerSearchForm : Form
    {
        private readonly FormMain _parentForm;
        private readonly List<ListedProducer> _producerList;

        public ProducerSearchForm(FormMain parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
            prodSearchReply.Text = "";
            _parentForm.DBConn.Open();
            _producerList = _parentForm.DBConn.GetFavoriteProducersForUser(_parentForm.UserID);
            _parentForm.DBConn.Close();
        }

        private async void SearchProducersClick(object sender, EventArgs e)
        {
            await SearchProducers();
        }

        internal async Task SearchProducers()
        {
            if (producerSearchBox.Text == "") //check if box is empty
            {
                FormMain.WriteError(prodSearchReply, Resources.enter_producer_name, true);
                return;
            }
            var producerName = producerSearchBox.Text;
            string prodSearchQuery = $"get producer basic (search~\"{producerName}\") {{\"results\":25}}";
            var result = await _parentForm.TryQuery(prodSearchQuery, Resources.ps_query_error, prodSearchReply);
            if (!result) return;
            var prodRoot = JsonConvert.DeserializeObject<ProducersRoot>(_parentForm.Conn.LastResponse.JsonPayload);
            List<ProducerItem> prodItems = prodRoot.Items;
            var searchedProducers = new List<ListedSearchedProducer>();
            //alternative LINQ syntax (lol)
            //searchedProducers.AddRange(from item in prodMoreItems let inList = _producerList?.Find(item2 => item2.ID == item.ID) != null ? "No" : "Yes" select new ListedSearchedProducer(item.Name, inList, item.ID));
            foreach (var producer in prodItems)
            {
                var inList = _producerList.Find(item2 => item2.ID == producer.ID) != null ? "Yes" : "No";
                searchedProducers.Add(new ListedSearchedProducer(producer.Name, inList, producer.ID));
            }
            var moreResults = prodRoot.More;
            var pageNo = 1;
            while (moreResults)
            {
                pageNo++;
                string prodSearchMoreQuery =
                    $"get producer basic (search~\"{producerName}\") {{\"results\":25, \"page\":{pageNo}}}";
                var moreResult =
                    await _parentForm.TryQuery(prodSearchMoreQuery, Resources.ps_query_error, prodSearchReply);
                if (!moreResult) return;
                var prodMoreRoot =
                    JsonConvert.DeserializeObject<ProducersRoot>(_parentForm.Conn.LastResponse.JsonPayload);
                List<ProducerItem> prodMoreItems = prodMoreRoot.Items;
                foreach (var producer in prodMoreItems)
                {
                    var inList = _producerList.Find(item2 => item2.ID == producer.ID) != null ? "Yes" : "No";
                    searchedProducers.Add(new ListedSearchedProducer(producer.Name, inList, producer.ID));
                }
                moreResults = prodMoreRoot.More;
            }
            olProdSearch.SetObjects(searchedProducers);
            olProdSearch.Sort(olProdSearch.GetColumn(0), SortOrder.Ascending);
            prodSearchReply.Text = $"{searchedProducers.Count} producers found.";
        }

        private void AddProducerByDoubleClick(object sender, MouseEventArgs e)
        {
            AddProducersClick(null, null);
        }

        private async void SearchProducersEnterKey(object sender, KeyEventArgs e) //press enter on producer search
        {
            if (e.KeyCode == Keys.Enter)
            {
                await SearchProducers();
            }
        }

        private void AddProducersClick(object sender, EventArgs e)
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
                        ? (double) droppedCount/(droppedCount + finishedCount)
                        : -1;
                }
                addProducerList.Add(new ListedProducer(producer.Name, -1, "No", DateTime.UtcNow, producer.ID,
                    userAverageVote, (int) Math.Round(userDropRate*100)));
            }
            _parentForm.DBConn.Open();
            _parentForm.DBConn.InsertFavoriteProducers(addProducerList, _parentForm.UserID);
            _parentForm.DBConn.Close();
            _producerList.AddRange(addProducerList);
            prodSearchReply.Text = $"{addProducerList.Count} added.";
        }
    }
}