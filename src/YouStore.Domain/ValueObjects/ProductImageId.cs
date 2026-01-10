using System;

namespace YouStore.Domain.ValueObjects;

public readonly record struct ProductImageId(Guid Value)
{
    public static ProductImageId New() => new(Guid.NewGuid());
    public static ProductImageId From(Guid value) => new(value);
}
