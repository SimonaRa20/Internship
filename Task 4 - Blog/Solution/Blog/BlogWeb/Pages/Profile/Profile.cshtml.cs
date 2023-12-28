using BlogWeb.Models.Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace BlogWeb.Pages.Profile
{
    public class ProfileModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        [BindProperty]
        public UserProfileResponse UserProfile { get; set; }

        public ProfileModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _config = configuration;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var token = User.FindFirstValue("Token");
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var apiUrl = _config["ApiUrl"] + "/User/" + userId;

                    var response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        UserProfile = await response.Content.ReadFromJsonAsync<UserProfileResponse>();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode}");
                    }
                    return Page();
                }
            }
            return Redirect("/");    
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var token = User.FindFirstValue("Token");
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var apiUrl = _config["ApiUrl"] + "/User/Update/" + userId;

                    var response = await httpClient.PutAsJsonAsync(apiUrl, UserProfile);

                    if (response.IsSuccessStatusCode)
                    {
                        UserProfile = await response.Content.ReadFromJsonAsync<UserProfileResponse>();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode}");
                    }
                    return Page();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
