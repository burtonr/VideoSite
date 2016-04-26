using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using VideoLab.Repository.Models;

namespace VideoLab.Repository.IntegrationTests
{
    /// <summary>
    /// Test classes named with ??_ prefix to ensure order is followed (insert first, then retrieve...)
    /// </summary>
    [TestFixture]
    public class MongoDbTests
    {
        private VideoLabVideo _testVideo;
        private TestMongoDbRepository _mongoRepo;
        private MongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;

        [OneTimeSetUp]
        public void MongoTestSetup()
        {

            _mongoRepo = new TestMongoDbRepository();

            _testVideo = new VideoLabVideo()
            {
                VideoId = "TestVideoID",
                Title = "Test Video Insert",
                Description = "This is only a test record to make sure that the insert function works as it should",
                VideoImageUrl = "HTTP://www.somewebsite/withimages/videoImage.img",
                Duration = 50000,
                PublishedDate = DateTime.Today,
                ViewCount = 42,
                Tags = new List<string>
                {
                    "Tag1", "Tag2", "Tag3", "Some other really long tag", "New Tag after update"
                },
                Comments = new List<VideoLabComment>
                {
                    new VideoLabComment()
                    {
                        Author = "Writer", Text = "This is a comment", VideoLabVideoId = "TestVideoID"
                    },
                    new VideoLabComment()
                    {
                        Author = "AnotherWriter", Text = "Another commnet on the same video", VideoLabVideoId = "TestVideoID"
                    }
                }
            };
        }

        [Test]
        public void AA_CanConnectToMongo_DefaultConnection()
        {
            _mongoClient = new MongoClient();

            Assert.NotNull(_mongoClient);
        }

        [Test]
        public void AB_SaveVideoLabVideoToMongo()
        {
            _mongoRepo.Save(_testVideo);
        }

        [Test]
        public void BB_LoadVideoLabVideo_FromMongo_ByID()
        {
            var result = _mongoRepo.Get("TestVideoID");

            Assert.AreSame(typeof (VideoLabVideo), result.GetType());
            Assert.AreEqual("TestVideoID", result.VideoId);
            Assert.AreEqual("Test Video Insert", result.Title);
        }

        [Test]
        [TestCase("Tag1", "Tag2")]
        [TestCase("Tag1", "Tag3")]
        [TestCase("Tag1", "Some other really long tag")]
        public void BC_LoadVideoLabVideo_FromMongo_ByTags(string testTag1, string testTag2)
        {
            var testTags = new List<string> {testTag1, testTag2};

            var resultSet = _mongoRepo.Search(testTags, 10);

            var videoLabResults = resultSet;

            Assert.IsNotEmpty(videoLabResults);
            Assert.NotNull(videoLabResults.FirstOrDefault());
        }

        [Test]
        [TestCase("Tag1", "Tag99")]
        [TestCase("Tag1", "tag3")]
        [TestCase("Tag1", "Some tag that doesnt exist in this video")]
        public void BD_Not_LoadVideoLabVideo_FromMongo_ByTags(string testTag1, string testTag2)
        {
            var testTags = new List<string> { testTag1, testTag2 };

            var resultSet = _mongoRepo.Search(testTags, 10);

            var videoLabResults = resultSet;

            Assert.IsEmpty(videoLabResults);
            Assert.Null(videoLabResults.FirstOrDefault());
        }

        [Test]
        [TestCase("Test Video Insert", 5)]
        [TestCase("Test Video Insert", 0)]
        public void BE_LoadVideoLabVideo_FromMongo_ByTitle(string title, int results)
        {
            var resultSet = results == 0 ? 
                _mongoRepo.Search(title) : _mongoRepo.Search(title, results);

            Assert.IsNotEmpty(resultSet);
            Assert.NotNull(resultSet.FirstOrDefault());
        }

        [OneTimeTearDown]
        public void CleanTestDb()
        {
            if(_mongoClient == null) _mongoClient = new MongoClient();

            _mongoDatabase = _mongoClient.GetDatabase("test");
            var collection = _mongoDatabase.GetCollection<VideoLabVideo>(_mongoDatabase.DatabaseNamespace.DatabaseName);

            collection.DeleteMany(new BsonDocument());
        }
    }
}
