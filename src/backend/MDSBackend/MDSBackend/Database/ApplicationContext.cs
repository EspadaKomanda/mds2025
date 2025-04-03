using MDSBackend.Models.Database;
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
    public DbSet<Instruction> Instructions { get; set; }
    public DbSet<InstructionCategory> InstructionCategories { get; set; }
    public DbSet<InstructionParagraph> InstructionParagraphs { get; set; }
    public DbSet<InstructionTest> InstructionTests { get; set; }
    public DbSet<InstructionTestQuestion> InstructionTestQuestions { get; set; }
    public DbSet<InstructionTestResult> InstructionTestResults { get; set; }
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        
        modelBuilder.Entity<RoleRight>()
            .HasKey(rr => new { rr.RoleId, rr.RightId });

        modelBuilder.Entity<InstructionTestResult>()
            .HasOne(itr => itr.InstructionTest);

        modelBuilder.Entity<Instruction>()
            .HasOne(i => i.Category);

        modelBuilder.Entity<Instruction>()
            .HasMany(i => i.Paragraphs);
    }
}
