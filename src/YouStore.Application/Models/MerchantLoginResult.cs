using System;

namespace YouStore.Application.Models;

public sealed record MerchantLoginResult(string Token, Guid MerchantId, Guid TenantId);
