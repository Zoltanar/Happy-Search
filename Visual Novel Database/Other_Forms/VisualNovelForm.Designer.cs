namespace Happy_Search.Other_Forms
{
    partial class VisualNovelForm
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
            this.pcbImages = new System.Windows.Forms.PictureBox();
            this.vnUpdateLink = new System.Windows.Forms.LinkLabel();
            this.vnTagCB = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.vnDesc = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.vnID = new System.Windows.Forms.LinkLabel();
            this.vnProducer = new System.Windows.Forms.Label();
            this.vnDate = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.vnUserStatus = new System.Windows.Forms.Label();
            this.vnLength = new System.Windows.Forms.Label();
            this.vnPopularity = new System.Windows.Forms.Label();
            this.vnRating = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.vnRelationsCB = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.picturePanel = new System.Windows.Forms.Panel();
            this.vnAnimeCB = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.vnTraitsCB = new System.Windows.Forms.ComboBox();
            this.tagTypeS = new System.Windows.Forms.CheckBox();
            this.tagTypeT = new System.Windows.Forms.CheckBox();
            this.tagTypeC = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.vnName = new System.Windows.Forms.TextBox();
            this.vnKanjiName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pcbImages)).BeginInit();
            this.SuspendLayout();
            // 
            // pcbImages
            // 
            this.pcbImages.BackColor = System.Drawing.Color.Transparent;
            this.pcbImages.Location = new System.Drawing.Point(417, 36);
            this.pcbImages.Name = "pcbImages";
            this.pcbImages.Size = new System.Drawing.Size(256, 394);
            this.pcbImages.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pcbImages.TabIndex = 38;
            this.pcbImages.TabStop = false;
            this.pcbImages.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowLeftclick);
            // 
            // vnUpdateLink
            // 
            this.vnUpdateLink.BackColor = System.Drawing.Color.Transparent;
            this.vnUpdateLink.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.vnUpdateLink.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.vnUpdateLink.Location = new System.Drawing.Point(417, 433);
            this.vnUpdateLink.Name = "vnUpdateLink";
            this.vnUpdateLink.Size = new System.Drawing.Size(256, 13);
            this.vnUpdateLink.TabIndex = 49;
            this.vnUpdateLink.TabStop = true;
            this.vnUpdateLink.Text = "(vnUpdateLink)";
            this.vnUpdateLink.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.vnUpdateLink.VisitedLinkColor = System.Drawing.Color.Blue;
            this.vnUpdateLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.vnUpdateLink_LinkClicked);
            // 
            // vnTagCB
            // 
            this.vnTagCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vnTagCB.FormattingEnabled = true;
            this.vnTagCB.Location = new System.Drawing.Point(75, 46);
            this.vnTagCB.Name = "vnTagCB";
            this.vnTagCB.Size = new System.Drawing.Size(219, 21);
            this.vnTagCB.TabIndex = 48;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.Lavender;
            this.label5.Location = new System.Drawing.Point(9, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 22);
            this.label5.TabIndex = 46;
            this.label5.Text = "Producer";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowLeftclick);
            // 
            // vnDesc
            // 
            this.vnDesc.BackColor = System.Drawing.Color.Black;
            this.vnDesc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.vnDesc.ForeColor = System.Drawing.SystemColors.Window;
            this.vnDesc.Location = new System.Drawing.Point(75, 224);
            this.vnDesc.Name = "vnDesc";
            this.vnDesc.ReadOnly = true;
            this.vnDesc.Size = new System.Drawing.Size(336, 222);
            this.vnDesc.TabIndex = 43;
            this.vnDesc.Text = "";
            this.vnDesc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CloseByEscape);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.Lavender;
            this.label3.Location = new System.Drawing.Point(9, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 22);
            this.label3.TabIndex = 41;
            this.label3.Text = "Tags";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowLeftclick);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Lavender;
            this.label2.Location = new System.Drawing.Point(9, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "Original";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowLeftclick);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Lavender;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Title";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowLeftclick);
            // 
            // vnID
            // 
            this.vnID.BackColor = System.Drawing.Color.Transparent;
            this.vnID.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.vnID.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.vnID.Location = new System.Drawing.Point(537, 7);
            this.vnID.Name = "vnID";
            this.vnID.Size = new System.Drawing.Size(86, 22);
            this.vnID.TabIndex = 52;
            this.vnID.TabStop = true;
            this.vnID.Text = "(vnID)";
            this.vnID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.vnID.VisitedLinkColor = System.Drawing.Color.Blue;
            this.vnID.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.vnID_LinkClicked);
            // 
            // vnProducer
            // 
            this.vnProducer.BackColor = System.Drawing.Color.Transparent;
            this.vnProducer.Location = new System.Drawing.Point(75, 68);
            this.vnProducer.Name = "vnProducer";
            this.vnProducer.Size = new System.Drawing.Size(204, 22);
            this.vnProducer.TabIndex = 54;
            this.vnProducer.Text = "(vnProducer)";
            this.vnProducer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.vnProducer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowLeftclick);
            // 
            // vnDate
            // 
            this.vnDate.BackColor = System.Drawing.Color.Transparent;
            this.vnDate.Location = new System.Drawing.Point(285, 68);
            this.vnDate.Name = "vnDate";
            this.vnDate.Size = new System.Drawing.Size(126, 22);
            this.vnDate.TabIndex = 55;
            this.vnDate.Text = "(vnDate)";
            this.vnDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.vnDate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowLeftclick);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Gainsboro;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Location = new System.Drawing.Point(629, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(44, 23);
            this.button1.TabIndex = 56;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.CloseButton);
            // 
            // vnUserStatus
            // 
            this.vnUserStatus.BackColor = System.Drawing.Color.Transparent;
            this.vnUserStatus.Location = new System.Drawing.Point(75, 197);
            this.vnUserStatus.Name = "vnUserStatus";
            this.vnUserStatus.Size = new System.Drawing.Size(204, 22);
            this.vnUserStatus.TabIndex = 57;
            this.vnUserStatus.Text = "(vnUserStatus)";
            this.vnUserStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // vnLength
            // 
            this.vnLength.BackColor = System.Drawing.Color.Transparent;
            this.vnLength.Location = new System.Drawing.Point(285, 197);
            this.vnLength.Name = "vnLength";
            this.vnLength.Size = new System.Drawing.Size(126, 22);
            this.vnLength.TabIndex = 58;
            this.vnLength.Text = "(vnLength)";
            this.vnLength.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // vnPopularity
            // 
            this.vnPopularity.BackColor = System.Drawing.Color.Transparent;
            this.vnPopularity.Location = new System.Drawing.Point(279, 90);
            this.vnPopularity.Name = "vnPopularity";
            this.vnPopularity.Size = new System.Drawing.Size(132, 22);
            this.vnPopularity.TabIndex = 59;
            this.vnPopularity.Text = "(vnPopularity)";
            this.vnPopularity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // vnRating
            // 
            this.vnRating.BackColor = System.Drawing.Color.Transparent;
            this.vnRating.Location = new System.Drawing.Point(75, 90);
            this.vnRating.Name = "vnRating";
            this.vnRating.Size = new System.Drawing.Size(198, 22);
            this.vnRating.TabIndex = 60;
            this.vnRating.Text = "(vnRating)";
            this.vnRating.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.Lavender;
            this.label4.Location = new System.Drawing.Point(9, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 22);
            this.label4.TabIndex = 61;
            this.label4.Text = "Rating";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // vnRelationsCB
            // 
            this.vnRelationsCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vnRelationsCB.FormattingEnabled = true;
            this.vnRelationsCB.Location = new System.Drawing.Point(75, 115);
            this.vnRelationsCB.Name = "vnRelationsCB";
            this.vnRelationsCB.Size = new System.Drawing.Size(336, 21);
            this.vnRelationsCB.TabIndex = 62;
            this.vnRelationsCB.SelectedIndexChanged += new System.EventHandler(this.RelationSelected);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.Lavender;
            this.label6.Location = new System.Drawing.Point(9, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 22);
            this.label6.TabIndex = 63;
            this.label6.Text = "Relations";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // picturePanel
            // 
            this.picturePanel.AutoScroll = true;
            this.picturePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.picturePanel.Location = new System.Drawing.Point(679, 7);
            this.picturePanel.Name = "picturePanel";
            this.picturePanel.Size = new System.Drawing.Size(695, 439);
            this.picturePanel.TabIndex = 64;
            // 
            // vnAnimeCB
            // 
            this.vnAnimeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vnAnimeCB.FormattingEnabled = true;
            this.vnAnimeCB.Location = new System.Drawing.Point(75, 142);
            this.vnAnimeCB.Name = "vnAnimeCB";
            this.vnAnimeCB.Size = new System.Drawing.Size(336, 21);
            this.vnAnimeCB.TabIndex = 65;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.Color.Lavender;
            this.label7.Location = new System.Drawing.Point(9, 142);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 22);
            this.label7.TabIndex = 66;
            this.label7.Text = "Anime";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.ForeColor = System.Drawing.Color.Lavender;
            this.label8.Location = new System.Drawing.Point(9, 167);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 33);
            this.label8.TabIndex = 67;
            this.label8.Text = "Character Traits";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // vnTraitsCB
            // 
            this.vnTraitsCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vnTraitsCB.FormattingEnabled = true;
            this.vnTraitsCB.Location = new System.Drawing.Point(75, 173);
            this.vnTraitsCB.Name = "vnTraitsCB";
            this.vnTraitsCB.Size = new System.Drawing.Size(336, 21);
            this.vnTraitsCB.TabIndex = 68;
            // 
            // tagTypeS
            // 
            this.tagTypeS.AutoSize = true;
            this.tagTypeS.BackColor = System.Drawing.Color.Transparent;
            this.tagTypeS.Checked = true;
            this.tagTypeS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tagTypeS.Location = new System.Drawing.Point(339, 48);
            this.tagTypeS.Name = "tagTypeS";
            this.tagTypeS.Size = new System.Drawing.Size(33, 17);
            this.tagTypeS.TabIndex = 71;
            this.tagTypeS.Text = "S";
            this.tagTypeS.UseVisualStyleBackColor = false;
            // 
            // tagTypeT
            // 
            this.tagTypeT.AutoSize = true;
            this.tagTypeT.BackColor = System.Drawing.Color.Transparent;
            this.tagTypeT.Checked = true;
            this.tagTypeT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tagTypeT.Location = new System.Drawing.Point(378, 48);
            this.tagTypeT.Name = "tagTypeT";
            this.tagTypeT.Size = new System.Drawing.Size(33, 17);
            this.tagTypeT.TabIndex = 70;
            this.tagTypeT.Text = "T";
            this.tagTypeT.UseVisualStyleBackColor = false;
            // 
            // tagTypeC
            // 
            this.tagTypeC.AutoSize = true;
            this.tagTypeC.BackColor = System.Drawing.Color.Transparent;
            this.tagTypeC.Checked = true;
            this.tagTypeC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tagTypeC.Location = new System.Drawing.Point(300, 48);
            this.tagTypeC.Name = "tagTypeC";
            this.tagTypeC.Size = new System.Drawing.Size(33, 17);
            this.tagTypeC.TabIndex = 69;
            this.tagTypeC.Text = "C";
            this.tagTypeC.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Gainsboro;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button2.Location = new System.Drawing.Point(12, 420);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(57, 23);
            this.button2.TabIndex = 72;
            this.button2.Text = "Refresh";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.RefreshData);
            // 
            // vnName
            // 
            this.vnName.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.vnName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.vnName.ForeColor = System.Drawing.Color.White;
            this.vnName.Location = new System.Drawing.Point(75, 7);
            this.vnName.Name = "vnName";
            this.vnName.ReadOnly = true;
            this.vnName.Size = new System.Drawing.Size(456, 13);
            this.vnName.TabIndex = 73;
            this.vnName.Text = "(vnName)";
            // 
            // vnKanjiName
            // 
            this.vnKanjiName.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.vnKanjiName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.vnKanjiName.ForeColor = System.Drawing.Color.White;
            this.vnKanjiName.Location = new System.Drawing.Point(75, 25);
            this.vnKanjiName.Name = "vnKanjiName";
            this.vnKanjiName.ReadOnly = true;
            this.vnKanjiName.Size = new System.Drawing.Size(336, 13);
            this.vnKanjiName.TabIndex = 74;
            this.vnKanjiName.Text = "(vnKanjiName)";
            // 
            // VisualNovelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.BackgroundImage = global::Happy_Search.Properties.Resources.dark_striped;
            this.ClientSize = new System.Drawing.Size(1386, 455);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.vnKanjiName);
            this.Controls.Add(this.vnName);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tagTypeS);
            this.Controls.Add(this.tagTypeT);
            this.Controls.Add(this.tagTypeC);
            this.Controls.Add(this.vnTraitsCB);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.vnAnimeCB);
            this.Controls.Add(this.picturePanel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.vnRelationsCB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.vnRating);
            this.Controls.Add(this.vnPopularity);
            this.Controls.Add(this.vnLength);
            this.Controls.Add(this.vnUserStatus);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.vnDate);
            this.Controls.Add(this.vnProducer);
            this.Controls.Add(this.vnID);
            this.Controls.Add(this.pcbImages);
            this.Controls.Add(this.vnUpdateLink);
            this.Controls.Add(this.vnTagCB);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.vnDesc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VisualNovelForm";
            this.Text = "(windowTitle) - Happy Search";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CloseByEscape);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowLeftclick);
            ((System.ComponentModel.ISupportInitialize)(this.pcbImages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.PictureBox pcbImages;
        internal System.Windows.Forms.LinkLabel vnUpdateLink;
        internal System.Windows.Forms.ComboBox vnTagCB;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.RichTextBox vnDesc;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.LinkLabel vnID;
        internal System.Windows.Forms.Label vnProducer;
        internal System.Windows.Forms.Label vnDate;
        private System.Windows.Forms.Button button1;
        internal System.Windows.Forms.Label vnUserStatus;
        internal System.Windows.Forms.Label vnLength;
        internal System.Windows.Forms.Label vnPopularity;
        internal System.Windows.Forms.Label vnRating;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.ComboBox vnRelationsCB;
        internal System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel picturePanel;
        internal System.Windows.Forms.ComboBox vnAnimeCB;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.ComboBox vnTraitsCB;
        private System.Windows.Forms.CheckBox tagTypeS;
        private System.Windows.Forms.CheckBox tagTypeT;
        internal System.Windows.Forms.CheckBox tagTypeC;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox vnName;
        private System.Windows.Forms.TextBox vnKanjiName;
    }
}