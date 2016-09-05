namespace Happy_Search
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
            this.vnName = new System.Windows.Forms.Label();
            this.vnID = new System.Windows.Forms.LinkLabel();
            this.vnKanjiName = new System.Windows.Forms.Label();
            this.vnProducer = new System.Windows.Forms.Label();
            this.vnDate = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.vnUserStatus = new System.Windows.Forms.Label();
            this.vnLength = new System.Windows.Forms.Label();
            this.vnPopularity = new System.Windows.Forms.Label();
            this.vnRating = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pcbImages)).BeginInit();
            this.SuspendLayout();
            // 
            // pcbImages
            // 
            this.pcbImages.BackColor = System.Drawing.Color.Transparent;
            this.pcbImages.Location = new System.Drawing.Point(417, 32);
            this.pcbImages.Name = "pcbImages";
            this.pcbImages.Size = new System.Drawing.Size(256, 398);
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
            this.vnTagCB.Location = new System.Drawing.Point(75, 51);
            this.vnTagCB.Name = "vnTagCB";
            this.vnTagCB.Size = new System.Drawing.Size(336, 21);
            this.vnTagCB.TabIndex = 48;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.Lavender;
            this.label5.Location = new System.Drawing.Point(9, 73);
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
            this.vnDesc.Location = new System.Drawing.Point(75, 192);
            this.vnDesc.Name = "vnDesc";
            this.vnDesc.ReadOnly = true;
            this.vnDesc.Size = new System.Drawing.Size(336, 238);
            this.vnDesc.TabIndex = 43;
            this.vnDesc.Text = "";
            this.vnDesc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CloseByEscape);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.Lavender;
            this.label3.Location = new System.Drawing.Point(9, 51);
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
            this.label2.Location = new System.Drawing.Point(9, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 22);
            this.label2.TabIndex = 40;
            this.label2.Text = "Original";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowLeftclick);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Lavender;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 22);
            this.label1.TabIndex = 39;
            this.label1.Text = "Title";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowLeftclick);
            // 
            // vnName
            // 
            this.vnName.BackColor = System.Drawing.Color.Transparent;
            this.vnName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.vnName.Location = new System.Drawing.Point(75, 7);
            this.vnName.Name = "vnName";
            this.vnName.Size = new System.Drawing.Size(456, 22);
            this.vnName.TabIndex = 51;
            this.vnName.Text = "(vnName)";
            this.vnName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.vnName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowLeftclick);
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
            // vnKanjiName
            // 
            this.vnKanjiName.BackColor = System.Drawing.Color.Transparent;
            this.vnKanjiName.Location = new System.Drawing.Point(75, 29);
            this.vnKanjiName.Name = "vnKanjiName";
            this.vnKanjiName.Size = new System.Drawing.Size(336, 22);
            this.vnKanjiName.TabIndex = 53;
            this.vnKanjiName.Text = "(vnKanjiName)";
            this.vnKanjiName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.vnKanjiName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowLeftclick);
            // 
            // vnProducer
            // 
            this.vnProducer.BackColor = System.Drawing.Color.Transparent;
            this.vnProducer.Location = new System.Drawing.Point(75, 73);
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
            this.vnDate.Location = new System.Drawing.Point(285, 73);
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
            this.vnUserStatus.Location = new System.Drawing.Point(75, 167);
            this.vnUserStatus.Name = "vnUserStatus";
            this.vnUserStatus.Size = new System.Drawing.Size(204, 22);
            this.vnUserStatus.TabIndex = 57;
            this.vnUserStatus.Text = "(vnUserStatus)";
            this.vnUserStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // vnLength
            // 
            this.vnLength.BackColor = System.Drawing.Color.Transparent;
            this.vnLength.Location = new System.Drawing.Point(285, 167);
            this.vnLength.Name = "vnLength";
            this.vnLength.Size = new System.Drawing.Size(126, 22);
            this.vnLength.TabIndex = 58;
            this.vnLength.Text = "(vnLength)";
            this.vnLength.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // vnPopularity
            // 
            this.vnPopularity.BackColor = System.Drawing.Color.Transparent;
            this.vnPopularity.Location = new System.Drawing.Point(279, 95);
            this.vnPopularity.Name = "vnPopularity";
            this.vnPopularity.Size = new System.Drawing.Size(132, 22);
            this.vnPopularity.TabIndex = 59;
            this.vnPopularity.Text = "(vnPopularity)";
            this.vnPopularity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // vnRating
            // 
            this.vnRating.BackColor = System.Drawing.Color.Transparent;
            this.vnRating.Location = new System.Drawing.Point(75, 95);
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
            this.label4.Location = new System.Drawing.Point(9, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 22);
            this.label4.TabIndex = 61;
            this.label4.Text = "Rating";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // VisualNovelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.BackgroundImage = global::Happy_Search.Properties.Resources.dark_striped;
            this.ClientSize = new System.Drawing.Size(685, 455);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.vnRating);
            this.Controls.Add(this.vnPopularity);
            this.Controls.Add(this.vnLength);
            this.Controls.Add(this.vnUserStatus);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.vnDate);
            this.Controls.Add(this.vnProducer);
            this.Controls.Add(this.vnKanjiName);
            this.Controls.Add(this.vnID);
            this.Controls.Add(this.vnName);
            this.Controls.Add(this.pcbImages);
            this.Controls.Add(this.vnUpdateLink);
            this.Controls.Add(this.vnTagCB);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.vnDesc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VisualNovelForm";
            this.Text = "VisualNovelForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CloseByEscape);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowLeftclick);
            ((System.ComponentModel.ISupportInitialize)(this.pcbImages)).EndInit();
            this.ResumeLayout(false);

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
        internal System.Windows.Forms.Label vnName;
        internal System.Windows.Forms.LinkLabel vnID;
        internal System.Windows.Forms.Label vnKanjiName;
        internal System.Windows.Forms.Label vnProducer;
        internal System.Windows.Forms.Label vnDate;
        private System.Windows.Forms.Button button1;
        internal System.Windows.Forms.Label vnUserStatus;
        internal System.Windows.Forms.Label vnLength;
        internal System.Windows.Forms.Label vnPopularity;
        internal System.Windows.Forms.Label vnRating;
        internal System.Windows.Forms.Label label4;
    }
}