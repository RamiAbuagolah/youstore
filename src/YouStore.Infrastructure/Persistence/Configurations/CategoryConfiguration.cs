using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Infrastructure.Persistence.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, value => CategoryId.From(value))
            .ValueGeneratedNever();
        builder.Property(c => c.TenantId)
            .HasConversion(id => id.Value, value => TenantId.From(value))
            .IsRequired();
        builder.Property(c => c.StoreId)
            .HasConversion(id => id.Value, value => StoreId.From(value))
            .IsRequired();
        builder.Property(c => c.Name)
            .HasMaxLength(128)
            .IsRequired();
        builder.Property(c => c.Slug)
            .HasMaxLength(128)
            .IsRequired();
        builder.Property(c => c.Description)
            .HasMaxLength(512);
        builder.Property(c => c.CreatedAt).IsRequired();
    }
}
