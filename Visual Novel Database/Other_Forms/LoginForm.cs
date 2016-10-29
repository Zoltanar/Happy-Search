using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
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
        /// <param name="parentForm"></param>
        public LoginForm(FormMain parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
            rememberBox.Checked = Settings.Default.RememberCredentials;
            rememberBox.CheckedChanged += rememberBox_CheckedChanged;
            loginInstructions.Text =
                @"All settings are saved by UserID, this is the ID of your account in vndb.org
You can see this number in your profile page (https://vndb.org/u######)
The UserID is used to get the user's lists.
If you wish to change userlist/wishlist status and/or votes,
you will also need to enter your Username and Password";
        }

        private void setUserIDButton_Click(object sender, EventArgs e)
        {
            int userID;
            if (!int.TryParse(userIDBox.Text, out userID))
            {
                replyLabel.Text = Resources.userid_only_numbers;
                return;
            }
            _parentForm.UserID = userID;
            Settings.Default.URTUpdate = DateTime.MinValue;
            rememberBox.Checked = false;
            if (_parentForm.Conn.LogIn != VndbConnection.LogInStatus.Yes)
            {
                if (_parentForm.Conn.Status != VndbConnection.APIStatus.Closed)
                {
                    _parentForm.Conn.Close();
                }
                _parentForm.Conn.Open();
                if (_parentForm.Conn.Status == VndbConnection.APIStatus.Error)
                {
                    _parentForm.ChangeAPIStatus(_parentForm.Conn.Status, "Login");
                    DialogResult = DialogResult.OK;
                    return;
                }
                _parentForm.APILogin();
            }
            else
            {
                _parentForm.loginReply.ForeColor = Color.LightGreen;
                _parentForm.loginReply.Text = _parentForm.UserID > 0
                    ? $"Connected with ID {_parentForm.UserID}."
                    : "Connected without ID.";
            }
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Login with credentials (username and password).
        /// </summary>
        private void loginButton_Click(object sender, EventArgs e)
        {
            int userID;
            if (!int.TryParse(userIDBox.Text, out userID))
            {
                replyLabel.Text = Resources.userid_only_numbers;
                return;
            }
            var username = UsernameBox.Text;
            char[] password = PasswordBox.Text.ToCharArray();
            var m = new Regex(@"[a-z0-9]+");

            if (!m.Match(username).Success || !password.Any())
            {
                replyLabel.Text = Resources.enter_username_password +
                                  Resources._username_only_alphanumeric;
                FormMain.FadeLabel(replyLabel);
                return;
            }
            FormMain.LogToFile("Login Credentials Validated");
            if (rememberBox.Checked) FormMain.SaveCredentials(username, password);
            //
            _parentForm.UserID = userID;
            if (_parentForm.Conn.Status != VndbConnection.APIStatus.Closed)
            {
                _parentForm.Conn.Close();
            }
            _parentForm.Conn.Open();
            if (_parentForm.Conn.Status == VndbConnection.APIStatus.Error)
            {
                _parentForm.ChangeAPIStatus(_parentForm.Conn.Status,"Login with Credentials");
                DialogResult = DialogResult.OK;
                return;
            }
            _parentForm.APILoginWithCredentials(new KeyValuePair<string, char[]>(username, password));
            DialogResult = DialogResult.OK;
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