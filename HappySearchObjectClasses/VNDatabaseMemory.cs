using System.Collections.Generic;
using System.Linq;

namespace Happy_Apps_Core
{
    partial class VNDatabase
    {
        // ReSharper disable once UnusedMember.Global
        public IEnumerable<ListedVN> ListVNByNameOrAlias(string searchString)
        {
            searchString = searchString.ToLowerInvariant();
            return VNList.Where(vn =>
                vn.Title.ToLowerInvariant().Contains(searchString) ||
                vn.KanjiTitle.ToLowerInvariant().Contains(searchString) ||
                vn.Aliases.ToLowerInvariant().Contains(searchString));
        }
    }
}
