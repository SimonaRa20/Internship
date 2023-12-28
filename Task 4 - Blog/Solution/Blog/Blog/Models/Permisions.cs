namespace Blog.Models
{
    public class Permisions
    {
        public int Id { get; set; }
        public bool WriteArticals { get; set; }
        public bool RateArticals { get; set; }
        public bool WriteComments { get; set; }
        public int UserId { get; set; }
    }
}
