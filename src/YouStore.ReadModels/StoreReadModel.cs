using System;

namespace YouStore.ReadModels;

public sealed class StoreReadModel
{
    public Guid StoreId { get; set; }
    public Guid TenantId { get; set; }
    public Guid MerchantId { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public Guid TemplateId { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
}
