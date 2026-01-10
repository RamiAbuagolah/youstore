using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouStore.Application.Interfaces.Repositories;
using YouStore.ReadModels;
using YouStore.Infrastructure.Persistence;

namespace YouStore.Infrastructure.Repositories.ReadModel;

internal sealed class StoreReadModelRepository : IStoreReadModelRepository
{
    private readonly YouStoreDbContext _context;

    public StoreReadModelRepository(YouStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StoreReadModel>> ListAsync(string? search, CancellationToken cancellationToken = default)
    {
        var query = _context.StoreReadModels.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => x.Name.Contains(search) || x.Slug.Contains(search));
        }

        return await query.OrderBy(x => x.Name).ToListAsync(cancellationToken);
    }

    public async Task<StoreReadModel?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.StoreReadModels.AsNoTracking().SingleOrDefaultAsync(x => x.Slug == slug, cancellationToken);
    }
}
