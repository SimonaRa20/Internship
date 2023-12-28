namespace Blog.Models
{
    public class Article
    {
        public int Id { get; set; }
        public byte[]? Image { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ArticleRate> ArticleRates { get; set;}
		public List<Comment> ArticleComments { get; set; }
	}
}
