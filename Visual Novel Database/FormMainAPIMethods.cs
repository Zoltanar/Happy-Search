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

        /// <summary>
        /// Send query through API Connection.
        /// </summary>
        /// <param name="query">Command to be sent</param>
        /// <param name="errorMessage">Message to be printed in case of error</param>
        /// <returns>Returns whether it was successful.</returns>
        internal async Task<bool> TryQuery(string query, string errorMessage)
        {
            var result = await Conn.TryQuery(query, errorMessage, HandleAdvancedMode);
            while (result == VndbConnection.QueryResult.Throttled)
            {
                if (Conn.ActiveQuery.RefreshList)
                {
                    await ReloadListsFromDbAsync();
                    LoadVNListToGui();
                }
                ChangeAPIStatus(Conn.Status);
                await Task.Delay(Conn.ThrottleWaitTime);
                ChangeAPIStatus(VndbConnection.APIStatus.Busy);
                result = await Conn.TryQuery(query, errorMessage, HandleAdvancedMode);
                ChangeAPIStatus(Conn.Status);
            }
            ChangeAPIStatus(Conn.Status);
            return result == VndbConnection.QueryResult.Success;
        }

        private void HandleAdvancedMode(string query)
        {
            if (!AdvancedMode) return;
            if (serverR.TextLength > 10000) ClearLog(null, null);
            serverQ.Invoke(new MethodInvoker(() => serverQ.Text += query + Environment.NewLine));
            serverR.Invoke(new MethodInvoker(() => serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine));
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
            HandleAdvancedMode(query);
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
            LocalDatabase.BeginTransaction();
            foreach (var character in charRoot.Items) LocalDatabase.UpsertSingleCharacter(character);
            LocalDatabase.EndTransaction();
            bool moreResults = charRoot.More;
            int pageNo = 1;
            while (moreResults)
            {
                if (!await HandleMoreResults()) return;
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
                LocalDatabase.BeginTransaction();
                foreach (var character in charRoot.Items) LocalDatabase.UpsertSingleCharacter(character);
                LocalDatabase.EndTransaction();
                moreResults = charRoot.More;
                pageNo = 1;
                while (moreResults)
                {
                    if (!await HandleMoreResults()) return;
                }
                done += APIMaxResults;
            }

            async Task<bool> HandleMoreResults()
            {
                // ReSharper disable AccessToModifiedClosure
                pageNo++;
                charsForVNQuery = $"get character traits,vns (vn = {currentArrayString}) {{{MaxResultsString}, \"page\":{pageNo}}}";
                queryResult = await TryQuery(charsForVNQuery, "GetCharactersForMultipleVN Query Error");
                if (!queryResult) return false;
                charRoot = JsonConvert.DeserializeObject<CharacterRoot>(Conn.LastResponse.JsonPayload);
                LocalDatabase.BeginTransaction();
                foreach (var character in charRoot.Items) LocalDatabase.UpsertSingleCharacter(character);
                LocalDatabase.EndTransaction();
                moreResults = charRoot.More;
                // ReSharper restore AccessToModifiedClosure
                return true;
            }
        }

        private async Task GetLanguagesForProducers(int[] producerIDs)
        {
            if (!producerIDs.Any()) return;
            var producerList = new List<ListedProducer>();
            foreach (var producerID in producerIDs)
            {
                var result = await GetProducer(producerID, "GetLanguagesForProducers Error", false);
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
                if (LocalDatabase.ProducerList.Exists(x => x.Name.Equals(prodItems[index].Name))) prodItems.RemoveAt(index);
            }
            LocalDatabase.BeginTransaction();
            foreach (var producer in prodItems) LocalDatabase.InsertProducer((ListedProducer)producer, true);
            LocalDatabase.EndTransaction();
            return prodItems;

        }

        /// <summary>
        /// Get data about multiple visual novels.
        /// Creates its own SQLite Transactions.
        /// </summary>
        /// <param name="vnIDs">List of visual novel IDs</param>
        /// <param name="updateAll">If false, will skip VNs already fetched</param>
        internal async Task GetMultipleVN(int[] vnIDs, bool updateAll)
        {
            List<int> vnsToGet = new List<int>();
            await Task.Run(() =>
            {
                if (updateAll) vnsToGet = vnIDs.ToList();
                else
                {
                    vnsToGet = vnIDs.Except(LocalDatabase.VNList.Select(x => x.VNID)).ToList();
                    Conn.TitlesSkipped = Conn.TitlesSkipped + vnIDs.Length - vnsToGet.Count;
                }
                vnsToGet.Remove(0);
            });
            if (!vnsToGet.Any()) return;
            int[] currentArray = vnsToGet.Take(APIMaxResults).ToArray();
            string currentArrayString = '[' + string.Join(",", currentArray) + ']';
            string multiVNQuery = $"get vn basic,details,tags,stats (id = {currentArrayString}) {{{MaxResultsString}}}";
            var queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error);
            if (!queryResult) return;
            var vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
            RemoveDeletedVNs(vnRoot, currentArray);
            var vnsToBeUpserted = new List<(VNItem VN, ProducerItem Producer, VNLanguages Languages)>();
            var producersToBeUpserted = new List<ListedProducer>();
            await HandleVNItems(vnRoot.Items);
            LocalDatabase.BeginTransaction();
            vnsToBeUpserted.ForEach(vn => LocalDatabase.UpsertSingleVN(vn, true));
            foreach (var producer in producersToBeUpserted) LocalDatabase.InsertProducer(producer, true);
            LocalDatabase.EndTransaction();
            await GetCharactersForMultipleVN(currentArray);
            int done = APIMaxResults;
            while (done < vnsToGet.Count)
            {
                currentArray = vnsToGet.Skip(done).Take(APIMaxResults).ToArray();
                currentArrayString = '[' + string.Join(",", currentArray) + ']';
                multiVNQuery = $"get vn basic,details,tags,stats (id = {currentArrayString}) {{{MaxResultsString}}}";
                queryResult = await TryQuery(multiVNQuery, Resources.gmvn_query_error);
                if (!queryResult) return;
                vnRoot = JsonConvert.DeserializeObject<VNRoot>(Conn.LastResponse.JsonPayload);
                RemoveDeletedVNs(vnRoot, currentArray);
                vnsToBeUpserted.Clear();
                producersToBeUpserted.Clear();
                await HandleVNItems(vnRoot.Items);
                LocalDatabase.BeginTransaction();
                vnsToBeUpserted.ForEach(vn => LocalDatabase.UpsertSingleVN(vn, true));
                foreach (var producer in producersToBeUpserted) LocalDatabase.InsertProducer(producer, true);
                LocalDatabase.EndTransaction();
                await GetCharactersForMultipleVN(currentArray);
                done += APIMaxResults;
            }
            await ReloadListsFromDbAsync();

            async Task HandleVNItems(List<VNItem> itemList)
            {
                foreach (var vnItem in itemList)
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
                    Conn.TitlesAdded++;
                    vnsToBeUpserted.Add((vnItem, relProducer, languages));
                }
            }
        }

        private void RemoveDeletedVNs(VNRoot root, int[] currentArray)
        {
            if (root.Num >= currentArray.Length) return;
            //some vns were deleted, find which ones and remove them
            IEnumerable<int> deletedVNs = currentArray.Where(currentvn => root.Items.All(receivedvn => receivedvn.ID != currentvn));
            LocalDatabase.BeginTransaction();
            foreach (var deletedVN in deletedVNs) LocalDatabase.RemoveVisualNovel(deletedVN);
            LocalDatabase.EndTransaction();
        }

        /// <summary>
        /// Update tags, traits and stats of titles.
        /// </summary>
        /// <param name="vnIDs">List of IDs of titles to be updated.</param>
        private async Task UpdateTagsTraitsStats(IEnumerable<int> vnIDs)
        {
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
                LocalDatabase.BeginTransaction();
                foreach (var deletedVN in deletedVNs) LocalDatabase.RemoveVisualNovel(deletedVN);
                LocalDatabase.EndTransaction();
            }
            LocalDatabase.BeginTransaction();
            foreach (var vnItem in vnRoot.Items)
            {
                LocalDatabase.UpdateVNTagsStats(vnItem);
                Conn.TitlesAdded++;
            }
            LocalDatabase.EndTransaction();
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
                    LocalDatabase.BeginTransaction();
                    foreach (var deletedVN in deletedVNs) LocalDatabase.RemoveVisualNovel(deletedVN);
                    LocalDatabase.EndTransaction();
                }
                LocalDatabase.BeginTransaction();
                foreach (var vnItem in vnRoot.Items)
                {
                    LocalDatabase.UpdateVNTagsStats(vnItem);
                    Conn.TitlesAdded++;
                }
                LocalDatabase.EndTransaction();
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
            if (!update && (producerID == -1 || LocalDatabase.ProducerList.Exists(p => p.ID == producerID))) return (true, null);
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
        /// Change userlist status, wishlist priority or user vote.
        /// </summary>
        /// <param name="vn">VN which will be changed</param>
        /// <param name="type">What is being changed</param>
        /// <param name="statusInt">The new value</param>
        /// <param name="newVoteValue">New vote value</param>
        /// <returns>Returns whether it as successful.</returns>
        internal async Task<bool> ChangeVNStatus(ListedVN vn, VNDatabase.ChangeType type, int statusInt, double newVoteValue = -1)
        {
            var hasULStatus = vn.ULStatus > UserlistStatus.None;
            var hasWLStatus = vn.WLStatus > WishlistStatus.None;
            var hasVote = vn.Vote > 0;
            string queryString;
            var result = Conn.StartQuery(replyText, "Change VN Status", false, false, true);
            if (!result) return false;
            ChangeAPIStatus(VndbConnection.APIStatus.Busy);
            switch (type)
            {
                case VNDatabase.ChangeType.UL:
                    queryString = statusInt == -1
                        ? $"set vnlist {vn.VNID}"
                        : $"set vnlist {vn.VNID} {{\"status\":{statusInt}}}";
                    result = await TryQuery(queryString, Resources.cvns_query_error);
                    if (!result) return false;
                    LocalDatabase.BeginTransaction();
                    if (hasWLStatus || hasVote)
                        LocalDatabase.UpdateVNStatus(Settings.UserID, vn.VNID, VNDatabase.ChangeType.UL, statusInt, VNDatabase.Command.Update);
                    else if (statusInt == -1)
                        LocalDatabase.UpdateVNStatus(Settings.UserID, vn.VNID, VNDatabase.ChangeType.UL, statusInt, VNDatabase.Command.Delete);
                    else LocalDatabase.UpdateVNStatus(Settings.UserID, vn.VNID, VNDatabase.ChangeType.UL, statusInt, VNDatabase.Command.New);
                    break;
                case VNDatabase.ChangeType.WL:
                    queryString = statusInt == -1
                        ? $"set wishlist {vn.VNID}"
                        : $"set wishlist {vn.VNID} {{\"priority\":{statusInt}}}";
                    result = await TryQuery(queryString, Resources.cvns_query_error);
                    if (!result) return false;
                    LocalDatabase.BeginTransaction();
                    if (hasULStatus || hasVote)
                        LocalDatabase.UpdateVNStatus(Settings.UserID, vn.VNID, VNDatabase.ChangeType.WL, statusInt, VNDatabase.Command.Update);
                    else if (statusInt == -1)
                        LocalDatabase.UpdateVNStatus(Settings.UserID, vn.VNID, VNDatabase.ChangeType.WL, statusInt, VNDatabase.Command.Delete);
                    else LocalDatabase.UpdateVNStatus(Settings.UserID, vn.VNID, VNDatabase.ChangeType.WL, statusInt, VNDatabase.Command.New);
                    break;
                case VNDatabase.ChangeType.Vote:
                    int vote = (int)Math.Floor(newVoteValue * 10);
                    queryString = statusInt == -1
                        ? $"set votelist {vn.VNID}"
                        : $"set votelist {vn.VNID} {{\"vote\":{vote}}}";
                    result = await TryQuery(queryString, Resources.cvns_query_error);
                    if (!result) return false;
                    LocalDatabase.BeginTransaction();
                    if (hasULStatus || hasWLStatus)
                        LocalDatabase.UpdateVNStatus(Settings.UserID, vn.VNID, VNDatabase.ChangeType.Vote, statusInt, VNDatabase.Command.Update, newVoteValue);
                    else if (statusInt == -1)
                        LocalDatabase.UpdateVNStatus(Settings.UserID, vn.VNID, VNDatabase.ChangeType.Vote, statusInt, VNDatabase.Command.Delete);
                    else LocalDatabase.UpdateVNStatus(Settings.UserID, vn.VNID, VNDatabase.ChangeType.Vote, statusInt, VNDatabase.Command.New, newVoteValue);
                    break;
            }
            LocalDatabase.EndTransaction();
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