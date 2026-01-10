using System;

namespace YouStore.Domain.ValueObjects;

public readonly record struct MerchantId(Guid Value)
{
    public static MerchantId New() => new(Guid.NewGuid());
    public static MerchantId From(Guid value) => new(value);
}
