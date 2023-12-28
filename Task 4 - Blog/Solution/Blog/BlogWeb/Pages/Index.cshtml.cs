using BlogWeb.Models.Artical;
using BlogWeb.Models.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Security.Claims;

namespace BlogWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        [BindProperty]
        public string VotedArtical {  get; set; } = string.Empty;
        [BindProperty]
        public int Vote { get; set; } = 0;
        public List<ArticleResponse> Articals { get; set; }
        public List<ArticleResponse> ArticalsByRank { get; set; }
        public List<ArticleResponse> ArticalsByComment{ get; set; }
        public PermissionResponse UserPermissions { get; set; } = new PermissionResponse();

        public IndexModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _config = configuration;
        }

        public async Task<IActionResult> OnGetAsync(string searchQuery)
        {
            var token = User.FindFirstValue("Token");
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var apiUrl = _config["ApiUrl"] + "/Article";

                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    Articals = await response.Content.ReadFromJsonAsync<List<ArticleResponse>>();
                }

                Articals = Articals.TakeLast(5).ToList();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    Articals = Articals.Where(x=>x.Title.Contains(searchQuery)).ToList();
                }

                var apiUrlForRank = _config["ApiUrl"] + "/Article/byRank";

                var responseRank = await httpClient.GetAsync(apiUrlForRank);

                if (responseRank.IsSuccessStatusCode)
                {
                    ArticalsByRank = await responseRank.Content.ReadFromJsonAsync<List<ArticleResponse>>();
                }

                var apiUrlForComments = _config["ApiUrl"] + "/Article/byComments";

                var responseComments = await httpClient.GetAsync(apiUrlForComments);

                if (responseComments.IsSuccessStatusCode)
                {
                    ArticalsByComment = await responseComments.Content.ReadFromJsonAsync<List<ArticleResponse>>();
                }

                if (User.Identity.IsAuthenticated)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var userPermissionsApiUrl = _config["ApiUrl"] + $"/Permissions/{userId}";

                    var userPermissionResponse = await httpClient.GetAsync(userPermissionsApiUrl);

                    if (userPermissionResponse.IsSuccessStatusCode)
                    {
                        UserPermissions = await userPermissionResponse.Content.ReadFromJsonAsync<PermissionResponse>();
                        return Page();
                    }
                }


            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var token = User.FindFirstValue("Token");
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var voteApiUrl = $"{_config["ApiUrl"]}/Article/VoteArticle/{VotedArtical}/{userId}/{Vote}";

                    var response = await httpClient.PutAsync(voteApiUrl, null);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        return BadRequest($"Failed to vote: {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}