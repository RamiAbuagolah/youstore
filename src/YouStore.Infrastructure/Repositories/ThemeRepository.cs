using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;
using YouStore.Infrastructure.Persistence;

namespace YouStore.Infrastructure.Repositories;

internal sealed class ThemeRepository : IThemeRepository
{
    private readonly YouStoreDbContext _context;

    public ThemeRepository(YouStoreDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(ThemeConfig themeConfig)
    {
        return _context.ThemeConfigs.AddAsync(themeConfig).AsTask();
    }

    public async Task<ThemeConfig?> GetByStoreIdAsync(StoreId storeId)
    {
        return await _context.ThemeConfigs
            .SingleOrDefaultAsync(t => t.StoreId == storeId);
    }
}
