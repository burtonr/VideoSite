using System.Collections.Generic;
using VideoLab.Repository.DataStoreInterfaces;
using VideoLab.Repository.Models;

namespace VideoLab.Repository.VideoLabStorage
{
    public class LocalVideoDataStore : ILocalVideoDataStore
    {
        private readonly IVideoLabRepository<VideoLabVideo> _videoLabRepository;

        public LocalVideoDataStore(IVideoLabRepository<VideoLabVideo> videoLabRepository)
        {
            _videoLabRepository = videoLabRepository;
        }

        public VideoStoreLocation StoreLocation => VideoStoreLocation.Local;

        public IList<VideoLabVideo> SearchVideos(IList<string> tags, int maxResults)
        {
            return _videoLabRepository.Search(tags, maxResults);
        }

        public IList<VideoLabVideo> SearchVideos(string title, int maxResults)
        {
            throw new System.NotImplementedException();
        }

        public VideoLabVideo LoadVideo(string videoId)
        {
            var loadedVideo = _videoLabRepository.Get(videoId);
            return loadedVideo;
        }

        public bool VideoExists(string videoId)
        {
            return _videoLabRepository.Get(videoId) != null;
        }

        public void StoreVideo(VideoLabVideo video)
        {
            _videoLabRepository.Save(video);
        }
    }
}