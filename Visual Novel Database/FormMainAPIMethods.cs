﻿using System;
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
            //change status to busy until it is solved, if error is returned then status is changed to throttled or ready if the error isn't throttling error
            //if response type is unknown then status is changed to error because it's probably a connection loss and will require reconnecting.
            ChangeAPIStatus(VndbConnection.APIStatus.Busy);
            if (Settings.Default.Limit10Years && !ignoreDateLimit && query.StartsWith("get vn ") && !query.Contains("id = "))
            {
                query = Regex.Replace(query, "\\)", $" and released > \"{DateTime.UtcNow.Year - 10}\")");
            }
            Debug.Print(query);
            await Conn.QueryAsync(query); //request detailed information
            if(serverR.TextLength>10000) ClearLog(null,null);
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
                string additionalWarning = $" Added {_vnsAdded}/skipped {_vnsSkipped} so far...";
                var fullThrottleMessage = additionalMessage ? normalWarning + additionalWarning : normalWarning;
                WriteWarning(replyLabel, fullThrottleMessage);
                ChangeAPIStatus(VndbConnection.APIStatus.Throttled);
                var waitMS = minWait * 1000;
                var wait = Convert.ToInt32(waitMS);
                Debug.Print($"{DateTime.UtcNow} - {fullThrottleMessage}");
                if (refreshList)
                {
                    ReloadLists();
                    RefreshVNList();
                }
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
        /// Get character data about multiple visual novels.
        /// </summary>
        /// <param name="vnIDs">List of VNs</param>
        /// <param name="replyLabel">Where reply will be shown</param>
        internal async Task GetCharactersForMultipleVN(List<int> vnIDs, Label replyLabel)
        {
            ReloadLists();
            if (!vnIDs.Any()) return;
            int[] current25 = vnIDs.Take(25).ToArray();
            string first25 = '[' + string.Join(",", current25) + ']';
            string charsForVNQuery = $"get character traits,vns (vn = {first25}) {{{APIMaxResults}}}";
            var queryResult = await TryQuery(charsForVNQuery, "GetCharactersForMultipleVN Query Error", replyLabel);
            if (!queryResult) return;
            var charRoot = JsonConvert.DeserializeObject<CharacterRoot>(Conn.LastResponse.JsonPayload);
            foreach (var character in charRoot.Items)
            {
                DBConn.Open();
                DBConn.UpsertSingleCharacter(character);
                DBConn.Close();
            }
            bool moreResults = charRoot.More;
            int pageNo = 1;
            while (moreResults)
            {
                pageNo++;
                charsForVNQuery = $"get character traits,vns (vn = {first25}) {{{APIMaxResults}, \"page\":{pageNo}}}";
                queryResult = await TryQuery(charsForVNQuery, "GetCharactersForMultipleVN Query Error", replyLabel);
                if (!queryResult) return;
                charRoot = JsonConvert.DeserializeObject<CharacterRoot>(Conn.LastResponse.JsonPayload);
                foreach (var character in charRoot.Items)
                {
                    DBConn.Open();
                    DBConn.UpsertSingleCharacter(character);
                    DBConn.Close();
                }
                moreResults = charRoot.More;
            }
            int done = 25;
            while (done < vnIDs.Count)
            {
                current25 = vnIDs.Skip(done).Take(25).ToArray();
                string next25 = '[' + string.Join(",", current25) + ']';
                charsForVNQuery = $"get character traits,vns (vn = {next25}) {{{APIMaxResults}}}";
                queryResult = await TryQuery(charsForVNQuery, "GetCharactersForMultipleVN Query Error", replyLabel);
                if (!queryResult) return;
                charRoot = JsonConvert.DeserializeObject<CharacterRoot>(Conn.LastResponse.JsonPayload);
                foreach (var character in charRoot.Items)
                {
                    DBConn.Open();
                    DBConn.UpsertSingleCharacter(character);
                    DBConn.Close();
                }
                moreResults = charRoot.More;
                pageNo = 1;
                while (moreResults)
                {
                    pageNo++;
                    charsForVNQuery = $"get character traits,vns (vn = {next25}) {{{APIMaxResults}, \"page\":{pageNo}}}";
                    queryResult = await TryQuery(charsForVNQuery, "GetCharactersForMultipleVN Query Error", replyLabel);
                    if (!queryResult) return;
                    charRoot = JsonConvert.DeserializeObject<CharacterRoot>(Conn.LastResponse.JsonPayload);
                    foreach (var character in charRoot.Items)
                    {
                        DBConn.Open();
                        DBConn.UpsertSingleCharacter(character);
                        DBConn.Close();
                    }
                    moreResults = charRoot.More;
                }
                done += 25;
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
            ReloadLists();
            string singleVNQuery = $"get vn basic,details,tags,stats (id = {vnid})";
            var result = await TryQuery(singleVNQuery, Resources.usvn_query_error, updateLink);
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
        internal async Task GetSingleVN(int vnid, Label replyLabel, bool forceUpdate = false, bool additionalMessage = false, bool refreshList = false)
        {
            int[] vnIDList = _vnList.Select(x => x.VNID).ToArray();
            if (forceUpdate == false && vnIDList.Contains(vnid))
            {
                _vnsSkipped++;
                return;
            }
            string singleVNQuery = $"get vn basic,details,tags,stats (id = {vnid})";
            var result =
                await TryQuery(singleVNQuery, Resources.svn_query_error, replyLabel, additionalMessage, refreshList);
            if (!result) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            if (vnRoot.Num == 0)
            {
                //this vn has been deleted (or something along those lines)
                DBConn.Open();
                DBConn.RemoveVisualNovel(vnid);
                DBConn.Close();
                return;
            }
            var vnItem = vnRoot.Items[0];
            SaveImage(vnItem);
            var relProducer = await GetDeveloper(vnid, Resources.svn_query_error, replyLabel, additionalMessage, refreshList);
            await GetProducer(relProducer, Resources.svn_query_error, replyLabel, additionalMessage, refreshList);
            await GetCharactersForMultipleVN(new List<int>(vnid), replyLabel);
            DBConn.Open();
            DBConn.UpsertSingleVN(vnItem, relProducer, false);
            DBConn.Close();
            _vnsAdded++;
        }

        /// <summary>
        /// Get data about multiple visual novels.
        /// </summary>
        /// <param name="vnIDs">List of visual novel IDs</param>
        /// <param name="replyLabel">Label where reply will be printed.</param>
        /// <param name="refreshList">Should OLV be refreshed on throttled connection?</param>
        /// <param name="updateAll">Should VNs be updated even if they;re already in local database?</param>
        internal async Task GetMultipleVN(IEnumerable<int> vnIDs, Label replyLabel, bool refreshList = false, bool updateAll = false)
        {
            ReloadLists();
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
            int[] current25 = vnsToGet.Take(25).ToArray();
            string first25 = '[' + string.Join(",", current25) + ']';
            string multiVNQuery = $"get vn basic,details,tags,stats (id = {first25}) {{{APIMaxResults}}}";
            var queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error, replyLabel, true, refreshList);
            if (!queryResult) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            if (vnRoot.Num < current25.Length)
            {
                //some vns were deleted, find which ones and remove them
                var root = vnRoot;
                IEnumerable<int> deletedVNs = current25.Where(currentvn => root.Items.All(receivedvn => receivedvn.ID != currentvn));
                DBConn.Open();
                foreach (var deletedVN in deletedVNs) DBConn.RemoveVisualNovel(deletedVN);
                DBConn.Close();
            }
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
            await GetCharactersForMultipleVN(current25.ToList(), replyLabel);
            int done = 25;
            while (done < vnsToGet.Count)
            {
                current25 = vnsToGet.Skip(done).Take(25).ToArray();
                string next25 = '[' + string.Join(",", current25) + ']';
                multiVNQuery = $"get vn basic,details,tags (id = {next25}) {{{APIMaxResults}}}";
                queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error, replyLabel, true, refreshList);
                if (!queryResult) return;
                vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                if (vnRoot.Num < current25.Length)
                {
                    //some vns were deleted, find which ones and remove them
                    var root = vnRoot;
                    IEnumerable<int> deletedVNs = current25.Where(currentvn => root.Items.All(receivedvn => receivedvn.ID != currentvn));
                    DBConn.Open();
                    foreach (var deletedVN in deletedVNs) DBConn.RemoveVisualNovel(deletedVN);
                    DBConn.Close();
                }
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
                await GetCharactersForMultipleVN(current25.ToList(), replyLabel);
                done += 25;
            }
        }

        /// <summary>
        /// Update titles to include all fields in latest version of Happy Search.
        /// </summary>
        /// <param name="vnIDs">List of visual novel IDs</param>
        internal async Task UpdateTitlesToLatestVersion(IEnumerable<int> vnIDs)
        {
            var replyLabel = userListReply;
            ReloadLists();
            _vnsAdded = 0;
            List<int> vnsToGet = vnIDs.ToList();
            if (!vnsToGet.Any()) return;
            int[] current25 = vnsToGet.Take(25).ToArray();
            string first25 = '[' + string.Join(",", current25) + ']';
            string multiVNQuery = $"get vn stats (id = {first25}) {{{APIMaxResults}}}";
            var queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error, replyLabel, true, true);
            if (!queryResult) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            if (vnRoot.Num < current25.Length)
            {
                //some vns were deleted, find which ones and remove them
                var root = vnRoot;
                IEnumerable<int> deletedVNs = current25.Where(currentvn => root.Items.All(receivedvn => receivedvn.ID != currentvn));
                DBConn.Open();
                foreach (var deletedVN in deletedVNs) DBConn.RemoveVisualNovel(deletedVN);
                DBConn.Close();
            }
            DBConn.Open();
            foreach (var vnItem in vnRoot.Items)
            {
                DBConn.UpdateVNToLatestVersion(vnItem);
                _vnsAdded++;
            }
            DBConn.Close();
            int done = 25;
            while (done < vnsToGet.Count)
            {
                current25 = vnsToGet.Skip(done).Take(25).ToArray();
                string next25 = '[' + string.Join(",", current25) + ']';
                multiVNQuery = $"get vn stats (id = {next25}) {{{APIMaxResults}}}";
                queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error, replyLabel, true, true);
                if (!queryResult) return;
                vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                if (vnRoot.Num < current25.Length)
                {
                    //some vns were deleted, find which ones and remove them
                    var root = vnRoot;
                    IEnumerable<int> deletedVNs = current25.Where(currentvn => root.Items.All(receivedvn => receivedvn.ID != currentvn));
                    DBConn.Open();
                    foreach (var deletedVN in deletedVNs) DBConn.RemoveVisualNovel(deletedVN);
                    DBConn.Close();
                }
                DBConn.Open();
                foreach (var vnItem in vnRoot.Items)
                {
                    DBConn.UpdateVNToLatestVersion(vnItem);
                    _vnsAdded++;
                }
                DBConn.Close();
                done += 25;
            }
        }

        /// <summary>
        /// Update tags of titles that haven't been updated in over 7 days.
        /// </summary>
        /// <param name="vnIDs">List of visual novel IDs</param>
        internal async Task UpdateTitleTags(IEnumerable<int> vnIDs)
        {
            var replyLabel = userListReply;
            ReloadLists();
            _vnsAdded = 0;
            List<int> vnsToGet = vnIDs.ToList();
            if (!vnsToGet.Any()) return;
            int[] current25 = vnsToGet.Take(25).ToArray();
            string first25 = '[' + string.Join(",", current25) + ']';
            string multiVNQuery = $"get vn tags (id = {first25}) {{{APIMaxResults}}}";
            var queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error, replyLabel, true, true);
            if (!queryResult) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            if (vnRoot.Num < current25.Length)
            {
                //some vns were deleted, find which ones and remove them
                var root = vnRoot;
                IEnumerable<int> deletedVNs = current25.Where(currentvn => root.Items.All(receivedvn => receivedvn.ID != currentvn));
                DBConn.Open();
                foreach (var deletedVN in deletedVNs) DBConn.RemoveVisualNovel(deletedVN);
                DBConn.Close();
            }
            DBConn.Open();
            foreach (var vnItem in vnRoot.Items)
            {
                DBConn.UpdateVNTags(vnItem);
                _vnsAdded++;
            }
            DBConn.Close();
            int done = 25;
            while (done < vnsToGet.Count)
            {
                current25 = vnsToGet.Skip(done).Take(25).ToArray();
                string next25 = '[' + string.Join(",", current25) + ']';
                multiVNQuery = $"get vn tags (id = {next25}) {{{APIMaxResults}}}";
                queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error, replyLabel, true, true);
                if (!queryResult) return;
                vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                if (vnRoot.Num < current25.Length)
                {
                    //some vns were deleted, find which ones and remove them
                    var root = vnRoot;
                    IEnumerable<int> deletedVNs = current25.Where(currentvn => root.Items.All(receivedvn => receivedvn.ID != currentvn));
                    DBConn.Open();
                    foreach (var deletedVN in deletedVNs) DBConn.RemoveVisualNovel(deletedVN);
                    DBConn.Close();
                }
                DBConn.Open();
                foreach (var vnItem in vnRoot.Items)
                {
                    DBConn.UpdateVNTags(vnItem);
                    _vnsAdded++;
                }
                DBConn.Close();
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
            DBConn.InsertProducer((ListedProducer)producer);
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