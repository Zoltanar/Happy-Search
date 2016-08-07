using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
// ReSharper disable InconsistentNaming

namespace Visual_Novel_Database
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
        public List<object> Anime { get; set; }     //flag: anime
        public List<object> Relations { get; set; } //flag: relations
        public List<TagItem> Tags { get; set; }     //flag: tags
        //flag: stats
        public double Popularity { get; set; }
        public double Rating { get; set; }
        public int Votecount { get; set; }
        public List<object> Screens { get; set; }   //flag: screens


        public override string ToString() => $"ID={ID}\t\tTitle={Title}";

        public bool Equals(VNItem other)
        {
            return ID == other.ID;
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            int hashID = ID == -1 ? 0 : ID.GetHashCode();
            return hashID;
        }
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
        public int ID { get { return (int) this[0]; } set { this[0] = value; } }
        public double Score { get { return this[1]; } set { this[1] = value; } }
        public int Spoiler { get { return (int)this[2]; } set { this[2] = value; } }

        public override string ToString()
        {
            return $"[{ID},{Score},{Spoiler}]";
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

        public override string ToString()
        {
            return $"ID={ID}\t \tName={Name}";
        }
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
    }
}
