using MDSBackend.Models.Database;
using MDSBackend.Services.CurrentUsers;
using MDSBackend.Database.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MDSBackend.Database;


public class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
    
    public DbSet<Right> Rights { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ApplicationRole> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RoleRight> RoleRights { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        
        modelBuilder.Entity<RoleRight>()
            .HasKey(rr => new { rr.RoleId, rr.RightId });
    }
}
