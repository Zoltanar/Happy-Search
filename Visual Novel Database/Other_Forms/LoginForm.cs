using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Happy_Search.Properties;
using Microsoft.Win32;

namespace Happy_Search.Other_Forms
{
    /// <summary>
    /// Form for User to log into VNDB.org
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly FormMain _parentForm;

        /// <summary>
        /// Load form for user to log into VNDB.org
        /// </summary>
        public LoginForm(FormMain parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
            rememberBox.Checked = Settings.Default.RememberCredentials;
            rememberBox.CheckedChanged += rememberBox_CheckedChanged;
        }

        /// <summary>
        /// Login using just username/user id.
        /// </summary>
        private void LoginButtonClick(object sender, EventArgs e)
        {
            int userID;
            switch (GetIDMethod(out userID))
            {
                case 0:
                    ChangeUserOnly(newId: userID);
                    return;
                case 1:
                    ChangeUserOnly(newUsername: UsernameBox.Text.ToLower());
                    return;
                default:
                    return;
            }
        }

        /// <summary>
        /// Find if user entered ID or username and validate it
        /// </summary>
        /// <param name="id"></param>
        /// <returns>-1 for error, 0 for user ID, 1 for username</returns>
        private int GetIDMethod(out int id)
        {

            id = -1;
            string text = UsernameBox.Text;
            if (text.Equals(""))
            {
                replyLabel.Text = @"Enter username or user ID.";
                return -1;
            }
            //try ID
            bool parse = int.TryParse(text, out id);
            if (parse)
            {
                if (id > 0) return 0;
                replyLabel.Text = @"User ID must be higher than 0.";
                return -1;
            }
            //try username
            var m = new Regex(@"[a-z0-9]+");
            if (m.Match(text).Success) return 1;
            replyLabel.Text = Resources._username_only_alphanumeric;
            return -1;
        }


        /// <summary>
        /// Login using just username/user id.
        /// </summary>
        private void ChangeUserOnly(int newId = -1, string newUsername = "")
        {
            _parentForm.Username = "";
            _parentForm.UserID = -1;
            if (newId > 0)
            {
                _parentForm.UserID = newId;
            }
            else if (!newUsername.Equals(""))
            {
                _parentForm.Username = newUsername;
            }
            else
            {
                replyLabel.Text = @"CID Error";
                return;
            }
            ClearSavedCredentials(null,null);
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Login with credentials (username and password).
        /// </summary>
        private async void LoginWithPasswordButtonClick(object sender, EventArgs e)
        {
            int userID;
            string username;
            _parentForm.CurrentFeatureName = "Login with credentials";
            switch (GetIDMethod(out userID))
            {
                case 0:
                    username = await _parentForm.GetUsernameFromID(userID);
                    if (username.Equals(""))
                    {
                        replyLabel.Text = @"User with that ID was not found.";
                        return;
                    }
                    break;
                case 1:
                    username = UsernameBox.Text.ToLower();
                    break;
                default:
                    return;
            }
            char[] password = PasswordBox.Text.ToCharArray();
            ClearSavedCredentials(null, null);
            if (rememberBox.Checked) FormMain.SaveCredentials(username, password);
            await APILoginWithCredentials(new KeyValuePair<string, char[]>(username, password),userID);
        }


        /// <summary>
        /// Log into VNDB with credentials.
        /// </summary>
        /// <param name="credentials">User's username and password</param>
        /// <param name="userId">VNDB User ID</param>
        private async Task APILoginWithCredentials(KeyValuePair<string, char[]> credentials, int userId)
        {
            if (_parentForm.Conn.Status == VndbConnection.APIStatus.Error)
            {
                replyLabel.Text = @"There was an error opening connection to API server.";
                return;
            }
            _parentForm.Conn.Login(FormMain.ClientName, FormMain.ClientVersion, credentials.Key, credentials.Value);
            _parentForm.ChangeAPIStatus(_parentForm.Conn.Status);
            switch (_parentForm.Conn.LastResponse.Type)
            {
                case ResponseType.Ok:
                    _parentForm.Username = credentials.Key;
                    if (userId < 1) _parentForm.UserID = await _parentForm.GetIDFromUsername(credentials.Key);
                    DialogResult = DialogResult.Yes;
                    return;
                case ResponseType.Error:
                    if (_parentForm.Conn.LastResponse.Error.ID.Equals("loggedin"))
                    {
                        //should never happen
                        replyLabel.ForeColor = Color.LightGreen;
                        replyLabel.Text = Resources.already_logged_in;
                        break;
                    }
                    if (_parentForm.Conn.LastResponse.Error.ID.Equals("auth"))
                    {
                        replyLabel.ForeColor = Color.Red;
                        replyLabel.Text = _parentForm.Conn.LastResponse.Error.Msg;
                        break;
                    }
                    replyLabel.ForeColor = Color.Red;
                    replyLabel.Text = @"ALC Unknown Error 1";
                    break;
                default:
                    replyLabel.ForeColor = Color.Red;
                    replyLabel.Text = @"ALC Unknown Error 2";
                    break;
            }
            if (FormMain.AdvancedMode)
            {
                _parentForm.serverR.Text += _parentForm.Conn.LastResponse.JsonPayload + Environment.NewLine;
            }
        }


        private void rememberBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.RememberCredentials = rememberBox.Checked;
            Settings.Default.Save();
        }

        private void ClearSavedCredentials(object sender, EventArgs e)
        {
            var key = Registry.CurrentUser.OpenSubKey($"SOFTWARE\\{FormMain.ClientName}");
            if (key != null) Registry.CurrentUser.DeleteSubKey($"SOFTWARE\\{FormMain.ClientName}");
        }
    }
}