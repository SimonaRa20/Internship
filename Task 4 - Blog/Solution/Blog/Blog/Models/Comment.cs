namespace Blog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsReported { get; set; }
        public bool IsBlocked { get; set; }
        public int ArticleId { get; set; }
        public int UserId { get; set; }
    }
}
