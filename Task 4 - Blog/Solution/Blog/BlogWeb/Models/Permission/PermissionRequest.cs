namespace BlogWeb.Models.Permission
{
    public class PermissionRequest
    {
        public bool WriteArticals { get; set; }
        public bool RateArticals { get; set; }
        public bool WriteComments { get; set; }
    }
}
