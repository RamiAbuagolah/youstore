using System;
using System.Threading;
using System.Threading.Tasks;
using YouStore.Application.Interfaces;
using YouStore.Domain.Events;
using YouStore.Infrastructure.Persistence;
using YouStore.ReadModels;

namespace YouStore.Infrastructure.Consumers;

internal sealed class ProductReadModelEventHandler :
    IIntegrationEventHandler<ProductCreatedEvent>,
    IIntegrationEventHandler<ProductUpdatedEvent>,
    IIntegrationEventHandler<ProductPublishedEvent>,
    IIntegrationEventHandler<ProductDiscountChangedEvent>
{
    private readonly YouStoreDbContext _context;

    public ProductReadModelEventHandler(YouStoreDbContext context)
    {
        _context = context;
    }

    public Task HandleAsync(ProductCreatedEvent @event, CancellationToken cancellationToken = default)
        => UpsertAsync(@event.ProductId, @event.StoreId, @event.TenantId, @event.CategoryId, @event.Name, @event.Description, @event.Price, @event.Currency, @event.IsPublished, @event.DiscountPrice, @event.DiscountDescription, @event.CreatedAt, @event.CreatedAt, cancellationToken);

    public Task HandleAsync(ProductUpdatedEvent @event, CancellationToken cancellationToken = default)
        => UpsertAsync(@event.ProductId, @event.StoreId, @event.TenantId, @event.CategoryId, @event.Name, @event.Description, @event.Price, @event.Currency, @event.IsPublished, @event.DiscountPrice, @event.DiscountDescription, @event.CreatedAt, @event.UpdatedAt, cancellationToken);

    public Task HandleAsync(ProductPublishedEvent @event, CancellationToken cancellationToken = default)
        => UpdatePublishedAsync(@event.ProductId, @event.IsPublished, @event.UpdatedAt, cancellationToken);

    public Task HandleAsync(ProductDiscountChangedEvent @event, CancellationToken cancellationToken = default)
        => UpdateDiscountAsync(@event.ProductId, @event.DiscountPrice, @event.DiscountDescription, @event.UpdatedAt, cancellationToken);

    private async Task UpsertAsync(Guid productId, Guid storeId, Guid tenantId, Guid categoryId, string name, string description, decimal price, string currency, bool isPublished, decimal? discountPrice, string? discountDescription, DateTime createdAt, DateTime updatedAt, CancellationToken cancellationToken)
    {
        var existing = await _context.ProductReadModels.FindAsync(new object[] { productId }, cancellationToken);
        if (existing is null)
        {
            existing = new ProductReadModel
            {
                ProductId = productId,
                StoreId = storeId,
                TenantId = tenantId,
                CategoryId = categoryId,
                Name = name,
                Description = description,
                Price = price,
                Currency = currency,
                DiscountPrice = discountPrice,
                DiscountDescription = discountDescription,
                IsPublished = isPublished,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt
            };
            await _context.ProductReadModels.AddAsync(existing, cancellationToken);
        }
        else
        {
            existing.Name = name;
            existing.Description = description;
            existing.Price = price;
            existing.Currency = currency;
            existing.CategoryId = categoryId;
            existing.TenantId = tenantId;
            existing.StoreId = storeId;
            existing.DiscountPrice = discountPrice;
            existing.DiscountDescription = discountDescription;
            existing.IsPublished = isPublished;
            existing.UpdatedAt = updatedAt;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task UpdatePublishedAsync(Guid productId, bool isPublished, DateTime updatedAt, CancellationToken cancellationToken)
    {
        var existing = await _context.ProductReadModels.FindAsync(new object[] { productId }, cancellationToken);
        if (existing is null)
        {
            return;
        }

        existing.IsPublished = isPublished;
        existing.UpdatedAt = updatedAt;
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task UpdateDiscountAsync(Guid productId, decimal? discountPrice, string? discountDescription, DateTime updatedAt, CancellationToken cancellationToken)
    {
        var existing = await _context.ProductReadModels.FindAsync(new object[] { productId }, cancellationToken);
        if (existing is null)
        {
            return;
        }

        existing.DiscountPrice = discountPrice;
        existing.DiscountDescription = discountDescription;
        existing.UpdatedAt = updatedAt;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
