using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using YouStore.Application.Interfaces;
using YouStore.Domain.Entities;

namespace YouStore.Api.Services;

internal sealed class JwtAuthTokenGenerator : IAuthTokenGenerator
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _securityKey;

    public JwtAuthTokenGenerator(IConfiguration configuration)
    {
        _issuer = configuration["Jwt:Issuer"] ?? "YouStore.Api";
        _audience = configuration["Jwt:Audience"] ?? "YouStore";
        _securityKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key must be configured.");
    }

    public string GenerateToken(MerchantUser merchant)
    {
        var now = DateTime.UtcNow;
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, merchant.Id.Value.ToString()),
            new Claim("tenantId", merchant.TenantId.Value.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, merchant.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: now,
            expires: now.AddHours(4),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
