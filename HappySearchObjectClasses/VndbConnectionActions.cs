using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Happy_Apps_Core.StaticHelpers;

namespace Happy_Apps_Core
{
    partial class VndbConnection
    {
        public ApiQuery ActiveQuery;

        /// <summary>
        /// Count of titles added in last query.
        /// </summary>
        public int TitlesAdded { get; set; }
        /// <summary>
        /// Count of titles skipped in last query.
        /// </summary>
        public int TitlesSkipped { get; set; }


        /// <summary>
        /// Check if API Connection is ready, change status accordingly and write error if it isnt ready.
        /// </summary>
        /// <param name="replyLabel">Label where error reply will be printed</param>
        /// <param name="featureName">Name of feature calling the query</param>
        /// <param name="refreshList">Refresh OLV on throttled connection</param>
        /// <param name="additionalMessage">Print Added/Skipped message on throttled connection</param>
        /// <param name="ignoreDateLimit">Ignore 10 year limit (if applicable)</param>
        /// <returns>If connection was ready</returns>
        public bool StartQuery(Label replyLabel, string featureName, bool refreshList, bool additionalMessage, bool ignoreDateLimit)
        {
            if (!ActiveQuery.Completed)
            {
                WriteError(replyLabel, $"Wait until {ActiveQuery.ActionName} is done.");
                return false;
            }
            ActiveQuery = new ApiQuery(featureName, replyLabel, refreshList, additionalMessage, ignoreDateLimit);
            TitlesAdded = 0;
            TitlesSkipped = 0;
            return true;
        }

        public enum QueryResult { Fail, Success, Throttled}
        
        public int ThrottleWaitTime;


        /// <summary>
        /// Send query through API Connection.
        /// </summary>
        /// <param name="query">Command to be sent</param>
        /// <param name="errorMessage">Message to be printed in case of error</param>
        /// <param name="advancedAction">Method which operates on the query string</param>
        /// <returns>Returns whether it was successful.</returns>
        public async Task<QueryResult> TryQuery(string query, string errorMessage, Action<string> advancedAction)
        {
            if (Status != APIStatus.Ready)
            {
                WriteError(ActiveQuery.ReplyLabel, "API Connection isn't ready.");
                return QueryResult.Fail;
            }
            await Task.Run(() =>
            {
                if (Settings.DecadeLimit && !ActiveQuery.IgnoreDateLimit && query.StartsWith("get vn ") && !query.Contains("id = "))
                {
                    query = Regex.Replace(query, "\\)", $" and released > \"{DateTime.UtcNow.Year - 10}\")");
                }
                LogToFile(query);
                Query(query);
            });
            advancedAction(query);
            if (LastResponse.Type == ResponseType.Unknown)
            {
                return QueryResult.Fail;
            }
            while (LastResponse.Type == ResponseType.Error)
            {
                if (!LastResponse.Error.ID.Equals("throttled"))
                {
                    WriteError(ActiveQuery.ReplyLabel, errorMessage);
                    return QueryResult.Fail;
                }
                string fullThrottleMessage = "";
                double minWait = 0;
                await Task.Run(() =>
                {
                    minWait = Math.Min(5 * 60, LastResponse.Error.Fullwait); //wait 5 minutes
                    string normalWarning = $"Throttled for {Math.Floor(minWait / 60)} mins.";
                    string additionalWarning = "";
                    if (TitlesAdded > 0) additionalWarning += $" Added {TitlesAdded}.";
                    if (TitlesSkipped > 0) additionalWarning += $" Skipped {TitlesSkipped}.";
                    fullThrottleMessage = ActiveQuery.AdditionalMessage ? normalWarning + additionalWarning : normalWarning;
                });
                WriteWarning(ActiveQuery.ReplyLabel, fullThrottleMessage);
                LogToFile($"Local: {DateTime.Now} - {fullThrottleMessage}");
                _throttledQuery = query;
                var waitMS = minWait * 1000;
                ThrottleWaitTime = Convert.ToInt32(waitMS);
                return QueryResult.Throttled;
            }
            return QueryResult.Success;
        }
    }
}
