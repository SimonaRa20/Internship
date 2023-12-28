using Blog.Models;

namespace Blog.Interfaces
{
    public interface IAuthRepository
    {
        Task<User>GetUserByEmail(string email);
        Task<User> GetUserById(int id);
        Task AddUser(User user);
        public Task Save();
    }
}
