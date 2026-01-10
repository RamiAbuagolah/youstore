using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;
using YouStore.Infrastructure.Persistence;

namespace YouStore.Infrastructure.Repositories;

internal sealed class StoreRepository : IStoreRepository
{
    private readonly YouStoreDbContext _context;

    public StoreRepository(YouStoreDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Store store)
    {
        await _context.Stores.AddAsync(store);
    }

    public Task<bool> ExistsBySlugAsync(TenantId tenantId, string slug)
    {
        return _context.Stores
            .AnyAsync(s => s.TenantId == tenantId && s.Slug == slug);
    }

    public Task<Store?> GetByIdAsync(StoreId storeId)
    {
        return _context.Stores
            .SingleOrDefaultAsync(s => s.Id == storeId);
    }
}
