using NUnit.Framework;
using VideoLab.Repository.APIStorage.DataStores;
using VideoLab.Repository.APIStorage.Repositories;

namespace VideoLab.Repository.IntegrationTests
{
    public class YouTubeDataStoreTest
    {
        [Test]
        public void CallYouTube_ReturnVideo()
        {
            const string expectedTitle = "Countdown 54321";
            var service = new YouTubeDataStore(new YouTubeRepository(), new YouTubeSearchRepository());

            var videoResult = service.LoadVideo("YVgc2PQd_bo");

            Assert.IsNotNull(videoResult);
            Assert.AreEqual(expectedTitle, videoResult.Title);
        }

        [Test]
        public void GetCommentsForVideo()
        {
            var service = new YouTubeCommentRepository();
            var result = service.Get("YVgc2PQd_bo");

            Assert.NotNull(result);
        }

        [Test]
        public void GetTagsForVideo()
        {
            
        }
    }
}
