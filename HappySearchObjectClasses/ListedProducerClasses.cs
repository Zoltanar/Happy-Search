using System;

namespace Happy_Apps_Core
{
    public class ListedProducerBase
    {
        /// <summary>
        /// Producer Name
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Producer ID
        /// </summary>
        public int ID { get; protected set; }

        /// <summary>
        /// Language of Producer
        /// </summary>
        public string Language { get; protected set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"ID={ID} Name={Name}";
    }

    /// <summary>
    /// Object for Favorite Producers in Object List View.
    /// </summary>
    public class ListedProducer : ListedProducerBase
    {
        /// <summary>
        /// Constructor for ListedProducer, not favorite producers.
        /// </summary>
        /// <param name="name">Producer Name</param>
        /// <param name="numberOfTitles">Number of Producer's titles</param>
        /// <param name="updated">Date of last update to Producer</param>
        /// <param name="id">Producer ID</param>
        /// <param name="language">Language of producer</param>
        public ListedProducer(string name, int numberOfTitles, DateTime updated, int id, string language)
        {
            Name = name;
            NumberOfTitles = numberOfTitles;
            Updated = StaticHelpers.DaysSince(updated);
            ID = id;
            Language = language;
        }

        /// <summary>
        /// Constructor for ListedProducer for favorite producers.
        /// </summary>
        /// <param name="name">Producer Name</param>
        /// <param name="numberOfTitles">Number of Producer's titles</param>
        /// <param name="updated">Date of last update to Producer</param>
        /// <param name="id">Producer ID</param>
        /// <param name="language">Language of producer</param>
        /// <param name="userAverageVote">User's average vote on Producer titles. (Only titles with votes)</param>
        /// <param name="userDropRate">User's average drop rate on Producer titles. (Dropped / (Finished+Dropped)</param>
        public ListedProducer(string name, int numberOfTitles, DateTime updated, int id, string language,
            double userAverageVote, int userDropRate)
        {
            Name = name;
            NumberOfTitles = numberOfTitles;
            Updated = StaticHelpers.DaysSince(updated);
            ID = id;
            Language = language;
            UserAverageVote = Math.Round(userAverageVote, 2);
            UserDropRate = userDropRate;
        }

        /// <summary>
        /// Number of Producer's titles
        /// </summary>
        public int NumberOfTitles { get; set; }
        /// <summary>
        /// Date of last update to Producer
        /// </summary>
        public int Updated { get; set; }
        /// <summary>
        /// User's average vote on Producer titles. (Only titles with votes)
        /// </summary>
        public double UserAverageVote { get; set; }
        /// <summary>
        /// User's average drop rate on Producer titles. (Dropped / (Finished+Dropped)
        /// </summary>
        public int UserDropRate { get; set; }
        /// <summary>
        /// Bayesian average score of votes by all users.
        /// </summary>
        public double GeneralRating { get; set; }
        
    }

    /// <summary>
    /// Object for displaying producer search results in OLV.
    /// </summary>
    public class ListedSearchedProducer : ListedProducerBase
    {
        /// <summary>
        /// Constructor for Searched Producer.
        /// </summary>
        /// <param name="name">Name of producer</param>
        /// <param name="inList">Is producer already in favorite producers list? (Yes/No)</param>
        /// <param name="id">ID of producer</param>
        /// <param name="language">Language of producer</param>
        /// <param name="finishedTitles">Number of producer's titles finished by user</param>
        /// <param name="urtTitles">Number of producer's titles related to user</param>
        public ListedSearchedProducer(string name, string inList, int id, string language, int finishedTitles, int urtTitles)
        {
            Name = name;
            InList = inList;
            ID = id;
            Language = language;
            FinishedTitles = finishedTitles;
            URTTitles = urtTitles;

        }

        /// <summary>
        /// Convert ListedSearchedProducer to ListedProducer.
        /// </summary>
        /// <param name="searchedProducer">Producer to be converted</param>
        /// <returns>ListedProducer with name and ID of ListedSearchedProducer</returns>
        public static explicit operator ListedProducer(ListedSearchedProducer searchedProducer)
        {
            return new ListedProducer(searchedProducer.Name, -1, DateTime.MinValue, searchedProducer.ID, searchedProducer.Language);
        }
        
        /// <summary>
        /// Is producer already in favorite producers list? (Yes/No)
        /// </summary>
        public string InList { get; set; }

        /// <summary>
        /// Number of producer's titles finished by user
        /// </summary>
        public int FinishedTitles { get; set; }

        /// <summary>
        /// Number of producer's titles related to user
        /// </summary>
        public int URTTitles { get; set; }
    }
}