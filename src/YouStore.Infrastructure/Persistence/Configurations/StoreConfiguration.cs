using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Infrastructure.Persistence.Configurations;

internal sealed class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.ToTable("stores");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasConversion(id => id.Value, value => StoreId.From(value))
            .ValueGeneratedNever();
        builder.Property(s => s.TenantId)
            .HasConversion(t => t.Value, value => TenantId.From(value))
            .IsRequired();
        builder.Property(s => s.MerchantId)
            .HasConversion(m => m.Value, value => MerchantId.From(value))
            .IsRequired();
        builder.Property(s => s.TemplateId)
            .HasConversion(t => t.Value, value => TemplateId.From(value))
            .IsRequired();
        builder.Property(s => s.Name)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(s => s.Slug)
            .HasMaxLength(128)
            .IsRequired();
        builder.Property(s => s.CreatedAt)
            .IsRequired();
        builder.Property(s => s.IsPublished)
            .HasDefaultValue(false);
        builder.HasIndex(s => new { s.TenantId, s.Slug }).IsUnique();
    }
}
