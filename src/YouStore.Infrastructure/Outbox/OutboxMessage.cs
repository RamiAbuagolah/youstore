using System;

namespace YouStore.Infrastructure.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public DateTime OccurredOn { get; set; }
    public bool Processed { get; set; }
    public DateTime? ProcessedAt { get; set; }
}
