using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Infrastructure.Persistence.Configurations;

internal sealed class ThemeConfigConfiguration : IEntityTypeConfiguration<ThemeConfig>
{
    public void Configure(EntityTypeBuilder<ThemeConfig> builder)
    {
        builder.ToTable("theme_configs");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.StoreId)
            .HasConversion(id => id.Value, value => StoreId.From(value))
            .IsRequired();
        builder.Property(t => t.TenantId)
            .HasConversion(id => id.Value, value => TenantId.From(value))
            .IsRequired();
        builder.Property(t => t.PrimaryColor).HasMaxLength(64).IsRequired();
        builder.Property(t => t.AccentColor).HasMaxLength(64).IsRequired();
        builder.Property(t => t.FontFamily).HasMaxLength(128).IsRequired();
        builder.Property(t => t.Background).HasMaxLength(256).IsRequired();
        builder.Property(t => t.UpdatedAt).IsRequired();
        builder.HasIndex(t => t.StoreId).IsUnique();
    }
}
