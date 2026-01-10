using System;

namespace YouStore.Domain.Events;

public sealed record StoreCreatedEvent(Guid StoreId, Guid TenantId, Guid MerchantId, string Name, string Slug, Guid TemplateId, DateTime CreatedAt) : IIntegrationEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public sealed record CategoryCreatedEvent(Guid CategoryId, Guid StoreId, Guid TenantId, string Name, string Slug, DateTime CreatedAt) : IIntegrationEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public sealed record CategoryUpdatedEvent(Guid CategoryId, Guid StoreId, Guid TenantId, string Name, string Slug, DateTime UpdatedAt) : IIntegrationEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public sealed record ProductCreatedEvent(Guid ProductId, Guid StoreId, Guid TenantId, Guid CategoryId, string Name, string Description, decimal Price, string Currency, bool IsPublished, decimal? DiscountPrice, string? DiscountDescription, DateTime CreatedAt) : IIntegrationEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public sealed record ProductUpdatedEvent(Guid ProductId, Guid StoreId, Guid TenantId, Guid CategoryId, string Name, string Description, decimal Price, string Currency, bool IsPublished, decimal? DiscountPrice, string? DiscountDescription, DateTime CreatedAt, DateTime UpdatedAt) : IIntegrationEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public sealed record ProductPublishedEvent(Guid ProductId, Guid StoreId, Guid TenantId, bool IsPublished, DateTime UpdatedAt) : IIntegrationEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public sealed record ProductDiscountChangedEvent(Guid ProductId, Guid StoreId, Guid TenantId, decimal? DiscountPrice, string? DiscountDescription, DateTime UpdatedAt) : IIntegrationEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
