using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;
using YouStore.Infrastructure.Seed;

namespace YouStore.Infrastructure.Persistence.Configurations;

internal sealed class TemplateConfiguration : IEntityTypeConfiguration<Template>
{
    public void Configure(EntityTypeBuilder<Template> builder)
    {
        builder.ToTable("templates");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasConversion(id => id.Value, value => TemplateId.From(value))
            .ValueGeneratedNever();
        builder.Property(t => t.Name)
            .HasMaxLength(128)
            .IsRequired();
        builder.Property(t => t.Description)
            .HasMaxLength(512)
            .IsRequired();
        builder.Property(t => t.PreviewImageUrl)
            .HasMaxLength(1024)
            .IsRequired();

        builder.HasData(TemplateSeed.AllTemplates);
    }
}
