using System.Windows.Forms;

namespace Happy_Search
{
    public partial class HtmlForm : Form
    {
        public HtmlForm(string urlString)
        {
            InitializeComponent();
            htmlBox.Navigate(urlString);
        }
    }
}
