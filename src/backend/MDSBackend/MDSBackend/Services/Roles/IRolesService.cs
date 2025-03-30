using MDSBackend.Models.Database;

namespace MDSBackend.Services.Roles;

public interface IRolesService
{
    Task<ApplicationRole> CreateRoleAsync(string roleName, string description);
    Task<bool> UpdateRoleAsync(long roleId, string newRoleName, string newDescription);
    Task<bool> DeleteRoleAsync(long roleId);
    Task<bool> AddRightToRoleAsync(long roleId, long rightId);
    Task<bool> RemoveRightFromRoleAsync(long roleId, long rightId);
    Task<ApplicationRole> GetRoleByIdAsync(long roleId);
    Task<(List<ApplicationRole> Roles, int TotalCount)> GetAllRolesAsync(int pageNumber = 1, int pageSize = 10);
}