namespace Happy_Search
{
    partial class HtmlForm
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
            this.htmlBox = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // htmlBox
            // 
            this.htmlBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htmlBox.Location = new System.Drawing.Point(0, 0);
            this.htmlBox.MinimumSize = new System.Drawing.Size(20, 20);
            this.htmlBox.Name = "htmlBox";
            this.htmlBox.Size = new System.Drawing.Size(444, 459);
            this.htmlBox.TabIndex = 0;
            // 
            // HtmlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 459);
            this.Controls.Add(this.htmlBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "HtmlForm";
            this.Text = "Help";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser htmlBox;
    }
}