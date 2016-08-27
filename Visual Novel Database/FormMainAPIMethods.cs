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
        /// Sending query through API Connection.
        /// </summary>
        /// <param name="query">Command to be sent</param>
        /// <param name="errorMessage">Message to be printed in case of error</param>
        /// <param name="replyLabel">Label where reply will be printed.</param>
        /// <param name="additionalMessage">Should added/skipped message be printed if connection is throttled?</param>
        /// <param name="refreshList">Should OLV be refreshed on throttled connection?</param>
        /// <param name="ignoreDateLimit">Ignore 10 Year VN Limit (if enabled)?</param>
        /// <returns>Returns whether it was successful.</returns>
        internal async Task<bool> TryQuery(string query, string errorMessage, Label replyLabel,
            bool additionalMessage = false, bool refreshList = false, bool ignoreDateLimit = false)
        {
            if (Conn.Status != VndbConnection.APIStatus.Ready)
            {
                WriteError(replyLabel, "API Connection isn't ready.", true);
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
                    WriteError(replyLabel, errorMessage);
                    ChangeAPIStatus(Conn.Status);
                    return false;
                }
                var waitS = Conn.LastResponse.Error.Minwait * 30;
                var minWait = Math.Min(waitS, Conn.LastResponse.Error.Fullwait);
                string normalWarning = $"Throttled for {Math.Floor(minWait)} secs.";
                string additionalWarning = $" Added {_vnsAdded} and skipped {_vnsSkipped} so far...";
                var fullThrottleMessage = additionalMessage ? normalWarning + additionalWarning : normalWarning;
                WriteWarning(replyLabel, fullThrottleMessage);
                ChangeAPIStatus(VndbConnection.APIStatus.Throttled);
                var waitMS = minWait * 1000;
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
        /// Get new data about a single visual novel.
        /// </summary>
        /// <param name="vnid">ID of VN to be updated</param>
        /// <param name="updateLink">Linklabel where reply will be printed</param>
        /// <returns></returns>
        internal async Task<ListedVN> UpdateSingleVN(int vnid, LinkLabel updateLink)
        {
            ReloadLists();
            string singleVNQuery = $"get vn basic,details,tags (id = {vnid})";
            var result = await TryQuery(singleVNQuery, Resources.usvn_query_error, updateLink);
            if (!result) return null;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            var vnItem = vnRoot.Items[0];
            SaveImage(vnItem);
            var relProducer = await GetDeveloper(vnid, Resources.usvn_query_error, updateLink);
            await GetProducer(relProducer, Resources.usvn_query_error, updateLink);
            DBConn.Open();
            DBConn.UpsertSingleVN(vnItem, relProducer, false);
            var vn = DBConn.GetSingleVN(vnid, UserID);
            DBConn.Close();
            WriteText(updateLink, Resources.vn_updated);
            return vn;
        }

        /// <summary>
        /// Get data about a single visual novel.
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
            if (forceUpdate == false && vnIDList.Contains(vnid))
            {
                _vnsSkipped++;
                return;
            }
            string singleVNQuery = $"get vn basic,details,tags (id = {vnid})";
            var result =
                await TryQuery(singleVNQuery, Resources.svn_query_error, replyLabel, additionalMessage, refreshList);
            if (!result) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            if (vnRoot.Num == 0) return;
            var vnItem = vnRoot.Items[0];
            SaveImage(vnItem);
            var relProducer = await GetDeveloper(vnid, Resources.svn_query_error, replyLabel, additionalMessage, refreshList);
            await GetProducer(relProducer, Resources.svn_query_error, replyLabel, additionalMessage, refreshList);
            DBConn.Open();
            DBConn.UpsertSingleVN(vnItem, relProducer, false);
            DBConn.Close();
            _vnsAdded++;
        }

        /// <summary>
        /// Get data about multiple visual novels.
        /// </summary>
        /// <param name="vnIDsEnumerable">List of IDs of VNs to be retrieved.</param>
        /// <param name="replyLabel">Label where reply will be printed.</param>
        /// <param name="refreshList">Should OLV be refreshed on throttled connection?</param>
        /// <param name="updateAll">Should VNs be updated if they are already in VNList?</param>
        /// <returns></returns>
        internal async Task GetMultipleVNOld(IEnumerable<int> vnIDsEnumerable, Label replyLabel, bool refreshList = false, bool updateAll = false)
        {
            ReloadLists();
            //Old - one by one
            foreach (var id in vnIDsEnumerable)
            {
                int[] vnIDList = _vnList.Select(x => x.VNID).ToArray();
                if (!updateAll && vnIDList.Contains(id))
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
                SaveImage(vnItem);
                var relProducer = await GetDeveloper(id, Resources.gmvn_query_error, replyLabel, true, refreshList);
                await GetProducer(relProducer, Resources.gmvn_query_error, replyLabel, true, refreshList);
                _vnsAdded++;
                DBConn.Open();
                DBConn.UpsertSingleVN(vnItem, relProducer, false);
                DBConn.Close();
            }
        }

        internal async Task GetMultipleVN(IEnumerable<int> vnIDs, Label replyLabel, bool refreshList = false, bool updateAll = false)
        {
            //TODO Change to array queries
            ReloadLists();
            //New - 25 by 25
            var vnsToGet = new List<int>();
            int[] vnIDList = _vnList.Select(x => x.VNID).ToArray();
            //remove already present vns
            if (!updateAll)
            {
                foreach (var id in vnIDs)
                {
                    if (vnIDList.Contains(id)) _vnsSkipped++;
                    else vnsToGet.Add(id);
                }
            }
            else vnsToGet = vnIDs.ToList();
            if (!vnsToGet.Any()) return;
            string first25 = '[' + string.Join(",", vnsToGet.Take(25)) + ']';
            string multiVNQuery = $"get vn basic,details,tags (id = {first25}) {{{APIMaxResults}}}";
            var queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error, replyLabel, true, refreshList);
            if (!queryResult) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            if (vnRoot.Num == 0) return;
            foreach (var vnItem in vnRoot.Items)
            {
                SaveImage(vnItem);
                var relProducer = await GetDeveloper(vnItem.ID, Resources.gmvn_query_error, replyLabel, true, refreshList);
                await GetProducer(relProducer, Resources.gmvn_query_error, replyLabel, true, refreshList);
                _vnsAdded++;
                DBConn.Open();
                DBConn.UpsertSingleVN(vnItem, relProducer, false);
                DBConn.Close();
            }
            int done = 25;
            while (done < vnsToGet.Count)
            {
                string next25 = '[' + string.Join(",", vnsToGet.Skip(done).Take(25)) + ']';
                multiVNQuery = $"get vn basic,details,tags (id = {next25}) {{{APIMaxResults}}}";
                queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error, replyLabel, true, refreshList);
                if (!queryResult) return;
                vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                if (vnRoot.Num == 0) return;
                foreach (var vnItem in vnRoot.Items)
                {
                    SaveImage(vnItem);
                    var relProducer = await GetDeveloper(vnItem.ID, Resources.gmvn_query_error, replyLabel, true, refreshList);
                    await GetProducer(relProducer, Resources.gmvn_query_error, replyLabel, true, refreshList);
                    _vnsAdded++;
                    DBConn.Open();
                    DBConn.UpsertSingleVN(vnItem, relProducer, false);
                    DBConn.Close();
                }
                done += 25;
            }
        }

        /// <summary>
        /// Get Developer for VN by VNID.
        /// </summary>
        /// <param name="vnid">ID of VN</param>
        /// <param name="errorMessage">Message to be printed in case of error</param>
        /// <param name="replyLabel">Label where reply will be printed.</param>
        /// <param name="additionalMessage">Should added/skipped message be printed if connection is throttled?</param>
        /// <param name="refreshList">Should OLV be refreshed on throttled connection?</param>
        /// <returns></returns>
        internal async Task<int> GetDeveloper(int vnid, string errorMessage, Label replyLabel, bool additionalMessage = false, bool refreshList = false)
        {
            string developerQuery = $"get release basic,producers (vn =\"{vnid}\") {{{APIMaxResults}}}";
            var releaseResult =
                await TryQuery(developerQuery, errorMessage, replyLabel, additionalMessage, refreshList);
            if (!releaseResult) return -1;
            var relInfo = JsonConvert.DeserializeObject<ReleasesRoot>(Conn.LastResponse.JsonPayload);
            List<ReleaseItem> relItem = relInfo.Items;
            relItem.Sort((x, y) => DateTime.Compare(StringToDate(x.Released), StringToDate(y.Released)));
            if (!relItem.Any()) return -1;
            foreach (var item in relItem)
            {
                var dev = item.Producers.Find(x => x.Developer);
                if (dev != null) return dev.ID;
            }
            return -1;
        }

        /// <summary>
        /// Get Producer from Producer ID.
        /// </summary>
        /// <param name="producerID">ID of Producer</param>
        /// <param name="errorMessage">Message to be printed in case of error</param>
        /// <param name="replyLabel">Label where reply will be printed.</param>
        /// <param name="additionalMessage">Should added/skipped message be printed if connection is throttled?</param>
        /// <param name="refreshList">Should OLV be refreshed on throttled connection?</param>
        /// <returns></returns>
        internal async Task GetProducer(int producerID, string errorMessage, Label replyLabel, bool additionalMessage = false, bool refreshList = false)
        {
            int[] producerIDList = _producerList.Select(x => x.ID).ToArray();
            if (producerID == -1 || producerIDList.Contains(producerID)) return;
            string producerQuery = $"get producer basic (id={producerID})";
            var producerResult =
                await TryQuery(producerQuery, errorMessage, replyLabel, additionalMessage, refreshList);
            if (!producerResult) return;
            var root = JsonConvert.DeserializeObject<ProducersRoot>(Conn.LastResponse.JsonPayload);
            List<ProducerItem> producers = root.Items;
            if (!producers.Any()) return;
            var producer = producers.First();
            DBConn.Open();
            DBConn.InsertProducer(new ListedProducer(producer.Name, -1, "No", DateTime.UtcNow, producer.ID));
            DBConn.Close();
        }

        /// <summary>
        /// Change text and color of API Status label based on status.
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
        /// Change userlist status, wishlist priority or user vote.
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
                        : $"set votelist {vn.VNID} {{\"vote\":{statusInt * 10}}}";
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
        /// Log into VNDB without credentials.
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
        /// Log into VNDB with credentials.
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