using EFCore9Bug.Encryption;
using Microsoft.EntityFrameworkCore;

namespace EFCore9Bug.EntityFramework;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    private readonly IEncryptor _encryptor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IEncryptor encryptor)
        : base(options) =>
        _encryptor = encryptor;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=app.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.UserId);
        modelBuilder.Entity<Role>().HasKey(r => r.RoleId);
        modelBuilder.Entity<UserRole>().HasKey(ur => ur.UserRoleId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        // Seed Data
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, RoleName = "Admin" },
            new Role { RoleId = 2, RoleName = "User" }
        );

        modelBuilder.Entity<User>().HasData(
            new User { UserId = 1, Username = "alice", Encrypted = "some value" }, 
            new User { UserId = 2, Username = "bob", Encrypted = "another value" }
        );

        modelBuilder.Entity<UserRole>().HasData(
            new UserRole { UserRoleId = 1, UserId = 1, RoleId = 1 }, 
            new UserRole { UserRoleId = 2, UserId = 1, RoleId = 2 }, 
            new UserRole { UserRoleId = 3, UserId = 2, RoleId = 2 }
        );
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Conventions.Add(_ => new EncryptedConvention(_encryptor));
    }
}