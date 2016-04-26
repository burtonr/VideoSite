using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Apis.YouTube.v3.Data;
using VideoLab.Repository.APIStorage.Repositories;
using VideoLab.Repository.DataStoreInterfaces;
using VideoLab.Repository.Models;
using VideoLab.Repository.RepositoryInterfaces;

namespace VideoLab.Repository.APIStorage.DataStores
{
    public class YouTubeDataStore : IExternalVideoDataStore
    {
        private readonly IYouTubeRepository _youTubeRepository;
        private readonly ISearchableRepository<SearchResult> _youTubeSearchableRepository;

        public YouTubeDataStore(IYouTubeRepository youTubeRepository, ISearchableRepository<SearchResult> youTubeSearchableRepository)
        {
            _youTubeRepository = youTubeRepository;
            _youTubeSearchableRepository = youTubeSearchableRepository;
        }

        public VideoStoreLocation StoreLocation => VideoStoreLocation.YouTube;

        public IList<VideoLabVideo> SearchVideos(IList<string> tags, int maxResults)
        {
            var youTubeResults = _youTubeSearchableRepository.Search(tags, maxResults);
            
            return youTubeResults.Select(TranslateSearchResultToVideoLabVideo).ToList();
        }

        public IList<VideoLabVideo> SearchVideos(string title, int maxResults)
        {
            var youTubeResults = _youTubeSearchableRepository.Search(title, maxResults);

            return youTubeResults.Select(TranslateSearchResultToVideoLabVideo).ToList();
        }

        public VideoLabVideo LoadVideo(string videoId)
        {
            var youTubeVideo = _youTubeRepository.Get(videoId);

            if (youTubeVideo == null) throw new FileNotFoundException();

            return TranslateToVideoLabVideo(youTubeVideo);
        }

        public VideoLabVideo TranslateToVideoLabVideo(Video youTubeVideoData)
        {
            var imageUrl = GetHighestQualityImage(youTubeVideoData.Snippet.Thumbnails);

            ulong viewCount = ulong.MinValue, duration = ulong.MinValue;

            if (youTubeVideoData.Statistics?.ViewCount != null)
                viewCount = (ulong) youTubeVideoData.Statistics.ViewCount;

            if (youTubeVideoData.FileDetails?.DurationMs != null)
                duration = (ulong) youTubeVideoData.FileDetails.DurationMs;

            return new VideoLabVideo()
            {
                VideoId = youTubeVideoData.Id,
                Description = youTubeVideoData.Snippet.Description,
                Title = youTubeVideoData.Snippet.Title,
                VideoImageUrl = imageUrl,
                Duration = duration,
                ViewCount = viewCount,
                PublishedDate = youTubeVideoData.Snippet.PublishedAt,
                Tags = youTubeVideoData.Snippet.Tags
            };
        }

        public VideoLabVideo TranslateSearchResultToVideoLabVideo(SearchResult searchResult)
        {
            var imageUrl = GetHighestQualityImage(searchResult.Snippet.Thumbnails);

            return new VideoLabVideo
            {
                VideoId = searchResult.Id.VideoId,
                Title = searchResult.Snippet.Title,
                Description = searchResult.Snippet.Description,
                VideoImageUrl = imageUrl
            };

        }

        public string GetHighestQualityImage(ThumbnailDetails thumbnailDetails)
        {
            var highRes = thumbnailDetails.Maxres;
            var high = thumbnailDetails.High;
            var standard = thumbnailDetails.Standard;
            var medium = thumbnailDetails.Medium;

            return highRes != null ? highRes.Url
                : (high != null ? high.Url
                    : (standard != null ? standard.Url 
                        : (medium != null ? medium.Url 
                            : thumbnailDetails.Default__.Url)));
        }

        async void StoreVideoTagsAsync(IList<string> tagList)
        {
            throw new System.NotImplementedException();
        }

        //TODO: Add function to get comments for the video. Should only happen when the video is selected to be watched

        //See here: https://developers.google.com/youtube/v3/docs/commentThreads/list
    }
}