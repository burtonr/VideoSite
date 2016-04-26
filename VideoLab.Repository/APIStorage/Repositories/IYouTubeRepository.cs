using Google.Apis.YouTube.v3.Data;
using VideoLab.Repository.RepositoryInterfaces;

namespace VideoLab.Repository.APIStorage.Repositories
{
    public interface IYouTubeRepository : IExternalRepository<Video>
    { }
}