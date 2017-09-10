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
}