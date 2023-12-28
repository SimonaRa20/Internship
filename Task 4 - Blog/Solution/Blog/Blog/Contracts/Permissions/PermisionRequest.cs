namespace Blog.Contracts.Permissions
{
    public class PermisionRequest
    {
        public bool WriteArticals { get; set; }
        public bool RateArticals { get; set; }
        public bool WriteComments { get; set; }
    }
}
