using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using VideoLab.Repository.Models;

namespace VideoLab.Repository.VideoLabStorage
{
    public class VideoLabMongoDbRepository : IVideoLabRepository<VideoLabVideo>
    {
        public MongoClient VideoLabMongoClient { get; set; }
        public IMongoDatabase VideoLabDb { get; set; }
        public IMongoCollection<VideoLabVideo> VideoLabBdCollection { get; set; }
        protected bool Initialized;

        public void Initialize()
        {
            VideoLabMongoClient = new MongoClient();
            VideoLabDb = VideoLabMongoClient.GetDatabase("VideoLabDB");
            VideoLabBdCollection = VideoLabDb.GetCollection<VideoLabVideo>(VideoLabDb.DatabaseNamespace.DatabaseName);
            
            Initialized = true;
        }

        public VideoLabVideo Get(string videoId)
        {
            if(!Initialized) Initialize();

            var searchFilter = new BsonDocument { {"_id", videoId} };

            var dbVideo = VideoLabBdCollection.Find(searchFilter).FirstOrDefault();

            return dbVideo;
        }

        public void Save(VideoLabVideo entityToSave)
        {
            if (!Initialized) Initialize();

            var videoId = entityToSave.VideoId;

            var filter = new BsonDocument { {"_id", videoId} };
            var update = new BsonDocument("$set", entityToSave.ToBsonDocument());
            var options = new UpdateOptions { IsUpsert = true };

            VideoLabBdCollection.UpdateMany(filter, update, options);
            //TODO: use the result of the above command to handle logging errors saving locally
        }

        public IList<VideoLabVideo> Search(IList<string> searchParams, int maxResults)
        {
            if (!Initialized) Initialize();

            return VideoLabBdCollection.Find(x => searchParams.All(search => x.Tags.Contains(search))).ToList();
        }

        public IList<VideoLabVideo> Search(string title, int maxResults)
        {
            if (!Initialized) Initialize();

            FieldDefinition<VideoLabVideo, string> fel = "Title";
            var filter = Builders<VideoLabVideo>.Filter.Eq(fel, title);

            return VideoLabBdCollection.Find(filter).Limit(maxResults).ToList();
        }
    }
}