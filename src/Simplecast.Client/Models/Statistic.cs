using System.Collections.Generic;

namespace Simplecast.Client.Models
{
    public class Statistic
    {
        public List<Datum> Data { get; set; }

        public int TotalListens { get; set; }
    }
}