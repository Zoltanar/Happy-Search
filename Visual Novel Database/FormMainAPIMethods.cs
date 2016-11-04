using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Happy_Search.Properties;
using Newtonsoft.Json;

namespace Happy_Search
{
    public partial class FormMain
    {
        internal string CurrentFeatureName;
        /// <summary>
        /// Send query through API Connection.
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
            await Task.Run(() =>
            {
                if (Settings.Default.Limit10Years && !ignoreDateLimit && query.StartsWith("get vn ") && !query.Contains("id = "))
                {
                    query = Regex.Replace(query, "\\)", $" and released > \"{DateTime.UtcNow.Year - 10}\")");
                }
                LogToFile(query);
                Conn.Query(query);
            });
            if (_advancedMode)
            {
                if (serverR.TextLength > 10000) ClearLog(null, null);
                serverQ.Text += query + Environment.NewLine;
                serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
            }
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
                string fullThrottleMessage = "";
                double minWait = 0;
                await Task.Run(() =>
                {
                    minWait = Math.Min(5 * 60, Conn.LastResponse.Error.Fullwait); //wait 5 minutes
                    string normalWarning = $"Throttled for {Math.Floor(minWait / 60)} mins.";
                    string additionalWarning = $" Added {_vnsAdded}/skipped {_vnsSkipped}...";
                    fullThrottleMessage = additionalMessage ? normalWarning + additionalWarning : normalWarning;
                });
                WriteWarning(replyLabel, fullThrottleMessage);
                ChangeAPIStatus(VndbConnection.APIStatus.Throttled);
                LogToFile($"{DateTime.UtcNow} - {fullThrottleMessage}");
                if (refreshList)
                {
                    await ReloadListsFromDbAsync();
                    LoadVNListToGui();
                }
                var waitMS = minWait * 1000;
                var wait = Convert.ToInt32(waitMS);
                await Task.Delay(wait);
                ChangeAPIStatus(VndbConnection.APIStatus.Busy);
                await Conn.QueryAsync(query);
                if (_advancedMode)
                {
                    if (serverR.TextLength > 10000) ClearLog(null, null);
                    serverQ.Text += query + Environment.NewLine;
                    serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
                }
            }
            return true;
        }

        /// <summary>
        /// Get character data about multiple visual novels.
        /// Creates its own SQLite Transactions.
        /// </summary>
        /// <param name="vnIDs">List of VNs</param>
        /// <param name="replyLabel">Where reply will be shown</param>
        /// <param name="additionalMessage">Should added/skipped message be printed if connection is throttled?</param>
        /// <param name="refreshList">Should OLV be refreshed on throttled connection?</param>
        internal async Task GetCharactersForMultipleVN(int[] vnIDs, Label replyLabel, bool additionalMessage = false, bool refreshList = false)
        {
            if (!vnIDs.Any()) return;
            int[] currentArray = vnIDs.Take(APIMaxResults).ToArray();
            string currentArrayString = '[' + string.Join(",", currentArray) + ']';
            string charsForVNQuery = $"get character traits,vns (vn = {currentArrayString}) {{{MaxResultsString}}}";
            var queryResult = await TryQuery(charsForVNQuery, "GetCharactersForMultipleVN Query Error", replyLabel, additionalMessage, refreshList);
            if (!queryResult) return;
            var charRoot = JsonConvert.DeserializeObject<CharacterRoot>(Conn.LastResponse.JsonPayload);
            DBConn.BeginTransaction();
            foreach (var character in charRoot.Items) DBConn.UpsertSingleCharacter(character);
            DBConn.EndTransaction();
            bool moreResults = charRoot.More;
            int pageNo = 1;
            while (moreResults)
            {
                pageNo++;
                charsForVNQuery = $"get character traits,vns (vn = {currentArrayString}) {{{MaxResultsString}, \"page\":{pageNo}}}";
                queryResult = await TryQuery(charsForVNQuery, "GetCharactersForMultipleVN Query Error", replyLabel, additionalMessage, refreshList);
                if (!queryResult) return;
                charRoot = JsonConvert.DeserializeObject<CharacterRoot>(Conn.LastResponse.JsonPayload);
                DBConn.BeginTransaction();
                foreach (var character in charRoot.Items) DBConn.UpsertSingleCharacter(character);
                DBConn.EndTransaction();
                moreResults = charRoot.More;
            }
            int done = APIMaxResults;
            while (done < vnIDs.Length)
            {
                currentArray = vnIDs.Skip(done).Take(APIMaxResults).ToArray();
                currentArrayString = '[' + string.Join(",", currentArray) + ']';
                charsForVNQuery = $"get character traits,vns (vn = {currentArrayString}) {{{MaxResultsString}}}";
                queryResult = await TryQuery(charsForVNQuery, "GetCharactersForMultipleVN Query Error", replyLabel, additionalMessage, refreshList);
                if (!queryResult) return;
                charRoot = JsonConvert.DeserializeObject<CharacterRoot>(Conn.LastResponse.JsonPayload);
                DBConn.BeginTransaction();
                foreach (var character in charRoot.Items) DBConn.UpsertSingleCharacter(character);
                DBConn.EndTransaction();
                moreResults = charRoot.More;
                pageNo = 1;
                while (moreResults)
                {
                    pageNo++;
                    charsForVNQuery = $"get character traits,vns (vn = {currentArrayString}) {{{MaxResultsString}, \"page\":{pageNo}}}";
                    queryResult = await TryQuery(charsForVNQuery, "GetCharactersForMultipleVN Query Error", replyLabel, additionalMessage, refreshList);
                    if (!queryResult) return;
                    charRoot = JsonConvert.DeserializeObject<CharacterRoot>(Conn.LastResponse.JsonPayload);
                    DBConn.BeginTransaction();
                    foreach (var character in charRoot.Items) DBConn.UpsertSingleCharacter(character);
                    DBConn.EndTransaction();
                    moreResults = charRoot.More;
                }
                done += APIMaxResults;
            }
            await ReloadListsFromDbAsync();
        }

        /// <summary>
        /// Get new data about a single visual novel.
        /// </summary>
        /// <param name="vnid">ID of VN to be updated</param>
        /// <param name="updateLink">Linklabel where reply will be printed</param>
        /// <returns></returns>
        internal async Task<ListedVN> UpdateSingleVN(int vnid, LinkLabel updateLink)
        {
            string singleVNQuery = $"get vn basic,details,tags,stats (id = {vnid})";
            var result = StartQuery(userListReply, "Update Single VN");
            if (!result) return null;
            result = await TryQuery(singleVNQuery, Resources.usvn_query_error, updateLink);
            if (!result) return null;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            if (vnRoot.Num == 0)
            {
                //this vn has been deleted (or something along those lines)
                DBConn.Open();
                DBConn.RemoveVisualNovel(vnid);
                DBConn.Close();
                return new ListedVN();
            }
            var vnItem = vnRoot.Items[0];
            SaveImage(vnItem, true);
            var relProducer = await GetDeveloper(vnid, Resources.usvn_query_error, updateLink);
            await GetProducer(relProducer, Resources.usvn_query_error, updateLink);
            DBConn.Open();
            DBConn.UpsertSingleVN(vnItem, relProducer);
            var vn = DBConn.GetSingleVN(vnid, UserID);
            DBConn.Close();
            await ReloadListsFromDbAsync();
            WriteText(updateLink, Resources.vn_updated);
            ChangeAPIStatus(Conn.Status);
            return vn;
        }



        /// <summary>
        /// Get data about multiple visual novels.
        /// Creates its own SQLite Transactions.
        /// </summary>
        /// <param name="vnIDs">List of visual novel IDs</param>
        /// <param name="replyLabel">Label where reply will be printed.</param>
        /// <param name="refreshList">Should OLV be refreshed on throttled connection?</param>
        /// <param name="updateAll">Should VNs be updated even if they;re already in local database?</param>
        private async Task GetMultipleVN(IEnumerable<int> vnIDs, Label replyLabel, bool refreshList = false, bool updateAll = false)
        {
            var vnsToGet = new List<int>();
            await Task.Run(() =>
            {
                int[] vnIDList = _vnList.Select(x => x.VNID).ToArray();
                //remove already present vns
                if (!updateAll)
                {
                    foreach (var id in vnIDs)
                    {
                        if (id == 0) continue;
                        if (vnIDList.Contains(id)) _vnsSkipped++;
                        else vnsToGet.Add(id);
                    }
                }
                else
                {
                    vnsToGet = vnIDs.ToList();
                    vnsToGet.Remove(0);
                }
            });
            if (!vnsToGet.Any()) return;
            int[] currentArray = vnsToGet.Take(APIMaxResults).ToArray();
            string currentArrayString = '[' + string.Join(",", currentArray) + ']';
            string multiVNQuery = $"get vn basic,details,tags,stats (id = {currentArrayString}) {{{MaxResultsString}}}";
            var queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error, replyLabel, true, refreshList);
            if (!queryResult)
            {
                return;
            }
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            if (vnRoot.Num < currentArray.Length)
            {
                //some vns were deleted, find which ones and remove them
                var root = vnRoot;
                IEnumerable<int> deletedVNs = currentArray.Where(currentvn => root.Items.All(receivedvn => receivedvn.ID != currentvn));
                DBConn.BeginTransaction();
                foreach (var deletedVN in deletedVNs) DBConn.RemoveVisualNovel(deletedVN);
                DBConn.EndTransaction();
            }
            DBConn.BeginTransaction();
            foreach (var vnItem in vnRoot.Items)
            {
                SaveImage(vnItem);
                var relProducer = await GetDeveloper(vnItem.ID, Resources.gmvn_query_error, replyLabel, true, refreshList);
                await GetProducer(relProducer, Resources.gmvn_query_error, replyLabel, true, refreshList);
                _vnsAdded++;
                DBConn.UpsertSingleVN(vnItem, relProducer);
            }
            DBConn.EndTransaction();
            await GetCharactersForMultipleVN(currentArray, replyLabel, true, refreshList);
            int done = APIMaxResults;
            while (done < vnsToGet.Count)
            {
                currentArray = vnsToGet.Skip(done).Take(APIMaxResults).ToArray();
                currentArrayString = '[' + string.Join(",", currentArray) + ']';
                multiVNQuery = $"get vn basic,details,tags,stats (id = {currentArrayString}) {{{MaxResultsString}}}";
                queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error, replyLabel, true, refreshList);
                if (!queryResult)
                {
                    return;
                }
                vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                if (vnRoot.Num < currentArray.Length)
                {
                    //some vns were deleted, find which ones and remove them
                    var root = vnRoot;
                    IEnumerable<int> deletedVNs = currentArray.Where(currentvn => root.Items.All(receivedvn => receivedvn.ID != currentvn));
                    DBConn.BeginTransaction();
                    foreach (var deletedVN in deletedVNs) DBConn.RemoveVisualNovel(deletedVN);
                    DBConn.EndTransaction();
                }
                DBConn.BeginTransaction();
                foreach (var vnItem in vnRoot.Items)
                {
                    SaveImage(vnItem);
                    var relProducer = await GetDeveloper(vnItem.ID, Resources.gmvn_query_error, replyLabel, true, refreshList);
                    await GetProducer(relProducer, Resources.gmvn_query_error, replyLabel, true, refreshList);
                    _vnsAdded++;
                    DBConn.UpsertSingleVN(vnItem, relProducer);
                }
                DBConn.EndTransaction();
                await GetCharactersForMultipleVN(currentArray, replyLabel, true, refreshList);
                done += APIMaxResults;
            }
            await ReloadListsFromDbAsync();
        }

        /// <summary>
        /// Update titles to include all fields in latest version of Happy Search.
        /// </summary>
        /// <param name="vnIDs">List of visual novel IDs</param>
        private async Task GetOldVNStats(IEnumerable<int> vnIDs)
        {
            var replyLabel = userListReply;
            _vnsAdded = 0;
            List<int> vnsToGet = vnIDs.ToList();
            if (!vnsToGet.Any()) return;
            int[] currentArray = vnsToGet.Take(APIMaxResults).ToArray();
            string currentArrayString = '[' + string.Join(",", currentArray) + ']';
            string multiVNQuery = $"get vn stats (id = {currentArrayString}) {{{MaxResultsString}}}";
            var queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error, replyLabel, true, true);
            if (!queryResult) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            if (vnRoot.Num < currentArray.Length)
            {
                //some vns were deleted, find which ones and remove them
                var root = vnRoot;
                IEnumerable<int> deletedVNs = currentArray.Where(currentvn => root.Items.All(receivedvn => receivedvn.ID != currentvn));
                DBConn.BeginTransaction();
                foreach (var deletedVN in deletedVNs) DBConn.RemoveVisualNovel(deletedVN);
                DBConn.EndTransaction();
            }
            DBConn.BeginTransaction();
            foreach (var vnItem in vnRoot.Items)
            {
                DBConn.UpdateVNToLatestVersion(vnItem);
                _vnsAdded++;
            }
            DBConn.EndTransaction();
            int done = APIMaxResults;
            while (done < vnsToGet.Count)
            {
                currentArray = vnsToGet.Skip(done).Take(APIMaxResults).ToArray();
                currentArrayString = '[' + string.Join(",", currentArray) + ']';
                multiVNQuery = $"get vn stats (id = {currentArrayString}) {{{MaxResultsString}}}";
                queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error, replyLabel, true, true);
                if (!queryResult) return;
                vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                if (vnRoot.Num < currentArray.Length)
                {
                    //some vns were deleted, find which ones and remove them
                    var root = vnRoot;
                    IEnumerable<int> deletedVNs = currentArray.Where(currentvn => root.Items.All(receivedvn => receivedvn.ID != currentvn));
                    DBConn.BeginTransaction();
                    foreach (var deletedVN in deletedVNs) DBConn.RemoveVisualNovel(deletedVN);
                    DBConn.EndTransaction();
                }
                DBConn.BeginTransaction();
                foreach (var vnItem in vnRoot.Items)
                {
                    DBConn.UpdateVNToLatestVersion(vnItem);
                    _vnsAdded++;
                }
                DBConn.EndTransaction();
                done += APIMaxResults;
            }
            await ReloadListsFromDbAsync();
        }

        /// <summary>
        /// Update tags of titles that haven't been updated in over 7 days.
        /// </summary>
        /// <param name="vnIDs">List of visual novel IDs</param>
        private async Task UpdateTitleData(IEnumerable<int> vnIDs)
        {
            var replyLabel = userListReply;
            _vnsAdded = 0;
            List<int> vnsToGet = vnIDs.ToList();
            if (!vnsToGet.Any()) return;
            int[] currentArray = vnsToGet.Take(APIMaxResults).ToArray();
            string currentArrayString = '[' + string.Join(",", currentArray) + ']';
            string multiVNQuery = $"get vn tags,stats (id = {currentArrayString}) {{{MaxResultsString}}}";
            var queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error, replyLabel, true, true);
            if (!queryResult) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            if (vnRoot.Num < currentArray.Length)
            {
                //some vns were deleted, find which ones and remove them
                var root = vnRoot;
                IEnumerable<int> deletedVNs = currentArray.Where(currentvn => root.Items.All(receivedvn => receivedvn.ID != currentvn));
                DBConn.BeginTransaction();
                foreach (var deletedVN in deletedVNs) DBConn.RemoveVisualNovel(deletedVN);
                DBConn.EndTransaction();
            }
            DBConn.BeginTransaction();
            foreach (var vnItem in vnRoot.Items)
            {
                DBConn.UpdateVNData(vnItem);
                _vnsAdded++;
            }
            DBConn.EndTransaction();
            await GetCharactersForMultipleVN(currentArray, replyLabel, true, true);
            int done = APIMaxResults;
            while (done < vnsToGet.Count)
            {
                currentArray = vnsToGet.Skip(done).Take(APIMaxResults).ToArray();
                currentArrayString = '[' + string.Join(",", currentArray) + ']';
                multiVNQuery = $"get vn tags,stats (id = {currentArrayString}) {{{MaxResultsString}}}";
                queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error, replyLabel, true, true);
                if (!queryResult) return;
                vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                if (vnRoot.Num < currentArray.Length)
                {
                    //some vns were deleted, find which ones and remove them
                    var root = vnRoot;
                    IEnumerable<int> deletedVNs = currentArray.Where(currentvn => root.Items.All(receivedvn => receivedvn.ID != currentvn));
                    DBConn.BeginTransaction();
                    foreach (var deletedVN in deletedVNs) DBConn.RemoveVisualNovel(deletedVN);
                    DBConn.EndTransaction();
                }
                DBConn.BeginTransaction();
                foreach (var vnItem in vnRoot.Items)
                {
                    DBConn.UpdateVNData(vnItem);
                    _vnsAdded++;
                }
                DBConn.EndTransaction();
                await GetCharactersForMultipleVN(currentArray, replyLabel, true, true);
                done += APIMaxResults;
            }
            await ReloadListsFromDbAsync();
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
            string developerQuery = $"get release basic,producers (vn =\"{vnid}\") {{{MaxResultsString}}}";
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
        /// A DBConnection must be open or a transaction must be occuring.
        /// </summary>
        /// <param name="producerID">ID of Producer</param>
        /// <param name="errorMessage">Message to be printed in case of error</param>
        /// <param name="replyLabel">Label where reply will be printed.</param>
        /// <param name="additionalMessage">Should added/skipped message be printed if connection is throttled?</param>
        /// <param name="refreshList">Should OLV be refreshed on throttled connection?</param>
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
            DBConn.InsertProducer((ListedProducer)producer);
        }

        /// <summary>
        /// Change text and color of API Status label based on status.
        /// </summary>
        /// <param name="apiStatus">Status of API Connection</param>
        internal void ChangeAPIStatus(VndbConnection.APIStatus apiStatus)
        {
            switch (apiStatus)
            {
                case VndbConnection.APIStatus.Ready:
                    CurrentFeatureName = "";
                    statusLabel.Text = @"Ready";
                    statusLabel.ForeColor = Color.Black;
                    statusLabel.BackColor = Color.LightGreen;
                    break;
                case VndbConnection.APIStatus.Busy:
                    statusLabel.Text = $@"Busy ({CurrentFeatureName})";
                    statusLabel.ForeColor = Color.Red;
                    statusLabel.BackColor = Color.Khaki;
                    break;
                case VndbConnection.APIStatus.Throttled:
                    statusLabel.Text = $@"Throttled ({CurrentFeatureName})";
                    statusLabel.ForeColor = Color.DarkRed;
                    statusLabel.BackColor = Color.Khaki;
                    break;
                case VndbConnection.APIStatus.Error:
                    statusLabel.Text = $@"Error ({CurrentFeatureName})";
                    statusLabel.ForeColor = Color.Black;
                    statusLabel.BackColor = Color.Red;
                    Conn.Close();
                    break;
                case VndbConnection.APIStatus.Closed:
                    statusLabel.Text = $@"Closed ({CurrentFeatureName})";
                    statusLabel.ForeColor = Color.White;
                    statusLabel.BackColor = Color.Black;
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
            var result = StartQuery(replyText, "Change VN Status");
            if (!result) return false;
            switch (type)
            {
                case ChangeType.UL:
                    queryString = statusInt == -1
                        ? $"set vnlist {vn.VNID}"
                        : $"set vnlist {vn.VNID} {{\"status\":{statusInt}}}";
                    result = await TryQuery(queryString, Resources.cvns_query_error, replyText);
                    if (!result) return false;
                    DBConn.BeginTransaction();
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
                    DBConn.BeginTransaction();
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
                    DBConn.BeginTransaction();
                    if (hasULStatus || hasWLStatus)
                        DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.Vote, statusInt, Command.Update);
                    else if (statusInt == -1)
                        DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.Vote, statusInt, Command.Delete);
                    else DBConn.UpdateVNStatus(UserID, vn.VNID, ChangeType.Vote, statusInt, Command.New);
                    break;
            }
            DBConn.EndTransaction();
            await UpdateFavoriteProducerForURTChange(vn.Producer);
            ChangeAPIStatus(Conn.Status);
            return true;
        }

        /// <summary>
        /// Log into VNDB without credentials.
        /// </summary>
        internal void APILogin()
        {
            Conn.Login(ClientName, ClientVersion);
            CurrentFeatureName = "Login";
            switch (Conn.LastResponse.Type)
            {
                case ResponseType.Ok:
                    loginReply.ForeColor = Color.LightGreen;
                    loginReply.Text = UserID > 0 ? $"Connected with ID {UserID}." : "Connected without ID.";
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
            CurrentFeatureName = "Login with credentials";
            Conn.Login(ClientName, ClientVersion, credentials.Key, credentials.Value);
            switch (Conn.LastResponse.Type)
            {
                case ResponseType.Ok:
                    ChangeAPIStatus(Conn.Status);
                    loginReply.ForeColor = Color.LightGreen;
                    loginReply.Text = $@"Logged in as {credentials.Key}.";
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

        /// <summary>
        /// Check if API Connection is ready, change status accordingly and write error if it isnt ready.
        /// </summary>
        /// <param name="replyLabel">Label where error reply will be printed</param>
        /// <param name="featureName">Name of feature calling the query</param>
        /// <returns>If connection was ready</returns>
        internal bool StartQuery(Label replyLabel, string featureName)
        {
            if (!CurrentFeatureName.Equals(""))
            {
                WriteError(replyLabel, $"Wait until {CurrentFeatureName} is done.");
                return false;
            }
            CurrentFeatureName = featureName;
            ChangeAPIStatus(VndbConnection.APIStatus.Busy);
            return true;
        }

    }
}