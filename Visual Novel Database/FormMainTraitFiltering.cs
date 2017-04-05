﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Happy_Search.Other_Forms;
using Happy_Search.Properties;
using static Happy_Search.StaticHelpers;

namespace Happy_Search
{
    public partial class FormMain
    {
        private const string TraitLabel = "traitFilterLabel";
        private readonly List<CustomTraitFilter> _customTraitFilters;
        private List<WrittenTrait> _activeTraitFilter = new List<WrittenTrait>();
        private Func<ListedVN, bool> _traitFunction = x => true;

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
            if (customTraitFilters.SelectedIndex < 2) return; //shouldnt happen
            var askBox = MessageBox.Show(Resources.are_you_sure, Resources.are_you_sure, MessageBoxButtons.YesNo);
            if (askBox != DialogResult.Yes) return;
            var selectedFilter = customTraitFilters.SelectedIndex;
            customTraitFilters.Items.RemoveAt(selectedFilter);
            _customTraitFilters.RemoveAt(selectedFilter - 2);
            SaveMainXML();
            WriteText(traitReply, Resources.filter_deleted);
            customTraitFilterNameBox.Text = "";
            customTraitFilters.SelectedIndex = 0;
        }

        /// <summary>
        ///     Display VNs matching traits in selected custom filter.
        /// </summary>
        private void Filter_CustomTraits(object sender, EventArgs e)
        {
            if (DontTriggerEvent) return;
            switch (customTraitFilters.SelectedIndex)
            {
                case 0:
                    deleteCustomTraitFilterButton.Enabled = false;
                    return;
                case 1:
                    customTraitFilters.SelectedIndex = 0;
                    return;
                default:
                    deleteCustomTraitFilterButton.Enabled = true;
                    DisplayFilterTraits(true);
                    _activeTraitFilter =
                        new List<WrittenTrait>(_customTraitFilters[customTraitFilters.SelectedIndex - 2].Filters);
                    customTraitFilterNameBox.Text = _customTraitFilters[customTraitFilters.SelectedIndex - 2].Name;
                    DisplayFilterTraits();
                    break;
            }
            LoadVNListToGui();
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
            var customFilter = _customTraitFilters.Find(x => x.Name.Equals(filterName));
            if (customFilter != null)
            {
                var askBox = MessageBox.Show(@"Do you wish to overwrite present custom filter?", Resources.ask_overwrite,
                    MessageBoxButtons.YesNo);
                if (askBox != DialogResult.Yes) return;
                customFilter.Filters = new List<WrittenTrait>(_activeTraitFilter);
                customFilter.Updated = DateTime.UtcNow;
                SaveMainXML();
                WriteText(traitReply, Resources.filter_saved);
                customTraitFilters.SelectedIndex = customTraitFilters.Items.IndexOf(filterName);
                return;
            }
            customTraitFilters.Items.Add(filterName);
            _customTraitFilters.Add(new CustomTraitFilter(filterName, new List<WrittenTrait>(_activeTraitFilter)));
            SaveMainXML();
            WriteText(traitReply, Resources.filter_saved);
            customTraitFilters.SelectedIndex = customTraitFilters.Items.Count - 1;
        }

        /// <summary>
        ///     Clear list of active trait filters.
        /// </summary>
        private void ClearTraitFilter(object sender, EventArgs e)
        {
            DisplayFilterTraits(true);
            customTraitFilters.SelectedIndex = 0;
            ApplyListFilters();
            WriteText(traitReply, "Trait filter cleared.");
        }

        /// <summary>
        ///     Display or clear list of active trait filters.
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
                    Size = new Size(342, 17),
                    Text = trait.ToString(),
                    Checked = true,
                    AutoEllipsis = true
                };

                traitLabel.CheckedChanged += TraitFilterRemoved;
                count++;
                traitFilteringBox.Controls.Add(traitLabel);
            }

            IEnumerable<CharacterItem> traitCharacters = CharacterList.Where(x => x.ContainsTraits(_activeTraitFilter));
            var characterVNs = new List<int>();
            foreach (var characterItem in traitCharacters)
            {
                characterVNs.AddRange(characterItem.VNs.Select(x => x.ID));
            }
            _traitFunction = x => characterVNs.Contains(x.VNID);
            ApplyListFilters();
        }

        /// <summary>
        ///     Remove trait from active filters.
        /// </summary>
        private void TraitFilterRemoved(object sender, EventArgs e)
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

        /// <summary>
        ///     Change selected trait type.
        /// </summary>
        private void TraitRootChanged(object sender, EventArgs e)
        {
            if (traitRootsDropdown.SelectedIndex < 0) return;
            var trait = PlainTraits.Find(x => x.Name.Equals(traitRootsDropdown.SelectedItem));
            if (trait == null)
            {
                WriteError(traitReply, "Root trait not found.");
                return;
            }
            var traitSource = new AutoCompleteStringCollection();
            traitSource.AddRange(PlainTraits.Where(x => x.TopmostParent == trait.ID).Select(x => x.Name).ToArray());
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
            var root = PlainTraits.Find(x => x.Name.Equals(traitRootsDropdown.SelectedItem));
            var trait =
                PlainTraits.Find(x =>
                        x.Name.Equals(traitName, StringComparison.InvariantCultureIgnoreCase) &&
                        x.TopmostParent == root.ID);
            if (trait == null)
            {
                //WriteError(traitReply, $"{root} > {traitName} not found.", true);
                SearchTraits(null,null);
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
            if (_activeTraitFilter.Contains(trait))
            {
                WriteError(traitReply, "Trait is already in filter.");
                return;
            }
            _activeTraitFilter.Add(trait);
            DisplayFilterTraits();
        }

        /// <summary>
        /// Search for traits by name/alias.
        /// </summary>
        private void SearchTraits(object sender, EventArgs e)
        {
            traitSearchResultBox.Visible = false;
            if (traitSearchBox.Text == "") //check if box is empty
            {
                WriteError(traitReply, "Enter trait name.");
                return;
            }
            var text = traitSearchBox.Text.ToLowerInvariant();
            var results = PlainTraits.Where(t => t.Name.ToLowerInvariant().Contains(text) ||
                                            t.Aliases.Exists(a=>a.ToLowerInvariant().Contains(text))).ToArray();
            if (results.Length == 0)
            {
                WriteError(traitReply,"No traits with that name/alias found.");
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

        /// <summary>
        ///     Holds details of user-created custom trait filter.
        /// </summary>
        [Serializable, XmlRoot("CustomTraitFilter")]
        public class CustomTraitFilter
        {
            /// <summary>
            ///     Constructor for ComplexFilter (Custom Filter).
            /// </summary>
            /// <param name="name">User-set name of filter</param>
            /// <param name="filters">List of traits in filter</param>
            public CustomTraitFilter(string name, List<WrittenTrait> filters)
            {
                Name = name;
                Filters = filters;
            }

            /// <summary>
            ///     Empty Constructor needed for XML.
            /// </summary>
            public CustomTraitFilter()
            {
            }

            /// <summary>
            ///     User-set name of custom filter
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            ///     List of traits in custom filter
            /// </summary>
            public List<WrittenTrait> Filters { get; set; }

            /// <summary>
            ///     Date of last update to custom filter
            /// </summary>
            public DateTime Updated { get; set; }
        }
    }
}