using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouStore.Application.Interfaces.Repositories;
using YouStore.ReadModels;
using YouStore.Infrastructure.Persistence;

namespace YouStore.Infrastructure.Repositories.ReadModel;

internal sealed class ProductReadModelRepository : IProductReadModelRepository
{
    private readonly YouStoreDbContext _context;

    public ProductReadModelRepository(YouStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductReadModel>> SearchAsync(string? search, CancellationToken cancellationToken = default)
    {
        var query = _context.ProductReadModels.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => x.Name.Contains(search) || x.Description.Contains(search));
        }

        return await query.OrderByDescending(x => x.UpdatedAt).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductReadModel>> ListByStoreAsync(string slug, CancellationToken cancellationToken = default)
    {
        var store = await _context.StoreReadModels.AsNoTracking().SingleOrDefaultAsync(x => x.Slug == slug, cancellationToken);
        if (store is null)
        {
            return Enumerable.Empty<ProductReadModel>();
        }

        return await _context.ProductReadModels
            .AsNoTracking()
            .Where(x => x.StoreId == store.StoreId)
            .OrderByDescending(x => x.UpdatedAt)
            .ToListAsync(cancellationToken);
    }
}
