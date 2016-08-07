﻿namespace Happy_Search
{
    partial class ProducerSearchForm
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
            this.olProdSearch = new BrightIdeasSoftware.ObjectListView();
            this.ol3Name = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol3InList = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol3ID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.prodAddButton = new System.Windows.Forms.Button();
            this.prodSearchReply = new System.Windows.Forms.Label();
            this.producerSearchLabel = new System.Windows.Forms.Label();
            this.producerSearchButton = new System.Windows.Forms.Button();
            this.producerSearchBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.olProdSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // olProdSearch
            // 
            this.olProdSearch.AllColumns.Add(this.ol3Name);
            this.olProdSearch.AllColumns.Add(this.ol3InList);
            this.olProdSearch.AllColumns.Add(this.ol3ID);
            this.olProdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olProdSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ol3Name,
            this.ol3InList,
            this.ol3ID});
            this.olProdSearch.FullRowSelect = true;
            this.olProdSearch.GridLines = true;
            this.olProdSearch.Location = new System.Drawing.Point(12, 12);
            this.olProdSearch.Name = "olProdSearch";
            this.olProdSearch.ShowGroups = false;
            this.olProdSearch.Size = new System.Drawing.Size(677, 118);
            this.olProdSearch.TabIndex = 4;
            this.olProdSearch.UseCompatibleStateImageBehavior = false;
            this.olProdSearch.View = System.Windows.Forms.View.Details;
            this.olProdSearch.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.AddProducerByDoubleClick);
            // 
            // ol3Name
            // 
            this.ol3Name.AspectName = "Name";
            this.ol3Name.FillsFreeSpace = true;
            this.ol3Name.Text = "Name";
            this.ol3Name.Width = 427;
            // 
            // ol3InList
            // 
            this.ol3InList.AspectName = "InList";
            this.ol3InList.Text = "In List?";
            this.ol3InList.Width = 47;
            // 
            // ol3ID
            // 
            this.ol3ID.AspectName = "ID";
            this.ol3ID.Text = "Producer ID";
            this.ol3ID.Width = 70;
            // 
            // prodAddButton
            // 
            this.prodAddButton.BackColor = System.Drawing.Color.MistyRose;
            this.prodAddButton.FlatAppearance.BorderSize = 0;
            this.prodAddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.prodAddButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.prodAddButton.Location = new System.Drawing.Point(575, 136);
            this.prodAddButton.Name = "prodAddButton";
            this.prodAddButton.Size = new System.Drawing.Size(114, 20);
            this.prodAddButton.TabIndex = 7;
            this.prodAddButton.Text = "Add Selected to List";
            this.prodAddButton.UseVisualStyleBackColor = false;
            this.prodAddButton.Click += new System.EventHandler(this.AddProducersClick);
            // 
            // prodSearchReply
            // 
            this.prodSearchReply.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.prodSearchReply.Location = new System.Drawing.Point(251, 136);
            this.prodSearchReply.Name = "prodSearchReply";
            this.prodSearchReply.Size = new System.Drawing.Size(121, 20);
            this.prodSearchReply.TabIndex = 12;
            this.prodSearchReply.Text = "(prodSearchReply)";
            this.prodSearchReply.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // producerSearchLabel
            // 
            this.producerSearchLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.producerSearchLabel.Location = new System.Drawing.Point(12, 139);
            this.producerSearchLabel.Name = "producerSearchLabel";
            this.producerSearchLabel.Size = new System.Drawing.Size(92, 17);
            this.producerSearchLabel.TabIndex = 9;
            this.producerSearchLabel.Text = "Search Producers";
            // 
            // producerSearchButton
            // 
            this.producerSearchButton.BackColor = System.Drawing.Color.Coral;
            this.producerSearchButton.FlatAppearance.BorderSize = 0;
            this.producerSearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.producerSearchButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.producerSearchButton.Location = new System.Drawing.Point(216, 136);
            this.producerSearchButton.Name = "producerSearchButton";
            this.producerSearchButton.Size = new System.Drawing.Size(29, 20);
            this.producerSearchButton.TabIndex = 11;
            this.producerSearchButton.Text = "Go";
            this.producerSearchButton.UseVisualStyleBackColor = false;
            this.producerSearchButton.Click += new System.EventHandler(this.SearchProducersClick);
            // 
            // producerSearchBox
            // 
            this.producerSearchBox.Location = new System.Drawing.Point(110, 136);
            this.producerSearchBox.Name = "producerSearchBox";
            this.producerSearchBox.Size = new System.Drawing.Size(100, 20);
            this.producerSearchBox.TabIndex = 10;
            this.producerSearchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchProducersEnterKey);
            // 
            // ProducerSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(701, 165);
            this.Controls.Add(this.prodSearchReply);
            this.Controls.Add(this.producerSearchLabel);
            this.Controls.Add(this.producerSearchButton);
            this.Controls.Add(this.producerSearchBox);
            this.Controls.Add(this.prodAddButton);
            this.Controls.Add(this.olProdSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ProducerSearchForm";
            this.Text = "ProducerSearchForm";
            ((System.ComponentModel.ISupportInitialize)(this.olProdSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olProdSearch;
        private BrightIdeasSoftware.OLVColumn ol3Name;
        private BrightIdeasSoftware.OLVColumn ol3InList;
        private BrightIdeasSoftware.OLVColumn ol3ID;
        private System.Windows.Forms.Button prodAddButton;
        private System.Windows.Forms.Label prodSearchReply;
        private System.Windows.Forms.Label producerSearchLabel;
        private System.Windows.Forms.Button producerSearchButton;
        private System.Windows.Forms.TextBox producerSearchBox;
    }
}