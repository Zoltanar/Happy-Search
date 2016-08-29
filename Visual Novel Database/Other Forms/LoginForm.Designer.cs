namespace Happy_Search
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
            this.setUserIDButton = new System.Windows.Forms.Button();
            this.loginButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.userIDBox = new System.Windows.Forms.TextBox();
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
            // setUserIDButton
            // 
            this.setUserIDButton.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.setUserIDButton.FlatAppearance.BorderSize = 0;
            this.setUserIDButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.setUserIDButton.Location = new System.Drawing.Point(184, 11);
            this.setUserIDButton.Name = "setUserIDButton";
            this.setUserIDButton.Size = new System.Drawing.Size(75, 32);
            this.setUserIDButton.TabIndex = 0;
            this.setUserIDButton.Text = "Set UserID";
            this.setUserIDButton.UseVisualStyleBackColor = false;
            this.setUserIDButton.Click += new System.EventHandler(this.setUserIDButton_Click);
            // 
            // loginButton
            // 
            this.loginButton.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.loginButton.FlatAppearance.BorderSize = 0;
            this.loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginButton.Location = new System.Drawing.Point(184, 49);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 33);
            this.loginButton.TabIndex = 1;
            this.loginButton.Text = "Log In";
            this.loginButton.UseVisualStyleBackColor = false;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "UserID";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userIDBox
            // 
            this.userIDBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.userIDBox.Location = new System.Drawing.Point(78, 11);
            this.userIDBox.Name = "userIDBox";
            this.userIDBox.Size = new System.Drawing.Size(100, 20);
            this.userIDBox.TabIndex = 3;
            // 
            // UsernameBox
            // 
            this.UsernameBox.Location = new System.Drawing.Point(78, 37);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Size = new System.Drawing.Size(100, 20);
            this.UsernameBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Username";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Password";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PasswordBox
            // 
            this.PasswordBox.Location = new System.Drawing.Point(78, 63);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.PasswordChar = '●';
            this.PasswordBox.Size = new System.Drawing.Size(100, 20);
            this.PasswordBox.TabIndex = 8;
            // 
            // loginInstructions
            // 
            this.loginInstructions.Location = new System.Drawing.Point(13, 137);
            this.loginInstructions.Name = "loginInstructions";
            this.loginInstructions.Size = new System.Drawing.Size(247, 83);
            this.loginInstructions.TabIndex = 9;
            // 
            // replyLabel
            // 
            this.replyLabel.Location = new System.Drawing.Point(16, 86);
            this.replyLabel.Name = "replyLabel";
            this.replyLabel.Size = new System.Drawing.Size(162, 48);
            this.replyLabel.TabIndex = 10;
            this.replyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rememberBox
            // 
            this.rememberBox.AutoSize = true;
            this.rememberBox.Location = new System.Drawing.Point(184, 89);
            this.rememberBox.Name = "rememberBox";
            this.rememberBox.Size = new System.Drawing.Size(77, 17);
            this.rememberBox.TabIndex = 11;
            this.rememberBox.Text = "Remember";
            this.rememberBox.UseVisualStyleBackColor = true;
            // 
            // clearSaved
            // 
            this.clearSaved.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.clearSaved.FlatAppearance.BorderSize = 0;
            this.clearSaved.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearSaved.Location = new System.Drawing.Point(183, 112);
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
            this.Controls.Add(this.userIDBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.setUserIDButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button setUserIDButton;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox userIDBox;
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