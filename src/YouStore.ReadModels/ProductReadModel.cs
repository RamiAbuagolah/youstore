using System;

namespace YouStore.ReadModels;

public sealed class ProductReadModel
{
    public Guid ProductId { get; set; }
    public Guid StoreId { get; set; }
    public Guid TenantId { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string Currency { get; set; } = null!;
    public decimal? DiscountPrice { get; set; }
    public string? DiscountDescription { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
