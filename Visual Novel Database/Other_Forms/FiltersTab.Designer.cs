namespace Happy_Search.Other_Forms
{
    partial class FiltersTab
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tagOrTraitSearchButton = new System.Windows.Forms.Button();
            this.tagOrTraitSearchBox = new System.Windows.Forms.TextBox();
            this.traitRootsDropdown = new System.Windows.Forms.ComboBox();
            this.tagOrTraitSearchResultBox = new System.Windows.Forms.ListBox();
            this.label20 = new System.Windows.Forms.Label();
            this.customFilterNameBox = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.customFilterReply = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.filterDropdown = new System.Windows.Forms.ComboBox();
            this.permanentAndListbox = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.orGroupCheckbox = new System.Windows.Forms.CheckBox();
            this.tagOrTraitReply = new System.Windows.Forms.Label();
            this.button12 = new System.Windows.Forms.Button();
            this.excludeFilterCheckbox = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.filterValueCombobox = new System.Windows.Forms.ComboBox();
            this.filterTypeCombobox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.permanentOrListbox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.customAndListbox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.customOrListbox = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tagOrTraitSearchButton
            // 
            this.tagOrTraitSearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tagOrTraitSearchButton.BackColor = System.Drawing.Color.SteelBlue;
            this.tagOrTraitSearchButton.FlatAppearance.BorderSize = 0;
            this.tagOrTraitSearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tagOrTraitSearchButton.ForeColor = System.Drawing.Color.White;
            this.tagOrTraitSearchButton.Location = new System.Drawing.Point(265, 67);
            this.tagOrTraitSearchButton.Name = "tagOrTraitSearchButton";
            this.tagOrTraitSearchButton.Size = new System.Drawing.Size(17, 22);
            this.tagOrTraitSearchButton.TabIndex = 98;
            this.tagOrTraitSearchButton.Text = "S";
            this.tagOrTraitSearchButton.UseVisualStyleBackColor = false;
            this.tagOrTraitSearchButton.Click += new System.EventHandler(this.SearchTagsOrTraits);
            // 
            // tagOrTraitSearchBox
            // 
            this.tagOrTraitSearchBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagOrTraitSearchBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tagOrTraitSearchBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tagOrTraitSearchBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagOrTraitSearchBox.ForeColor = System.Drawing.Color.Black;
            this.tagOrTraitSearchBox.Location = new System.Drawing.Point(107, 67);
            this.tagOrTraitSearchBox.Name = "tagOrTraitSearchBox";
            this.tagOrTraitSearchBox.Size = new System.Drawing.Size(152, 22);
            this.tagOrTraitSearchBox.TabIndex = 95;
            this.tagOrTraitSearchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddTagOrTraitBySearch);
            // 
            // traitRootsDropdown
            // 
            this.traitRootsDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.traitRootsDropdown.ForeColor = System.Drawing.Color.Black;
            this.traitRootsDropdown.FormattingEnabled = true;
            this.traitRootsDropdown.Location = new System.Drawing.Point(9, 67);
            this.traitRootsDropdown.Name = "traitRootsDropdown";
            this.traitRootsDropdown.Size = new System.Drawing.Size(92, 21);
            this.traitRootsDropdown.TabIndex = 0;
            this.traitRootsDropdown.SelectedIndexChanged += new System.EventHandler(this.TraitRootChanged);
            // 
            // tagOrTraitSearchResultBox
            // 
            this.tagOrTraitSearchResultBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagOrTraitSearchResultBox.ForeColor = System.Drawing.Color.Black;
            this.tagOrTraitSearchResultBox.FormattingEnabled = true;
            this.tagOrTraitSearchResultBox.Location = new System.Drawing.Point(107, 88);
            this.tagOrTraitSearchResultBox.Name = "tagOrTraitSearchResultBox";
            this.tagOrTraitSearchResultBox.Size = new System.Drawing.Size(152, 121);
            this.tagOrTraitSearchResultBox.TabIndex = 97;
            this.tagOrTraitSearchResultBox.Visible = false;
            this.tagOrTraitSearchResultBox.SelectedIndexChanged += new System.EventHandler(this.AddTagOrTraitFromList);
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(672, 283);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(100, 13);
            this.label20.TabIndex = 98;
            this.label20.Text = "Filter Name";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // customFilterNameBox
            // 
            this.customFilterNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.customFilterNameBox.Location = new System.Drawing.Point(672, 299);
            this.customFilterNameBox.Name = "customFilterNameBox";
            this.customFilterNameBox.Size = new System.Drawing.Size(100, 20);
            this.customFilterNameBox.TabIndex = 99;
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.Location = new System.Drawing.Point(672, 325);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(100, 21);
            this.button5.TabIndex = 100;
            this.button5.Text = "Save Filter";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.SaveCustomFilter);
            // 
            // customFilterReply
            // 
            this.customFilterReply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.customFilterReply.ForeColor = System.Drawing.Color.White;
            this.customFilterReply.Location = new System.Drawing.Point(672, 349);
            this.customFilterReply.Name = "customFilterReply";
            this.customFilterReply.Size = new System.Drawing.Size(100, 40);
            this.customFilterReply.TabIndex = 101;
            this.customFilterReply.Text = "(customFilterReply)";
            this.customFilterReply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.BackColor = System.Drawing.Color.Khaki;
            this.button8.FlatAppearance.BorderSize = 0;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.ForeColor = System.Drawing.Color.Black;
            this.button8.Location = new System.Drawing.Point(489, 299);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(71, 21);
            this.button8.TabIndex = 108;
            this.button8.Text = "Filters";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.Help_Filters);
            // 
            // filterDropdown
            // 
            this.filterDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.filterDropdown.BackColor = System.Drawing.Color.SteelBlue;
            this.filterDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterDropdown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.filterDropdown.ForeColor = System.Drawing.Color.White;
            this.filterDropdown.FormattingEnabled = true;
            this.filterDropdown.Location = new System.Drawing.Point(566, 299);
            this.filterDropdown.Name = "filterDropdown";
            this.filterDropdown.Size = new System.Drawing.Size(100, 21);
            this.filterDropdown.TabIndex = 108;
            this.filterDropdown.SelectedIndexChanged += new System.EventHandler(this.FilterChanged);
            // 
            // permanentAndListbox
            // 
            this.permanentAndListbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.permanentAndListbox.FormattingEnabled = true;
            this.permanentAndListbox.Location = new System.Drawing.Point(5, 37);
            this.permanentAndListbox.Name = "permanentAndListbox";
            this.permanentAndListbox.Size = new System.Drawing.Size(226, 186);
            this.permanentAndListbox.TabIndex = 109;
            this.permanentAndListbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RemoveFromListBox);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.orGroupCheckbox);
            this.groupBox1.Controls.Add(this.tagOrTraitReply);
            this.groupBox1.Controls.Add(this.button12);
            this.groupBox1.Controls.Add(this.traitRootsDropdown);
            this.groupBox1.Controls.Add(this.excludeFilterCheckbox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.filterValueCombobox);
            this.groupBox1.Controls.Add(this.filterTypeCombobox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tagOrTraitSearchBox);
            this.groupBox1.Controls.Add(this.tagOrTraitSearchResultBox);
            this.groupBox1.Controls.Add(this.tagOrTraitSearchButton);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Location = new System.Drawing.Point(490, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 277);
            this.groupBox1.TabIndex = 114;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add Filter";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.SteelBlue;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(231, 248);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(51, 23);
            this.button1.TabIndex = 117;
            this.button1.Text = "Add Filter";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.AddFilterClick);
            // 
            // orGroupCheckbox
            // 
            this.orGroupCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.orGroupCheckbox.AutoSize = true;
            this.orGroupCheckbox.Location = new System.Drawing.Point(6, 229);
            this.orGroupCheckbox.Name = "orGroupCheckbox";
            this.orGroupCheckbox.Size = new System.Drawing.Size(74, 17);
            this.orGroupCheckbox.TabIndex = 116;
            this.orGroupCheckbox.Text = "OR Group";
            this.orGroupCheckbox.UseVisualStyleBackColor = true;
            // 
            // tagOrTraitReply
            // 
            this.tagOrTraitReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagOrTraitReply.ForeColor = System.Drawing.Color.White;
            this.tagOrTraitReply.Location = new System.Drawing.Point(107, 222);
            this.tagOrTraitReply.Name = "tagOrTraitReply";
            this.tagOrTraitReply.Size = new System.Drawing.Size(175, 21);
            this.tagOrTraitReply.TabIndex = 115;
            this.tagOrTraitReply.Text = "(tagOrTraitReply)";
            this.tagOrTraitReply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button12
            // 
            this.button12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button12.BackColor = System.Drawing.Color.SteelBlue;
            this.button12.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button12.ForeColor = System.Drawing.Color.White;
            this.button12.Location = new System.Drawing.Point(107, 248);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(118, 23);
            this.button12.TabIndex = 6;
            this.button12.Text = "Add Permanent Filter";
            this.button12.UseVisualStyleBackColor = false;
            this.button12.Click += new System.EventHandler(this.AddPermanentFilterClick);
            // 
            // excludeFilterCheckbox
            // 
            this.excludeFilterCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.excludeFilterCheckbox.AutoSize = true;
            this.excludeFilterCheckbox.Location = new System.Drawing.Point(6, 252);
            this.excludeFilterCheckbox.Name = "excludeFilterCheckbox";
            this.excludeFilterCheckbox.Size = new System.Drawing.Size(64, 17);
            this.excludeFilterCheckbox.TabIndex = 5;
            this.excludeFilterCheckbox.Text = "Exclude";
            this.excludeFilterCheckbox.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Value";
            // 
            // filterValueCombobox
            // 
            this.filterValueCombobox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterValueCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterValueCombobox.FormattingEnabled = true;
            this.filterValueCombobox.Location = new System.Drawing.Point(107, 40);
            this.filterValueCombobox.Name = "filterValueCombobox";
            this.filterValueCombobox.Size = new System.Drawing.Size(175, 21);
            this.filterValueCombobox.TabIndex = 2;
            // 
            // filterTypeCombobox
            // 
            this.filterTypeCombobox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterTypeCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterTypeCombobox.FormattingEnabled = true;
            this.filterTypeCombobox.Location = new System.Drawing.Point(107, 13);
            this.filterTypeCombobox.Name = "filterTypeCombobox";
            this.filterTypeCombobox.Size = new System.Drawing.Size(175, 21);
            this.filterTypeCombobox.TabIndex = 1;
            this.filterTypeCombobox.SelectedIndexChanged += new System.EventHandler(this.FilterTypeChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Type";
            // 
            // permanentOrListbox
            // 
            this.permanentOrListbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.permanentOrListbox.FormattingEnabled = true;
            this.permanentOrListbox.Location = new System.Drawing.Point(5, 258);
            this.permanentOrListbox.Name = "permanentOrListbox";
            this.permanentOrListbox.Size = new System.Drawing.Size(226, 121);
            this.permanentOrListbox.TabIndex = 115;
            this.permanentOrListbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RemoveFromListBox);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(6, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(182, 13);
            this.label2.TabIndex = 117;
            this.label2.Text = "AND Group (All of these are required)";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label6.Location = new System.Drawing.Point(6, 242);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(176, 13);
            this.label6.TabIndex = 118;
            this.label6.Text = "OR Group (One of these is required)";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.permanentAndListbox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.permanentOrListbox);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(237, 388);
            this.groupBox2.TabIndex = 119;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Permanent Filters";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.customAndListbox);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.customOrListbox);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(246, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(237, 388);
            this.groupBox3.TabIndex = 120;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Custom Filter";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(6, 242);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 13);
            this.label1.TabIndex = 118;
            this.label1.Text = "OR Group (One of these is required)";
            // 
            // customAndListbox
            // 
            this.customAndListbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.customAndListbox.FormattingEnabled = true;
            this.customAndListbox.Location = new System.Drawing.Point(5, 37);
            this.customAndListbox.Name = "customAndListbox";
            this.customAndListbox.Size = new System.Drawing.Size(226, 186);
            this.customAndListbox.TabIndex = 109;
            this.customAndListbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RemoveFromListBox);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(6, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(182, 13);
            this.label3.TabIndex = 117;
            this.label3.Text = "AND Group (All of these are required)";
            // 
            // customOrListbox
            // 
            this.customOrListbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customOrListbox.FormattingEnabled = true;
            this.customOrListbox.Location = new System.Drawing.Point(5, 258);
            this.customOrListbox.Name = "customOrListbox";
            this.customOrListbox.Size = new System.Drawing.Size(226, 121);
            this.customOrListbox.TabIndex = 115;
            this.customOrListbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RemoveFromListBox);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(566, 325);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 21);
            this.button2.TabIndex = 121;
            this.button2.Text = "Delete Filter";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.DeleteFilterClick);
            // 
            // FiltersTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.filterDropdown);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.customFilterReply);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.customFilterNameBox);
            this.Controls.Add(this.label20);
            this.Name = "FiltersTab";
            this.Size = new System.Drawing.Size(784, 401);
            this.Click += new System.EventHandler(this.ClearTagOrTraitResults);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button tagOrTraitSearchButton;
        private System.Windows.Forms.TextBox tagOrTraitSearchBox;
        private System.Windows.Forms.ListBox tagOrTraitSearchResultBox;
        private System.Windows.Forms.ComboBox traitRootsDropdown;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox customFilterNameBox;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label customFilterReply;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.ComboBox filterDropdown;
        private System.Windows.Forms.ListBox permanentAndListbox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.CheckBox excludeFilterCheckbox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox filterTypeCombobox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox filterValueCombobox;
        private System.Windows.Forms.Label tagOrTraitReply;
        private System.Windows.Forms.ListBox permanentOrListbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox customAndListbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox customOrListbox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox orGroupCheckbox;
        private System.Windows.Forms.Button button2;
    }
}
