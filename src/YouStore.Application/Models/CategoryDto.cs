using System;

namespace YouStore.Application.Models;

public sealed record CategoryDto(Guid Id, Guid TenantId, Guid StoreId, string Name, string Slug, string? Description, DateTime CreatedAt);
