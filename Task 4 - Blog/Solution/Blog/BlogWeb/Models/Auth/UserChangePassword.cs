using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Models.Auth
{
    public class UserChangePassword
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
