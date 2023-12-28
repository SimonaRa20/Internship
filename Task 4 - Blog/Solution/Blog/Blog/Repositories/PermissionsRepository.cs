using Blog.Constants;
using Blog.Contracts.Permissions;
using Blog.Database;
using Blog.Interfaces;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories
{
    public class PermissionsRepository : IPermissionsRepository
    {
        private readonly BlogDbContext _dbContext;

        public PermissionsRepository(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<PermisionResponse> GetPermissionsByUserId(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user == null
                ? null
                : new PermisionResponse
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    PermissionId = user.Permissions.Id,
                    WriteArticals = user.Permissions.WriteArticals,
                    RateArticals = user.Permissions.RateArticals,
                    WriteComments = user.Permissions.WriteComments
                };
        }

        public async Task<List<PermisionResponse>> GetUsersPermissions()
        {
            return await _dbContext.Users
                .Where(x => x.Role == Role.User)
                .Select(user => new PermisionResponse
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    PermissionId = user.Permissions.Id,
                    WriteArticals = user.Permissions.WriteArticals,
                    RateArticals = user.Permissions.RateArticals,
                    WriteComments = user.Permissions.WriteComments
                })
                .ToListAsync();
        }

        public async Task UpdatePermissions(int userId, PermisionRequest updatedPermissions)
        {
            var user = await _dbContext.Users
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Id == userId && u.Role == Role.User);

            if (user != null)
            {
                // Update permissions
                user.Permissions.WriteArticals = updatedPermissions.WriteArticals;
                user.Permissions.RateArticals = updatedPermissions.RateArticals;
                user.Permissions.WriteComments = updatedPermissions.WriteComments;
            }
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddPermission(Permisions permisions)
        {
            await _dbContext.AddAsync(permisions);
        }
    }
}
