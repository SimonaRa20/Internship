using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlogWeb.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BlogWeb.Pages.Profile
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        [BindProperty]
        public UserChangePassword UserChangePassword { get; set; }

        public ChangePasswordModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _config = configuration;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var httpClient = _httpClientFactory.CreateClient())
                    {
                        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        var apiUrl = _config["ApiUrl"] + "/User/ChangePassword/" + userId;

                        var response = await httpClient.PutAsJsonAsync(apiUrl, UserChangePassword);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToPage("/Profile/Profile");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                }
            }

            return Page();
        }
    }
}
