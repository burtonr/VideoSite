using System.Collections.Generic;
using VideoLab.Repository.Models;

namespace VideoLab.Repository.DataStoreInterfaces
{
    public interface IVideoDataStore
    {
        VideoLabVideo LoadVideo(string videoId);
        VideoStoreLocation StoreLocation { get; }
        IList<VideoLabVideo> SearchVideos(IList<string> tags, int maxResults);
        IList<VideoLabVideo> SearchVideos(string title, int maxResults);
    }
}