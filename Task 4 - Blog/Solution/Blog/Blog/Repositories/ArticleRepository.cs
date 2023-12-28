using Blog.Database;
using Blog.Interfaces;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly BlogDbContext _dbContext;

        public ArticleRepository(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Article> Add(Article article)
        {
            await _dbContext.Articles.AddAsync(article);
            await Save();
            return article;
        }

        public async Task Delete(int id)
        {
            Article article = await _dbContext.Articles.FindAsync(id);
            if (article != null)
            {
                _dbContext.Articles.Remove(article);
            }
            await Save();
        }

        public async Task<IQueryable<Article>> GetAll()
        {
            return _dbContext.Articles.Include(a=>a.ArticleComments);
        }

        public async Task<Article> GetById(int id)
        {
            return await _dbContext.Articles.FindAsync(id);
        }

        public async Task<int> GetTotalVotes(int articleId)
        {
            return await _dbContext.ArticleRates.Where(x=>x.ArticleId == articleId).SumAsync(v=>v.Value);
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Article article)
        {
            _dbContext.Entry(article).State = EntityState.Modified;
            await Save();
        }

        public async Task<bool> VoteArtical(int articleId, int userId, int voteValue)
        {
            var article = await _dbContext.Articles
                .Include(x=>x.ArticleRates)
                .FirstOrDefaultAsync(x=>x.Id == articleId);

            if (article == null)
            {
                return false;
            }

            var existingVote = article.ArticleRates.FirstOrDefault(x=>x.UserId == userId);

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
    }
}
