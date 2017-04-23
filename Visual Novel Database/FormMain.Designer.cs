using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;
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
            this.traitSearchBox = new System.Windows.Forms.TextBox();
            this.customTraitFilterNameBox = new System.Windows.Forms.TextBox();
            this.traitSearchButton = new System.Windows.Forms.Button();
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
            this.tagSearchButton = new System.Windows.Forms.Button();
            this.tagSearchBox = new System.Windows.Forms.TextBox();
            this.TagFilteringHelpButton = new System.Windows.Forms.Button();
            this.updateFilterResultsButton = new System.Windows.Forms.Button();
            this.customTagFilterNameBox = new System.Windows.Forms.TextBox();
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.ListByTB = new System.Windows.Forms.TextBox();
            this.viewPicker = new System.Windows.Forms.ComboBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.replyText = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.ListByCBQuery = new System.Windows.Forms.ComboBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.traitFilteringBox = new System.Windows.Forms.TabPage();
            this.traitSearchResultBox = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.traitFilteringHelpButton = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.customTraitFilters = new System.Windows.Forms.ComboBox();
            this.button5 = new System.Windows.Forms.Button();
            this.deleteCustomTraitFilterButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.traitReply = new System.Windows.Forms.Label();
            this.traitRootsDropdown = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.otherMethodsCB = new System.Windows.Forms.ComboBox();
            this.nsfwToggle = new System.Windows.Forms.CheckBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.userListButt = new System.Windows.Forms.Button();
            this.userListReply = new System.Windows.Forms.Label();
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
            this.TabsControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tagsOrTraits = new System.Windows.Forms.CheckBox();
            this.tagSearchResultBox = new System.Windows.Forms.ListBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label24 = new System.Windows.Forms.Label();
            this.languageTFCB = new System.Windows.Forms.ComboBox();
            this.languageFixed = new System.Windows.Forms.CheckBox();
            this.languageTF = new System.Windows.Forms.CheckBox();
            this.languageTFLB = new System.Windows.Forms.ListBox();
            this.label21 = new System.Windows.Forms.Label();
            this.filterDropdown = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.traitsPanel = new System.Windows.Forms.Panel();
            this.traitsFixed = new System.Windows.Forms.CheckBox();
            this.traitsTF = new System.Windows.Forms.CheckBox();
            this.button7 = new System.Windows.Forms.Button();
            this.traitsTFLB = new System.Windows.Forms.ListBox();
            this.label18 = new System.Windows.Forms.Label();
            this.tagsPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.tagReply = new System.Windows.Forms.Label();
            this.saveCustomFilterButton = new System.Windows.Forms.Button();
            this.customTagFilters = new System.Windows.Forms.ComboBox();
            this.clearFilterButton = new System.Windows.Forms.Button();
            this.deleteCustomTagFilterButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.tagsFixed = new System.Windows.Forms.CheckBox();
            this.tagsTF = new System.Windows.Forms.CheckBox();
            this.button6 = new System.Windows.Forms.Button();
            this.tagsTFLB = new System.Windows.Forms.ListBox();
            this.label30 = new System.Windows.Forms.Label();
            this.panel13 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.originalLanguageTFCB = new System.Windows.Forms.ComboBox();
            this.originalLanguageFixed = new System.Windows.Forms.CheckBox();
            this.originalLanguageTF = new System.Windows.Forms.CheckBox();
            this.originalLanguageTFLB = new System.Windows.Forms.ListBox();
            this.label20 = new System.Windows.Forms.Label();
            this.userlistPanel = new System.Windows.Forms.Panel();
            this.userlistFixed = new System.Windows.Forms.CheckBox();
            this.userlistTF = new System.Windows.Forms.CheckBox();
            this.userlistTFDropped = new System.Windows.Forms.CheckBox();
            this.userlistTFStalled = new System.Windows.Forms.CheckBox();
            this.userlistTFFinished = new System.Windows.Forms.CheckBox();
            this.userlistTFPlaying = new System.Windows.Forms.CheckBox();
            this.userlistTFUnknown = new System.Windows.Forms.CheckBox();
            this.userlistTFNa = new System.Windows.Forms.CheckBox();
            this.label23 = new System.Windows.Forms.Label();
            this.wishlistPanel = new System.Windows.Forms.Panel();
            this.wishlistFixed = new System.Windows.Forms.CheckBox();
            this.wishlistTF = new System.Windows.Forms.CheckBox();
            this.wishlistTFLow = new System.Windows.Forms.CheckBox();
            this.wishlistTFMedium = new System.Windows.Forms.CheckBox();
            this.wishlistTFHigh = new System.Windows.Forms.CheckBox();
            this.wishlistTFNa = new System.Windows.Forms.CheckBox();
            this.label26 = new System.Windows.Forms.Label();
            this.favoriteProducersPanel = new System.Windows.Forms.Panel();
            this.favoriteProducersFixed = new System.Windows.Forms.CheckBox();
            this.favoriteProducersTF = new System.Windows.Forms.CheckBox();
            this.favoriteProducersTFNo = new System.Windows.Forms.RadioButton();
            this.favoriteProducersTFYes = new System.Windows.Forms.RadioButton();
            this.label25 = new System.Windows.Forms.Label();
            this.votedPanel = new System.Windows.Forms.Panel();
            this.votedFixed = new System.Windows.Forms.CheckBox();
            this.votedTF = new System.Windows.Forms.CheckBox();
            this.votedTFNo = new System.Windows.Forms.RadioButton();
            this.votedTFYes = new System.Windows.Forms.RadioButton();
            this.label17 = new System.Windows.Forms.Label();
            this.blacklistedPanel = new System.Windows.Forms.Panel();
            this.blacklistedFixed = new System.Windows.Forms.CheckBox();
            this.blacklistedTF = new System.Windows.Forms.CheckBox();
            this.blacklistedTFNo = new System.Windows.Forms.RadioButton();
            this.blacklistedTFYes = new System.Windows.Forms.RadioButton();
            this.label16 = new System.Windows.Forms.Label();
            this.unreleasedPanel = new System.Windows.Forms.Panel();
            this.unreleasedFixed = new System.Windows.Forms.CheckBox();
            this.unreleasedTFReleased = new System.Windows.Forms.CheckBox();
            this.unreleasedTF = new System.Windows.Forms.CheckBox();
            this.unreleasedTFWithoutReleaseDate = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.unreleasedTFWithReleaseDate = new System.Windows.Forms.CheckBox();
            this.releaseDatePanel = new System.Windows.Forms.Panel();
            this.releaseDateTFResponse = new System.Windows.Forms.Label();
            this.releaseDateFixed = new System.Windows.Forms.CheckBox();
            this.releaseDateTF = new System.Windows.Forms.CheckBox();
            this.releaseDateTFToYear = new System.Windows.Forms.NumericUpDown();
            this.label27 = new System.Windows.Forms.Label();
            this.releaseDateTFToMonth = new System.Windows.Forms.DomainUpDown();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.releaseDateTFToDay = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.releaseDateTFFromYear = new System.Windows.Forms.NumericUpDown();
            this.label34 = new System.Windows.Forms.Label();
            this.releaseDateTFFromMonth = new System.Windows.Forms.DomainUpDown();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.releaseDateTFFromDay = new System.Windows.Forms.NumericUpDown();
            this.label31 = new System.Windows.Forms.Label();
            this.lengthPanel = new System.Windows.Forms.Panel();
            this.lengthFixed = new System.Windows.Forms.CheckBox();
            this.lengthTF = new System.Windows.Forms.CheckBox();
            this.lengthTFOverFiftyHours = new System.Windows.Forms.CheckBox();
            this.lengthTFThirtyToFiftyHours = new System.Windows.Forms.CheckBox();
            this.lengthTFTenToThirtyHours = new System.Windows.Forms.CheckBox();
            this.lengthTFTwoToTenHours = new System.Windows.Forms.CheckBox();
            this.lengthTFUnderTwoHours = new System.Windows.Forms.CheckBox();
            this.lengthTFNa = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
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
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olFavoriteProducers)).BeginInit();
            this.panel3.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.traitFilteringBox.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tileOLV)).BeginInit();
            this.TabsControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.traitsPanel.SuspendLayout();
            this.tagsPanel.SuspendLayout();
            this.panel13.SuspendLayout();
            this.userlistPanel.SuspendLayout();
            this.wishlistPanel.SuspendLayout();
            this.favoriteProducersPanel.SuspendLayout();
            this.votedPanel.SuspendLayout();
            this.blacklistedPanel.SuspendLayout();
            this.unreleasedPanel.SuspendLayout();
            this.releaseDatePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.releaseDateTFToYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.releaseDateTFToDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.releaseDateTFFromYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.releaseDateTFFromDay)).BeginInit();
            this.lengthPanel.SuspendLayout();
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
            this.getNewProducersButton.Location = new System.Drawing.Point(129, 270);
            this.getNewProducersButton.Name = "getNewProducersButton";
            this.getNewProducersButton.Size = new System.Drawing.Size(85, 23);
            this.getNewProducersButton.TabIndex = 37;
            this.getNewProducersButton.Text = "Get New Titles";
            this.toolTip.SetToolTip(this.getNewProducersButton, "Get titles by favorite producers that aren\'t in local database yet.");
            this.getNewProducersButton.UseVisualStyleBackColor = false;
            this.getNewProducersButton.Click += new System.EventHandler(this.GetNewFavoriteProducerTitles);
            // 
            // autoUpdateURTBox
            // 
            this.autoUpdateURTBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.autoUpdateURTBox.Location = new System.Drawing.Point(3, 205);
            this.autoUpdateURTBox.Name = "autoUpdateURTBox";
            this.autoUpdateURTBox.Size = new System.Drawing.Size(131, 17);
            this.autoUpdateURTBox.TabIndex = 83;
            this.autoUpdateURTBox.Text = "Auto-Update URT";
            this.autoUpdateURTBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.autoUpdateURTBox.UseVisualStyleBackColor = true;
            this.autoUpdateURTBox.Click += new System.EventHandler(this.ToggleAutoUpdateURT);
            // 
            // yearLimitBox
            // 
            this.yearLimitBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.yearLimitBox.Location = new System.Drawing.Point(3, 223);
            this.yearLimitBox.Name = "yearLimitBox";
            this.yearLimitBox.Size = new System.Drawing.Size(131, 17);
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
            this.addProducersButton.Location = new System.Drawing.Point(129, 241);
            this.addProducersButton.Name = "addProducersButton";
            this.addProducersButton.Size = new System.Drawing.Size(85, 23);
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
            this.selectedProducersVNButton.Location = new System.Drawing.Point(3, 270);
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
            this.removeProducersButton.Location = new System.Drawing.Point(220, 241);
            this.removeProducersButton.Name = "removeProducersButton";
            this.removeProducersButton.Size = new System.Drawing.Size(105, 23);
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
            this.updateTagsAndTraitsButton.Location = new System.Drawing.Point(-1, 61);
            this.updateTagsAndTraitsButton.Name = "updateTagsAndTraitsButton";
            this.updateTagsAndTraitsButton.Size = new System.Drawing.Size(139, 23);
            this.updateTagsAndTraitsButton.TabIndex = 91;
            this.updateTagsAndTraitsButton.Text = "Update Tags/Traits/Stats";
            this.toolTip.SetToolTip(this.updateTagsAndTraitsButton, "Update tags, traits and stats of titles that haven\'t been updated in over 7 days." +
        "");
            this.updateTagsAndTraitsButton.UseVisualStyleBackColor = false;
            this.updateTagsAndTraitsButton.Click += new System.EventHandler(this.UpdateTagsTraitsStatsClick);
            // 
            // traitSearchBox
            // 
            this.traitSearchBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.traitSearchBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.traitSearchBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.traitSearchBox.Location = new System.Drawing.Point(185, 6);
            this.traitSearchBox.Name = "traitSearchBox";
            this.traitSearchBox.Size = new System.Drawing.Size(140, 22);
            this.traitSearchBox.TabIndex = 99;
            this.toolTip.SetToolTip(this.traitSearchBox, "Enter trait name here.");
            this.traitSearchBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AddTraitBySearch);
            // 
            // customTraitFilterNameBox
            // 
            this.customTraitFilterNameBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTraitFilterNameBox.Location = new System.Drawing.Point(442, 19);
            this.customTraitFilterNameBox.Name = "customTraitFilterNameBox";
            this.customTraitFilterNameBox.Size = new System.Drawing.Size(100, 22);
            this.customTraitFilterNameBox.TabIndex = 103;
            this.toolTip.SetToolTip(this.customTraitFilterNameBox, "Enter Custom Filter name here");
            this.customTraitFilterNameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EnterCustomTraitFilterName);
            // 
            // traitSearchButton
            // 
            this.traitSearchButton.BackColor = System.Drawing.Color.SteelBlue;
            this.traitSearchButton.FlatAppearance.BorderSize = 0;
            this.traitSearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.traitSearchButton.ForeColor = System.Drawing.Color.White;
            this.traitSearchButton.Location = new System.Drawing.Point(331, 6);
            this.traitSearchButton.Name = "traitSearchButton";
            this.traitSearchButton.Size = new System.Drawing.Size(17, 22);
            this.traitSearchButton.TabIndex = 111;
            this.traitSearchButton.Text = "S";
            this.toolTip.SetToolTip(this.traitSearchButton, "Search for a trait based on name and/or alias.");
            this.traitSearchButton.UseVisualStyleBackColor = false;
            this.traitSearchButton.Click += new System.EventHandler(this.SearchTraits);
            // 
            // ListByGoButton
            // 
            this.ListByGoButton.BackColor = System.Drawing.Color.SteelBlue;
            this.ListByGoButton.FlatAppearance.BorderSize = 0;
            this.ListByGoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ListByGoButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ListByGoButton.Location = new System.Drawing.Point(262, 0);
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
            this.ListByUpdateButton.Location = new System.Drawing.Point(298, 0);
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
            "By Group",
            "By Language"});
            this.ListByCB.Location = new System.Drawing.Point(36, 1);
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
            this.favoriteProducersHelpButton.Location = new System.Drawing.Point(2, 241);
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
            this.listResultsButton.Location = new System.Drawing.Point(659, 1);
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
            this.GetStartedHelpButton.Location = new System.Drawing.Point(3, 271);
            this.GetStartedHelpButton.Name = "GetStartedHelpButton";
            this.GetStartedHelpButton.Size = new System.Drawing.Size(131, 23);
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
            this.closeAllFormsButton.Location = new System.Drawing.Point(3, 242);
            this.closeAllFormsButton.Name = "closeAllFormsButton";
            this.closeAllFormsButton.Size = new System.Drawing.Size(131, 23);
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
            this.SearchingAndFilteringButton.Location = new System.Drawing.Point(546, 1);
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
            this.refreshAllProducersButton.Location = new System.Drawing.Point(220, 270);
            this.refreshAllProducersButton.Name = "refreshAllProducersButton";
            this.refreshAllProducersButton.Size = new System.Drawing.Size(105, 23);
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
            this.toggleViewButton.Location = new System.Drawing.Point(937, 1);
            this.toggleViewButton.Name = "toggleViewButton";
            this.toggleViewButton.Size = new System.Drawing.Size(108, 23);
            this.toggleViewButton.TabIndex = 93;
            this.toggleViewButton.Text = "▲ Hide Options ▲";
            this.toolTip.SetToolTip(this.toggleViewButton, "Show/hide settings, filtering and favorite producers sections.");
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
            this.multiActionBox.Location = new System.Drawing.Point(817, 3);
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
            this.button1.Location = new System.Drawing.Point(0, 89);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 23);
            this.button1.TabIndex = 93;
            this.button1.Text = "Update All Data";
            this.toolTip.SetToolTip(this.button1, "Update all data for titles that haven\'t been updated in over 7 days.");
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.UpdateAllDataClick);
            // 
            // tagSearchButton
            // 
            this.tagSearchButton.BackColor = System.Drawing.Color.SteelBlue;
            this.tagSearchButton.FlatAppearance.BorderSize = 0;
            this.tagSearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tagSearchButton.ForeColor = System.Drawing.Color.White;
            this.tagSearchButton.Location = new System.Drawing.Point(261, 217);
            this.tagSearchButton.Name = "tagSearchButton";
            this.tagSearchButton.Size = new System.Drawing.Size(17, 22);
            this.tagSearchButton.TabIndex = 98;
            this.tagSearchButton.Text = "S";
            this.toolTip.SetToolTip(this.tagSearchButton, "Search for a tag based on name and/or alias.");
            this.tagSearchButton.UseVisualStyleBackColor = false;
            this.tagSearchButton.Click += new System.EventHandler(this.SearchTags);
            // 
            // tagSearchBox
            // 
            this.tagSearchBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tagSearchBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tagSearchBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagSearchBox.Location = new System.Drawing.Point(53, 218);
            this.tagSearchBox.Name = "tagSearchBox";
            this.tagSearchBox.Size = new System.Drawing.Size(202, 22);
            this.tagSearchBox.TabIndex = 95;
            this.toolTip.SetToolTip(this.tagSearchBox, "Enter tag name/alias here.");
            this.tagSearchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddTagBySearch);
            // 
            // TagFilteringHelpButton
            // 
            this.TagFilteringHelpButton.BackColor = System.Drawing.Color.Khaki;
            this.TagFilteringHelpButton.FlatAppearance.BorderSize = 0;
            this.TagFilteringHelpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TagFilteringHelpButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TagFilteringHelpButton.Location = new System.Drawing.Point(284, 246);
            this.TagFilteringHelpButton.Name = "TagFilteringHelpButton";
            this.TagFilteringHelpButton.Size = new System.Drawing.Size(100, 23);
            this.TagFilteringHelpButton.TabIndex = 106;
            this.TagFilteringHelpButton.Text = "Tag Filtering";
            this.toolTip.SetToolTip(this.TagFilteringHelpButton, "Help on Tag Filtering.");
            this.TagFilteringHelpButton.UseVisualStyleBackColor = false;
            this.TagFilteringHelpButton.Click += new System.EventHandler(this.Help_TagFiltering);
            // 
            // updateFilterResultsButton
            // 
            this.updateFilterResultsButton.BackColor = System.Drawing.Color.MistyRose;
            this.updateFilterResultsButton.FlatAppearance.BorderSize = 0;
            this.updateFilterResultsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateFilterResultsButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.updateFilterResultsButton.Location = new System.Drawing.Point(284, 169);
            this.updateFilterResultsButton.Name = "updateFilterResultsButton";
            this.updateFilterResultsButton.Size = new System.Drawing.Size(100, 23);
            this.updateFilterResultsButton.TabIndex = 103;
            this.updateFilterResultsButton.Text = "Update Results";
            this.toolTip.SetToolTip(this.updateFilterResultsButton, "Get titles that match active tag filter from VNDB.");
            this.updateFilterResultsButton.UseVisualStyleBackColor = false;
            this.updateFilterResultsButton.Click += new System.EventHandler(this.UpdateTagResults);
            // 
            // customTagFilterNameBox
            // 
            this.customTagFilterNameBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTagFilterNameBox.Location = new System.Drawing.Point(284, 27);
            this.customTagFilterNameBox.Name = "customTagFilterNameBox";
            this.customTagFilterNameBox.Size = new System.Drawing.Size(100, 22);
            this.customTagFilterNameBox.TabIndex = 101;
            this.toolTip.SetToolTip(this.customTagFilterNameBox, "Enter Custom Filter name here");
            this.customTagFilterNameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EnterCustomTagFilterName);
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
            this.vnTab.Controls.Add(this.panel2);
            this.vnTab.Controls.Add(this.panel3);
            this.vnTab.Controls.Add(this.tabControl2);
            this.vnTab.Controls.Add(this.panel1);
            this.vnTab.Controls.Add(this.tileOLV);
            this.vnTab.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.vnTab.Location = new System.Drawing.Point(4, 22);
            this.vnTab.Name = "vnTab";
            this.vnTab.Padding = new System.Windows.Forms.Padding(3);
            this.vnTab.Size = new System.Drawing.Size(1196, 595);
            this.vnTab.TabIndex = 1;
            this.vnTab.Text = "Main";
            this.vnTab.Enter += new System.EventHandler(this.vnTab_Enter);
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
            this.panel2.Location = new System.Drawing.Point(725, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(463, 299);
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
            this.olFavoriteProducers.Location = new System.Drawing.Point(3, 3);
            this.olFavoriteProducers.Name = "olFavoriteProducers";
            this.olFavoriteProducers.ShowGroups = false;
            this.olFavoriteProducers.Size = new System.Drawing.Size(454, 232);
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
            this.prodReply.Location = new System.Drawing.Point(331, 242);
            this.prodReply.Name = "prodReply";
            this.prodReply.Size = new System.Drawing.Size(126, 51);
            this.prodReply.TabIndex = 32;
            this.prodReply.Text = "(prodReply)";
            this.prodReply.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.multiActionBox);
            this.panel3.Controls.Add(this.toggleViewButton);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.ListByGoButton);
            this.panel3.Controls.Add(this.ListByUpdateButton);
            this.panel3.Controls.Add(this.ListByTB);
            this.panel3.Controls.Add(this.viewPicker);
            this.panel3.Controls.Add(this.ListByCB);
            this.panel3.Controls.Add(this.resultLabel);
            this.panel3.Controls.Add(this.replyText);
            this.panel3.Controls.Add(this.statusLabel);
            this.panel3.Controls.Add(this.ListByCBQuery);
            this.panel3.Controls.Add(this.SearchingAndFilteringButton);
            this.panel3.Controls.Add(this.listResultsButton);
            this.panel3.Location = new System.Drawing.Point(6, 306);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1187, 58);
            this.panel3.TabIndex = 108;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 23);
            this.label4.TabIndex = 23;
            this.label4.Text = "List:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ListByTB
            // 
            this.ListByTB.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.ListByTB.Location = new System.Drawing.Point(137, 3);
            this.ListByTB.Name = "ListByTB";
            this.ListByTB.Size = new System.Drawing.Size(119, 20);
            this.ListByTB.TabIndex = 105;
            this.ListByTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ListByTB_KeyPress);
            this.ListByTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ListByTB_KeyUp);
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
            this.viewPicker.Location = new System.Drawing.Point(722, 3);
            this.viewPicker.Name = "viewPicker";
            this.viewPicker.Size = new System.Drawing.Size(89, 21);
            this.viewPicker.TabIndex = 77;
            this.viewPicker.SelectedIndexChanged += new System.EventHandler(this.OLVChangeView);
            // 
            // resultLabel
            // 
            this.resultLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resultLabel.BackColor = System.Drawing.Color.Transparent;
            this.resultLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.resultLabel.Location = new System.Drawing.Point(1054, 0);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(123, 24);
            this.resultLabel.TabIndex = 43;
            this.resultLabel.Text = "(resultLabel)";
            this.resultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // replyText
            // 
            this.replyText.BackColor = System.Drawing.Color.Transparent;
            this.replyText.Location = new System.Drawing.Point(356, 2);
            this.replyText.Name = "replyText";
            this.replyText.Size = new System.Drawing.Size(184, 20);
            this.replyText.TabIndex = 28;
            this.replyText.Text = "(replyText)";
            this.replyText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.Gray;
            this.statusLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.statusLabel.Location = new System.Drawing.Point(4, 26);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(152, 30);
            this.statusLabel.TabIndex = 80;
            this.statusLabel.Text = "(statusLabel)";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ListByCBQuery
            // 
            this.ListByCBQuery.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ListByCBQuery.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.ListByCBQuery.BackColor = System.Drawing.Color.White;
            this.ListByCBQuery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ListByCBQuery.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ListByCBQuery.FormattingEnabled = true;
            this.ListByCBQuery.Items.AddRange(new object[] {
            "Show URT",
            "Hide URT",
            "Only URT",
            "Only Unplayed"});
            this.ListByCBQuery.Location = new System.Drawing.Point(137, 2);
            this.ListByCBQuery.Name = "ListByCBQuery";
            this.ListByCBQuery.Size = new System.Drawing.Size(119, 21);
            this.ListByCBQuery.TabIndex = 100;
            this.ListByCBQuery.Visible = false;
            this.ListByCBQuery.SelectedIndexChanged += new System.EventHandler(this.List_Group);
            this.ListByCBQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListByCbEnter);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.traitFilteringBox);
            this.tabControl2.Location = new System.Drawing.Point(151, 6);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(572, 299);
            this.tabControl2.TabIndex = 97;
            // 
            // traitFilteringBox
            // 
            this.traitFilteringBox.AutoScroll = true;
            this.traitFilteringBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.traitFilteringBox.Controls.Add(this.traitSearchButton);
            this.traitFilteringBox.Controls.Add(this.traitSearchResultBox);
            this.traitFilteringBox.Controls.Add(this.label5);
            this.traitFilteringBox.Controls.Add(this.traitFilteringHelpButton);
            this.traitFilteringBox.Controls.Add(this.customTraitFilterNameBox);
            this.traitFilteringBox.Controls.Add(this.button4);
            this.traitFilteringBox.Controls.Add(this.customTraitFilters);
            this.traitFilteringBox.Controls.Add(this.button5);
            this.traitFilteringBox.Controls.Add(this.deleteCustomTraitFilterButton);
            this.traitFilteringBox.Controls.Add(this.traitSearchBox);
            this.traitFilteringBox.Controls.Add(this.label3);
            this.traitFilteringBox.Controls.Add(this.traitReply);
            this.traitFilteringBox.Controls.Add(this.traitRootsDropdown);
            this.traitFilteringBox.Location = new System.Drawing.Point(4, 22);
            this.traitFilteringBox.Name = "traitFilteringBox";
            this.traitFilteringBox.Padding = new System.Windows.Forms.Padding(3);
            this.traitFilteringBox.Size = new System.Drawing.Size(564, 273);
            this.traitFilteringBox.TabIndex = 1;
            this.traitFilteringBox.Text = "Trait Filtering Box";
            this.traitFilteringBox.Click += new System.EventHandler(this.ClearTraitResults);
            // 
            // traitSearchResultBox
            // 
            this.traitSearchResultBox.FormattingEnabled = true;
            this.traitSearchResultBox.Location = new System.Drawing.Point(6, 32);
            this.traitSearchResultBox.Name = "traitSearchResultBox";
            this.traitSearchResultBox.Size = new System.Drawing.Size(342, 238);
            this.traitSearchResultBox.TabIndex = 110;
            this.traitSearchResultBox.Visible = false;
            this.traitSearchResultBox.SelectedIndexChanged += new System.EventHandler(this.AddTraitFromList);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(439, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 109;
            this.label5.Text = "Custom Filter Name:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // traitFilteringHelpButton
            // 
            this.traitFilteringHelpButton.BackColor = System.Drawing.Color.Khaki;
            this.traitFilteringHelpButton.FlatAppearance.BorderSize = 0;
            this.traitFilteringHelpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.traitFilteringHelpButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.traitFilteringHelpButton.Location = new System.Drawing.Point(442, 249);
            this.traitFilteringHelpButton.Name = "traitFilteringHelpButton";
            this.traitFilteringHelpButton.Size = new System.Drawing.Size(100, 23);
            this.traitFilteringHelpButton.TabIndex = 108;
            this.traitFilteringHelpButton.Text = "Trait Filtering";
            this.traitFilteringHelpButton.UseVisualStyleBackColor = false;
            this.traitFilteringHelpButton.Click += new System.EventHandler(this.Help_TraitFiltering);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.SteelBlue;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(442, 74);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 23);
            this.button4.TabIndex = 101;
            this.button4.Text = "Save Filter";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.SaveCustomTraitFilter);
            // 
            // customTraitFilters
            // 
            this.customTraitFilters.BackColor = System.Drawing.Color.SteelBlue;
            this.customTraitFilters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.customTraitFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customTraitFilters.ForeColor = System.Drawing.Color.White;
            this.customTraitFilters.FormattingEnabled = true;
            this.customTraitFilters.Items.AddRange(new object[] {
            "Custom Filters",
            "----------"});
            this.customTraitFilters.Location = new System.Drawing.Point(442, 47);
            this.customTraitFilters.Name = "customTraitFilters";
            this.customTraitFilters.Size = new System.Drawing.Size(100, 21);
            this.customTraitFilters.TabIndex = 106;
            this.customTraitFilters.SelectedIndexChanged += new System.EventHandler(this.Filter_CustomTraits);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.SteelBlue;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(442, 132);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(100, 23);
            this.button5.TabIndex = 102;
            this.button5.Text = "Clear Filter";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.ClearTraitFilter);
            // 
            // deleteCustomTraitFilterButton
            // 
            this.deleteCustomTraitFilterButton.BackColor = System.Drawing.Color.SteelBlue;
            this.deleteCustomTraitFilterButton.Enabled = false;
            this.deleteCustomTraitFilterButton.FlatAppearance.BorderSize = 0;
            this.deleteCustomTraitFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteCustomTraitFilterButton.ForeColor = System.Drawing.Color.White;
            this.deleteCustomTraitFilterButton.Location = new System.Drawing.Point(442, 103);
            this.deleteCustomTraitFilterButton.Name = "deleteCustomTraitFilterButton";
            this.deleteCustomTraitFilterButton.Size = new System.Drawing.Size(100, 23);
            this.deleteCustomTraitFilterButton.TabIndex = 107;
            this.deleteCustomTraitFilterButton.Text = "Delete Filter";
            this.deleteCustomTraitFilterButton.UseVisualStyleBackColor = false;
            this.deleteCustomTraitFilterButton.Click += new System.EventHandler(this.DeleteCustomTraitFilter);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 21);
            this.label3.TabIndex = 100;
            this.label3.Text = "Search:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // traitReply
            // 
            this.traitReply.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.traitReply.Location = new System.Drawing.Point(442, 158);
            this.traitReply.Name = "traitReply";
            this.traitReply.Size = new System.Drawing.Size(100, 88);
            this.traitReply.TabIndex = 92;
            this.traitReply.Text = "(traitReply)";
            this.traitReply.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // traitRootsDropdown
            // 
            this.traitRootsDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.traitRootsDropdown.FormattingEnabled = true;
            this.traitRootsDropdown.Location = new System.Drawing.Point(53, 6);
            this.traitRootsDropdown.Name = "traitRootsDropdown";
            this.traitRootsDropdown.Size = new System.Drawing.Size(126, 21);
            this.traitRootsDropdown.TabIndex = 0;
            this.traitRootsDropdown.SelectedIndexChanged += new System.EventHandler(this.TraitRootChanged);
            // 
            // panel1
            // 
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
            this.panel1.Size = new System.Drawing.Size(139, 299);
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
            this.otherMethodsCB.Location = new System.Drawing.Point(-1, 120);
            this.otherMethodsCB.Name = "otherMethodsCB";
            this.otherMethodsCB.Size = new System.Drawing.Size(140, 21);
            this.otherMethodsCB.TabIndex = 92;
            this.otherMethodsCB.SelectedIndexChanged += new System.EventHandler(this.OtherMethodChosen);
            // 
            // nsfwToggle
            // 
            this.nsfwToggle.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.nsfwToggle.Location = new System.Drawing.Point(3, 187);
            this.nsfwToggle.Name = "nsfwToggle";
            this.nsfwToggle.Size = new System.Drawing.Size(131, 17);
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
            this.loginButton.Location = new System.Drawing.Point(-1, 3);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(139, 23);
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
            this.userListButt.Location = new System.Drawing.Point(-1, 32);
            this.userListButt.Name = "userListButt";
            this.userListButt.Size = new System.Drawing.Size(139, 23);
            this.userListButt.TabIndex = 27;
            this.userListButt.Text = "Update List";
            this.userListButt.UseVisualStyleBackColor = false;
            this.userListButt.Click += new System.EventHandler(this.UpdateURTButtonClick);
            // 
            // userListReply
            // 
            this.userListReply.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.userListReply.Location = new System.Drawing.Point(4, 149);
            this.userListReply.Name = "userListReply";
            this.userListReply.Size = new System.Drawing.Size(131, 35);
            this.userListReply.TabIndex = 28;
            this.userListReply.Text = "(userListReply)";
            this.userListReply.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
            this.tileOLV.Location = new System.Drawing.Point(6, 365);
            this.tileOLV.Name = "tileOLV";
            this.tileOLV.OwnerDraw = true;
            this.tileOLV.ShowCommandMenuOnRightClick = true;
            this.tileOLV.ShowGroups = false;
            this.tileOLV.Size = new System.Drawing.Size(1184, 224);
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
            this.tileOLV.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(this.FormatVNCell);
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
            // TabsControl
            // 
            this.TabsControl.Controls.Add(this.vnTab);
            this.TabsControl.Controls.Add(this.infoTab);
            this.TabsControl.Controls.Add(this.tabPage1);
            this.TabsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabsControl.Location = new System.Drawing.Point(0, 0);
            this.TabsControl.Name = "TabsControl";
            this.TabsControl.Padding = new System.Drawing.Point(0, 0);
            this.TabsControl.SelectedIndex = 0;
            this.TabsControl.Size = new System.Drawing.Size(1204, 621);
            this.TabsControl.TabIndex = 0;
            this.TabsControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CloseTabMiddleClick);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tagsOrTraits);
            this.tabPage1.Controls.Add(this.tagSearchResultBox);
            this.tabPage1.Controls.Add(this.panel4);
            this.tabPage1.Controls.Add(this.filterDropdown);
            this.tabPage1.Controls.Add(this.label19);
            this.tabPage1.Controls.Add(this.traitsPanel);
            this.tabPage1.Controls.Add(this.tagsPanel);
            this.tabPage1.Controls.Add(this.panel13);
            this.tabPage1.Controls.Add(this.userlistPanel);
            this.tabPage1.Controls.Add(this.wishlistPanel);
            this.tabPage1.Controls.Add(this.favoriteProducersPanel);
            this.tabPage1.Controls.Add(this.votedPanel);
            this.tabPage1.Controls.Add(this.blacklistedPanel);
            this.tabPage1.Controls.Add(this.unreleasedPanel);
            this.tabPage1.Controls.Add(this.releaseDatePanel);
            this.tabPage1.Controls.Add(this.lengthPanel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1196, 595);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "Filters";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Leave += new System.EventHandler(this.OnFiltersLeave);
            // 
            // tagsOrTraits
            // 
            this.tagsOrTraits.Appearance = System.Windows.Forms.Appearance.Button;
            this.tagsOrTraits.BackColor = System.Drawing.Color.PaleTurquoise;
            this.tagsOrTraits.FlatAppearance.BorderSize = 0;
            this.tagsOrTraits.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.tagsOrTraits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tagsOrTraits.ForeColor = System.Drawing.Color.Black;
            this.tagsOrTraits.Location = new System.Drawing.Point(691, 350);
            this.tagsOrTraits.Name = "tagsOrTraits";
            this.tagsOrTraits.Size = new System.Drawing.Size(102, 23);
            this.tagsOrTraits.TabIndex = 31;
            this.tagsOrTraits.Text = "Tags AND Traits";
            this.tagsOrTraits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tagsOrTraits.UseVisualStyleBackColor = false;
            this.tagsOrTraits.CheckedChanged += new System.EventHandler(this.tagsOrTraits_CheckedChanged);
            // 
            // tagSearchResultBox
            // 
            this.tagSearchResultBox.FormattingEnabled = true;
            this.tagSearchResultBox.Location = new System.Drawing.Point(853, 255);
            this.tagSearchResultBox.Name = "tagSearchResultBox";
            this.tagSearchResultBox.Size = new System.Drawing.Size(202, 238);
            this.tagSearchResultBox.TabIndex = 97;
            this.tagSearchResultBox.Visible = false;
            this.tagSearchResultBox.SelectedIndexChanged += new System.EventHandler(this.AddTagFromList);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label24);
            this.panel4.Controls.Add(this.languageTFCB);
            this.panel4.Controls.Add(this.languageFixed);
            this.panel4.Controls.Add(this.languageTF);
            this.panel4.Controls.Add(this.languageTFLB);
            this.panel4.Controls.Add(this.label21);
            this.panel4.Location = new System.Drawing.Point(6, 347);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(128, 212);
            this.panel4.TabIndex = 40;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(3, 164);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(122, 39);
            this.label24.TabIndex = 103;
            this.label24.Text = "Titles that have releases\r\nfor one of the entered\r\n languages.";
            // 
            // languageTFCB
            // 
            this.languageTFCB.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.languageTFCB.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.languageTFCB.BackColor = System.Drawing.Color.White;
            this.languageTFCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.languageTFCB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.languageTFCB.FormattingEnabled = true;
            this.languageTFCB.Location = new System.Drawing.Point(3, 140);
            this.languageTFCB.Name = "languageTFCB";
            this.languageTFCB.Size = new System.Drawing.Size(120, 21);
            this.languageTFCB.TabIndex = 102;
            this.languageTFCB.SelectedIndexChanged += new System.EventHandler(this.AddLanguage);
            this.languageTFCB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddLanguageEnter);
            // 
            // languageFixed
            // 
            this.languageFixed.Appearance = System.Windows.Forms.Appearance.Button;
            this.languageFixed.AutoSize = true;
            this.languageFixed.BackColor = System.Drawing.Color.DarkRed;
            this.languageFixed.FlatAppearance.BorderSize = 0;
            this.languageFixed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.languageFixed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.languageFixed.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.languageFixed.Location = new System.Drawing.Point(40, 6);
            this.languageFixed.Name = "languageFixed";
            this.languageFixed.Size = new System.Drawing.Size(62, 23);
            this.languageFixed.TabIndex = 39;
            this.languageFixed.Text = "Not Fixed";
            this.languageFixed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.languageFixed.UseVisualStyleBackColor = false;
            // 
            // languageTF
            // 
            this.languageTF.Appearance = System.Windows.Forms.Appearance.Button;
            this.languageTF.AutoSize = true;
            this.languageTF.BackColor = System.Drawing.Color.DarkRed;
            this.languageTF.FlatAppearance.BorderSize = 0;
            this.languageTF.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.languageTF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.languageTF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.languageTF.Location = new System.Drawing.Point(3, 6);
            this.languageTF.Name = "languageTF";
            this.languageTF.Size = new System.Drawing.Size(31, 23);
            this.languageTF.TabIndex = 21;
            this.languageTF.Text = "Off";
            this.languageTF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.languageTF.UseVisualStyleBackColor = false;
            this.languageTF.CheckedChanged += new System.EventHandler(this.ToggleThisFilter);
            // 
            // languageTFLB
            // 
            this.languageTFLB.FormattingEnabled = true;
            this.languageTFLB.Location = new System.Drawing.Point(3, 52);
            this.languageTFLB.Name = "languageTFLB";
            this.languageTFLB.Size = new System.Drawing.Size(120, 82);
            this.languageTFLB.TabIndex = 1;
            this.languageTFLB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RemoveFromListBox);
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(3, 32);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(100, 23);
            this.label21.TabIndex = 0;
            this.label21.Text = "Language";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // filterDropdown
            // 
            this.filterDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterDropdown.FormattingEnabled = true;
            this.filterDropdown.Items.AddRange(new object[] {
            "No Filters",
            "Never Played",
            "Only URT",
            "Hide URT",
            "By Favorite Producers"});
            this.filterDropdown.Location = new System.Drawing.Point(552, 371);
            this.filterDropdown.Name = "filterDropdown";
            this.filterDropdown.Size = new System.Drawing.Size(121, 21);
            this.filterDropdown.TabIndex = 27;
            this.filterDropdown.SelectedIndexChanged += new System.EventHandler(this.FilterChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(568, 351);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(34, 13);
            this.label19.TabIndex = 26;
            this.label19.Text = "Filters";
            // 
            // traitsPanel
            // 
            this.traitsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.traitsPanel.Controls.Add(this.traitsFixed);
            this.traitsPanel.Controls.Add(this.traitsTF);
            this.traitsPanel.Controls.Add(this.button7);
            this.traitsPanel.Controls.Add(this.traitsTFLB);
            this.traitsPanel.Controls.Add(this.label18);
            this.traitsPanel.Location = new System.Drawing.Point(799, 307);
            this.traitsPanel.Name = "traitsPanel";
            this.traitsPanel.Size = new System.Drawing.Size(389, 282);
            this.traitsPanel.TabIndex = 24;
            // 
            // traitsFixed
            // 
            this.traitsFixed.Appearance = System.Windows.Forms.Appearance.Button;
            this.traitsFixed.AutoSize = true;
            this.traitsFixed.BackColor = System.Drawing.Color.DarkRed;
            this.traitsFixed.FlatAppearance.BorderSize = 0;
            this.traitsFixed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.traitsFixed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.traitsFixed.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.traitsFixed.Location = new System.Drawing.Point(40, 6);
            this.traitsFixed.Name = "traitsFixed";
            this.traitsFixed.Size = new System.Drawing.Size(62, 23);
            this.traitsFixed.TabIndex = 38;
            this.traitsFixed.Text = "Not Fixed";
            this.traitsFixed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.traitsFixed.UseVisualStyleBackColor = false;
            // 
            // traitsTF
            // 
            this.traitsTF.Appearance = System.Windows.Forms.Appearance.Button;
            this.traitsTF.AutoSize = true;
            this.traitsTF.BackColor = System.Drawing.Color.DarkRed;
            this.traitsTF.FlatAppearance.BorderSize = 0;
            this.traitsTF.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.traitsTF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.traitsTF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.traitsTF.Location = new System.Drawing.Point(3, 6);
            this.traitsTF.Name = "traitsTF";
            this.traitsTF.Size = new System.Drawing.Size(31, 23);
            this.traitsTF.TabIndex = 21;
            this.traitsTF.Text = "Off";
            this.traitsTF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.traitsTF.UseVisualStyleBackColor = false;
            this.traitsTF.CheckedChanged += new System.EventHandler(this.ToggleThisFilter);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(3, 254);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(243, 23);
            this.button7.TabIndex = 21;
            this.button7.Text = "Change";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // traitsTFLB
            // 
            this.traitsTFLB.FormattingEnabled = true;
            this.traitsTFLB.Location = new System.Drawing.Point(3, 39);
            this.traitsTFLB.Name = "traitsTFLB";
            this.traitsTFLB.Size = new System.Drawing.Size(243, 212);
            this.traitsTFLB.TabIndex = 1;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(111, 6);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(135, 23);
            this.label18.TabIndex = 0;
            this.label18.Text = "Traits";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tagsPanel
            // 
            this.tagsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tagsPanel.Controls.Add(this.label2);
            this.tagsPanel.Controls.Add(this.TagFilteringHelpButton);
            this.tagsPanel.Controls.Add(this.updateFilterResultsButton);
            this.tagsPanel.Controls.Add(this.tagReply);
            this.tagsPanel.Controls.Add(this.customTagFilterNameBox);
            this.tagsPanel.Controls.Add(this.saveCustomFilterButton);
            this.tagsPanel.Controls.Add(this.customTagFilters);
            this.tagsPanel.Controls.Add(this.clearFilterButton);
            this.tagsPanel.Controls.Add(this.deleteCustomTagFilterButton);
            this.tagsPanel.Controls.Add(this.tagSearchButton);
            this.tagsPanel.Controls.Add(this.tagSearchBox);
            this.tagsPanel.Controls.Add(this.label7);
            this.tagsPanel.Controls.Add(this.tagsFixed);
            this.tagsPanel.Controls.Add(this.tagsTF);
            this.tagsPanel.Controls.Add(this.button6);
            this.tagsPanel.Controls.Add(this.tagsTFLB);
            this.tagsPanel.Controls.Add(this.label30);
            this.tagsPanel.Location = new System.Drawing.Point(799, 14);
            this.tagsPanel.Name = "tagsPanel";
            this.tagsPanel.Size = new System.Drawing.Size(389, 277);
            this.tagsPanel.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(281, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 107;
            this.label2.Text = "Custom Filter Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tagReply
            // 
            this.tagReply.Location = new System.Drawing.Point(284, 195);
            this.tagReply.Name = "tagReply";
            this.tagReply.Size = new System.Drawing.Size(100, 48);
            this.tagReply.TabIndex = 102;
            this.tagReply.Text = "(tagReply)";
            this.tagReply.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // saveCustomFilterButton
            // 
            this.saveCustomFilterButton.BackColor = System.Drawing.Color.SteelBlue;
            this.saveCustomFilterButton.FlatAppearance.BorderSize = 0;
            this.saveCustomFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveCustomFilterButton.ForeColor = System.Drawing.Color.White;
            this.saveCustomFilterButton.Location = new System.Drawing.Point(284, 82);
            this.saveCustomFilterButton.Name = "saveCustomFilterButton";
            this.saveCustomFilterButton.Size = new System.Drawing.Size(100, 23);
            this.saveCustomFilterButton.TabIndex = 99;
            this.saveCustomFilterButton.Text = "Save Filter";
            this.saveCustomFilterButton.UseVisualStyleBackColor = false;
            this.saveCustomFilterButton.Click += new System.EventHandler(this.SaveCustomTagFilter);
            // 
            // customTagFilters
            // 
            this.customTagFilters.BackColor = System.Drawing.Color.SteelBlue;
            this.customTagFilters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.customTagFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customTagFilters.ForeColor = System.Drawing.Color.White;
            this.customTagFilters.FormattingEnabled = true;
            this.customTagFilters.Items.AddRange(new object[] {
            "Custom Filters",
            "----------"});
            this.customTagFilters.Location = new System.Drawing.Point(284, 55);
            this.customTagFilters.Name = "customTagFilters";
            this.customTagFilters.Size = new System.Drawing.Size(100, 21);
            this.customTagFilters.TabIndex = 104;
            this.customTagFilters.SelectedIndexChanged += new System.EventHandler(this.Filter_CustomTags);
            // 
            // clearFilterButton
            // 
            this.clearFilterButton.BackColor = System.Drawing.Color.SteelBlue;
            this.clearFilterButton.FlatAppearance.BorderSize = 0;
            this.clearFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearFilterButton.ForeColor = System.Drawing.Color.White;
            this.clearFilterButton.Location = new System.Drawing.Point(284, 140);
            this.clearFilterButton.Name = "clearFilterButton";
            this.clearFilterButton.Size = new System.Drawing.Size(100, 23);
            this.clearFilterButton.TabIndex = 100;
            this.clearFilterButton.Text = "Clear Filter";
            this.clearFilterButton.UseVisualStyleBackColor = false;
            this.clearFilterButton.Click += new System.EventHandler(this.ClearTagFilter);
            // 
            // deleteCustomTagFilterButton
            // 
            this.deleteCustomTagFilterButton.BackColor = System.Drawing.Color.SteelBlue;
            this.deleteCustomTagFilterButton.Enabled = false;
            this.deleteCustomTagFilterButton.FlatAppearance.BorderSize = 0;
            this.deleteCustomTagFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteCustomTagFilterButton.ForeColor = System.Drawing.Color.White;
            this.deleteCustomTagFilterButton.Location = new System.Drawing.Point(284, 111);
            this.deleteCustomTagFilterButton.Name = "deleteCustomTagFilterButton";
            this.deleteCustomTagFilterButton.Size = new System.Drawing.Size(100, 23);
            this.deleteCustomTagFilterButton.TabIndex = 105;
            this.deleteCustomTagFilterButton.Text = "Delete Filter";
            this.deleteCustomTagFilterButton.UseVisualStyleBackColor = false;
            this.deleteCustomTagFilterButton.Click += new System.EventHandler(this.DeleteCustomTagFilter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 220);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 96;
            this.label7.Text = "Search:";
            // 
            // tagsFixed
            // 
            this.tagsFixed.Appearance = System.Windows.Forms.Appearance.Button;
            this.tagsFixed.AutoSize = true;
            this.tagsFixed.BackColor = System.Drawing.Color.DarkRed;
            this.tagsFixed.FlatAppearance.BorderSize = 0;
            this.tagsFixed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.tagsFixed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tagsFixed.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.tagsFixed.Location = new System.Drawing.Point(40, 6);
            this.tagsFixed.Name = "tagsFixed";
            this.tagsFixed.Size = new System.Drawing.Size(62, 23);
            this.tagsFixed.TabIndex = 39;
            this.tagsFixed.Text = "Not Fixed";
            this.tagsFixed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tagsFixed.UseVisualStyleBackColor = false;
            // 
            // tagsTF
            // 
            this.tagsTF.Appearance = System.Windows.Forms.Appearance.Button;
            this.tagsTF.AutoSize = true;
            this.tagsTF.BackColor = System.Drawing.Color.DarkRed;
            this.tagsTF.FlatAppearance.BorderSize = 0;
            this.tagsTF.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.tagsTF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tagsTF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.tagsTF.Location = new System.Drawing.Point(3, 6);
            this.tagsTF.Name = "tagsTF";
            this.tagsTF.Size = new System.Drawing.Size(31, 23);
            this.tagsTF.TabIndex = 21;
            this.tagsTF.Text = "Off";
            this.tagsTF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tagsTF.UseVisualStyleBackColor = false;
            this.tagsTF.CheckedChanged += new System.EventHandler(this.ToggleThisFilter);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(6, 246);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(272, 23);
            this.button6.TabIndex = 21;
            this.button6.Text = "Change";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // tagsTFLB
            // 
            this.tagsTFLB.FormattingEnabled = true;
            this.tagsTFLB.Location = new System.Drawing.Point(3, 39);
            this.tagsTFLB.Name = "tagsTFLB";
            this.tagsTFLB.Size = new System.Drawing.Size(275, 173);
            this.tagsTFLB.TabIndex = 1;
            // 
            // label30
            // 
            this.label30.Location = new System.Drawing.Point(108, 6);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(138, 23);
            this.label30.TabIndex = 0;
            this.label30.Text = "Tags";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
            this.panel13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel13.Controls.Add(this.label6);
            this.panel13.Controls.Add(this.originalLanguageTFCB);
            this.panel13.Controls.Add(this.originalLanguageFixed);
            this.panel13.Controls.Add(this.originalLanguageTF);
            this.panel13.Controls.Add(this.originalLanguageTFLB);
            this.panel13.Controls.Add(this.label20);
            this.panel13.Location = new System.Drawing.Point(140, 347);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(128, 212);
            this.panel13.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 39);
            this.label6.TabIndex = 102;
            this.label6.Text = "Titles that are originally\r\nin one of the entered\r\n languages.";
            // 
            // originalLanguageTFCB
            // 
            this.originalLanguageTFCB.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.originalLanguageTFCB.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.originalLanguageTFCB.BackColor = System.Drawing.Color.White;
            this.originalLanguageTFCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.originalLanguageTFCB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.originalLanguageTFCB.FormattingEnabled = true;
            this.originalLanguageTFCB.Location = new System.Drawing.Point(3, 140);
            this.originalLanguageTFCB.Name = "originalLanguageTFCB";
            this.originalLanguageTFCB.Size = new System.Drawing.Size(119, 21);
            this.originalLanguageTFCB.TabIndex = 101;
            this.originalLanguageTFCB.SelectedIndexChanged += new System.EventHandler(this.AddOriginalLanguage);
            this.originalLanguageTFCB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddOriginalLanguageEnter);
            // 
            // originalLanguageFixed
            // 
            this.originalLanguageFixed.Appearance = System.Windows.Forms.Appearance.Button;
            this.originalLanguageFixed.AutoSize = true;
            this.originalLanguageFixed.BackColor = System.Drawing.Color.DarkRed;
            this.originalLanguageFixed.FlatAppearance.BorderSize = 0;
            this.originalLanguageFixed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.originalLanguageFixed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.originalLanguageFixed.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.originalLanguageFixed.Location = new System.Drawing.Point(40, 6);
            this.originalLanguageFixed.Name = "originalLanguageFixed";
            this.originalLanguageFixed.Size = new System.Drawing.Size(62, 23);
            this.originalLanguageFixed.TabIndex = 39;
            this.originalLanguageFixed.Text = "Not Fixed";
            this.originalLanguageFixed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.originalLanguageFixed.UseVisualStyleBackColor = false;
            // 
            // originalLanguageTF
            // 
            this.originalLanguageTF.Appearance = System.Windows.Forms.Appearance.Button;
            this.originalLanguageTF.AutoSize = true;
            this.originalLanguageTF.BackColor = System.Drawing.Color.DarkRed;
            this.originalLanguageTF.FlatAppearance.BorderSize = 0;
            this.originalLanguageTF.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.originalLanguageTF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.originalLanguageTF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.originalLanguageTF.Location = new System.Drawing.Point(3, 6);
            this.originalLanguageTF.Name = "originalLanguageTF";
            this.originalLanguageTF.Size = new System.Drawing.Size(31, 23);
            this.originalLanguageTF.TabIndex = 21;
            this.originalLanguageTF.Text = "Off";
            this.originalLanguageTF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.originalLanguageTF.UseVisualStyleBackColor = false;
            this.originalLanguageTF.CheckedChanged += new System.EventHandler(this.ToggleThisFilter);
            // 
            // originalLanguageTFLB
            // 
            this.originalLanguageTFLB.FormattingEnabled = true;
            this.originalLanguageTFLB.Location = new System.Drawing.Point(3, 52);
            this.originalLanguageTFLB.Name = "originalLanguageTFLB";
            this.originalLanguageTFLB.Size = new System.Drawing.Size(119, 82);
            this.originalLanguageTFLB.TabIndex = 1;
            this.originalLanguageTFLB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RemoveFromListBox);
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(3, 32);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(100, 23);
            this.label20.TabIndex = 0;
            this.label20.Text = "Original Language";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userlistPanel
            // 
            this.userlistPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userlistPanel.Controls.Add(this.userlistFixed);
            this.userlistPanel.Controls.Add(this.userlistTF);
            this.userlistPanel.Controls.Add(this.userlistTFDropped);
            this.userlistPanel.Controls.Add(this.userlistTFStalled);
            this.userlistPanel.Controls.Add(this.userlistTFFinished);
            this.userlistPanel.Controls.Add(this.userlistTFPlaying);
            this.userlistPanel.Controls.Add(this.userlistTFUnknown);
            this.userlistPanel.Controls.Add(this.userlistTFNa);
            this.userlistPanel.Controls.Add(this.label23);
            this.userlistPanel.Location = new System.Drawing.Point(6, 307);
            this.userlistPanel.Name = "userlistPanel";
            this.userlistPanel.Size = new System.Drawing.Size(787, 37);
            this.userlistPanel.TabIndex = 18;
            // 
            // userlistFixed
            // 
            this.userlistFixed.Appearance = System.Windows.Forms.Appearance.Button;
            this.userlistFixed.AutoSize = true;
            this.userlistFixed.BackColor = System.Drawing.Color.DarkRed;
            this.userlistFixed.FlatAppearance.BorderSize = 0;
            this.userlistFixed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.userlistFixed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.userlistFixed.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.userlistFixed.Location = new System.Drawing.Point(40, 3);
            this.userlistFixed.Name = "userlistFixed";
            this.userlistFixed.Size = new System.Drawing.Size(62, 23);
            this.userlistFixed.TabIndex = 37;
            this.userlistFixed.Text = "Not Fixed";
            this.userlistFixed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.userlistFixed.UseVisualStyleBackColor = false;
            this.userlistFixed.CheckedChanged += new System.EventHandler(this.FilterFixedChanged);
            // 
            // userlistTF
            // 
            this.userlistTF.Appearance = System.Windows.Forms.Appearance.Button;
            this.userlistTF.AutoSize = true;
            this.userlistTF.BackColor = System.Drawing.Color.DarkRed;
            this.userlistTF.FlatAppearance.BorderSize = 0;
            this.userlistTF.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.userlistTF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.userlistTF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.userlistTF.Location = new System.Drawing.Point(3, 3);
            this.userlistTF.Name = "userlistTF";
            this.userlistTF.Size = new System.Drawing.Size(31, 23);
            this.userlistTF.TabIndex = 22;
            this.userlistTF.Text = "Off";
            this.userlistTF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.userlistTF.UseVisualStyleBackColor = false;
            this.userlistTF.CheckedChanged += new System.EventHandler(this.ToggleThisFilter);
            // 
            // userlistTFDropped
            // 
            this.userlistTFDropped.AutoSize = true;
            this.userlistTFDropped.Location = new System.Drawing.Point(674, 7);
            this.userlistTFDropped.Name = "userlistTFDropped";
            this.userlistTFDropped.Size = new System.Drawing.Size(67, 17);
            this.userlistTFDropped.TabIndex = 6;
            this.userlistTFDropped.Text = "Dropped";
            this.userlistTFDropped.UseVisualStyleBackColor = true;
            this.userlistTFDropped.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // userlistTFStalled
            // 
            this.userlistTFStalled.AutoSize = true;
            this.userlistTFStalled.Location = new System.Drawing.Point(582, 7);
            this.userlistTFStalled.Name = "userlistTFStalled";
            this.userlistTFStalled.Size = new System.Drawing.Size(58, 17);
            this.userlistTFStalled.TabIndex = 5;
            this.userlistTFStalled.Text = "Stalled";
            this.userlistTFStalled.UseVisualStyleBackColor = true;
            this.userlistTFStalled.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // userlistTFFinished
            // 
            this.userlistTFFinished.AutoSize = true;
            this.userlistTFFinished.Location = new System.Drawing.Point(490, 7);
            this.userlistTFFinished.Name = "userlistTFFinished";
            this.userlistTFFinished.Size = new System.Drawing.Size(65, 17);
            this.userlistTFFinished.TabIndex = 4;
            this.userlistTFFinished.Text = "Finished";
            this.userlistTFFinished.UseVisualStyleBackColor = true;
            this.userlistTFFinished.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // userlistTFPlaying
            // 
            this.userlistTFPlaying.AutoSize = true;
            this.userlistTFPlaying.Location = new System.Drawing.Point(398, 7);
            this.userlistTFPlaying.Name = "userlistTFPlaying";
            this.userlistTFPlaying.Size = new System.Drawing.Size(60, 17);
            this.userlistTFPlaying.TabIndex = 3;
            this.userlistTFPlaying.Text = "Playing";
            this.userlistTFPlaying.UseVisualStyleBackColor = true;
            this.userlistTFPlaying.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // userlistTFUnknown
            // 
            this.userlistTFUnknown.AutoSize = true;
            this.userlistTFUnknown.Location = new System.Drawing.Point(306, 7);
            this.userlistTFUnknown.Name = "userlistTFUnknown";
            this.userlistTFUnknown.Size = new System.Drawing.Size(72, 17);
            this.userlistTFUnknown.TabIndex = 2;
            this.userlistTFUnknown.Text = "Unknown";
            this.userlistTFUnknown.UseVisualStyleBackColor = true;
            this.userlistTFUnknown.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // userlistTFNa
            // 
            this.userlistTFNa.AutoSize = true;
            this.userlistTFNa.Location = new System.Drawing.Point(214, 7);
            this.userlistTFNa.Name = "userlistTFNa";
            this.userlistTFNa.Size = new System.Drawing.Size(46, 17);
            this.userlistTFNa.TabIndex = 1;
            this.userlistTFNa.Text = "N/A";
            this.userlistTFNa.UseVisualStyleBackColor = true;
            this.userlistTFNa.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(108, 3);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(100, 23);
            this.label23.TabIndex = 0;
            this.label23.Text = "Userlist Status";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // wishlistPanel
            // 
            this.wishlistPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wishlistPanel.Controls.Add(this.wishlistFixed);
            this.wishlistPanel.Controls.Add(this.wishlistTF);
            this.wishlistPanel.Controls.Add(this.wishlistTFLow);
            this.wishlistPanel.Controls.Add(this.wishlistTFMedium);
            this.wishlistPanel.Controls.Add(this.wishlistTFHigh);
            this.wishlistPanel.Controls.Add(this.wishlistTFNa);
            this.wishlistPanel.Controls.Add(this.label26);
            this.wishlistPanel.Location = new System.Drawing.Point(6, 264);
            this.wishlistPanel.Name = "wishlistPanel";
            this.wishlistPanel.Size = new System.Drawing.Size(787, 37);
            this.wishlistPanel.TabIndex = 18;
            // 
            // wishlistFixed
            // 
            this.wishlistFixed.Appearance = System.Windows.Forms.Appearance.Button;
            this.wishlistFixed.AutoSize = true;
            this.wishlistFixed.BackColor = System.Drawing.Color.DarkRed;
            this.wishlistFixed.FlatAppearance.BorderSize = 0;
            this.wishlistFixed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.wishlistFixed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.wishlistFixed.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.wishlistFixed.Location = new System.Drawing.Point(40, 3);
            this.wishlistFixed.Name = "wishlistFixed";
            this.wishlistFixed.Size = new System.Drawing.Size(62, 23);
            this.wishlistFixed.TabIndex = 36;
            this.wishlistFixed.Text = "Not Fixed";
            this.wishlistFixed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.wishlistFixed.UseVisualStyleBackColor = false;
            this.wishlistFixed.CheckedChanged += new System.EventHandler(this.FilterFixedChanged);
            // 
            // wishlistTF
            // 
            this.wishlistTF.Appearance = System.Windows.Forms.Appearance.Button;
            this.wishlistTF.AutoSize = true;
            this.wishlistTF.BackColor = System.Drawing.Color.DarkRed;
            this.wishlistTF.FlatAppearance.BorderSize = 0;
            this.wishlistTF.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.wishlistTF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.wishlistTF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.wishlistTF.Location = new System.Drawing.Point(3, 3);
            this.wishlistTF.Name = "wishlistTF";
            this.wishlistTF.Size = new System.Drawing.Size(31, 23);
            this.wishlistTF.TabIndex = 23;
            this.wishlistTF.Text = "Off";
            this.wishlistTF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.wishlistTF.UseVisualStyleBackColor = false;
            this.wishlistTF.CheckedChanged += new System.EventHandler(this.ToggleThisFilter);
            // 
            // wishlistTFLow
            // 
            this.wishlistTFLow.AutoSize = true;
            this.wishlistTFLow.Location = new System.Drawing.Point(490, 7);
            this.wishlistTFLow.Name = "wishlistTFLow";
            this.wishlistTFLow.Size = new System.Drawing.Size(46, 17);
            this.wishlistTFLow.TabIndex = 4;
            this.wishlistTFLow.Text = "Low";
            this.wishlistTFLow.UseVisualStyleBackColor = true;
            this.wishlistTFLow.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // wishlistTFMedium
            // 
            this.wishlistTFMedium.AutoSize = true;
            this.wishlistTFMedium.Location = new System.Drawing.Point(398, 7);
            this.wishlistTFMedium.Name = "wishlistTFMedium";
            this.wishlistTFMedium.Size = new System.Drawing.Size(63, 17);
            this.wishlistTFMedium.TabIndex = 3;
            this.wishlistTFMedium.Text = "Medium";
            this.wishlistTFMedium.UseVisualStyleBackColor = true;
            this.wishlistTFMedium.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // wishlistTFHigh
            // 
            this.wishlistTFHigh.AutoSize = true;
            this.wishlistTFHigh.Location = new System.Drawing.Point(306, 7);
            this.wishlistTFHigh.Name = "wishlistTFHigh";
            this.wishlistTFHigh.Size = new System.Drawing.Size(48, 17);
            this.wishlistTFHigh.TabIndex = 2;
            this.wishlistTFHigh.Text = "High";
            this.wishlistTFHigh.UseVisualStyleBackColor = true;
            this.wishlistTFHigh.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // wishlistTFNa
            // 
            this.wishlistTFNa.AutoSize = true;
            this.wishlistTFNa.Location = new System.Drawing.Point(214, 7);
            this.wishlistTFNa.Name = "wishlistTFNa";
            this.wishlistTFNa.Size = new System.Drawing.Size(46, 17);
            this.wishlistTFNa.TabIndex = 1;
            this.wishlistTFNa.Text = "N/A";
            this.wishlistTFNa.UseVisualStyleBackColor = true;
            this.wishlistTFNa.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // label26
            // 
            this.label26.Location = new System.Drawing.Point(108, 3);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(100, 23);
            this.label26.TabIndex = 0;
            this.label26.Text = "Wishlist Status";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // favoriteProducersPanel
            // 
            this.favoriteProducersPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.favoriteProducersPanel.Controls.Add(this.favoriteProducersFixed);
            this.favoriteProducersPanel.Controls.Add(this.favoriteProducersTF);
            this.favoriteProducersPanel.Controls.Add(this.favoriteProducersTFNo);
            this.favoriteProducersPanel.Controls.Add(this.favoriteProducersTFYes);
            this.favoriteProducersPanel.Controls.Add(this.label25);
            this.favoriteProducersPanel.Location = new System.Drawing.Point(6, 221);
            this.favoriteProducersPanel.Name = "favoriteProducersPanel";
            this.favoriteProducersPanel.Size = new System.Drawing.Size(787, 37);
            this.favoriteProducersPanel.TabIndex = 20;
            // 
            // favoriteProducersFixed
            // 
            this.favoriteProducersFixed.Appearance = System.Windows.Forms.Appearance.Button;
            this.favoriteProducersFixed.AutoSize = true;
            this.favoriteProducersFixed.BackColor = System.Drawing.Color.DarkRed;
            this.favoriteProducersFixed.FlatAppearance.BorderSize = 0;
            this.favoriteProducersFixed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.favoriteProducersFixed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.favoriteProducersFixed.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.favoriteProducersFixed.Location = new System.Drawing.Point(40, 3);
            this.favoriteProducersFixed.Name = "favoriteProducersFixed";
            this.favoriteProducersFixed.Size = new System.Drawing.Size(62, 23);
            this.favoriteProducersFixed.TabIndex = 35;
            this.favoriteProducersFixed.Text = "Not Fixed";
            this.favoriteProducersFixed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.favoriteProducersFixed.UseVisualStyleBackColor = false;
            this.favoriteProducersFixed.CheckedChanged += new System.EventHandler(this.FilterFixedChanged);
            // 
            // favoriteProducersTF
            // 
            this.favoriteProducersTF.Appearance = System.Windows.Forms.Appearance.Button;
            this.favoriteProducersTF.AutoSize = true;
            this.favoriteProducersTF.BackColor = System.Drawing.Color.DarkRed;
            this.favoriteProducersTF.FlatAppearance.BorderSize = 0;
            this.favoriteProducersTF.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.favoriteProducersTF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.favoriteProducersTF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.favoriteProducersTF.Location = new System.Drawing.Point(3, 3);
            this.favoriteProducersTF.Name = "favoriteProducersTF";
            this.favoriteProducersTF.Size = new System.Drawing.Size(31, 23);
            this.favoriteProducersTF.TabIndex = 24;
            this.favoriteProducersTF.Text = "Off";
            this.favoriteProducersTF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.favoriteProducersTF.UseVisualStyleBackColor = false;
            this.favoriteProducersTF.CheckedChanged += new System.EventHandler(this.ToggleThisFilter);
            // 
            // favoriteProducersTFNo
            // 
            this.favoriteProducersTFNo.AutoSize = true;
            this.favoriteProducersTFNo.Location = new System.Drawing.Point(305, 6);
            this.favoriteProducersTFNo.Name = "favoriteProducersTFNo";
            this.favoriteProducersTFNo.Size = new System.Drawing.Size(39, 17);
            this.favoriteProducersTFNo.TabIndex = 2;
            this.favoriteProducersTFNo.TabStop = true;
            this.favoriteProducersTFNo.Tag = "";
            this.favoriteProducersTFNo.Text = "No";
            this.favoriteProducersTFNo.UseVisualStyleBackColor = true;
            // 
            // favoriteProducersTFYes
            // 
            this.favoriteProducersTFYes.AutoSize = true;
            this.favoriteProducersTFYes.Location = new System.Drawing.Point(214, 6);
            this.favoriteProducersTFYes.Name = "favoriteProducersTFYes";
            this.favoriteProducersTFYes.Size = new System.Drawing.Size(43, 17);
            this.favoriteProducersTFYes.TabIndex = 1;
            this.favoriteProducersTFYes.TabStop = true;
            this.favoriteProducersTFYes.Tag = "";
            this.favoriteProducersTFYes.Text = "Yes";
            this.favoriteProducersTFYes.UseVisualStyleBackColor = true;
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(108, -3);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(100, 35);
            this.label25.TabIndex = 0;
            this.label25.Text = "By Favorite Producer";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // votedPanel
            // 
            this.votedPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.votedPanel.Controls.Add(this.votedFixed);
            this.votedPanel.Controls.Add(this.votedTF);
            this.votedPanel.Controls.Add(this.votedTFNo);
            this.votedPanel.Controls.Add(this.votedTFYes);
            this.votedPanel.Controls.Add(this.label17);
            this.votedPanel.Location = new System.Drawing.Point(6, 178);
            this.votedPanel.Name = "votedPanel";
            this.votedPanel.Size = new System.Drawing.Size(787, 37);
            this.votedPanel.TabIndex = 19;
            // 
            // votedFixed
            // 
            this.votedFixed.Appearance = System.Windows.Forms.Appearance.Button;
            this.votedFixed.AutoSize = true;
            this.votedFixed.BackColor = System.Drawing.Color.DarkRed;
            this.votedFixed.FlatAppearance.BorderSize = 0;
            this.votedFixed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.votedFixed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.votedFixed.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.votedFixed.Location = new System.Drawing.Point(40, 3);
            this.votedFixed.Name = "votedFixed";
            this.votedFixed.Size = new System.Drawing.Size(62, 23);
            this.votedFixed.TabIndex = 34;
            this.votedFixed.Text = "Not Fixed";
            this.votedFixed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.votedFixed.UseVisualStyleBackColor = false;
            this.votedFixed.CheckedChanged += new System.EventHandler(this.FilterFixedChanged);
            // 
            // votedTF
            // 
            this.votedTF.Appearance = System.Windows.Forms.Appearance.Button;
            this.votedTF.AutoSize = true;
            this.votedTF.BackColor = System.Drawing.Color.DarkRed;
            this.votedTF.FlatAppearance.BorderSize = 0;
            this.votedTF.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.votedTF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.votedTF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.votedTF.Location = new System.Drawing.Point(3, 3);
            this.votedTF.Name = "votedTF";
            this.votedTF.Size = new System.Drawing.Size(31, 23);
            this.votedTF.TabIndex = 25;
            this.votedTF.Text = "Off";
            this.votedTF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.votedTF.UseVisualStyleBackColor = false;
            this.votedTF.CheckedChanged += new System.EventHandler(this.ToggleThisFilter);
            // 
            // votedTFNo
            // 
            this.votedTFNo.AutoSize = true;
            this.votedTFNo.Location = new System.Drawing.Point(305, 6);
            this.votedTFNo.Name = "votedTFNo";
            this.votedTFNo.Size = new System.Drawing.Size(39, 17);
            this.votedTFNo.TabIndex = 2;
            this.votedTFNo.TabStop = true;
            this.votedTFNo.Tag = "";
            this.votedTFNo.Text = "No";
            this.votedTFNo.UseVisualStyleBackColor = true;
            // 
            // votedTFYes
            // 
            this.votedTFYes.AutoSize = true;
            this.votedTFYes.Location = new System.Drawing.Point(214, 6);
            this.votedTFYes.Name = "votedTFYes";
            this.votedTFYes.Size = new System.Drawing.Size(43, 17);
            this.votedTFYes.TabIndex = 1;
            this.votedTFYes.TabStop = true;
            this.votedTFYes.Tag = "";
            this.votedTFYes.Text = "Yes";
            this.votedTFYes.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(108, 3);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(100, 23);
            this.label17.TabIndex = 0;
            this.label17.Text = "Voted";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // blacklistedPanel
            // 
            this.blacklistedPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.blacklistedPanel.Controls.Add(this.blacklistedFixed);
            this.blacklistedPanel.Controls.Add(this.blacklistedTF);
            this.blacklistedPanel.Controls.Add(this.blacklistedTFNo);
            this.blacklistedPanel.Controls.Add(this.blacklistedTFYes);
            this.blacklistedPanel.Controls.Add(this.label16);
            this.blacklistedPanel.Location = new System.Drawing.Point(6, 135);
            this.blacklistedPanel.Name = "blacklistedPanel";
            this.blacklistedPanel.Size = new System.Drawing.Size(787, 37);
            this.blacklistedPanel.TabIndex = 19;
            // 
            // blacklistedFixed
            // 
            this.blacklistedFixed.Appearance = System.Windows.Forms.Appearance.Button;
            this.blacklistedFixed.AutoSize = true;
            this.blacklistedFixed.BackColor = System.Drawing.Color.DarkRed;
            this.blacklistedFixed.FlatAppearance.BorderSize = 0;
            this.blacklistedFixed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.blacklistedFixed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.blacklistedFixed.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.blacklistedFixed.Location = new System.Drawing.Point(40, 3);
            this.blacklistedFixed.Name = "blacklistedFixed";
            this.blacklistedFixed.Size = new System.Drawing.Size(62, 23);
            this.blacklistedFixed.TabIndex = 33;
            this.blacklistedFixed.Text = "Not Fixed";
            this.blacklistedFixed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.blacklistedFixed.UseVisualStyleBackColor = false;
            this.blacklistedFixed.CheckedChanged += new System.EventHandler(this.FilterFixedChanged);
            // 
            // blacklistedTF
            // 
            this.blacklistedTF.Appearance = System.Windows.Forms.Appearance.Button;
            this.blacklistedTF.AutoSize = true;
            this.blacklistedTF.BackColor = System.Drawing.Color.DarkRed;
            this.blacklistedTF.FlatAppearance.BorderSize = 0;
            this.blacklistedTF.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.blacklistedTF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.blacklistedTF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.blacklistedTF.Location = new System.Drawing.Point(3, 3);
            this.blacklistedTF.Name = "blacklistedTF";
            this.blacklistedTF.Size = new System.Drawing.Size(31, 23);
            this.blacklistedTF.TabIndex = 26;
            this.blacklistedTF.Text = "Off";
            this.blacklistedTF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.blacklistedTF.UseVisualStyleBackColor = false;
            this.blacklistedTF.CheckedChanged += new System.EventHandler(this.ToggleThisFilter);
            // 
            // blacklistedTFNo
            // 
            this.blacklistedTFNo.AutoSize = true;
            this.blacklistedTFNo.Location = new System.Drawing.Point(305, 6);
            this.blacklistedTFNo.Name = "blacklistedTFNo";
            this.blacklistedTFNo.Size = new System.Drawing.Size(39, 17);
            this.blacklistedTFNo.TabIndex = 2;
            this.blacklistedTFNo.TabStop = true;
            this.blacklistedTFNo.Tag = "";
            this.blacklistedTFNo.Text = "No";
            this.blacklistedTFNo.UseVisualStyleBackColor = true;
            // 
            // blacklistedTFYes
            // 
            this.blacklistedTFYes.AutoSize = true;
            this.blacklistedTFYes.Location = new System.Drawing.Point(214, 6);
            this.blacklistedTFYes.Name = "blacklistedTFYes";
            this.blacklistedTFYes.Size = new System.Drawing.Size(43, 17);
            this.blacklistedTFYes.TabIndex = 1;
            this.blacklistedTFYes.TabStop = true;
            this.blacklistedTFYes.Tag = "";
            this.blacklistedTFYes.Text = "Yes";
            this.blacklistedTFYes.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(108, 3);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(100, 23);
            this.label16.TabIndex = 0;
            this.label16.Text = "Blacklisted";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // unreleasedPanel
            // 
            this.unreleasedPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.unreleasedPanel.Controls.Add(this.unreleasedFixed);
            this.unreleasedPanel.Controls.Add(this.unreleasedTFReleased);
            this.unreleasedPanel.Controls.Add(this.unreleasedTF);
            this.unreleasedPanel.Controls.Add(this.unreleasedTFWithoutReleaseDate);
            this.unreleasedPanel.Controls.Add(this.label15);
            this.unreleasedPanel.Controls.Add(this.unreleasedTFWithReleaseDate);
            this.unreleasedPanel.Location = new System.Drawing.Point(6, 92);
            this.unreleasedPanel.Name = "unreleasedPanel";
            this.unreleasedPanel.Size = new System.Drawing.Size(787, 37);
            this.unreleasedPanel.TabIndex = 18;
            // 
            // unreleasedFixed
            // 
            this.unreleasedFixed.Appearance = System.Windows.Forms.Appearance.Button;
            this.unreleasedFixed.AutoSize = true;
            this.unreleasedFixed.BackColor = System.Drawing.Color.DarkRed;
            this.unreleasedFixed.FlatAppearance.BorderSize = 0;
            this.unreleasedFixed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.unreleasedFixed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.unreleasedFixed.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.unreleasedFixed.Location = new System.Drawing.Point(40, 3);
            this.unreleasedFixed.Name = "unreleasedFixed";
            this.unreleasedFixed.Size = new System.Drawing.Size(62, 23);
            this.unreleasedFixed.TabIndex = 32;
            this.unreleasedFixed.Text = "Not Fixed";
            this.unreleasedFixed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.unreleasedFixed.UseVisualStyleBackColor = false;
            this.unreleasedFixed.CheckedChanged += new System.EventHandler(this.FilterFixedChanged);
            // 
            // unreleasedTFReleased
            // 
            this.unreleasedTFReleased.AutoSize = true;
            this.unreleasedTFReleased.Location = new System.Drawing.Point(511, 7);
            this.unreleasedTFReleased.Name = "unreleasedTFReleased";
            this.unreleasedTFReleased.Size = new System.Drawing.Size(71, 17);
            this.unreleasedTFReleased.TabIndex = 39;
            this.unreleasedTFReleased.Text = "Released";
            this.unreleasedTFReleased.UseVisualStyleBackColor = true;
            // 
            // unreleasedTF
            // 
            this.unreleasedTF.Appearance = System.Windows.Forms.Appearance.Button;
            this.unreleasedTF.AutoSize = true;
            this.unreleasedTF.BackColor = System.Drawing.Color.DarkRed;
            this.unreleasedTF.FlatAppearance.BorderSize = 0;
            this.unreleasedTF.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.unreleasedTF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.unreleasedTF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.unreleasedTF.Location = new System.Drawing.Point(3, 3);
            this.unreleasedTF.Name = "unreleasedTF";
            this.unreleasedTF.Size = new System.Drawing.Size(31, 23);
            this.unreleasedTF.TabIndex = 27;
            this.unreleasedTF.Text = "Off";
            this.unreleasedTF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.unreleasedTF.UseVisualStyleBackColor = false;
            this.unreleasedTF.CheckedChanged += new System.EventHandler(this.ToggleThisFilter);
            // 
            // unreleasedTFWithoutReleaseDate
            // 
            this.unreleasedTFWithoutReleaseDate.AutoSize = true;
            this.unreleasedTFWithoutReleaseDate.Location = new System.Drawing.Point(209, 7);
            this.unreleasedTFWithoutReleaseDate.Name = "unreleasedTFWithoutReleaseDate";
            this.unreleasedTFWithoutReleaseDate.Size = new System.Drawing.Size(131, 17);
            this.unreleasedTFWithoutReleaseDate.TabIndex = 38;
            this.unreleasedTFWithoutReleaseDate.Text = "Without Release Date";
            this.unreleasedTFWithoutReleaseDate.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(108, 3);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 23);
            this.label15.TabIndex = 0;
            this.label15.Text = "Unreleased";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // unreleasedTFWithReleaseDate
            // 
            this.unreleasedTFWithReleaseDate.AutoSize = true;
            this.unreleasedTFWithReleaseDate.Location = new System.Drawing.Point(367, 7);
            this.unreleasedTFWithReleaseDate.Name = "unreleasedTFWithReleaseDate";
            this.unreleasedTFWithReleaseDate.Size = new System.Drawing.Size(116, 17);
            this.unreleasedTFWithReleaseDate.TabIndex = 37;
            this.unreleasedTFWithReleaseDate.Text = "With Release Date";
            this.unreleasedTFWithReleaseDate.UseVisualStyleBackColor = true;
            // 
            // releaseDatePanel
            // 
            this.releaseDatePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.releaseDatePanel.Controls.Add(this.releaseDateTFResponse);
            this.releaseDatePanel.Controls.Add(this.releaseDateFixed);
            this.releaseDatePanel.Controls.Add(this.releaseDateTF);
            this.releaseDatePanel.Controls.Add(this.releaseDateTFToYear);
            this.releaseDatePanel.Controls.Add(this.label27);
            this.releaseDatePanel.Controls.Add(this.releaseDateTFToMonth);
            this.releaseDatePanel.Controls.Add(this.label28);
            this.releaseDatePanel.Controls.Add(this.label29);
            this.releaseDatePanel.Controls.Add(this.releaseDateTFToDay);
            this.releaseDatePanel.Controls.Add(this.label22);
            this.releaseDatePanel.Controls.Add(this.releaseDateTFFromYear);
            this.releaseDatePanel.Controls.Add(this.label34);
            this.releaseDatePanel.Controls.Add(this.releaseDateTFFromMonth);
            this.releaseDatePanel.Controls.Add(this.label33);
            this.releaseDatePanel.Controls.Add(this.label32);
            this.releaseDatePanel.Controls.Add(this.releaseDateTFFromDay);
            this.releaseDatePanel.Controls.Add(this.label31);
            this.releaseDatePanel.Location = new System.Drawing.Point(6, 53);
            this.releaseDatePanel.Name = "releaseDatePanel";
            this.releaseDatePanel.Size = new System.Drawing.Size(787, 37);
            this.releaseDatePanel.TabIndex = 18;
            // 
            // releaseDateTFResponse
            // 
            this.releaseDateTFResponse.AutoSize = true;
            this.releaseDateTFResponse.ForeColor = System.Drawing.Color.Red;
            this.releaseDateTFResponse.Location = new System.Drawing.Point(722, 11);
            this.releaseDateTFResponse.Name = "releaseDateTFResponse";
            this.releaseDateTFResponse.Size = new System.Drawing.Size(64, 13);
            this.releaseDateTFResponse.TabIndex = 32;
            this.releaseDateTFResponse.Text = "Invalid Date";
            // 
            // releaseDateFixed
            // 
            this.releaseDateFixed.Appearance = System.Windows.Forms.Appearance.Button;
            this.releaseDateFixed.AutoSize = true;
            this.releaseDateFixed.BackColor = System.Drawing.Color.DarkRed;
            this.releaseDateFixed.FlatAppearance.BorderSize = 0;
            this.releaseDateFixed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.releaseDateFixed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.releaseDateFixed.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.releaseDateFixed.Location = new System.Drawing.Point(40, 3);
            this.releaseDateFixed.Name = "releaseDateFixed";
            this.releaseDateFixed.Size = new System.Drawing.Size(62, 23);
            this.releaseDateFixed.TabIndex = 31;
            this.releaseDateFixed.Text = "Not Fixed";
            this.releaseDateFixed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.releaseDateFixed.UseVisualStyleBackColor = false;
            this.releaseDateFixed.CheckedChanged += new System.EventHandler(this.FilterFixedChanged);
            // 
            // releaseDateTF
            // 
            this.releaseDateTF.Appearance = System.Windows.Forms.Appearance.Button;
            this.releaseDateTF.AutoSize = true;
            this.releaseDateTF.BackColor = System.Drawing.Color.DarkRed;
            this.releaseDateTF.FlatAppearance.BorderSize = 0;
            this.releaseDateTF.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.releaseDateTF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.releaseDateTF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.releaseDateTF.Location = new System.Drawing.Point(3, 3);
            this.releaseDateTF.Name = "releaseDateTF";
            this.releaseDateTF.Size = new System.Drawing.Size(31, 23);
            this.releaseDateTF.TabIndex = 28;
            this.releaseDateTF.Text = "Off";
            this.releaseDateTF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.releaseDateTF.UseVisualStyleBackColor = false;
            this.releaseDateTF.CheckedChanged += new System.EventHandler(this.ToggleThisFilter);
            // 
            // releaseDateTFToYear
            // 
            this.releaseDateTFToYear.Location = new System.Drawing.Point(658, 13);
            this.releaseDateTFToYear.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.releaseDateTFToYear.Minimum = new decimal(new int[] {
            1970,
            0,
            0,
            0});
            this.releaseDateTFToYear.Name = "releaseDateTFToYear";
            this.releaseDateTFToYear.Size = new System.Drawing.Size(53, 20);
            this.releaseDateTFToYear.TabIndex = 13;
            this.releaseDateTFToYear.Value = new decimal(new int[] {
            2017,
            0,
            0,
            0});
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(662, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(29, 13);
            this.label27.TabIndex = 12;
            this.label27.Text = "Year";
            // 
            // releaseDateTFToMonth
            // 
            this.releaseDateTFToMonth.Items.Add("Jan");
            this.releaseDateTFToMonth.Items.Add("Feb");
            this.releaseDateTFToMonth.Items.Add("Mar");
            this.releaseDateTFToMonth.Items.Add("Apr");
            this.releaseDateTFToMonth.Items.Add("May");
            this.releaseDateTFToMonth.Items.Add("Jun");
            this.releaseDateTFToMonth.Items.Add("Jul");
            this.releaseDateTFToMonth.Items.Add("Aug");
            this.releaseDateTFToMonth.Items.Add("Sep");
            this.releaseDateTFToMonth.Items.Add("Oct");
            this.releaseDateTFToMonth.Items.Add("Nov");
            this.releaseDateTFToMonth.Items.Add("Dec");
            this.releaseDateTFToMonth.Location = new System.Drawing.Point(599, 13);
            this.releaseDateTFToMonth.Name = "releaseDateTFToMonth";
            this.releaseDateTFToMonth.Size = new System.Drawing.Size(45, 20);
            this.releaseDateTFToMonth.TabIndex = 11;
            this.releaseDateTFToMonth.Text = "Jan";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(603, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(37, 13);
            this.label28.TabIndex = 10;
            this.label28.Text = "Month";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(538, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(26, 13);
            this.label29.TabIndex = 9;
            this.label29.Text = "Day";
            // 
            // releaseDateTFToDay
            // 
            this.releaseDateTFToDay.Location = new System.Drawing.Point(532, 13);
            this.releaseDateTFToDay.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.releaseDateTFToDay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.releaseDateTFToDay.Name = "releaseDateTFToDay";
            this.releaseDateTFToDay.Size = new System.Drawing.Size(42, 20);
            this.releaseDateTFToDay.TabIndex = 8;
            this.releaseDateTFToDay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(462, 12);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(25, 13);
            this.label22.TabIndex = 7;
            this.label22.Text = "and";
            // 
            // releaseDateTFFromYear
            // 
            this.releaseDateTFFromYear.Location = new System.Drawing.Point(367, 13);
            this.releaseDateTFFromYear.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.releaseDateTFFromYear.Minimum = new decimal(new int[] {
            1970,
            0,
            0,
            0});
            this.releaseDateTFFromYear.Name = "releaseDateTFFromYear";
            this.releaseDateTFFromYear.Size = new System.Drawing.Size(53, 20);
            this.releaseDateTFFromYear.TabIndex = 6;
            this.releaseDateTFFromYear.Value = new decimal(new int[] {
            2017,
            0,
            0,
            0});
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(366, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(29, 13);
            this.label34.TabIndex = 5;
            this.label34.Text = "Year";
            // 
            // releaseDateTFFromMonth
            // 
            this.releaseDateTFFromMonth.Items.Add("Jan");
            this.releaseDateTFFromMonth.Items.Add("Feb");
            this.releaseDateTFFromMonth.Items.Add("Mar");
            this.releaseDateTFFromMonth.Items.Add("Apr");
            this.releaseDateTFFromMonth.Items.Add("May");
            this.releaseDateTFFromMonth.Items.Add("Jun");
            this.releaseDateTFFromMonth.Items.Add("Jul");
            this.releaseDateTFFromMonth.Items.Add("Aug");
            this.releaseDateTFFromMonth.Items.Add("Sep");
            this.releaseDateTFFromMonth.Items.Add("Oct");
            this.releaseDateTFFromMonth.Items.Add("Nov");
            this.releaseDateTFFromMonth.Items.Add("Dec");
            this.releaseDateTFFromMonth.Location = new System.Drawing.Point(305, 13);
            this.releaseDateTFFromMonth.Name = "releaseDateTFFromMonth";
            this.releaseDateTFFromMonth.Size = new System.Drawing.Size(45, 20);
            this.releaseDateTFFromMonth.TabIndex = 4;
            this.releaseDateTFFromMonth.Text = "Jan";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(303, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(37, 13);
            this.label33.TabIndex = 3;
            this.label33.Text = "Month";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(250, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(26, 13);
            this.label32.TabIndex = 2;
            this.label32.Text = "Day";
            // 
            // releaseDateTFFromDay
            // 
            this.releaseDateTFFromDay.Location = new System.Drawing.Point(246, 13);
            this.releaseDateTFFromDay.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.releaseDateTFFromDay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.releaseDateTFFromDay.Name = "releaseDateTFFromDay";
            this.releaseDateTFFromDay.Size = new System.Drawing.Size(42, 20);
            this.releaseDateTFFromDay.TabIndex = 1;
            this.releaseDateTFFromDay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label31
            // 
            this.label31.Location = new System.Drawing.Point(108, 3);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(100, 23);
            this.label31.TabIndex = 0;
            this.label31.Text = "Released Between";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lengthPanel
            // 
            this.lengthPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lengthPanel.Controls.Add(this.lengthFixed);
            this.lengthPanel.Controls.Add(this.lengthTF);
            this.lengthPanel.Controls.Add(this.lengthTFOverFiftyHours);
            this.lengthPanel.Controls.Add(this.lengthTFThirtyToFiftyHours);
            this.lengthPanel.Controls.Add(this.lengthTFTenToThirtyHours);
            this.lengthPanel.Controls.Add(this.lengthTFTwoToTenHours);
            this.lengthPanel.Controls.Add(this.lengthTFUnderTwoHours);
            this.lengthPanel.Controls.Add(this.lengthTFNa);
            this.lengthPanel.Controls.Add(this.label14);
            this.lengthPanel.Location = new System.Drawing.Point(6, 14);
            this.lengthPanel.Name = "lengthPanel";
            this.lengthPanel.Size = new System.Drawing.Size(787, 37);
            this.lengthPanel.TabIndex = 17;
            // 
            // lengthFixed
            // 
            this.lengthFixed.Appearance = System.Windows.Forms.Appearance.Button;
            this.lengthFixed.AutoSize = true;
            this.lengthFixed.BackColor = System.Drawing.Color.DarkRed;
            this.lengthFixed.FlatAppearance.BorderSize = 0;
            this.lengthFixed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.lengthFixed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lengthFixed.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lengthFixed.Location = new System.Drawing.Point(40, 3);
            this.lengthFixed.Name = "lengthFixed";
            this.lengthFixed.Size = new System.Drawing.Size(62, 23);
            this.lengthFixed.TabIndex = 30;
            this.lengthFixed.Text = "Not Fixed";
            this.lengthFixed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lengthFixed.UseVisualStyleBackColor = false;
            this.lengthFixed.CheckedChanged += new System.EventHandler(this.FilterFixedChanged);
            // 
            // lengthTF
            // 
            this.lengthTF.Appearance = System.Windows.Forms.Appearance.Button;
            this.lengthTF.AutoSize = true;
            this.lengthTF.BackColor = System.Drawing.Color.DarkRed;
            this.lengthTF.FlatAppearance.BorderSize = 0;
            this.lengthTF.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.lengthTF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lengthTF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lengthTF.Location = new System.Drawing.Point(3, 3);
            this.lengthTF.Name = "lengthTF";
            this.lengthTF.Size = new System.Drawing.Size(31, 23);
            this.lengthTF.TabIndex = 29;
            this.lengthTF.Text = "Off";
            this.lengthTF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lengthTF.UseVisualStyleBackColor = false;
            this.lengthTF.CheckedChanged += new System.EventHandler(this.ToggleThisFilter);
            // 
            // lengthTFOverFiftyHours
            // 
            this.lengthTFOverFiftyHours.AutoSize = true;
            this.lengthTFOverFiftyHours.Location = new System.Drawing.Point(674, 7);
            this.lengthTFOverFiftyHours.Name = "lengthTFOverFiftyHours";
            this.lengthTFOverFiftyHours.Size = new System.Drawing.Size(75, 17);
            this.lengthTFOverFiftyHours.TabIndex = 6;
            this.lengthTFOverFiftyHours.Text = ">50 Hours";
            this.lengthTFOverFiftyHours.UseVisualStyleBackColor = true;
            this.lengthTFOverFiftyHours.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // lengthTFThirtyToFiftyHours
            // 
            this.lengthTFThirtyToFiftyHours.AutoSize = true;
            this.lengthTFThirtyToFiftyHours.Location = new System.Drawing.Point(582, 7);
            this.lengthTFThirtyToFiftyHours.Name = "lengthTFThirtyToFiftyHours";
            this.lengthTFThirtyToFiftyHours.Size = new System.Drawing.Size(84, 17);
            this.lengthTFThirtyToFiftyHours.TabIndex = 5;
            this.lengthTFThirtyToFiftyHours.Text = "30-50 Hours";
            this.lengthTFThirtyToFiftyHours.UseVisualStyleBackColor = true;
            this.lengthTFThirtyToFiftyHours.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // lengthTFTenToThirtyHours
            // 
            this.lengthTFTenToThirtyHours.AutoSize = true;
            this.lengthTFTenToThirtyHours.Location = new System.Drawing.Point(490, 7);
            this.lengthTFTenToThirtyHours.Name = "lengthTFTenToThirtyHours";
            this.lengthTFTenToThirtyHours.Size = new System.Drawing.Size(84, 17);
            this.lengthTFTenToThirtyHours.TabIndex = 4;
            this.lengthTFTenToThirtyHours.Text = "10-30 Hours";
            this.lengthTFTenToThirtyHours.UseVisualStyleBackColor = true;
            this.lengthTFTenToThirtyHours.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // lengthTFTwoToTenHours
            // 
            this.lengthTFTwoToTenHours.AutoSize = true;
            this.lengthTFTwoToTenHours.Location = new System.Drawing.Point(398, 7);
            this.lengthTFTwoToTenHours.Name = "lengthTFTwoToTenHours";
            this.lengthTFTwoToTenHours.Size = new System.Drawing.Size(78, 17);
            this.lengthTFTwoToTenHours.TabIndex = 3;
            this.lengthTFTwoToTenHours.Text = "2-10 Hours";
            this.lengthTFTwoToTenHours.UseVisualStyleBackColor = true;
            this.lengthTFTwoToTenHours.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // lengthTFUnderTwoHours
            // 
            this.lengthTFUnderTwoHours.AutoSize = true;
            this.lengthTFUnderTwoHours.Location = new System.Drawing.Point(306, 7);
            this.lengthTFUnderTwoHours.Name = "lengthTFUnderTwoHours";
            this.lengthTFUnderTwoHours.Size = new System.Drawing.Size(69, 17);
            this.lengthTFUnderTwoHours.TabIndex = 2;
            this.lengthTFUnderTwoHours.Text = "<2 Hours";
            this.lengthTFUnderTwoHours.UseVisualStyleBackColor = true;
            this.lengthTFUnderTwoHours.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // lengthTFNa
            // 
            this.lengthTFNa.AutoSize = true;
            this.lengthTFNa.Location = new System.Drawing.Point(214, 7);
            this.lengthTFNa.Name = "lengthTFNa";
            this.lengthTFNa.Size = new System.Drawing.Size(46, 17);
            this.lengthTFNa.TabIndex = 1;
            this.lengthTFNa.Text = "N/A";
            this.lengthTFNa.UseVisualStyleBackColor = true;
            this.lengthTFNa.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilterCheckboxClick);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(108, 3);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(100, 23);
            this.label14.TabIndex = 0;
            this.label14.Text = "Length";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olFavoriteProducers)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.traitFilteringBox.ResumeLayout(false);
            this.traitFilteringBox.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tileOLV)).EndInit();
            this.TabsControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.traitsPanel.ResumeLayout(false);
            this.traitsPanel.PerformLayout();
            this.tagsPanel.ResumeLayout(false);
            this.tagsPanel.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.userlistPanel.ResumeLayout(false);
            this.userlistPanel.PerformLayout();
            this.wishlistPanel.ResumeLayout(false);
            this.wishlistPanel.PerformLayout();
            this.favoriteProducersPanel.ResumeLayout(false);
            this.favoriteProducersPanel.PerformLayout();
            this.votedPanel.ResumeLayout(false);
            this.votedPanel.PerformLayout();
            this.blacklistedPanel.ResumeLayout(false);
            this.blacklistedPanel.PerformLayout();
            this.unreleasedPanel.ResumeLayout(false);
            this.unreleasedPanel.PerformLayout();
            this.releaseDatePanel.ResumeLayout(false);
            this.releaseDatePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.releaseDateTFToYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.releaseDateTFToDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.releaseDateTFFromYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.releaseDateTFFromDay)).EndInit();
            this.lengthPanel.ResumeLayout(false);
            this.lengthPanel.PerformLayout();
            this.ContextMenuVN.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void tileOLV_MouseClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
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
        private TabControl tabControl2;
        private TabPage traitFilteringBox;
        private ComboBox traitRootsDropdown;
        private Label traitReply;
        private TextBox traitSearchBox;
        private Label label3;
        private Label label5;
        private Button traitFilteringHelpButton;
        private TextBox customTraitFilterNameBox;
        private Button button4;
        private ComboBox customTraitFilters;
        private Button button5;
        private Button deleteCustomTraitFilterButton;
        private OLVColumn ol2GeneralRating;
        private ComboBox otherMethodsCB;
        private ToolStripMenuItem addChangeVNNoteToolStripMenuItem;
        private ToolStripMenuItem addChangeVNGroupsToolStripMenuItem;
        private Button sendQueryButton;
        private CheckBox advancedCheckBox;
        internal ComboBox ListByCBQuery;
        private ListBox traitSearchResultBox;
        private Button traitSearchButton;
        private ComboBox ListByCB;
        private TextBox ListByTB;
        private Button ListByUpdateButton;
        private Button ListByGoButton;
        private Panel panel3;
        private Button refreshAllProducersButton;
        private Button toggleViewButton;
        private ComboBox multiActionBox;
        private ToolStripMenuItem preciseNumberToolStripMenuItem;
        internal TabControl TabsControl;
        private Button button1;
        private OLVColumn tileColumnLength;
        private TabPage tabPage1;
        private Label label14;
        private Panel lengthPanel;
        private Panel releaseDatePanel;
        private Label label32;
        private Label label31;
        private Label label33;
        private Label label34;
        private Panel votedPanel;
        private Label label17;
        private Panel blacklistedPanel;
        private Label label16;
        private Panel unreleasedPanel;
        private Label label15;
        private Label label27;
        private Label label28;
        private Label label29;
        private Label label22;
        private Panel wishlistPanel;
        private Label label26;
        private Panel favoriteProducersPanel;
        private Label label25;
        private Panel userlistPanel;
        private Label label23;
        private Panel tagsPanel;
        private Button button6;
        private ListBox tagsTFLB;
        private Label label30;
        private Panel panel13;
        private ListBox originalLanguageTFLB;
        private Label label20;
        private Panel traitsPanel;
        private Button button7;
        private ListBox traitsTFLB;
        private Label label18;
        private ComboBox filterDropdown;
        private Label label19;
        internal CheckBox userlistFixed;
        internal CheckBox wishlistFixed;
        internal CheckBox favoriteProducersFixed;
        internal CheckBox votedFixed;
        internal CheckBox blacklistedFixed;
        internal CheckBox unreleasedFixed;
        internal CheckBox releaseDateFixed;
        internal CheckBox lengthFixed;
        internal CheckBox lengthTFUnderTwoHours;
        internal CheckBox lengthTFNa;
        internal CheckBox lengthTFThirtyToFiftyHours;
        internal CheckBox lengthTFTenToThirtyHours;
        internal CheckBox lengthTFTwoToTenHours;
        internal CheckBox lengthTFOverFiftyHours;
        internal CheckBox wishlistTFLow;
        internal CheckBox wishlistTFMedium;
        internal CheckBox wishlistTFHigh;
        internal CheckBox wishlistTFNa;
        internal CheckBox userlistTFDropped;
        internal CheckBox userlistTFStalled;
        internal CheckBox userlistTFFinished;
        internal CheckBox userlistTFPlaying;
        internal CheckBox userlistTFUnknown;
        internal CheckBox userlistTFNa;
        internal CheckBox tagsTF;
        internal CheckBox traitsTF;
        internal CheckBox userlistTF;
        internal CheckBox wishlistTF;
        internal CheckBox favoriteProducersTF;
        internal CheckBox votedTF;
        internal CheckBox blacklistedTF;
        internal CheckBox unreleasedTF;
        internal CheckBox releaseDateTF;
        internal CheckBox lengthTF;
        private RadioButton votedTFNo;
        private RadioButton votedTFYes;
        private RadioButton blacklistedTFNo;
        private RadioButton blacklistedTFYes;
        private RadioButton favoriteProducersTFNo;
        private RadioButton favoriteProducersTFYes;
        private Panel panel4;
        internal CheckBox languageFixed;
        private CheckBox languageTF;
        private ListBox languageTFLB;
        private Label label21;
        internal CheckBox traitsFixed;
        internal CheckBox tagsFixed;
        internal CheckBox originalLanguageFixed;
        private CheckBox originalLanguageTF;
        private Label releaseDateTFResponse;
        private NumericUpDown releaseDateTFFromDay;
        private DomainUpDown releaseDateTFFromMonth;
        private NumericUpDown releaseDateTFFromYear;
        private NumericUpDown releaseDateTFToYear;
        private DomainUpDown releaseDateTFToMonth;
        private NumericUpDown releaseDateTFToDay;
        internal CheckBox unreleasedTFReleased;
        internal CheckBox unreleasedTFWithoutReleaseDate;
        internal CheckBox unreleasedTFWithReleaseDate;
        private ListBox tagSearchResultBox;
        private Button tagSearchButton;
        private TextBox tagSearchBox;
        private Label label7;
        private Label label2;
        private Button TagFilteringHelpButton;
        private Button updateFilterResultsButton;
        private Label tagReply;
        private TextBox customTagFilterNameBox;
        private Button saveCustomFilterButton;
        private ComboBox customTagFilters;
        private Button clearFilterButton;
        private Button deleteCustomTagFilterButton;
        internal CheckBox tagsOrTraits;
        private ComboBox languageTFCB;
        private ComboBox originalLanguageTFCB;
        private Label label24;
        private Label label6;
    }
}

