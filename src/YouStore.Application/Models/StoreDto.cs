using System;

namespace YouStore.Application.Models;

public sealed record StoreDto(Guid Id, Guid TenantId, Guid MerchantId, string Name, string Slug, Guid TemplateId, DateTime CreatedAt);
