using System.Threading;
using System.Threading.Tasks;
using YouStore.Domain.Events;

namespace YouStore.Application.Interfaces;

public interface IOutboxService
{
    Task SaveEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent;
}
