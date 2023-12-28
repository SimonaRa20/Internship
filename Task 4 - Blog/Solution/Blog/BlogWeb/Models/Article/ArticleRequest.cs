namespace BlogWeb.Models.Artical
{
    public class ArticleRequest
    {
        public IFormFile? Image { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
    }
}
