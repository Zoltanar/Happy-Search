using System;
using System.ComponentModel;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Happy_Search.Other_Forms;

//#pragma warning disable 1591

namespace Happy_Search
{
    public partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tagTypeS2 = new System.Windows.Forms.CheckBox();
            this.tagTypeT2 = new System.Windows.Forms.CheckBox();
            this.tagTypeC2 = new System.Windows.Forms.CheckBox();
            this.getNewProducersButton = new System.Windows.Forms.Button();
            this.autoUpdateURTBox = new System.Windows.Forms.CheckBox();
            this.yearLimitBox = new System.Windows.Forms.CheckBox();
            this.addProducersButton = new System.Windows.Forms.Button();
            this.selectedProducersVNButton = new System.Windows.Forms.Button();
            this.removeProducersButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.updateTagsAndTraitsButton = new System.Windows.Forms.Button();
            this.ListByGoButton = new System.Windows.Forms.Button();
            this.ListByUpdateButton = new System.Windows.Forms.Button();
            this.ListByCB = new System.Windows.Forms.ComboBox();
            this.favoriteProducersHelpButton = new System.Windows.Forms.Button();
            this.listResultsButton = new System.Windows.Forms.Button();
            this.GetStartedHelpButton = new System.Windows.Forms.Button();
            this.closeAllFormsButton = new System.Windows.Forms.Button();
            this.SearchingAndFilteringButton = new System.Windows.Forms.Button();
            this.refreshAllProducersButton = new System.Windows.Forms.Button();
            this.toggleViewButton = new System.Windows.Forms.Button();
            this.multiActionBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.infoTab = new System.Windows.Forms.TabPage();
            this.logGB = new System.Windows.Forms.GroupBox();
            this.advancedCheckBox = new System.Windows.Forms.CheckBox();
            this.sendQueryButton = new System.Windows.Forms.Button();
            this.serverR = new System.Windows.Forms.RichTextBox();
            this.logReplyLabel = new System.Windows.Forms.Label();
            this.clearLogButton = new System.Windows.Forms.Button();
            this.serverQ = new System.Windows.Forms.RichTextBox();
            this.logQueryLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.questionBox = new System.Windows.Forms.RichTextBox();
            this.statBox = new System.Windows.Forms.GroupBox();
            this.dbs9r = new System.Windows.Forms.Label();
            this.dbs8r = new System.Windows.Forms.Label();
            this.dbs7r = new System.Windows.Forms.Label();
            this.dbs6r = new System.Windows.Forms.Label();
            this.dbs5r = new System.Windows.Forms.Label();
            this.dbs4r = new System.Windows.Forms.Label();
            this.dbs3r = new System.Windows.Forms.Label();
            this.dbs2r = new System.Windows.Forms.Label();
            this.dbs1r = new System.Windows.Forms.Label();
            this.dbs9 = new System.Windows.Forms.Label();
            this.dbs8 = new System.Windows.Forms.Label();
            this.dbs7 = new System.Windows.Forms.Label();
            this.dbs6 = new System.Windows.Forms.Label();
            this.dbs5 = new System.Windows.Forms.Label();
            this.dbs4 = new System.Windows.Forms.Label();
            this.dbs3 = new System.Windows.Forms.Label();
            this.dbs2 = new System.Windows.Forms.Label();
            this.dbs1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mctULLabel10 = new System.Windows.Forms.Label();
            this.mctULLabel9 = new System.Windows.Forms.Label();
            this.mctULLabel8 = new System.Windows.Forms.Label();
            this.mctULLabel7 = new System.Windows.Forms.Label();
            this.mctULLabel6 = new System.Windows.Forms.Label();
            this.mctULLabel5 = new System.Windows.Forms.Label();
            this.mctULLabel4 = new System.Windows.Forms.Label();
            this.mctULLabel3 = new System.Windows.Forms.Label();
            this.mctULLabel2 = new System.Windows.Forms.Label();
            this.mctULLabel1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.ulstatsavs = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.ulstatsvl = new System.Windows.Forms.Label();
            this.ulstatswl = new System.Windows.Forms.Label();
            this.ulstatsul = new System.Windows.Forms.Label();
            this.ulstatsall = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.aboutGB = new System.Windows.Forms.GroupBox();
            this.aboutTextBox = new System.Windows.Forms.RichTextBox();
            this.vnTab = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.filterDropdown = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.tileOLV = new BrightIdeasSoftware.ObjectListView();
            this.tileColumnTitle = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnProducer = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnLength = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnULS = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnULAdded = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnULNote = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnWLS = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnWLAdded = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnVote = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnRating = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnPopularity = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnUpdated = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ListByTB = new System.Windows.Forms.TextBox();
            this.ListByCBQuery = new System.Windows.Forms.ComboBox();
            this.viewPicker = new System.Windows.Forms.ComboBox();
            this.replyText = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.olFavoriteProducers = new BrightIdeasSoftware.ObjectListView();
            this.ol2Name = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2ItemCount = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2UserDropRate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2UserAverageVote = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2GeneralRating = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2Updated = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2ID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.prodReply = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.otherMethodsCB = new System.Windows.Forms.ComboBox();
            this.nsfwToggle = new System.Windows.Forms.CheckBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.userListButt = new System.Windows.Forms.Button();
            this.userListReply = new System.Windows.Forms.Label();
            this.TabsControl = new System.Windows.Forms.TabControl();
            this.filtersTab = new System.Windows.Forms.TabPage();
            this.ContextMenuVN = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.userlistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unknownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.finishedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stalledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.droppedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wishlistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.highToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blacklistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.voteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.preciseNumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showProducerTitlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addProducerToFavoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addChangeVNNoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addChangeVNGroupsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoTab.SuspendLayout();
            this.logGB.SuspendLayout();
            this.statBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.aboutGB.SuspendLayout();
            this.vnTab.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tileOLV)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olFavoriteProducers)).BeginInit();
            this.panel1.SuspendLayout();
            this.TabsControl.SuspendLayout();
            this.ContextMenuVN.SuspendLayout();
            this.SuspendLayout();
            // 
            // tagTypeS2
            // 
            this.tagTypeS2.AutoSize = true;
            this.tagTypeS2.Checked = true;
            this.tagTypeS2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tagTypeS2.Location = new System.Drawing.Point(436, 15);
            this.tagTypeS2.Name = "tagTypeS2";
            this.tagTypeS2.Size = new System.Drawing.Size(33, 17);
            this.tagTypeS2.TabIndex = 63;
            this.tagTypeS2.Text = "S";
            this.tagTypeS2.UseVisualStyleBackColor = true;
            this.tagTypeS2.Click += new System.EventHandler(this.DisplayCommonTagsURT);
            // 
            // tagTypeT2
            // 
            this.tagTypeT2.AutoSize = true;
            this.tagTypeT2.Checked = true;
            this.tagTypeT2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tagTypeT2.Location = new System.Drawing.Point(475, 15);
            this.tagTypeT2.Name = "tagTypeT2";
            this.tagTypeT2.Size = new System.Drawing.Size(33, 17);
            this.tagTypeT2.TabIndex = 62;
            this.tagTypeT2.Text = "T";
            this.tagTypeT2.UseVisualStyleBackColor = true;
            this.tagTypeT2.Click += new System.EventHandler(this.DisplayCommonTagsURT);
            // 
            // tagTypeC2
            // 
            this.tagTypeC2.AutoSize = true;
            this.tagTypeC2.Checked = true;
            this.tagTypeC2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tagTypeC2.Location = new System.Drawing.Point(397, 15);
            this.tagTypeC2.Name = "tagTypeC2";
            this.tagTypeC2.Size = new System.Drawing.Size(33, 17);
            this.tagTypeC2.TabIndex = 61;
            this.tagTypeC2.Text = "C";
            this.tagTypeC2.UseVisualStyleBackColor = true;
            this.tagTypeC2.Click += new System.EventHandler(this.DisplayCommonTagsURT);
            // 
            // getNewProducersButton
            // 
            this.getNewProducersButton.BackColor = System.Drawing.Color.MistyRose;
            this.getNewProducersButton.FlatAppearance.BorderSize = 0;
            this.getNewProducersButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.getNewProducersButton.ForeColor = System.Drawing.Color.Black;
            this.getNewProducersButton.Location = new System.Drawing.Point(3, 90);
            this.getNewProducersButton.Name = "getNewProducersButton";
            this.getNewProducersButton.Size = new System.Drawing.Size(120, 23);
            this.getNewProducersButton.TabIndex = 37;
            this.getNewProducersButton.Text = "Get New Titles";
            this.toolTip.SetToolTip(this.getNewProducersButton, "Get titles by favorite producers that aren\'t in local database yet.");
            this.getNewProducersButton.UseVisualStyleBackColor = false;
            this.getNewProducersButton.Click += new System.EventHandler(this.GetNewFavoriteProducerTitles);
            // 
            // autoUpdateURTBox
            // 
            this.autoUpdateURTBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.autoUpdateURTBox.Location = new System.Drawing.Point(118, 25);
            this.autoUpdateURTBox.Name = "autoUpdateURTBox";
            this.autoUpdateURTBox.Size = new System.Drawing.Size(123, 17);
            this.autoUpdateURTBox.TabIndex = 83;
            this.autoUpdateURTBox.Text = "Auto-Update URT";
            this.autoUpdateURTBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.autoUpdateURTBox.UseVisualStyleBackColor = true;
            this.autoUpdateURTBox.Click += new System.EventHandler(this.ToggleAutoUpdateURT);
            // 
            // yearLimitBox
            // 
            this.yearLimitBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.yearLimitBox.Location = new System.Drawing.Point(118, 43);
            this.yearLimitBox.Name = "yearLimitBox";
            this.yearLimitBox.Size = new System.Drawing.Size(123, 17);
            this.yearLimitBox.TabIndex = 84;
            this.yearLimitBox.Text = "10 Year Limit";
            this.yearLimitBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.yearLimitBox.UseVisualStyleBackColor = true;
            this.yearLimitBox.Click += new System.EventHandler(this.ToggleLimit10Years);
            // 
            // addProducersButton
            // 
            this.addProducersButton.BackColor = System.Drawing.Color.SteelBlue;
            this.addProducersButton.FlatAppearance.BorderSize = 0;
            this.addProducersButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addProducersButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.addProducersButton.Location = new System.Drawing.Point(3, 3);
            this.addProducersButton.Name = "addProducersButton";
            this.addProducersButton.Size = new System.Drawing.Size(120, 23);
            this.addProducersButton.TabIndex = 36;
            this.addProducersButton.Text = "Add Producers";
            this.addProducersButton.UseVisualStyleBackColor = false;
            this.addProducersButton.Click += new System.EventHandler(this.AddProducers);
            // 
            // selectedProducersVNButton
            // 
            this.selectedProducersVNButton.BackColor = System.Drawing.Color.SteelBlue;
            this.selectedProducersVNButton.FlatAppearance.BorderSize = 0;
            this.selectedProducersVNButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectedProducersVNButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.selectedProducersVNButton.Location = new System.Drawing.Point(3, 61);
            this.selectedProducersVNButton.Name = "selectedProducersVNButton";
            this.selectedProducersVNButton.Size = new System.Drawing.Size(120, 23);
            this.selectedProducersVNButton.TabIndex = 33;
            this.selectedProducersVNButton.Text = "List Titles By Selected";
            this.toolTip.SetToolTip(this.selectedProducersVNButton, "List titles by the selected favorite producers.");
            this.selectedProducersVNButton.UseVisualStyleBackColor = false;
            this.selectedProducersVNButton.Click += new System.EventHandler(this.ShowSelectedProducerVNs);
            // 
            // removeProducersButton
            // 
            this.removeProducersButton.BackColor = System.Drawing.Color.SteelBlue;
            this.removeProducersButton.FlatAppearance.BorderSize = 0;
            this.removeProducersButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeProducersButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.removeProducersButton.Location = new System.Drawing.Point(3, 32);
            this.removeProducersButton.Name = "removeProducersButton";
            this.removeProducersButton.Size = new System.Drawing.Size(120, 23);
            this.removeProducersButton.TabIndex = 7;
            this.removeProducersButton.Text = "Remove Selected";
            this.toolTip.SetToolTip(this.removeProducersButton, "Remove selected producers from favorite producers list.");
            this.removeProducersButton.UseVisualStyleBackColor = false;
            this.removeProducersButton.Click += new System.EventHandler(this.RemoveProducers);
            // 
            // updateTagsAndTraitsButton
            // 
            this.updateTagsAndTraitsButton.BackColor = System.Drawing.Color.MistyRose;
            this.updateTagsAndTraitsButton.FlatAppearance.BorderSize = 0;
            this.updateTagsAndTraitsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateTagsAndTraitsButton.ForeColor = System.Drawing.Color.Black;
            this.updateTagsAndTraitsButton.Location = new System.Drawing.Point(3, 61);
            this.updateTagsAndTraitsButton.Name = "updateTagsAndTraitsButton";
            this.updateTagsAndTraitsButton.Size = new System.Drawing.Size(109, 35);
            this.updateTagsAndTraitsButton.TabIndex = 91;
            this.updateTagsAndTraitsButton.Text = "Update Tags/Traits/Stats";
            this.toolTip.SetToolTip(this.updateTagsAndTraitsButton, "Update tags, traits and stats of titles that haven\'t been updated in over 7 days." +
        "");
            this.updateTagsAndTraitsButton.UseVisualStyleBackColor = false;
            this.updateTagsAndTraitsButton.Click += new System.EventHandler(this.UpdateTagsTraitsStatsClick);
            // 
            // ListByGoButton
            // 
            this.ListByGoButton.BackColor = System.Drawing.Color.SteelBlue;
            this.ListByGoButton.FlatAppearance.BorderSize = 0;
            this.ListByGoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ListByGoButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ListByGoButton.Location = new System.Drawing.Point(418, 4);
            this.ListByGoButton.Name = "ListByGoButton";
            this.ListByGoButton.Size = new System.Drawing.Size(29, 23);
            this.ListByGoButton.TabIndex = 107;
            this.ListByGoButton.Text = "Go";
            this.toolTip.SetToolTip(this.ListByGoButton, "Lists titles in local database.");
            this.ListByGoButton.UseVisualStyleBackColor = false;
            this.ListByGoButton.Click += new System.EventHandler(this.ListByGo);
            // 
            // ListByUpdateButton
            // 
            this.ListByUpdateButton.BackColor = System.Drawing.Color.MistyRose;
            this.ListByUpdateButton.FlatAppearance.BorderSize = 0;
            this.ListByUpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ListByUpdateButton.ForeColor = System.Drawing.Color.Black;
            this.ListByUpdateButton.Location = new System.Drawing.Point(454, 4);
            this.ListByUpdateButton.Name = "ListByUpdateButton";
            this.ListByUpdateButton.Size = new System.Drawing.Size(52, 23);
            this.ListByUpdateButton.TabIndex = 106;
            this.ListByUpdateButton.Text = "Update";
            this.toolTip.SetToolTip(this.ListByUpdateButton, "Gets titles from VNDB that aren\'t yet in local database.");
            this.ListByUpdateButton.UseVisualStyleBackColor = false;
            this.ListByUpdateButton.Click += new System.EventHandler(this.ListByUpdate);
            // 
            // ListByCB
            // 
            this.ListByCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ListByCB.FormattingEnabled = true;
            this.ListByCB.Items.AddRange(new object[] {
            "By Name/Alias",
            "By Producer",
            "By Year",
            "By Group"});
            this.ListByCB.Location = new System.Drawing.Point(192, 5);
            this.ListByCB.Name = "ListByCB";
            this.ListByCB.Size = new System.Drawing.Size(95, 21);
            this.ListByCB.TabIndex = 104;
            this.toolTip.SetToolTip(this.ListByCB, "Select method by which to list titles.");
            this.ListByCB.SelectedIndexChanged += new System.EventHandler(this.ChangeListBy);
            // 
            // favoriteProducersHelpButton
            // 
            this.favoriteProducersHelpButton.BackColor = System.Drawing.Color.Khaki;
            this.favoriteProducersHelpButton.FlatAppearance.BorderSize = 0;
            this.favoriteProducersHelpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.favoriteProducersHelpButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.favoriteProducersHelpButton.Location = new System.Drawing.Point(3, 148);
            this.favoriteProducersHelpButton.Name = "favoriteProducersHelpButton";
            this.favoriteProducersHelpButton.Size = new System.Drawing.Size(121, 23);
            this.favoriteProducersHelpButton.TabIndex = 88;
            this.favoriteProducersHelpButton.Text = "Favorite Producers";
            this.toolTip.SetToolTip(this.favoriteProducersHelpButton, "Help on Favorite Producers.");
            this.favoriteProducersHelpButton.UseVisualStyleBackColor = false;
            this.favoriteProducersHelpButton.Click += new System.EventHandler(this.Help_FavoriteProducers);
            // 
            // listResultsButton
            // 
            this.listResultsButton.BackColor = System.Drawing.Color.Khaki;
            this.listResultsButton.FlatAppearance.BorderSize = 0;
            this.listResultsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.listResultsButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listResultsButton.Location = new System.Drawing.Point(625, 5);
            this.listResultsButton.Name = "listResultsButton";
            this.listResultsButton.Size = new System.Drawing.Size(54, 23);
            this.listResultsButton.TabIndex = 95;
            this.listResultsButton.Text = "Results";
            this.toolTip.SetToolTip(this.listResultsButton, "Help on Right-click functions for results.");
            this.listResultsButton.UseVisualStyleBackColor = false;
            this.listResultsButton.Click += new System.EventHandler(this.Help_ListResults);
            // 
            // GetStartedHelpButton
            // 
            this.GetStartedHelpButton.BackColor = System.Drawing.Color.Khaki;
            this.GetStartedHelpButton.FlatAppearance.BorderSize = 0;
            this.GetStartedHelpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetStartedHelpButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.GetStartedHelpButton.Location = new System.Drawing.Point(118, 91);
            this.GetStartedHelpButton.Name = "GetStartedHelpButton";
            this.GetStartedHelpButton.Size = new System.Drawing.Size(123, 23);
            this.GetStartedHelpButton.TabIndex = 89;
            this.GetStartedHelpButton.Text = "Get Started";
            this.toolTip.SetToolTip(this.GetStartedHelpButton, "Help for getting started.");
            this.GetStartedHelpButton.UseVisualStyleBackColor = false;
            this.GetStartedHelpButton.Click += new System.EventHandler(this.Help_GetStarted);
            // 
            // closeAllFormsButton
            // 
            this.closeAllFormsButton.BackColor = System.Drawing.Color.SteelBlue;
            this.closeAllFormsButton.FlatAppearance.BorderSize = 0;
            this.closeAllFormsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeAllFormsButton.ForeColor = System.Drawing.Color.White;
            this.closeAllFormsButton.Location = new System.Drawing.Point(118, 62);
            this.closeAllFormsButton.Name = "closeAllFormsButton";
            this.closeAllFormsButton.Size = new System.Drawing.Size(123, 23);
            this.closeAllFormsButton.TabIndex = 81;
            this.closeAllFormsButton.Text = "Close All VN Tabs";
            this.toolTip.SetToolTip(this.closeAllFormsButton, "Close all VN Info Tabs that you have opened.");
            this.closeAllFormsButton.UseVisualStyleBackColor = false;
            this.closeAllFormsButton.Click += new System.EventHandler(this.CloseVNTabs);
            // 
            // SearchingAndFilteringButton
            // 
            this.SearchingAndFilteringButton.BackColor = System.Drawing.Color.Khaki;
            this.SearchingAndFilteringButton.FlatAppearance.BorderSize = 0;
            this.SearchingAndFilteringButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SearchingAndFilteringButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SearchingAndFilteringButton.Location = new System.Drawing.Point(512, 5);
            this.SearchingAndFilteringButton.Name = "SearchingAndFilteringButton";
            this.SearchingAndFilteringButton.Size = new System.Drawing.Size(107, 23);
            this.SearchingAndFilteringButton.TabIndex = 91;
            this.SearchingAndFilteringButton.Text = "Searching / Listing";
            this.toolTip.SetToolTip(this.SearchingAndFilteringButton, "Help on Searching/Listing/Filtering titles.");
            this.SearchingAndFilteringButton.UseVisualStyleBackColor = false;
            this.SearchingAndFilteringButton.Click += new System.EventHandler(this.Help_SearchingAndFiltering);
            // 
            // refreshAllProducersButton
            // 
            this.refreshAllProducersButton.BackColor = System.Drawing.Color.MistyRose;
            this.refreshAllProducersButton.FlatAppearance.BorderSize = 0;
            this.refreshAllProducersButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshAllProducersButton.ForeColor = System.Drawing.Color.Black;
            this.refreshAllProducersButton.Location = new System.Drawing.Point(3, 119);
            this.refreshAllProducersButton.Name = "refreshAllProducersButton";
            this.refreshAllProducersButton.Size = new System.Drawing.Size(120, 23);
            this.refreshAllProducersButton.TabIndex = 32;
            this.refreshAllProducersButton.Text = "Update All Titles";
            this.toolTip.SetToolTip(this.refreshAllProducersButton, "Updates data on all titles by favorite producers , also gets new titles.");
            this.refreshAllProducersButton.UseVisualStyleBackColor = false;
            this.refreshAllProducersButton.Click += new System.EventHandler(this.RefreshAllFavoriteProducerTitles);
            // 
            // toggleViewButton
            // 
            this.toggleViewButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toggleViewButton.BackColor = System.Drawing.Color.SteelBlue;
            this.toggleViewButton.FlatAppearance.BorderSize = 0;
            this.toggleViewButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toggleViewButton.ForeColor = System.Drawing.Color.White;
            this.toggleViewButton.Location = new System.Drawing.Point(903, 7);
            this.toggleViewButton.Name = "toggleViewButton";
            this.toggleViewButton.Size = new System.Drawing.Size(274, 23);
            this.toggleViewButton.TabIndex = 93;
            this.toggleViewButton.Text = "▲ Hide Options ▲";
            this.toolTip.SetToolTip(this.toggleViewButton, "Show/hide settings and favorite producers section.");
            this.toggleViewButton.UseVisualStyleBackColor = false;
            this.toggleViewButton.Click += new System.EventHandler(this.ToggleWideView);
            // 
            // multiActionBox
            // 
            this.multiActionBox.BackColor = System.Drawing.Color.Navy;
            this.multiActionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.multiActionBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.multiActionBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.multiActionBox.FormattingEnabled = true;
            this.multiActionBox.Items.AddRange(new object[] {
            "Multi Actions",
            "--------------",
            "Deselect All",
            "Remove From DB",
            "Update Tags/Trait/Stats",
            "Update All Data"});
            this.multiActionBox.Location = new System.Drawing.Point(783, 7);
            this.multiActionBox.Name = "multiActionBox";
            this.multiActionBox.Size = new System.Drawing.Size(114, 21);
            this.multiActionBox.TabIndex = 108;
            this.toolTip.SetToolTip(this.multiActionBox, "Perform actions on all selected titles. Select by Ctrl+clicking.");
            this.multiActionBox.SelectedIndexChanged += new System.EventHandler(this.MultiActionSelect);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.MistyRose;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(3, 102);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 23);
            this.button1.TabIndex = 93;
            this.button1.Text = "Update All Data";
            this.toolTip.SetToolTip(this.button1, "Update all data for titles that haven\'t been updated in over 7 days.");
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.UpdateAllDataClick);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.SteelBlue;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button2.Location = new System.Drawing.Point(152, 30);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 23);
            this.button2.TabIndex = 111;
            this.button2.Text = "All Titles";
            this.toolTip.SetToolTip(this.button2, "Lists titles in local database.");
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.SetAllTitles);
            // 
            // infoTab
            // 
            this.infoTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.infoTab.Controls.Add(this.logGB);
            this.infoTab.Controls.Add(this.statBox);
            this.infoTab.Controls.Add(this.groupBox1);
            this.infoTab.Controls.Add(this.aboutGB);
            this.infoTab.Location = new System.Drawing.Point(4, 22);
            this.infoTab.Name = "infoTab";
            this.infoTab.Padding = new System.Windows.Forms.Padding(3);
            this.infoTab.Size = new System.Drawing.Size(1196, 595);
            this.infoTab.TabIndex = 2;
            this.infoTab.Text = "Information";
            // 
            // logGB
            // 
            this.logGB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logGB.BackColor = System.Drawing.Color.Transparent;
            this.logGB.Controls.Add(this.advancedCheckBox);
            this.logGB.Controls.Add(this.sendQueryButton);
            this.logGB.Controls.Add(this.serverR);
            this.logGB.Controls.Add(this.logReplyLabel);
            this.logGB.Controls.Add(this.clearLogButton);
            this.logGB.Controls.Add(this.serverQ);
            this.logGB.Controls.Add(this.logQueryLabel);
            this.logGB.Controls.Add(this.label1);
            this.logGB.Controls.Add(this.questionBox);
            this.logGB.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.logGB.Location = new System.Drawing.Point(8, 189);
            this.logGB.Name = "logGB";
            this.logGB.Size = new System.Drawing.Size(1180, 403);
            this.logGB.TabIndex = 35;
            this.logGB.TabStop = false;
            this.logGB.Text = "Log";
            // 
            // advancedCheckBox
            // 
            this.advancedCheckBox.AutoSize = true;
            this.advancedCheckBox.Location = new System.Drawing.Point(134, 106);
            this.advancedCheckBox.Name = "advancedCheckBox";
            this.advancedCheckBox.Size = new System.Drawing.Size(105, 17);
            this.advancedCheckBox.TabIndex = 41;
            this.advancedCheckBox.Text = "Advanced Mode";
            this.advancedCheckBox.UseVisualStyleBackColor = true;
            this.advancedCheckBox.CheckedChanged += new System.EventHandler(this.ToggleAdvancedMode);
            // 
            // sendQueryButton
            // 
            this.sendQueryButton.BackColor = System.Drawing.Color.SteelBlue;
            this.sendQueryButton.Enabled = false;
            this.sendQueryButton.FlatAppearance.BorderSize = 0;
            this.sendQueryButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sendQueryButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.sendQueryButton.Location = new System.Drawing.Point(297, 102);
            this.sendQueryButton.Name = "sendQueryButton";
            this.sendQueryButton.Size = new System.Drawing.Size(41, 23);
            this.sendQueryButton.TabIndex = 40;
            this.sendQueryButton.Text = "Send";
            this.sendQueryButton.UseVisualStyleBackColor = false;
            this.sendQueryButton.Click += new System.EventHandler(this.LogQuestion);
            // 
            // serverR
            // 
            this.serverR.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverR.BackColor = System.Drawing.SystemColors.InfoText;
            this.serverR.Enabled = false;
            this.serverR.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverR.ForeColor = System.Drawing.SystemColors.Info;
            this.serverR.Location = new System.Drawing.Point(344, 32);
            this.serverR.Name = "serverR";
            this.serverR.Size = new System.Drawing.Size(830, 363);
            this.serverR.TabIndex = 39;
            this.serverR.Text = "(Advanced Mode Disabled)";
            // 
            // logReplyLabel
            // 
            this.logReplyLabel.AutoSize = true;
            this.logReplyLabel.Location = new System.Drawing.Point(344, 16);
            this.logReplyLabel.Name = "logReplyLabel";
            this.logReplyLabel.Size = new System.Drawing.Size(102, 13);
            this.logReplyLabel.TabIndex = 38;
            this.logReplyLabel.Text = "Replies From Server";
            // 
            // clearLogButton
            // 
            this.clearLogButton.BackColor = System.Drawing.Color.MistyRose;
            this.clearLogButton.Enabled = false;
            this.clearLogButton.FlatAppearance.BorderSize = 0;
            this.clearLogButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearLogButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.clearLogButton.Location = new System.Drawing.Point(245, 102);
            this.clearLogButton.Name = "clearLogButton";
            this.clearLogButton.Size = new System.Drawing.Size(46, 23);
            this.clearLogButton.TabIndex = 37;
            this.clearLogButton.Text = "Clear";
            this.clearLogButton.UseVisualStyleBackColor = false;
            this.clearLogButton.Click += new System.EventHandler(this.ClearLog);
            // 
            // serverQ
            // 
            this.serverQ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.serverQ.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.serverQ.Enabled = false;
            this.serverQ.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverQ.ForeColor = System.Drawing.SystemColors.Info;
            this.serverQ.Location = new System.Drawing.Point(4, 131);
            this.serverQ.Name = "serverQ";
            this.serverQ.Size = new System.Drawing.Size(334, 264);
            this.serverQ.TabIndex = 36;
            this.serverQ.Text = "(Advanced Mode Disabled)";
            // 
            // logQueryLabel
            // 
            this.logQueryLabel.AutoSize = true;
            this.logQueryLabel.Location = new System.Drawing.Point(6, 115);
            this.logQueryLabel.Name = "logQueryLabel";
            this.logQueryLabel.Size = new System.Drawing.Size(93, 13);
            this.logQueryLabel.TabIndex = 35;
            this.logQueryLabel.Text = "Queries To Server";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Send Query Here";
            // 
            // questionBox
            // 
            this.questionBox.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.questionBox.Enabled = false;
            this.questionBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.questionBox.ForeColor = System.Drawing.SystemColors.InfoText;
            this.questionBox.Location = new System.Drawing.Point(4, 32);
            this.questionBox.Name = "questionBox";
            this.questionBox.Size = new System.Drawing.Size(334, 64);
            this.questionBox.TabIndex = 33;
            this.questionBox.Text = "(Advanced Mode Disabled)";
            // 
            // statBox
            // 
            this.statBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.statBox.BackColor = System.Drawing.Color.Transparent;
            this.statBox.Controls.Add(this.dbs9r);
            this.statBox.Controls.Add(this.dbs8r);
            this.statBox.Controls.Add(this.dbs7r);
            this.statBox.Controls.Add(this.dbs6r);
            this.statBox.Controls.Add(this.dbs5r);
            this.statBox.Controls.Add(this.dbs4r);
            this.statBox.Controls.Add(this.dbs3r);
            this.statBox.Controls.Add(this.dbs2r);
            this.statBox.Controls.Add(this.dbs1r);
            this.statBox.Controls.Add(this.dbs9);
            this.statBox.Controls.Add(this.dbs8);
            this.statBox.Controls.Add(this.dbs7);
            this.statBox.Controls.Add(this.dbs6);
            this.statBox.Controls.Add(this.dbs5);
            this.statBox.Controls.Add(this.dbs4);
            this.statBox.Controls.Add(this.dbs3);
            this.statBox.Controls.Add(this.dbs2);
            this.statBox.Controls.Add(this.dbs1);
            this.statBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.statBox.Location = new System.Drawing.Point(1049, 6);
            this.statBox.Name = "statBox";
            this.statBox.Size = new System.Drawing.Size(139, 177);
            this.statBox.TabIndex = 32;
            this.statBox.TabStop = false;
            this.statBox.Text = "VNDB Stats";
            // 
            // dbs9r
            // 
            this.dbs9r.Location = new System.Drawing.Point(67, 124);
            this.dbs9r.Name = "dbs9r";
            this.dbs9r.Size = new System.Drawing.Size(55, 13);
            this.dbs9r.TabIndex = 40;
            this.dbs9r.Text = "(blank)";
            // 
            // dbs8r
            // 
            this.dbs8r.Location = new System.Drawing.Point(67, 111);
            this.dbs8r.Name = "dbs8r";
            this.dbs8r.Size = new System.Drawing.Size(55, 13);
            this.dbs8r.TabIndex = 39;
            this.dbs8r.Text = "(blank)";
            // 
            // dbs7r
            // 
            this.dbs7r.Location = new System.Drawing.Point(67, 98);
            this.dbs7r.Name = "dbs7r";
            this.dbs7r.Size = new System.Drawing.Size(55, 13);
            this.dbs7r.TabIndex = 38;
            this.dbs7r.Text = "(blank)";
            // 
            // dbs6r
            // 
            this.dbs6r.Location = new System.Drawing.Point(67, 85);
            this.dbs6r.Name = "dbs6r";
            this.dbs6r.Size = new System.Drawing.Size(55, 13);
            this.dbs6r.TabIndex = 37;
            this.dbs6r.Text = "(blank)";
            // 
            // dbs5r
            // 
            this.dbs5r.Location = new System.Drawing.Point(67, 72);
            this.dbs5r.Name = "dbs5r";
            this.dbs5r.Size = new System.Drawing.Size(55, 13);
            this.dbs5r.TabIndex = 36;
            this.dbs5r.Text = "(blank)";
            // 
            // dbs4r
            // 
            this.dbs4r.Location = new System.Drawing.Point(67, 59);
            this.dbs4r.Name = "dbs4r";
            this.dbs4r.Size = new System.Drawing.Size(55, 13);
            this.dbs4r.TabIndex = 35;
            this.dbs4r.Text = "(blank)";
            // 
            // dbs3r
            // 
            this.dbs3r.Location = new System.Drawing.Point(67, 47);
            this.dbs3r.Name = "dbs3r";
            this.dbs3r.Size = new System.Drawing.Size(55, 13);
            this.dbs3r.TabIndex = 34;
            this.dbs3r.Text = "(blank)";
            // 
            // dbs2r
            // 
            this.dbs2r.Location = new System.Drawing.Point(67, 34);
            this.dbs2r.Name = "dbs2r";
            this.dbs2r.Size = new System.Drawing.Size(55, 13);
            this.dbs2r.TabIndex = 33;
            this.dbs2r.Text = "(blank)";
            // 
            // dbs1r
            // 
            this.dbs1r.Location = new System.Drawing.Point(67, 21);
            this.dbs1r.Name = "dbs1r";
            this.dbs1r.Size = new System.Drawing.Size(55, 13);
            this.dbs1r.TabIndex = 32;
            this.dbs1r.Text = "(blank)";
            // 
            // dbs9
            // 
            this.dbs9.Location = new System.Drawing.Point(8, 124);
            this.dbs9.Name = "dbs9";
            this.dbs9.Size = new System.Drawing.Size(55, 13);
            this.dbs9.TabIndex = 31;
            this.dbs9.Text = "Traits";
            this.dbs9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dbs8
            // 
            this.dbs8.Location = new System.Drawing.Point(8, 111);
            this.dbs8.Name = "dbs8";
            this.dbs8.Size = new System.Drawing.Size(55, 13);
            this.dbs8.TabIndex = 30;
            this.dbs8.Text = "VN";
            this.dbs8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dbs7
            // 
            this.dbs7.Location = new System.Drawing.Point(8, 98);
            this.dbs7.Name = "dbs7";
            this.dbs7.Size = new System.Drawing.Size(55, 13);
            this.dbs7.TabIndex = 29;
            this.dbs7.Text = "Posts";
            this.dbs7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dbs6
            // 
            this.dbs6.Location = new System.Drawing.Point(8, 85);
            this.dbs6.Name = "dbs6";
            this.dbs6.Size = new System.Drawing.Size(55, 13);
            this.dbs6.TabIndex = 28;
            this.dbs6.Text = "Chars";
            this.dbs6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dbs5
            // 
            this.dbs5.Location = new System.Drawing.Point(8, 72);
            this.dbs5.Name = "dbs5";
            this.dbs5.Size = new System.Drawing.Size(55, 13);
            this.dbs5.TabIndex = 27;
            this.dbs5.Text = "Producers";
            this.dbs5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dbs4
            // 
            this.dbs4.Location = new System.Drawing.Point(8, 60);
            this.dbs4.Name = "dbs4";
            this.dbs4.Size = new System.Drawing.Size(55, 13);
            this.dbs4.TabIndex = 26;
            this.dbs4.Text = "Releases";
            this.dbs4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dbs3
            // 
            this.dbs3.Location = new System.Drawing.Point(8, 47);
            this.dbs3.Name = "dbs3";
            this.dbs3.Size = new System.Drawing.Size(55, 13);
            this.dbs3.TabIndex = 25;
            this.dbs3.Text = "Tags";
            this.dbs3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dbs2
            // 
            this.dbs2.Location = new System.Drawing.Point(8, 34);
            this.dbs2.Name = "dbs2";
            this.dbs2.Size = new System.Drawing.Size(55, 13);
            this.dbs2.TabIndex = 24;
            this.dbs2.Text = "Threads";
            this.dbs2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dbs1
            // 
            this.dbs1.Location = new System.Drawing.Point(8, 21);
            this.dbs1.Name = "dbs1";
            this.dbs1.Size = new System.Drawing.Size(55, 13);
            this.dbs1.TabIndex = 23;
            this.dbs1.Text = "Users";
            this.dbs1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.mctULLabel10);
            this.groupBox1.Controls.Add(this.mctULLabel9);
            this.groupBox1.Controls.Add(this.mctULLabel8);
            this.groupBox1.Controls.Add(this.mctULLabel7);
            this.groupBox1.Controls.Add(this.mctULLabel6);
            this.groupBox1.Controls.Add(this.mctULLabel5);
            this.groupBox1.Controls.Add(this.mctULLabel4);
            this.groupBox1.Controls.Add(this.mctULLabel3);
            this.groupBox1.Controls.Add(this.mctULLabel2);
            this.groupBox1.Controls.Add(this.mctULLabel1);
            this.groupBox1.Controls.Add(this.tagTypeS2);
            this.groupBox1.Controls.Add(this.tagTypeT2);
            this.groupBox1.Controls.Add(this.tagTypeC2);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.ulstatsavs);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.ulstatsvl);
            this.groupBox1.Controls.Add(this.ulstatswl);
            this.groupBox1.Controls.Add(this.ulstatsul);
            this.groupBox1.Controls.Add(this.ulstatsall);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Location = new System.Drawing.Point(520, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(523, 177);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Stats";
            // 
            // mctULLabel10
            // 
            this.mctULLabel10.Location = new System.Drawing.Point(258, 150);
            this.mctULLabel10.Name = "mctULLabel10";
            this.mctULLabel10.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel10.TabIndex = 74;
            this.mctULLabel10.Text = "(blank)";
            // 
            // mctULLabel9
            // 
            this.mctULLabel9.Location = new System.Drawing.Point(258, 137);
            this.mctULLabel9.Name = "mctULLabel9";
            this.mctULLabel9.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel9.TabIndex = 73;
            this.mctULLabel9.Text = "(blank)";
            // 
            // mctULLabel8
            // 
            this.mctULLabel8.Location = new System.Drawing.Point(258, 124);
            this.mctULLabel8.Name = "mctULLabel8";
            this.mctULLabel8.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel8.TabIndex = 72;
            this.mctULLabel8.Text = "(blank)";
            // 
            // mctULLabel7
            // 
            this.mctULLabel7.Location = new System.Drawing.Point(258, 111);
            this.mctULLabel7.Name = "mctULLabel7";
            this.mctULLabel7.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel7.TabIndex = 71;
            this.mctULLabel7.Text = "(blank)";
            // 
            // mctULLabel6
            // 
            this.mctULLabel6.Location = new System.Drawing.Point(258, 98);
            this.mctULLabel6.Name = "mctULLabel6";
            this.mctULLabel6.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel6.TabIndex = 70;
            this.mctULLabel6.Text = "(blank)";
            // 
            // mctULLabel5
            // 
            this.mctULLabel5.Location = new System.Drawing.Point(258, 85);
            this.mctULLabel5.Name = "mctULLabel5";
            this.mctULLabel5.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel5.TabIndex = 69;
            this.mctULLabel5.Text = "(blank)";
            // 
            // mctULLabel4
            // 
            this.mctULLabel4.Location = new System.Drawing.Point(258, 72);
            this.mctULLabel4.Name = "mctULLabel4";
            this.mctULLabel4.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel4.TabIndex = 68;
            this.mctULLabel4.Text = "(blank)";
            // 
            // mctULLabel3
            // 
            this.mctULLabel3.Location = new System.Drawing.Point(258, 59);
            this.mctULLabel3.Name = "mctULLabel3";
            this.mctULLabel3.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel3.TabIndex = 67;
            this.mctULLabel3.Text = "(blank)";
            // 
            // mctULLabel2
            // 
            this.mctULLabel2.Location = new System.Drawing.Point(258, 46);
            this.mctULLabel2.Name = "mctULLabel2";
            this.mctULLabel2.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel2.TabIndex = 66;
            this.mctULLabel2.Text = "(blank)";
            // 
            // mctULLabel1
            // 
            this.mctULLabel1.Location = new System.Drawing.Point(258, 33);
            this.mctULLabel1.Name = "mctULLabel1";
            this.mctULLabel1.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel1.TabIndex = 65;
            this.mctULLabel1.Text = "(blank)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new System.Drawing.Point(258, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 13);
            this.label13.TabIndex = 60;
            this.label13.Text = "Most Common Tags";
            // 
            // ulstatsavs
            // 
            this.ulstatsavs.Location = new System.Drawing.Point(148, 72);
            this.ulstatsavs.Name = "ulstatsavs";
            this.ulstatsavs.Size = new System.Drawing.Size(55, 13);
            this.ulstatsavs.TabIndex = 49;
            this.ulstatsavs.Text = "(blank)";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(6, 72);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(136, 13);
            this.label12.TabIndex = 48;
            this.label12.Text = "Average Vote Score";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ulstatsvl
            // 
            this.ulstatsvl.Location = new System.Drawing.Point(148, 58);
            this.ulstatsvl.Name = "ulstatsvl";
            this.ulstatsvl.Size = new System.Drawing.Size(55, 13);
            this.ulstatsvl.TabIndex = 47;
            this.ulstatsvl.Text = "(blank)";
            // 
            // ulstatswl
            // 
            this.ulstatswl.Location = new System.Drawing.Point(148, 46);
            this.ulstatswl.Name = "ulstatswl";
            this.ulstatswl.Size = new System.Drawing.Size(55, 13);
            this.ulstatswl.TabIndex = 46;
            this.ulstatswl.Text = "(blank)";
            // 
            // ulstatsul
            // 
            this.ulstatsul.Location = new System.Drawing.Point(148, 33);
            this.ulstatsul.Name = "ulstatsul";
            this.ulstatsul.Size = new System.Drawing.Size(55, 13);
            this.ulstatsul.TabIndex = 45;
            this.ulstatsul.Text = "(blank)";
            // 
            // ulstatsall
            // 
            this.ulstatsall.Location = new System.Drawing.Point(148, 20);
            this.ulstatsall.Name = "ulstatsall";
            this.ulstatsall.Size = new System.Drawing.Size(55, 13);
            this.ulstatsall.TabIndex = 41;
            this.ulstatsall.Text = "(blank)";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(6, 59);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(136, 13);
            this.label11.TabIndex = 44;
            this.label11.Text = "Titles in Votelist";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 46);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(136, 13);
            this.label10.TabIndex = 43;
            this.label10.Text = "Titles in Wishlist";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(6, 33);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(136, 13);
            this.label9.TabIndex = 42;
            this.label9.Text = "Titles in Userlist";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(136, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "All Titles Related to User";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // aboutGB
            // 
            this.aboutGB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutGB.BackColor = System.Drawing.Color.Transparent;
            this.aboutGB.Controls.Add(this.aboutTextBox);
            this.aboutGB.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.aboutGB.Location = new System.Drawing.Point(8, 6);
            this.aboutGB.Name = "aboutGB";
            this.aboutGB.Size = new System.Drawing.Size(506, 177);
            this.aboutGB.TabIndex = 1;
            this.aboutGB.TabStop = false;
            this.aboutGB.Text = "About";
            // 
            // aboutTextBox
            // 
            this.aboutTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutTextBox.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.aboutTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.aboutTextBox.Enabled = false;
            this.aboutTextBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.aboutTextBox.Location = new System.Drawing.Point(6, 19);
            this.aboutTextBox.Name = "aboutTextBox";
            this.aboutTextBox.ReadOnly = true;
            this.aboutTextBox.Size = new System.Drawing.Size(494, 152);
            this.aboutTextBox.TabIndex = 0;
            this.aboutTextBox.Text = "";
            // 
            // vnTab
            // 
            this.vnTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.vnTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.vnTab.Controls.Add(this.panel3);
            this.vnTab.Controls.Add(this.panel2);
            this.vnTab.Controls.Add(this.panel1);
            this.vnTab.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.vnTab.Location = new System.Drawing.Point(4, 22);
            this.vnTab.Name = "vnTab";
            this.vnTab.Padding = new System.Windows.Forms.Padding(3);
            this.vnTab.Size = new System.Drawing.Size(1196, 595);
            this.vnTab.TabIndex = 1;
            this.vnTab.Text = "Main";
            this.vnTab.Enter += new System.EventHandler(this.EnterMainTab);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.button2);
            this.panel3.Controls.Add(this.filterDropdown);
            this.panel3.Controls.Add(this.label24);
            this.panel3.Controls.Add(this.statusLabel);
            this.panel3.Controls.Add(this.resultLabel);
            this.panel3.Controls.Add(this.ListByCB);
            this.panel3.Controls.Add(this.tileOLV);
            this.panel3.Controls.Add(this.multiActionBox);
            this.panel3.Controls.Add(this.ListByTB);
            this.panel3.Controls.Add(this.ListByUpdateButton);
            this.panel3.Controls.Add(this.toggleViewButton);
            this.panel3.Controls.Add(this.ListByCBQuery);
            this.panel3.Controls.Add(this.viewPicker);
            this.panel3.Controls.Add(this.replyText);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.listResultsButton);
            this.panel3.Controls.Add(this.ListByGoButton);
            this.panel3.Controls.Add(this.SearchingAndFilteringButton);
            this.panel3.Location = new System.Drawing.Point(6, 252);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1182, 335);
            this.panel3.TabIndex = 109;
            // 
            // filterDropdown
            // 
            this.filterDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterDropdown.ForeColor = System.Drawing.Color.Black;
            this.filterDropdown.FormattingEnabled = true;
            this.filterDropdown.Location = new System.Drawing.Point(291, 32);
            this.filterDropdown.Name = "filterDropdown";
            this.filterDropdown.Size = new System.Drawing.Size(121, 21);
            this.filterDropdown.TabIndex = 110;
            this.filterDropdown.SelectedIndexChanged += new System.EventHandler(this.FilterChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.ForeColor = System.Drawing.Color.White;
            this.label24.Location = new System.Drawing.Point(251, 35);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(34, 13);
            this.label24.TabIndex = 109;
            this.label24.Text = "Filters";
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.Gray;
            this.statusLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.statusLabel.Location = new System.Drawing.Point(-2, -1);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(148, 57);
            this.statusLabel.TabIndex = 80;
            this.statusLabel.Text = "(statusLabel)";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // resultLabel
            // 
            this.resultLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resultLabel.BackColor = System.Drawing.Color.Transparent;
            this.resultLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.resultLabel.Location = new System.Drawing.Point(930, 35);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(247, 21);
            this.resultLabel.TabIndex = 43;
            this.resultLabel.Text = "(resultLabel)";
            this.resultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tileOLV
            // 
            this.tileOLV.AllColumns.Add(this.tileColumnTitle);
            this.tileOLV.AllColumns.Add(this.tileColumnDate);
            this.tileOLV.AllColumns.Add(this.tileColumnProducer);
            this.tileOLV.AllColumns.Add(this.tileColumnLength);
            this.tileOLV.AllColumns.Add(this.tileColumnULS);
            this.tileOLV.AllColumns.Add(this.tileColumnULAdded);
            this.tileOLV.AllColumns.Add(this.tileColumnULNote);
            this.tileOLV.AllColumns.Add(this.tileColumnWLS);
            this.tileOLV.AllColumns.Add(this.tileColumnWLAdded);
            this.tileOLV.AllColumns.Add(this.tileColumnVote);
            this.tileOLV.AllColumns.Add(this.tileColumnRating);
            this.tileOLV.AllColumns.Add(this.tileColumnPopularity);
            this.tileOLV.AllColumns.Add(this.tileColumnUpdated);
            this.tileOLV.AllColumns.Add(this.tileColumnID);
            this.tileOLV.AlternateRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tileOLV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tileOLV.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tileOLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.tileColumnTitle,
            this.tileColumnDate,
            this.tileColumnProducer,
            this.tileColumnLength,
            this.tileColumnULS,
            this.tileColumnULAdded,
            this.tileColumnULNote,
            this.tileColumnWLS,
            this.tileColumnWLAdded,
            this.tileColumnVote,
            this.tileColumnRating,
            this.tileColumnPopularity,
            this.tileColumnUpdated,
            this.tileColumnID});
            this.tileOLV.FullRowSelect = true;
            this.tileOLV.HideSelection = false;
            this.tileOLV.Location = new System.Drawing.Point(-1, 59);
            this.tileOLV.Name = "tileOLV";
            this.tileOLV.OwnerDraw = true;
            this.tileOLV.ShowCommandMenuOnRightClick = true;
            this.tileOLV.ShowGroups = false;
            this.tileOLV.Size = new System.Drawing.Size(1182, 275);
            this.tileOLV.TabIndex = 63;
            this.tileOLV.TileSize = new System.Drawing.Size(230, 375);
            this.tileOLV.UseAlternatingBackColors = true;
            this.tileOLV.UseCellFormatEvents = true;
            this.tileOLV.UseCompatibleStateImageBehavior = false;
            this.tileOLV.UseFiltering = true;
            this.tileOLV.View = System.Windows.Forms.View.Tile;
            this.tileOLV.CellClick += new System.EventHandler<BrightIdeasSoftware.CellClickEventArgs>(this.VisualNovelDoubleClick);
            this.tileOLV.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.ShowContextMenu);
            this.tileOLV.CellToolTipShowing += new System.EventHandler<BrightIdeasSoftware.ToolTipShowingEventArgs>(this.VNToolTip);
            this.tileOLV.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.FormatVNRow);
            this.tileOLV.ItemsChanged += new System.EventHandler<BrightIdeasSoftware.ItemsChangedEventArgs>(this.OLV_ItemsChanged);
            this.tileOLV.Resize += new System.EventHandler(this.OLV_Resize);
            // 
            // tileColumnTitle
            // 
            this.tileColumnTitle.AspectName = "Title";
            this.tileColumnTitle.FillsFreeSpace = true;
            this.tileColumnTitle.IsTileViewColumn = true;
            this.tileColumnTitle.Text = "Title";
            this.tileColumnTitle.Width = 376;
            // 
            // tileColumnDate
            // 
            this.tileColumnDate.AspectName = "DateForSorting";
            this.tileColumnDate.Text = "Release Date";
            this.tileColumnDate.Width = 77;
            // 
            // tileColumnProducer
            // 
            this.tileColumnProducer.AspectName = "Producer";
            this.tileColumnProducer.IsTileViewColumn = true;
            this.tileColumnProducer.Text = "Producer";
            this.tileColumnProducer.Width = 176;
            // 
            // tileColumnLength
            // 
            this.tileColumnLength.AspectName = "LengthString";
            this.tileColumnLength.DisplayIndex = 13;
            this.tileColumnLength.Text = "Length";
            // 
            // tileColumnULS
            // 
            this.tileColumnULS.AspectName = "ULStatus";
            this.tileColumnULS.DisplayIndex = 3;
            this.tileColumnULS.Text = "Userlist";
            this.tileColumnULS.Width = 77;
            // 
            // tileColumnULAdded
            // 
            this.tileColumnULAdded.AspectName = "ULAdded";
            this.tileColumnULAdded.DisplayIndex = 4;
            this.tileColumnULAdded.Text = "Added to UL";
            this.tileColumnULAdded.Width = 80;
            // 
            // tileColumnULNote
            // 
            this.tileColumnULNote.AspectName = "ULNote";
            this.tileColumnULNote.DisplayIndex = 5;
            this.tileColumnULNote.Text = "Notes";
            // 
            // tileColumnWLS
            // 
            this.tileColumnWLS.AspectName = "WLStatus";
            this.tileColumnWLS.DisplayIndex = 6;
            this.tileColumnWLS.Text = "Wishlist";
            // 
            // tileColumnWLAdded
            // 
            this.tileColumnWLAdded.AspectName = "WLAdded";
            this.tileColumnWLAdded.DisplayIndex = 7;
            this.tileColumnWLAdded.Text = "Added to WL";
            this.tileColumnWLAdded.Width = 82;
            // 
            // tileColumnVote
            // 
            this.tileColumnVote.AspectName = "Vote";
            this.tileColumnVote.DisplayIndex = 8;
            this.tileColumnVote.Text = "Vote";
            this.tileColumnVote.ToolTipText = "The score given by you";
            // 
            // tileColumnRating
            // 
            this.tileColumnRating.AspectName = "Rating";
            this.tileColumnRating.DisplayIndex = 9;
            this.tileColumnRating.Text = "Rating";
            this.tileColumnRating.ToolTipText = "Average of votes by all users";
            // 
            // tileColumnPopularity
            // 
            this.tileColumnPopularity.AspectName = "Popularity";
            this.tileColumnPopularity.DisplayIndex = 10;
            this.tileColumnPopularity.Text = "Popularity";
            this.tileColumnPopularity.ToolTipText = "How popular in relation to most popular VN";
            // 
            // tileColumnUpdated
            // 
            this.tileColumnUpdated.AspectName = "UpdatedDate";
            this.tileColumnUpdated.DisplayIndex = 11;
            this.tileColumnUpdated.Text = "Updated";
            // 
            // tileColumnID
            // 
            this.tileColumnID.AspectName = "VNID";
            this.tileColumnID.DisplayIndex = 12;
            this.tileColumnID.Text = "VNID";
            // 
            // ListByTB
            // 
            this.ListByTB.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.ListByTB.Location = new System.Drawing.Point(293, 7);
            this.ListByTB.Name = "ListByTB";
            this.ListByTB.Size = new System.Drawing.Size(119, 20);
            this.ListByTB.TabIndex = 105;
            this.ListByTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ListByTB_KeyPress);
            this.ListByTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ListByTB_KeyUp);
            // 
            // ListByCBQuery
            // 
            this.ListByCBQuery.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ListByCBQuery.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.ListByCBQuery.BackColor = System.Drawing.Color.White;
            this.ListByCBQuery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ListByCBQuery.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ListByCBQuery.FormattingEnabled = true;
            this.ListByCBQuery.Location = new System.Drawing.Point(293, 5);
            this.ListByCBQuery.Name = "ListByCBQuery";
            this.ListByCBQuery.Size = new System.Drawing.Size(119, 21);
            this.ListByCBQuery.TabIndex = 100;
            this.ListByCBQuery.Visible = false;
            this.ListByCBQuery.SelectedIndexChanged += new System.EventHandler(this.List_Group);
            this.ListByCBQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListByCbEnter);
            // 
            // viewPicker
            // 
            this.viewPicker.BackColor = System.Drawing.Color.Navy;
            this.viewPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.viewPicker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.viewPicker.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.viewPicker.FormattingEnabled = true;
            this.viewPicker.Items.AddRange(new object[] {
            "Tile View",
            "Detailed View"});
            this.viewPicker.Location = new System.Drawing.Point(688, 7);
            this.viewPicker.Name = "viewPicker";
            this.viewPicker.Size = new System.Drawing.Size(89, 21);
            this.viewPicker.TabIndex = 77;
            this.viewPicker.SelectedIndexChanged += new System.EventHandler(this.OLVChangeView);
            // 
            // replyText
            // 
            this.replyText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.replyText.BackColor = System.Drawing.Color.Transparent;
            this.replyText.Location = new System.Drawing.Point(418, 35);
            this.replyText.Name = "replyText";
            this.replyText.Size = new System.Drawing.Size(506, 21);
            this.replyText.TabIndex = 28;
            this.replyText.Text = "(replyText)";
            this.replyText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(152, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 23);
            this.label4.TabIndex = 23;
            this.label4.Text = "List:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.olFavoriteProducers);
            this.panel2.Controls.Add(this.favoriteProducersHelpButton);
            this.panel2.Controls.Add(this.removeProducersButton);
            this.panel2.Controls.Add(this.getNewProducersButton);
            this.panel2.Controls.Add(this.selectedProducersVNButton);
            this.panel2.Controls.Add(this.prodReply);
            this.panel2.Controls.Add(this.refreshAllProducersButton);
            this.panel2.Controls.Add(this.addProducersButton);
            this.panel2.Location = new System.Drawing.Point(264, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(924, 240);
            this.panel2.TabIndex = 96;
            // 
            // olFavoriteProducers
            // 
            this.olFavoriteProducers.AllColumns.Add(this.ol2Name);
            this.olFavoriteProducers.AllColumns.Add(this.ol2ItemCount);
            this.olFavoriteProducers.AllColumns.Add(this.ol2UserDropRate);
            this.olFavoriteProducers.AllColumns.Add(this.ol2UserAverageVote);
            this.olFavoriteProducers.AllColumns.Add(this.ol2GeneralRating);
            this.olFavoriteProducers.AllColumns.Add(this.ol2Updated);
            this.olFavoriteProducers.AllColumns.Add(this.ol2ID);
            this.olFavoriteProducers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olFavoriteProducers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ol2Name,
            this.ol2ItemCount,
            this.ol2UserDropRate,
            this.ol2UserAverageVote,
            this.ol2GeneralRating,
            this.ol2Updated,
            this.ol2ID});
            this.olFavoriteProducers.FullRowSelect = true;
            this.olFavoriteProducers.GridLines = true;
            this.olFavoriteProducers.Location = new System.Drawing.Point(129, 3);
            this.olFavoriteProducers.Name = "olFavoriteProducers";
            this.olFavoriteProducers.ShowGroups = false;
            this.olFavoriteProducers.Size = new System.Drawing.Size(789, 232);
            this.olFavoriteProducers.TabIndex = 0;
            this.olFavoriteProducers.UseCompatibleStateImageBehavior = false;
            this.olFavoriteProducers.View = System.Windows.Forms.View.Details;
            this.olFavoriteProducers.CellClick += new System.EventHandler<BrightIdeasSoftware.CellClickEventArgs>(this.FavoriteProducerDoubleClick);
            this.olFavoriteProducers.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.FormatRowFavoriteProducers);
            // 
            // ol2Name
            // 
            this.ol2Name.AspectName = "Name";
            this.ol2Name.FillsFreeSpace = true;
            this.ol2Name.Text = "Producer";
            this.ol2Name.Width = 279;
            // 
            // ol2ItemCount
            // 
            this.ol2ItemCount.AspectName = "NumberOfTitles";
            this.ol2ItemCount.Text = "Titles";
            this.ol2ItemCount.Width = 38;
            // 
            // ol2UserDropRate
            // 
            this.ol2UserDropRate.AspectName = "UserDropRate";
            this.ol2UserDropRate.AspectToStringFormat = "{0}%";
            this.ol2UserDropRate.DisplayIndex = 4;
            this.ol2UserDropRate.Text = "Drop Rate";
            this.ol2UserDropRate.ToolTipText = "Your rate of dropped vs finished titles.";
            this.ol2UserDropRate.Width = 63;
            // 
            // ol2UserAverageVote
            // 
            this.ol2UserAverageVote.AspectName = "UserAverageVote";
            this.ol2UserAverageVote.DisplayIndex = 2;
            this.ol2UserAverageVote.Text = "Your Score";
            this.ol2UserAverageVote.ToolTipText = "Average score given by you to this producer\'s titles.";
            this.ol2UserAverageVote.Width = 66;
            // 
            // ol2GeneralRating
            // 
            this.ol2GeneralRating.AspectName = "GeneralRating";
            this.ol2GeneralRating.DisplayIndex = 3;
            this.ol2GeneralRating.Text = "Rating";
            this.ol2GeneralRating.ToolTipText = "Average score given by all users to this producer\'s titles.";
            this.ol2GeneralRating.Width = 49;
            // 
            // ol2Updated
            // 
            this.ol2Updated.AspectName = "Updated";
            this.ol2Updated.AspectToStringFormat = "{0} days ago";
            this.ol2Updated.Text = "Updated";
            this.ol2Updated.Width = 70;
            // 
            // ol2ID
            // 
            this.ol2ID.AspectName = "ID";
            this.ol2ID.Text = "ID";
            this.ol2ID.Width = 49;
            // 
            // prodReply
            // 
            this.prodReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prodReply.Location = new System.Drawing.Point(3, 174);
            this.prodReply.Name = "prodReply";
            this.prodReply.Size = new System.Drawing.Size(126, 61);
            this.prodReply.TabIndex = 32;
            this.prodReply.Text = "(prodReply)";
            this.prodReply.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.otherMethodsCB);
            this.panel1.Controls.Add(this.yearLimitBox);
            this.panel1.Controls.Add(this.GetStartedHelpButton);
            this.panel1.Controls.Add(this.autoUpdateURTBox);
            this.panel1.Controls.Add(this.updateTagsAndTraitsButton);
            this.panel1.Controls.Add(this.nsfwToggle);
            this.panel1.Controls.Add(this.closeAllFormsButton);
            this.panel1.Controls.Add(this.loginButton);
            this.panel1.Controls.Add(this.userListButt);
            this.panel1.Controls.Add(this.userListReply);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(252, 240);
            this.panel1.TabIndex = 94;
            // 
            // otherMethodsCB
            // 
            this.otherMethodsCB.BackColor = System.Drawing.Color.MistyRose;
            this.otherMethodsCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.otherMethodsCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.otherMethodsCB.ForeColor = System.Drawing.Color.Black;
            this.otherMethodsCB.FormattingEnabled = true;
            this.otherMethodsCB.Items.AddRange(new object[] {
            "Other Functions",
            "----------------",
            "Get Missing Covers",
            "Update Tags/Traits/Stats (All)",
            "Updata All Data (All)",
            "Get Producer Languages"});
            this.otherMethodsCB.Location = new System.Drawing.Point(3, 131);
            this.otherMethodsCB.Name = "otherMethodsCB";
            this.otherMethodsCB.Size = new System.Drawing.Size(109, 21);
            this.otherMethodsCB.TabIndex = 92;
            this.otherMethodsCB.SelectedIndexChanged += new System.EventHandler(this.OtherMethodChosen);
            // 
            // nsfwToggle
            // 
            this.nsfwToggle.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.nsfwToggle.Location = new System.Drawing.Point(118, 7);
            this.nsfwToggle.Name = "nsfwToggle";
            this.nsfwToggle.Size = new System.Drawing.Size(127, 17);
            this.nsfwToggle.TabIndex = 80;
            this.nsfwToggle.Text = "Show NSFW Images";
            this.nsfwToggle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.nsfwToggle.UseVisualStyleBackColor = true;
            this.nsfwToggle.Click += new System.EventHandler(this.ToggleNSFWImages);
            // 
            // loginButton
            // 
            this.loginButton.BackColor = System.Drawing.Color.MistyRose;
            this.loginButton.FlatAppearance.BorderSize = 0;
            this.loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginButton.ForeColor = System.Drawing.Color.Black;
            this.loginButton.Location = new System.Drawing.Point(3, 3);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(109, 23);
            this.loginButton.TabIndex = 82;
            this.loginButton.Text = "Log In";
            this.loginButton.UseVisualStyleBackColor = false;
            this.loginButton.Click += new System.EventHandler(this.LogInDialog);
            // 
            // userListButt
            // 
            this.userListButt.BackColor = System.Drawing.Color.MistyRose;
            this.userListButt.FlatAppearance.BorderSize = 0;
            this.userListButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.userListButt.ForeColor = System.Drawing.Color.Black;
            this.userListButt.Location = new System.Drawing.Point(3, 32);
            this.userListButt.Name = "userListButt";
            this.userListButt.Size = new System.Drawing.Size(109, 23);
            this.userListButt.TabIndex = 27;
            this.userListButt.Text = "Update List";
            this.userListButt.UseVisualStyleBackColor = false;
            this.userListButt.Click += new System.EventHandler(this.UpdateURTButtonClick);
            // 
            // userListReply
            // 
            this.userListReply.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.userListReply.Location = new System.Drawing.Point(3, 155);
            this.userListReply.Name = "userListReply";
            this.userListReply.Size = new System.Drawing.Size(109, 80);
            this.userListReply.TabIndex = 28;
            this.userListReply.Text = "(userListReply)";
            this.userListReply.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // TabsControl
            // 
            this.TabsControl.Controls.Add(this.vnTab);
            this.TabsControl.Controls.Add(this.infoTab);
            this.TabsControl.Controls.Add(this.filtersTab);
            this.TabsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabsControl.Location = new System.Drawing.Point(0, 0);
            this.TabsControl.Name = "TabsControl";
            this.TabsControl.Padding = new System.Drawing.Point(0, 0);
            this.TabsControl.SelectedIndex = 0;
            this.TabsControl.Size = new System.Drawing.Size(1204, 621);
            this.TabsControl.TabIndex = 0;
            this.TabsControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CloseTabMiddleClick);
            // 
            // filtersTab
            // 
            this.filtersTab.Location = new System.Drawing.Point(4, 22);
            this.filtersTab.Name = "filtersTab";
            this.filtersTab.Padding = new System.Windows.Forms.Padding(3);
            this.filtersTab.Size = new System.Drawing.Size(1196, 595);
            this.filtersTab.TabIndex = 3;
            this.filtersTab.Text = "Filters";
            this.filtersTab.UseVisualStyleBackColor = true;
            this.filtersTab.Leave += new System.EventHandler(this.OnFiltersLeave);
            // 
            // ContextMenuVN
            // 
            this.ContextMenuVN.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userlistToolStripMenuItem,
            this.wishlistToolStripMenuItem,
            this.voteToolStripMenuItem,
            this.toolStripSeparator1,
            this.showProducerTitlesToolStripMenuItem,
            this.addProducerToFavoritesToolStripMenuItem,
            this.addChangeVNNoteToolStripMenuItem,
            this.addChangeVNGroupsToolStripMenuItem});
            this.ContextMenuVN.Name = "contextMenuStrip1";
            this.ContextMenuVN.Size = new System.Drawing.Size(214, 164);
            // 
            // userlistToolStripMenuItem
            // 
            this.userlistToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem,
            this.unknownToolStripMenuItem,
            this.playingToolStripMenuItem,
            this.finishedToolStripMenuItem,
            this.stalledToolStripMenuItem,
            this.droppedToolStripMenuItem});
            this.userlistToolStripMenuItem.Name = "userlistToolStripMenuItem";
            this.userlistToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.userlistToolStripMenuItem.Text = "Userlist";
            this.userlistToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.RightClickChangeVNStatus);
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.noneToolStripMenuItem.Text = "None";
            // 
            // unknownToolStripMenuItem
            // 
            this.unknownToolStripMenuItem.Name = "unknownToolStripMenuItem";
            this.unknownToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.unknownToolStripMenuItem.Text = "Unknown";
            // 
            // playingToolStripMenuItem
            // 
            this.playingToolStripMenuItem.Name = "playingToolStripMenuItem";
            this.playingToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.playingToolStripMenuItem.Text = "Playing";
            // 
            // finishedToolStripMenuItem
            // 
            this.finishedToolStripMenuItem.Name = "finishedToolStripMenuItem";
            this.finishedToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.finishedToolStripMenuItem.Text = "Finished";
            // 
            // stalledToolStripMenuItem
            // 
            this.stalledToolStripMenuItem.Name = "stalledToolStripMenuItem";
            this.stalledToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.stalledToolStripMenuItem.Text = "Stalled";
            // 
            // droppedToolStripMenuItem
            // 
            this.droppedToolStripMenuItem.Name = "droppedToolStripMenuItem";
            this.droppedToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.droppedToolStripMenuItem.Text = "Dropped";
            // 
            // wishlistToolStripMenuItem
            // 
            this.wishlistToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem1,
            this.highToolStripMenuItem,
            this.mediumToolStripMenuItem,
            this.lowToolStripMenuItem,
            this.blacklistToolStripMenuItem});
            this.wishlistToolStripMenuItem.Name = "wishlistToolStripMenuItem";
            this.wishlistToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.wishlistToolStripMenuItem.Text = "Wishlist";
            this.wishlistToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.RightClickChangeVNStatus);
            // 
            // noneToolStripMenuItem1
            // 
            this.noneToolStripMenuItem1.Name = "noneToolStripMenuItem1";
            this.noneToolStripMenuItem1.Size = new System.Drawing.Size(119, 22);
            this.noneToolStripMenuItem1.Text = "None";
            // 
            // highToolStripMenuItem
            // 
            this.highToolStripMenuItem.Name = "highToolStripMenuItem";
            this.highToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.highToolStripMenuItem.Text = "High";
            // 
            // mediumToolStripMenuItem
            // 
            this.mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
            this.mediumToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.mediumToolStripMenuItem.Text = "Medium";
            // 
            // lowToolStripMenuItem
            // 
            this.lowToolStripMenuItem.Name = "lowToolStripMenuItem";
            this.lowToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.lowToolStripMenuItem.Text = "Low";
            // 
            // blacklistToolStripMenuItem
            // 
            this.blacklistToolStripMenuItem.Name = "blacklistToolStripMenuItem";
            this.blacklistToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.blacklistToolStripMenuItem.Text = "Blacklist";
            // 
            // voteToolStripMenuItem
            // 
            this.voteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem2,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.toolStripMenuItem9,
            this.toolStripMenuItem10,
            this.toolStripMenuItem11,
            this.preciseNumberToolStripMenuItem});
            this.voteToolStripMenuItem.Name = "voteToolStripMenuItem";
            this.voteToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.voteToolStripMenuItem.Text = "Vote";
            this.voteToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.RightClickChangeVNStatus);
            // 
            // noneToolStripMenuItem2
            // 
            this.noneToolStripMenuItem2.Name = "noneToolStripMenuItem2";
            this.noneToolStripMenuItem2.Size = new System.Drawing.Size(158, 22);
            this.noneToolStripMenuItem2.Text = "None";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem2.Text = "1";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem3.Text = "2";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem4.Text = "3";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem5.Text = "4";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem6.Text = "5";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem7.Text = "6";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem8.Text = "7";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem9.Text = "8";
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem10.Text = "9";
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem11.Text = "10";
            // 
            // preciseNumberToolStripMenuItem
            // 
            this.preciseNumberToolStripMenuItem.Name = "preciseNumberToolStripMenuItem";
            this.preciseNumberToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.preciseNumberToolStripMenuItem.Text = "Precise Number";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(210, 6);
            // 
            // showProducerTitlesToolStripMenuItem
            // 
            this.showProducerTitlesToolStripMenuItem.Name = "showProducerTitlesToolStripMenuItem";
            this.showProducerTitlesToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.showProducerTitlesToolStripMenuItem.Text = "Show Producer Titles";
            this.showProducerTitlesToolStripMenuItem.Click += new System.EventHandler(this.RightClickShowProducerTitles);
            // 
            // addProducerToFavoritesToolStripMenuItem
            // 
            this.addProducerToFavoritesToolStripMenuItem.Name = "addProducerToFavoritesToolStripMenuItem";
            this.addProducerToFavoritesToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.addProducerToFavoritesToolStripMenuItem.Text = "Add Producer To Favorites";
            this.addProducerToFavoritesToolStripMenuItem.Click += new System.EventHandler(this.RightClickAddProducer);
            // 
            // addChangeVNNoteToolStripMenuItem
            // 
            this.addChangeVNNoteToolStripMenuItem.Enabled = false;
            this.addChangeVNNoteToolStripMenuItem.Name = "addChangeVNNoteToolStripMenuItem";
            this.addChangeVNNoteToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.addChangeVNNoteToolStripMenuItem.Text = "Add/Change VN Note";
            this.addChangeVNNoteToolStripMenuItem.ToolTipText = "Only for titles in Userlist";
            this.addChangeVNNoteToolStripMenuItem.Click += new System.EventHandler(this.RightClickAddNote);
            // 
            // addChangeVNGroupsToolStripMenuItem
            // 
            this.addChangeVNGroupsToolStripMenuItem.Enabled = false;
            this.addChangeVNGroupsToolStripMenuItem.Name = "addChangeVNGroupsToolStripMenuItem";
            this.addChangeVNGroupsToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.addChangeVNGroupsToolStripMenuItem.Text = "Add/Change VN Groups";
            this.addChangeVNGroupsToolStripMenuItem.ToolTipText = "Only for titles in Userlist";
            this.addChangeVNGroupsToolStripMenuItem.Click += new System.EventHandler(this.RightClickAddGroup);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1204, 621);
            this.Controls.Add(this.TabsControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(50, 50);
            this.MinimumSize = new System.Drawing.Size(1220, 660);
            this.Name = "FormMain";
            this.Text = "Happy Search";
            this.Load += new System.EventHandler(this.OnLoadRoutines);
            this.infoTab.ResumeLayout(false);
            this.logGB.ResumeLayout(false);
            this.logGB.PerformLayout();
            this.statBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.aboutGB.ResumeLayout(false);
            this.vnTab.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tileOLV)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olFavoriteProducers)).EndInit();
            this.panel1.ResumeLayout(false);
            this.TabsControl.ResumeLayout(false);
            this.ContextMenuVN.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        private ToolTip toolTip;
        private TabPage infoTab;
        private GroupBox aboutGB;
        private RichTextBox aboutTextBox;
        private TabPage vnTab;
        private Label userListReply;
        private Button userListButt;
        private Label resultLabel;
        private Label label4;
        private GroupBox statBox;
        private Label dbs9r;
        private Label dbs8r;
        private Label dbs7r;
        private Label dbs6r;
        private Label dbs5r;
        private Label dbs4r;
        private Label dbs3r;
        private Label dbs2r;
        private Label dbs1r;
        private Label dbs9;
        private Label dbs8;
        private Label dbs7;
        private Label dbs6;
        private Label dbs5;
        private Label dbs4;
        private Label dbs3;
        private Label dbs2;
        private Label dbs1;
        private GroupBox groupBox1;
        private Label mctULLabel10;
        private Label mctULLabel9;
        private Label mctULLabel8;
        private Label mctULLabel7;
        private Label mctULLabel6;
        private Label mctULLabel5;
        private Label mctULLabel4;
        private Label mctULLabel3;
        private Label mctULLabel2;
        private Label mctULLabel1;
        internal CheckBox tagTypeS2;
        internal CheckBox tagTypeT2;
        internal CheckBox tagTypeC2;
        private Label label13;
        private Label ulstatsavs;
        private Label label12;
        private Label ulstatsvl;
        private Label ulstatswl;
        private Label ulstatsul;
        private Label ulstatsall;
        private Label label11;
        private Label label10;
        private Label label9;
        private Label label8;
        private ContextMenuStrip ContextMenuVN;
        private ToolStripMenuItem userlistToolStripMenuItem;
        private ToolStripMenuItem noneToolStripMenuItem;
        private ToolStripMenuItem unknownToolStripMenuItem;
        private ToolStripMenuItem playingToolStripMenuItem;
        private ToolStripMenuItem finishedToolStripMenuItem;
        private ToolStripMenuItem stalledToolStripMenuItem;
        private ToolStripMenuItem droppedToolStripMenuItem;
        private ToolStripMenuItem wishlistToolStripMenuItem;
        private ToolStripMenuItem noneToolStripMenuItem1;
        private ToolStripMenuItem highToolStripMenuItem;
        private ToolStripMenuItem mediumToolStripMenuItem;
        private ToolStripMenuItem lowToolStripMenuItem;
        private ToolStripMenuItem blacklistToolStripMenuItem;
        private ToolStripMenuItem voteToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem toolStripMenuItem9;
        private ToolStripMenuItem toolStripMenuItem10;
        private ToolStripMenuItem toolStripMenuItem11;
        private ToolStripMenuItem noneToolStripMenuItem2;
        internal ObjectListView olFavoriteProducers;
        private OLVColumn ol2Name;
        private OLVColumn ol2ItemCount;
        private OLVColumn ol2Updated;
        private OLVColumn ol2ID;
        private Button selectedProducersVNButton;
        private Button removeProducersButton;
        private Button addProducersButton;
        private Label prodReply;
        private Label replyText;
        private ComboBox viewPicker;
        private ObjectListView tileOLV;
        private OLVColumn tileColumnTitle;
        private OLVColumn tileColumnDate;
        private OLVColumn tileColumnProducer;
        private OLVColumn tileColumnULS;
        private OLVColumn tileColumnULAdded;
        private OLVColumn tileColumnULNote;
        private OLVColumn tileColumnWLS;
        private OLVColumn tileColumnWLAdded;
        private OLVColumn tileColumnVote;
        private OLVColumn tileColumnUpdated;
        private OLVColumn tileColumnID;
        private CheckBox nsfwToggle;
        private Button closeAllFormsButton;
        private Button getNewProducersButton;
        private Button loginButton;
        private CheckBox autoUpdateURTBox;
        private Label statusLabel;
        private CheckBox yearLimitBox;
        private OLVColumn ol2UserAverageVote;
        private OLVColumn ol2UserDropRate;
        private RichTextBox questionBox;
        private GroupBox logGB;
        private Label logQueryLabel;
        private Label label1;
        internal RichTextBox serverR;
        private Label logReplyLabel;
        private Button clearLogButton;
        private RichTextBox serverQ;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem showProducerTitlesToolStripMenuItem;
        private Button favoriteProducersHelpButton;
        private ToolStripMenuItem addProducerToFavoritesToolStripMenuItem;
        private Button GetStartedHelpButton;
        private Button SearchingAndFilteringButton;
        private OLVColumn tileColumnRating;
        private OLVColumn tileColumnPopularity;
        private Panel panel1;
        private Button updateTagsAndTraitsButton;
        private Panel panel2;
        private Button listResultsButton;
        private OLVColumn ol2GeneralRating;
        private ComboBox otherMethodsCB;
        private ToolStripMenuItem addChangeVNNoteToolStripMenuItem;
        private ToolStripMenuItem addChangeVNGroupsToolStripMenuItem;
        private Button sendQueryButton;
        private CheckBox advancedCheckBox;
        internal ComboBox ListByCBQuery;
        private ComboBox ListByCB;
        private TextBox ListByTB;
        private Button ListByUpdateButton;
        private Button ListByGoButton;
        private Button refreshAllProducersButton;
        private Button toggleViewButton;
        private ComboBox multiActionBox;
        private ToolStripMenuItem preciseNumberToolStripMenuItem;
        internal TabControl TabsControl;
        private Button button1;
        private OLVColumn tileColumnLength;
        private TabPage filtersTab;
        private Panel panel3;
        private Label label24;
        internal ComboBox filterDropdown;
        private Button button2;
    }
}

