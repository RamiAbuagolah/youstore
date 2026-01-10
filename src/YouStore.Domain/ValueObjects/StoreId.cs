using System;

namespace YouStore.Domain.ValueObjects;

public readonly record struct StoreId(Guid Value)
{
    public static StoreId New() => new(Guid.NewGuid());
    public static StoreId From(Guid value) => new(value);
}
