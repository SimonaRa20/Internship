using Blog.Database;
using Blog.Interfaces;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly BlogDbContext _dbContext;

        public AuthRepository(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddUser(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
