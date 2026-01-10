using System;
using YouStore.Domain.Common;
using YouStore.Domain.ValueObjects;

namespace YouStore.Domain.Entities;

public class ProductImage : Entity<ProductImageId>
{
    private ProductImage() { }

    private ProductImage(ProductImageId id, TenantId tenantId, StoreId storeId, ProductId productId, string url, bool isPrimary)
    {
        Id = id;
        TenantId = tenantId;
        StoreId = storeId;
        ProductId = productId;
        Url = url;
        IsPrimary = isPrimary;
        UploadedAt = DateTime.UtcNow;
    }

    public TenantId TenantId { get; private set; }
    public StoreId StoreId { get; private set; }
    public ProductId ProductId { get; private set; }
    public string Url { get; private set; } = null!;
    public bool IsPrimary { get; private set; }
    public DateTime UploadedAt { get; private set; }

    public static ProductImage Create(TenantId tenantId, StoreId storeId, ProductId productId, string url, bool isPrimary)
        => new(ProductImageId.New(), tenantId, storeId, productId, url, isPrimary);
}
