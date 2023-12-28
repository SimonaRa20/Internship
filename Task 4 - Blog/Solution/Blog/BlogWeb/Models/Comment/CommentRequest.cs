namespace BlogWeb.Models.Comment
{
    public class CommentRequest
    {
        public int UserId { get; set; }
        public int ArticleId { get; set; }
        public string Text { get; set; }
    }
}
