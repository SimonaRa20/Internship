using BlogWeb.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlogWeb.Pages.Auth
{
    public class RestorePasswordModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        [BindProperty]
        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }

        [BindProperty]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Token { get; set; }

        private UserRestorePassword userRestorePassword { get; set; } = new UserRestorePassword();

        public RestorePasswordModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public IActionResult OnGet(string token)
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var apiUrl = _config["ApiUrl"] + "/Auth/RestoreForgotPassword";

                userRestorePassword.Token = Token;
                userRestorePassword.Password = NewPassword;

                var response = await httpClient.PutAsJsonAsync(apiUrl, userRestorePassword);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Auth/Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email, username, or password. Please try again.");
                    return Page();
                }
            }
        }
    }
}
