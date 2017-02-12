﻿using System;
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
                WriteError(replyLabel, "API Connection isn't ready.");
                return false;
            }
            await Task.Run(() =>
            {
                if (Settings.DecadeLimit && !ignoreDateLimit && query.StartsWith("get vn ") && !query.Contains("id = "))
                {
                    query = Regex.Replace(query, "\\)", $" and released > \"{DateTime.UtcNow.Year - 10}\")");
                }
                LogToFile(query);
                Conn.Query(query);
            });
            if (AdvancedMode)
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
                    string additionalWarning = "";
                    if (_vnsAdded > 0) additionalWarning += $" Added {_vnsAdded}.";
                    if (_vnsSkipped > 0) additionalWarning += $" Skipped {_vnsSkipped}.";
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
                if (AdvancedMode)
                {
                    if (serverR.TextLength > 10000) ClearLog(null, null);
                    serverQ.Text += query + Environment.NewLine;
                    serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
                }
            }
            return true;
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
                ChangeAPIStatus(Conn.Status);
                return new ListedVN();
            }
            var vnItem = vnRoot.Items[0];
            SaveImage(vnItem, true);
            var releases = await GetReleases(vnid, Resources.svn_query_error, updateLink);
            var mainRelease = releases.FirstOrDefault(item => item.Producers.Exists(x => x.Developer));
            var relProducer = mainRelease?.Producers.FirstOrDefault(p => p.Developer);
            var languages = mainRelease != null ? new VNLanguages(mainRelease.Languages, releases.SelectMany(r => r.Languages).ToArray()) : null;
            if (relProducer != null)
            {
                var gpResult = await GetProducer(relProducer.ID, Resources.usvn_query_error, updateLink);
                if (!gpResult.Item1)
                {
                    ChangeAPIStatus(Conn.Status);
                    return null;
                }
                if (gpResult.Item2 != null)
                {
                    DBConn.Open();
                    DBConn.InsertProducer(gpResult.Item2, true);
                    DBConn.Close();
                }
            }
            DBConn.Open();
            DBConn.UpsertSingleVN(vnItem, relProducer, languages, true);
            var vn = DBConn.GetSingleVN(vnid, Settings.UserID);
            DBConn.Close();
            await ReloadListsFromDbAsync();
            WriteText(updateLink, Resources.vn_updated);
            ChangeAPIStatus(Conn.Status);
            return vn;
        }


        /// <summary>
        /// Searches VNDB for producers by name, independent.
        /// Call StartQuery prior to it and ChangeAPIStatus afterwards.
        /// </summary>
        internal async Task<List<ProducerItem>> AddProducersBySearchedName(string producerName, Label replyLabel)
        {
            string prodSearchQuery = $"get producer basic (search~\"{producerName}\") {{{MaxResultsString}}}";
            var result = await TryQuery(prodSearchQuery, Resources.ps_query_error, replyLabel);
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
                    await TryQuery(prodSearchMoreQuery, Resources.ps_query_error, replyLabel);
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
        /// <param name="replyLabel">Label where reply will be printed.</param>
        /// <param name="refreshList">Should OLV be refreshed on throttled connection?</param>
        /// <param name="updateAll">Should VNs be updated even if they;re already in local database?</param>
        private async Task GetMultipleVN(IEnumerable<int> vnIDs, Label replyLabel, bool refreshList, bool updateAll)
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
            var vnsToBeUpserted = new List<Tuple<VNItem, ProducerItem, VNLanguages>>();
            var producersToBeUpserted = new List<ListedProducer>();
            foreach (var vnItem in vnRoot.Items)
            {
                SaveImage(vnItem);
                var releases = await GetReleases(vnItem.ID, Resources.svn_query_error, replyLabel);
                var mainRelease = releases.FirstOrDefault(item => item.Producers.Exists(x => x.Developer));
                var relProducer = mainRelease?.Producers.FirstOrDefault(p => p.Developer);
                VNLanguages languages = mainRelease != null ? new VNLanguages(mainRelease.Languages, releases.SelectMany(r => r.Languages).ToArray()) : null;
                if (relProducer != null)
                {
                    var gpResult = await GetProducer(relProducer.ID, Resources.gmvn_query_error, replyLabel);
                    if (!gpResult.Item1)
                    {
                        ChangeAPIStatus(Conn.Status);
                        return;
                    }
                    if (gpResult.Item2 != null) producersToBeUpserted.Add(gpResult.Item2);
                }
                _vnsAdded++;
                vnsToBeUpserted.Add(new Tuple<VNItem, ProducerItem, VNLanguages>(vnItem, relProducer, languages));
            }
            DBConn.BeginTransaction();
            foreach (Tuple<VNItem, ProducerItem, VNLanguages> vn in vnsToBeUpserted) DBConn.UpsertSingleVN(vn.Item1, vn.Item2, vn.Item3, true);
            foreach (var producer in producersToBeUpserted) DBConn.InsertProducer(producer, true);
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
                vnsToBeUpserted.Clear();
                producersToBeUpserted.Clear();
                foreach (var vnItem in vnRoot.Items)
                {
                    SaveImage(vnItem);
                    var releases = await GetReleases(vnItem.ID, Resources.svn_query_error, replyLabel);
                    var mainRelease = releases.FirstOrDefault(item => item.Producers.Exists(x => x.Developer));
                    var relProducer = mainRelease?.Producers.FirstOrDefault(p => p.Developer);
                    VNLanguages languages = mainRelease != null ? new VNLanguages(mainRelease.Languages, releases.SelectMany(r => r.Languages).ToArray()) : null;
                    if (relProducer != null)
                    {
                        var gpResult = await GetProducer(relProducer.ID, Resources.gmvn_query_error, replyLabel);
                        if (!gpResult.Item1)
                        {
                            ChangeAPIStatus(Conn.Status);
                            return;
                        }
                        if (gpResult.Item2 != null) producersToBeUpserted.Add(gpResult.Item2);
                    }
                    _vnsAdded++;
                    vnsToBeUpserted.Add(new Tuple<VNItem, ProducerItem, VNLanguages>(vnItem, relProducer, languages));
                }
                DBConn.BeginTransaction();
                foreach (Tuple<VNItem, ProducerItem, VNLanguages> vn in vnsToBeUpserted) DBConn.UpsertSingleVN(vn.Item1, vn.Item2, vn.Item3, true);
                foreach (var producer in producersToBeUpserted) DBConn.InsertProducer(producer, true);
                DBConn.EndTransaction();
                await GetCharactersForMultipleVN(currentArray, replyLabel, true, refreshList);
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
                DBConn.UpdateVNTagsStats(vnItem);
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
                    DBConn.UpdateVNTagsStats(vnItem);
                    _vnsAdded++;
                }
                DBConn.EndTransaction();
                await GetCharactersForMultipleVN(currentArray, replyLabel, true, true);
                done += APIMaxResults;
            }
            await ReloadListsFromDbAsync();
        }

        /// <summary>
        /// Get Releases for VN.
        /// </summary>
        /// <param name="vnid">ID of VN</param>
        /// <param name="errorMessage">Message to be printed in case of error</param>
        /// <param name="replyLabel">Label where reply will be printed.</param>
        /// <param name="additionalMessage">Should added/skipped message be printed if connection is throttled?</param>
        /// <param name="refreshList">Should OLV be refreshed on throttled connection?</param>
        internal async Task<List<ReleaseItem>> GetReleases(int vnid, string errorMessage, Label replyLabel, bool additionalMessage = false, bool refreshList = false)
        {
            string developerQuery = $"get release basic,producers (vn =\"{vnid}\") {{{MaxResultsString}}}";
            var releaseResult =
                await TryQuery(developerQuery, errorMessage, replyLabel, additionalMessage, refreshList);
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
        /// <param name="replyLabel">Label where reply will be printed.</param>
        /// <param name="additionalMessage">Should added/skipped message be printed if connection is throttled?</param>
        /// <param name="refreshList">Should OLV be refreshed on throttled connection?</param>
        /// <param name="update">Should producer data be fetched even if it is already present in local db?</param>
        /// <returns>Tuple of bool (indicating successful api connection) and ListedProducer (null if none found or already added)</returns>
        internal async Task<Tuple<bool, ListedProducer>> GetProducer(int producerID, string errorMessage, Label replyLabel, bool additionalMessage = false, bool refreshList = false, bool update = false)
        {
            if (!update && (producerID == -1 || ProducerList.Exists(p => p.ID == producerID))) return new Tuple<bool, ListedProducer>(true, null);
            string producerQuery = $"get producer basic (id={producerID})";
            var producerResult =
                await TryQuery(producerQuery, errorMessage, replyLabel, additionalMessage, refreshList);
            if (!producerResult) return new Tuple<bool, ListedProducer>(false, null);
            var root = JsonConvert.DeserializeObject<ProducersRoot>(Conn.LastResponse.JsonPayload);
            List<ProducerItem> producers = root.Items;
            if (!producers.Any()) return new Tuple<bool, ListedProducer>(true, null);
            var producer = producers.First();
            return new Tuple<bool, ListedProducer>(true, (ListedProducer)producer);
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
                    if (Environment.GetCommandLineArgs().Contains("-debug")) LogToFile($"{CurrentFeatureName} Ended");
                    CurrentFeatureName = "";
                    statusLabel.Text = LoginString + Environment.NewLine + @"Ready";
                    statusLabel.ForeColor = Color.Black;
                    statusLabel.BackColor = Color.LightGreen;
                    break;
                case VndbConnection.APIStatus.Busy:
                    if (Environment.GetCommandLineArgs().Contains("-debug")) LogToFile($"{CurrentFeatureName} Started");
                    statusLabel.Text = LoginString + Environment.NewLine + $@"Busy ({CurrentFeatureName})";
                    statusLabel.ForeColor = Color.Red;
                    statusLabel.BackColor = Color.Khaki;
                    break;
                case VndbConnection.APIStatus.Throttled:
                    if (Environment.GetCommandLineArgs().Contains("-debug")) LogToFile($"{CurrentFeatureName} Throttled");
                    statusLabel.Text = LoginString + Environment.NewLine + $@"Throttled ({CurrentFeatureName})";
                    statusLabel.ForeColor = Color.DarkRed;
                    statusLabel.BackColor = Color.Khaki;
                    break;
                case VndbConnection.APIStatus.Error:
                    statusLabel.Text = LoginString + Environment.NewLine + $@"Error ({CurrentFeatureName})";
                    statusLabel.ForeColor = Color.Black;
                    statusLabel.BackColor = Color.Red;
                    Conn.Close();
                    break;
                case VndbConnection.APIStatus.Closed:
                    statusLabel.Text = LoginString + Environment.NewLine + $@"Closed ({CurrentFeatureName})";
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
                        DBConn.UpdateVNStatus(Settings.UserID, vn.VNID, ChangeType.UL, statusInt, Command.Update);
                    else if (statusInt == -1)
                        DBConn.UpdateVNStatus(Settings.UserID, vn.VNID, ChangeType.UL, statusInt, Command.Delete);
                    else DBConn.UpdateVNStatus(Settings.UserID, vn.VNID, ChangeType.UL, statusInt, Command.New);
                    break;
                case ChangeType.WL:
                    queryString = statusInt == -1
                        ? $"set wishlist {vn.VNID}"
                        : $"set wishlist {vn.VNID} {{\"priority\":{statusInt}}}";
                    result = await TryQuery(queryString, Resources.cvns_query_error, replyText);
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
                    result = await TryQuery(queryString, Resources.cvns_query_error, replyText);
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
            CurrentFeatureName = "Login";
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
            CurrentFeatureName = "Login with credentials";
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