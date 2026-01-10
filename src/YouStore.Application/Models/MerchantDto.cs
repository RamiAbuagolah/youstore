using System;

namespace YouStore.Application.Models;

public sealed record MerchantDto(Guid Id, Guid TenantId, string Email);
