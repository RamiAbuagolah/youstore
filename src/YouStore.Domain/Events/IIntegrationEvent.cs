using System;

namespace YouStore.Domain.Events;

public interface IIntegrationEvent
{
    DateTime OccurredOn { get; }
}
