using System;
using YouStore.Domain.Common;
using YouStore.Domain.ValueObjects;

namespace YouStore.Domain.Entities;

public class MerchantUser : Entity<MerchantId>, IAggregateRoot
{
    private MerchantUser() { }

    private MerchantUser(MerchantId id, TenantId tenantId, string email, string passwordHash, DateTime createdAt)
    {
        Id = id;
        TenantId = tenantId;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = createdAt;
    }

    public TenantId TenantId { get; private set; }
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    public static MerchantUser Create(TenantId tenantId, string email, string passwordHash)
    {
        return new MerchantUser(MerchantId.New(), tenantId, email, passwordHash, DateTime.UtcNow);
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
}
