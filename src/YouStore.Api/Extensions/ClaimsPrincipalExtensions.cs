using System;
using System.Security.Claims;

namespace YouStore.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetGuidClaim(this ClaimsPrincipal principal, string claimType)
    {
        var raw = principal.FindFirst(claimType)?.Value;
        return Guid.TryParse(raw, out var value)
            ? value
            : throw new UnauthorizedAccessException("Claim is missing or invalid.");
    }
}
