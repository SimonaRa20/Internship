using BlogWeb.Models.Artical;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace BlogWeb.Pages.Articals
{
    public class EditArticleModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public EditArticleModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _config = configuration;
        }

        [BindProperty]
        public ArticleResponse Artical { get; set; }

        [BindProperty]
        public ArticleRequest UpdateArtical { get; set; }

        [BindProperty]
        public int setArticalId {  get; set; }

        public async Task<IActionResult> OnGetAsync(int articalId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var token = User.FindFirstValue("Token");
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    setArticalId = articalId;
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var apiUrl = $"{_config["ApiUrl"]}/Article/{articalId}";

                    var response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        Artical = await response.Content.ReadFromJsonAsync<ArticleResponse>();

                        UpdateArtical = new ArticleRequest
                        {
                            Title = Artical.Title,
                            Text = Artical.Text,
                        };
                    }
                    else
                    {
                        return RedirectToPage("/NotFound");
                    }

                    return Page();
                }
            }
            else
            {
                return RedirectToPage("/");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if(User.Identity.IsAuthenticated)
                {
                    var token = User.FindFirstValue("Token");
                    using (var httpClient = _httpClientFactory.CreateClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                        var apiUrl = _config["ApiUrl"] + $"/Article/{setArticalId}";

                        using (var content = new MultipartFormDataContent())
                        {
                            content.Add(new StringContent(UpdateArtical.Title), "Title");
                            if (UpdateArtical.Text != null)
                            {
                                content.Add(new StringContent(UpdateArtical.Text), "Text");
                            }

                            if (UpdateArtical.Image != null)
                            {
                                var imageContent = new StreamContent(UpdateArtical.Image.OpenReadStream());
                                content.Add(imageContent, "Image", UpdateArtical.Image.FileName);
                            }

                            var response = await httpClient.PutAsync(apiUrl, content);

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
