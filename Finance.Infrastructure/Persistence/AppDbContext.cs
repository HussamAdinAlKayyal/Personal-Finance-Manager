using Finance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Finance.Infrastructure.Identity;
using Finance.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;

namespace Finance.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Fund> Funds => Set<Fund>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FundConfiguration).Assembly);
        
        modelBuilder.Entity<ApplicationUser>()
            .ToTable("Users");
        modelBuilder.Entity<IdentityRole>()
            .ToTable("Roles");
        modelBuilder.Entity<IdentityUserToken<string>>()
            .ToTable("UserTokens");
        modelBuilder.Entity<IdentityUserClaim<string>>()
            .ToTable("UserClaims");
        modelBuilder.Entity<IdentityRoleClaim<string>>()
            .ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>()
            .ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserRole<string>>()
            .ToTable("UserRoles");
    }
}
