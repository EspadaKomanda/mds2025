using MDSBackend.Models.Database;

namespace MDSBackend.Services.Roles;

public interface IRolesService
{
    Task<ApplicationRole> CreateRoleAsync(string roleName, string description);
    Task UpdateRoleAsync(long roleId, string newRoleName, string newDescription);
    Task DeleteRoleAsync(long roleId);
    Task AddRightToRoleAsync(long roleId, long rightId);
    Task RemoveRightFromRoleAsync(long roleId, long rightId);
    Task<ApplicationRole> GetRoleByIdAsync(long roleId);
    Task<(List<ApplicationRole> Roles, int TotalCount)> GetAllRolesAsync(int pageNumber = 1, int pageSize = 10);
}