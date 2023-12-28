using BlogWeb.Models.Artical;
using BlogWeb.Models.Comment;
using BlogWeb.Models.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BlogWeb.Pages.Articles
{
    public class DetailedArticleModel : PageModel
    {
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _config;

		public DetailedArticleModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_config = configuration;
		}

		[BindProperty]
		public ArticleResponse Artical { get; set; }
		[BindProperty]
		public List<CommentResponse> Comments { get; set; }

		[BindProperty]
		public CommentRequest NewComment { get; set; }
		public PermissionResponse UserPermissions { get; set; } = new PermissionResponse();
        [BindProperty]
        public int CommentId { get; set; }

        [BindProperty]
        public string EditedCommentText { get; set; }
        public int UserId { get; set; }
        public async Task<IActionResult> OnGetAsync(int articalId)
		{
			using (var httpClient = _httpClientFactory.CreateClient())
			{
				var apiUrl = $"{_config["ApiUrl"]}/Article/{articalId}";

				var response = await httpClient.GetAsync(apiUrl);

				if (response.IsSuccessStatusCode)
				{
					Artical = await response.Content.ReadFromJsonAsync<ArticleResponse>();

					var commentsResponse = await httpClient.GetAsync($"{_config["ApiUrl"]}/Comment/{articalId}");
					if(commentsResponse.IsSuccessStatusCode)
					{
						Comments = await commentsResponse.Content.ReadFromJsonAsync<List<CommentResponse>>();
					}
				}
				else
				{
					return RedirectToPage("/NotFound");
				}

				if (User.Identity.IsAuthenticated)
				{
					var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
					UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                    var userPermissionsApiUrl = _config["ApiUrl"] + $"/Permissions/{userId}";
                    var token = User.FindFirstValue("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var userPermissionResponse = await httpClient.GetAsync(userPermissionsApiUrl);

					if (userPermissionResponse.IsSuccessStatusCode)
					{
						UserPermissions = await userPermissionResponse.Content.ReadFromJsonAsync<PermissionResponse>();
						return Page();
					}
				}

				return Page();
			}
		}

		public async Task<IActionResult> OnPost(int articalId)
		{
			if(User.Identity.IsAuthenticated)
			{
                var token = User.FindFirstValue("Token");
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var apiUrl = $"{_config["ApiUrl"]}/Comment";

                    NewComment.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    NewComment.ArticleId = articalId;

                    var response = await httpClient.PostAsJsonAsync(apiUrl, NewComment);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("/Articles/DetailedArticle", new { articalId });
                    }
                    else
                    {

                    }
                }
                return Page();
            }
			else
            {
                return RedirectToPage("/");
            }
		}

        public async Task<IActionResult> OnPostDeleteCommentAsync(int articalId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var token = User.FindFirstValue("Token");
                using (var httpClient = _httpClientFactory.CreateClient())
                {

                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var apiUrl = $"{_config["ApiUrl"]}/Comment/{CommentId}";

                    var response = await httpClient.DeleteAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("/Articles/DetailedArticle", new { articalId });
                    }
                    else
                    {
                        // Handle error
                        return Page();
                    }
                }
            }
            else
            {
                return RedirectToPage("/");
            }
        }
        public async Task<IActionResult> OnPostReportCommentAsync(int articalId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var token = User.FindFirstValue("Token");
                using (var httpClient = _httpClientFactory.CreateClient())
                {

                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var apiUrl = $"{_config["ApiUrl"]}/Comment/{CommentId}/report";

                    var response = await httpClient.PostAsync(apiUrl,null);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("/Articles/DetailedArticle", new { articalId });
                    }
                    else
                    {
                        // Handle error
                        return Page();
                    }
                }
            }
            else
            {
                return RedirectToPage("/");
            }
        }

        public async Task<IActionResult> OnPostBlockCommentAsync(int articalId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var token = User.FindFirstValue("Token");
                using (var httpClient = _httpClientFactory.CreateClient())
                {

                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var apiUrl = $"{_config["ApiUrl"]}/Comment/{CommentId}/block";

                    var response = await httpClient.PostAsync(apiUrl, null);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("/Articles/DetailedArticle", new { articalId });
                    }
                    else
                    {
                        // Handle error
                        return Page();
                    }
                }
            }
            else
            {
                return RedirectToPage("/");
            }
        }

        public async Task<IActionResult> OnPostEditCommentAsync(int articalId)
        {
            if(User.Identity.IsAuthenticated)
            {
                var token = User.FindFirstValue("Token");
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var apiUrl = $"{_config["ApiUrl"]}/Comment/{CommentId}";

                    var response = await httpClient.PutAsJsonAsync(apiUrl, EditedCommentText);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("/Articles/DetailedArticle", new { articalId });
                    }
                    else
                    {
                        // Handle error
                        return Page();
                    }
                }
            }    
            else
            {
                return RedirectToPage("/");
            }
        }

    }
}
