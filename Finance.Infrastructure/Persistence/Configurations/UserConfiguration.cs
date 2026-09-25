using Finance.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance.Infrastructure.Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .Property(x => x.FirstName)
            .HasMaxLength(256)
            .IsRequired();

        builder 
            .Property(x => x.LastName)
            .HasMaxLength(256)
            .IsRequired();

        builder
            .Property(x => x.DateOfBirth)
            .IsRequired();
    }
}
