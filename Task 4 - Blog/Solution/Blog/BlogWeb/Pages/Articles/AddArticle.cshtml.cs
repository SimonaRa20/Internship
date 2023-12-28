using BlogWeb.Models.Artical;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BlogWeb.Pages.Articals
{
    public class AddArticleModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public AddArticleModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _config = configuration;
        }

        [BindProperty]
        public ArticleRequest Artical { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var token = User.FindFirstValue("Token");
                    using (var httpClient = _httpClientFactory.CreateClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        var apiUrl = _config["ApiUrl"] + "/Article";

                        using (var content = new MultipartFormDataContent())
                        {
                            if (Artical.Title != null)
                            {
                                content.Add(new StringContent(Artical.Title), "Title");
                            }

                            if (Artical.Text != null)
                            {
                                content.Add(new StringContent(Artical.Text), "Text");
                            }


                            if (Artical.Image != null)
                            {
                                using (var stream = new MemoryStream())
                                {
                                    await Artical.Image.CopyToAsync(stream);
                                    content.Add(new ByteArrayContent(stream.ToArray()), "Image", Artical.Image.FileName);
                                }
                            }

                            var response = await httpClient.PostAsync(apiUrl, content);

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
