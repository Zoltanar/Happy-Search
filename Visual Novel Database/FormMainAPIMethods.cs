using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Happy_Search.Properties;
using Newtonsoft.Json;

namespace Happy_Search
{
    partial class FormMain
    {
        /// <summary>
        ///     Method for sending query/command through API Connection.
        /// </summary>
        /// <param name="query">Command to be sent</param>
        /// <param name="errorMessage">Message to be printed in case of error</param>
        /// <param name="label">Label where reply will be printed.</param>
        /// <param name="additionalMessage">Should added/skipped message be printed if connection is throttled?</param>
        /// <param name="refreshList">Should OLV be refreshed on throttled connection?</param>
        /// <param name="ignoreDateLimit">Ignore 10 Year VN Limit (if enabled)?</param>
        /// <returns>Returns whether it was successful.</returns>
        internal async Task<bool> TryQuery(string query, string errorMessage, Label label,
            bool additionalMessage = false, bool refreshList = false, bool ignoreDateLimit = false)
        {
            if (Conn.Status != VndbConnection.APIStatus.Ready)
            {
                WriteError(label, "API Connection isn't ready.");
                return false;
            }
            //change status to busy until it is solved, if error is returned then status is changed to throttled or ready if the error isn't throttling error
            //if response type is unknown then status is changed to error because it's probably a connection loss and will require reconnecting.
            ChangeAPIStatus(VndbConnection.APIStatus.Busy);
            if (Settings.Default.Limit10Years && !ignoreDateLimit && query.StartsWith("get vn ") && !query.Contains("id = "))
            {
                query = Regex.Replace(query, "\\)", $" and released > \"{DateTime.UtcNow.Year - 10}\")");
            }
            Debug.Print(query);
            await Conn.QueryAsync(query); //request detailed information
            serverQ.Text += query + Environment.NewLine;
            serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
            if (Conn.LastResponse.Type == ResponseType.Unknown)
            {
                ChangeAPIStatus(VndbConnection.APIStatus.Error);
                return false;
            }
            while (Conn.LastResponse.Type == ResponseType.Error)
            {
                if (!Conn.LastResponse.Error.ID.Equals("throttled"))
                {
                    WriteError(label, errorMessage);
                    ChangeAPIStatus(Conn.Status);
                    return false;
                }
                var waitS = Conn.LastResponse.Error.Minwait*30;
                var minWait = Math.Min(waitS, Conn.LastResponse.Error.Fullwait);
                string normalWarning = $"Throttled for {Math.Floor(minWait)} secs.";
                string additionalWarning = $" Added {_vnsAdded} and skipped {_vnsSkipped} so far...";
                var fullThrottleMessage = additionalMessage ? normalWarning + additionalWarning : normalWarning;
                WriteWarning(label, fullThrottleMessage);
                ChangeAPIStatus(VndbConnection.APIStatus.Throttled);
                var waitMS = minWait*1000;
                var wait = Convert.ToInt32(waitMS);
                Debug.Print($"{DateTime.UtcNow} - {fullThrottleMessage}");
                if (refreshList) tileOLV.SetObjects(_vnList.Where(_currentList));
                await Task.Delay(wait);
                ClearLog(null, null);
                await Conn.QueryAsync(query); //request detailed information
                serverQ.Text += query + Environment.NewLine;
                serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
            }
            ChangeAPIStatus(VndbConnection.APIStatus.Ready);
            return true;
        }

        /// <summary>
        ///     Method for updating data on a visual novel.
        /// </summary>
        /// <param name="vnid">ID of VN to be updated</param>
        /// <param name="updateLink">Linklabel where reply will be printed</param>
        /// 
        /// <returns></returns>
        internal async Task UpdateSingleVN(int vnid, LinkLabel updateLink)
        {
            ReloadLists();
            var producerIDList = _producerList.Select(x => x.ID).ToArray();
            string singleVNQuery = $"get vn basic,details,tags (id = {vnid})";
            var result = await TryQuery(singleVNQuery, Resources.usvn_query_error, updateLink);
            if (!result) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            var vnItem = vnRoot.Items[0];
            var relProducer = -1;
            SaveImage(vnItem);
            //fetch developer from releases
            string relInfoQuery = $"get release producers (vn =\"{vnid}\")";
            var releaseResult = await TryQuery(relInfoQuery, Resources.usvn_query_error, updateLink);
            if (!releaseResult) return;
            var relInfo = JsonConvert.DeserializeObject<ReleasesRoot>(Conn.LastResponse.JsonPayload);
            List<ReleaseItem> relItem = relInfo.Items;
            if (relItem.Count != 0)
            {
                foreach (var item in relItem)
                {
                    relProducer = item.Producers.Find(x => x.Developer)?.ID ?? -1;
                    if (relProducer > 0) break;
                }
            }
            if (relProducer != -1 && !producerIDList.Contains(relProducer))
            {
                //query api
                string producerQuery = $"get producer basic (id={relProducer})";
                var producerResult = await TryQuery(producerQuery, Resources.usvn_query_error, updateLink);
                if (!producerResult) return;
                var root = JsonConvert.DeserializeObject<ProducersRoot>(Conn.LastResponse.JsonPayload);
                List<ProducerItem> producers = root.Items;
                DBConn.Open();
                foreach (var producer in producers)
                {
                    if (producerIDList.Contains(producer.ID)) continue;

                    DBConn.InsertProducer(new ListedProducer(producer.Name, -1, "No", DateTime.UtcNow, producer.ID));
                }
                DBConn.Close();
            }
            DBConn.Open();
            DBConn.UpsertSingleVN(vnItem, relProducer, false);
            DBConn.Close();
            WriteText(updateLink, Resources.vn_updated);
            DBConn.Open();
            UpdatingVN = DBConn.GetSingleVN(vnid, UserID);
            DBConn.Close();
        }

        /// <summary>
        ///     Method for retrieving data about a single visual novel.
        /// </summary>
        /// <param name="vnid">ID of VN to be retrieved.</param>
        /// <param name="replyLabel">Label where reply will be printed.</param>
        /// <param name="forceUpdate">Should VN be updated even if it's already in VNList?</param>
        /// <param name="additionalMessage">Should added/skipped message be printed if connection is throttled?</param>
        /// <param name="refreshList">Should OLV be refreshed on throttled connection?</param>
        /// <returns></returns>
        internal async Task GetSingleVN(int vnid, Label replyLabel, bool forceUpdate = false, bool additionalMessage = false, bool refreshList = false)
        {
            int[] vnIDList = _vnList.Select(x => x.VNID).ToArray();
            int[] producerIDList = _producerList.Select(x => x.ID).ToArray();
            if (vnIDList.Contains(vnid) && forceUpdate == false)
            {
                _vnsSkipped++;
                return;
            }
            //fetch visual novel information
            string singleVNQuery = $"get vn basic,details,tags (id = {vnid})";
            var result =
                await TryQuery(singleVNQuery, Resources.svn_query_error, replyLabel, additionalMessage, refreshList);
            if (!result) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            var vnItem = vnRoot.Items[0];
            var relProducer = -1;
            SaveImage(vnItem);
            //fetch developer from releases
            string relInfoQuery = $"get release producers (vn =\"{vnid}\")";
            var releaseResult =
                await TryQuery(relInfoQuery, Resources.gsvn_query_error, replyLabel, additionalMessage, refreshList);
            if (!releaseResult) return;
            var relInfo = JsonConvert.DeserializeObject<ReleasesRoot>(Conn.LastResponse.JsonPayload);
            List<ReleaseItem> relItem = relInfo.Items;
            if (relItem.Any())
            {
                foreach (var item in relItem)
                {
                    relProducer = item.Producers.Find(x => x.Developer)?.ID ?? -1;
                    if (relProducer > 0) break;
                }
            }
            //get producer information if not already present
            if (relProducer != -1 && !producerIDList.Contains(relProducer))
            {
                string producerQuery = $"get producer basic (id={relProducer})";
                var producerResult =
                    await TryQuery(producerQuery, Resources.sp_query_error, replyLabel, additionalMessage, refreshList);
                if (!producerResult) return;
                var root = JsonConvert.DeserializeObject<ProducersRoot>(Conn.LastResponse.JsonPayload);
                List<ProducerItem> producers = root.Items;
                DBConn.Open();
                //insert all producers that weren't already present
                foreach (var producer in producers)
                {
                    if (producerIDList.Contains(producer.ID)) continue;
                    DBConn.InsertProducer(new ListedProducer(producer.Name, -1, "No", DateTime.UtcNow, producer.ID));
                }
                DBConn.Close();
            }
            DBConn.Open();
            DBConn.UpsertSingleVN(vnItem, relProducer, false);
            DBConn.Close();
            _vnsAdded++;
        }

        /// <summary>
        /// Method for retrieving data about multiple visual novels, to be changed into array query later.
        /// </summary>
        /// <param name="vnIDs">List of IDs of VNs to be retrieved.</param>
        /// <param name="replyLabel">Label where reply will be printed.</param>
        /// <param name="refreshList">Should OLV be refreshed on throttled connection?</param> 
        /// <returns></returns>
        internal async Task GetMultipleVN(IEnumerable<int> vnIDs, Label replyLabel, bool refreshList = false)
        {
            ReloadLists();
            int[] producerIDList = _producerList.Select(x => x.ID).ToArray();
            foreach (var id in vnIDs)
            {
                int[] vnIDList = _vnList.Select(x => x.VNID).ToArray();
                if (vnIDList.Contains(id))
                {
                    _vnsSkipped++;
                    continue;
                }
                string singleVNQuery = $"get vn basic,details,tags (id = {id})";
                var result = await TryQuery(singleVNQuery, Resources.gmvn_query_error, replyLabel, true, refreshList);
                if (!result) continue;
                var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                if (vnRoot.Num == 0) continue;
                var vnItem = vnRoot.Items[0];
                var relProducer = -1;
                SaveImage(vnItem);
                //fetch developer from releases
                string relInfoQuery = $"get release producers (vn =\"{id}\")";
                var releaseResult =
                    await TryQuery(relInfoQuery, Resources.gmvn_query_error, replyLabel, true, refreshList);
                if (!releaseResult) continue;
                var relInfo = JsonConvert.DeserializeObject<ReleasesRoot>(Conn.LastResponse.JsonPayload);
                List<ReleaseItem> relItem = relInfo.Items;
                if (relItem.Any())
                {
                    foreach (var item in relItem)
                    {
                        relProducer = item.Producers.Find(x => x.Developer)?.ID ?? -1;
                        if (relProducer > 0) break;
                    }
                }
                if (relProducer != -1 && !producerIDList.Contains(relProducer))
                {
                    //query api
                    string producerQuery = $"get producer basic (id={relProducer})";
                    var producerResult =
                        await TryQuery(producerQuery, Resources.gmvn_query_error, replyLabel, true, refreshList);
                    if (!producerResult) return;
                    var root = JsonConvert.DeserializeObject<ProducersRoot>(Conn.LastResponse.JsonPayload);
                    List<ProducerItem> producers = root.Items;
                    DBConn.Open();
                    foreach (var producer in producers)
                    {
                        if (producerIDList.Contains(producer.ID)) continue;
                        DBConn.InsertProducer(new ListedProducer(producer.Name, -1, "No", DateTime.UtcNow, producer.ID));
                    }
                    DBConn.Close();
                }
                _vnsAdded++;
                DBConn.Open();
                DBConn.UpsertSingleVN(vnItem, relProducer, false);
                DBConn.Close();
            }
        }

        /// <summary>
        ///     Method for changing text and color of API Status label
        /// </summary>
        /// <param name="apiStatus">Status of API Connection</param>
        private void ChangeAPIStatus(VndbConnection.APIStatus apiStatus)
        {
            switch (apiStatus)
            {
                case VndbConnection.APIStatus.Ready:
                    statusLabel.Text = @"Ready";
                    statusLabel.BackColor = Color.LightGreen;
                    break;
                case VndbConnection.APIStatus.Busy:
                    statusLabel.Text = @"Busy";
                    statusLabel.BackColor = Color.Khaki;
                    break;
                case VndbConnection.APIStatus.Throttled:
                    statusLabel.Text = @"Throttled";
                    statusLabel.BackColor = Color.Khaki;
                    break;
                case VndbConnection.APIStatus.Error:
                    statusLabel.Text = @"Error";
                    statusLabel.BackColor = Color.Red;
                    Conn.Close();
                    break;
            }
        }

        /// <summary>
        ///     Method for changing userlist status, wishlist priority or user vote.
        /// </summary>
        /// <param name="vn">VN which will be changed</param>
        /// <param name="type">What is being changed</param>
        /// <param name="statusInt">The new value</param>
        /// <returns>Returns whether it as successful.</returns>
        private async Task<bool> ChangeVNStatus(ListedVN vn, ChangeType type, int statusInt)
        {
            var hasULStatus = vn.ULStatus != null && !vn.ULStatus.Equals("");
            var hasWLStatus = vn.WLStatus != null && !vn.WLStatus.Equals("");
            var hasVote = vn.Vote > 0;
            string queryString;
            bool result;
            switch (type)
            {
                case ChangeType.UL:
                    queryString = statusInt == -1
                        ? $"set vnlist {vn.VNID}"
                        : $"set vnlist {vn.VNID} {{\"status\":{statusInt}}}";
                    result = await TryQuery(queryString, Resources.cvns_query_error, replyText);
                    if (!result) return false;
                    DBConn.Open();
                    if (hasWLStatus || hasVote)
                        DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.UL, statusInt, Command.Update);
                    else if (statusInt == -1)
                        DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.UL, statusInt, Command.Delete);
                    else DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.UL, statusInt, Command.New);
                    break;
                case ChangeType.WL:
                    queryString = statusInt == -1
                        ? $"set wishlist {vn.VNID}"
                        : $"set wishlist {vn.VNID} {{\"priority\":{statusInt}}}";
                    result = await TryQuery(queryString, Resources.cvns_query_error, replyText);
                    if (!result) return false;
                    DBConn.Open();
                    if (hasULStatus || hasVote)
                        DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.WL, statusInt, Command.Update);
                    else if (statusInt == -1)
                        DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.WL, statusInt, Command.Delete);
                    else DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.WL, statusInt, Command.New);
                    break;
                case ChangeType.Vote:
                    queryString = statusInt == -1
                        ? $"set votelist {vn.VNID}"
                        : $"set votelist {vn.VNID} {{\"vote\":{statusInt*10}}}";
                    result = await TryQuery(queryString, Resources.cvns_query_error, replyText);
                    if (!result) return false;
                    DBConn.Open();
                    if (hasULStatus || hasWLStatus)
                        DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.Vote, statusInt, Command.Update);
                    else if (statusInt == -1)
                        DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.Vote, statusInt, Command.Delete);
                    else DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.Vote, statusInt, Command.New);
                    break;
            }
            DBConn.Close();
            return true;
        }

        /// <summary>
        /// Method for logging into VNDB without credentials.
        /// </summary>
        internal void APILogin()
        {
            Conn.Login(ClientName, ClientVersion);
            switch (Conn.LastResponse.Type)
            {
                case ResponseType.Ok:
                    loginReply.ForeColor = Color.LightGreen;
                    loginReply.Text = UserID > 0 ? $"Connected with ID {UserID}." : "Connected without ID.";
                    Console.WriteLine(loginReply.Text);
                    ChangeAPIStatus(Conn.Status);
                    return;
                case ResponseType.Error:
                    if (Conn.LastResponse.Error.ID.Equals("loggedin"))
                    {
                        //should never happen
                        loginReply.ForeColor = Color.LightGreen;
                        loginReply.Text = Resources.already_logged_in;
                        break;
                    }
                    loginReply.ForeColor = Color.Red;
                    loginReply.Text = Resources.connection_failed;
                    break;
                default:
                    loginReply.ForeColor = Color.Red;
                    loginReply.Text = Resources.login_unknown_error;
                    break;
            }
            serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
            ChangeAPIStatus(Conn.Status);
        }

        /// <summary>
        /// Method for logging into VNDB with credentials.
        /// </summary>
        /// <param name="credentials">User's username and password</param>
        internal void APILoginWithCredentials(KeyValuePair<string, char[]> credentials)
        {
            Conn.Login(ClientName, ClientVersion, credentials.Key, credentials.Value);
            switch (Conn.LastResponse.Type)
            {
                case ResponseType.Ok:
                    ChangeAPIStatus(Conn.Status);
                    loginReply.ForeColor = Color.LightGreen;
                    loginReply.Text = $"Logged in as {credentials.Key}.";
                    return;
                case ResponseType.Error:
                    if (Conn.LastResponse.Error.ID.Equals("loggedin"))
                    {
                        //should never happen
                        loginReply.ForeColor = Color.LightGreen;
                        loginReply.Text = Resources.already_logged_in;
                        break;
                    }
                    if (Conn.LastResponse.Error.ID.Equals("auth"))
                    {
                        loginReply.ForeColor = Color.Red;
                        loginReply.Text = Conn.LastResponse.Error.Msg;
                        break;
                    }
                    loginReply.ForeColor = Color.Red;
                    loginReply.Text = Resources.connection_failed;
                    break;
                default:
                    loginReply.ForeColor = Color.Red;
                    loginReply.Text = Resources.login_unknown_error;
                    break;
            }
            serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
            ChangeAPIStatus(Conn.Status);
        }

    }
}