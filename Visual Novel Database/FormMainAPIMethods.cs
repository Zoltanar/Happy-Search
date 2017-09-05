using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Happy_Search.Properties;
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
            _filtersTab?.PopulateLanguages(false);
            LoadVNListToGui();
        }

        /// <summary>
        /// Change text and color of API Status label based on status.
        /// </summary>
        /// <param name="apiStatus">Status of API Connection</param>
        internal void ChangeAPIStatus(VndbConnection.APIStatus apiStatus)
        {
            var loginString = TruncateString(_loginString, 28) + Environment.NewLine;
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
        private void APILogin()
        {
            Conn.ActiveQuery = new ApiQuery(true, replyText);
            Conn.Login(ClientName, ClientVersion);
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
                        _loginString = Resources.already_logged_in;
                        break;
                    }
                    _loginString = Resources.connection_failed;
                    break;
                default:
                    _loginString = Resources.login_unknown_error;
                    break;
            }
            ChangeAPIStatus(Conn.Status);
            if (AdvancedMode) serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
        }

        /// <summary>
        /// Log into VNDB with credentials.
        /// </summary>
        /// <param name="password">User's password</param>
        private void APILoginWithPassword(char[] password)
        {
            Conn.ActiveQuery = new ApiQuery(true, replyText);
            Conn.Login(ClientName, ClientVersion, Settings.Username, password);
            switch (Conn.LastResponse.Type)
            {
                case ResponseType.Ok:
                    _loginString = $@"Logged in as {Settings.Username}.";
                    ChangeAPIStatus(Conn.Status);
                    return;
                case ResponseType.Error:
                    if (Conn.LastResponse.Error.ID.Equals("loggedin"))
                    {
                        //should never happen
                        _loginString = Resources.already_logged_in;
                        break;
                    }
                    if (Conn.LastResponse.Error.ID.Equals("auth"))
                    {
                        _loginString = Conn.LastResponse.Error.Msg;
                        break;
                    }
                    _loginString = Resources.connection_failed;
                    break;
                default:
                    _loginString = Resources.login_unknown_error;
                    break;
            }
            ChangeAPIStatus(Conn.Status);
            if (AdvancedMode) serverR.Text += Conn.LastResponse.JsonPayload + Environment.NewLine;
        }


    }
}