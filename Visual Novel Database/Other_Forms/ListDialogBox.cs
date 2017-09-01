using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Happy_Search.Other_Forms
{
    /// <summary>
    /// Dialog Box that displays a list of strings and allows user to manipulate the list.
    /// </summary>
    public sealed partial class ListDialogBox : Form
    {
        private readonly List<string> _stringList;

        /// <summary>
        /// Show a dialog box with a group of string that the user can manipulate.
        /// </summary>
        /// <param name="stringList">List of string to be used</param>
        /// <param name="windowTitle">Title of the window</param>
        /// <param name="listLabel">The label for the list</param>
        public ListDialogBox(List<string> stringList, string windowTitle, string listLabel)
        {
            InitializeComponent();
            replyLabel.Text = "";
            Text = $@"{windowTitle} - Happy Search";
            listViewLabel.Text = listLabel;
            _stringList = stringList;
            var items = _stringList.Select(stringItem => new ListViewItem(stringItem)).ToArray();
            listView.Items.AddRange(items);
        }


        private void addButton_Click(object sender, System.EventArgs e)
        {
            if (inputBox.Text.Equals("")) return;
            listView.Items.Add(inputBox.Text);
            inputBox.Text = "";
        }

        private void removeAllButton_Click(object sender, System.EventArgs e)
        {
            listView.Items.Clear();
            replyLabel.Text = @"All items were removed.";
        }

        private void removeSelectedButton_Click(object sender, System.EventArgs e)
        {
            foreach (ListViewItem item in listView.SelectedItems)
            {
                listView.Items.Remove(item);
            }
            replyLabel.Text = @"Selected items were removed.";
        }


        private void confirmButton_Click(object sender, System.EventArgs e)
        {
            _stringList.Clear();
            foreach (ListViewItem listViewItem in listView.Items)
            {
                _stringList.Add(listViewItem.Text);
            }
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void inputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) addButton_Click(null, null);
        }
    }
}
