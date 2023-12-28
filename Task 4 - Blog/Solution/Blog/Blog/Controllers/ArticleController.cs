using AutoMapper;
using Blog.Contracts.Artical;
using Blog.Contracts.Permissions;
using Blog.Interfaces;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArticleController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IPermissionsRepository _permissionsRepository;
        private readonly IMapper _mapper;

        public ArticleController(IArticleRepository articleRepository, IMapper mapper, IPermissionsRepository permissionsRepository)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
            _permissionsRepository = permissionsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetArticals()
        {
            try
            {
                var articals = await _articleRepository.GetAll();

                if (articals == null)
                {
                    return NotFound("Articals not found");
                }

                var articleResponses = _mapper.Map<List<ArticleResponse>>(articals);

                foreach (var article in articleResponses)
                {
                    article.TotalVotes = await _articleRepository.GetTotalVotes(article.Id);
                }

                return Ok(articleResponses);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while fetching articals: {ex.Message}");
            }
        }

        [HttpGet("byRank")]
        public async Task<IActionResult> GetArticalsOrderByRank()
        {
            try
            {
                var articals = await _articleRepository.GetAll();

                if (articals == null)
                {
                    return NotFound("Articals not found");
                }

                var articleResponses = _mapper.Map<List<ArticleResponse>>(articals);

                foreach (var article in articleResponses)
                {
                    article.TotalVotes = await _articleRepository.GetTotalVotes(article.Id);
                }

                List<ArticleResponse> top3Articles = (List<ArticleResponse>)articleResponses.OrderByDescending(x => x.TotalVotes).Take(3).ToList();

                return Ok(top3Articles);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while fetching articals: {ex.Message}");
            }
        }

        [HttpGet("byComments")]
        public async Task<IActionResult> GetArticalsByComments()
        {
            try
            {
                var articals = await _articleRepository.GetAll();

                if (articals == null)
                {
                    return NotFound("Articals not found");
                }

                var last3CommentedArticles = articals
                    .Include(article => article.ArticleComments)
                    .OrderByDescending(article => article.ArticleComments.Max(Comment => Comment.CreatedDate))
                    .Take(3);

                var articleResponses = _mapper.Map<List<ArticleResponse>>(last3CommentedArticles);

                foreach (var article in articleResponses)
                {
                    article.TotalVotes = await _articleRepository.GetTotalVotes(article.Id);
                }

                List<ArticleResponse> top3Articles = articleResponses.ToList();

                if(last3CommentedArticles == null)
                {
                    return Ok(null);
                }
                else
                {
                    var articleResponse = _mapper.Map<List<ArticleResponse>>(top3Articles);

                    return Ok(articleResponse);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while fetching articals: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateArtical([FromForm] ArticleRequest articalRequest)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                PermisionResponse permissions = await _permissionsRepository.GetPermissionsByUserId(userId);

                if(!permissions.WriteArticals)
                {
                    return Forbid();
                }

                if (articalRequest.Image != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await articalRequest.Image.CopyToAsync(memoryStream);

                        var article = new Article
                        {
                            Image = memoryStream.ToArray(),
                            Title = articalRequest.Title,
                            Text = articalRequest.Text,
                            CreatedDate = DateTime.Now,
                        };

                        var arti = await _articleRepository.Add(article);
                    }
                }
                else
                {
                    var article = new Article
                    {
                        Title = articalRequest.Title,
                        Text = articalRequest.Text
                    };
                    await _articleRepository.Add(article);
                }

                await _articleRepository.Save();

                return Ok("Article created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while creating the article: {ex.Message}");
            }
        }

        [HttpGet("{articalId}")]
        public async Task<IActionResult> GetArtical(int articalId)
        {
            try
            {
                var artical = await _articleRepository.GetById(articalId);

                if (artical == null)
                {
                    return NotFound($"Article with ID {articalId} not found");
                }

                var articalResponses = _mapper.Map<ArticleResponse>(artical);

                return Ok(articalResponses);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while deleting the article: {ex.Message}");
            }
        }

        [HttpDelete("{articalId}")]
        [Authorize]
        public async Task<IActionResult> DeleteArtical(int articalId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                PermisionResponse permissions = await _permissionsRepository.GetPermissionsByUserId(userId);

                if (!permissions.WriteArticals)
                {
                    return Forbid();
                }

                await _articleRepository.Delete(articalId);
                await _articleRepository.Save();

                return Ok($"Article with ID {articalId} deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while deleting the article: {ex.Message}");
            }
        }

        [HttpPut("{articalId}")]
        [Authorize]
        public async Task<IActionResult> UpdateArtical(int articalId, [FromForm] ArticleRequest articalRequest)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                PermisionResponse permissions = await _permissionsRepository.GetPermissionsByUserId(userId);

                if (!permissions.WriteArticals)
                {
                    return Forbid();
                }

                var artical = await _articleRepository.GetById(articalId);

                if (artical == null)
                {
                    return NotFound($"Article with ID {articalId} not found");
                }

                if (articalRequest.Image != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await articalRequest.Image.CopyToAsync(memoryStream);
                        artical.Image = memoryStream.ToArray();
                    }
                }

                artical.Id = articalId;
                artical.Title = articalRequest.Title;
                artical.Text = articalRequest.Text;

                await _articleRepository.Update(artical);
                await _articleRepository.Save();

                return Ok($"Article with ID {articalId} updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating the article: {ex.Message}");
            }
        }

        [HttpPut("VoteArticle/{articleId}/{userId}/{voteValue}")]
        [Authorize]
        public async Task<IActionResult> VoteArticle(int articleId, int userId, int voteValue)
        {
            try
            {
                var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                PermisionResponse permissions = await _permissionsRepository.GetPermissionsByUserId(id);

                if (!permissions.RateArticals)
                {
                    return Forbid();
                }

                var voteArticle = await _articleRepository.VoteArtical(articleId, userId, voteValue);
                if (voteArticle)
                {
                    return Ok(voteArticle);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
