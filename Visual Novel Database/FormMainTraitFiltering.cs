using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Happy_Search
{
    partial class FormMain
    {
        private const string TraitLabel = "traitFilterLabel";
        private Func<ListedVN,bool> _traitFunction = x => true;

        /// <summary>
        /// Display or clear list of active trait filters.
        /// </summary>
        /// <param name="clear">Should list be cleared?</param>
        private void DisplayFilterTraits(bool clear = false)
        {
            //clear old labels
            var oldCount = 0;
            var oldLabel = (CheckBox)Controls.Find(TraitLabel + 0, true).FirstOrDefault();
            while (oldLabel != null)
            {
                oldLabel.Dispose();
                oldCount++;
                oldLabel = (CheckBox)Controls.Find(TraitLabel + oldCount, true).FirstOrDefault();
            }
            if (clear || _activeTraitFilter.Count == 0)
            {
                _activeTraitFilter = new List<WrittenTrait>();
                _traitFunction = x => true;
                ApplyListFilters();
                return;
            }
            //add labels
            var count = 0;
            foreach (var trait in _activeTraitFilter)
            {
                var traitLabel = new CheckBox
                {
                    AutoSize = false,
                    Location = new Point(6, 33 + count * 22),
                    Name = TraitLabel + count,
                    Size = new Size(200, 17),
                    Text = $"{trait.Print()}",
                    Checked = true,
                    AutoEllipsis = true
                };

                traitLabel.CheckedChanged += TraitFilterRemoved;
                count++;
                traitFilteringBox.Controls.Add(traitLabel);
            }
            
            IEnumerable<CharacterItem> traitCharacters = _characterList.Where(x => x.ContainsTraits(_activeTraitFilter.Select(trait => trait.ID)));
            var characterVNs = new List<int>();
            foreach (var characterItem in traitCharacters)
            {
                characterVNs.AddRange(characterItem.VNs.Select(x => x.ID));
            }
            _traitFunction = x => characterVNs.Contains(x.VNID);
            ApplyListFilters();
        }
        
        private void TraitFilterRemoved(object sender, EventArgs eventArgs)
        {
            var checkbox = (CheckBox)sender;
            if (checkbox.Checked)
            {
                //shouldnt happen
                return;
            }
            var filterNo = Convert.ToInt32(checkbox.Name.Remove(0, TraitLabel.Length));
            _activeTraitFilter.RemoveAt(filterNo);
            DisplayFilterTraits();
        }
        
        private void TraitRootChanged(object sender, EventArgs e)
        {
            if (traitRootsDropdown.SelectedIndex < 0) return;
            var trait = PlainTraits.Find(x => x.Name.Equals(traitRootsDropdown.SelectedItem));
            if (trait == null)
            {
                WriteError(traitReply,"Root trait not found.", true);
                return;
            }
            var traitSource = new AutoCompleteStringCollection();
            traitSource.AddRange(PlainTraits.Where(x=>x.TopmostParent==trait.ID).Select(x=>x.Name).ToArray());
            traitSearchBox.AutoCompleteCustomSource = traitSource;
        }

        private void traitSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (traitSearchBox.Text == "") //check if box is empty
            {
                WriteError(traitReply, "Enter trait ID.", true);
                return;
            }
            var traitName = traitSearchBox.Text;
            var root = PlainTraits.Find(x => x.Name.Equals(traitRootsDropdown.SelectedItem));
            var trait = PlainTraits.Find(x => x.Name.Equals(traitName, StringComparison.InvariantCultureIgnoreCase) && x.TopmostParent == root.ID);
            if (trait == null)
            {
                WriteError(traitReply, $"{root} > {traitName} not found.", true);
                return;
            }
            _activeTraitFilter.Add(trait);
            DisplayFilterTraits();
            var s = (Control)sender;
            s.Text = "";
            WriteText(traitReply, $"Filtered by {trait.Print()}", true);
        }

    }
}
