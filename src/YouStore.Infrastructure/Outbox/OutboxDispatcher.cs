using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YouStore.Application.Interfaces;
using YouStore.Domain.Events;
using YouStore.Infrastructure.Persistence;

namespace YouStore.Infrastructure.Outbox;

internal sealed class OutboxDispatcher : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<OutboxDispatcher> _logger;

    public OutboxDispatcher(IServiceProvider provider, ILogger<OutboxDispatcher> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = _provider.CreateAsyncScope();
                var context = scope.ServiceProvider.GetRequiredService<YouStoreDbContext>();
                var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

                var pending = await context.OutboxMessages
                    .Where(o => !o.Processed)
                    .OrderBy(o => o.OccurredOn)
                    .ToListAsync(stoppingToken);

                foreach (var message in pending)
                {
                    var eventType = Type.GetType(message.EventType);
                    if (eventType is null)
                    {
                        _logger.LogWarning("Unknown outbox event type {EventType}", message.EventType);
                        continue;
                    }

                    var integrationEvent = JsonSerializer.Deserialize(message.Payload, eventType, options) as IIntegrationEvent;
                    if (integrationEvent is null)
                    {
                        _logger.LogWarning("Failed to deserialize outbox event {Id}", message.Id);
                        continue;
                    }

                    await eventBus.PublishAsync((dynamic)integrationEvent, stoppingToken);
                    message.Processed = true;
                    message.ProcessedAt = DateTime.UtcNow;
                }

                await context.SaveChangesAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish outbox events");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
