using Blog.Contracts.Comment;
using Blog.Database;
using Blog.Interfaces;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogDbContext _dbContext;

        public CommentRepository(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Article article)
        {
            _dbContext.Entry(article).State = EntityState.Modified;
        }

        public async Task<bool> VoteArtical(int articleId, int userId, int voteValue)
        {
            var article = await _dbContext.Articles
                .Include(x => x.ArticleRates)
                .FirstOrDefaultAsync(x => x.Id == articleId);

            if (article == null)
            {
                return false;
            }

            var existingVote = article.ArticleRates.FirstOrDefault(x => x.UserId == userId);

            if (existingVote != null)
            {
                existingVote.Value = voteValue;
            }
            else
            {
                var newVote = new ArticleRate
                {
                    Value = voteValue,
                    UserId = userId
                };
                article.ArticleRates.Add(newVote);
            }

            await Save();
            return true;
        }

        public async Task AddComment(CommentRequest commentRequest)
        {
            var comment = new Comment
            {
                Text = commentRequest.Text,
                ArticleId = commentRequest.ArticleId,
                UserId = commentRequest.UserId,
                CreatedDate = DateTime.Now
            };

            await _dbContext.Comments.AddAsync(comment);
        }

        public async Task DeleteComment(int commentId)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == commentId);

            _dbContext.Comments.Remove(comment);
        }

        public async Task<List<CommentResponse>> GetCommentsArticle(int articleId)
        {
            var comments = await _dbContext.Comments
                .Where(x => x.ArticleId == articleId)
                .ToListAsync();

            var commentResponse = comments.Select(comment => new CommentResponse
            {
                Id = comment.Id,
                UserId = comment.UserId,
                ArticleId = comment.ArticleId,
                Text = comment.Text,
                UserName = GetUserName(comment.UserId),
                CreatedDate = comment.CreatedDate,
                IsReported = comment.IsReported,
                IsBlocked = comment.IsBlocked,
            }).ToList();

            return commentResponse;
        }

        private string GetUserName(int userId)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            return user.UserName;
        }

        public async Task Update(Comment comment)
        {
            _dbContext.Entry(comment).State = EntityState.Modified;
        }

        public async Task<Comment> GetCommentById(int id)
        {
            return await _dbContext.Comments.FindAsync(id);
        }

        public async Task ReportComment(int commentId)
        {
            var comment = await _dbContext.Comments.FindAsync(commentId);

            if(comment != null)
            {
                comment.IsReported = true;
            }
        }

        public async Task BlockComment(int commentId)
        {
            var comment = await _dbContext.Comments.FindAsync(commentId);

            if (comment != null)
            {
                comment.IsBlocked = true;
            }
        }
    }
}
