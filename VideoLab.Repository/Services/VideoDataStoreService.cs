using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using VideoLab.Repository.DataStoreInterfaces;
using VideoLab.Repository.Models;

namespace VideoLab.Repository.Services
{
    public class VideoDataStoreService
    {
        private readonly Dictionary<VideoStoreLocation, IVideoDataStore> _videoDataStores;
        public int ResultCount = 50;

        public VideoDataStoreService(IList<IVideoDataStore> videoDataStores)
        {
            _videoDataStores = videoDataStores.ToDictionary(k => k.StoreLocation, value => value);
        }

        public VideoLabVideo LoadVideo(string videoId)
        {
            var localDataStore = _videoDataStores[VideoStoreLocation.Local] as ILocalVideoDataStore;
            VideoLabVideo loadedVideo;

            if (localDataStore != null && localDataStore.VideoExists(videoId))
            {
                loadedVideo = localDataStore.LoadVideo(videoId);
            }
            else
            {
                var externalDataStore = DetermineDataStoreFromId(videoId);

                loadedVideo = externalDataStore.LoadVideo(videoId);
            }

            return loadedVideo;
        }

        public IList<VideoLabVideo> ListVideosByTags(IList<string> tags)
        {
            var videoList = new List<VideoLabVideo>();

            if(_videoDataStores.ContainsKey(VideoStoreLocation.Local))
            {
                var localDatStore = _videoDataStores[VideoStoreLocation.Local];
                videoList.AddRange(localDatStore.SearchVideos(tags, ResultCount));
            }

            if (videoList.Count < ResultCount)
            {
                ListVideosFromExternalSource(videoList, tags);
            }
            
            return videoList;
        }

        private void ListVideosFromExternalSource(List<VideoLabVideo> videoList, IList<string> tags)
        {
            var externalSources =
                _videoDataStores.Where(source => source.Value.StoreLocation != VideoStoreLocation.Local).ToList();

            if (!externalSources.Any()) return;

            var remainingResults = ResultCount;

            if (videoList.Count > 0)
            {
                remainingResults = videoList.Count - ResultCount;
            }

            //YouTube only allows 50 search results at a time. 
            //TODO: put limit on YouTube data store

            //spit the remaining available list between the external sources
            var resultsPerSource = remainingResults/externalSources.Count();
            //call search on each of the external sources to get the amount of results for each
            foreach (var externalResults in externalSources.Select(externalSource => externalSource.Value.SearchVideos(tags, resultsPerSource)))
            {
                foreach (var externalResult in externalResults)
                {
                    StoreExternalVideoDataLocally(externalResult);
                }

                videoList.AddRange(externalResults);
            }
        }

        public IVideoDataStore DetermineDataStoreFromId(string videoId)
        {
            //TODO:Implement a TryParse function to determine if the id is Vimeo (long) or YouTube (GUID-like string)
            return _videoDataStores[VideoStoreLocation.YouTube];
        }

        public void StoreExternalVideoDataLocally(VideoLabVideo videoToSave)
        {
            if (!_videoDataStores.ContainsKey(VideoStoreLocation.Local)) return;

            var localDataStore = _videoDataStores[VideoStoreLocation.Local] as ILocalVideoDataStore;
            localDataStore?.StoreVideo(videoToSave);
        }
    }
}
