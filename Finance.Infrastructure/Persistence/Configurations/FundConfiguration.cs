using Finance.Domain.Entities;
using Finance.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance.Infrastructure.Persistence.Configurations;

internal class FundConfiguration : IEntityTypeConfiguration<Fund>
{
    public void Configure(EntityTypeBuilder<Fund> builder)
    {
        builder
            .HasKey(f => f.Id);

        builder
            .Property(f => f.Date)
            .IsRequired();

        builder
            .Property(f => f.Description)
            .HasMaxLength(500);

        builder
            .Property(f => f.CategoryId)
            .IsRequired();

        builder
            .Property(f => f.UserId)
            .IsRequired();

        builder
            .Property(f => f.ExchangeRateInUsd)
            .IsRequired();

        builder
            .Property(f => f.FundType)
            .HasConversion<string>()
            .HasMaxLength(7)
            .IsRequired();

        builder
            .OwnsOne(f => f.Money, builder =>
            {
                builder
                    .Property(f => f.Amount)
                    .HasColumnName("Amount")
                    .HasPrecision(18, 2)
                    .IsRequired();

                builder
                    .Property(f => f.CurrencyId)
                    .HasColumnName("CurrencyId")
                    .IsRequired();

                builder
                    .HasOne<Currency>()
                    .WithMany()
                    .HasForeignKey(f => f.CurrencyId)
                    .HasPrincipalKey(c => c.Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });

        builder
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(f => f.CategoryId)
            .HasPrincipalKey(c => c.Id)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .HasPrincipalKey(u => u.Id)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasIndex(f => f.UserId);

        builder
            .HasIndex(f => f.CategoryId);

        builder
            .HasIndex(f => f.FundType);


        builder
            .ToTable(t =>
            {
                t.HasCheckConstraint("CK_Funds_FundType", "FundType = 'Income' OR FundType = 'Expense'");
                t.HasCheckConstraint("CK_Funds_Amount", "Amount > 0");
            });
    }
}
