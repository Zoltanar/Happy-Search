namespace Happy_Search.Other_Forms
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.loginButton = new System.Windows.Forms.Button();
            this.loginWithPasswordButton = new System.Windows.Forms.Button();
            this.UsernameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PasswordBox = new System.Windows.Forms.TextBox();
            this.loginInstructions = new System.Windows.Forms.Label();
            this.replyLabel = new System.Windows.Forms.Label();
            this.rememberBox = new System.Windows.Forms.CheckBox();
            this.clearSaved = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.loginButton.FlatAppearance.BorderSize = 0;
            this.loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginButton.Location = new System.Drawing.Point(184, 11);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 44);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = false;
            this.loginButton.Click += new System.EventHandler(this.LoginButtonClick);
            // 
            // loginWithPasswordButton
            // 
            this.loginWithPasswordButton.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.loginWithPasswordButton.FlatAppearance.BorderSize = 0;
            this.loginWithPasswordButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginWithPasswordButton.Location = new System.Drawing.Point(90, 61);
            this.loginWithPasswordButton.Name = "loginWithPasswordButton";
            this.loginWithPasswordButton.Size = new System.Drawing.Size(168, 22);
            this.loginWithPasswordButton.TabIndex = 1;
            this.loginWithPasswordButton.Text = "Log In With Password";
            this.loginWithPasswordButton.UseVisualStyleBackColor = false;
            this.loginWithPasswordButton.Click += new System.EventHandler(this.LoginWithPasswordButtonClick);
            // 
            // UsernameBox
            // 
            this.UsernameBox.Location = new System.Drawing.Point(90, 9);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Size = new System.Drawing.Size(88, 20);
            this.UsernameBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Username/ID";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Password";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PasswordBox
            // 
            this.PasswordBox.Location = new System.Drawing.Point(90, 35);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.PasswordChar = '●';
            this.PasswordBox.Size = new System.Drawing.Size(88, 20);
            this.PasswordBox.TabIndex = 8;
            // 
            // loginInstructions
            // 
            this.loginInstructions.Location = new System.Drawing.Point(13, 137);
            this.loginInstructions.Name = "loginInstructions";
            this.loginInstructions.Size = new System.Drawing.Size(247, 83);
            this.loginInstructions.TabIndex = 9;
            this.loginInstructions.Text = resources.GetString("loginInstructions.Text");
            // 
            // replyLabel
            // 
            this.replyLabel.Location = new System.Drawing.Point(16, 86);
            this.replyLabel.Name = "replyLabel";
            this.replyLabel.Size = new System.Drawing.Size(162, 51);
            this.replyLabel.TabIndex = 10;
            this.replyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rememberBox
            // 
            this.rememberBox.AutoSize = true;
            this.rememberBox.Location = new System.Drawing.Point(7, 65);
            this.rememberBox.Name = "rememberBox";
            this.rememberBox.Size = new System.Drawing.Size(77, 17);
            this.rememberBox.TabIndex = 11;
            this.rememberBox.Text = "Remember";
            this.rememberBox.UseVisualStyleBackColor = true;
            // 
            // clearSaved
            // 
            this.clearSaved.BackColor = System.Drawing.Color.MistyRose;
            this.clearSaved.FlatAppearance.BorderSize = 0;
            this.clearSaved.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearSaved.Location = new System.Drawing.Point(183, 89);
            this.clearSaved.Name = "clearSaved";
            this.clearSaved.Size = new System.Drawing.Size(75, 22);
            this.clearSaved.TabIndex = 12;
            this.clearSaved.Text = "Clear Saved";
            this.clearSaved.UseVisualStyleBackColor = false;
            this.clearSaved.Click += new System.EventHandler(this.ClearSavedCredentials);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 229);
            this.Controls.Add(this.clearSaved);
            this.Controls.Add(this.rememberBox);
            this.Controls.Add(this.replyLabel);
            this.Controls.Add(this.loginInstructions);
            this.Controls.Add(this.PasswordBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.UsernameBox);
            this.Controls.Add(this.loginWithPasswordButton);
            this.Controls.Add(this.loginButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LoginForm";
            this.Text = "Login - Happy Search";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Button loginWithPasswordButton;
        private System.Windows.Forms.TextBox UsernameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PasswordBox;
        private System.Windows.Forms.Label loginInstructions;
        private System.Windows.Forms.Label replyLabel;
        private System.Windows.Forms.CheckBox rememberBox;
        private System.Windows.Forms.Button clearSaved;
    }
}