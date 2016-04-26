using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using VideoLab.Repository.APIStorage.DataStores;
using VideoLab.Repository.APIStorage.Repositories;
using VideoLab.Repository.DataStoreInterfaces;
using VideoLab.Repository.Services;
using VideoLab.Repository.VideoLabStorage;

namespace VideoLab.Repository.UnitTests
{
    [TestFixture]
    public class VideoDataStoreServiceTests
    {
        private Mock<YouTubeDataStore> _youTubeDataStoreMoq;
        private YouTubeDataStore _youTubeDataStore;
        private Mock<LocalVideoDataStore> _localVideoStoreMoq;
        private LocalVideoDataStore _localVideoStore;
        private Mock<VideoDataStoreService> _videoDataStoreServiceMoq;

        [OneTimeSetUp]
        public void InstantiateVideoDataStoreService()
        {
            _youTubeDataStoreMoq = new Mock<YouTubeDataStore>();
            _youTubeDataStore = new YouTubeDataStore(new YouTubeRepository(), new YouTubeSearchRepository());
            _localVideoStoreMoq = new Mock<LocalVideoDataStore>();
            _localVideoStore = new LocalVideoDataStore(new VideoLabMongoDbRepository());

            var videoStoresMoq = new List<IVideoDataStore> {_youTubeDataStore, _localVideoStore};

            _videoDataStoreServiceMoq = new Mock<VideoDataStoreService>(videoStoresMoq);
        }

        [Test]
        public void Should_SkipLocalVideoStoreIfNonIsProvided()
        {
            var externalSource = new YouTubeDataStore(new YouTubeRepository(), new YouTubeSearchRepository());
            var service = new VideoDataStoreService(new List<IVideoDataStore> {externalSource})
            {
                ResultCount = 1
            };

            var result = service.ListVideosByTags(new List<string> {"Something"});

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void Should_ReturnYouTubeFromIdWithAlphanumericId()
        {
            var result = _videoDataStoreServiceMoq.Object.DetermineDataStoreFromId("YVgc2PQd_bo");
            Assert.AreEqual(_youTubeDataStore.GetType(), result.GetType());
        }

        [Test]
        public void Should_LoadVideoFromLocalStore()
        {
//            _localVideoStoreMoq.Setup(x => x.VideoExists("something")).Returns(true);
//
//            var result = _videoDataStoreServiceMoq.Object.LoadVideo("something");

            Assert.Pass("Test not implemented yet");
        }
    }
}
