using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Infrastructure.Persistence.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, value => ProductId.From(value))
            .ValueGeneratedNever();
        builder.Property(p => p.TenantId)
            .HasConversion(id => id.Value, value => TenantId.From(value))
            .IsRequired();
        builder.Property(p => p.StoreId)
            .HasConversion(id => id.Value, value => StoreId.From(value))
            .IsRequired();
        builder.Property(p => p.CategoryId)
            .HasConversion(id => id.Value, value => CategoryId.From(value))
            .IsRequired();
        builder.Property(p => p.Name)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(p => p.Description)
            .HasMaxLength(1024)
            .IsRequired();
        builder.Property(p => p.Price).HasPrecision(18, 2);
        builder.Property(p => p.Currency)
            .HasMaxLength(8)
            .IsRequired();
        builder.Property(p => p.DiscountPrice).HasPrecision(18, 2);
        builder.Property(p => p.DiscountDescription)
            .HasMaxLength(512);
        builder.Property(p => p.IsPublished).HasDefaultValue(false);
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired();
    }
}
