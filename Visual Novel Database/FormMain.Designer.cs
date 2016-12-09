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
            this.refreshAllProducersButton = new System.Windows.Forms.Button();
            this.selectedProducersVNButton = new System.Windows.Forms.Button();
            this.removeProducersButton = new System.Windows.Forms.Button();
            this.suggestProducersButton = new System.Windows.Forms.Button();
            this.tagTypeS = new System.Windows.Forms.CheckBox();
            this.tagTypeT = new System.Windows.Forms.CheckBox();
            this.tagTypeC = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.customTagFilterNameBox = new System.Windows.Forms.TextBox();
            this.tagSearchBox = new System.Windows.Forms.TextBox();
            this.updateTagsAndTraitsButton = new System.Windows.Forms.Button();
            this.traitSearchBox = new System.Windows.Forms.TextBox();
            this.customTraitFilterNameBox = new System.Windows.Forms.TextBox();
            this.tagSignaler = new System.Windows.Forms.Button();
            this.traitSignaler = new System.Windows.Forms.Button();
            this.ToggleFiltersModeButton = new System.Windows.Forms.CheckBox();
            this.tagSearchButton = new System.Windows.Forms.Button();
            this.traitSearchButton = new System.Windows.Forms.Button();
            this.ListByGoButton = new System.Windows.Forms.Button();
            this.ListByUpdateButton = new System.Windows.Forms.Button();
            this.ListByCB = new System.Windows.Forms.ComboBox();
            this.infoTab = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
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
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.aboutTextBox = new System.Windows.Forms.RichTextBox();
            this.vnTab = new System.Windows.Forms.TabPage();
            this.ListByTB = new System.Windows.Forms.TextBox();
            this.groupListBox = new System.Windows.Forms.ComboBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tagFilteringBox = new System.Windows.Forms.TabPage();
            this.tagSearchResultBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TagFilteringHelpButton = new System.Windows.Forms.Button();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.updateFilterResultsButton = new System.Windows.Forms.Button();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.mctLoadingLabel = new System.Windows.Forms.Label();
            this.tagReply = new System.Windows.Forms.Label();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.saveCustomFilterButton = new System.Windows.Forms.Button();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.customTagFilters = new System.Windows.Forms.ComboBox();
            this.clearFilterButton = new System.Windows.Forms.Button();
            this.deleteCustomTagFilterButton = new System.Windows.Forms.Button();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.olFavoriteProducers = new BrightIdeasSoftware.ObjectListView();
            this.ol2Name = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2ItemCount = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2UserDropRate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2UserAverageVote = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2GeneralRating = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2Updated = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2ID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.favoriteProducersHelpButton = new System.Windows.Forms.Button();
            this.prodReply = new System.Windows.Forms.Label();
            this.listResultsButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.otherMethodsCB = new System.Windows.Forms.ComboBox();
            this.GetStartedHelpButton = new System.Windows.Forms.Button();
            this.nsfwToggle = new System.Windows.Forms.CheckBox();
            this.closeAllFormsButton = new System.Windows.Forms.Button();
            this.loginReply = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.userListButt = new System.Windows.Forms.Button();
            this.userListReply = new System.Windows.Forms.Label();
            this.wlStatusDropDown = new System.Windows.Forms.ComboBox();
            this.SearchingAndFilteringButton = new System.Windows.Forms.Button();
            this.BlacklistToggleBox = new System.Windows.Forms.ComboBox();
            this.UnreleasedToggleBox = new System.Windows.Forms.ComboBox();
            this.URTToggleBox = new System.Windows.Forms.ComboBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.viewPicker = new System.Windows.Forms.ComboBox();
            this.tileOLV = new BrightIdeasSoftware.ObjectListView();
            this.tileColumnTitle = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnProducer = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
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
            this.replyText = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ulStatusDropDown = new System.Windows.Forms.ComboBox();
            this.quickFilter0 = new System.Windows.Forms.Button();
            this.quickFilter1 = new System.Windows.Forms.Button();
            this.resultLabel = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
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
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showProducerTitlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addProducerToFavoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addChangeVNNoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addChangeVNGroupsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoTab.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.vnTab.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tagFilteringBox.SuspendLayout();
            this.traitFilteringBox.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olFavoriteProducers)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tileOLV)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.ContextMenuVN.SuspendLayout();
            this.SuspendLayout();
            // 
            // tagTypeS2
            // 
            this.tagTypeS2.AutoSize = true;
            this.tagTypeS2.Checked = true;
            this.tagTypeS2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tagTypeS2.Location = new System.Drawing.Point(636, 15);
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
            this.tagTypeT2.Location = new System.Drawing.Point(675, 15);
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
            this.tagTypeC2.Location = new System.Drawing.Point(597, 15);
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
            this.getNewProducersButton.Location = new System.Drawing.Point(3, 299);
            this.getNewProducersButton.Name = "getNewProducersButton";
            this.getNewProducersButton.Size = new System.Drawing.Size(148, 23);
            this.getNewProducersButton.TabIndex = 37;
            this.getNewProducersButton.Text = "Get New Producer Titles";
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
            this.addProducersButton.Location = new System.Drawing.Point(125, 241);
            this.addProducersButton.Name = "addProducersButton";
            this.addProducersButton.Size = new System.Drawing.Size(85, 23);
            this.addProducersButton.TabIndex = 36;
            this.addProducersButton.Text = "Add Producers";
            this.addProducersButton.UseVisualStyleBackColor = false;
            this.addProducersButton.Click += new System.EventHandler(this.AddProducers);
            // 
            // refreshAllProducersButton
            // 
            this.refreshAllProducersButton.BackColor = System.Drawing.Color.MistyRose;
            this.refreshAllProducersButton.FlatAppearance.BorderSize = 0;
            this.refreshAllProducersButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshAllProducersButton.ForeColor = System.Drawing.Color.Black;
            this.refreshAllProducersButton.Location = new System.Drawing.Point(157, 299);
            this.refreshAllProducersButton.Name = "refreshAllProducersButton";
            this.refreshAllProducersButton.Size = new System.Drawing.Size(164, 23);
            this.refreshAllProducersButton.TabIndex = 32;
            this.refreshAllProducersButton.Text = "Update All Producer Titles";
            this.toolTip.SetToolTip(this.refreshAllProducersButton, "Updates data on all titles by favorite producers , also gets new titles.");
            this.refreshAllProducersButton.UseVisualStyleBackColor = false;
            this.refreshAllProducersButton.Click += new System.EventHandler(this.RefreshAllFavoriteProducerTitles);
            // 
            // selectedProducersVNButton
            // 
            this.selectedProducersVNButton.BackColor = System.Drawing.Color.SteelBlue;
            this.selectedProducersVNButton.FlatAppearance.BorderSize = 0;
            this.selectedProducersVNButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectedProducersVNButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.selectedProducersVNButton.Location = new System.Drawing.Point(3, 270);
            this.selectedProducersVNButton.Name = "selectedProducersVNButton";
            this.selectedProducersVNButton.Size = new System.Drawing.Size(148, 23);
            this.selectedProducersVNButton.TabIndex = 33;
            this.selectedProducersVNButton.Text = "Show Titles By Selected";
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
            this.removeProducersButton.Location = new System.Drawing.Point(157, 271);
            this.removeProducersButton.Name = "removeProducersButton";
            this.removeProducersButton.Size = new System.Drawing.Size(164, 23);
            this.removeProducersButton.TabIndex = 7;
            this.removeProducersButton.Text = "Remove Selected From List";
            this.toolTip.SetToolTip(this.removeProducersButton, "Remove selected producers from favorite producers list.");
            this.removeProducersButton.UseVisualStyleBackColor = false;
            this.removeProducersButton.Click += new System.EventHandler(this.RemoveProducers);
            // 
            // suggestProducersButton
            // 
            this.suggestProducersButton.BackColor = System.Drawing.Color.SteelBlue;
            this.suggestProducersButton.FlatAppearance.BorderSize = 0;
            this.suggestProducersButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.suggestProducersButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.suggestProducersButton.Location = new System.Drawing.Point(216, 241);
            this.suggestProducersButton.Name = "suggestProducersButton";
            this.suggestProducersButton.Size = new System.Drawing.Size(105, 23);
            this.suggestProducersButton.TabIndex = 89;
            this.suggestProducersButton.Text = "Suggest Producers";
            this.toolTip.SetToolTip(this.suggestProducersButton, "Displays a list of producers with 2+ finished titles by the user.");
            this.suggestProducersButton.UseVisualStyleBackColor = false;
            this.suggestProducersButton.Click += new System.EventHandler(this.SuggestProducers);
            // 
            // tagTypeS
            // 
            this.tagTypeS.AutoSize = true;
            this.tagTypeS.Checked = true;
            this.tagTypeS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tagTypeS.Location = new System.Drawing.Point(152, 2);
            this.tagTypeS.Name = "tagTypeS";
            this.tagTypeS.Size = new System.Drawing.Size(33, 17);
            this.tagTypeS.TabIndex = 37;
            this.tagTypeS.Text = "S";
            this.tagTypeS.UseVisualStyleBackColor = true;
            this.tagTypeS.Click += new System.EventHandler(this.DisplayCommonTags);
            // 
            // tagTypeT
            // 
            this.tagTypeT.AutoSize = true;
            this.tagTypeT.Checked = true;
            this.tagTypeT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tagTypeT.Location = new System.Drawing.Point(191, 2);
            this.tagTypeT.Name = "tagTypeT";
            this.tagTypeT.Size = new System.Drawing.Size(33, 17);
            this.tagTypeT.TabIndex = 36;
            this.tagTypeT.Text = "T";
            this.tagTypeT.UseVisualStyleBackColor = true;
            this.tagTypeT.Click += new System.EventHandler(this.DisplayCommonTags);
            // 
            // tagTypeC
            // 
            this.tagTypeC.AutoSize = true;
            this.tagTypeC.Checked = true;
            this.tagTypeC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tagTypeC.Location = new System.Drawing.Point(113, 2);
            this.tagTypeC.Name = "tagTypeC";
            this.tagTypeC.Size = new System.Drawing.Size(33, 17);
            this.tagTypeC.TabIndex = 35;
            this.tagTypeC.Text = "C";
            this.tagTypeC.UseVisualStyleBackColor = true;
            this.tagTypeC.Click += new System.EventHandler(this.DisplayCommonTags);
            // 
            // customTagFilterNameBox
            // 
            this.customTagFilterNameBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTagFilterNameBox.Location = new System.Drawing.Point(446, 19);
            this.customTagFilterNameBox.Name = "customTagFilterNameBox";
            this.customTagFilterNameBox.Size = new System.Drawing.Size(100, 22);
            this.customTagFilterNameBox.TabIndex = 41;
            this.toolTip.SetToolTip(this.customTagFilterNameBox, "Enter Custom Filter name here");
            this.customTagFilterNameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EnterCustomTagFilterName);
            // 
            // tagSearchBox
            // 
            this.tagSearchBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tagSearchBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tagSearchBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagSearchBox.Location = new System.Drawing.Point(274, 1);
            this.tagSearchBox.Name = "tagSearchBox";
            this.tagSearchBox.Size = new System.Drawing.Size(140, 22);
            this.tagSearchBox.TabIndex = 29;
            this.toolTip.SetToolTip(this.tagSearchBox, "Enter tag name/alias here.");
            this.tagSearchBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AddTagBySearch);
            // 
            // updateTagsAndTraitsButton
            // 
            this.updateTagsAndTraitsButton.BackColor = System.Drawing.Color.MistyRose;
            this.updateTagsAndTraitsButton.FlatAppearance.BorderSize = 0;
            this.updateTagsAndTraitsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateTagsAndTraitsButton.ForeColor = System.Drawing.Color.Black;
            this.updateTagsAndTraitsButton.Location = new System.Drawing.Point(3, 91);
            this.updateTagsAndTraitsButton.Name = "updateTagsAndTraitsButton";
            this.updateTagsAndTraitsButton.Size = new System.Drawing.Size(131, 23);
            this.updateTagsAndTraitsButton.TabIndex = 91;
            this.updateTagsAndTraitsButton.Text = "Update Title Data";
            this.toolTip.SetToolTip(this.updateTagsAndTraitsButton, "Update tags, traits and stats of titles that haven\'t been updated in over 7 days." +
        "");
            this.updateTagsAndTraitsButton.UseVisualStyleBackColor = false;
            this.updateTagsAndTraitsButton.Click += new System.EventHandler(this.UpdateTitleDataClick);
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
            // tagSignaler
            // 
            this.tagSignaler.BackColor = System.Drawing.Color.LightGray;
            this.tagSignaler.FlatAppearance.BorderSize = 0;
            this.tagSignaler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tagSignaler.Location = new System.Drawing.Point(640, 337);
            this.tagSignaler.Name = "tagSignaler";
            this.tagSignaler.Size = new System.Drawing.Size(23, 23);
            this.tagSignaler.TabIndex = 101;
            this.toolTip.SetToolTip(this.tagSignaler, "Red when tag filter is active, Gray when not active, click to clear filter.");
            this.tagSignaler.UseVisualStyleBackColor = false;
            this.tagSignaler.Click += new System.EventHandler(this.ClearTagFilter);
            // 
            // traitSignaler
            // 
            this.traitSignaler.BackColor = System.Drawing.Color.LightGray;
            this.traitSignaler.FlatAppearance.BorderSize = 0;
            this.traitSignaler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.traitSignaler.Location = new System.Drawing.Point(686, 337);
            this.traitSignaler.Name = "traitSignaler";
            this.traitSignaler.Size = new System.Drawing.Size(23, 23);
            this.traitSignaler.TabIndex = 92;
            this.toolTip.SetToolTip(this.traitSignaler, "Red when trait filter is active, Gray when not active, click to clear filter.");
            this.traitSignaler.UseVisualStyleBackColor = false;
            this.traitSignaler.Click += new System.EventHandler(this.ClearTraitFilter);
            // 
            // ToggleFiltersModeButton
            // 
            this.ToggleFiltersModeButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.ToggleFiltersModeButton.BackColor = System.Drawing.Color.Black;
            this.ToggleFiltersModeButton.FlatAppearance.BorderSize = 0;
            this.ToggleFiltersModeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ToggleFiltersModeButton.Location = new System.Drawing.Point(640, 339);
            this.ToggleFiltersModeButton.Name = "ToggleFiltersModeButton";
            this.ToggleFiltersModeButton.Size = new System.Drawing.Size(69, 19);
            this.ToggleFiltersModeButton.TabIndex = 103;
            this.ToggleFiltersModeButton.Text = "And";
            this.ToggleFiltersModeButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.ToggleFiltersModeButton, "Click to toggle between \'and\' and \'or\'. Changes how tag/trait filters are used.");
            this.ToggleFiltersModeButton.UseVisualStyleBackColor = false;
            this.ToggleFiltersModeButton.CheckedChanged += new System.EventHandler(this.ToggleFiltersMode);
            // 
            // tagSearchButton
            // 
            this.tagSearchButton.BackColor = System.Drawing.Color.SteelBlue;
            this.tagSearchButton.FlatAppearance.BorderSize = 0;
            this.tagSearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tagSearchButton.ForeColor = System.Drawing.Color.White;
            this.tagSearchButton.Location = new System.Drawing.Point(420, 1);
            this.tagSearchButton.Name = "tagSearchButton";
            this.tagSearchButton.Size = new System.Drawing.Size(17, 22);
            this.tagSearchButton.TabIndex = 94;
            this.tagSearchButton.Text = "S";
            this.toolTip.SetToolTip(this.tagSearchButton, "Search for a tag based on name and/or alias.");
            this.tagSearchButton.UseVisualStyleBackColor = false;
            this.tagSearchButton.Click += new System.EventHandler(this.SearchTags);
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
            this.ListByGoButton.Location = new System.Drawing.Point(264, 308);
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
            this.ListByUpdateButton.Location = new System.Drawing.Point(300, 308);
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
            this.ListByCB.Location = new System.Drawing.Point(38, 309);
            this.ListByCB.Name = "ListByCB";
            this.ListByCB.Size = new System.Drawing.Size(95, 21);
            this.ListByCB.TabIndex = 104;
            this.toolTip.SetToolTip(this.ListByCB, "Select method by which to list titles.");
            this.ListByCB.SelectedIndexChanged += new System.EventHandler(this.ChangeListBy);
            // 
            // infoTab
            // 
            this.infoTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.infoTab.Controls.Add(this.groupBox3);
            this.infoTab.Controls.Add(this.statBox);
            this.infoTab.Controls.Add(this.groupBox1);
            this.infoTab.Controls.Add(this.groupBox9);
            this.infoTab.Location = new System.Drawing.Point(4, 22);
            this.infoTab.Name = "infoTab";
            this.infoTab.Padding = new System.Windows.Forms.Padding(3);
            this.infoTab.Size = new System.Drawing.Size(1196, 595);
            this.infoTab.TabIndex = 2;
            this.infoTab.Text = "Information";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.advancedCheckBox);
            this.groupBox3.Controls.Add(this.sendQueryButton);
            this.groupBox3.Controls.Add(this.serverR);
            this.groupBox3.Controls.Add(this.logReplyLabel);
            this.groupBox3.Controls.Add(this.clearLogButton);
            this.groupBox3.Controls.Add(this.serverQ);
            this.groupBox3.Controls.Add(this.logQueryLabel);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.questionBox);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox3.Location = new System.Drawing.Point(8, 189);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1394, 560);
            this.groupBox3.TabIndex = 35;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Log";
            // 
            // advancedCheckBox
            // 
            this.advancedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.advancedCheckBox.AutoSize = true;
            this.advancedCheckBox.Location = new System.Drawing.Point(233, 535);
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
            this.serverR.Size = new System.Drawing.Size(1044, 522);
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
            this.clearLogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearLogButton.BackColor = System.Drawing.Color.MistyRose;
            this.clearLogButton.Enabled = false;
            this.clearLogButton.FlatAppearance.BorderSize = 0;
            this.clearLogButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearLogButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.clearLogButton.Location = new System.Drawing.Point(6, 531);
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
            this.serverQ.Size = new System.Drawing.Size(334, 394);
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
            this.statBox.Location = new System.Drawing.Point(1263, 6);
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
            this.groupBox1.Location = new System.Drawing.Point(540, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(717, 177);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Stats";
            // 
            // mctULLabel10
            // 
            this.mctULLabel10.Location = new System.Drawing.Point(458, 150);
            this.mctULLabel10.Name = "mctULLabel10";
            this.mctULLabel10.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel10.TabIndex = 74;
            this.mctULLabel10.Text = "(blank)";
            // 
            // mctULLabel9
            // 
            this.mctULLabel9.Location = new System.Drawing.Point(458, 137);
            this.mctULLabel9.Name = "mctULLabel9";
            this.mctULLabel9.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel9.TabIndex = 73;
            this.mctULLabel9.Text = "(blank)";
            // 
            // mctULLabel8
            // 
            this.mctULLabel8.Location = new System.Drawing.Point(458, 124);
            this.mctULLabel8.Name = "mctULLabel8";
            this.mctULLabel8.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel8.TabIndex = 72;
            this.mctULLabel8.Text = "(blank)";
            // 
            // mctULLabel7
            // 
            this.mctULLabel7.Location = new System.Drawing.Point(458, 111);
            this.mctULLabel7.Name = "mctULLabel7";
            this.mctULLabel7.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel7.TabIndex = 71;
            this.mctULLabel7.Text = "(blank)";
            // 
            // mctULLabel6
            // 
            this.mctULLabel6.Location = new System.Drawing.Point(458, 98);
            this.mctULLabel6.Name = "mctULLabel6";
            this.mctULLabel6.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel6.TabIndex = 70;
            this.mctULLabel6.Text = "(blank)";
            // 
            // mctULLabel5
            // 
            this.mctULLabel5.Location = new System.Drawing.Point(458, 85);
            this.mctULLabel5.Name = "mctULLabel5";
            this.mctULLabel5.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel5.TabIndex = 69;
            this.mctULLabel5.Text = "(blank)";
            // 
            // mctULLabel4
            // 
            this.mctULLabel4.Location = new System.Drawing.Point(458, 72);
            this.mctULLabel4.Name = "mctULLabel4";
            this.mctULLabel4.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel4.TabIndex = 68;
            this.mctULLabel4.Text = "(blank)";
            // 
            // mctULLabel3
            // 
            this.mctULLabel3.Location = new System.Drawing.Point(458, 59);
            this.mctULLabel3.Name = "mctULLabel3";
            this.mctULLabel3.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel3.TabIndex = 67;
            this.mctULLabel3.Text = "(blank)";
            // 
            // mctULLabel2
            // 
            this.mctULLabel2.Location = new System.Drawing.Point(458, 46);
            this.mctULLabel2.Name = "mctULLabel2";
            this.mctULLabel2.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel2.TabIndex = 66;
            this.mctULLabel2.Text = "(blank)";
            // 
            // mctULLabel1
            // 
            this.mctULLabel1.Location = new System.Drawing.Point(458, 33);
            this.mctULLabel1.Name = "mctULLabel1";
            this.mctULLabel1.Size = new System.Drawing.Size(253, 13);
            this.mctULLabel1.TabIndex = 65;
            this.mctULLabel1.Text = "(blank)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new System.Drawing.Point(458, 16);
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
            // groupBox9
            // 
            this.groupBox9.BackColor = System.Drawing.Color.Transparent;
            this.groupBox9.Controls.Add(this.aboutTextBox);
            this.groupBox9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox9.Location = new System.Drawing.Point(8, 6);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(526, 177);
            this.groupBox9.TabIndex = 1;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "About";
            // 
            // aboutTextBox
            // 
            this.aboutTextBox.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.aboutTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.aboutTextBox.Enabled = false;
            this.aboutTextBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.aboutTextBox.Location = new System.Drawing.Point(6, 19);
            this.aboutTextBox.Name = "aboutTextBox";
            this.aboutTextBox.ReadOnly = true;
            this.aboutTextBox.Size = new System.Drawing.Size(514, 152);
            this.aboutTextBox.TabIndex = 0;
            this.aboutTextBox.Text = "";
            // 
            // vnTab
            // 
            this.vnTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.vnTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.vnTab.Controls.Add(this.ListByGoButton);
            this.vnTab.Controls.Add(this.ListByUpdateButton);
            this.vnTab.Controls.Add(this.ListByTB);
            this.vnTab.Controls.Add(this.ListByCB);
            this.vnTab.Controls.Add(this.tagSignaler);
            this.vnTab.Controls.Add(this.traitSignaler);
            this.vnTab.Controls.Add(this.groupListBox);
            this.vnTab.Controls.Add(this.tabControl2);
            this.vnTab.Controls.Add(this.panel2);
            this.vnTab.Controls.Add(this.listResultsButton);
            this.vnTab.Controls.Add(this.panel1);
            this.vnTab.Controls.Add(this.wlStatusDropDown);
            this.vnTab.Controls.Add(this.SearchingAndFilteringButton);
            this.vnTab.Controls.Add(this.BlacklistToggleBox);
            this.vnTab.Controls.Add(this.UnreleasedToggleBox);
            this.vnTab.Controls.Add(this.URTToggleBox);
            this.vnTab.Controls.Add(this.statusLabel);
            this.vnTab.Controls.Add(this.viewPicker);
            this.vnTab.Controls.Add(this.tileOLV);
            this.vnTab.Controls.Add(this.replyText);
            this.vnTab.Controls.Add(this.label4);
            this.vnTab.Controls.Add(this.ulStatusDropDown);
            this.vnTab.Controls.Add(this.quickFilter0);
            this.vnTab.Controls.Add(this.quickFilter1);
            this.vnTab.Controls.Add(this.resultLabel);
            this.vnTab.Controls.Add(this.ToggleFiltersModeButton);
            this.vnTab.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.vnTab.Location = new System.Drawing.Point(4, 22);
            this.vnTab.Name = "vnTab";
            this.vnTab.Padding = new System.Windows.Forms.Padding(3);
            this.vnTab.Size = new System.Drawing.Size(1196, 595);
            this.vnTab.TabIndex = 1;
            this.vnTab.Text = "Visual Novel Info";
            // 
            // ListByTB
            // 
            this.ListByTB.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.ListByTB.Location = new System.Drawing.Point(139, 310);
            this.ListByTB.Name = "ListByTB";
            this.ListByTB.Size = new System.Drawing.Size(119, 20);
            this.ListByTB.TabIndex = 105;
            this.ListByTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ListByTB_KeyPress);
            this.ListByTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ListByTB_KeyUp);
            // 
            // groupListBox
            // 
            this.groupListBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.groupListBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.groupListBox.BackColor = System.Drawing.Color.White;
            this.groupListBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupListBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupListBox.FormattingEnabled = true;
            this.groupListBox.Items.AddRange(new object[] {
            "Show URT",
            "Hide URT",
            "Only URT",
            "Only Unplayed"});
            this.groupListBox.Location = new System.Drawing.Point(139, 310);
            this.groupListBox.Name = "groupListBox";
            this.groupListBox.Size = new System.Drawing.Size(119, 21);
            this.groupListBox.TabIndex = 100;
            this.groupListBox.Visible = false;
            this.groupListBox.SelectedIndexChanged += new System.EventHandler(this.List_Group);
            this.groupListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.List_GroupEnter);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tagFilteringBox);
            this.tabControl2.Controls.Add(this.traitFilteringBox);
            this.tabControl2.Location = new System.Drawing.Point(151, 6);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(572, 299);
            this.tabControl2.TabIndex = 97;
            // 
            // tagFilteringBox
            // 
            this.tagFilteringBox.AutoScroll = true;
            this.tagFilteringBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.tagFilteringBox.Controls.Add(this.tagSearchButton);
            this.tagFilteringBox.Controls.Add(this.tagSearchResultBox);
            this.tagFilteringBox.Controls.Add(this.label2);
            this.tagFilteringBox.Controls.Add(this.label6);
            this.tagFilteringBox.Controls.Add(this.TagFilteringHelpButton);
            this.tagFilteringBox.Controls.Add(this.tagTypeS);
            this.tagFilteringBox.Controls.Add(this.checkBox6);
            this.tagFilteringBox.Controls.Add(this.updateFilterResultsButton);
            this.tagFilteringBox.Controls.Add(this.checkBox5);
            this.tagFilteringBox.Controls.Add(this.tagSearchBox);
            this.tagFilteringBox.Controls.Add(this.mctLoadingLabel);
            this.tagFilteringBox.Controls.Add(this.tagReply);
            this.tagFilteringBox.Controls.Add(this.tagTypeT);
            this.tagFilteringBox.Controls.Add(this.checkBox10);
            this.tagFilteringBox.Controls.Add(this.checkBox3);
            this.tagFilteringBox.Controls.Add(this.customTagFilterNameBox);
            this.tagFilteringBox.Controls.Add(this.checkBox4);
            this.tagFilteringBox.Controls.Add(this.checkBox7);
            this.tagFilteringBox.Controls.Add(this.saveCustomFilterButton);
            this.tagFilteringBox.Controls.Add(this.checkBox8);
            this.tagFilteringBox.Controls.Add(this.tagTypeC);
            this.tagFilteringBox.Controls.Add(this.checkBox1);
            this.tagFilteringBox.Controls.Add(this.checkBox2);
            this.tagFilteringBox.Controls.Add(this.label7);
            this.tagFilteringBox.Controls.Add(this.customTagFilters);
            this.tagFilteringBox.Controls.Add(this.clearFilterButton);
            this.tagFilteringBox.Controls.Add(this.deleteCustomTagFilterButton);
            this.tagFilteringBox.Controls.Add(this.checkBox9);
            this.tagFilteringBox.Location = new System.Drawing.Point(4, 22);
            this.tagFilteringBox.Name = "tagFilteringBox";
            this.tagFilteringBox.Padding = new System.Windows.Forms.Padding(3);
            this.tagFilteringBox.Size = new System.Drawing.Size(564, 273);
            this.tagFilteringBox.TabIndex = 0;
            this.tagFilteringBox.Text = "Tag Filtering";
            this.tagFilteringBox.Click += new System.EventHandler(this.ClearListBox);
            // 
            // tagSearchResultBox
            // 
            this.tagSearchResultBox.FormattingEnabled = true;
            this.tagSearchResultBox.Location = new System.Drawing.Point(274, 28);
            this.tagSearchResultBox.Name = "tagSearchResultBox";
            this.tagSearchResultBox.Size = new System.Drawing.Size(163, 238);
            this.tagSearchResultBox.TabIndex = 93;
            this.tagSearchResultBox.Visible = false;
            this.tagSearchResultBox.SelectedIndexChanged += new System.EventHandler(this.AddTagFromList);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(443, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 91;
            this.label2.Text = "Custom Filter Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Most Common Tags";
            // 
            // TagFilteringHelpButton
            // 
            this.TagFilteringHelpButton.BackColor = System.Drawing.Color.Khaki;
            this.TagFilteringHelpButton.FlatAppearance.BorderSize = 0;
            this.TagFilteringHelpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TagFilteringHelpButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TagFilteringHelpButton.Location = new System.Drawing.Point(446, 249);
            this.TagFilteringHelpButton.Name = "TagFilteringHelpButton";
            this.TagFilteringHelpButton.Size = new System.Drawing.Size(100, 23);
            this.TagFilteringHelpButton.TabIndex = 90;
            this.TagFilteringHelpButton.Text = "Tag Filtering";
            this.TagFilteringHelpButton.UseVisualStyleBackColor = false;
            this.TagFilteringHelpButton.Click += new System.EventHandler(this.Help_TagFiltering);
            // 
            // checkBox6
            // 
            this.checkBox6.Location = new System.Drawing.Point(6, 140);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(200, 17);
            this.checkBox6.TabIndex = 3;
            this.checkBox6.Text = "checkBox6";
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // updateFilterResultsButton
            // 
            this.updateFilterResultsButton.BackColor = System.Drawing.Color.MistyRose;
            this.updateFilterResultsButton.FlatAppearance.BorderSize = 0;
            this.updateFilterResultsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateFilterResultsButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.updateFilterResultsButton.Location = new System.Drawing.Point(446, 161);
            this.updateFilterResultsButton.Name = "updateFilterResultsButton";
            this.updateFilterResultsButton.Size = new System.Drawing.Size(100, 23);
            this.updateFilterResultsButton.TabIndex = 50;
            this.updateFilterResultsButton.Text = "Update Results";
            this.updateFilterResultsButton.UseVisualStyleBackColor = false;
            this.updateFilterResultsButton.Click += new System.EventHandler(this.UpdateResults);
            // 
            // checkBox5
            // 
            this.checkBox5.Location = new System.Drawing.Point(6, 117);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(200, 17);
            this.checkBox5.TabIndex = 4;
            this.checkBox5.Text = "checkBox5";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // mctLoadingLabel
            // 
            this.mctLoadingLabel.AutoSize = true;
            this.mctLoadingLabel.Location = new System.Drawing.Point(6, 252);
            this.mctLoadingLabel.Name = "mctLoadingLabel";
            this.mctLoadingLabel.Size = new System.Drawing.Size(94, 13);
            this.mctLoadingLabel.TabIndex = 38;
            this.mctLoadingLabel.Text = "(mctLoadingLabel)";
            // 
            // tagReply
            // 
            this.tagReply.Location = new System.Drawing.Point(446, 187);
            this.tagReply.Name = "tagReply";
            this.tagReply.Size = new System.Drawing.Size(100, 57);
            this.tagReply.TabIndex = 49;
            this.tagReply.Text = "(tagReply)";
            this.tagReply.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // checkBox10
            // 
            this.checkBox10.Location = new System.Drawing.Point(6, 232);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(200, 17);
            this.checkBox10.TabIndex = 9;
            this.checkBox10.Text = "checkBox10";
            this.checkBox10.UseVisualStyleBackColor = true;
            this.checkBox10.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // checkBox3
            // 
            this.checkBox3.Location = new System.Drawing.Point(6, 73);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(200, 17);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Text = "checkBox3";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // checkBox4
            // 
            this.checkBox4.Location = new System.Drawing.Point(6, 94);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(200, 17);
            this.checkBox4.TabIndex = 5;
            this.checkBox4.Text = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // checkBox7
            // 
            this.checkBox7.Location = new System.Drawing.Point(6, 163);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(200, 17);
            this.checkBox7.TabIndex = 8;
            this.checkBox7.Text = "checkBox7";
            this.checkBox7.UseVisualStyleBackColor = true;
            this.checkBox7.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // saveCustomFilterButton
            // 
            this.saveCustomFilterButton.BackColor = System.Drawing.Color.SteelBlue;
            this.saveCustomFilterButton.FlatAppearance.BorderSize = 0;
            this.saveCustomFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveCustomFilterButton.ForeColor = System.Drawing.Color.White;
            this.saveCustomFilterButton.Location = new System.Drawing.Point(446, 74);
            this.saveCustomFilterButton.Name = "saveCustomFilterButton";
            this.saveCustomFilterButton.Size = new System.Drawing.Size(100, 23);
            this.saveCustomFilterButton.TabIndex = 39;
            this.saveCustomFilterButton.Text = "Save Filter";
            this.saveCustomFilterButton.UseVisualStyleBackColor = false;
            this.saveCustomFilterButton.Click += new System.EventHandler(this.SaveCustomTagFilter);
            // 
            // checkBox8
            // 
            this.checkBox8.Location = new System.Drawing.Point(6, 186);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(200, 17);
            this.checkBox8.TabIndex = 7;
            this.checkBox8.Text = "checkBox8";
            this.checkBox8.UseVisualStyleBackColor = true;
            this.checkBox8.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(6, 28);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(200, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // checkBox2
            // 
            this.checkBox2.Location = new System.Drawing.Point(6, 50);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(200, 17);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(224, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Search:";
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
            this.customTagFilters.Location = new System.Drawing.Point(446, 47);
            this.customTagFilters.Name = "customTagFilters";
            this.customTagFilters.Size = new System.Drawing.Size(100, 21);
            this.customTagFilters.TabIndex = 57;
            this.customTagFilters.SelectedIndexChanged += new System.EventHandler(this.Filter_CustomTags);
            // 
            // clearFilterButton
            // 
            this.clearFilterButton.BackColor = System.Drawing.Color.SteelBlue;
            this.clearFilterButton.FlatAppearance.BorderSize = 0;
            this.clearFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearFilterButton.ForeColor = System.Drawing.Color.White;
            this.clearFilterButton.Location = new System.Drawing.Point(446, 132);
            this.clearFilterButton.Name = "clearFilterButton";
            this.clearFilterButton.Size = new System.Drawing.Size(100, 23);
            this.clearFilterButton.TabIndex = 40;
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
            this.deleteCustomTagFilterButton.Location = new System.Drawing.Point(446, 103);
            this.deleteCustomTagFilterButton.Name = "deleteCustomTagFilterButton";
            this.deleteCustomTagFilterButton.Size = new System.Drawing.Size(100, 23);
            this.deleteCustomTagFilterButton.TabIndex = 78;
            this.deleteCustomTagFilterButton.Text = "Delete Filter";
            this.deleteCustomTagFilterButton.UseVisualStyleBackColor = false;
            this.deleteCustomTagFilterButton.Click += new System.EventHandler(this.DeleteCustomTagFilter);
            // 
            // checkBox9
            // 
            this.checkBox9.Location = new System.Drawing.Point(6, 209);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(200, 17);
            this.checkBox9.TabIndex = 6;
            this.checkBox9.Text = "checkBox9";
            this.checkBox9.UseVisualStyleBackColor = true;
            this.checkBox9.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
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
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.suggestProducersButton);
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
            this.panel2.Size = new System.Drawing.Size(463, 329);
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
            // favoriteProducersHelpButton
            // 
            this.favoriteProducersHelpButton.BackColor = System.Drawing.Color.Khaki;
            this.favoriteProducersHelpButton.FlatAppearance.BorderSize = 0;
            this.favoriteProducersHelpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.favoriteProducersHelpButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.favoriteProducersHelpButton.Location = new System.Drawing.Point(3, 241);
            this.favoriteProducersHelpButton.Name = "favoriteProducersHelpButton";
            this.favoriteProducersHelpButton.Size = new System.Drawing.Size(116, 23);
            this.favoriteProducersHelpButton.TabIndex = 88;
            this.favoriteProducersHelpButton.Text = "Favorite Producers";
            this.favoriteProducersHelpButton.UseVisualStyleBackColor = false;
            this.favoriteProducersHelpButton.Click += new System.EventHandler(this.Help_FavoriteProducers);
            // 
            // prodReply
            // 
            this.prodReply.Location = new System.Drawing.Point(327, 242);
            this.prodReply.Name = "prodReply";
            this.prodReply.Size = new System.Drawing.Size(130, 80);
            this.prodReply.TabIndex = 32;
            this.prodReply.Text = "(prodReply)";
            // 
            // listResultsButton
            // 
            this.listResultsButton.BackColor = System.Drawing.Color.Khaki;
            this.listResultsButton.FlatAppearance.BorderSize = 0;
            this.listResultsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.listResultsButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listResultsButton.Location = new System.Drawing.Point(560, 338);
            this.listResultsButton.Name = "listResultsButton";
            this.listResultsButton.Size = new System.Drawing.Size(74, 23);
            this.listResultsButton.TabIndex = 95;
            this.listResultsButton.Text = "List Results";
            this.listResultsButton.UseVisualStyleBackColor = false;
            this.listResultsButton.Click += new System.EventHandler(this.Help_ListResults);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.otherMethodsCB);
            this.panel1.Controls.Add(this.yearLimitBox);
            this.panel1.Controls.Add(this.GetStartedHelpButton);
            this.panel1.Controls.Add(this.autoUpdateURTBox);
            this.panel1.Controls.Add(this.updateTagsAndTraitsButton);
            this.panel1.Controls.Add(this.nsfwToggle);
            this.panel1.Controls.Add(this.closeAllFormsButton);
            this.panel1.Controls.Add(this.loginReply);
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
            this.otherMethodsCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.otherMethodsCB.FormattingEnabled = true;
            this.otherMethodsCB.Items.AddRange(new object[] {
            "Other Functions",
            "----------------",
            "Get All VN Stats",
            "Get All Character Data",
            "Get Missing Covers",
            "Update Title Data (All)"});
            this.otherMethodsCB.Location = new System.Drawing.Point(3, 120);
            this.otherMethodsCB.Name = "otherMethodsCB";
            this.otherMethodsCB.Size = new System.Drawing.Size(131, 21);
            this.otherMethodsCB.TabIndex = 92;
            this.otherMethodsCB.SelectedIndexChanged += new System.EventHandler(this.OtherMethodChosen);
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
            this.GetStartedHelpButton.UseVisualStyleBackColor = false;
            this.GetStartedHelpButton.Click += new System.EventHandler(this.Help_GetStarted);
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
            this.closeAllFormsButton.Text = "Close All VN Windows";
            this.closeAllFormsButton.UseVisualStyleBackColor = false;
            this.closeAllFormsButton.Click += new System.EventHandler(this.CloseAllForms);
            // 
            // loginReply
            // 
            this.loginReply.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.loginReply.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.loginReply.Location = new System.Drawing.Point(3, 4);
            this.loginReply.Name = "loginReply";
            this.loginReply.Size = new System.Drawing.Size(131, 26);
            this.loginReply.TabIndex = 30;
            this.loginReply.Text = "(loginReply)";
            this.loginReply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // loginButton
            // 
            this.loginButton.BackColor = System.Drawing.Color.MistyRose;
            this.loginButton.FlatAppearance.BorderSize = 0;
            this.loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginButton.ForeColor = System.Drawing.Color.Black;
            this.loginButton.Location = new System.Drawing.Point(3, 33);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(131, 23);
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
            this.userListButt.Location = new System.Drawing.Point(3, 62);
            this.userListButt.Name = "userListButt";
            this.userListButt.Size = new System.Drawing.Size(131, 23);
            this.userListButt.TabIndex = 27;
            this.userListButt.Text = "Update List";
            this.userListButt.UseVisualStyleBackColor = false;
            this.userListButt.Click += new System.EventHandler(this.UpdateURTButtonClick);
            // 
            // userListReply
            // 
            this.userListReply.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.userListReply.Location = new System.Drawing.Point(3, 149);
            this.userListReply.Name = "userListReply";
            this.userListReply.Size = new System.Drawing.Size(131, 35);
            this.userListReply.TabIndex = 28;
            this.userListReply.Text = "(userListReply)";
            this.userListReply.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // wlStatusDropDown
            // 
            this.wlStatusDropDown.BackColor = System.Drawing.Color.SteelBlue;
            this.wlStatusDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wlStatusDropDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.wlStatusDropDown.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.wlStatusDropDown.FormattingEnabled = true;
            this.wlStatusDropDown.Items.AddRange(new object[] {
            "WL Status",
            "----------",
            "Wishlist Titles",
            "High",
            "Medium",
            "Low",
            "Blacklist"});
            this.wlStatusDropDown.Location = new System.Drawing.Point(335, 338);
            this.wlStatusDropDown.Name = "wlStatusDropDown";
            this.wlStatusDropDown.Size = new System.Drawing.Size(108, 21);
            this.wlStatusDropDown.TabIndex = 93;
            this.wlStatusDropDown.SelectedIndexChanged += new System.EventHandler(this.List_WLStatus);
            // 
            // SearchingAndFilteringButton
            // 
            this.SearchingAndFilteringButton.BackColor = System.Drawing.Color.Khaki;
            this.SearchingAndFilteringButton.FlatAppearance.BorderSize = 0;
            this.SearchingAndFilteringButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SearchingAndFilteringButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SearchingAndFilteringButton.Location = new System.Drawing.Point(560, 309);
            this.SearchingAndFilteringButton.Name = "SearchingAndFilteringButton";
            this.SearchingAndFilteringButton.Size = new System.Drawing.Size(161, 23);
            this.SearchingAndFilteringButton.TabIndex = 91;
            this.SearchingAndFilteringButton.Text = "Searching, Listing and Filtering";
            this.SearchingAndFilteringButton.UseVisualStyleBackColor = false;
            this.SearchingAndFilteringButton.Click += new System.EventHandler(this.Help_SearchingAndFiltering);
            // 
            // BlacklistToggleBox
            // 
            this.BlacklistToggleBox.BackColor = System.Drawing.Color.Navy;
            this.BlacklistToggleBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BlacklistToggleBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BlacklistToggleBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BlacklistToggleBox.FormattingEnabled = true;
            this.BlacklistToggleBox.Items.AddRange(new object[] {
            "Show Blacklisted",
            "Hide Blacklisted",
            "Only Blacklisted"});
            this.BlacklistToggleBox.Location = new System.Drawing.Point(943, 339);
            this.BlacklistToggleBox.Name = "BlacklistToggleBox";
            this.BlacklistToggleBox.Size = new System.Drawing.Size(108, 21);
            this.BlacklistToggleBox.TabIndex = 87;
            this.BlacklistToggleBox.SelectedIndexChanged += new System.EventHandler(this.Filter_Blacklist);
            // 
            // UnreleasedToggleBox
            // 
            this.UnreleasedToggleBox.BackColor = System.Drawing.Color.Navy;
            this.UnreleasedToggleBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UnreleasedToggleBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UnreleasedToggleBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.UnreleasedToggleBox.FormattingEnabled = true;
            this.UnreleasedToggleBox.Items.AddRange(new object[] {
            "Show Unreleased",
            "Hide Unreleased",
            "Only Unreleased",
            "Hide No Release Date"});
            this.UnreleasedToggleBox.Location = new System.Drawing.Point(823, 339);
            this.UnreleasedToggleBox.Name = "UnreleasedToggleBox";
            this.UnreleasedToggleBox.Size = new System.Drawing.Size(114, 21);
            this.UnreleasedToggleBox.TabIndex = 86;
            this.UnreleasedToggleBox.SelectedIndexChanged += new System.EventHandler(this.Filter_Unreleased);
            // 
            // URTToggleBox
            // 
            this.URTToggleBox.BackColor = System.Drawing.Color.Navy;
            this.URTToggleBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.URTToggleBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.URTToggleBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.URTToggleBox.FormattingEnabled = true;
            this.URTToggleBox.Items.AddRange(new object[] {
            "Show URT",
            "Hide URT",
            "Only URT",
            "Only Unplayed"});
            this.URTToggleBox.Location = new System.Drawing.Point(715, 339);
            this.URTToggleBox.Name = "URTToggleBox";
            this.URTToggleBox.Size = new System.Drawing.Size(102, 21);
            this.URTToggleBox.TabIndex = 85;
            this.URTToggleBox.SelectedIndexChanged += new System.EventHandler(this.Filter_URT);
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.Gray;
            this.statusLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.statusLabel.Location = new System.Drawing.Point(6, 334);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(152, 30);
            this.statusLabel.TabIndex = 80;
            this.statusLabel.Text = "(statusLabel)";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.viewPicker.Location = new System.Drawing.Point(1057, 339);
            this.viewPicker.Name = "viewPicker";
            this.viewPicker.Size = new System.Drawing.Size(89, 21);
            this.viewPicker.TabIndex = 77;
            this.viewPicker.SelectedIndexChanged += new System.EventHandler(this.OLVChangeView);
            // 
            // tileOLV
            // 
            this.tileOLV.AllColumns.Add(this.tileColumnTitle);
            this.tileOLV.AllColumns.Add(this.tileColumnDate);
            this.tileOLV.AllColumns.Add(this.tileColumnProducer);
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
            this.tileOLV.MultiSelect = false;
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
            this.tileOLV.CellClick += new System.EventHandler<BrightIdeasSoftware.CellClickEventArgs>(this.VisualNovelLeftClick);
            this.tileOLV.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.ShowContextMenu);
            this.tileOLV.CellToolTipShowing += new System.EventHandler<BrightIdeasSoftware.ToolTipShowingEventArgs>(this.VNToolTip);
            this.tileOLV.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(this.FormatVNCell);
            this.tileOLV.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.FormatVNRow);
            this.tileOLV.ItemsChanged += new System.EventHandler<BrightIdeasSoftware.ItemsChangedEventArgs>(this.objectList_ItemsChanged);
            this.tileOLV.Resize += new System.EventHandler(this.tileOLV_Resize);
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
            // tileColumnULS
            // 
            this.tileColumnULS.AspectName = "ULStatus";
            this.tileColumnULS.Text = "Userlist";
            this.tileColumnULS.Width = 77;
            // 
            // tileColumnULAdded
            // 
            this.tileColumnULAdded.AspectName = "ULAdded";
            this.tileColumnULAdded.Text = "Added to UL";
            this.tileColumnULAdded.Width = 80;
            // 
            // tileColumnULNote
            // 
            this.tileColumnULNote.AspectName = "ULNote";
            this.tileColumnULNote.Text = "Notes";
            // 
            // tileColumnWLS
            // 
            this.tileColumnWLS.AspectName = "WLStatus";
            this.tileColumnWLS.Text = "Wishlist";
            // 
            // tileColumnWLAdded
            // 
            this.tileColumnWLAdded.AspectName = "WLAdded";
            this.tileColumnWLAdded.Text = "Added to WL";
            this.tileColumnWLAdded.Width = 82;
            // 
            // tileColumnVote
            // 
            this.tileColumnVote.AspectName = "Vote";
            this.tileColumnVote.Text = "Vote";
            this.tileColumnVote.ToolTipText = "The score given by you";
            // 
            // tileColumnRating
            // 
            this.tileColumnRating.AspectName = "Rating";
            this.tileColumnRating.Text = "Rating";
            this.tileColumnRating.ToolTipText = "Average of votes by all users";
            // 
            // tileColumnPopularity
            // 
            this.tileColumnPopularity.AspectName = "Popularity";
            this.tileColumnPopularity.Text = "Popularity";
            this.tileColumnPopularity.ToolTipText = "How popular in relation to most popular VN";
            // 
            // tileColumnUpdated
            // 
            this.tileColumnUpdated.AspectName = "UpdatedDate";
            this.tileColumnUpdated.Text = "Updated";
            // 
            // tileColumnID
            // 
            this.tileColumnID.AspectName = "VNID";
            this.tileColumnID.Text = "VNID";
            // 
            // replyText
            // 
            this.replyText.BackColor = System.Drawing.Color.Transparent;
            this.replyText.Location = new System.Drawing.Point(358, 310);
            this.replyText.Name = "replyText";
            this.replyText.Size = new System.Drawing.Size(196, 20);
            this.replyText.TabIndex = 28;
            this.replyText.Text = "(replyText)";
            this.replyText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(6, 308);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 23);
            this.label4.TabIndex = 23;
            this.label4.Text = "List:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ulStatusDropDown
            // 
            this.ulStatusDropDown.BackColor = System.Drawing.Color.SteelBlue;
            this.ulStatusDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ulStatusDropDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ulStatusDropDown.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ulStatusDropDown.FormattingEnabled = true;
            this.ulStatusDropDown.Items.AddRange(new object[] {
            "UL Status",
            "----------",
            "Userlist Titles",
            "Unknown",
            "Playing",
            "Finished",
            "Stalled",
            "Dropped"});
            this.ulStatusDropDown.Location = new System.Drawing.Point(449, 338);
            this.ulStatusDropDown.Name = "ulStatusDropDown";
            this.ulStatusDropDown.Size = new System.Drawing.Size(105, 21);
            this.ulStatusDropDown.TabIndex = 56;
            this.ulStatusDropDown.SelectedIndexChanged += new System.EventHandler(this.List_ULStatus);
            // 
            // quickFilter0
            // 
            this.quickFilter0.BackColor = System.Drawing.Color.SteelBlue;
            this.quickFilter0.FlatAppearance.BorderSize = 0;
            this.quickFilter0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.quickFilter0.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.quickFilter0.Location = new System.Drawing.Point(164, 337);
            this.quickFilter0.Name = "quickFilter0";
            this.quickFilter0.Size = new System.Drawing.Size(54, 23);
            this.quickFilter0.TabIndex = 48;
            this.quickFilter0.Text = "All Titles";
            this.quickFilter0.UseVisualStyleBackColor = false;
            this.quickFilter0.Click += new System.EventHandler(this.List_All);
            // 
            // quickFilter1
            // 
            this.quickFilter1.BackColor = System.Drawing.Color.SteelBlue;
            this.quickFilter1.FlatAppearance.BorderSize = 0;
            this.quickFilter1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.quickFilter1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.quickFilter1.Location = new System.Drawing.Point(224, 337);
            this.quickFilter1.Name = "quickFilter1";
            this.quickFilter1.Size = new System.Drawing.Size(105, 23);
            this.quickFilter1.TabIndex = 47;
            this.quickFilter1.Text = "Favorite Producers";
            this.quickFilter1.UseVisualStyleBackColor = false;
            this.quickFilter1.Click += new System.EventHandler(this.List_FavoriteProducers);
            // 
            // resultLabel
            // 
            this.resultLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resultLabel.BackColor = System.Drawing.Color.Transparent;
            this.resultLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.resultLabel.Location = new System.Drawing.Point(966, 338);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(222, 24);
            this.resultLabel.TabIndex = 43;
            this.resultLabel.Text = "(resultLabel)";
            this.resultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.vnTab);
            this.tabControl1.Controls.Add(this.infoTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1204, 621);
            this.tabControl1.TabIndex = 0;
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
            this.noneToolStripMenuItem.Text = "(None)";
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
            this.noneToolStripMenuItem1.Text = "(None)";
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
            this.toolStripMenuItem11});
            this.voteToolStripMenuItem.Name = "voteToolStripMenuItem";
            this.voteToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.voteToolStripMenuItem.Text = "Vote";
            this.voteToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.RightClickChangeVNStatus);
            // 
            // noneToolStripMenuItem2
            // 
            this.noneToolStripMenuItem2.Name = "noneToolStripMenuItem2";
            this.noneToolStripMenuItem2.Size = new System.Drawing.Size(111, 22);
            this.noneToolStripMenuItem2.Text = "(None)";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(111, 22);
            this.toolStripMenuItem2.Text = "1";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(111, 22);
            this.toolStripMenuItem3.Text = "2";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(111, 22);
            this.toolStripMenuItem4.Text = "3";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(111, 22);
            this.toolStripMenuItem5.Text = "4";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(111, 22);
            this.toolStripMenuItem6.Text = "5";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(111, 22);
            this.toolStripMenuItem7.Text = "6";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(111, 22);
            this.toolStripMenuItem8.Text = "7";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(111, 22);
            this.toolStripMenuItem9.Text = "8";
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(111, 22);
            this.toolStripMenuItem10.Text = "9";
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(111, 22);
            this.toolStripMenuItem11.Text = "10";
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
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(50, 50);
            this.MinimumSize = new System.Drawing.Size(1220, 660);
            this.Name = "FormMain";
            this.Text = "Happy Search";
            this.Load += new System.EventHandler(this.OnLoadRoutines);
            this.infoTab.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.statBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.vnTab.ResumeLayout(false);
            this.vnTab.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tagFilteringBox.ResumeLayout(false);
            this.tagFilteringBox.PerformLayout();
            this.traitFilteringBox.ResumeLayout(false);
            this.traitFilteringBox.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olFavoriteProducers)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tileOLV)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ContextMenuVN.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ToolTip toolTip;
        private TabPage infoTab;
        private GroupBox groupBox9;
        private RichTextBox aboutTextBox;
        private TabPage vnTab;
        internal Label loginReply;
        private Label userListReply;
        private Button userListButt;
        private Label resultLabel;
        private Label label4;
        private TabControl tabControl1;
        private Button quickFilter1;
        private Button quickFilter0;
        private ComboBox ulStatusDropDown;
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
        private Button refreshAllProducersButton;
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
        private GroupBox groupBox3;
        private Label logQueryLabel;
        private Label label1;
        internal RichTextBox serverR;
        private Label logReplyLabel;
        private Button clearLogButton;
        private RichTextBox serverQ;
        private ComboBox URTToggleBox;
        private ComboBox UnreleasedToggleBox;
        private ComboBox BlacklistToggleBox;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem showProducerTitlesToolStripMenuItem;
        private Button favoriteProducersHelpButton;
        private ToolStripMenuItem addProducerToFavoritesToolStripMenuItem;
        private Button GetStartedHelpButton;
        private Button suggestProducersButton;
        private Button SearchingAndFilteringButton;
        private Button TagFilteringHelpButton;
        private Label label6;
        private Button updateFilterResultsButton;
        private TextBox tagSearchBox;
        private Label tagReply;
        private CheckBox checkBox10;
        private TextBox customTagFilterNameBox;
        private CheckBox checkBox7;
        private CheckBox checkBox8;
        private CheckBox checkBox1;
        private Label label7;
        private Button clearFilterButton;
        private CheckBox checkBox9;
        private Button deleteCustomTagFilterButton;
        private ComboBox customTagFilters;
        private CheckBox checkBox2;
        internal CheckBox tagTypeC;
        private Button saveCustomFilterButton;
        private CheckBox checkBox4;
        private CheckBox checkBox3;
        internal CheckBox tagTypeT;
        private Label mctLoadingLabel;
        private CheckBox checkBox5;
        private CheckBox checkBox6;
        internal CheckBox tagTypeS;
        private Label label2;
        private OLVColumn tileColumnRating;
        private OLVColumn tileColumnPopularity;
        private ComboBox wlStatusDropDown;
        private Panel panel1;
        private Button updateTagsAndTraitsButton;
        private Panel panel2;
        private Button listResultsButton;
        private TabControl tabControl2;
        private TabPage tagFilteringBox;
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
        private ComboBox groupListBox;
        private Button tagSignaler;
        private Button traitSignaler;
        private CheckBox ToggleFiltersModeButton;
        private ListBox tagSearchResultBox;
        private Button tagSearchButton;
        private ListBox traitSearchResultBox;
        private Button traitSearchButton;
        private ComboBox ListByCB;
        private TextBox ListByTB;
        private Button ListByUpdateButton;
        private Button ListByGoButton;
    }
}

