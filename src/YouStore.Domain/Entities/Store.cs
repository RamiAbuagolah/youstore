using System;
using YouStore.Domain.Common;
using YouStore.Domain.ValueObjects;

namespace YouStore.Domain.Entities;

public class Store : Entity<StoreId>, IAggregateRoot
{
    private Store() { }

    private Store(StoreId id, TenantId tenantId, MerchantId merchantId, string name, string slug, TemplateId templateId)
    {
        Id = id;
        TenantId = tenantId;
        MerchantId = merchantId;
        Name = name;
        Slug = slug;
        TemplateId = templateId;
        CreatedAt = DateTime.UtcNow;
    }

    public TenantId TenantId { get; private set; }
    public MerchantId MerchantId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public TemplateId TemplateId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsPublished { get; private set; }

    public static Store CreateTenantStore(TenantId tenantId, MerchantId merchantId, string name, string slug, TemplateId templateId)
    {
        return new Store(StoreId.New(), tenantId, merchantId, name, slug, templateId);
    }

    public void UpdateTemplate(TemplateId templateId)
    {
        TemplateId = templateId;
    }

    public void SetPublished(bool published)
    {
        IsPublished = published;
    }
}
