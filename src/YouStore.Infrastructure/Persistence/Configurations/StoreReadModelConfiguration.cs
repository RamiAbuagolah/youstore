using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouStore.ReadModels;

namespace YouStore.Infrastructure.Persistence.Configurations;

internal sealed class StoreReadModelConfiguration : IEntityTypeConfiguration<StoreReadModel>
{
    public void Configure(EntityTypeBuilder<StoreReadModel> builder)
    {
        builder.ToTable("store_read_models");
        builder.HasKey(s => s.StoreId);
        builder.Property(s => s.Name).HasMaxLength(256).IsRequired();
        builder.Property(s => s.Slug).HasMaxLength(128).IsRequired();
        builder.Property(s => s.TemplateId).IsRequired();
        builder.Property(s => s.MerchantId).IsRequired();
        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.IsPublished).IsRequired();
    }
}
