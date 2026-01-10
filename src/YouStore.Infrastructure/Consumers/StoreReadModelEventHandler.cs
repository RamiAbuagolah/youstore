using System.Threading;
using System.Threading.Tasks;
using YouStore.Application.Interfaces;
using YouStore.Domain.Events;
using YouStore.Infrastructure.Persistence;
using YouStore.ReadModels;

namespace YouStore.Infrastructure.Consumers;

internal sealed class StoreReadModelEventHandler : IIntegrationEventHandler<StoreCreatedEvent>
{
    private readonly YouStoreDbContext _context;

    public StoreReadModelEventHandler(YouStoreDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(StoreCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        var existing = await _context.StoreReadModels.FindAsync(new object[] { @event.StoreId }, cancellationToken);
        if (existing is null)
        {
            existing = new StoreReadModel()
            {
                StoreId = @event.StoreId,
                TenantId = @event.TenantId,
                MerchantId = @event.MerchantId,
                Name = @event.Name,
                Slug = @event.Slug,
                TemplateId = @event.TemplateId,
                CreatedAt = @event.CreatedAt,
                IsPublished = false
            };
            await _context.StoreReadModels.AddAsync(existing, cancellationToken);
        }
        else
        {
            existing.Name = @event.Name;
            existing.Slug = @event.Slug;
            existing.TemplateId = @event.TemplateId;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
