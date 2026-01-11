using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Infrastructure.Persistence.Configurations;

internal sealed class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("product_images");
        builder.HasKey(pi => pi.Id);
        builder.Property(pi => pi.Id)
            .HasConversion(id => id.Value, value => ProductImageId.From(value))
            .ValueGeneratedNever();
        builder.Property(pi => pi.TenantId)
            .HasConversion(id => id.Value, value => TenantId.From(value))
            .IsRequired();
        builder.Property(pi => pi.StoreId)
            .HasConversion(id => id.Value, value => StoreId.From(value))
            .IsRequired();
        builder.Property(pi => pi.ProductId)
            .HasConversion(id => id.Value, value => ProductId.From(value))
            .IsRequired();
        builder.Property(pi => pi.Url)
            .HasMaxLength(2048)
            .IsRequired();
        builder.Property(pi => pi.IsPrimary).IsRequired();
        builder.Property(pi => pi.UploadedAt).IsRequired();
    }
}
