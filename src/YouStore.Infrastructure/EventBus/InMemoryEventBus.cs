using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YouStore.Application.Interfaces;
using YouStore.Domain.Events;

namespace YouStore.Infrastructure.EventBus;

internal sealed class InMemoryEventBus : IEventBus
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<InMemoryEventBus> _logger;

    public InMemoryEventBus(IServiceProvider provider, ILogger<InMemoryEventBus> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent
    {
        var handlers = _provider.GetServices<IIntegrationEventHandler<TEvent>>();
        if (handlers is null)
        {
            _logger.LogInformation("No handlers registered for {EventType}", typeof(TEvent).Name);
            return;
        }

        foreach (var handler in handlers)
        {
            await handler.HandleAsync(@event, cancellationToken);
        }
    }
}
