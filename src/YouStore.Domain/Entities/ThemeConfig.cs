using System;
using YouStore.Domain.Common;
using YouStore.Domain.ValueObjects;

namespace YouStore.Domain.Entities;

public class ThemeConfig : Entity<Guid>, IAggregateRoot
{
    private ThemeConfig() { }

    public ThemeConfig(StoreId storeId, TenantId tenantId, string primaryColor, string accentColor, string fontFamily, string background)
    {
        Id = Guid.NewGuid();
        StoreId = storeId;
        TenantId = tenantId;
        PrimaryColor = primaryColor;
        AccentColor = accentColor;
        FontFamily = fontFamily;
        Background = background;
        UpdatedAt = DateTime.UtcNow;
    }

    public StoreId StoreId { get; private set; }
    public TenantId TenantId { get; private set; }
    public string PrimaryColor { get; private set; } = null!;
    public string AccentColor { get; private set; } = null!;
    public string FontFamily { get; private set; } = null!;
    public string Background { get; private set; } = null!;
    public DateTime UpdatedAt { get; private set; }

    public void Update(string primaryColor, string accentColor, string fontFamily, string background)
    {
        PrimaryColor = primaryColor;
        AccentColor = accentColor;
        FontFamily = fontFamily;
        Background = background;
        UpdatedAt = DateTime.UtcNow;
    }
}
