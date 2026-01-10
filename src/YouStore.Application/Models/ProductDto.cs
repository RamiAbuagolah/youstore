using System;

namespace YouStore.Application.Models;

public sealed record ProductDto(
    Guid Id,
    Guid TenantId,
    Guid StoreId,
    Guid CategoryId,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    decimal? DiscountPrice,
    string? DiscountDescription,
    bool IsPublished,
    DateTime CreatedAt,
    DateTime UpdatedAt);
