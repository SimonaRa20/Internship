using Blog.Models;

namespace Blog.Interfaces
{
    public interface IArticleRepository
    {
        Task<IQueryable<Article>> GetAll();
        Task<Article> GetById(int id);
        Task<Article> Add(Article article);
        Task Update(Article article);
        Task Delete(int id);
        Task Save();
        Task<bool> VoteArtical(int articleId, int userId, int voteValue);
        Task<int> GetTotalVotes(int articleId);
    }
}
