using System.ComponentModel;
using System.Linq;
using Happy_Apps_Core;
using Happy_Search.Other_Forms;

namespace Happy_Search
{
    /// <summary>
    /// New custom filter class (second overhaul)
    /// </summary>
    public class CustomFilter
    {
        /// <summary>
        /// Name of custom filter
        /// </summary>
        public string Name;

        /// <summary>
        /// List of filters which must all be true
        /// </summary>
        public readonly BindingList<FilterItem> AndFilters = new BindingList<FilterItem>();
        /// <summary>
        /// List of filters in which at least one must be true
        /// </summary>
        public readonly BindingList<FilterItem> OrFilters = new BindingList<FilterItem>();

        /// <inheritdoc />
        public override string ToString() => Name;

        /// <summary>
        /// Create a custom filter with copies of filters from an existing filter.
        /// </summary>
        /// <param name="existingFilter"></param>
        public CustomFilter(CustomFilter existingFilter)
        {
            Name = existingFilter.Name;
            AndFilters = new BindingList<FilterItem>();
            AndFilters.AddRange(existingFilter.AndFilters.ToArray());
            OrFilters = new BindingList<FilterItem>();
            OrFilters.AddRange(existingFilter.OrFilters.ToArray());
        }

        /// <summary>
        /// Constructor for an empty custom filter
        /// </summary>
        public CustomFilter()
        {
            Name = "Custom Filter";
        }
    }
}
