using System;
using System.Collections.Generic;

namespace Simplecast.Client.Models
{
    public class Episode
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public int Season { get; set; }

        public int PodcastId { get; set; }

        public string Guid { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int Duration { get; set; }

        public bool Explicit { get; set; }

        public bool Published { get; set; }

        public string Description { get; set; }

        public string LongDescription { get; set; }

        public DateTime PublishedAt { get; set; }

        public int AudioFileSize { get; set; }

        public string AudioUrl { get; set; }

        public string SharingUrl { get; set; }

        public Images Images { get; set; }

        public bool IsHidden { get; set; }

        public List<string> Sponsors { get; set; }
    }
}