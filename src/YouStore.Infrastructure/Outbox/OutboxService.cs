using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using YouStore.Application.Interfaces;
using YouStore.Domain.Events;
using YouStore.Infrastructure.Persistence;

namespace YouStore.Infrastructure.Outbox;

internal sealed class OutboxService : IOutboxService
{
    private readonly YouStoreDbContext _context;
    private readonly JsonSerializerOptions _options;

    public OutboxService(YouStoreDbContext context)
    {
        _context = context;
        _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task SaveEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent
    {
        var message = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            EventType = @event.GetType().AssemblyQualifiedName!,
            Payload = JsonSerializer.Serialize(@event, _options),
            OccurredOn = @event.OccurredOn
        };

        await _context.Set<OutboxMessage>().AddAsync(message, cancellationToken);
    }
}
