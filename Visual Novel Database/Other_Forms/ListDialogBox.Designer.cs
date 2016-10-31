namespace Happy_Search.Other_Forms
{
    partial class ListDialogBox
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
            this.listViewLabel = new System.Windows.Forms.Label();
            this.listView = new System.Windows.Forms.ListView();
            this.removeAllButton = new System.Windows.Forms.Button();
            this.removeSelectedButton = new System.Windows.Forms.Button();
            this.inputBox = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.confirmButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.replyLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listViewLabel
            // 
            this.listViewLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.listViewLabel.Location = new System.Drawing.Point(12, 9);
            this.listViewLabel.Name = "listViewLabel";
            this.listViewLabel.Size = new System.Drawing.Size(260, 23);
            this.listViewLabel.TabIndex = 1;
            this.listViewLabel.Text = "(listViewLabel)";
            this.listViewLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listView
            // 
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(15, 35);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(257, 97);
            this.listView.TabIndex = 2;
            this.listView.UseCompatibleStateImageBehavior = false;
            // 
            // removeAllButton
            // 
            this.removeAllButton.BackColor = System.Drawing.Color.MistyRose;
            this.removeAllButton.FlatAppearance.BorderSize = 0;
            this.removeAllButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeAllButton.Location = new System.Drawing.Point(187, 138);
            this.removeAllButton.Name = "removeAllButton";
            this.removeAllButton.Size = new System.Drawing.Size(85, 23);
            this.removeAllButton.TabIndex = 3;
            this.removeAllButton.Text = "Remove All";
            this.removeAllButton.UseVisualStyleBackColor = false;
            this.removeAllButton.Click += new System.EventHandler(this.removeAllButton_Click);
            // 
            // removeSelectedButton
            // 
            this.removeSelectedButton.BackColor = System.Drawing.Color.MistyRose;
            this.removeSelectedButton.FlatAppearance.BorderSize = 0;
            this.removeSelectedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeSelectedButton.Location = new System.Drawing.Point(15, 138);
            this.removeSelectedButton.Name = "removeSelectedButton";
            this.removeSelectedButton.Size = new System.Drawing.Size(166, 23);
            this.removeSelectedButton.TabIndex = 4;
            this.removeSelectedButton.Text = "Remove Selected";
            this.removeSelectedButton.UseVisualStyleBackColor = false;
            this.removeSelectedButton.Click += new System.EventHandler(this.removeSelectedButton_Click);
            // 
            // inputBox
            // 
            this.inputBox.Location = new System.Drawing.Point(15, 169);
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(166, 20);
            this.inputBox.TabIndex = 5;
            this.inputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inputBox_KeyPress);
            // 
            // addButton
            // 
            this.addButton.BackColor = System.Drawing.Color.SteelBlue;
            this.addButton.FlatAppearance.BorderSize = 0;
            this.addButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.addButton.Location = new System.Drawing.Point(187, 167);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(85, 23);
            this.addButton.TabIndex = 6;
            this.addButton.Text = "Add Item";
            this.addButton.UseVisualStyleBackColor = false;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // confirmButton
            // 
            this.confirmButton.BackColor = System.Drawing.Color.SteelBlue;
            this.confirmButton.FlatAppearance.BorderSize = 0;
            this.confirmButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.confirmButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.confirmButton.Location = new System.Drawing.Point(187, 226);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(85, 23);
            this.confirmButton.TabIndex = 7;
            this.confirmButton.Text = "Save Changes";
            this.confirmButton.UseVisualStyleBackColor = false;
            this.confirmButton.Click += new System.EventHandler(this.confirmButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.MistyRose;
            this.cancelButton.FlatAppearance.BorderSize = 0;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Location = new System.Drawing.Point(96, 226);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(85, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // replyLabel
            // 
            this.replyLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.replyLabel.Location = new System.Drawing.Point(15, 196);
            this.replyLabel.Name = "replyLabel";
            this.replyLabel.Size = new System.Drawing.Size(257, 23);
            this.replyLabel.TabIndex = 9;
            this.replyLabel.Text = "(replyLabel)";
            this.replyLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ListDialogBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.replyLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.inputBox);
            this.Controls.Add(this.removeSelectedButton);
            this.Controls.Add(this.removeAllButton);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.listViewLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ListDialogBox";
            this.Text = "(windowTitle)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label listViewLabel;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.Button removeAllButton;
        private System.Windows.Forms.Button removeSelectedButton;
        private System.Windows.Forms.TextBox inputBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button confirmButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label replyLabel;
    }
}