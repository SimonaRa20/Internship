using Blog.Contracts.Comment;
using Blog.Models;

namespace Blog.Interfaces
{
    public interface ICommentRepository
    {
        Task AddComment(CommentRequest commentRequest);
        Task<List<CommentResponse>> GetCommentsArticle(int articleId);
        Task DeleteComment(int commentId);
        Task Update(Comment comment);
        Task<Comment> GetCommentById(int id);
        Task ReportComment(int commentId);
        Task BlockComment(int commentId);
        Task Save();
    }
}
