using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using static Happy_Apps_Core.StaticHelpers;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Happy_Apps_Core
{
    /// <summary>
    /// From get vn commands
    /// </summary>
    public class VNItem : IEquatable<VNItem>
    {
        public int ID { get; set; }
        //flag: basic
        public string Title { get; set; }
        /// <summary>
        /// Original title (Kanji title)
        /// </summary>
        public string Original { get; set; }
        public string Released { get; set; }
        public string[] Languages { get; set; }
        public string[] Orig_Lang { get; set; }
        public List<string> Platforms { get; set; }
        //flag: details
        public string Aliases { get; set; }
        public int? Length { get; set; }
        public string Description { get; set; }
        public WebLinks Links { get; set; }
        public string Image { get; set; }
        public bool Image_Nsfw { get; set; }
        public AnimeItem[] Anime { get; set; } //flag: anime
        public RelationsItem[] Relations { get; set; } //flag: relations
        public List<TagItem> Tags { get; set; } //flag: tags
                                                //flag: stats
        public double Popularity { get; set; }
        public double Rating { get; set; }
        public int VoteCount { get; set; }
        public ScreenItem[] Screens { get; set; } //flag: screens

        public bool Equals(VNItem other)
        {
            // ReSharper disable once PossibleNullReferenceException
            return ID == other.ID;
        }


        public override string ToString() => $"ID={ID} Title={Title}";

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            var hashID = ID == -1 ? 0 : ID.GetHashCode();
            return hashID;
        }

        /// <summary>
        /// In VNItem, when details flag is present
        /// </summary>
        public class AnimeItem
        {
            public int ID { get; set; }
            public int Ann_ID { get; set; }
            public string Nfo_ID { get; set; }
            public string Title_Romaji { get; set; }
            public string Title_Kanji { get; set; }
            public int Year { get; set; }
            public string Type { get; set; }

            public string Print()
            {
                var sb = new StringBuilder();
                if (Title_Romaji != null) sb.Append(Title_Romaji);
                else if (Title_Kanji != null) sb.Append(Title_Kanji);
                else sb.Append(ID);
                if (Year > 0) sb.Append($" ({Year})");
                if (Type != null) sb.Append($" ({Type})");
                return sb.ToString();
            }
        }

        /// <summary>
        /// In VNItem, when details screens is present
        /// </summary>
        public class ScreenItem
        {
            /// <summary>
            /// URL of screenshot 
            /// </summary>
            public string Image { get; set; }
            public int RID { get; set; }
            public bool Nsfw { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
        }

        /// <summary>
        /// In VNItem, when relations flag is present
        /// </summary>
        public class RelationsItem
        {
            public int ID { get; set; }
            public string Relation { get; set; }
            public string Title { get; set; }
            public string Original { get; set; }
            public bool Official { get; set; }

            public static readonly Dictionary<string, string> relationDict = new Dictionary<string, string>
        {
            { "seq", "Sequel"},
            { "preq", "Prequel"},
            { "set", "Same Setting"},
            { "alt", "Alternative Version"},
            { "char", "Shares Characters"},
            { "side", "Side Story"},
            { "par", "Parent Story"},
            { "ser", "Same Series"},
            { "fan", "Fandisc"},
            { "orig", "Original Game"}
        };

            public string Print() => $"{(Official ? "" : "[Unofficial] ")}{relationDict[Relation]} - {Title} - {ID}";

            public override string ToString() => $"ID={ID} Title={Title}";
        }

        /// <summary>
        /// In VNItem, when details flag is present
        /// </summary>
        public class WebLinks
        {
            public object Renai { get; set; }
            public object Encubed { get; set; }
            public object Wikipedia { get; set; }
        }

        /// <summary>
        /// In VNItem, when tags flag is present
        /// </summary>
        public class TagItem : List<double>
        {
            public int ID
            {
                get => (int)this[0];
                set => this[0] = value;
            }

            public double Score
            {
                get => this[1];
                set => this[1] = value;
            }

            public int Spoiler
            {
                get => (int)this[2];
                set => this[2] = value;
            }

            public TagCategory Category { get; set; }

            public override string ToString()
            {
                return $"[{ID},{Score},{Spoiler}]";
            }

            public string GetName(List<DumpFiles.WrittenTag> plainTags)
            {
                return plainTags.Find(item => item.ID == ID)?.Name;
            }

            public void SetCategory(List<DumpFiles.WrittenTag> plainTags)
            {
                string cat = plainTags.Find(item => item.ID == ID)?.Cat;
                switch (cat)
                {
                    case ContentTag:
                        Category = TagCategory.Content;
                        return;
                    case SexualTag:
                        Category = TagCategory.Sexual;
                        return;
                    case TechnicalTag:
                        Category = TagCategory.Technical;
                        return;
                    default:
                        return;
                }
            }

            /// <summary>
            /// Return string with Tag name and score, if tag isn't found in list, "Not Approved" is returned.
            /// </summary>
            /// <param name="plainTags">List of tags from tagdump</param>
            /// <returns>String with tag name and score</returns>
            public string Print(List<DumpFiles.WrittenTag> plainTags)
            {
                var name = GetName(plainTags);
                return name != null ? $"{GetName(plainTags)} ({Score:0.00})" : "Not Approved";
            }
        }


        /// <summary>
        /// Custom Json class for having notes and groups as a string in title notes.
        /// </summary>
        public class CustomItemNotes
        {
            public string Notes { get; set; }
            public List<string> Groups { get; set; }

            public CustomItemNotes(string notes, List<string> groups)
            {
                Notes = notes;
                Groups = groups;
            }

            public string Serialize()
            {
                if (Notes.Equals("") && !Groups.Any()) return "";
                string serializedString = $"Notes: {Notes}|Groups: {string.Join(",", Groups)}";
                string escapedString = JsonConvert.ToString(serializedString);
                return escapedString.Substring(1, escapedString.Length - 2);
            }
        }


    }

    /// <summary>
    /// From get release commands
    /// </summary>
    public class ReleaseItem
    {
        public List<ProducerItem> Producers { get; set; }
        public List<VNItem> VN { get; set; }
        public int ID { get; set; }
        public string Released { get; set; }
        public string Title { get; set; }
        public bool Patch { get; set; }
        public bool Freeware { get; set; }
        public string Type { get; set; }
        public string Original { get; set; }
        public string[] Languages { get; set; }
        public bool Doujin { get; set; }

        public override string ToString()
        {
            return $"{Title} \t({Released})";
        }
    }

    /// <summary>
    /// From get producer commands
    /// </summary>
    public class ProducerItem
    {
        public int ID { get; set; }
        public bool Developer { get; set; }
        public bool Publisher { get; set; }
        public string Name { get; set; }
        public string Original { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }


        /// <summary>
        /// Convert ProducerItem to ListedProducer.
        /// </summary>
        /// <param name="producer">Producer to be converted</param>
        public static explicit operator ListedProducer(ProducerItem producer)
        {
            return new ListedProducer(producer.Name, -1, DateTime.MinValue, producer.ID, producer.Language);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"ID={ID} Name={Name}";
    }

    /// <summary>
    /// From get character commands
    /// </summary>
    public class CharacterItem
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Aliases { get; set; }
        public List<TraitItem> Traits { get; set; }
        public string Image { get; set; }
        public List<VNItem> VNs { get; set; }

        public bool CharacterIsInVN(int vnid)
        {
            IEnumerable<int> listOfVNIDs = VNs.Select(x => x.ID);
            return listOfVNIDs.Contains(vnid);
        }
        public bool ContainsTraits(IEnumerable<DumpFiles.WrittenTrait> traitFilters)
        {
            //remove all numbers in traits from traitIDs, if nothing is left then it matched all
            int[] traits = Traits.Select(x => x.ID).ToArray();
            return traitFilters.All(writtenTrait => traits.Any(characterTrait => writtenTrait.AllIDs.Contains(characterTrait)));
        }



        public class TraitItem : List<int>
        {
            public int ID
            {
                get => this[0];
                set => this[0] = value;
            }
            public int Spoiler
            {
                get => this[1];
                set => this[1] = value;
            }
        }

        public class VNItem : List<object>
        {
            public int ID
            {
                get => Convert.ToInt32(this[0]);
                set => this[0] = value;
            }
            public int RID
            {
                get => Convert.ToInt32(this[1]);
                set => this[1] = value;
            }
            public int Spoiler
            {
                get => Convert.ToInt32(this[2]);
                set => this[2] = value;
            }
            public string Role
            {
                get => Convert.ToString(this[3]);
                set => this[3] = value;
            }
        }

    }

    /// <summary>
    /// From get user commands
    /// </summary>
    public class UserItem
    {
        public int ID { get; set; }
        public string Username { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"ID={ID} Username={Username}";
    }

    /// <summary>
    /// From get votelist commands
    /// </summary>
    public class VoteListItem
    {
        public VoteListItem(int vn, int vote, int added)
        {
            VN = vn;
            Vote = vote;
            Added = added;
        }

        public int VN { get; set; }
        public int Vote { get; set; }
        public int Added { get; set; }
    }

    /// <summary>
    /// From get vnlist commands
    /// </summary>
    public class UserListItem
    {
        public UserListItem(int vn, int status, int added, string notes)
        {
            VN = vn;
            Status = status;
            Added = added;
            Notes = notes;
        }

        public int VN { get; set; }
        public int Status { get; set; }
        public int Added { get; set; }
        public string Notes { get; set; }
    }

    /// <summary>
    /// From get wishlist commands
    /// </summary>
    public class WishListItem
    {
        public WishListItem(int vn, int priority, int added)
        {
            VN = vn;
            Priority = priority;
            Added = added;
        }

        public int VN { get; set; }
        public int Priority { get; set; }
        public int Added { get; set; }
    }

}
