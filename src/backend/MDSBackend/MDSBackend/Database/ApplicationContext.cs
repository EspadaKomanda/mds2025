using MDSBackend.Models.Database;
using MDSBackend.Services.CurrentUserServiceNamespace;
using MDSBackend.Database.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MDSBackend.Database;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    
    private readonly ICurrentUserService _currentUserService;

    public ApplicationContext(DbContextOptions<ApplicationContext> options, ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
        ChangeTracker.SetAuditProperties(_currentUserService);
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role {Id = 1, Name = "User"},
            new Role {Id = 2, Name = "Admin"}
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

}
