using System.Collections.Generic;

namespace Simplecast.Client.Models
{
    public class Podcast
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string RssUrl { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string Copyright { get; set; }

        public string Keywords { get; set; }

        public string Subdomain { get; set; }

        public List<string> Categories { get; set; }

        public string ItunesUrl { get; set; }

        public string Language { get; set; }

        public string Website { get; set; }

        public string Twitter { get; set; }

        public bool Explicit { get; set; }

        public Images Images { get; set; }
    }
}