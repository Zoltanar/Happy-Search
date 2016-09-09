using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Serialization;
#pragma warning disable 1591

// ReSharper disable InconsistentNaming

namespace Happy_Search
{
    //object received by 'get vn' command
    public class VNRoot
    {
        public int Num { get; set; }
        public List<VNItem> Items { get; set; }
        public bool More { get; set; }
    }

    //object contained in VNRoot
    public class VNItem : IEquatable<VNItem>
    {
        public int ID { get; set; }
        //flag: basic
        public string Title { get; set; }
        public string Original { get; set; }
        public string Released { get; set; }
        public List<string> Languages { get; set; }
        public List<string> Orig_Lang { get; set; }
        public List<string> Platforms { get; set; }
        //flag: details
        public string Aliases { get; set; }
        public int? Length { get; set; }
        public string Description { get; set; }
        public Links Links { get; set; }
        public string Image { get; set; }
        public bool Image_Nsfw { get; set; }
        public List<AnimeItem> Anime { get; set; } //flag: anime
        public List<RelationsItem> Relations { get; set; } //flag: relations
        public List<TagItem> Tags { get; set; } //flag: tags
        //flag: stats
        public double Popularity { get; set; }
        public double Rating { get; set; }
        public int VoteCount { get; set; }
        public List<ScreenItem> Screens { get; set; } //flag: screens

        public bool Equals(VNItem other)
        {
            return ID == other.ID;
        }


        public override string ToString() => $"ID={ID} Title={Title}";

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            var hashID = ID == -1 ? 0 : ID.GetHashCode();
            return hashID;
        }
    }

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
            if (Type!= null) sb.Append($" ({Type})");
            return sb.ToString();
        }
    }

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

    //object contained in VNItem.Relations
    public class RelationsItem
    {
        public int ID { get; set; }
        public string Relation { get; set; }
        public string Title { get; set; }
        public string Original { get; set; }

        private readonly Dictionary<string,string> relationDict = new Dictionary<string, string>
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

        public string Print() => $"{relationDict[Relation]} - {Title} - {ID}";

        public override string ToString() => $"ID={ID} Title={Title}";
        

    }

    //object contained in VNItem
    public class Links
    {
        public object Renai { get; set; }
        public object Encubed { get; set; }
        public object Wikipedia { get; set; }
    }

    //object contained in VNItem.Tags
    public class TagItem : List<double>
    {
        public int ID
        {
            get { return (int)this[0]; }
            set { this[0] = value; }
        }

        public double Score
        {
            get { return this[1]; }
            set { this[1] = value; }
        }

        public int Spoiler
        {
            get { return (int)this[2]; }
            set { this[2] = value; }
        }

        public override string ToString()
        {
            return $"[{ID},{Score},{Spoiler}]";
        }

        public string GetName(List<WrittenTag> plainTags)
        {
            return plainTags.Find(item => item.ID == ID)?.Name;
        }

        public string Print(List<WrittenTag> plainTags)
        {
            var name = GetName(plainTags);
            return name != null ? $"{GetName(plainTags)} ({Score.ToString("0.00")})" : "Not Approved";
        }
    }

    //object received by 'get producers' command
    public class ProducersRoot
    {
        public int Num { get; set; }
        public List<ProducerItem> Items { get; set; }
        public bool More { get; set; }
    }

    //object contained in ProducersRoot
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
        /// Convert ListedSearchedProducer to ListedProducer.
        /// </summary>
        /// <param name="searchedProducer">Producer to be converted</param>
        /// <returns>ListedProducer with name and ID of ListedSearchedProducer</returns>
        public static explicit operator ListedProducer(ProducerItem searchedProducer)
        {
            return new ListedProducer(searchedProducer.Name, -1, "No", DateTime.MinValue, searchedProducer.ID);
        }


        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"ID={ID} Name={Name}";
    }

    //object received by 'get release' command
    public class ReleasesRoot
    {
        public int Num { get; set; }
        public List<ReleaseItem> Items { get; set; }
        public bool More { get; set; }
    }

    //object contained in ReleasesRoot
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

    public class UserListRoot
    {
        public int Num { get; set; }
        public List<UserListItem> Items { get; set; }
        public bool More { get; set; }
    }

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

    public class WishListRoot
    {
        public int Num { get; set; }
        public List<WishListItem> Items { get; set; }
        public bool More { get; set; }
    }

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

    public class VoteListRoot
    {
        public int Num { get; set; }
        public List<VoteListItem> Items { get; set; }
        public bool More { get; set; }
    }

    //object received by 'dbstats' command
    [Serializable, XmlRoot("DbRoot")]
    public class DbRoot
    {
        public int Tags { get; set; }
        public int Threads { get; set; }
        public int Chars { get; set; }
        public int Posts { get; set; }
        public int Releases { get; set; }
        public int Traits { get; set; }
        public int Producers { get; set; }
        public int Staff { get; set; }
        public int VN { get; set; }
        public int Users { get; set; }
    }

    public class CharacterItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Aliases { get; set; }
        public string Image { get; set; }
    }

    public class CharacterRoot
    {
        public List<CharacterItem> Items { get; set; }
        public bool More { get; set; }
        public int Num { get; set; }
    }

    //object for error response possibly received from any command
    public class ErrorResponse
    {
        public double Fullwait { get; set; }
        public string Type { get; set; }
        public string ID { get; set; }
        public string Msg { get; set; }
        public double Minwait { get; set; }
    }


    //These class is used to read the 'tag-dump'
    //object contained in tag dump
    public class WrittenTag
    {
        public List<object> Aliases { get; set; }
        public int VNs { get; set; }
        public int ID { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Cat { get; set; }
        public List<int> Parents { get; set; }
        public bool Meta { get; set; }


        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"ID={ID} Name={Name}";
    }

}