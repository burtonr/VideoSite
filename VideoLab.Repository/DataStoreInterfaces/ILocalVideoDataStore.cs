using VideoLab.Repository.Models;

namespace VideoLab.Repository.DataStoreInterfaces
{
    public interface ILocalVideoDataStore : IVideoDataStore
    {
        bool VideoExists(string videoId);
        void StoreVideo(VideoLabVideo video);
    }
}