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

namespace Happy_Search
{
    public partial class FormMain
    {
        private void HandleAdvancedMode(string query)
        {
            if (!AdvancedMode) return;
            if (serverR.TextLength > 10000) ClearLog(null, null);
            serverQ.Invoke(new MethodInvoker(() => serverQ.Text += query + Environment.NewLine));
            serverR.Invoke(new MethodInvoker(() => serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine));
        }

        private void RefreshListAction()
        {
            ReloadListsFromDb();
            FiltersTab?.PopulateLanguages(false);
            LoadVNListToGui();
        }

        /// <summary>
        /// Change text and color of API Status label based on status.
        /// </summary>
        /// <param name="apiStatus">Status of API Connection</param>
        internal void ChangeAPIStatus(VndbConnection.APIStatus apiStatus)
        {
            var loginString = TruncateString(LoginString, 28) + Environment.NewLine;
            switch (apiStatus)
            {
                case VndbConnection.APIStatus.Ready:
                    if (Environment.GetCommandLineArgs().Contains("-debug")) LogToFile($"{Conn.ActiveQuery.ActionName} Ended");
                    Conn.ActiveQuery.Completed = true;
                    statusLabel.Text = loginString + @"Ready";
                    statusLabel.ForeColor = Color.Black;
                    statusLabel.BackColor = Color.LightGreen;
                    break;
                case VndbConnection.APIStatus.Busy:
                    if (Environment.GetCommandLineArgs().Contains("-debug")) LogToFile($"{Conn.ActiveQuery.ActionName} Started");
                    statusLabel.Text = loginString + $@"Busy ({Conn.ActiveQuery.ActionName})";
                    statusLabel.ForeColor = Color.Red;
                    statusLabel.BackColor = Color.Khaki;
                    break;
                case VndbConnection.APIStatus.Throttled:
                    if (Environment.GetCommandLineArgs().Contains("-debug")) LogToFile($"{Conn.ActiveQuery.ActionName} Throttled");
                    statusLabel.Text = loginString + $@"Throttled ({Conn.ActiveQuery.ActionName})";
                    statusLabel.ForeColor = Color.DarkRed;
                    statusLabel.BackColor = Color.Khaki;
                    break;
                case VndbConnection.APIStatus.Error:
                    statusLabel.Text = loginString + $@"Error ({Conn.ActiveQuery.ActionName})";
                    statusLabel.ForeColor = Color.Black;
                    statusLabel.BackColor = Color.Red;
                    Conn.Close();
                    break;
                case VndbConnection.APIStatus.Closed:
                    statusLabel.Text = loginString + $@"Closed ({Conn.ActiveQuery.ActionName})";
                    statusLabel.ForeColor = Color.White;
                    statusLabel.BackColor = Color.Black;
                    break;
            }
        }

        /// <summary>
        /// Get username from VNDB user ID, returns empty string if error.
        /// </summary>
        internal async Task<string> GetUsernameFromID(int userID)
        {

            var result = await Conn.TryQueryNoReply($"get user basic (id={userID})");
            if (!result)
            {
                ChangeAPIStatus(Conn.Status);
                return "";
            }
            var response = JsonConvert.DeserializeObject<UserRootItem>(Conn.LastResponse.JsonPayload);
            return response.Items.Any() ? response.Items[0].Username : "";
        }

        /// <summary>
        /// Get user ID from VNDB username, returns -1 if error.
        /// </summary>
        internal async Task<int> GetIDFromUsername(string username)
        {
            var result = await Conn.TryQueryNoReply($"get user basic (username=\"{username}\")");
            if (!result)
            {
                ChangeAPIStatus(Conn.Status);
                return -1;
            }
            var response = JsonConvert.DeserializeObject<UserRootItem>(Conn.LastResponse.JsonPayload);
            return response.Items.Any() ? response.Items[0].ID : -1;
        }

        private async Task GetLanguagesForProducers(int[] producerIDs)
        {
            if (!producerIDs.Any()) return;
            var producerList = new List<ListedProducer>();
            foreach (var producerID in producerIDs)
            {
                var result = await Conn.GetProducer(producerID, "GetLanguagesForProducers Error", false);
                if (!result.Item1 || result.Item2 == null) continue;
                producerList.Add(result.Item2);
                Conn.TitlesAdded++;
                if (producerList.Count > 24)
                {
                    LocalDatabase.BeginTransaction();
                    foreach (var producer in producerList) LocalDatabase.SetProducerLanguage(producer);
                    LocalDatabase.EndTransaction();
                    producerList.Clear();
                }
            }
            LocalDatabase.BeginTransaction();
            foreach (var producer in producerList) LocalDatabase.SetProducerLanguage(producer);
            LocalDatabase.EndTransaction();
        }

        /// <summary>
        /// Searches VNDB for producers by name, independent.
        /// Call StartQuery prior to it and ChangeAPIStatus afterwards.
        /// </summary>
        internal async Task<List<ProducerItem>> AddProducersBySearchedName(string producerName)
        {
            string prodSearchQuery = $"get producer basic (search~\"{producerName}\") {{{MaxResultsString}}}";
            var result = await Conn.TryQuery(prodSearchQuery, Resources.ps_query_error);
            if (!result) return null;
            var prodRoot = JsonConvert.DeserializeObject<ProducersRoot>(Conn.LastResponse.JsonPayload);
            List<ProducerItem> prodItems = prodRoot.Items;
            var moreResults = prodRoot.More;
            var pageNo = 1;
            while (moreResults)
            {
                pageNo++;
                string prodSearchMoreQuery =
                    $"get producer basic (search~\"{producerName}\") {{{MaxResultsString}, \"page\":{pageNo}}}";
                var moreResult =
                    await Conn.TryQuery(prodSearchMoreQuery, Resources.ps_query_error);
                if (!moreResult) return null;
                var prodMoreRoot = JsonConvert.DeserializeObject<ProducersRoot>(Conn.LastResponse.JsonPayload);
                prodItems.AddRange(prodMoreRoot.Items);
                moreResults = prodMoreRoot.More;
            }
            for (int index = prodItems.Count - 1; index >= 0; index--)
            {
                if (LocalDatabase.ProducerList.Exists(x => x.Name.Equals(prodItems[index].Name))) prodItems.RemoveAt(index);
            }
            LocalDatabase.BeginTransaction();
            foreach (var producer in prodItems) LocalDatabase.InsertProducer((ListedProducer)producer, true);
            LocalDatabase.EndTransaction();
            return prodItems;

        }

        /// <summary>
        /// Change userlist status, wishlist priority or user vote.
        /// </summary>
        /// <param name="vn">VN which will be changed</param>
        /// <param name="type">What is being changed</param>
        /// <param name="statusInt">The new value</param>
        /// <param name="newVoteValue">New vote value</param>
        /// <returns>Returns whether it as successful.</returns>
        internal async Task<bool> ChangeVNStatus(ListedVN vn, VNDatabase.ChangeType type, int statusInt,
            double newVoteValue = -1)
        {
            var result = Conn.StartQuery(replyText, "Change VN Status", false, false, true);
            if (!result) return false;
            result = await Conn.ChangeVNStatus(vn, type, statusInt, newVoteValue);
            if (result) UpdateFavoriteProducerForURTChange(vn.Producer);
            return result;
        }

        /// <summary>
        /// Log into VNDB without credentials.
        /// </summary>
        internal void APILogin()
        {
            Conn.Login(ClientName, ClientVersion);
            Conn.ActiveQuery = new ApiQuery(true, replyText);
            ChangeAPIStatus(Conn.Status);
            switch (Conn.LastResponse.Type)
            {
                case ResponseType.Ok:
                    SetLoginText();
                    return;
                case ResponseType.Error:
                    if (Conn.LastResponse.Error.ID.Equals("loggedin"))
                    {
                        //should never happen
                        LoginString = Resources.already_logged_in;
                        break;
                    }
                    LoginString = Resources.connection_failed;
                    break;
                default:
                    LoginString = Resources.login_unknown_error;
                    break;
            }
            ChangeAPIStatus(Conn.Status);
            if (AdvancedMode) serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
        }

        /// <summary>
        /// Log into VNDB with credentials.
        /// </summary>
        /// <param name="password">User's password</param>
        internal void APILoginWithPassword(char[] password)
        {
            Conn.ActiveQuery = new ApiQuery(true, replyText);
            Conn.Login(ClientName, ClientVersion, Settings.Username, password);
            switch (Conn.LastResponse.Type)
            {
                case ResponseType.Ok:
                    LoginString = $@"Logged in as {Settings.Username}.";
                    ChangeAPIStatus(Conn.Status);
                    return;
                case ResponseType.Error:
                    if (Conn.LastResponse.Error.ID.Equals("loggedin"))
                    {
                        //should never happen
                        LoginString = Resources.already_logged_in;
                        break;
                    }
                    if (Conn.LastResponse.Error.ID.Equals("auth"))
                    {
                        LoginString = Conn.LastResponse.Error.Msg;
                        break;
                    }
                    LoginString = Resources.connection_failed;
                    break;
                default:
                    LoginString = Resources.login_unknown_error;
                    break;
            }
            serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
            ChangeAPIStatus(Conn.Status);
        }


    }
}