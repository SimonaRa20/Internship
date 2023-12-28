using BlogWeb.Models.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Security.Claims;

namespace BlogWeb.Pages.Permissions
{
    public class EditPermissionsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        [BindProperty]
        public string UserId { get; set; }

        [BindProperty]
        public string WriteArticals { get; set; }

        [BindProperty]
        public string RateArticals { get; set; }

        [BindProperty]
        public string WriteComments { get; set; }

        public string UserName { get; set; }
        public EditPermissionsModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _config = configuration;
        }

        public List<SelectListItem> PermissionOptions { get; } = new List<SelectListItem>
        {
            new SelectListItem("True", "true"),
            new SelectListItem("False", "false")
        };

        [BindProperty]
        public PermissionRequest PermissionResponse { get; set; }

        public async Task<IActionResult> OnGet(string userId)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var token = User.FindFirstValue("Token");
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var apiUrl = _config["ApiUrl"]+ $"/Permissions/{userId}";
                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var permissions = await response.Content.ReadFromJsonAsync<PermissionResponse>();

                    UserId = permissions.UserId.ToString();
                    UserName = permissions.UserName;
                    WriteArticals = permissions.WriteArticals.ToString();
                    RateArticals = permissions.RateArticals.ToString();
                    WriteComments = permissions.WriteComments.ToString();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode}");
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    var token = User.FindFirstValue("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var apiUrl = _config["ApiUrl"] + $"/Permissions/{UserId}";

                    var response = await httpClient.PutAsJsonAsync(apiUrl, PermissionResponse);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("./Permissions");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Page();
        }
    }
}
