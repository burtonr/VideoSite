using System;
using System.Collections.Generic;

namespace VideoLab.Repository.Models
{
    public class VideoLabVideo
    {
        public VideoStoreLocation VideoStoreLocation;
        public string VideoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoImageUrl { get; set; }
        public ulong? Duration { get; set; }
        public DateTime? PublishedDate { get; set; }
        public ulong? ViewCount { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<VideoLabComment> Comments { get; set; }
    }
}