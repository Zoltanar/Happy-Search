using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
