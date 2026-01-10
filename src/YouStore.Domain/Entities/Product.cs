using System;
using System.Collections.Generic;
using YouStore.Domain.Common;
using YouStore.Domain.ValueObjects;

namespace YouStore.Domain.Entities;

public class Product : Entity<ProductId>
{
    private Product() { }

    private Product(ProductId id, TenantId tenantId, StoreId storeId, CategoryId categoryId, string name, string description, decimal price, string currency)
    {
        Id = id;
        TenantId = tenantId;
        StoreId = storeId;
        CategoryId = categoryId;
        Name = name;
        Description = description;
        Currency = currency;
        Price = price;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public TenantId TenantId { get; private set; }
    public StoreId StoreId { get; private set; }
    public CategoryId CategoryId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = null!;
    public decimal? DiscountPrice { get; private set; }
    public string? DiscountDescription { get; private set; }
    public bool IsPublished { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public static Product Create(TenantId tenantId, StoreId storeId, CategoryId categoryId, string name, string description, decimal price, string currency)
    {
        return new Product(ProductId.New(), tenantId, storeId, categoryId, name, description, price, currency);
    }

    public void UpdateDetails(string name, string description, decimal price, string currency)
    {
        Name = name;
        Description = description;
        Price = price;
        Currency = currency;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetDiscount(decimal discountPrice, string description)
    {
        DiscountPrice = discountPrice;
        DiscountDescription = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ClearDiscount()
    {
        DiscountPrice = null;
        DiscountDescription = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPublished(bool published)
    {
        IsPublished = published;
        UpdatedAt = DateTime.UtcNow;
    }
}
