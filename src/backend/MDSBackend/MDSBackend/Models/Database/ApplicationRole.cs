using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace MDSBackend.Models.Database;


public class ApplicationRole : IdentityRole<long>
{
    public ApplicationRole() : base() { }
    public ApplicationRole(string roleName) : base(roleName) { }
    
    public string? Description { get; set; }
    
    public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public List<RoleRight> RoleRights { get; set; } = new List<RoleRight>();
}
