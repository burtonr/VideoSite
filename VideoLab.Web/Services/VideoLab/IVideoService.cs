using System.Collections.Generic;
using VideoLab.Repository.Models;

namespace VideoLab.Web.Services.VideoLab
{
    public interface IVideoService
    {
        IList<VideoLabVideo> FetchVideos(IList<string> searchQuery);
    }
}