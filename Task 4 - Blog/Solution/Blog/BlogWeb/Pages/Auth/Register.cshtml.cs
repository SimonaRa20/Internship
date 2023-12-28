using BlogWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;

namespace BlogWeb.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        [BindProperty]
        public UserRegisterRequest UserRegisterRequest { get; set; }

        public RegisterModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var apiUrl = _config["ApiUrl"] + "/Auth/Register";

                var response = await httpClient.PostAsJsonAsync(apiUrl, UserRegisterRequest);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Auth/Login");
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
