using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace BlogWeb.Pages.Auth
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public ForgotPasswordModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var apiUrl = _config["ApiUrl"] + "/Auth/RestorePassword";

                var response = await httpClient.PostAsJsonAsync(apiUrl, Email);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./CheckEmail");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email, username or password. Please try again.");
                    return Page();
                }
            }
        }
    }
}
