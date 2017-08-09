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
        ///     Bring up dialog explaining features of the 'Trait Filtering' section.
        /// </summary>
        private void Help_TraitFiltering(object sender, EventArgs e)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            Debug.Assert(path != null, "path != null");
            var helpFile = $"{Path.Combine(path, "Program Data\\Help\\traitfiltering.html")}";
            new HtmlForm($"file:///{helpFile}").Show();
        }

        #region Custom Trait Filter Saving/Loading
        private void EnterCustomTraitFilterName(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SaveCustomTraitFilter(sender, e);
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
                    Resources.ask_overwrite, MessageBoxButtons.YesNo);
                if (askBox != DialogResult.Yes) return;
                customFilter.Filters = _filters.Traits.ToArray();
                customFilter.Updated = DateTime.UtcNow;
                DontTriggerEvent = true;
                traitFiltersCB.SelectedItem = customFilter;
                DontTriggerEvent = false;
            }
            else
            {
                DontTriggerEvent = true;
                _customTraitFilters.Add(new CustomTraitFilter(filterName, _filters.Traits.ToArray()));
                traitFiltersCB.SelectedIndex = _customTraitFilters.Count - 1;
                DontTriggerEvent = false;
            }
            SaveObjectToJsonFile(_customTraitFilters, CustomTraitFiltersJson);
            WriteText(traitReply, Resources.filter_saved);
        }

        /// <summary>
        ///     Delete custom trait filter.
        /// </summary>
        private void DeleteCustomTraitFilter(object sender, EventArgs e)
        {
            var askBox = MessageBox.Show(Resources.are_you_sure, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            var selectedFilter = traitFiltersCB.SelectedItem as CustomTraitFilter;
            if (selectedFilter == null) return; //shouldnt happen
            _customTraitFilters.Remove(selectedFilter);
            SaveObjectToJsonFile(_customTraitFilters, CustomTraitFiltersJson);
            WriteText(traitReply, Resources.filter_deleted);
        }

        /// <summary>
        ///     Clear list of active trait filters.
        /// </summary>
        private void ClearTraitFilter(object sender, EventArgs e)
        {
            _filters.Traits.Clear();
            WriteText(traitReply, "Trait filter cleared.");
        }
        #endregion

        /// <summary>
        ///     Display VNs matching traits in selected custom filter.
        /// </summary>
        private void Filter_CustomTraits(object sender, EventArgs e)
        {
            if (DontTriggerEvent) return;
            var selectedItem = traitFiltersCB.SelectedItem as CustomTraitFilter;
            if (selectedItem == null) return;
            customTraitFilterNameBox.Text = selectedItem.Name;
            _filters.Traits.SetRange(selectedItem.Filters.ToArray());
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
            var trait = FormMain.PlainTraits.Find(x =>
                    x.Name.Equals(traitName, StringComparison.InvariantCultureIgnoreCase) &&
                    x.TopmostParent == root.ID);
            if (trait != null)
            {
                traitSearchBox.Text = "";
                AddFilterTrait(trait);
                WriteText(traitReply, $"Added trait {trait}");
                return;
            }
            SearchTraits();
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
            if (_filters.Traits.Contains(trait))
            {
                WriteError(traitReply, "Trait is already in filter.");
                return;
            }
            _filters.Traits.Add(trait);
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
