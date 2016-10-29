using System.Windows.Forms;

namespace Happy_Search.Other_Forms
{
    /// <summary>
    /// Form for showing a HTML document.
    /// </summary>
    public partial class HtmlForm : Form
    {
        /// <summary>
        /// Load HTML file in a new form.
        /// </summary>
        /// <param name="urlString">URL of HTML file</param>
        public HtmlForm(string urlString)
        {
            InitializeComponent();
            htmlBox.Navigate(urlString);
        }
    }
}
