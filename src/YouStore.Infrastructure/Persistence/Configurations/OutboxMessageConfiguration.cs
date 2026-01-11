using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouStore.Infrastructure.Outbox;

namespace YouStore.Infrastructure.Persistence.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.EventType)
            .HasMaxLength(512)
            .IsRequired();
        builder.Property(o => o.Payload).IsRequired();
        builder.Property(o => o.OccurredOn).IsRequired();
        builder.Property(o => o.Processed).HasDefaultValue(false);
        builder.Property(o => o.ProcessedAt);
        builder.HasIndex(o => o.Processed);
    }
}
