using System.Collections.Generic;
using VideoLab.Repository.Models;
using VideoLab.Repository.Services;
using VideoLab.Repository.StructureMap;

namespace VideoLab.Web.Services.VideoLab
{
    public class VideoService : IVideoService
    {
        private readonly VideoDataStoreService _videoDataStoreService;
        public VideoService()
        {
            var container = Bootstraper.Bootstrap();
            _videoDataStoreService = container.GetInstance<VideoDataStoreService>(); 
            
        }

        public IList<VideoLabVideo> FetchVideos(IList<string> searchQuery)
        {
            var results = _videoDataStoreService.ListVideosByTags(searchQuery);
            return results;
        }
    }
}