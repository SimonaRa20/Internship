using AutoMapper;
using Blog.Contracts.Comment;
using Blog.Contracts.Permissions;
using Blog.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPermissionsRepository _permissionsRepository;
        private readonly IMapper _mapper;

        public CommentController(ICommentRepository commentRepository, IMapper mapper, IPermissionsRepository permissionsRepository)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _permissionsRepository = permissionsRepository;
        }

        [HttpGet("{articleId}")]
        public async Task<IActionResult> GetArticleComments(int articleId)
        {
            try
            {
                var comments = await _commentRepository.GetCommentsArticle(articleId);
                var commentResponse = _mapper.Map<List<CommentResponse>>(comments);

                return Ok(commentResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(CommentRequest comment)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                PermisionResponse permissions = await _permissionsRepository.GetPermissionsByUserId(userId);

                if (!permissions.WriteComments)
                {
                    return Forbid();
                }

                await _commentRepository.AddComment(comment);
                await _commentRepository.Save();

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{commentId}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                PermisionResponse permissions = await _permissionsRepository.GetPermissionsByUserId(userId);

                if (!permissions.WriteComments)
                {
                    return Forbid();
                }

                await _commentRepository.DeleteComment(commentId);
                await _commentRepository.Save();
                return Ok($"Comment with ID {commentId} updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{commentId}")]
        [Authorize]
        public async Task<IActionResult> UpdateArtical(int commentId, [FromBody] string text)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                PermisionResponse permissions = await _permissionsRepository.GetPermissionsByUserId(userId);

                if (!permissions.WriteComments)
                {
                    return Forbid();
                }

                var comment = await _commentRepository.GetCommentById(commentId);

                if (comment == null)
                {
                    return NotFound($"Comment with ID {commentId} not found");
                }


                comment.Text = text;

                await _commentRepository.Update(comment);
                await _commentRepository.Save();

                return Ok($"Comment with ID {commentId} updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating the article: {ex.Message}");
            }
        }

        [HttpPost("{commentId}/report")]
        [Authorize]
        public async Task<IActionResult> ReportComment(int commentId)
        {
            try
            {
                await _commentRepository.ReportComment(commentId);
                await _commentRepository.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{commentId}/block")]
        [Authorize]
        public async Task<IActionResult> BlockComment(int commentId)
        {
            try
            {
                await _commentRepository.BlockComment(commentId);
                await _commentRepository.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
