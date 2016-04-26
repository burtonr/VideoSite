using System;
using System.Linq;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace VideoLab.Repository.APIStorage.Repositories
{
    public class YouTubeRepository : IYouTubeRepository
    {
        protected YouTubeService VideoLabYouTubeService;
        protected bool Initialized;

        public void Initialize()
        {
            var youTubeApiKey = Properties.Settings.Default.YouTubeApiKey;
            var youTubeApplicationName = Properties.Settings.Default.YouTubeApplicationName;

            if (string.IsNullOrEmpty(youTubeApiKey) || string.IsNullOrEmpty(youTubeApplicationName))
            {
                throw new ApplicationException("The YouTube API key, or Application Name could not be found. Check the project properties settings.");
            }

            VideoLabYouTubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = youTubeApiKey,
                ApplicationName = youTubeApplicationName
            });

            Initialized = true;
        }

        public Video Get(string videoId)
        {
            if(!Initialized) Initialize();

            var request = VideoLabYouTubeService.Videos.List("snippet, Id");

            request.Id = videoId;

            var response = request.Execute();

            return response.Items.FirstOrDefault();
        }
    }
}