namespace Blog.Models
{
    public class ArticleRate
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public int ArticleId { get; set; }
        public int UserId { get; set; }
    }
}
