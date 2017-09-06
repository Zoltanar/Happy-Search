using System.Collections.Generic;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#pragma warning disable 1591

// ReSharper disable InconsistentNaming

namespace Happy_Apps_Core
{
    /// <summary>
    /// From dbstats command
    /// </summary>
    public class DbStats
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

    /// <summary>
    /// From any command when command fails
    /// </summary>
    public class ErrorResponse
    {
        public double Fullwait { get; set; }
        public string Type { get; set; }
        public string ID { get; set; }
        public string Msg { get; set; }
        public double Minwait { get; set; }
    }
    
    /// <summary>
    /// From all get commands
    /// </summary>
    /// <typeparam name="T">Type of object contained in Items</typeparam>
    public class ResultsRoot<T>
    {
        public List<T> Items { get; set; }
        public bool More { get; set; }
        public int Num { get; set; }
    }
}