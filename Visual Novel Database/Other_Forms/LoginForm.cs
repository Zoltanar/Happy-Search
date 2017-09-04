using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Happy_Search.Properties;
using Happy_Apps_Core;
using Microsoft.Win32;
using static Happy_Apps_Core.StaticHelpers;

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
            rememberBox.Checked = FormMain.GuiSettings.RememberPassword;
            rememberBox.CheckedChanged += rememberBox_CheckedChanged;
        }

        /// <summary>
        /// Login with User ID
        /// </summary>
        private async void LoginWithIDClick(object sender, EventArgs e)
        {

            if (!ValidateCredentials(false)) return;
            await APILoginWithUserID(int.Parse(UsernameBox.Text));
        }

        /// <summary>
        /// Login with credentials (username and password).
        /// </summary>
        private async void LoginWithPasswordButtonClick(object sender, EventArgs e)
        {
            _parentForm.Conn.ActiveQuery = new ApiQuery(true, _parentForm.replyText);
            if (!ValidateCredentials(true)) return;
            char[] password = PasswordBox.Text.ToCharArray();
            ClearSavedCredentials(null, null);
            if (rememberBox.Checked) FormMain.SaveCredentials(password);
            await APILoginWithCredentials(new KeyValuePair<string, char[]>(UsernameBox.Text, password));
        }

        /// <summary>
        /// Find if user entered ID/username and optionally password, and validate it
        /// </summary>
        private bool ValidateCredentials(bool withPassword)
        {
            string usernameOrId = UsernameBox.Text;
            string password = PasswordBox.Text;
            if (string.IsNullOrWhiteSpace(usernameOrId))
            {
                replyLabel.Text = withPassword ? "Enter Username." : "Enter User ID.";
                return false;
            }
            if (withPassword)
            {
                //try username
                var m = new Regex(@"[a-z0-9]+");
                if (!m.Match(usernameOrId).Success)
                {
                    replyLabel.Text = Resources._username_only_alphanumeric;
                    return false;
                }
                if (string.IsNullOrWhiteSpace(password))
                {
                    replyLabel.Text = "Enter Password.";
                    return false;
                }
            }
            else
            {
                //try ID
                bool parse = int.TryParse(usernameOrId, out int id);
                if (!parse)
                {
                    replyLabel.Text = "UserID must be a number.";
                    return false;
                }
                if (id > 0) return true;
                replyLabel.Text = @"User ID must be higher than 0.";
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Log into VNDB with credentials.
        /// </summary>
        /// <param name="credentials">User's username and password</param>
        private async Task APILoginWithCredentials(KeyValuePair<string, char[]> credentials)
        {
            if (_parentForm.Conn.Status == VndbConnection.APIStatus.Error)
            {
                replyLabel.Text = @"There was an error opening connection to API server.";
                return;
            }
            _parentForm.Conn.Login(ClientName, ClientVersion, credentials.Key, credentials.Value);
            _parentForm.ChangeAPIStatus(_parentForm.Conn.Status);
            switch (_parentForm.Conn.LastResponse.Type)
            {
                case ResponseType.Ok:
                    Settings.Username = credentials.Key;
                    Settings.UserID = await _parentForm.GetIDFromUsername(credentials.Key);
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
        
        /// <summary>
        /// Log into VNDB with user id.
        /// </summary>
        /// <param name="userId">VNDB User ID</param>
        private async Task APILoginWithUserID(int userId)
        {
            if (_parentForm.Conn.Status == VndbConnection.APIStatus.Error)
            {
                replyLabel.Text = @"There was an error opening connection to API server.";
                return;
            }
            Settings.UserID = userId;
            _parentForm.Conn.Login(ClientName, ClientVersion);
            _parentForm.ChangeAPIStatus(_parentForm.Conn.Status);
            switch (_parentForm.Conn.LastResponse.Type)
            {
                case ResponseType.Ok:
                    Settings.Username = await _parentForm.GetUsernameFromID(userId);
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
            FormMain.GuiSettings.RememberPassword = rememberBox.Checked;
            FormMain.GuiSettings.Save();
        }

        private void ClearSavedCredentials(object sender, EventArgs e)
        {
            var key = Registry.CurrentUser.OpenSubKey($"SOFTWARE\\{ClientName}");
            if (key != null) Registry.CurrentUser.DeleteSubKey($"SOFTWARE\\{ClientName}");
        }
    }
}