using System.Threading;
using System.Threading.Tasks;
using YouStore.Domain.Events;

namespace YouStore.Application.Interfaces;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent;
}
