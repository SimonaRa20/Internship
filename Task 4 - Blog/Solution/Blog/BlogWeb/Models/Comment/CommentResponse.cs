using System.Runtime.Serialization;

namespace BlogWeb.Models.Comment
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public bool IsReported { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
