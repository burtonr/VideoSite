using System.Collections.Generic;
using System.Linq;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using VideoLab.Repository.RepositoryInterfaces;

namespace VideoLab.Repository.APIStorage.Repositories
{
    public class YouTubeSearchRepository : YouTubeRepository, ISearchableRepository<SearchResult>
    {
        public IList<SearchResult> Search(IList<string> searchParams, int maxResults)
        {
            if(!Initialized) Initialize();

            var request = VideoLabYouTubeService.Search.List("snippet, Id");

            request.Type = "video";
            request.VideoEmbeddable = SearchResource.ListRequest.VideoEmbeddableEnum.True__;
            request.MaxResults = maxResults;
            request.Q = string.Join("|", searchParams);

            var response = request.Execute();

            return response.Items.ToList();
        }

        public IList<SearchResult> Search(string title, int maxResults)
        {
            if (!Initialized) Initialize();

            var request = VideoLabYouTubeService.Search.List("snippet, Id");

            request.Type = "video";
            request.VideoEmbeddable = SearchResource.ListRequest.VideoEmbeddableEnum.True__;
            request.MaxResults = maxResults;
            request.Q = title;

            var response = request.Execute();

            return response.Items.ToList();
        }
    }
}
