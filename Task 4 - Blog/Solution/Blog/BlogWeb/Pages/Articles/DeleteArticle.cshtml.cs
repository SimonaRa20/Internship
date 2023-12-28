using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BlogWeb.Pages.Articals
{
    public class DeleteArticleModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public DeleteArticleModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _config = configuration;
        }

        [BindProperty(SupportsGet = true)]
        public int ArticalId { get; set; }

        public void OnGet()
        {
            // Additional setup logic if needed
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var token = User.FindFirstValue("Token");
                    using (var httpClient = _httpClientFactory.CreateClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        var apiUrl = $"{_config["ApiUrl"]}/Article/{ArticalId}";

                        var response = await httpClient.DeleteAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToPage("/Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode}");
                            return Page();
                        }
                    }
                }
                else
                {
                    return RedirectToPage("/");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }
    }
}
