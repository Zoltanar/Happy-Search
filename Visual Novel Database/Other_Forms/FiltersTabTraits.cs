using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Happy_Search.Properties;
using static Happy_Search.StaticHelpers;

namespace Happy_Search.Other_Forms
{
    partial class FiltersTab
    {
        /// <summary>
        /// List of saved custom trait filters.
        /// </summary>
        private readonly BindingList<CustomTraitFilter> _customTraitFilters = new BindingList<CustomTraitFilter>();
        /// <summary>
        /// Contains traits in TraitsTFLB (not active traits)
        /// </summary>
        public readonly BindingList<WrittenTrait> TraitsPre = new BindingList<WrittenTrait>();

        /// <summary>
        ///     Bring up dialog explaining features of the 'Trait Filtering' section.
        /// </summary>
        private void Help_TraitFiltering(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            Debug.Assert(path != null, "path != null");
            var helpFile = $"{Path.Combine(path, "Program Data\\Help\\traitfiltering.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

        /// <summary>
        ///     Delete custom trait filter.
        /// </summary>
        private void DeleteCustomTraitFilter(object sender, EventArgs e)
        {
            if (traitFiltersCB.SelectedIndex < 1) return; //shouldnt happen
            var askBox = MessageBox.Show(Resources.are_you_sure, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            var selectedFilter = traitFiltersCB.SelectedItem as CustomTraitFilter;
            if (selectedFilter == null) return; //shouldnt happen
            _customTraitFilters.Remove(selectedFilter);
            SaveObjectToJsonFile(_customTraitFilters,CustomTraitFiltersJson);
            WriteText(traitReply, Resources.filter_deleted);
            customTraitFilterNameBox.Text = "";
            traitFiltersCB.SelectedIndex = 0;
        }

        /// <summary>
        ///     Display VNs matching traits in selected custom filter.
        /// </summary>
        private void Filter_CustomTraits(object sender, EventArgs e)
        {
            if (_mainForm.DontTriggerEvent) return;
            var selectedItem = traitFiltersCB.SelectedItem as CustomTraitFilter;
            if (selectedItem == null || selectedItem.Name == "Select Filter")
            {
                customTraitFilterNameBox.Text = "";
                deleteCustomTraitFilterButton.Enabled = false;
                return;
            }
            deleteCustomTraitFilterButton.Enabled = true;
            customTraitFilterNameBox.Text = selectedItem.Name;
            TraitsPre.Clear();
            TraitsPre.AddRange(selectedItem.Filters.ToArray());
        }

        /// <summary>
        ///     Save list of active trait filters to file as a custom filter with user-entered name.
        /// </summary>
        private void SaveCustomTraitFilter(object sender, EventArgs e)
        {
            var filterName = customTraitFilterNameBox.Text;
            if (filterName.Length == 0)
            {
                WriteText(traitReply, "Enter name of filter.");
                return;
            }
            //Ask to overwrite if name entered is already in use
            var customFilter = _customTraitFilters.FirstOrDefault(x => x.Name.Equals(filterName));
            if (customFilter != null)
            {
                var askBox = MessageBox.Show(@"Do you wish to overwrite present custom filter?",
                    Resources.ask_overwrite,
                    MessageBoxButtons.YesNo);
                if (askBox != DialogResult.Yes) return;
                customFilter.Filters = TraitsPre.ToArray();
                customFilter.Updated = DateTime.UtcNow;
                _mainForm.DontTriggerEvent = true;
                traitFiltersCB.SelectedIndex = traitFiltersCB.Items.IndexOf(filterName);
                _mainForm.DontTriggerEvent = false;
            }
            else
            {
                _mainForm.DontTriggerEvent = true;
                _customTraitFilters.Add(new CustomTraitFilter(filterName, TraitsPre.ToArray()));
                traitFiltersCB.SelectedIndex = _customTraitFilters.Count - 1;
                _mainForm.DontTriggerEvent = false;
            }
            SaveObjectToJsonFile(_customTraitFilters,CustomTraitFiltersJson);
            WriteText(traitReply, Resources.filter_saved);
        }

        private void EnterCustomTraitFilterName(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SaveCustomTraitFilter(sender, e);
        }

        /// <summary>
        ///     Clear list of active trait filters.
        /// </summary>
        private void ClearTraitFilter(object sender, EventArgs e)
        {
            traitFiltersCB.SelectedIndex = 0;
            TraitsPre.Clear();
            WriteText(traitReply, "Trait filter cleared.");
        }

        /// <summary>
        ///     Change selected trait type.
        /// </summary>
        private void TraitRootChanged(object sender, EventArgs e)
        {
            if (traitRootsDropdown.SelectedIndex < 0) return;
            var trait = FormMain.PlainTraits.Find(x => x.Name.Equals(traitRootsDropdown.SelectedItem));
            if (trait == null)
            {
                WriteError(traitReply, "Root trait not found.");
                return;
            }
            var traitSource = new AutoCompleteStringCollection();
            traitSource.AddRange(FormMain.PlainTraits.Where(x => x.TopmostParent == trait.ID).Select(x => x.Name).ToArray());
            traitSearchBox.AutoCompleteCustomSource = traitSource;
        }

        /// <summary>
        ///     Add trait with name entered by user under selected trait type.
        /// </summary>
        private void AddTraitBySearch(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            traitSearchResultBox.Visible = false;
            if (traitSearchBox.Text == "") //check if box is empty
            {
                WriteError(traitReply, "Enter trait name.");
                return;
            }
            var traitName = traitSearchBox.Text;
            var root = FormMain.PlainTraits.Find(x => x.Name.Equals(traitRootsDropdown.SelectedItem));
            var trait =
                FormMain.PlainTraits.Find(x =>
                    x.Name.Equals(traitName, StringComparison.InvariantCultureIgnoreCase) &&
                    x.TopmostParent == root.ID);
            if (trait == null)
            {
                SearchTraits();
                return;
            }
            AddFilterTrait(trait);
            var s = (Control)sender;
            s.Text = "";
            WriteText(traitReply, $"Added trait {trait}");
        }

        /// <summary>
        /// Add trait to active filter.
        /// </summary>
        /// <param name="trait"></param>
        private void AddFilterTrait(WrittenTrait trait)
        {
            if (trait == null)
            {
                WriteError(traitReply, "Trait not found.");
                return;
            }
            if (TraitsPre.Contains(trait))
            {
                WriteError(traitReply, "Trait is already in filter.");
                return;
            }
            TraitsPre.Add(trait);
        }

        /// <summary>
        /// Search for traits by name/alias.
        /// </summary>
        private void SearchTraits()
        {
            traitSearchResultBox.Visible = false;
            if (traitSearchBox.Text == "") //check if box is empty
            {
                WriteError(traitReply, "Enter trait name.");
                return;
            }
            var text = traitSearchBox.Text.ToLowerInvariant();
            var results = FormMain.PlainTraits.Where(t => t.Name.ToLowerInvariant().Contains(text) ||
                                                 t.Aliases.Exists(a => a.ToLowerInvariant().Contains(text))).ToArray();
            if (results.Length == 0)
            {
                WriteError(traitReply, "No traits with that name/alias found.");
                return;
            }
            if (results.Length == 1)
            {
                traitSearchBox.Text = "";
                WriteText(traitReply, $"Added trait {results.First()}");
                AddFilterTrait(results.First());
                return;
            }
            traitSearchResultBox.Items.Clear();
            // ReSharper disable once CoVariantArrayConversion
            traitSearchResultBox.Items.AddRange(results);
            traitSearchResultBox.Visible = true;
        }

        /// <summary>
        /// Add trait from search results.
        /// </summary>
        private void AddTraitFromList(object sender, EventArgs e)
        {
            var lb = (ListBox)sender;
            traitSearchBox.Text = "";
            lb.Visible = false;
            AddFilterTrait(lb.SelectedItem as WrittenTrait);
        }

        /// <summary>
        /// Clear results list from view.
        /// </summary>
        private void ClearTraitResults(object sender, EventArgs e)
        {
            traitSearchResultBox.Visible = false;
        }
    }
}
