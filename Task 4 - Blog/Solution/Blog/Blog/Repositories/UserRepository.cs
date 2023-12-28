using Blog.Database;
using Blog.Interfaces;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BlogDbContext _dbContext;
        public UserRepository(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task ChangePassword(int userId, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if(user != null)
            {
                user.Password = password;
            }
        }

        public async Task<User> GetUserById(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateUser(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
