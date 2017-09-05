using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Happy_Apps_Core
{
    /// <summary>
    /// Contains original and other languages available for vn.
    /// </summary>
    [Serializable]
    public class VNLanguages
    {
        /// <summary>
        /// Languages for original release
        /// </summary>
        public string[] Originals { get; set; }
        /// <summary>
        /// Languages for other releases
        /// </summary>
        public string[] Others { get; set; }

        /// <summary>
        /// Languages for all releases
        /// </summary>
        public IEnumerable<string> All => Originals.Concat(Others);

        /// <summary>
        /// Empty Constructor for serialization
        /// </summary>
        public VNLanguages() { }

        /// <summary>
        /// Constructor for vn languages.
        /// </summary>
        /// <param name="originals">Languages for original release</param>
        /// <param name="all">Languages for all releases</param>
        public VNLanguages(string[] originals, string[] all)
        {
            Originals = originals;
            Others = all.Except(originals).ToArray();
        }

        /// <summary>
        /// Displays a json-serialized string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

#pragma warning disable 1591
    /// <summary>
    /// Map Wishlist status numbers to words.
    /// </summary>
    public enum WishlistStatus
    {
        None = -1,
        High = 0,
        Medium = 1,
        Low = 2,
        Blacklist = 3
    }
    
    /// <summary>
    /// Map Userlist status numbers to words.
    /// </summary>
    public enum UserlistStatus
    {
        None = -1,
        Unknown = 0,
        Playing = 1,
        Finished = 2,
        Stalled = 3,
        Dropped = 4
    }
#pragma warning restore 1591

}