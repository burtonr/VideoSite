using System.Collections.Generic;
using System.Linq;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using VideoLab.Repository.RepositoryInterfaces;

namespace VideoLab.Repository.APIStorage.Repositories
{
    public class YouTubeCommentRepository : YouTubeRepository, IReadOnlyRepository<IList<Comment>>
    {
        public new IList<Comment> Get(string videoId)
        {
            if(!Initialized) Initialize();

            var request = VideoLabYouTubeService.CommentThreads.List("snippet");

            request.VideoId = videoId;

            var result = request.Execute();

            return result.Items.Select(item => item.Snippet.TopLevelComment).ToList();
        }
    }
}
