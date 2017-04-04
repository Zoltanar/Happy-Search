using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Happy_Search.Properties;
using Newtonsoft.Json;
using static Happy_Search.StaticHelpers;

namespace Happy_Search
{
    public partial class FormMain
    {
        internal ApiQuery ActiveQuery;

        /// <summary>
        /// Send query through API Connection.
        /// </summary>
        /// <param name="query">Command to be sent</param>
        /// <param name="errorMessage">Message to be printed in case of error</param>
        /// <returns>Returns whether it was successful.</returns>
        internal async Task<bool> TryQuery(string query, string errorMessage)
        {
            if (Conn.Status != VndbConnection.APIStatus.Ready)
            {
                WriteError(ActiveQuery.ReplyLabel, "API Connection isn't ready.");
                return false;
            }
            await Task.Run(() =>
            {
                if (Settings.DecadeLimit && !ActiveQuery.IgnoreDateLimit && query.StartsWith("get vn ") && !query.Contains("id = "))
                {
                    query = Regex.Replace(query, "\\)", $" and released > \"{DateTime.UtcNow.Year - 10}\")");
                }
                LogToFile(query);
                Conn.Query(query);
            });
            HandleAdvancedMode(query);
            if (Conn.LastResponse.Type == ResponseType.Unknown)
            {
                ChangeAPIStatus(VndbConnection.APIStatus.Error);
                return false;
            }
            while (Conn.LastResponse.Type == ResponseType.Error)
            {
                if (!Conn.LastResponse.Error.ID.Equals("throttled"))
                {
                    WriteError(ActiveQuery.ReplyLabel, errorMessage);
                    ChangeAPIStatus(Conn.Status);
                    return false;
                }
                string fullThrottleMessage = "";
                double minWait = 0;
                await Task.Run(() =>
                {
                    minWait = Math.Min(5 * 60, Conn.LastResponse.Error.Fullwait); //wait 5 minutes
                    string normalWarning = $"Throttled for {Math.Floor(minWait / 60)} mins.";
                    string additionalWarning = "";
                    if (_vnsAdded > 0) additionalWarning += $" Added {_vnsAdded}.";
                    if (_vnsSkipped > 0) additionalWarning += $" Skipped {_vnsSkipped}.";
                    fullThrottleMessage = ActiveQuery.AdditionalMessage ? normalWarning + additionalWarning : normalWarning;
                });
                WriteWarning(ActiveQuery.ReplyLabel, fullThrottleMessage);
                ChangeAPIStatus(VndbConnection.APIStatus.Throttled);
                LogToFile($"Local: {DateTime.Now} - {fullThrottleMessage}");
                if (ActiveQuery.RefreshList)
                {
                    await ReloadListsFromDbAsync();
                    LoadVNListToGui();
                }
                var waitMS = minWait * 1000;
                var wait = Convert.ToInt32(waitMS);
                await Task.Delay(wait);
                ChangeAPIStatus(VndbConnection.APIStatus.Busy);
                await Conn.QueryAsync(query);
                if (AdvancedMode)
                {
                    if (serverR.TextLength > 10000) ClearLog(null, null);
                    serverQ.Text += query + Environment.NewLine;
                    serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
                }
            }
            return true;
        }

        private void HandleAdvancedMode(string query)
        {
            if (AdvancedMode)
            {
                if (serverR.TextLength > 10000) ClearLog(null, null);
                if (serverQ.InvokeRequired)
                    serverQ.Invoke(new MethodInvoker(() => serverQ.Text += query + Environment.NewLine));
                else
                    serverQ.Text += query + Environment.NewLine;
                if (serverR.InvokeRequired)
                    serverR.Invoke(new MethodInvoker(() => serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine));
                else
                    serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
            }
        }

        /// <summary>
        /// Send query through API Connection, don't poste error messages back.
        /// </summary>
        /// <param name="query">Command to be sent</param>
        /// <returns>Returns whether it was successful.</returns>
        internal async Task<bool> TryQueryNoReply(string query)
        {
            if (Conn.Status != VndbConnection.APIStatus.Ready)
            {
                return false;
            }
            await Task.Run(() =>
            {
                LogToFile(query);
                Conn.Query(query);
            });
            if (AdvancedMode)
            {
                if (serverR.TextLength > 10000) ClearLog(null, null);
                serverQ.Text += query + Environment.NewLine;
                serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
            }
            return Conn.LastResponse.Type != ResponseType.Unknown && Conn.LastResponse.Type != ResponseType.Error;
        }

        /// <summary>
        /// Get username from VNDB user ID, returns empty string if error.
        /// </summary>
        internal async Task<string> GetUsernameFromID(int userID)
        {

            var result = await TryQueryNoReply($"get user basic (id={userID})");
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
            var result = await TryQueryNoReply($"get user basic (username=\"{username}\")");
            if (!result)
            {
                ChangeAPIStatus(Conn.Status);
                return -1;
            }
            var response = JsonConvert.DeserializeObject<UserRootItem>(Conn.LastResponse.JsonPayload);
            return response.Items.Any() ? response.Items[0].ID : -1;
        }

        /// <summary>
        /// Get character data about multiple visual novels.
        /// Creates its own SQLite Transactions.
        /// </summary>
        /// <param name="vnIDs">List of VNs</param>
        internal async Task GetCharactersForMultipleVN(int[] vnIDs)
        {
            if (!vnIDs.Any()) return;
            int[] currentArray = vnIDs.Take(APIMaxResults).ToArray();
            string currentArrayString = '[' + string.Join(",", currentArray) + ']';
            string charsForVNQuery = $"get character traits,vns (vn = {currentArrayString}) {{{MaxResultsString}}}";
            var queryResult = await TryQuery(charsForVNQuery, "GetCharactersForMultipleVN Query Error");
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
                queryResult = await TryQuery(charsForVNQuery, "GetCharactersForMultipleVN Query Error");
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
                queryResult = await TryQuery(charsForVNQuery, "GetCharactersForMultipleVN Query Error");
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
                    queryResult = await TryQuery(charsForVNQuery, "GetCharactersForMultipleVN Query Error");
                    if (!queryResult) return;
                    charRoot = JsonConvert.DeserializeObject<CharacterRoot>(Conn.LastResponse.JsonPayload);
                    DBConn.BeginTransaction();
                    foreach (var character in charRoot.Items) DBConn.UpsertSingleCharacter(character);
                    DBConn.EndTransaction();
                    moreResults = charRoot.More;
                }
                done += APIMaxResults;
            }
        }

        private async Task GetLanguagesForProducers(int[] producerIDs)
        {
            if (!producerIDs.Any()) return;
            _vnsAdded = 0;
            var producerList = new List<ListedProducer>();
            foreach (var producerID in producerIDs)
            {
                var result = await GetProducer(producerID, "GetLanguagesForProducers Error",false);
                if (!result.Item1 || result.Item2 == null) continue;
                producerList.Add(result.Item2);
                _vnsAdded++;
                if (producerList.Count > 24)
                {
                    DBConn.BeginTransaction();
                    foreach (var producer in producerList) DBConn.SetProducerLanguage(producer);
                    DBConn.EndTransaction();
                    producerList.Clear();
                }
            }
            DBConn.BeginTransaction();
            foreach (var producer in producerList) DBConn.SetProducerLanguage(producer);
            DBConn.EndTransaction();
        }

        /// <summary>
        /// Searches VNDB for producers by name, independent.
        /// Call StartQuery prior to it and ChangeAPIStatus afterwards.
        /// </summary>
        internal async Task<List<ProducerItem>> AddProducersBySearchedName(string producerName)
        {
            string prodSearchQuery = $"get producer basic (search~\"{producerName}\") {{{MaxResultsString}}}";
            var result = await TryQuery(prodSearchQuery, Resources.ps_query_error);
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
                    await TryQuery(prodSearchMoreQuery, Resources.ps_query_error);
                if (!moreResult) return null;
                var prodMoreRoot = JsonConvert.DeserializeObject<ProducersRoot>(Conn.LastResponse.JsonPayload);
                prodItems.AddRange(prodMoreRoot.Items);
                moreResults = prodMoreRoot.More;
            }
            for (int index = prodItems.Count - 1; index >= 0; index--)
            {
                if (ProducerList.Exists(x => x.Name.Equals(prodItems[index].Name))) prodItems.RemoveAt(index);
            }
            DBConn.BeginTransaction();
            foreach (var producer in prodItems) DBConn.InsertProducer((ListedProducer)producer, true);
            DBConn.EndTransaction();
            return prodItems;

        }

        /// <summary>
        /// Get data about multiple visual novels.
        /// Creates its own SQLite Transactions.
        /// </summary>
        /// <param name="vnIDs">List of visual novel IDs</param>
        /// <param name="updateAll">If false, will skip VNs already fetched</param>
        internal async Task GetMultipleVN(IEnumerable<int> vnIDs, bool updateAll)
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
            var queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error);
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
            var vnsToBeUpserted = new List<(VNItem VN, ProducerItem Producer, VNLanguages Languages)>();
            var producersToBeUpserted = new List<ListedProducer>();
            foreach (var vnItem in vnRoot.Items)
            {
                SaveImage(vnItem);
                var releases = await GetReleases(vnItem.ID, Resources.svn_query_error);
                var mainRelease = releases.FirstOrDefault(item => item.Producers.Exists(x => x.Developer));
                var relProducer = mainRelease?.Producers.FirstOrDefault(p => p.Developer);
                VNLanguages languages = mainRelease != null ? new VNLanguages(mainRelease.Languages, releases.SelectMany(r => r.Languages).ToArray()) : null;
                if (relProducer != null)
                {
                    var gpResult = await GetProducer(relProducer.ID, Resources.gmvn_query_error, updateAll);
                    if (!gpResult.Item1)
                    {
                        ChangeAPIStatus(Conn.Status);
                        return;
                    }
                    if (gpResult.Item2 != null) producersToBeUpserted.Add(gpResult.Item2);
                }
                _vnsAdded++;
                vnsToBeUpserted.Add((vnItem, relProducer, languages));
            }
            DBConn.BeginTransaction();
            vnsToBeUpserted.ForEach(vn => DBConn.UpsertSingleVN(vn, true));
            foreach (var producer in producersToBeUpserted) DBConn.InsertProducer(producer, true);
            DBConn.EndTransaction();
            await GetCharactersForMultipleVN(currentArray);
            int done = APIMaxResults;
            while (done < vnsToGet.Count)
            {
                currentArray = vnsToGet.Skip(done).Take(APIMaxResults).ToArray();
                currentArrayString = '[' + string.Join(",", currentArray) + ']';
                multiVNQuery = $"get vn basic,details,tags,stats (id = {currentArrayString}) {{{MaxResultsString}}}";
                queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error);
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
                vnsToBeUpserted.Clear();
                producersToBeUpserted.Clear();
                foreach (var vnItem in vnRoot.Items)
                {
                    SaveImage(vnItem);
                    var releases = await GetReleases(vnItem.ID, Resources.svn_query_error);
                    var mainRelease = releases.FirstOrDefault(item => item.Producers.Exists(x => x.Developer));
                    var relProducer = mainRelease?.Producers.FirstOrDefault(p => p.Developer);
                    VNLanguages languages = mainRelease != null ? new VNLanguages(mainRelease.Languages, releases.SelectMany(r => r.Languages).ToArray()) : null;
                    if (relProducer != null)
                    {
                        var gpResult = await GetProducer(relProducer.ID, Resources.gmvn_query_error,updateAll);
                        if (!gpResult.Item1)
                        {
                            ChangeAPIStatus(Conn.Status);
                            return;
                        }
                        if (gpResult.Item2 != null) producersToBeUpserted.Add(gpResult.Item2);
                    }
                    _vnsAdded++;
                    vnsToBeUpserted.Add((vnItem, relProducer, languages));
                }
                DBConn.BeginTransaction();
                vnsToBeUpserted.ForEach(vn => DBConn.UpsertSingleVN(vn, true));
                foreach (var producer in producersToBeUpserted) DBConn.InsertProducer(producer, true);
                DBConn.EndTransaction();
                await GetCharactersForMultipleVN(currentArray);
                done += APIMaxResults;
            }
            await ReloadListsFromDbAsync();
        }

        /// <summary>
        /// Update tags, traits and stats of titles.
        /// </summary>
        /// <param name="vnIDs">List of IDs of titles to be updated.</param>
        private async Task UpdateTagsTraitsStats(IEnumerable<int> vnIDs)
        {
            _vnsAdded = 0;
            List<int> vnsToGet = vnIDs.ToList();
            if (!vnsToGet.Any()) return;
            int[] currentArray = vnsToGet.Take(APIMaxResults).ToArray();
            string currentArrayString = '[' + string.Join(",", currentArray) + ']';
            string multiVNQuery = $"get vn tags,stats (id = {currentArrayString}) {{{MaxResultsString}}}";
            var queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error);
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
                DBConn.UpdateVNTagsStats(vnItem);
                _vnsAdded++;
            }
            DBConn.EndTransaction();
            await GetCharactersForMultipleVN(currentArray);
            int done = APIMaxResults;
            while (done < vnsToGet.Count)
            {
                currentArray = vnsToGet.Skip(done).Take(APIMaxResults).ToArray();
                currentArrayString = '[' + string.Join(",", currentArray) + ']';
                multiVNQuery = $"get vn tags,stats (id = {currentArrayString}) {{{MaxResultsString}}}";
                queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error);
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
                    DBConn.UpdateVNTagsStats(vnItem);
                    _vnsAdded++;
                }
                DBConn.EndTransaction();
                await GetCharactersForMultipleVN(currentArray);
                done += APIMaxResults;
            }
            await ReloadListsFromDbAsync();
        }

        /// <summary>
        /// Get Releases for VN.
        /// </summary>
        /// <param name="vnid">ID of VN</param>
        /// <param name="errorMessage">Message to be printed in case of error</param>
        internal async Task<List<ReleaseItem>> GetReleases(int vnid, string errorMessage)
        {
            string developerQuery = $"get release basic,producers (vn =\"{vnid}\") {{{MaxResultsString}}}";
            var releaseResult =
                await TryQuery(developerQuery, errorMessage);
            if (!releaseResult) return null;
            var relInfo = JsonConvert.DeserializeObject<ReleasesRoot>(Conn.LastResponse.JsonPayload);
            List<ReleaseItem> releaseItems = relInfo.Items.Where(rel => !rel.Type.Equals("trial")).ToList();
            if (!releaseItems.Any()) releaseItems = relInfo.Items;
            releaseItems.Sort((x, y) => DateTime.Compare(StringToDate(x.Released), StringToDate(y.Released)));
            return releaseItems;
        }

        /// <summary>
        /// Get Producer from VNDB using Producer ID.
        /// Add producer to database afterwards.
        /// </summary>
        /// <param name="producerID">ID of Producer</param>
        /// <param name="errorMessage">Message to be printed in case of error</param>
        /// <param name="update">Should producer data be fetched even if it is already present in local db?</param>
        /// <returns>Tuple of bool (indicating successful api connection) and ListedProducer (null if none found or already added)</returns>
        internal async Task<(bool, ListedProducer)> GetProducer(int producerID, string errorMessage, bool update)
        {
            if (!update && (producerID == -1 || ProducerList.Exists(p => p.ID == producerID))) return (true, null);
            string producerQuery = $"get producer basic (id={producerID})";
            var producerResult =
                await TryQuery(producerQuery, errorMessage);
            if (!producerResult) return (false, null);
            var root = JsonConvert.DeserializeObject<ProducersRoot>(Conn.LastResponse.JsonPayload);
            List<ProducerItem> producers = root.Items;
            if (!producers.Any()) return (true, null);
            var producer = producers.First();
            return (true, (ListedProducer)producer);
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
                    if (Environment.GetCommandLineArgs().Contains("-debug")) LogToFile($"{ActiveQuery.ActionName} Ended");
                    ActiveQuery.Completed = true;
                    statusLabel.Text = loginString + @"Ready";
                    statusLabel.ForeColor = Color.Black;
                    statusLabel.BackColor = Color.LightGreen;
                    break;
                case VndbConnection.APIStatus.Busy:
                    if (Environment.GetCommandLineArgs().Contains("-debug")) LogToFile($"{ActiveQuery.ActionName} Started");
                    statusLabel.Text = loginString + $@"Busy ({ActiveQuery.ActionName})";
                    statusLabel.ForeColor = Color.Red;
                    statusLabel.BackColor = Color.Khaki;
                    break;
                case VndbConnection.APIStatus.Throttled:
                    if (Environment.GetCommandLineArgs().Contains("-debug")) LogToFile($"{ActiveQuery.ActionName} Throttled");
                    statusLabel.Text = loginString + $@"Throttled ({ActiveQuery.ActionName})";
                    statusLabel.ForeColor = Color.DarkRed;
                    statusLabel.BackColor = Color.Khaki;
                    break;
                case VndbConnection.APIStatus.Error:
                    statusLabel.Text = loginString + $@"Error ({ActiveQuery.ActionName})";
                    statusLabel.ForeColor = Color.Black;
                    statusLabel.BackColor = Color.Red;
                    Conn.Close();
                    break;
                case VndbConnection.APIStatus.Closed:
                    statusLabel.Text = loginString + $@"Closed ({ActiveQuery.ActionName})";
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
        /// <param name="newVoteValue">New vote value</param>
        /// <returns>Returns whether it as successful.</returns>
        internal async Task<bool> ChangeVNStatus(ListedVN vn, ChangeType type, int statusInt, double newVoteValue = -1)
        {
            var hasULStatus = vn.ULStatus > UserlistStatus.Null;
            var hasWLStatus = vn.WLStatus > WishlistStatus.Null;
            var hasVote = vn.Vote > 0;
            string queryString;
            var result = StartQuery(replyText, "Change VN Status", false, false, true);
            if (!result) return false;
            switch (type)
            {
                case ChangeType.UL:
                    queryString = statusInt == -1
                        ? $"set vnlist {vn.VNID}"
                        : $"set vnlist {vn.VNID} {{\"status\":{statusInt}}}";
                    result = await TryQuery(queryString, Resources.cvns_query_error);
                    if (!result) return false;
                    DBConn.BeginTransaction();
                    if (hasWLStatus || hasVote)
                        DBConn.UpdateVNStatus(Settings.UserID, vn.VNID, ChangeType.UL, statusInt, Command.Update);
                    else if (statusInt == -1)
                        DBConn.UpdateVNStatus(Settings.UserID, vn.VNID, ChangeType.UL, statusInt, Command.Delete);
                    else DBConn.UpdateVNStatus(Settings.UserID, vn.VNID, ChangeType.UL, statusInt, Command.New);
                    break;
                case ChangeType.WL:
                    queryString = statusInt == -1
                        ? $"set wishlist {vn.VNID}"
                        : $"set wishlist {vn.VNID} {{\"priority\":{statusInt}}}";
                    result = await TryQuery(queryString, Resources.cvns_query_error);
                    if (!result) return false;
                    DBConn.BeginTransaction();
                    if (hasULStatus || hasVote)
                        DBConn.UpdateVNStatus(Settings.UserID, vn.VNID, ChangeType.WL, statusInt, Command.Update);
                    else if (statusInt == -1)
                        DBConn.UpdateVNStatus(Settings.UserID, vn.VNID, ChangeType.WL, statusInt, Command.Delete);
                    else DBConn.UpdateVNStatus(Settings.UserID, vn.VNID, ChangeType.WL, statusInt, Command.New);
                    break;
                case ChangeType.Vote:
                    int vote = (int)Math.Floor(newVoteValue * 10);
                    queryString = statusInt == -1
                        ? $"set votelist {vn.VNID}"
                        : $"set votelist {vn.VNID} {{\"vote\":{vote}}}";
                    result = await TryQuery(queryString, Resources.cvns_query_error);
                    if (!result) return false;
                    DBConn.BeginTransaction();
                    if (hasULStatus || hasWLStatus)
                        DBConn.UpdateVNStatus(Settings.UserID, vn.VNID, ChangeType.Vote, statusInt, Command.Update, newVoteValue);
                    else if (statusInt == -1)
                        DBConn.UpdateVNStatus(Settings.UserID, vn.VNID, ChangeType.Vote, statusInt, Command.Delete);
                    else DBConn.UpdateVNStatus(Settings.UserID, vn.VNID, ChangeType.Vote, statusInt, Command.New, newVoteValue);
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
            ActiveQuery = new ApiQuery(true,this);
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
            ActiveQuery = new ApiQuery(true,this);
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

        /// <summary>
        /// Check if API Connection is ready, change status accordingly and write error if it isnt ready.
        /// </summary>
        /// <param name="replyLabel">Label where error reply will be printed</param>
        /// <param name="featureName">Name of feature calling the query</param>
        /// <param name="refreshList">Refresh OLV on throttled connection</param>
        /// <param name="additionalMessage">Print Added/Skipped message on throttled connection</param>
        /// <param name="ignoreDateLimit">Ignore 10 year limit (if applicable)</param>
        /// <returns>If connection was ready</returns>
        internal bool StartQuery(Label replyLabel, string featureName, bool refreshList, bool additionalMessage, bool ignoreDateLimit)
        {
            if (!ActiveQuery.Completed)
            {
                WriteError(replyLabel, $"Wait until {ActiveQuery.ActionName} is done.");
                return false;
            }
            ActiveQuery = new ApiQuery(featureName, replyLabel, refreshList, additionalMessage, ignoreDateLimit);
            _vnsAdded = 0;
            _vnsSkipped = 0;
            ChangeAPIStatus(VndbConnection.APIStatus.Busy);
            return true;
        }

        /// <summary>
        /// Contains settings for API Query
        /// </summary>
        public struct ApiQuery
        {
            /// <summary>
            /// Name of action
            /// </summary>
            public readonly string ActionName;
            /// <summary>
            /// Refresh OLV on throttled connection
            /// </summary>
            public readonly bool RefreshList;
            /// <summary>
            /// Print Added/Skipped message on throttled connection
            /// </summary>
            public readonly bool AdditionalMessage;
            /// <summary>
            /// Ignore 10 year limit (if applicable)
            /// </summary>
            public readonly bool IgnoreDateLimit;
            /// <summary>
            /// Query has been completed
            /// </summary>
            public bool Completed;
            /// <summary>
            /// Label where result will be shown.
            /// </summary>
            public readonly Label ReplyLabel;

            /// <summary>
            /// Set API query settings.
            /// </summary>
            /// <param name="actionName">Name of action</param>
            /// <param name="replyLabel">Label where result will be shown</param>
            /// <param name="refreshList">Refresh OLV on throttled connection</param>
            /// <param name="additionalMessage">Print Added/Skipped message on throttled connection</param>
            /// <param name="ignoreDateLimit">Ignore 10 year limit (if applicable)</param>
            public ApiQuery(string actionName, Label replyLabel, bool refreshList, bool additionalMessage, bool ignoreDateLimit)
            {
                ActionName = actionName;
                ReplyLabel = replyLabel;
                RefreshList = refreshList;
                AdditionalMessage = additionalMessage;
                IgnoreDateLimit = ignoreDateLimit;
                Completed = false;
            }

            /// <summary>
            /// Constructor for initializing ActiveQuery.
            /// </summary>
            /// <param name="isStartup">Necessary parameter</param>
            /// <param name="form">Form calling the method</param>
            public ApiQuery(bool isStartup, FormMain form)
            {
                if (!isStartup) throw new ArgumentException("This method should only be used for startup.");
                ActionName = "Startup";
                ReplyLabel = form.replyText;
                RefreshList = false;
                AdditionalMessage = false;
                IgnoreDateLimit = false;
                Completed = true;
            }
        }

    }
}