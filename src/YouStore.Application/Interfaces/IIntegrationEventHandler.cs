using System.Threading;
using System.Threading.Tasks;
using YouStore.Domain.Events;

namespace YouStore.Application.Interfaces;

public interface IIntegrationEventHandler<in TEvent>
    where TEvent : IIntegrationEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}
