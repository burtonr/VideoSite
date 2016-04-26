namespace VideoLab.Repository.Models
{
    public class VideoLabComment
    {
        public string VideoLabVideoId { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public VideoLabComment ReplyComment { get; set; }
    }
}