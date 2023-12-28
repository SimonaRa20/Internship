namespace BlogWeb.Models.Artical
{
    public class ArticleResponse
    {
        public int Id { get; set; }
        public byte[]? Image { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public int TotalVotes { get; set; }
    }
}
