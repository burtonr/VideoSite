using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using VideoLab.Repository.Models;

namespace VideoLab.Repository.VideoLabStorage
{
    public static class VideoLabExtensions
    {
        public static BsonDocument VideoLabVideoToBson(this VideoLabVideo videoLabVideo)
        {
            MapVideoLabVideoToBson();

            return videoLabVideo.ToBsonDocument();
        }

        public static VideoLabVideo ToVideoLabVideo(this BsonDocument document)
        {
            MapVideoLabVideoToBson();

            return BsonSerializer.Deserialize<VideoLabVideo>(document);
        }

        public static IList<VideoLabVideo> ListToVideoLabVideos(this IList<BsonDocument> documentList)
        {
            return documentList.Select(doc => doc.ToVideoLabVideo()).ToList();
        }

        private static void MapVideoLabVideoToBson()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof (VideoLabVideo)))
            {
                BsonClassMap.RegisterClassMap<VideoLabVideo>(map =>
                {
                    map.AutoMap();
                    map.MapIdField(x => x.VideoId);
                });
            }
        }
    }
}
