using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace Visual_Novel_Database
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
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FormMain));
            this.toolTip1 = new ToolTip(this.components);
            this.tagTypeC = new CheckBox();
            this.tagTypeT = new CheckBox();
            this.tagTypeS = new CheckBox();
            this.tagTypeS2 = new CheckBox();
            this.tagTypeT2 = new CheckBox();
            this.tagTypeC2 = new CheckBox();
            this.updateProducersButton = new Button();
            this.autoUpdateURTBox = new CheckBox();
            this.yearLimitBox = new CheckBox();
            this.URTToggleBox = new CheckBox();
            this.langImages = new ImageList(this.components);
            this.toolTip2 = new ToolTip(this.components);
            this.toolTip3 = new ToolTip(this.components);
            this.searchText = new TextBox();
            this.tagSearchBox = new TextBox();
            this.ProducerFilterBox = new TextBox();
            this.filterNameBox = new TextBox();
            this.infoTab = new TabPage();
            this.statBox = new GroupBox();
            this.dbs9r = new Label();
            this.dbs8r = new Label();
            this.dbs7r = new Label();
            this.dbs6r = new Label();
            this.dbs5r = new Label();
            this.dbs4r = new Label();
            this.dbs3r = new Label();
            this.dbs2r = new Label();
            this.dbs1r = new Label();
            this.dbs9 = new Label();
            this.dbs8 = new Label();
            this.dbs7 = new Label();
            this.dbs6 = new Label();
            this.dbs5 = new Label();
            this.dbs4 = new Label();
            this.dbs3 = new Label();
            this.dbs2 = new Label();
            this.dbs1 = new Label();
            this.groupBox1 = new GroupBox();
            this.mctULLabel10 = new Label();
            this.mctULLabel9 = new Label();
            this.mctULLabel8 = new Label();
            this.mctULLabel7 = new Label();
            this.mctULLabel6 = new Label();
            this.mctULLabel5 = new Label();
            this.mctULLabel4 = new Label();
            this.mctULLabel3 = new Label();
            this.mctULLabel2 = new Label();
            this.mctULLabel1 = new Label();
            this.label13 = new Label();
            this.ulstatsavs = new Label();
            this.label12 = new Label();
            this.ulstatsvl = new Label();
            this.ulstatswl = new Label();
            this.ulstatsul = new Label();
            this.ulstatsall = new Label();
            this.label11 = new Label();
            this.label10 = new Label();
            this.label9 = new Label();
            this.label8 = new Label();
            this.groupBox10 = new GroupBox();
            this.richTextBox2 = new RichTextBox();
            this.groupBox9 = new GroupBox();
            this.richTextBox1 = new RichTextBox();
            this.logTab = new TabPage();
            this.logQueryLabel = new Label();
            this.logAskLabel = new Label();
            this.logReplyLabel = new Label();
            this.serverQ = new RichTextBox();
            this.questionBox = new RichTextBox();
            this.serverR = new RichTextBox();
            this.button1 = new Button();
            this.vnTab = new TabPage();
            this.UnreleasedToggleBox = new CheckBox();
            this.BlacklistToggleBox = new CheckBox();
            this.statusLabel = new Label();
            this.updateCustomFilterButton = new Button();
            this.deleteCustomFilterButton = new Button();
            this.viewPicker = new ComboBox();
            this.tileOLV = new ObjectListView();
            this.tileColumnTitle = ((OLVColumn)(new OLVColumn()));
            this.tileColumnDate = ((OLVColumn)(new OLVColumn()));
            this.tileColumnProducer = ((OLVColumn)(new OLVColumn()));
            this.tileColumnULS = ((OLVColumn)(new OLVColumn()));
            this.tileColumnULAdded = ((OLVColumn)(new OLVColumn()));
            this.tileColumnULNote = ((OLVColumn)(new OLVColumn()));
            this.tileColumnWLS = ((OLVColumn)(new OLVColumn()));
            this.tileColumnWLAdded = ((OLVColumn)(new OLVColumn()));
            this.tileColumnVote = ((OLVColumn)(new OLVColumn()));
            this.tileColumnUpdated = ((OLVColumn)(new OLVColumn()));
            this.tileColumnID = ((OLVColumn)(new OLVColumn()));
            this.groupBox2 = new GroupBox();
            this.button6 = new Button();
            this.prodReply = new Label();
            this.addProducersButton = new Button();
            this.olFavoriteProducers = new ObjectListView();
            this.ol2Name = ((OLVColumn)(new OLVColumn()));
            this.ol2ItemCount = ((OLVColumn)(new OLVColumn()));
            this.ol2UserAverageVote = ((OLVColumn)(new OLVColumn()));
            this.ol2UserDropRate = ((OLVColumn)(new OLVColumn()));
            this.ol2Loaded = ((OLVColumn)(new OLVColumn()));
            this.ol2Updated = ((OLVColumn)(new OLVColumn()));
            this.ol2ID = ((OLVColumn)(new OLVColumn()));
            this.loadUnloadedButton = new Button();
            this.reloadFavoriteProducersButton = new Button();
            this.selectedProducersVNButton = new Button();
            this.button3 = new Button();
            this.yearButton = new Button();
            this.replyText = new Label();
            this.label15 = new Label();
            this.yearBox = new TextBox();
            this.btnFetch = new Button();
            this.customFilters = new ComboBox();
            this.label4 = new Label();
            this.label14 = new Label();
            this.ULStatusDropDown = new ComboBox();
            this.quickFilter4 = new Button();
            this.quickFilter0 = new Button();
            this.quickFilter1 = new Button();
            this.settingsBox = new GroupBox();
            this.loginButton = new Button();
            this.closeAllFormsButton = new Button();
            this.nsfwToggle = new CheckBox();
            this.userListReply = new Label();
            this.userListButt = new Button();
            this.loginReply = new Label();
            this.resultLabel = new Label();
            this.tagFilteringBox = new GroupBox();
            this.filterReply = new Label();
            this.label6 = new Label();
            this.checkBox1 = new CheckBox();
            this.button5 = new Button();
            this.checkBox2 = new CheckBox();
            this.button2 = new Button();
            this.checkBox3 = new CheckBox();
            this.mctLoadingLabel = new Label();
            this.checkBox6 = new CheckBox();
            this.checkBox5 = new CheckBox();
            this.checkBox4 = new CheckBox();
            this.checkBox9 = new CheckBox();
            this.label7 = new Label();
            this.checkBox8 = new CheckBox();
            this.checkBox7 = new CheckBox();
            this.checkBox10 = new CheckBox();
            this.tabControl1 = new TabControl();
            this.vnImages = new ImageList(this.components);
            this.ContextMenuVN = new ContextMenuStrip(this.components);
            this.userlistToolStripMenuItem = new ToolStripMenuItem();
            this.noneToolStripMenuItem = new ToolStripMenuItem();
            this.unknownToolStripMenuItem = new ToolStripMenuItem();
            this.playingToolStripMenuItem = new ToolStripMenuItem();
            this.finishedToolStripMenuItem = new ToolStripMenuItem();
            this.stalledToolStripMenuItem = new ToolStripMenuItem();
            this.droppedToolStripMenuItem = new ToolStripMenuItem();
            this.wishlistToolStripMenuItem = new ToolStripMenuItem();
            this.noneToolStripMenuItem1 = new ToolStripMenuItem();
            this.highToolStripMenuItem = new ToolStripMenuItem();
            this.mediumToolStripMenuItem = new ToolStripMenuItem();
            this.lowToolStripMenuItem = new ToolStripMenuItem();
            this.blacklistToolStripMenuItem = new ToolStripMenuItem();
            this.voteToolStripMenuItem = new ToolStripMenuItem();
            this.noneToolStripMenuItem2 = new ToolStripMenuItem();
            this.toolStripMenuItem2 = new ToolStripMenuItem();
            this.toolStripMenuItem3 = new ToolStripMenuItem();
            this.toolStripMenuItem4 = new ToolStripMenuItem();
            this.toolStripMenuItem5 = new ToolStripMenuItem();
            this.toolStripMenuItem6 = new ToolStripMenuItem();
            this.toolStripMenuItem7 = new ToolStripMenuItem();
            this.toolStripMenuItem8 = new ToolStripMenuItem();
            this.toolStripMenuItem9 = new ToolStripMenuItem();
            this.toolStripMenuItem10 = new ToolStripMenuItem();
            this.toolStripMenuItem11 = new ToolStripMenuItem();
            this.infoTab.SuspendLayout();
            this.statBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.logTab.SuspendLayout();
            this.vnTab.SuspendLayout();
            ((ISupportInitialize)(this.tileOLV)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((ISupportInitialize)(this.olFavoriteProducers)).BeginInit();
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
            this.tagTypeC.CheckState = CheckState.Checked;
            this.tagTypeC.Location = new Point(113, 20);
            this.tagTypeC.Name = "tagTypeC";
            this.tagTypeC.Size = new Size(33, 17);
            this.tagTypeC.TabIndex = 35;
            this.tagTypeC.Text = "C";
            this.toolTip1.SetToolTip(this.tagTypeC, "Display Content Tags");
            this.tagTypeC.UseVisualStyleBackColor = true;
            this.tagTypeC.Click += new EventHandler(this.DisplayCommonTags);
            // 
            // tagTypeT
            // 
            this.tagTypeT.AutoSize = true;
            this.tagTypeT.Checked = true;
            this.tagTypeT.CheckState = CheckState.Checked;
            this.tagTypeT.Location = new Point(191, 20);
            this.tagTypeT.Name = "tagTypeT";
            this.tagTypeT.Size = new Size(33, 17);
            this.tagTypeT.TabIndex = 36;
            this.tagTypeT.Text = "T";
            this.toolTip1.SetToolTip(this.tagTypeT, "Display Technical Tags");
            this.tagTypeT.UseVisualStyleBackColor = true;
            this.tagTypeT.Click += new EventHandler(this.DisplayCommonTags);
            // 
            // tagTypeS
            // 
            this.tagTypeS.AutoSize = true;
            this.tagTypeS.Checked = true;
            this.tagTypeS.CheckState = CheckState.Checked;
            this.tagTypeS.Location = new Point(152, 20);
            this.tagTypeS.Name = "tagTypeS";
            this.tagTypeS.Size = new Size(33, 17);
            this.tagTypeS.TabIndex = 37;
            this.tagTypeS.Text = "S";
            this.toolTip1.SetToolTip(this.tagTypeS, "Display Sexual Content Tags");
            this.tagTypeS.UseVisualStyleBackColor = true;
            this.tagTypeS.Click += new EventHandler(this.DisplayCommonTags);
            // 
            // tagTypeS2
            // 
            this.tagTypeS2.AutoSize = true;
            this.tagTypeS2.Checked = true;
            this.tagTypeS2.CheckState = CheckState.Checked;
            this.tagTypeS2.Location = new Point(184, 96);
            this.tagTypeS2.Name = "tagTypeS2";
            this.tagTypeS2.Size = new Size(33, 17);
            this.tagTypeS2.TabIndex = 63;
            this.tagTypeS2.Text = "S";
            this.toolTip1.SetToolTip(this.tagTypeS2, "Display Sexual Content Tags");
            this.tagTypeS2.UseVisualStyleBackColor = true;
            this.tagTypeS2.Click += new EventHandler(this.DisplayCommonTagsULStats);
            // 
            // tagTypeT2
            // 
            this.tagTypeT2.AutoSize = true;
            this.tagTypeT2.Checked = true;
            this.tagTypeT2.CheckState = CheckState.Checked;
            this.tagTypeT2.Location = new Point(223, 96);
            this.tagTypeT2.Name = "tagTypeT2";
            this.tagTypeT2.Size = new Size(33, 17);
            this.tagTypeT2.TabIndex = 62;
            this.tagTypeT2.Text = "T";
            this.toolTip1.SetToolTip(this.tagTypeT2, "Display Technical Tags");
            this.tagTypeT2.UseVisualStyleBackColor = true;
            this.tagTypeT2.Click += new EventHandler(this.DisplayCommonTagsULStats);
            // 
            // tagTypeC2
            // 
            this.tagTypeC2.AutoSize = true;
            this.tagTypeC2.Checked = true;
            this.tagTypeC2.CheckState = CheckState.Checked;
            this.tagTypeC2.Location = new Point(145, 96);
            this.tagTypeC2.Name = "tagTypeC2";
            this.tagTypeC2.Size = new Size(33, 17);
            this.tagTypeC2.TabIndex = 61;
            this.tagTypeC2.Text = "C";
            this.toolTip1.SetToolTip(this.tagTypeC2, "Display Content Tags");
            this.tagTypeC2.UseVisualStyleBackColor = true;
            this.tagTypeC2.Click += new EventHandler(this.DisplayCommonTagsULStats);
            // 
            // updateProducersButton
            // 
            this.updateProducersButton.ForeColor = SystemColors.ControlText;
            this.updateProducersButton.Location = new Point(279, 289);
            this.updateProducersButton.Name = "updateProducersButton";
            this.updateProducersButton.Size = new Size(136, 23);
            this.updateProducersButton.TabIndex = 37;
            this.updateProducersButton.Text = "Get New Producer Titles";
            this.toolTip1.SetToolTip(this.updateProducersButton, "Get Producer titles added since last update.");
            this.updateProducersButton.UseVisualStyleBackColor = true;
            this.updateProducersButton.Click += new EventHandler(this.GetNewFavoriteProducerTitles);
            // 
            // autoUpdateURTBox
            // 
            this.autoUpdateURTBox.ForeColor = SystemColors.ControlLightLight;
            this.autoUpdateURTBox.Location = new Point(7, 204);
            this.autoUpdateURTBox.Name = "autoUpdateURTBox";
            this.autoUpdateURTBox.Size = new Size(126, 17);
            this.autoUpdateURTBox.TabIndex = 83;
            this.autoUpdateURTBox.Text = "Auto-Update URT";
            this.autoUpdateURTBox.TextAlign = ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.autoUpdateURTBox, "Update User Related Titles every 2 days.");
            this.autoUpdateURTBox.UseVisualStyleBackColor = true;
            this.autoUpdateURTBox.Click += new EventHandler(this.ToggleAutoUpdateURT);
            // 
            // yearLimitBox
            // 
            this.yearLimitBox.ForeColor = SystemColors.ControlLightLight;
            this.yearLimitBox.Location = new Point(7, 227);
            this.yearLimitBox.Name = "yearLimitBox";
            this.yearLimitBox.Size = new Size(126, 17);
            this.yearLimitBox.TabIndex = 84;
            this.yearLimitBox.Text = "10 Year Limit";
            this.yearLimitBox.TextAlign = ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.yearLimitBox, "Limits VNs fetched from VNDB to last 10 years.");
            this.yearLimitBox.UseVisualStyleBackColor = true;
            this.yearLimitBox.Click += new EventHandler(this.ToggleLimit10Years);
            // 
            // URTToggleBox
            // 
            this.URTToggleBox.Appearance = Appearance.Button;
            this.URTToggleBox.BackColor = Color.Black;
            this.URTToggleBox.Checked = true;
            this.URTToggleBox.CheckState = CheckState.Indeterminate;
            this.URTToggleBox.FlatAppearance.BorderColor = Color.Black;
            this.URTToggleBox.FlatAppearance.BorderSize = 0;
            this.URTToggleBox.FlatAppearance.CheckedBackColor = Color.Black;
            this.URTToggleBox.FlatStyle = FlatStyle.Flat;
            this.URTToggleBox.ForeColor = SystemColors.ControlLightLight;
            this.URTToggleBox.Location = new Point(1034, 336);
            this.URTToggleBox.Name = "URTToggleBox";
            this.URTToggleBox.Size = new Size(68, 23);
            this.URTToggleBox.TabIndex = 83;
            this.URTToggleBox.Text = "Show URT";
            this.URTToggleBox.TextAlign = ContentAlignment.MiddleCenter;
            this.URTToggleBox.ThreeState = true;
            this.toolTip1.SetToolTip(this.URTToggleBox, "URT - User Related Title");
            this.URTToggleBox.UseVisualStyleBackColor = false;
            this.URTToggleBox.Click += new EventHandler(this.URTToggle);
            // 
            // langImages
            // 
            this.langImages.ImageStream = ((ImageListStreamer)(resources.GetObject("langImages.ImageStream")));
            this.langImages.TransparentColor = Color.Transparent;
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
            // searchText
            // 
            this.searchText.ForeColor = SystemColors.ControlText;
            this.searchText.Location = new Point(93, 311);
            this.searchText.Name = "searchText";
            this.searchText.Size = new Size(92, 20);
            this.searchText.TabIndex = 21;
            this.toolTip3.SetToolTip(this.searchText, "Type VN name here.");
            this.searchText.KeyPress += new KeyPressEventHandler(this.searchButton_keyPress);
            // 
            // tagSearchBox
            // 
            this.tagSearchBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.tagSearchBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.tagSearchBox.Font = new Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.tagSearchBox.Location = new Point(264, 17);
            this.tagSearchBox.Name = "tagSearchBox";
            this.tagSearchBox.Size = new Size(187, 22);
            this.tagSearchBox.TabIndex = 29;
            this.toolTip3.SetToolTip(this.tagSearchBox, "Type VN name here.");
            this.tagSearchBox.KeyDown += new KeyEventHandler(this.tagSearchBox_KeyDown);
            // 
            // ProducerFilterBox
            // 
            this.ProducerFilterBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.ProducerFilterBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.ProducerFilterBox.Font = new Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.ProducerFilterBox.Location = new Point(746, 336);
            this.ProducerFilterBox.Name = "ProducerFilterBox";
            this.ProducerFilterBox.Size = new Size(145, 22);
            this.ProducerFilterBox.TabIndex = 39;
            this.toolTip3.SetToolTip(this.ProducerFilterBox, "Type VN name here.");
            this.ProducerFilterBox.KeyDown += new KeyEventHandler(this.Filter_Producer);
            // 
            // filterNameBox
            // 
            this.filterNameBox.Font = new Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.filterNameBox.Location = new Point(461, 45);
            this.filterNameBox.Name = "filterNameBox";
            this.filterNameBox.Size = new Size(100, 22);
            this.filterNameBox.TabIndex = 41;
            this.toolTip3.SetToolTip(this.filterNameBox, "Type VN name here.");
            this.filterNameBox.KeyDown += new KeyEventHandler(this.filterNameBox_KeyDown);
            // 
            // infoTab
            // 
            this.infoTab.BackColor = SystemColors.Control;
            this.infoTab.Controls.Add(this.statBox);
            this.infoTab.Controls.Add(this.groupBox1);
            this.infoTab.Controls.Add(this.groupBox10);
            this.infoTab.Controls.Add(this.groupBox9);
            this.infoTab.Location = new Point(4, 22);
            this.infoTab.Name = "infoTab";
            this.infoTab.Padding = new Padding(3);
            this.infoTab.Size = new Size(1410, 757);
            this.infoTab.TabIndex = 2;
            this.infoTab.Text = "Information";
            // 
            // statBox
            // 
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
            this.statBox.ForeColor = SystemColors.ControlText;
            this.statBox.Location = new Point(1263, 6);
            this.statBox.Name = "statBox";
            this.statBox.Size = new Size(139, 254);
            this.statBox.TabIndex = 32;
            this.statBox.TabStop = false;
            this.statBox.Text = "VNDB Stats";
            // 
            // dbs9r
            // 
            this.dbs9r.Location = new Point(67, 124);
            this.dbs9r.Name = "dbs9r";
            this.dbs9r.Size = new Size(55, 13);
            this.dbs9r.TabIndex = 40;
            this.dbs9r.Text = "(blank)";
            // 
            // dbs8r
            // 
            this.dbs8r.Location = new Point(67, 111);
            this.dbs8r.Name = "dbs8r";
            this.dbs8r.Size = new Size(55, 13);
            this.dbs8r.TabIndex = 39;
            this.dbs8r.Text = "(blank)";
            // 
            // dbs7r
            // 
            this.dbs7r.Location = new Point(67, 98);
            this.dbs7r.Name = "dbs7r";
            this.dbs7r.Size = new Size(55, 13);
            this.dbs7r.TabIndex = 38;
            this.dbs7r.Text = "(blank)";
            // 
            // dbs6r
            // 
            this.dbs6r.Location = new Point(67, 85);
            this.dbs6r.Name = "dbs6r";
            this.dbs6r.Size = new Size(55, 13);
            this.dbs6r.TabIndex = 37;
            this.dbs6r.Text = "(blank)";
            // 
            // dbs5r
            // 
            this.dbs5r.Location = new Point(67, 72);
            this.dbs5r.Name = "dbs5r";
            this.dbs5r.Size = new Size(55, 13);
            this.dbs5r.TabIndex = 36;
            this.dbs5r.Text = "(blank)";
            // 
            // dbs4r
            // 
            this.dbs4r.Location = new Point(67, 59);
            this.dbs4r.Name = "dbs4r";
            this.dbs4r.Size = new Size(55, 13);
            this.dbs4r.TabIndex = 35;
            this.dbs4r.Text = "(blank)";
            // 
            // dbs3r
            // 
            this.dbs3r.Location = new Point(67, 47);
            this.dbs3r.Name = "dbs3r";
            this.dbs3r.Size = new Size(55, 13);
            this.dbs3r.TabIndex = 34;
            this.dbs3r.Text = "(blank)";
            // 
            // dbs2r
            // 
            this.dbs2r.Location = new Point(67, 34);
            this.dbs2r.Name = "dbs2r";
            this.dbs2r.Size = new Size(55, 13);
            this.dbs2r.TabIndex = 33;
            this.dbs2r.Text = "(blank)";
            // 
            // dbs1r
            // 
            this.dbs1r.Location = new Point(67, 21);
            this.dbs1r.Name = "dbs1r";
            this.dbs1r.Size = new Size(55, 13);
            this.dbs1r.TabIndex = 32;
            this.dbs1r.Text = "(blank)";
            // 
            // dbs9
            // 
            this.dbs9.Location = new Point(8, 124);
            this.dbs9.Name = "dbs9";
            this.dbs9.Size = new Size(55, 13);
            this.dbs9.TabIndex = 31;
            this.dbs9.Text = "Traits";
            this.dbs9.TextAlign = ContentAlignment.TopRight;
            // 
            // dbs8
            // 
            this.dbs8.Location = new Point(8, 111);
            this.dbs8.Name = "dbs8";
            this.dbs8.Size = new Size(55, 13);
            this.dbs8.TabIndex = 30;
            this.dbs8.Text = "VN";
            this.dbs8.TextAlign = ContentAlignment.TopRight;
            // 
            // dbs7
            // 
            this.dbs7.Location = new Point(8, 98);
            this.dbs7.Name = "dbs7";
            this.dbs7.Size = new Size(55, 13);
            this.dbs7.TabIndex = 29;
            this.dbs7.Text = "Posts";
            this.dbs7.TextAlign = ContentAlignment.TopRight;
            // 
            // dbs6
            // 
            this.dbs6.Location = new Point(8, 85);
            this.dbs6.Name = "dbs6";
            this.dbs6.Size = new Size(55, 13);
            this.dbs6.TabIndex = 28;
            this.dbs6.Text = "Chars";
            this.dbs6.TextAlign = ContentAlignment.TopRight;
            // 
            // dbs5
            // 
            this.dbs5.Location = new Point(8, 72);
            this.dbs5.Name = "dbs5";
            this.dbs5.Size = new Size(55, 13);
            this.dbs5.TabIndex = 27;
            this.dbs5.Text = "Producers";
            this.dbs5.TextAlign = ContentAlignment.TopRight;
            // 
            // dbs4
            // 
            this.dbs4.Location = new Point(8, 60);
            this.dbs4.Name = "dbs4";
            this.dbs4.Size = new Size(55, 13);
            this.dbs4.TabIndex = 26;
            this.dbs4.Text = "Releases";
            this.dbs4.TextAlign = ContentAlignment.TopRight;
            // 
            // dbs3
            // 
            this.dbs3.Location = new Point(8, 47);
            this.dbs3.Name = "dbs3";
            this.dbs3.Size = new Size(55, 13);
            this.dbs3.TabIndex = 25;
            this.dbs3.Text = "Tags";
            this.dbs3.TextAlign = ContentAlignment.TopRight;
            // 
            // dbs2
            // 
            this.dbs2.Location = new Point(8, 34);
            this.dbs2.Name = "dbs2";
            this.dbs2.Size = new Size(55, 13);
            this.dbs2.TabIndex = 24;
            this.dbs2.Text = "Threads";
            this.dbs2.TextAlign = ContentAlignment.TopRight;
            // 
            // dbs1
            // 
            this.dbs1.Location = new Point(8, 21);
            this.dbs1.Name = "dbs1";
            this.dbs1.Size = new Size(55, 13);
            this.dbs1.TabIndex = 23;
            this.dbs1.Text = "Users";
            this.dbs1.TextAlign = ContentAlignment.TopRight;
            // 
            // groupBox1
            // 
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
            this.groupBox1.ForeColor = SystemColors.ControlText;
            this.groupBox1.Location = new Point(540, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(717, 254);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Stats";
            // 
            // mctULLabel10
            // 
            this.mctULLabel10.Location = new Point(6, 231);
            this.mctULLabel10.Name = "mctULLabel10";
            this.mctULLabel10.Size = new Size(253, 13);
            this.mctULLabel10.TabIndex = 74;
            this.mctULLabel10.Text = "(blank)";
            // 
            // mctULLabel9
            // 
            this.mctULLabel9.Location = new Point(6, 218);
            this.mctULLabel9.Name = "mctULLabel9";
            this.mctULLabel9.Size = new Size(253, 13);
            this.mctULLabel9.TabIndex = 73;
            this.mctULLabel9.Text = "(blank)";
            // 
            // mctULLabel8
            // 
            this.mctULLabel8.Location = new Point(6, 205);
            this.mctULLabel8.Name = "mctULLabel8";
            this.mctULLabel8.Size = new Size(253, 13);
            this.mctULLabel8.TabIndex = 72;
            this.mctULLabel8.Text = "(blank)";
            // 
            // mctULLabel7
            // 
            this.mctULLabel7.Location = new Point(6, 192);
            this.mctULLabel7.Name = "mctULLabel7";
            this.mctULLabel7.Size = new Size(253, 13);
            this.mctULLabel7.TabIndex = 71;
            this.mctULLabel7.Text = "(blank)";
            // 
            // mctULLabel6
            // 
            this.mctULLabel6.Location = new Point(6, 179);
            this.mctULLabel6.Name = "mctULLabel6";
            this.mctULLabel6.Size = new Size(253, 13);
            this.mctULLabel6.TabIndex = 70;
            this.mctULLabel6.Text = "(blank)";
            // 
            // mctULLabel5
            // 
            this.mctULLabel5.Location = new Point(6, 166);
            this.mctULLabel5.Name = "mctULLabel5";
            this.mctULLabel5.Size = new Size(253, 13);
            this.mctULLabel5.TabIndex = 69;
            this.mctULLabel5.Text = "(blank)";
            // 
            // mctULLabel4
            // 
            this.mctULLabel4.Location = new Point(6, 153);
            this.mctULLabel4.Name = "mctULLabel4";
            this.mctULLabel4.Size = new Size(253, 13);
            this.mctULLabel4.TabIndex = 68;
            this.mctULLabel4.Text = "(blank)";
            // 
            // mctULLabel3
            // 
            this.mctULLabel3.Location = new Point(6, 140);
            this.mctULLabel3.Name = "mctULLabel3";
            this.mctULLabel3.Size = new Size(253, 13);
            this.mctULLabel3.TabIndex = 67;
            this.mctULLabel3.Text = "(blank)";
            // 
            // mctULLabel2
            // 
            this.mctULLabel2.Location = new Point(6, 127);
            this.mctULLabel2.Name = "mctULLabel2";
            this.mctULLabel2.Size = new Size(253, 13);
            this.mctULLabel2.TabIndex = 66;
            this.mctULLabel2.Text = "(blank)";
            // 
            // mctULLabel1
            // 
            this.mctULLabel1.Location = new Point(6, 114);
            this.mctULLabel1.Name = "mctULLabel1";
            this.mctULLabel1.Size = new Size(253, 13);
            this.mctULLabel1.TabIndex = 65;
            this.mctULLabel1.Text = "(blank)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new Point(6, 97);
            this.label13.Name = "label13";
            this.label13.Size = new Size(101, 13);
            this.label13.TabIndex = 60;
            this.label13.Text = "Most Common Tags";
            // 
            // ulstatsavs
            // 
            this.ulstatsavs.Location = new Point(148, 72);
            this.ulstatsavs.Name = "ulstatsavs";
            this.ulstatsavs.Size = new Size(55, 13);
            this.ulstatsavs.TabIndex = 49;
            this.ulstatsavs.Text = "(blank)";
            // 
            // label12
            // 
            this.label12.Location = new Point(6, 72);
            this.label12.Name = "label12";
            this.label12.Size = new Size(136, 13);
            this.label12.TabIndex = 48;
            this.label12.Text = "Average Vote Score";
            this.label12.TextAlign = ContentAlignment.TopRight;
            // 
            // ulstatsvl
            // 
            this.ulstatsvl.Location = new Point(148, 58);
            this.ulstatsvl.Name = "ulstatsvl";
            this.ulstatsvl.Size = new Size(55, 13);
            this.ulstatsvl.TabIndex = 47;
            this.ulstatsvl.Text = "(blank)";
            // 
            // ulstatswl
            // 
            this.ulstatswl.Location = new Point(148, 46);
            this.ulstatswl.Name = "ulstatswl";
            this.ulstatswl.Size = new Size(55, 13);
            this.ulstatswl.TabIndex = 46;
            this.ulstatswl.Text = "(blank)";
            // 
            // ulstatsul
            // 
            this.ulstatsul.Location = new Point(148, 33);
            this.ulstatsul.Name = "ulstatsul";
            this.ulstatsul.Size = new Size(55, 13);
            this.ulstatsul.TabIndex = 45;
            this.ulstatsul.Text = "(blank)";
            // 
            // ulstatsall
            // 
            this.ulstatsall.Location = new Point(148, 20);
            this.ulstatsall.Name = "ulstatsall";
            this.ulstatsall.Size = new Size(55, 13);
            this.ulstatsall.TabIndex = 41;
            this.ulstatsall.Text = "(blank)";
            // 
            // label11
            // 
            this.label11.Location = new Point(6, 59);
            this.label11.Name = "label11";
            this.label11.Size = new Size(136, 13);
            this.label11.TabIndex = 44;
            this.label11.Text = "Titles in Votelist";
            this.label11.TextAlign = ContentAlignment.TopRight;
            // 
            // label10
            // 
            this.label10.Location = new Point(6, 46);
            this.label10.Name = "label10";
            this.label10.Size = new Size(136, 13);
            this.label10.TabIndex = 43;
            this.label10.Text = "Titles in Wishlist";
            this.label10.TextAlign = ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.Location = new Point(6, 33);
            this.label9.Name = "label9";
            this.label9.Size = new Size(136, 13);
            this.label9.TabIndex = 42;
            this.label9.Text = "Titles in Userlist";
            this.label9.TextAlign = ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.Location = new Point(6, 20);
            this.label8.Name = "label8";
            this.label8.Size = new Size(136, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "All Titles Related to User";
            this.label8.TextAlign = ContentAlignment.TopRight;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.richTextBox2);
            this.groupBox10.Location = new Point(8, 106);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new Size(526, 154);
            this.groupBox10.TabIndex = 0;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Usage Information";
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = SystemColors.Control;
            this.richTextBox2.BorderStyle = BorderStyle.None;
            this.richTextBox2.Enabled = false;
            this.richTextBox2.Location = new Point(6, 19);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new Size(514, 125);
            this.richTextBox2.TabIndex = 1;
            this.richTextBox2.Text = "This project is not completed.";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.richTextBox1);
            this.groupBox9.Location = new Point(8, 6);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new Size(526, 94);
            this.groupBox9.TabIndex = 1;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Basic Information";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = SystemColors.Control;
            this.richTextBox1.BorderStyle = BorderStyle.None;
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Location = new Point(6, 19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new Size(514, 69);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // logTab
            // 
            this.logTab.Controls.Add(this.logQueryLabel);
            this.logTab.Controls.Add(this.logAskLabel);
            this.logTab.Controls.Add(this.logReplyLabel);
            this.logTab.Controls.Add(this.serverQ);
            this.logTab.Controls.Add(this.questionBox);
            this.logTab.Controls.Add(this.serverR);
            this.logTab.Controls.Add(this.button1);
            this.logTab.Location = new Point(4, 22);
            this.logTab.Name = "logTab";
            this.logTab.Padding = new Padding(3);
            this.logTab.Size = new Size(1410, 757);
            this.logTab.TabIndex = 3;
            this.logTab.Text = "Log";
            this.logTab.UseVisualStyleBackColor = true;
            // 
            // logQueryLabel
            // 
            this.logQueryLabel.AutoSize = true;
            this.logQueryLabel.Location = new Point(8, 90);
            this.logQueryLabel.Name = "logQueryLabel";
            this.logQueryLabel.Size = new Size(93, 13);
            this.logQueryLabel.TabIndex = 33;
            this.logQueryLabel.Text = "Queries To Server";
            // 
            // logAskLabel
            // 
            this.logAskLabel.AutoSize = true;
            this.logAskLabel.Location = new Point(8, 7);
            this.logAskLabel.Name = "logAskLabel";
            this.logAskLabel.Size = new Size(89, 13);
            this.logAskLabel.TabIndex = 32;
            this.logAskLabel.Text = "Send Query Here";
            // 
            // logReplyLabel
            // 
            this.logReplyLabel.AutoSize = true;
            this.logReplyLabel.Location = new Point(348, 7);
            this.logReplyLabel.Name = "logReplyLabel";
            this.logReplyLabel.Size = new Size(102, 13);
            this.logReplyLabel.TabIndex = 31;
            this.logReplyLabel.Text = "Replies From Server";
            // 
            // serverQ
            // 
            this.serverQ.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left)));
            this.serverQ.BackColor = SystemColors.InactiveCaptionText;
            this.serverQ.Font = new Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.serverQ.ForeColor = SystemColors.Info;
            this.serverQ.Location = new Point(8, 106);
            this.serverQ.Name = "serverQ";
            this.serverQ.Size = new Size(334, 614);
            this.serverQ.TabIndex = 30;
            this.serverQ.Text = "";
            // 
            // questionBox
            // 
            this.questionBox.BackColor = SystemColors.Info;
            this.questionBox.Font = new Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.questionBox.ForeColor = SystemColors.InfoText;
            this.questionBox.Location = new Point(8, 23);
            this.questionBox.Name = "questionBox";
            this.questionBox.Size = new Size(334, 64);
            this.questionBox.TabIndex = 29;
            this.questionBox.Text = "";
            this.questionBox.KeyPress += new KeyPressEventHandler(this.LogQuestion);
            // 
            // serverR
            // 
            this.serverR.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.serverR.BackColor = SystemColors.InfoText;
            this.serverR.Font = new Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.serverR.ForeColor = SystemColors.Info;
            this.serverR.Location = new Point(348, 23);
            this.serverR.Name = "serverR";
            this.serverR.Size = new Size(1054, 726);
            this.serverR.TabIndex = 27;
            this.serverR.Text = "";
            // 
            // button1
            // 
            this.button1.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.button1.Location = new Point(8, 726);
            this.button1.Name = "button1";
            this.button1.Size = new Size(100, 23);
            this.button1.TabIndex = 28;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.ClearLog);
            // 
            // vnTab
            // 
            this.vnTab.BackColor = Color.Gray;
            this.vnTab.BackgroundImageLayout = ImageLayout.Stretch;
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
            this.vnTab.Controls.Add(this.searchText);
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
            this.vnTab.ForeColor = SystemColors.ControlLightLight;
            this.vnTab.Location = new Point(4, 22);
            this.vnTab.Name = "vnTab";
            this.vnTab.Padding = new Padding(3);
            this.vnTab.Size = new Size(1410, 757);
            this.vnTab.TabIndex = 1;
            this.vnTab.Text = "Visual Novel Info";
            // 
            // UnreleasedToggleBox
            // 
            this.UnreleasedToggleBox.Appearance = Appearance.Button;
            this.UnreleasedToggleBox.BackColor = Color.Black;
            this.UnreleasedToggleBox.Checked = true;
            this.UnreleasedToggleBox.CheckState = CheckState.Indeterminate;
            this.UnreleasedToggleBox.FlatAppearance.BorderColor = Color.Black;
            this.UnreleasedToggleBox.FlatAppearance.BorderSize = 0;
            this.UnreleasedToggleBox.FlatAppearance.CheckedBackColor = Color.Black;
            this.UnreleasedToggleBox.FlatStyle = FlatStyle.Flat;
            this.UnreleasedToggleBox.ForeColor = SystemColors.ControlLightLight;
            this.UnreleasedToggleBox.Location = new Point(1107, 336);
            this.UnreleasedToggleBox.Name = "UnreleasedToggleBox";
            this.UnreleasedToggleBox.Size = new Size(99, 23);
            this.UnreleasedToggleBox.TabIndex = 82;
            this.UnreleasedToggleBox.Text = "Show Unreleased";
            this.UnreleasedToggleBox.TextAlign = ContentAlignment.MiddleCenter;
            this.UnreleasedToggleBox.ThreeState = true;
            this.UnreleasedToggleBox.UseVisualStyleBackColor = false;
            this.UnreleasedToggleBox.Click += new EventHandler(this.UnreleasedToggle);
            // 
            // BlacklistToggleBox
            // 
            this.BlacklistToggleBox.Appearance = Appearance.Button;
            this.BlacklistToggleBox.BackColor = Color.Black;
            this.BlacklistToggleBox.Checked = true;
            this.BlacklistToggleBox.CheckState = CheckState.Indeterminate;
            this.BlacklistToggleBox.FlatAppearance.BorderColor = Color.Black;
            this.BlacklistToggleBox.FlatAppearance.BorderSize = 0;
            this.BlacklistToggleBox.FlatAppearance.CheckedBackColor = Color.Black;
            this.BlacklistToggleBox.FlatStyle = FlatStyle.Flat;
            this.BlacklistToggleBox.ForeColor = SystemColors.ControlLightLight;
            this.BlacklistToggleBox.Location = new Point(1212, 336);
            this.BlacklistToggleBox.Name = "BlacklistToggleBox";
            this.BlacklistToggleBox.Size = new Size(97, 23);
            this.BlacklistToggleBox.TabIndex = 81;
            this.BlacklistToggleBox.Text = "Show Blacklisted";
            this.BlacklistToggleBox.TextAlign = ContentAlignment.MiddleCenter;
            this.BlacklistToggleBox.ThreeState = true;
            this.BlacklistToggleBox.UseVisualStyleBackColor = false;
            this.BlacklistToggleBox.Click += new EventHandler(this.BlacklistToggle);
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.statusLabel.ForeColor = SystemColors.ControlText;
            this.statusLabel.Location = new Point(6, 739);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new Size(152, 13);
            this.statusLabel.TabIndex = 80;
            this.statusLabel.Text = "(statusLabel)";
            // 
            // updateCustomFilterButton
            // 
            this.updateCustomFilterButton.BackColor = Color.LightCoral;
            this.updateCustomFilterButton.Enabled = false;
            this.updateCustomFilterButton.FlatAppearance.BorderSize = 0;
            this.updateCustomFilterButton.FlatStyle = FlatStyle.Flat;
            this.updateCustomFilterButton.ForeColor = SystemColors.ControlText;
            this.updateCustomFilterButton.Location = new Point(631, 336);
            this.updateCustomFilterButton.Name = "updateCustomFilterButton";
            this.updateCustomFilterButton.Size = new Size(53, 23);
            this.updateCustomFilterButton.TabIndex = 79;
            this.updateCustomFilterButton.Text = "Update";
            this.updateCustomFilterButton.UseVisualStyleBackColor = false;
            this.updateCustomFilterButton.Click += new EventHandler(this.UpdateCustomFilter);
            // 
            // deleteCustomFilterButton
            // 
            this.deleteCustomFilterButton.BackColor = Color.LightCoral;
            this.deleteCustomFilterButton.Enabled = false;
            this.deleteCustomFilterButton.FlatAppearance.BorderSize = 0;
            this.deleteCustomFilterButton.FlatStyle = FlatStyle.Flat;
            this.deleteCustomFilterButton.ForeColor = SystemColors.ControlText;
            this.deleteCustomFilterButton.Location = new Point(574, 336);
            this.deleteCustomFilterButton.Name = "deleteCustomFilterButton";
            this.deleteCustomFilterButton.Size = new Size(53, 23);
            this.deleteCustomFilterButton.TabIndex = 78;
            this.deleteCustomFilterButton.Text = "Delete";
            this.deleteCustomFilterButton.UseVisualStyleBackColor = false;
            this.deleteCustomFilterButton.Click += new EventHandler(this.DeleteCustomFilter);
            // 
            // viewPicker
            // 
            this.viewPicker.DropDownStyle = ComboBoxStyle.DropDownList;
            this.viewPicker.FormattingEnabled = true;
            this.viewPicker.Items.AddRange(new object[] {
            "Tile",
            "Details"});
            this.viewPicker.Location = new Point(1315, 336);
            this.viewPicker.Name = "viewPicker";
            this.viewPicker.Size = new Size(89, 21);
            this.viewPicker.TabIndex = 77;
            this.viewPicker.SelectedIndexChanged += new EventHandler(this.OLVChangeView);
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
            this.tileOLV.AlternateRowBackColor = Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tileOLV.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.tileOLV.BackColor = SystemColors.InactiveCaption;
            this.tileOLV.Columns.AddRange(new ColumnHeader[] {
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
            this.tileOLV.Location = new Point(6, 365);
            this.tileOLV.MultiSelect = false;
            this.tileOLV.Name = "tileOLV";
            this.tileOLV.OwnerDraw = true;
            this.tileOLV.ShowCommandMenuOnRightClick = true;
            this.tileOLV.ShowGroups = false;
            this.tileOLV.Size = new Size(1398, 371);
            this.tileOLV.TabIndex = 63;
            this.tileOLV.TileSize = new Size(230, 375);
            this.tileOLV.UseAlternatingBackColors = true;
            this.tileOLV.UseCompatibleStateImageBehavior = false;
            this.tileOLV.UseFiltering = true;
            this.tileOLV.View = View.Tile;
            this.tileOLV.CellClick += new EventHandler<CellClickEventArgs>(this.ObjectList_SelectedIndexChanged);
            this.tileOLV.CellRightClick += new EventHandler<CellRightClickEventArgs>(this.ShowContextMenu);
            this.tileOLV.FormatRow += new EventHandler<FormatRowEventArgs>(this.FormatRow);
            this.tileOLV.ItemsChanged += new EventHandler<ItemsChangedEventArgs>(this.objectList_ItemsChanged);
            this.tileOLV.Resize += new EventHandler(this.tileOLV_Resize);
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
            this.groupBox2.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button6);
            this.groupBox2.Controls.Add(this.updateProducersButton);
            this.groupBox2.Controls.Add(this.prodReply);
            this.groupBox2.Controls.Add(this.addProducersButton);
            this.groupBox2.Controls.Add(this.olFavoriteProducers);
            this.groupBox2.Controls.Add(this.loadUnloadedButton);
            this.groupBox2.Controls.Add(this.reloadFavoriteProducersButton);
            this.groupBox2.Controls.Add(this.selectedProducersVNButton);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.ForeColor = SystemColors.ControlLightLight;
            this.groupBox2.Location = new Point(724, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(680, 318);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Favorite Producers";
            // 
            // button6
            // 
            this.button6.BackColor = Color.MistyRose;
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatStyle = FlatStyle.Flat;
            this.button6.ForeColor = SystemColors.ControlText;
            this.button6.Location = new Point(112, 260);
            this.button6.Name = "button6";
            this.button6.Size = new Size(104, 23);
            this.button6.TabIndex = 47;
            this.button6.Text = "Test";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new EventHandler(this.Test);
            // 
            // prodReply
            // 
            this.prodReply.Location = new Point(6, 289);
            this.prodReply.Name = "prodReply";
            this.prodReply.Size = new Size(267, 23);
            this.prodReply.TabIndex = 32;
            this.prodReply.Text = "(prodReply)";
            this.prodReply.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // addProducersButton
            // 
            this.addProducersButton.ForeColor = SystemColors.ControlText;
            this.addProducersButton.Location = new Point(6, 260);
            this.addProducersButton.Name = "addProducersButton";
            this.addProducersButton.Size = new Size(100, 23);
            this.addProducersButton.TabIndex = 36;
            this.addProducersButton.Text = "Add Producers";
            this.addProducersButton.UseVisualStyleBackColor = true;
            this.addProducersButton.Click += new EventHandler(this.AddProducers);
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
            this.olFavoriteProducers.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.olFavoriteProducers.Columns.AddRange(new ColumnHeader[] {
            this.ol2Name,
            this.ol2ItemCount,
            this.ol2UserAverageVote,
            this.ol2UserDropRate,
            this.ol2Loaded,
            this.ol2Updated,
            this.ol2ID});
            this.olFavoriteProducers.FullRowSelect = true;
            this.olFavoriteProducers.GridLines = true;
            this.olFavoriteProducers.Location = new Point(6, 21);
            this.olFavoriteProducers.Name = "olFavoriteProducers";
            this.olFavoriteProducers.ShowGroups = false;
            this.olFavoriteProducers.Size = new Size(668, 233);
            this.olFavoriteProducers.TabIndex = 0;
            this.olFavoriteProducers.UseCompatibleStateImageBehavior = false;
            this.olFavoriteProducers.View = View.Details;
            this.olFavoriteProducers.FormatRow += new EventHandler<FormatRowEventArgs>(this.FormatRowFavoriteProducers);
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
            this.loadUnloadedButton.ForeColor = SystemColors.ControlText;
            this.loadUnloadedButton.Location = new Point(573, 289);
            this.loadUnloadedButton.Name = "loadUnloadedButton";
            this.loadUnloadedButton.Size = new Size(101, 23);
            this.loadUnloadedButton.TabIndex = 35;
            this.loadUnloadedButton.Text = "Load Unloaded";
            this.loadUnloadedButton.UseVisualStyleBackColor = true;
            this.loadUnloadedButton.Click += new EventHandler(this.LoadUnloaded);
            // 
            // reloadFavoriteProducersButton
            // 
            this.reloadFavoriteProducersButton.ForeColor = SystemColors.ControlText;
            this.reloadFavoriteProducersButton.Location = new Point(421, 289);
            this.reloadFavoriteProducersButton.Name = "reloadFavoriteProducersButton";
            this.reloadFavoriteProducersButton.Size = new Size(146, 23);
            this.reloadFavoriteProducersButton.TabIndex = 32;
            this.reloadFavoriteProducersButton.Text = "Update All Producer Titles";
            this.reloadFavoriteProducersButton.UseVisualStyleBackColor = true;
            this.reloadFavoriteProducersButton.Click += new EventHandler(this.UpdateAllFavoriteProducerTitles);
            // 
            // selectedProducersVNButton
            // 
            this.selectedProducersVNButton.ForeColor = SystemColors.ControlText;
            this.selectedProducersVNButton.Location = new Point(421, 260);
            this.selectedProducersVNButton.Name = "selectedProducersVNButton";
            this.selectedProducersVNButton.Size = new Size(146, 23);
            this.selectedProducersVNButton.TabIndex = 33;
            this.selectedProducersVNButton.Text = "Show VNs From Selected";
            this.selectedProducersVNButton.UseVisualStyleBackColor = true;
            this.selectedProducersVNButton.Click += new EventHandler(this.ShowSelectedProducerVNs);
            // 
            // button3
            // 
            this.button3.ForeColor = SystemColors.ControlText;
            this.button3.Location = new Point(573, 260);
            this.button3.Name = "button3";
            this.button3.Size = new Size(101, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "Remove Selected";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new EventHandler(this.RemoveProducers);
            // 
            // yearButton
            // 
            this.yearButton.BackColor = Color.MistyRose;
            this.yearButton.FlatAppearance.BorderSize = 0;
            this.yearButton.FlatStyle = FlatStyle.Flat;
            this.yearButton.ForeColor = SystemColors.ControlText;
            this.yearButton.Location = new Point(378, 311);
            this.yearButton.Name = "yearButton";
            this.yearButton.Size = new Size(29, 20);
            this.yearButton.TabIndex = 46;
            this.yearButton.Text = "Go";
            this.yearButton.UseVisualStyleBackColor = false;
            this.yearButton.Click += new EventHandler(this.GetYearTitles);
            // 
            // replyText
            // 
            this.replyText.Location = new Point(413, 311);
            this.replyText.Name = "replyText";
            this.replyText.Size = new Size(305, 20);
            this.replyText.TabIndex = 28;
            this.replyText.Text = "(replyText)";
            this.replyText.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new Point(226, 314);
            this.label15.Name = "label15";
            this.label15.Size = new Size(102, 13);
            this.label15.TabIndex = 47;
            this.label15.Text = "Load VNs from Year";
            this.label15.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // yearBox
            // 
            this.yearBox.Location = new Point(334, 311);
            this.yearBox.Name = "yearBox";
            this.yearBox.Size = new Size(38, 20);
            this.yearBox.TabIndex = 36;
            this.yearBox.KeyDown += new KeyEventHandler(this.yearBox_KeyDown);
            // 
            // btnFetch
            // 
            this.btnFetch.BackColor = Color.MistyRose;
            this.btnFetch.FlatAppearance.BorderSize = 0;
            this.btnFetch.FlatStyle = FlatStyle.Flat;
            this.btnFetch.ForeColor = SystemColors.ControlText;
            this.btnFetch.Location = new Point(191, 311);
            this.btnFetch.Name = "btnFetch";
            this.btnFetch.Size = new Size(29, 20);
            this.btnFetch.TabIndex = 27;
            this.btnFetch.Text = "Go";
            this.btnFetch.UseVisualStyleBackColor = false;
            this.btnFetch.Click += new EventHandler(this.VNSearchButt);
            // 
            // customFilters
            // 
            this.customFilters.DropDownStyle = ComboBoxStyle.DropDownList;
            this.customFilters.ForeColor = SystemColors.ControlText;
            this.customFilters.FormattingEnabled = true;
            this.customFilters.Items.AddRange(new object[] {
            "Custom Filters",
            "----------"});
            this.customFilters.Location = new Point(447, 336);
            this.customFilters.Name = "customFilters";
            this.customFilters.Size = new Size(121, 21);
            this.customFilters.TabIndex = 57;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(10, 314);
            this.label4.Name = "label4";
            this.label4.Size = new Size(77, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Search For VN";
            this.label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = SystemColors.ControlLightLight;
            this.label14.Location = new Point(690, 341);
            this.label14.Name = "label14";
            this.label14.Size = new Size(50, 13);
            this.label14.TabIndex = 38;
            this.label14.Text = "Producer";
            // 
            // ULStatusDropDown
            // 
            this.ULStatusDropDown.DropDownStyle = ComboBoxStyle.DropDownList;
            this.ULStatusDropDown.ForeColor = SystemColors.ControlText;
            this.ULStatusDropDown.FormattingEnabled = true;
            this.ULStatusDropDown.Items.AddRange(new object[] {
            "UL Status",
            "----------",
            "Unknown",
            "Playing",
            "Finished",
            "Dropped"});
            this.ULStatusDropDown.Location = new Point(320, 336);
            this.ULStatusDropDown.Name = "ULStatusDropDown";
            this.ULStatusDropDown.Size = new Size(121, 21);
            this.ULStatusDropDown.TabIndex = 56;
            // 
            // quickFilter4
            // 
            this.quickFilter4.BackColor = Color.SteelBlue;
            this.quickFilter4.FlatAppearance.BorderSize = 0;
            this.quickFilter4.FlatStyle = FlatStyle.Flat;
            this.quickFilter4.ForeColor = SystemColors.ControlLightLight;
            this.quickFilter4.Location = new Point(203, 336);
            this.quickFilter4.Name = "quickFilter4";
            this.quickFilter4.Size = new Size(111, 23);
            this.quickFilter4.TabIndex = 51;
            this.quickFilter4.Text = "Wishlist Titles";
            this.quickFilter4.UseVisualStyleBackColor = false;
            this.quickFilter4.Click += new EventHandler(this.Filter_Wishlist);
            // 
            // quickFilter0
            // 
            this.quickFilter0.BackColor = Color.SteelBlue;
            this.quickFilter0.FlatAppearance.BorderSize = 0;
            this.quickFilter0.FlatStyle = FlatStyle.Flat;
            this.quickFilter0.ForeColor = SystemColors.ControlLightLight;
            this.quickFilter0.Location = new Point(8, 336);
            this.quickFilter0.Name = "quickFilter0";
            this.quickFilter0.Size = new Size(75, 23);
            this.quickFilter0.TabIndex = 48;
            this.quickFilter0.Text = "All Titles";
            this.quickFilter0.UseVisualStyleBackColor = false;
            this.quickFilter0.Click += new EventHandler(this.Filter_All);
            // 
            // quickFilter1
            // 
            this.quickFilter1.BackColor = Color.SteelBlue;
            this.quickFilter1.FlatAppearance.BorderSize = 0;
            this.quickFilter1.FlatStyle = FlatStyle.Flat;
            this.quickFilter1.ForeColor = SystemColors.ControlLightLight;
            this.quickFilter1.Location = new Point(89, 336);
            this.quickFilter1.Name = "quickFilter1";
            this.quickFilter1.Size = new Size(110, 23);
            this.quickFilter1.TabIndex = 47;
            this.quickFilter1.Text = "Favorite Producers";
            this.quickFilter1.UseVisualStyleBackColor = false;
            this.quickFilter1.Click += new EventHandler(this.Filter_FavoriteProducers);
            // 
            // settingsBox
            // 
            this.settingsBox.BackColor = Color.Gray;
            this.settingsBox.Controls.Add(this.yearLimitBox);
            this.settingsBox.Controls.Add(this.autoUpdateURTBox);
            this.settingsBox.Controls.Add(this.loginButton);
            this.settingsBox.Controls.Add(this.closeAllFormsButton);
            this.settingsBox.Controls.Add(this.nsfwToggle);
            this.settingsBox.Controls.Add(this.userListReply);
            this.settingsBox.Controls.Add(this.userListButt);
            this.settingsBox.Controls.Add(this.loginReply);
            this.settingsBox.ForeColor = SystemColors.ControlLightLight;
            this.settingsBox.Location = new Point(6, 6);
            this.settingsBox.Name = "settingsBox";
            this.settingsBox.Size = new Size(139, 298);
            this.settingsBox.TabIndex = 29;
            this.settingsBox.TabStop = false;
            this.settingsBox.Text = "Settings";
            // 
            // loginButton
            // 
            this.loginButton.ForeColor = SystemColors.ControlText;
            this.loginButton.Location = new Point(7, 46);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new Size(126, 23);
            this.loginButton.TabIndex = 82;
            this.loginButton.Text = "Log In";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new EventHandler(this.LogInDialog);
            // 
            // closeAllFormsButton
            // 
            this.closeAllFormsButton.ForeColor = SystemColors.ControlText;
            this.closeAllFormsButton.Location = new Point(7, 269);
            this.closeAllFormsButton.Name = "closeAllFormsButton";
            this.closeAllFormsButton.Size = new Size(126, 23);
            this.closeAllFormsButton.TabIndex = 81;
            this.closeAllFormsButton.Text = "Close All VN Windows";
            this.closeAllFormsButton.UseVisualStyleBackColor = true;
            this.closeAllFormsButton.Click += new EventHandler(this.CloseAllForms);
            // 
            // nsfwToggle
            // 
            this.nsfwToggle.ForeColor = SystemColors.ControlLightLight;
            this.nsfwToggle.Location = new Point(7, 181);
            this.nsfwToggle.Name = "nsfwToggle";
            this.nsfwToggle.Size = new Size(126, 17);
            this.nsfwToggle.TabIndex = 80;
            this.nsfwToggle.Text = "Show NSFW Images";
            this.nsfwToggle.TextAlign = ContentAlignment.MiddleCenter;
            this.nsfwToggle.UseVisualStyleBackColor = true;
            this.nsfwToggle.Click += new EventHandler(this.ToggleNSFWImages);
            // 
            // userListReply
            // 
            this.userListReply.Anchor = AnchorStyles.Top;
            this.userListReply.Location = new Point(7, 101);
            this.userListReply.Name = "userListReply";
            this.userListReply.Size = new Size(126, 41);
            this.userListReply.TabIndex = 28;
            this.userListReply.Text = "(userListReply)";
            this.userListReply.TextAlign = ContentAlignment.TopCenter;
            // 
            // userListButt
            // 
            this.userListButt.ForeColor = SystemColors.ControlText;
            this.userListButt.Location = new Point(7, 75);
            this.userListButt.Name = "userListButt";
            this.userListButt.Size = new Size(126, 23);
            this.userListButt.TabIndex = 27;
            this.userListButt.Text = "Update List";
            this.userListButt.UseVisualStyleBackColor = true;
            this.userListButt.Click += new EventHandler(this.UpdateURTButtonClick);
            // 
            // loginReply
            // 
            this.loginReply.Anchor = AnchorStyles.Top;
            this.loginReply.ForeColor = SystemColors.ControlLightLight;
            this.loginReply.Location = new Point(6, 16);
            this.loginReply.Name = "loginReply";
            this.loginReply.Size = new Size(127, 27);
            this.loginReply.TabIndex = 30;
            this.loginReply.Text = "(loginReply)";
            this.loginReply.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // resultLabel
            // 
            this.resultLabel.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.resultLabel.ForeColor = SystemColors.ControlLightLight;
            this.resultLabel.Location = new Point(1250, 739);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new Size(152, 13);
            this.resultLabel.TabIndex = 43;
            this.resultLabel.Text = "(resultLabel)";
            this.resultLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // tagFilteringBox
            // 
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
            this.tagFilteringBox.ForeColor = SystemColors.ControlLightLight;
            this.tagFilteringBox.Location = new Point(151, 6);
            this.tagFilteringBox.Name = "tagFilteringBox";
            this.tagFilteringBox.Size = new Size(567, 299);
            this.tagFilteringBox.TabIndex = 61;
            this.tagFilteringBox.TabStop = false;
            this.tagFilteringBox.Text = "Tag Filtering";
            // 
            // filterReply
            // 
            this.filterReply.Location = new Point(461, 99);
            this.filterReply.Name = "filterReply";
            this.filterReply.Size = new Size(100, 155);
            this.filterReply.TabIndex = 49;
            this.filterReply.Text = "(filterReply)";
            this.filterReply.TextAlign = ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new Point(6, 21);
            this.label6.Name = "label6";
            this.label6.Size = new Size(101, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Most Common Tags";
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new Point(6, 46);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(200, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new EventHandler(this.TagFilterAdded);
            // 
            // button5
            // 
            this.button5.ForeColor = SystemColors.ControlText;
            this.button5.Location = new Point(461, 73);
            this.button5.Name = "button5";
            this.button5.Size = new Size(100, 23);
            this.button5.TabIndex = 40;
            this.button5.Text = "Clear Filter";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new EventHandler(this.ClearFilter);
            // 
            // checkBox2
            // 
            this.checkBox2.Location = new Point(6, 68);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new Size(200, 17);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new EventHandler(this.TagFilterAdded);
            // 
            // button2
            // 
            this.button2.ForeColor = SystemColors.ControlText;
            this.button2.Location = new Point(461, 16);
            this.button2.Name = "button2";
            this.button2.Size = new Size(100, 23);
            this.button2.TabIndex = 39;
            this.button2.Text = "Save Filter";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.SaveCustomFilter);
            // 
            // checkBox3
            // 
            this.checkBox3.Location = new Point(6, 91);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new Size(200, 17);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Text = "checkBox3";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new EventHandler(this.TagFilterAdded);
            // 
            // mctLoadingLabel
            // 
            this.mctLoadingLabel.AutoSize = true;
            this.mctLoadingLabel.Location = new Point(6, 270);
            this.mctLoadingLabel.Name = "mctLoadingLabel";
            this.mctLoadingLabel.Size = new Size(94, 13);
            this.mctLoadingLabel.TabIndex = 38;
            this.mctLoadingLabel.Text = "(mctLoadingLabel)";
            // 
            // checkBox6
            // 
            this.checkBox6.Location = new Point(6, 158);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new Size(200, 17);
            this.checkBox6.TabIndex = 3;
            this.checkBox6.Text = "checkBox6";
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new EventHandler(this.TagFilterAdded);
            // 
            // checkBox5
            // 
            this.checkBox5.Location = new Point(6, 135);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new Size(200, 17);
            this.checkBox5.TabIndex = 4;
            this.checkBox5.Text = "checkBox5";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new EventHandler(this.TagFilterAdded);
            // 
            // checkBox4
            // 
            this.checkBox4.Location = new Point(6, 112);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new Size(200, 17);
            this.checkBox4.TabIndex = 5;
            this.checkBox4.Text = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new EventHandler(this.TagFilterAdded);
            // 
            // checkBox9
            // 
            this.checkBox9.Location = new Point(6, 227);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new Size(200, 17);
            this.checkBox9.TabIndex = 6;
            this.checkBox9.Text = "checkBox9";
            this.checkBox9.UseVisualStyleBackColor = true;
            this.checkBox9.CheckedChanged += new EventHandler(this.TagFilterAdded);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new Point(224, 21);
            this.label7.Name = "label7";
            this.label7.Size = new Size(34, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Filters";
            // 
            // checkBox8
            // 
            this.checkBox8.Location = new Point(6, 204);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new Size(200, 17);
            this.checkBox8.TabIndex = 7;
            this.checkBox8.Text = "checkBox8";
            this.checkBox8.UseVisualStyleBackColor = true;
            this.checkBox8.CheckedChanged += new EventHandler(this.TagFilterAdded);
            // 
            // checkBox7
            // 
            this.checkBox7.Location = new Point(6, 181);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new Size(200, 17);
            this.checkBox7.TabIndex = 8;
            this.checkBox7.Text = "checkBox7";
            this.checkBox7.UseVisualStyleBackColor = true;
            this.checkBox7.CheckedChanged += new EventHandler(this.TagFilterAdded);
            // 
            // checkBox10
            // 
            this.checkBox10.Location = new Point(6, 250);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new Size(200, 17);
            this.checkBox10.TabIndex = 9;
            this.checkBox10.Text = "checkBox10";
            this.checkBox10.UseVisualStyleBackColor = true;
            this.checkBox10.CheckedChanged += new EventHandler(this.TagFilterAdded);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.vnTab);
            this.tabControl1.Controls.Add(this.logTab);
            this.tabControl1.Controls.Add(this.infoTab);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(1418, 783);
            this.tabControl1.TabIndex = 0;
            // 
            // vnImages
            // 
            this.vnImages.ColorDepth = ColorDepth.Depth24Bit;
            this.vnImages.ImageSize = new Size(72, 128);
            this.vnImages.TransparentColor = Color.Transparent;
            // 
            // ContextMenuVN
            // 
            this.ContextMenuVN.Items.AddRange(new ToolStripItem[] {
            this.userlistToolStripMenuItem,
            this.wishlistToolStripMenuItem,
            this.voteToolStripMenuItem});
            this.ContextMenuVN.Name = "contextMenuStrip1";
            this.ContextMenuVN.Size = new Size(116, 70);
            // 
            // userlistToolStripMenuItem
            // 
            this.userlistToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.noneToolStripMenuItem,
            this.unknownToolStripMenuItem,
            this.playingToolStripMenuItem,
            this.finishedToolStripMenuItem,
            this.stalledToolStripMenuItem,
            this.droppedToolStripMenuItem});
            this.userlistToolStripMenuItem.Name = "userlistToolStripMenuItem";
            this.userlistToolStripMenuItem.Size = new Size(115, 22);
            this.userlistToolStripMenuItem.Text = "Userlist";
            this.userlistToolStripMenuItem.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.HandleContextItemClicked);
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new Size(125, 22);
            this.noneToolStripMenuItem.Text = "(None)";
            // 
            // unknownToolStripMenuItem
            // 
            this.unknownToolStripMenuItem.Name = "unknownToolStripMenuItem";
            this.unknownToolStripMenuItem.Size = new Size(125, 22);
            this.unknownToolStripMenuItem.Text = "Unknown";
            // 
            // playingToolStripMenuItem
            // 
            this.playingToolStripMenuItem.Name = "playingToolStripMenuItem";
            this.playingToolStripMenuItem.Size = new Size(125, 22);
            this.playingToolStripMenuItem.Text = "Playing";
            // 
            // finishedToolStripMenuItem
            // 
            this.finishedToolStripMenuItem.Name = "finishedToolStripMenuItem";
            this.finishedToolStripMenuItem.Size = new Size(125, 22);
            this.finishedToolStripMenuItem.Text = "Finished";
            // 
            // stalledToolStripMenuItem
            // 
            this.stalledToolStripMenuItem.Name = "stalledToolStripMenuItem";
            this.stalledToolStripMenuItem.Size = new Size(125, 22);
            this.stalledToolStripMenuItem.Text = "Stalled";
            // 
            // droppedToolStripMenuItem
            // 
            this.droppedToolStripMenuItem.Name = "droppedToolStripMenuItem";
            this.droppedToolStripMenuItem.Size = new Size(125, 22);
            this.droppedToolStripMenuItem.Text = "Dropped";
            // 
            // wishlistToolStripMenuItem
            // 
            this.wishlistToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.noneToolStripMenuItem1,
            this.highToolStripMenuItem,
            this.mediumToolStripMenuItem,
            this.lowToolStripMenuItem,
            this.blacklistToolStripMenuItem});
            this.wishlistToolStripMenuItem.Name = "wishlistToolStripMenuItem";
            this.wishlistToolStripMenuItem.Size = new Size(115, 22);
            this.wishlistToolStripMenuItem.Text = "Wishlist";
            this.wishlistToolStripMenuItem.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.HandleContextItemClicked);
            // 
            // noneToolStripMenuItem1
            // 
            this.noneToolStripMenuItem1.Name = "noneToolStripMenuItem1";
            this.noneToolStripMenuItem1.Size = new Size(119, 22);
            this.noneToolStripMenuItem1.Text = "(None)";
            // 
            // highToolStripMenuItem
            // 
            this.highToolStripMenuItem.Name = "highToolStripMenuItem";
            this.highToolStripMenuItem.Size = new Size(119, 22);
            this.highToolStripMenuItem.Text = "High";
            // 
            // mediumToolStripMenuItem
            // 
            this.mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
            this.mediumToolStripMenuItem.Size = new Size(119, 22);
            this.mediumToolStripMenuItem.Text = "Medium";
            // 
            // lowToolStripMenuItem
            // 
            this.lowToolStripMenuItem.Name = "lowToolStripMenuItem";
            this.lowToolStripMenuItem.Size = new Size(119, 22);
            this.lowToolStripMenuItem.Text = "Low";
            // 
            // blacklistToolStripMenuItem
            // 
            this.blacklistToolStripMenuItem.Name = "blacklistToolStripMenuItem";
            this.blacklistToolStripMenuItem.Size = new Size(119, 22);
            this.blacklistToolStripMenuItem.Text = "Blacklist";
            // 
            // voteToolStripMenuItem
            // 
            this.voteToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
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
            this.voteToolStripMenuItem.Size = new Size(115, 22);
            this.voteToolStripMenuItem.Text = "Vote";
            this.voteToolStripMenuItem.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.HandleContextItemClicked);
            // 
            // noneToolStripMenuItem2
            // 
            this.noneToolStripMenuItem2.Name = "noneToolStripMenuItem2";
            this.noneToolStripMenuItem2.Size = new Size(111, 22);
            this.noneToolStripMenuItem2.Text = "(None)";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new Size(111, 22);
            this.toolStripMenuItem2.Text = "1";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new Size(111, 22);
            this.toolStripMenuItem3.Text = "2";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new Size(111, 22);
            this.toolStripMenuItem4.Text = "3";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new Size(111, 22);
            this.toolStripMenuItem5.Text = "4";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new Size(111, 22);
            this.toolStripMenuItem6.Text = "5";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new Size(111, 22);
            this.toolStripMenuItem7.Text = "6";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new Size(111, 22);
            this.toolStripMenuItem8.Text = "7";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new Size(111, 22);
            this.toolStripMenuItem9.Text = "8";
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new Size(111, 22);
            this.toolStripMenuItem10.Text = "9";
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new Size(111, 22);
            this.toolStripMenuItem11.Text = "10";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.ClientSize = new Size(1418, 783);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.Location = new Point(50, 50);
            this.MinimumSize = new Size(1280, 720);
            this.Name = "FormMain";
            this.Text = "Happy Search By Zolty";
            this.Activated += new EventHandler(this.FormMain_Enter);
            this.infoTab.ResumeLayout(false);
            this.statBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.logTab.ResumeLayout(false);
            this.logTab.PerformLayout();
            this.vnTab.ResumeLayout(false);
            this.vnTab.PerformLayout();
            ((ISupportInitialize)(this.tileOLV)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((ISupportInitialize)(this.olFavoriteProducers)).EndInit();
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
        private GroupBox groupBox10;
        private RichTextBox richTextBox2;
        private GroupBox groupBox9;
        private RichTextBox richTextBox1;
        private TabPage logTab;
        private Label logQueryLabel;
        private Label logAskLabel;
        private Label logReplyLabel;
        public RichTextBox serverQ;
        private RichTextBox questionBox;
        public RichTextBox serverR;
        private Button button1;
        private TabPage vnTab;
        private Button yearButton;
        internal Label loginReply;
        private GroupBox settingsBox;
        private Label userListReply;
        private Button userListButt;
        private Label resultLabel;
        private Button btnFetch;
        private TextBox searchText;
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
    }
}

