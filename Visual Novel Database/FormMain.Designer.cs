using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace Happy_Search
{
    partial class FormMain
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tagTypeC = new System.Windows.Forms.CheckBox();
            this.tagTypeT = new System.Windows.Forms.CheckBox();
            this.tagTypeS = new System.Windows.Forms.CheckBox();
            this.tagTypeS2 = new System.Windows.Forms.CheckBox();
            this.tagTypeT2 = new System.Windows.Forms.CheckBox();
            this.tagTypeC2 = new System.Windows.Forms.CheckBox();
            this.updateProducersButton = new System.Windows.Forms.Button();
            this.autoUpdateURTBox = new System.Windows.Forms.CheckBox();
            this.yearLimitBox = new System.Windows.Forms.CheckBox();
            this.URTToggleBox = new System.Windows.Forms.CheckBox();
            this.langImages = new System.Windows.Forms.ImageList(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.searchBox = new System.Windows.Forms.TextBox();
            this.tagSearchBox = new System.Windows.Forms.TextBox();
            this.ProducerFilterBox = new System.Windows.Forms.TextBox();
            this.filterNameBox = new System.Windows.Forms.TextBox();
            this.infoTab = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
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
            this.updateProducerTitlesButton = new System.Windows.Forms.Button();
            this.UnreleasedToggleBox = new System.Windows.Forms.CheckBox();
            this.BlacklistToggleBox = new System.Windows.Forms.CheckBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.updateCustomFilterButton = new System.Windows.Forms.Button();
            this.deleteCustomFilterButton = new System.Windows.Forms.Button();
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
            this.tileColumnUpdated = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tileColumnID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button6 = new System.Windows.Forms.Button();
            this.prodReply = new System.Windows.Forms.Label();
            this.addProducersButton = new System.Windows.Forms.Button();
            this.olFavoriteProducers = new BrightIdeasSoftware.ObjectListView();
            this.ol2Name = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2ItemCount = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2UserAverageVote = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2UserDropRate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2Loaded = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2Updated = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ol2ID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.loadUnloadedButton = new System.Windows.Forms.Button();
            this.reloadFavoriteProducersButton = new System.Windows.Forms.Button();
            this.selectedProducersVNButton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.yearButton = new System.Windows.Forms.Button();
            this.replyText = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.yearBox = new System.Windows.Forms.TextBox();
            this.btnFetch = new System.Windows.Forms.Button();
            this.customFilters = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.ULStatusDropDown = new System.Windows.Forms.ComboBox();
            this.quickFilter4 = new System.Windows.Forms.Button();
            this.quickFilter0 = new System.Windows.Forms.Button();
            this.quickFilter1 = new System.Windows.Forms.Button();
            this.settingsBox = new System.Windows.Forms.GroupBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.closeAllFormsButton = new System.Windows.Forms.Button();
            this.nsfwToggle = new System.Windows.Forms.CheckBox();
            this.userListReply = new System.Windows.Forms.Label();
            this.userListButt = new System.Windows.Forms.Button();
            this.loginReply = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.tagFilteringBox = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.filterReply = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.mctLoadingLabel = new System.Windows.Forms.Label();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.vnImages = new System.Windows.Forms.ImageList(this.components);
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
            this.infoTab.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.vnTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tileOLV)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olFavoriteProducers)).BeginInit();
            this.settingsBox.SuspendLayout();
            this.tagFilteringBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.ContextMenuVN.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 0;
            this.toolTip1.AutoPopDelay = 0;
            this.toolTip1.InitialDelay = 0;
            this.toolTip1.ReshowDelay = 0;
            this.toolTip1.UseAnimation = false;
            this.toolTip1.UseFading = false;
            // 
            // tagTypeC
            // 
            this.tagTypeC.AutoSize = true;
            this.tagTypeC.Checked = true;
            this.tagTypeC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tagTypeC.Location = new System.Drawing.Point(113, 20);
            this.tagTypeC.Name = "tagTypeC";
            this.tagTypeC.Size = new System.Drawing.Size(33, 17);
            this.tagTypeC.TabIndex = 35;
            this.tagTypeC.Text = "C";
            this.toolTip1.SetToolTip(this.tagTypeC, "Display Content Tags");
            this.tagTypeC.UseVisualStyleBackColor = true;
            this.tagTypeC.Click += new System.EventHandler(this.DisplayCommonTags);
            // 
            // tagTypeT
            // 
            this.tagTypeT.AutoSize = true;
            this.tagTypeT.Checked = true;
            this.tagTypeT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tagTypeT.Location = new System.Drawing.Point(191, 20);
            this.tagTypeT.Name = "tagTypeT";
            this.tagTypeT.Size = new System.Drawing.Size(33, 17);
            this.tagTypeT.TabIndex = 36;
            this.tagTypeT.Text = "T";
            this.toolTip1.SetToolTip(this.tagTypeT, "Display Technical Tags");
            this.tagTypeT.UseVisualStyleBackColor = true;
            this.tagTypeT.Click += new System.EventHandler(this.DisplayCommonTags);
            // 
            // tagTypeS
            // 
            this.tagTypeS.AutoSize = true;
            this.tagTypeS.Checked = true;
            this.tagTypeS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tagTypeS.Location = new System.Drawing.Point(152, 20);
            this.tagTypeS.Name = "tagTypeS";
            this.tagTypeS.Size = new System.Drawing.Size(33, 17);
            this.tagTypeS.TabIndex = 37;
            this.tagTypeS.Text = "S";
            this.toolTip1.SetToolTip(this.tagTypeS, "Display Sexual Content Tags");
            this.tagTypeS.UseVisualStyleBackColor = true;
            this.tagTypeS.Click += new System.EventHandler(this.DisplayCommonTags);
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
            this.toolTip1.SetToolTip(this.tagTypeS2, "Display Sexual Content Tags");
            this.tagTypeS2.UseVisualStyleBackColor = true;
            this.tagTypeS2.Click += new System.EventHandler(this.DisplayCommonTagsULStats);
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
            this.toolTip1.SetToolTip(this.tagTypeT2, "Display Technical Tags");
            this.tagTypeT2.UseVisualStyleBackColor = true;
            this.tagTypeT2.Click += new System.EventHandler(this.DisplayCommonTagsULStats);
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
            this.toolTip1.SetToolTip(this.tagTypeC2, "Display Content Tags");
            this.tagTypeC2.UseVisualStyleBackColor = true;
            this.tagTypeC2.Click += new System.EventHandler(this.DisplayCommonTagsULStats);
            // 
            // updateProducersButton
            // 
            this.updateProducersButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.updateProducersButton.Location = new System.Drawing.Point(279, 289);
            this.updateProducersButton.Name = "updateProducersButton";
            this.updateProducersButton.Size = new System.Drawing.Size(136, 23);
            this.updateProducersButton.TabIndex = 37;
            this.updateProducersButton.Text = "Get New Producer Titles";
            this.toolTip1.SetToolTip(this.updateProducersButton, "Get Producer titles added since last update.");
            this.updateProducersButton.UseVisualStyleBackColor = true;
            this.updateProducersButton.Click += new System.EventHandler(this.GetNewFavoriteProducerTitles);
            // 
            // autoUpdateURTBox
            // 
            this.autoUpdateURTBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.autoUpdateURTBox.Location = new System.Drawing.Point(7, 204);
            this.autoUpdateURTBox.Name = "autoUpdateURTBox";
            this.autoUpdateURTBox.Size = new System.Drawing.Size(126, 17);
            this.autoUpdateURTBox.TabIndex = 83;
            this.autoUpdateURTBox.Text = "Auto-Update URT";
            this.autoUpdateURTBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.autoUpdateURTBox, "Update User Related Titles every 2 days.");
            this.autoUpdateURTBox.UseVisualStyleBackColor = true;
            this.autoUpdateURTBox.Click += new System.EventHandler(this.ToggleAutoUpdateURT);
            // 
            // yearLimitBox
            // 
            this.yearLimitBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.yearLimitBox.Location = new System.Drawing.Point(7, 227);
            this.yearLimitBox.Name = "yearLimitBox";
            this.yearLimitBox.Size = new System.Drawing.Size(126, 17);
            this.yearLimitBox.TabIndex = 84;
            this.yearLimitBox.Text = "10 Year Limit";
            this.yearLimitBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.yearLimitBox, "Limits VNs fetched from VNDB to last 10 years.");
            this.yearLimitBox.UseVisualStyleBackColor = true;
            this.yearLimitBox.Click += new System.EventHandler(this.ToggleLimit10Years);
            // 
            // URTToggleBox
            // 
            this.URTToggleBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.URTToggleBox.BackColor = System.Drawing.Color.Black;
            this.URTToggleBox.Checked = true;
            this.URTToggleBox.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.URTToggleBox.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.URTToggleBox.FlatAppearance.BorderSize = 0;
            this.URTToggleBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.Black;
            this.URTToggleBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.URTToggleBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.URTToggleBox.Location = new System.Drawing.Point(1034, 336);
            this.URTToggleBox.Name = "URTToggleBox";
            this.URTToggleBox.Size = new System.Drawing.Size(68, 23);
            this.URTToggleBox.TabIndex = 83;
            this.URTToggleBox.Text = "Show URT";
            this.URTToggleBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.URTToggleBox.ThreeState = true;
            this.toolTip1.SetToolTip(this.URTToggleBox, "URT - User Related Title");
            this.URTToggleBox.UseVisualStyleBackColor = false;
            this.URTToggleBox.Click += new System.EventHandler(this.URTToggle);
            // 
            // langImages
            // 
            this.langImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("langImages.ImageStream")));
            this.langImages.TransparentColor = System.Drawing.Color.Transparent;
            this.langImages.Images.SetKeyName(0, "af.png");
            this.langImages.Images.SetKeyName(1, "ar.png");
            this.langImages.Images.SetKeyName(2, "be.png");
            this.langImages.Images.SetKeyName(3, "bg.png");
            this.langImages.Images.SetKeyName(4, "bo.png");
            this.langImages.Images.SetKeyName(5, "ca.png");
            this.langImages.Images.SetKeyName(6, "cs.png");
            this.langImages.Images.SetKeyName(7, "da.png");
            this.langImages.Images.SetKeyName(8, "de.png");
            this.langImages.Images.SetKeyName(9, "el.png");
            this.langImages.Images.SetKeyName(10, "en.png");
            this.langImages.Images.SetKeyName(11, "eo.png");
            this.langImages.Images.SetKeyName(12, "es.png");
            this.langImages.Images.SetKeyName(13, "et.png");
            this.langImages.Images.SetKeyName(14, "eu.png");
            this.langImages.Images.SetKeyName(15, "fa.png");
            this.langImages.Images.SetKeyName(16, "fi.png");
            this.langImages.Images.SetKeyName(17, "fil.png");
            this.langImages.Images.SetKeyName(18, "fo.png");
            this.langImages.Images.SetKeyName(19, "fr.png");
            this.langImages.Images.SetKeyName(20, "ga.png");
            this.langImages.Images.SetKeyName(21, "gl.png");
            this.langImages.Images.SetKeyName(22, "he.png");
            this.langImages.Images.SetKeyName(23, "hi.png");
            this.langImages.Images.SetKeyName(24, "hr.png");
            this.langImages.Images.SetKeyName(25, "hu.png");
            this.langImages.Images.SetKeyName(26, "id.png");
            this.langImages.Images.SetKeyName(27, "is.png");
            this.langImages.Images.SetKeyName(28, "it.png");
            this.langImages.Images.SetKeyName(29, "ja.png");
            this.langImages.Images.SetKeyName(30, "km.png");
            this.langImages.Images.SetKeyName(31, "ko.png");
            this.langImages.Images.SetKeyName(32, "lb.png");
            this.langImages.Images.SetKeyName(33, "lt.png");
            this.langImages.Images.SetKeyName(34, "lv.png");
            this.langImages.Images.SetKeyName(35, "mn.png");
            this.langImages.Images.SetKeyName(36, "ms.png");
            this.langImages.Images.SetKeyName(37, "nb.png");
            this.langImages.Images.SetKeyName(38, "nl.png");
            this.langImages.Images.SetKeyName(39, "nn.png");
            this.langImages.Images.SetKeyName(40, "pl.png");
            this.langImages.Images.SetKeyName(41, "pt-br.png");
            this.langImages.Images.SetKeyName(42, "pt-pt.png");
            this.langImages.Images.SetKeyName(43, "ro.png");
            this.langImages.Images.SetKeyName(44, "ru.png");
            this.langImages.Images.SetKeyName(45, "sco.png");
            this.langImages.Images.SetKeyName(46, "se.png");
            this.langImages.Images.SetKeyName(47, "sk.png");
            this.langImages.Images.SetKeyName(48, "sl.png");
            this.langImages.Images.SetKeyName(49, "so.png");
            this.langImages.Images.SetKeyName(50, "sq.png");
            this.langImages.Images.SetKeyName(51, "sr.png");
            this.langImages.Images.SetKeyName(52, "sv.png");
            this.langImages.Images.SetKeyName(53, "tg.png");
            this.langImages.Images.SetKeyName(54, "th.png");
            this.langImages.Images.SetKeyName(55, "tl.png");
            this.langImages.Images.SetKeyName(56, "tr.png");
            this.langImages.Images.SetKeyName(57, "uk.png");
            this.langImages.Images.SetKeyName(58, "vi.png");
            this.langImages.Images.SetKeyName(59, "zh-hans.png");
            this.langImages.Images.SetKeyName(60, "zh-hant.png");
            // 
            // searchBox
            // 
            this.searchBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.searchBox.Location = new System.Drawing.Point(93, 311);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(92, 20);
            this.searchBox.TabIndex = 21;
            this.toolTip3.SetToolTip(this.searchBox, "Type VN name here.");
            this.searchBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.searchButton_keyPress);
            // 
            // tagSearchBox
            // 
            this.tagSearchBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tagSearchBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tagSearchBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagSearchBox.Location = new System.Drawing.Point(264, 17);
            this.tagSearchBox.Name = "tagSearchBox";
            this.tagSearchBox.Size = new System.Drawing.Size(187, 22);
            this.tagSearchBox.TabIndex = 29;
            this.toolTip3.SetToolTip(this.tagSearchBox, "Type VN name here.");
            this.tagSearchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tagSearchBox_KeyDown);
            // 
            // ProducerFilterBox
            // 
            this.ProducerFilterBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ProducerFilterBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.ProducerFilterBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProducerFilterBox.Location = new System.Drawing.Point(746, 336);
            this.ProducerFilterBox.Name = "ProducerFilterBox";
            this.ProducerFilterBox.Size = new System.Drawing.Size(145, 22);
            this.ProducerFilterBox.TabIndex = 39;
            this.toolTip3.SetToolTip(this.ProducerFilterBox, "Type VN name here.");
            this.ProducerFilterBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Filter_Producer);
            // 
            // filterNameBox
            // 
            this.filterNameBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterNameBox.Location = new System.Drawing.Point(461, 45);
            this.filterNameBox.Name = "filterNameBox";
            this.filterNameBox.Size = new System.Drawing.Size(100, 22);
            this.filterNameBox.TabIndex = 41;
            this.toolTip3.SetToolTip(this.filterNameBox, "Type VN name here.");
            this.filterNameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.filterNameBox_KeyDown);
            // 
            // infoTab
            // 
            this.infoTab.BackColor = System.Drawing.SystemColors.Control;
            this.infoTab.BackgroundImage = global::Happy_Search.Properties.Resources._2013_06_Dark_Black_Wallpaper_Background_Dekstop;
            this.infoTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.infoTab.Controls.Add(this.groupBox3);
            this.infoTab.Controls.Add(this.statBox);
            this.infoTab.Controls.Add(this.groupBox1);
            this.infoTab.Controls.Add(this.groupBox9);
            this.infoTab.Location = new System.Drawing.Point(4, 22);
            this.infoTab.Name = "infoTab";
            this.infoTab.Padding = new System.Windows.Forms.Padding(3);
            this.infoTab.Size = new System.Drawing.Size(1410, 757);
            this.infoTab.TabIndex = 2;
            this.infoTab.Text = "Information";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
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
            // serverR
            // 
            this.serverR.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverR.BackColor = System.Drawing.SystemColors.InfoText;
            this.serverR.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverR.ForeColor = System.Drawing.SystemColors.Info;
            this.serverR.Location = new System.Drawing.Point(344, 32);
            this.serverR.Name = "serverR";
            this.serverR.Size = new System.Drawing.Size(1044, 522);
            this.serverR.TabIndex = 39;
            this.serverR.Text = "";
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
            this.clearLogButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.clearLogButton.Location = new System.Drawing.Point(6, 531);
            this.clearLogButton.Name = "clearLogButton";
            this.clearLogButton.Size = new System.Drawing.Size(100, 23);
            this.clearLogButton.TabIndex = 37;
            this.clearLogButton.Text = "Clear";
            this.clearLogButton.UseVisualStyleBackColor = true;
            this.clearLogButton.Click += new System.EventHandler(this.ClearLog);
            // 
            // serverQ
            // 
            this.serverQ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.serverQ.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.serverQ.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverQ.ForeColor = System.Drawing.SystemColors.Info;
            this.serverQ.Location = new System.Drawing.Point(4, 115);
            this.serverQ.Name = "serverQ";
            this.serverQ.Size = new System.Drawing.Size(334, 410);
            this.serverQ.TabIndex = 36;
            this.serverQ.Text = "";
            // 
            // logQueryLabel
            // 
            this.logQueryLabel.AutoSize = true;
            this.logQueryLabel.Location = new System.Drawing.Point(6, 99);
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
            this.questionBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.questionBox.ForeColor = System.Drawing.SystemColors.InfoText;
            this.questionBox.Location = new System.Drawing.Point(4, 32);
            this.questionBox.Name = "questionBox";
            this.questionBox.Size = new System.Drawing.Size(334, 64);
            this.questionBox.TabIndex = 33;
            this.questionBox.Text = "";
            this.questionBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LogQuestion);
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
            this.aboutTextBox.Text = resources.GetString("aboutTextBox.Text");
            // 
            // vnTab
            // 
            this.vnTab.BackColor = System.Drawing.Color.Gray;
            this.vnTab.BackgroundImage = global::Happy_Search.Properties.Resources._2013_06_Dark_Black_Wallpaper_Background_Dekstop;
            this.vnTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.vnTab.Controls.Add(this.updateProducerTitlesButton);
            this.vnTab.Controls.Add(this.URTToggleBox);
            this.vnTab.Controls.Add(this.UnreleasedToggleBox);
            this.vnTab.Controls.Add(this.BlacklistToggleBox);
            this.vnTab.Controls.Add(this.statusLabel);
            this.vnTab.Controls.Add(this.updateCustomFilterButton);
            this.vnTab.Controls.Add(this.deleteCustomFilterButton);
            this.vnTab.Controls.Add(this.viewPicker);
            this.vnTab.Controls.Add(this.tileOLV);
            this.vnTab.Controls.Add(this.groupBox2);
            this.vnTab.Controls.Add(this.yearButton);
            this.vnTab.Controls.Add(this.replyText);
            this.vnTab.Controls.Add(this.label15);
            this.vnTab.Controls.Add(this.yearBox);
            this.vnTab.Controls.Add(this.btnFetch);
            this.vnTab.Controls.Add(this.customFilters);
            this.vnTab.Controls.Add(this.searchBox);
            this.vnTab.Controls.Add(this.ProducerFilterBox);
            this.vnTab.Controls.Add(this.label4);
            this.vnTab.Controls.Add(this.label14);
            this.vnTab.Controls.Add(this.ULStatusDropDown);
            this.vnTab.Controls.Add(this.quickFilter4);
            this.vnTab.Controls.Add(this.quickFilter0);
            this.vnTab.Controls.Add(this.quickFilter1);
            this.vnTab.Controls.Add(this.settingsBox);
            this.vnTab.Controls.Add(this.resultLabel);
            this.vnTab.Controls.Add(this.tagFilteringBox);
            this.vnTab.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.vnTab.Location = new System.Drawing.Point(4, 22);
            this.vnTab.Name = "vnTab";
            this.vnTab.Padding = new System.Windows.Forms.Padding(3);
            this.vnTab.Size = new System.Drawing.Size(1410, 757);
            this.vnTab.TabIndex = 1;
            this.vnTab.Text = "Visual Novel Info";
            // 
            // updateProducerTitlesButton
            // 
            this.updateProducerTitlesButton.BackColor = System.Drawing.Color.LightCoral;
            this.updateProducerTitlesButton.FlatAppearance.BorderSize = 0;
            this.updateProducerTitlesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateProducerTitlesButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.updateProducerTitlesButton.Location = new System.Drawing.Point(897, 336);
            this.updateProducerTitlesButton.Name = "updateProducerTitlesButton";
            this.updateProducerTitlesButton.Size = new System.Drawing.Size(53, 23);
            this.updateProducerTitlesButton.TabIndex = 84;
            this.updateProducerTitlesButton.Text = "Update";
            this.updateProducerTitlesButton.UseVisualStyleBackColor = false;
            this.updateProducerTitlesButton.Click += new System.EventHandler(this.UpdateProducerTitles);
            // 
            // UnreleasedToggleBox
            // 
            this.UnreleasedToggleBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.UnreleasedToggleBox.BackColor = System.Drawing.Color.Black;
            this.UnreleasedToggleBox.Checked = true;
            this.UnreleasedToggleBox.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.UnreleasedToggleBox.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.UnreleasedToggleBox.FlatAppearance.BorderSize = 0;
            this.UnreleasedToggleBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.Black;
            this.UnreleasedToggleBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UnreleasedToggleBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.UnreleasedToggleBox.Location = new System.Drawing.Point(1107, 336);
            this.UnreleasedToggleBox.Name = "UnreleasedToggleBox";
            this.UnreleasedToggleBox.Size = new System.Drawing.Size(99, 23);
            this.UnreleasedToggleBox.TabIndex = 82;
            this.UnreleasedToggleBox.Text = "Show Unreleased";
            this.UnreleasedToggleBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.UnreleasedToggleBox.ThreeState = true;
            this.UnreleasedToggleBox.UseVisualStyleBackColor = false;
            this.UnreleasedToggleBox.Click += new System.EventHandler(this.UnreleasedToggle);
            // 
            // BlacklistToggleBox
            // 
            this.BlacklistToggleBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.BlacklistToggleBox.BackColor = System.Drawing.Color.Black;
            this.BlacklistToggleBox.Checked = true;
            this.BlacklistToggleBox.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.BlacklistToggleBox.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BlacklistToggleBox.FlatAppearance.BorderSize = 0;
            this.BlacklistToggleBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.Black;
            this.BlacklistToggleBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BlacklistToggleBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BlacklistToggleBox.Location = new System.Drawing.Point(1212, 336);
            this.BlacklistToggleBox.Name = "BlacklistToggleBox";
            this.BlacklistToggleBox.Size = new System.Drawing.Size(97, 23);
            this.BlacklistToggleBox.TabIndex = 81;
            this.BlacklistToggleBox.Text = "Show Blacklisted";
            this.BlacklistToggleBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BlacklistToggleBox.ThreeState = true;
            this.BlacklistToggleBox.UseVisualStyleBackColor = false;
            this.BlacklistToggleBox.Click += new System.EventHandler(this.BlacklistToggle);
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusLabel.BackColor = System.Drawing.Color.Gray;
            this.statusLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.statusLabel.Location = new System.Drawing.Point(6, 739);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(152, 13);
            this.statusLabel.TabIndex = 80;
            this.statusLabel.Text = "(statusLabel)";
            // 
            // updateCustomFilterButton
            // 
            this.updateCustomFilterButton.BackColor = System.Drawing.Color.LightCoral;
            this.updateCustomFilterButton.Enabled = false;
            this.updateCustomFilterButton.FlatAppearance.BorderSize = 0;
            this.updateCustomFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateCustomFilterButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.updateCustomFilterButton.Location = new System.Drawing.Point(631, 336);
            this.updateCustomFilterButton.Name = "updateCustomFilterButton";
            this.updateCustomFilterButton.Size = new System.Drawing.Size(53, 23);
            this.updateCustomFilterButton.TabIndex = 79;
            this.updateCustomFilterButton.Text = "Update";
            this.updateCustomFilterButton.UseVisualStyleBackColor = false;
            this.updateCustomFilterButton.Click += new System.EventHandler(this.UpdateCustomFilter);
            // 
            // deleteCustomFilterButton
            // 
            this.deleteCustomFilterButton.BackColor = System.Drawing.Color.LightCoral;
            this.deleteCustomFilterButton.Enabled = false;
            this.deleteCustomFilterButton.FlatAppearance.BorderSize = 0;
            this.deleteCustomFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteCustomFilterButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.deleteCustomFilterButton.Location = new System.Drawing.Point(574, 336);
            this.deleteCustomFilterButton.Name = "deleteCustomFilterButton";
            this.deleteCustomFilterButton.Size = new System.Drawing.Size(53, 23);
            this.deleteCustomFilterButton.TabIndex = 78;
            this.deleteCustomFilterButton.Text = "Delete";
            this.deleteCustomFilterButton.UseVisualStyleBackColor = false;
            this.deleteCustomFilterButton.Click += new System.EventHandler(this.DeleteCustomFilter);
            // 
            // viewPicker
            // 
            this.viewPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.viewPicker.FormattingEnabled = true;
            this.viewPicker.Items.AddRange(new object[] {
            "Tile",
            "Details"});
            this.viewPicker.Location = new System.Drawing.Point(1315, 336);
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
            this.tileOLV.Size = new System.Drawing.Size(1398, 371);
            this.tileOLV.TabIndex = 63;
            this.tileOLV.TileSize = new System.Drawing.Size(230, 375);
            this.tileOLV.UseAlternatingBackColors = true;
            this.tileOLV.UseCompatibleStateImageBehavior = false;
            this.tileOLV.UseFiltering = true;
            this.tileOLV.View = System.Windows.Forms.View.Tile;
            this.tileOLV.CellClick += new System.EventHandler<BrightIdeasSoftware.CellClickEventArgs>(this.ObjectList_SelectedIndexChanged);
            this.tileOLV.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.ShowContextMenu);
            this.tileOLV.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.FormatRow);
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
            this.tileColumnDate.AspectName = "RelDate";
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
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.button6);
            this.groupBox2.Controls.Add(this.updateProducersButton);
            this.groupBox2.Controls.Add(this.prodReply);
            this.groupBox2.Controls.Add(this.addProducersButton);
            this.groupBox2.Controls.Add(this.olFavoriteProducers);
            this.groupBox2.Controls.Add(this.loadUnloadedButton);
            this.groupBox2.Controls.Add(this.reloadFavoriteProducersButton);
            this.groupBox2.Controls.Add(this.selectedProducersVNButton);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox2.Location = new System.Drawing.Point(724, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(680, 318);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Favorite Producers";
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.MistyRose;
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button6.Location = new System.Drawing.Point(112, 260);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(104, 23);
            this.button6.TabIndex = 47;
            this.button6.Text = "Test";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.Test);
            // 
            // prodReply
            // 
            this.prodReply.Location = new System.Drawing.Point(6, 289);
            this.prodReply.Name = "prodReply";
            this.prodReply.Size = new System.Drawing.Size(267, 23);
            this.prodReply.TabIndex = 32;
            this.prodReply.Text = "(prodReply)";
            this.prodReply.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // addProducersButton
            // 
            this.addProducersButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.addProducersButton.Location = new System.Drawing.Point(6, 260);
            this.addProducersButton.Name = "addProducersButton";
            this.addProducersButton.Size = new System.Drawing.Size(100, 23);
            this.addProducersButton.TabIndex = 36;
            this.addProducersButton.Text = "Add Producers";
            this.addProducersButton.UseVisualStyleBackColor = true;
            this.addProducersButton.Click += new System.EventHandler(this.AddProducers);
            // 
            // olFavoriteProducers
            // 
            this.olFavoriteProducers.AllColumns.Add(this.ol2Name);
            this.olFavoriteProducers.AllColumns.Add(this.ol2ItemCount);
            this.olFavoriteProducers.AllColumns.Add(this.ol2UserAverageVote);
            this.olFavoriteProducers.AllColumns.Add(this.ol2UserDropRate);
            this.olFavoriteProducers.AllColumns.Add(this.ol2Loaded);
            this.olFavoriteProducers.AllColumns.Add(this.ol2Updated);
            this.olFavoriteProducers.AllColumns.Add(this.ol2ID);
            this.olFavoriteProducers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olFavoriteProducers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ol2Name,
            this.ol2ItemCount,
            this.ol2UserAverageVote,
            this.ol2UserDropRate,
            this.ol2Loaded,
            this.ol2Updated,
            this.ol2ID});
            this.olFavoriteProducers.FullRowSelect = true;
            this.olFavoriteProducers.GridLines = true;
            this.olFavoriteProducers.Location = new System.Drawing.Point(6, 21);
            this.olFavoriteProducers.Name = "olFavoriteProducers";
            this.olFavoriteProducers.ShowGroups = false;
            this.olFavoriteProducers.Size = new System.Drawing.Size(668, 233);
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
            this.ol2Name.Width = 306;
            // 
            // ol2ItemCount
            // 
            this.ol2ItemCount.AspectName = "NumberOfTitles";
            this.ol2ItemCount.Text = "Titles";
            this.ol2ItemCount.Width = 38;
            // 
            // ol2UserAverageVote
            // 
            this.ol2UserAverageVote.AspectName = "UserAverageVote";
            this.ol2UserAverageVote.AspectToStringFormat = "";
            this.ol2UserAverageVote.Text = "Avg Vote";
            this.ol2UserAverageVote.ToolTipText = "Based on your votes.";
            this.ol2UserAverageVote.Width = 57;
            // 
            // ol2UserDropRate
            // 
            this.ol2UserDropRate.AspectName = "UserDropRate";
            this.ol2UserDropRate.AspectToStringFormat = "{0}%";
            this.ol2UserDropRate.Text = "Drop Rate";
            this.ol2UserDropRate.ToolTipText = "Your rate of dropped vs finished titles.";
            this.ol2UserDropRate.Width = 63;
            // 
            // ol2Loaded
            // 
            this.ol2Loaded.AspectName = "Loaded";
            this.ol2Loaded.Text = "Loaded?";
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
            // loadUnloadedButton
            // 
            this.loadUnloadedButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.loadUnloadedButton.Location = new System.Drawing.Point(573, 289);
            this.loadUnloadedButton.Name = "loadUnloadedButton";
            this.loadUnloadedButton.Size = new System.Drawing.Size(101, 23);
            this.loadUnloadedButton.TabIndex = 35;
            this.loadUnloadedButton.Text = "Load Unloaded";
            this.loadUnloadedButton.UseVisualStyleBackColor = true;
            this.loadUnloadedButton.Click += new System.EventHandler(this.LoadUnloaded);
            // 
            // reloadFavoriteProducersButton
            // 
            this.reloadFavoriteProducersButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.reloadFavoriteProducersButton.Location = new System.Drawing.Point(421, 289);
            this.reloadFavoriteProducersButton.Name = "reloadFavoriteProducersButton";
            this.reloadFavoriteProducersButton.Size = new System.Drawing.Size(146, 23);
            this.reloadFavoriteProducersButton.TabIndex = 32;
            this.reloadFavoriteProducersButton.Text = "Update All Producer Titles";
            this.reloadFavoriteProducersButton.UseVisualStyleBackColor = true;
            this.reloadFavoriteProducersButton.Click += new System.EventHandler(this.UpdateAllFavoriteProducerTitles);
            // 
            // selectedProducersVNButton
            // 
            this.selectedProducersVNButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.selectedProducersVNButton.Location = new System.Drawing.Point(421, 260);
            this.selectedProducersVNButton.Name = "selectedProducersVNButton";
            this.selectedProducersVNButton.Size = new System.Drawing.Size(146, 23);
            this.selectedProducersVNButton.TabIndex = 33;
            this.selectedProducersVNButton.Text = "Show VNs From Selected";
            this.selectedProducersVNButton.UseVisualStyleBackColor = true;
            this.selectedProducersVNButton.Click += new System.EventHandler(this.ShowSelectedProducerVNs);
            // 
            // button3
            // 
            this.button3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button3.Location = new System.Drawing.Point(573, 260);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(101, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "Remove Selected";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.RemoveProducers);
            // 
            // yearButton
            // 
            this.yearButton.BackColor = System.Drawing.Color.MistyRose;
            this.yearButton.FlatAppearance.BorderSize = 0;
            this.yearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.yearButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.yearButton.Location = new System.Drawing.Point(378, 311);
            this.yearButton.Name = "yearButton";
            this.yearButton.Size = new System.Drawing.Size(29, 20);
            this.yearButton.TabIndex = 46;
            this.yearButton.Text = "Go";
            this.yearButton.UseVisualStyleBackColor = false;
            this.yearButton.Click += new System.EventHandler(this.GetYearTitles);
            // 
            // replyText
            // 
            this.replyText.BackColor = System.Drawing.Color.Transparent;
            this.replyText.Location = new System.Drawing.Point(413, 311);
            this.replyText.Name = "replyText";
            this.replyText.Size = new System.Drawing.Size(305, 20);
            this.replyText.TabIndex = 28;
            this.replyText.Text = "(replyText)";
            this.replyText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Location = new System.Drawing.Point(226, 314);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(102, 13);
            this.label15.TabIndex = 47;
            this.label15.Text = "Load VNs from Year";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // yearBox
            // 
            this.yearBox.Location = new System.Drawing.Point(334, 311);
            this.yearBox.Name = "yearBox";
            this.yearBox.Size = new System.Drawing.Size(38, 20);
            this.yearBox.TabIndex = 36;
            this.yearBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.yearBox_KeyDown);
            // 
            // btnFetch
            // 
            this.btnFetch.BackColor = System.Drawing.Color.MistyRose;
            this.btnFetch.FlatAppearance.BorderSize = 0;
            this.btnFetch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFetch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnFetch.Location = new System.Drawing.Point(191, 311);
            this.btnFetch.Name = "btnFetch";
            this.btnFetch.Size = new System.Drawing.Size(29, 20);
            this.btnFetch.TabIndex = 27;
            this.btnFetch.Text = "Go";
            this.btnFetch.UseVisualStyleBackColor = false;
            this.btnFetch.Click += new System.EventHandler(this.VNSearch);
            // 
            // customFilters
            // 
            this.customFilters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.customFilters.ForeColor = System.Drawing.SystemColors.ControlText;
            this.customFilters.FormattingEnabled = true;
            this.customFilters.Items.AddRange(new object[] {
            "Custom Filters",
            "----------"});
            this.customFilters.Location = new System.Drawing.Point(447, 336);
            this.customFilters.Name = "customFilters";
            this.customFilters.Size = new System.Drawing.Size(121, 21);
            this.customFilters.TabIndex = 57;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(10, 314);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Search For VN";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label14.Location = new System.Drawing.Point(690, 341);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(50, 13);
            this.label14.TabIndex = 38;
            this.label14.Text = "Producer";
            // 
            // ULStatusDropDown
            // 
            this.ULStatusDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ULStatusDropDown.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ULStatusDropDown.FormattingEnabled = true;
            this.ULStatusDropDown.Items.AddRange(new object[] {
            "UL Status",
            "----------",
            "Unknown",
            "Playing",
            "Finished",
            "Dropped"});
            this.ULStatusDropDown.Location = new System.Drawing.Point(320, 336);
            this.ULStatusDropDown.Name = "ULStatusDropDown";
            this.ULStatusDropDown.Size = new System.Drawing.Size(121, 21);
            this.ULStatusDropDown.TabIndex = 56;
            // 
            // quickFilter4
            // 
            this.quickFilter4.BackColor = System.Drawing.Color.SteelBlue;
            this.quickFilter4.FlatAppearance.BorderSize = 0;
            this.quickFilter4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.quickFilter4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.quickFilter4.Location = new System.Drawing.Point(203, 336);
            this.quickFilter4.Name = "quickFilter4";
            this.quickFilter4.Size = new System.Drawing.Size(111, 23);
            this.quickFilter4.TabIndex = 51;
            this.quickFilter4.Text = "Wishlist Titles";
            this.quickFilter4.UseVisualStyleBackColor = false;
            this.quickFilter4.Click += new System.EventHandler(this.Filter_Wishlist);
            // 
            // quickFilter0
            // 
            this.quickFilter0.BackColor = System.Drawing.Color.SteelBlue;
            this.quickFilter0.FlatAppearance.BorderSize = 0;
            this.quickFilter0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.quickFilter0.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.quickFilter0.Location = new System.Drawing.Point(8, 336);
            this.quickFilter0.Name = "quickFilter0";
            this.quickFilter0.Size = new System.Drawing.Size(75, 23);
            this.quickFilter0.TabIndex = 48;
            this.quickFilter0.Text = "All Titles";
            this.quickFilter0.UseVisualStyleBackColor = false;
            this.quickFilter0.Click += new System.EventHandler(this.Filter_All);
            // 
            // quickFilter1
            // 
            this.quickFilter1.BackColor = System.Drawing.Color.SteelBlue;
            this.quickFilter1.FlatAppearance.BorderSize = 0;
            this.quickFilter1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.quickFilter1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.quickFilter1.Location = new System.Drawing.Point(89, 336);
            this.quickFilter1.Name = "quickFilter1";
            this.quickFilter1.Size = new System.Drawing.Size(110, 23);
            this.quickFilter1.TabIndex = 47;
            this.quickFilter1.Text = "Favorite Producers";
            this.quickFilter1.UseVisualStyleBackColor = false;
            this.quickFilter1.Click += new System.EventHandler(this.Filter_FavoriteProducers);
            // 
            // settingsBox
            // 
            this.settingsBox.BackColor = System.Drawing.Color.Transparent;
            this.settingsBox.Controls.Add(this.yearLimitBox);
            this.settingsBox.Controls.Add(this.autoUpdateURTBox);
            this.settingsBox.Controls.Add(this.loginButton);
            this.settingsBox.Controls.Add(this.closeAllFormsButton);
            this.settingsBox.Controls.Add(this.nsfwToggle);
            this.settingsBox.Controls.Add(this.userListReply);
            this.settingsBox.Controls.Add(this.userListButt);
            this.settingsBox.Controls.Add(this.loginReply);
            this.settingsBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.settingsBox.Location = new System.Drawing.Point(6, 6);
            this.settingsBox.Name = "settingsBox";
            this.settingsBox.Size = new System.Drawing.Size(139, 298);
            this.settingsBox.TabIndex = 29;
            this.settingsBox.TabStop = false;
            this.settingsBox.Text = "Settings";
            // 
            // loginButton
            // 
            this.loginButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.loginButton.Location = new System.Drawing.Point(7, 46);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(126, 23);
            this.loginButton.TabIndex = 82;
            this.loginButton.Text = "Log In";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.LogInDialog);
            // 
            // closeAllFormsButton
            // 
            this.closeAllFormsButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.closeAllFormsButton.Location = new System.Drawing.Point(7, 269);
            this.closeAllFormsButton.Name = "closeAllFormsButton";
            this.closeAllFormsButton.Size = new System.Drawing.Size(126, 23);
            this.closeAllFormsButton.TabIndex = 81;
            this.closeAllFormsButton.Text = "Close All VN Windows";
            this.closeAllFormsButton.UseVisualStyleBackColor = true;
            this.closeAllFormsButton.Click += new System.EventHandler(this.CloseAllForms);
            // 
            // nsfwToggle
            // 
            this.nsfwToggle.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.nsfwToggle.Location = new System.Drawing.Point(7, 181);
            this.nsfwToggle.Name = "nsfwToggle";
            this.nsfwToggle.Size = new System.Drawing.Size(126, 17);
            this.nsfwToggle.TabIndex = 80;
            this.nsfwToggle.Text = "Show NSFW Images";
            this.nsfwToggle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.nsfwToggle.UseVisualStyleBackColor = true;
            this.nsfwToggle.Click += new System.EventHandler(this.ToggleNSFWImages);
            // 
            // userListReply
            // 
            this.userListReply.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.userListReply.Location = new System.Drawing.Point(7, 101);
            this.userListReply.Name = "userListReply";
            this.userListReply.Size = new System.Drawing.Size(126, 41);
            this.userListReply.TabIndex = 28;
            this.userListReply.Text = "(userListReply)";
            this.userListReply.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // userListButt
            // 
            this.userListButt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.userListButt.Location = new System.Drawing.Point(7, 75);
            this.userListButt.Name = "userListButt";
            this.userListButt.Size = new System.Drawing.Size(126, 23);
            this.userListButt.TabIndex = 27;
            this.userListButt.Text = "Update List";
            this.userListButt.UseVisualStyleBackColor = true;
            this.userListButt.Click += new System.EventHandler(this.UpdateURTButtonClick);
            // 
            // loginReply
            // 
            this.loginReply.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.loginReply.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.loginReply.Location = new System.Drawing.Point(6, 16);
            this.loginReply.Name = "loginReply";
            this.loginReply.Size = new System.Drawing.Size(127, 27);
            this.loginReply.TabIndex = 30;
            this.loginReply.Text = "(loginReply)";
            this.loginReply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // resultLabel
            // 
            this.resultLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resultLabel.BackColor = System.Drawing.Color.Transparent;
            this.resultLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.resultLabel.Location = new System.Drawing.Point(1250, 739);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(152, 13);
            this.resultLabel.TabIndex = 43;
            this.resultLabel.Text = "(resultLabel)";
            this.resultLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tagFilteringBox
            // 
            this.tagFilteringBox.BackColor = System.Drawing.Color.Transparent;
            this.tagFilteringBox.Controls.Add(this.button1);
            this.tagFilteringBox.Controls.Add(this.filterReply);
            this.tagFilteringBox.Controls.Add(this.filterNameBox);
            this.tagFilteringBox.Controls.Add(this.label6);
            this.tagFilteringBox.Controls.Add(this.checkBox1);
            this.tagFilteringBox.Controls.Add(this.button5);
            this.tagFilteringBox.Controls.Add(this.checkBox2);
            this.tagFilteringBox.Controls.Add(this.button2);
            this.tagFilteringBox.Controls.Add(this.checkBox3);
            this.tagFilteringBox.Controls.Add(this.mctLoadingLabel);
            this.tagFilteringBox.Controls.Add(this.checkBox6);
            this.tagFilteringBox.Controls.Add(this.tagTypeS);
            this.tagFilteringBox.Controls.Add(this.checkBox5);
            this.tagFilteringBox.Controls.Add(this.tagTypeT);
            this.tagFilteringBox.Controls.Add(this.checkBox4);
            this.tagFilteringBox.Controls.Add(this.tagTypeC);
            this.tagFilteringBox.Controls.Add(this.checkBox9);
            this.tagFilteringBox.Controls.Add(this.label7);
            this.tagFilteringBox.Controls.Add(this.checkBox8);
            this.tagFilteringBox.Controls.Add(this.checkBox7);
            this.tagFilteringBox.Controls.Add(this.checkBox10);
            this.tagFilteringBox.Controls.Add(this.tagSearchBox);
            this.tagFilteringBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.tagFilteringBox.Location = new System.Drawing.Point(151, 6);
            this.tagFilteringBox.Name = "tagFilteringBox";
            this.tagFilteringBox.Size = new System.Drawing.Size(567, 299);
            this.tagFilteringBox.TabIndex = 61;
            this.tagFilteringBox.TabStop = false;
            this.tagFilteringBox.Text = "Tag Filtering";
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Location = new System.Drawing.Point(461, 102);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 50;
            this.button1.Text = "Update Results";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.UpdateResults);
            // 
            // filterReply
            // 
            this.filterReply.Location = new System.Drawing.Point(461, 137);
            this.filterReply.Name = "filterReply";
            this.filterReply.Size = new System.Drawing.Size(100, 155);
            this.filterReply.TabIndex = 49;
            this.filterReply.Text = "(filterReply)";
            this.filterReply.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Most Common Tags";
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(6, 46);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(200, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // button5
            // 
            this.button5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button5.Location = new System.Drawing.Point(461, 73);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(100, 23);
            this.button5.TabIndex = 40;
            this.button5.Text = "Clear Filter";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.ClearFilter);
            // 
            // checkBox2
            // 
            this.checkBox2.Location = new System.Drawing.Point(6, 68);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(200, 17);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // button2
            // 
            this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button2.Location = new System.Drawing.Point(461, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 23);
            this.button2.TabIndex = 39;
            this.button2.Text = "Save Filter";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.SaveCustomFilter);
            // 
            // checkBox3
            // 
            this.checkBox3.Location = new System.Drawing.Point(6, 91);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(200, 17);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Text = "checkBox3";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // mctLoadingLabel
            // 
            this.mctLoadingLabel.AutoSize = true;
            this.mctLoadingLabel.Location = new System.Drawing.Point(6, 270);
            this.mctLoadingLabel.Name = "mctLoadingLabel";
            this.mctLoadingLabel.Size = new System.Drawing.Size(94, 13);
            this.mctLoadingLabel.TabIndex = 38;
            this.mctLoadingLabel.Text = "(mctLoadingLabel)";
            // 
            // checkBox6
            // 
            this.checkBox6.Location = new System.Drawing.Point(6, 158);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(200, 17);
            this.checkBox6.TabIndex = 3;
            this.checkBox6.Text = "checkBox6";
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // checkBox5
            // 
            this.checkBox5.Location = new System.Drawing.Point(6, 135);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(200, 17);
            this.checkBox5.TabIndex = 4;
            this.checkBox5.Text = "checkBox5";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // checkBox4
            // 
            this.checkBox4.Location = new System.Drawing.Point(6, 112);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(200, 17);
            this.checkBox4.TabIndex = 5;
            this.checkBox4.Text = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // checkBox9
            // 
            this.checkBox9.Location = new System.Drawing.Point(6, 227);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(200, 17);
            this.checkBox9.TabIndex = 6;
            this.checkBox9.Text = "checkBox9";
            this.checkBox9.UseVisualStyleBackColor = true;
            this.checkBox9.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(224, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Filters";
            // 
            // checkBox8
            // 
            this.checkBox8.Location = new System.Drawing.Point(6, 204);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(200, 17);
            this.checkBox8.TabIndex = 7;
            this.checkBox8.Text = "checkBox8";
            this.checkBox8.UseVisualStyleBackColor = true;
            this.checkBox8.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // checkBox7
            // 
            this.checkBox7.Location = new System.Drawing.Point(6, 181);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(200, 17);
            this.checkBox7.TabIndex = 8;
            this.checkBox7.Text = "checkBox7";
            this.checkBox7.UseVisualStyleBackColor = true;
            this.checkBox7.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
            // 
            // checkBox10
            // 
            this.checkBox10.Location = new System.Drawing.Point(6, 250);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(200, 17);
            this.checkBox10.TabIndex = 9;
            this.checkBox10.Text = "checkBox10";
            this.checkBox10.UseVisualStyleBackColor = true;
            this.checkBox10.CheckedChanged += new System.EventHandler(this.TagFilterAdded);
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
            this.tabControl1.Size = new System.Drawing.Size(1418, 783);
            this.tabControl1.TabIndex = 0;
            // 
            // vnImages
            // 
            this.vnImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.vnImages.ImageSize = new System.Drawing.Size(72, 128);
            this.vnImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ContextMenuVN
            // 
            this.ContextMenuVN.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userlistToolStripMenuItem,
            this.wishlistToolStripMenuItem,
            this.voteToolStripMenuItem});
            this.ContextMenuVN.Name = "contextMenuStrip1";
            this.ContextMenuVN.Size = new System.Drawing.Size(116, 70);
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
            this.userlistToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.userlistToolStripMenuItem.Text = "Userlist";
            this.userlistToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.HandleContextItemClicked);
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
            this.wishlistToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.wishlistToolStripMenuItem.Text = "Wishlist";
            this.wishlistToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.HandleContextItemClicked);
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
            this.voteToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.voteToolStripMenuItem.Text = "Vote";
            this.voteToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.HandleContextItemClicked);
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
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1418, 783);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(50, 50);
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "FormMain";
            this.Text = "Happy Search By Zolty";
            this.Activated += new System.EventHandler(this.FormMain_Enter);
            this.infoTab.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.statBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.vnTab.ResumeLayout(false);
            this.vnTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tileOLV)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olFavoriteProducers)).EndInit();
            this.settingsBox.ResumeLayout(false);
            this.tagFilteringBox.ResumeLayout(false);
            this.tagFilteringBox.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ContextMenuVN.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ToolTip toolTip1;
        private ToolTip toolTip2;
        private ToolTip toolTip3;
        private ImageList langImages;
        private TabPage infoTab;
        private GroupBox groupBox9;
        private RichTextBox aboutTextBox;
        private TabPage vnTab;
        private Button yearButton;
        internal Label loginReply;
        private GroupBox settingsBox;
        private Label userListReply;
        private Button userListButt;
        private Label resultLabel;
        private Button btnFetch;
        private TextBox searchBox;
        private Label label4;
        private TabControl tabControl1;
        private TextBox tagSearchBox;
        private TextBox yearBox;
        private CheckBox checkBox10;
        private CheckBox checkBox7;
        private CheckBox checkBox8;
        private CheckBox checkBox9;
        private CheckBox checkBox4;
        private CheckBox checkBox5;
        private CheckBox checkBox6;
        private CheckBox checkBox3;
        private CheckBox checkBox2;
        private CheckBox checkBox1;
        private Label label7;
        private Label label6;
        private CheckBox tagTypeS;
        private CheckBox tagTypeT;
        private CheckBox tagTypeC;
        private Button quickFilter1;
        private Button quickFilter0;
        private Button quickFilter4;
        private Label mctLoadingLabel;
        private ComboBox ULStatusDropDown;
        private Label label14;
        private TextBox ProducerFilterBox;
        private ComboBox customFilters;
        private Button button2;
        private Button button5;
        private TextBox filterNameBox;
        private ImageList vnImages;
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
        private CheckBox tagTypeS2;
        private CheckBox tagTypeT2;
        private CheckBox tagTypeC2;
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
        private Label label15;
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
        private GroupBox tagFilteringBox;
        private Label filterReply;
        private GroupBox groupBox2;
        internal ObjectListView olFavoriteProducers;
        private OLVColumn ol2Name;
        private OLVColumn ol2ItemCount;
        private OLVColumn ol2Loaded;
        private OLVColumn ol2Updated;
        private OLVColumn ol2ID;
        private Button loadUnloadedButton;
        private Button reloadFavoriteProducersButton;
        private Button selectedProducersVNButton;
        private Button button3;
        private Button addProducersButton;
        private Label prodReply;
        private Label replyText;
        private ComboBox viewPicker;
        private ObjectListView tileOLV;
        internal OLVColumn tileColumnTitle;
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
        private Button updateProducersButton;
        private Button deleteCustomFilterButton;
        private Button updateCustomFilterButton;
        private Button button6;
        private Button loginButton;
        private CheckBox autoUpdateURTBox;
        private Label statusLabel;
        private CheckBox yearLimitBox;
        private OLVColumn ol2UserAverageVote;
        private OLVColumn ol2UserDropRate;
        private CheckBox BlacklistToggleBox;
        private CheckBox UnreleasedToggleBox;
        private CheckBox URTToggleBox;
        private RichTextBox questionBox;
        private GroupBox groupBox3;
        private Label logQueryLabel;
        private Label label1;
        public RichTextBox serverR;
        private Label logReplyLabel;
        private Button clearLogButton;
        public RichTextBox serverQ;
        private Button button1;
        private Button updateProducerTitlesButton;
    }
}

