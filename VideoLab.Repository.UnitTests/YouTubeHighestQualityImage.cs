using Google.Apis.YouTube.v3.Data;
using NUnit.Framework;
using VideoLab.Repository.APIStorage.DataStores;
using VideoLab.Repository.APIStorage.Repositories;

namespace VideoLab.Repository.UnitTests
{
    public class YouTubeHighestQualityImage
    {
        private YouTubeDataStore _youTubeDataStore;

        [OneTimeSetUp]
        public void Setup()
        {
            _youTubeDataStore = new YouTubeDataStore(new YouTubeRepository(), new YouTubeSearchRepository());
        }

        [Test]
        public void ImageURL_isMaxNotDefault()
        {
            var thumbnailDetailFake = new ThumbnailDetails()
            {
                Maxres = new Thumbnail() { Url = "MaxResUrl" },
                Default__ = new Thumbnail() { Url = "Default" }
            };

            var result = _youTubeDataStore.GetHighestQualityImage(thumbnailDetailFake);

            Assert.AreEqual(result, thumbnailDetailFake.Maxres.Url);
        }

        [Test]
        public void ImageURL_isHighNotDefault()
        {
            var thumbnailDetailFake = new ThumbnailDetails()
            {
                High = new Thumbnail() { Url = "HighUrl" },
                Default__ = new Thumbnail() { Url = "Default" }
            };

            var result = _youTubeDataStore.GetHighestQualityImage(thumbnailDetailFake);

            Assert.AreEqual(result, thumbnailDetailFake.High.Url);
        }

        [Test]
        public void ImageURL_isDefaultIfNoOther()
        {
            var thumbnailDetailFake = new ThumbnailDetails()
            {
                Default__ = new Thumbnail() { Url = "Default" }
            };

            var result = _youTubeDataStore.GetHighestQualityImage(thumbnailDetailFake);

            Assert.AreEqual(result, thumbnailDetailFake.Default__.Url);
        }
    }
}