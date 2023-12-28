using Blog.Contracts.Permissions;
using Blog.Models;

namespace Blog.Interfaces
{
    public interface IPermissionsRepository
    {
        Task<List<PermisionResponse>> GetUsersPermissions();
        Task AddPermission(Permisions permisions);
        Task<PermisionResponse> GetPermissionsByUserId(int userId);
        Task UpdatePermissions(int userId, PermisionRequest updatedPermissions);
        Task Save();
    }
}
