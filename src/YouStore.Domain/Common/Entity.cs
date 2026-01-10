using System;

namespace YouStore.Domain.Common;

public abstract class Entity<TId>
{
    public TId Id { get; protected set; } = default!;
}
