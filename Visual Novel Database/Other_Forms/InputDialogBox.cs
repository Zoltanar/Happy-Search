using System.Text;
using System.Windows.Forms;

namespace Happy_Search.Other_Forms
{

    /// <summary>
    /// Dialog Box that allows user to input a string.
    /// </summary>
    public sealed partial class InputDialogBox : Form
    {
        private readonly StringBuilder _inputText;

        /// <summary>
        /// Display a dialog box where the user can input a string.
        /// </summary>
        /// <param name="inputText">Possibly empty string which will be shown in input box and will become what the user has entered if they confirm it</param>
        /// <param name="windowTitle">The window title of the dialog box</param>
        /// <param name="question">The text that the user is presented with, in regards to the text that they will input</param>
        public InputDialogBox(StringBuilder inputText, string windowTitle, string question)
        {
            InitializeComponent();
            _inputText = inputText;
            Text = $@"{windowTitle} - Happy Search";
            questionLabel.Text = question;
            answerBox.Text = _inputText.ToString();
        }


        private void confirmButton_Click(object sender, System.EventArgs e)
        {
            _inputText.Clear();
            _inputText.Append(answerBox.Text);
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void answerBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) confirmButton_Click(null,null);
        }
    }
}
