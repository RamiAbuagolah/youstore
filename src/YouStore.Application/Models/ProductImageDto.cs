using System;

namespace YouStore.Application.Models;

public sealed record ProductImageDto(Guid Id, Guid TenantId, Guid StoreId, Guid ProductId, string Url, bool IsPrimary, DateTime UploadedAt);
