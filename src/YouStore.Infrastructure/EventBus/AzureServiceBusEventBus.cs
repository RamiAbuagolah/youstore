using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YouStore.Application.Interfaces;
using YouStore.Domain.Events;

namespace YouStore.Infrastructure.EventBus;

internal sealed class AzureServiceBusEventBus : IEventBus
{
    private readonly ServiceBusClient _client;
    private readonly string _queueName;
    private readonly IServiceProvider _provider;
    private readonly ILogger<AzureServiceBusEventBus> _logger;

    public AzureServiceBusEventBus(IConfiguration configuration, IServiceProvider provider, ILogger<AzureServiceBusEventBus> logger)
    {
        var connection = configuration["ServiceBusConnectionString"]
            ?? throw new InvalidOperationException("ServiceBusConnectionString must be configured to use Azure service bus.");
        _queueName = configuration["ServiceBusQueueName"] ?? "youstore-events";
        _client = new ServiceBusClient(connection);
        _provider = provider;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent
    {
        var sender = _client.CreateSender(_queueName);
        var payload = JsonSerializer.Serialize(@event);
        var message = new ServiceBusMessage(payload)
        {
            Subject = typeof(TEvent).Name
        };

        message.ApplicationProperties["eventType"] = typeof(TEvent).AssemblyQualifiedName;
        message.ApplicationProperties["OccurredOn"] = @event.OccurredOn;

        await sender.SendMessageAsync(message, cancellationToken);
        _logger.LogInformation("Published {EventType} to Azure Service Bus", typeof(TEvent).Name);

        var handlers = _provider.GetServices<IIntegrationEventHandler<TEvent>>();
        if (handlers is not null)
        {
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event, cancellationToken);
            }
        }
    }
}
