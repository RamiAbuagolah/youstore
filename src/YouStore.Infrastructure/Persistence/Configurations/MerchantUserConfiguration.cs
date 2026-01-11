using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Infrastructure.Persistence.Configurations;

internal sealed class MerchantUserConfiguration : IEntityTypeConfiguration<MerchantUser>
{
    public void Configure(EntityTypeBuilder<MerchantUser> builder)
    {
        builder.ToTable("merchant_users");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasConversion(id => id.Value, value => MerchantId.From(value))
            .ValueGeneratedNever();
        builder.Property(m => m.TenantId)
            .HasConversion(t => t.Value, value => TenantId.From(value))
            .IsRequired();
        builder.Property(m => m.Email)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(m => m.PasswordHash)
            .HasMaxLength(512)
            .IsRequired();
        builder.Property(m => m.CreatedAt)
            .IsRequired();
        builder.HasIndex(m => m.Email).IsUnique();
    }
}
