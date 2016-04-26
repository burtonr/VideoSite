using System;
using System.Diagnostics;
using System.IO;
using Google.Apis.YouTube.v3.Data;
using NUnit.Framework;
using VideoLab.Repository.APIStorage.DataStores;
using VideoLab.Repository.APIStorage.Repositories;

namespace VideoLab.Repository.UnitTests
{
    [TestFixture]
    public class YouTubeDataStoreTests
    {
        [Test]
        public void AppSettings_NotNull()
        {
            var youTubeKey = Properties.Settings.Default.YouTubeApiKey;
            var youTubeAppName = Properties.Settings.Default.YouTubeApplicationName;
            
            Assert.IsNotNull(youTubeKey);
            Assert.IsNotNull(youTubeAppName);
        }

        [Test]
        public void YouTubeDataStore_ShouldConstruct()
        {
            try
            {
                var youTubestore = new YouTubeDataStore(new YouTubeRepository(), new YouTubeSearchRepository());
                Assert.IsNotNull(youTubestore);
            }
            catch (Exception e)
            {
                Debug.Write(e.InnerException);
                Assert.Fail("Could not load YouTubeDataStore Service. Exception message: " + e);
            }
        }

        [Test]
        public void InvalidVideoID_ShouldThrowException()
        {
            var youtubeStore = new YouTubeDataStore(new YouTubeRepository(), new YouTubeSearchRepository());

            Assert.Throws<FileNotFoundException>(() => youtubeStore.LoadVideo("BadVideoID"));
        }

        [Test]
        public void TranslateFromSearchResult()
        {
            var searchresultFake = new SearchResult()
            {
                Id = new ResourceId() { VideoId = "TestVideoID" },
                Snippet = new SearchResultSnippet()
                {
                    Title = "TestVideoTitle",
                    Description = "TestVideoDescription", 
                    Thumbnails = new ThumbnailDetails() {  Default__ = new Thumbnail() { Url = "TestVideoDefaultImageUrl" } }
                }
            };

            var youTubeStore = new YouTubeDataStore(new YouTubeRepository(), new YouTubeSearchRepository());

            var result = youTubeStore.TranslateSearchResultToVideoLabVideo(searchresultFake);

            Assert.AreEqual(searchresultFake.Id.VideoId, result.VideoId);
            Assert.AreEqual(searchresultFake.Snippet.Title, result.Title);
            Assert.AreEqual(searchresultFake.Snippet.Description, result.Description);
        }
    }
}