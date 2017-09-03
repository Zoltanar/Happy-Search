using System.Text;
using static Happy_Apps_Core.StaticHelpers;
using System.Windows.Forms;
using Happy_Search.Properties;

namespace Happy_Search.Other_Forms
{

    /// <summary>
    /// Dialog Box that allows user to input a string.
    /// </summary>
    public sealed partial class InputDialogBox : Form
    {
        private readonly StringBuilder _inputText;
        private readonly bool _preciseVote;

        /// <summary>
        /// Display a dialog box where the user can input a string. Returns OK result if successful.
        /// </summary>
        /// <param name="inputText">Possibly empty string which will be shown in input box and will become what the user has entered if they confirm it</param>
        /// <param name="windowTitle">The window title of the dialog box</param>
        /// <param name="question">The text that the user is presented with, in regards to the text that they will input</param>
        /// <param name="preciseVote">Indicates that input must be a valid vote (from 1.00 to 10.00)</param>
        public InputDialogBox(StringBuilder inputText, string windowTitle, string question, bool preciseVote = false)
        {
            InitializeComponent();
            _inputText = inputText;
            _preciseVote = preciseVote;
            replyLabel.Text = preciseVote ? "Between 1 and 10, 1 decimal place allowed." : "";
            Text = $@"{windowTitle} - Happy Search";
            questionLabel.Text = question;
            answerBox.Text = _inputText.ToString();
        }


        private void confirmButton_Click(object sender, System.EventArgs e)
        {
            if (_preciseVote)
            {
                if (!double.TryParse(answerBox.Text, out var inputDouble))
                {
                    WriteError(replyLabel, Resources.valid_vote_error);
                    return;
                }
                if (inputDouble < 1 || inputDouble > 10)
                {
                    WriteError(replyLabel, Resources.valid_vote_error);
                    return;
                }
            }
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
