using Blog.Models;

namespace Blog.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int id);
        Task UpdateUser(User user);
        Task ChangePassword(int userId, string password);
        Task Save();
    }
}
