using System;

namespace Happy_Apps_Core
{
    partial class VNDatabase
    {
        public static Func<ListedVN,bool> ListVNByNameOrAliasFunc(string searchString)
        {
            searchString = searchString.ToLowerInvariant();
            return vn =>
                vn.Title.ToLowerInvariant().Contains(searchString) ||
                vn.KanjiTitle.ToLowerInvariant().Contains(searchString) ||
                vn.Aliases.ToLowerInvariant().Contains(searchString);
        }
    }
}
