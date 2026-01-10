using System;
using System.Collections.Generic;
using YouStore.Domain.Common;
using YouStore.Domain.ValueObjects;

namespace YouStore.Domain.Entities;

public class Category : Entity<CategoryId>
{
    private readonly List<ProductId> _productIds = new();
    private Category() { }

    private Category(CategoryId id, TenantId tenantId, StoreId storeId, string name, string slug, string? description)
    {
        Id = id;
        TenantId = tenantId;
        StoreId = storeId;
        Name = name;
        Slug = slug;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    public TenantId TenantId { get; private set; }
    public StoreId StoreId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyCollection<ProductId> ProductIds => _productIds.AsReadOnly();

    public static Category Create(TenantId tenantId, StoreId storeId, string name, string slug, string? description) =>
        new(CategoryId.New(), tenantId, storeId, name, slug, description);

    public void Update(string name, string slug, string? description)
    {
        Name = name;
        Slug = slug;
        Description = description;
    }

    public void AttachProduct(ProductId productId)
    {
        if (!_productIds.Contains(productId))
        {
            _productIds.Add(productId);
        }
    }
}
