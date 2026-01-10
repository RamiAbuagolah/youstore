using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;
using YouStore.Infrastructure.Persistence;

namespace YouStore.Infrastructure.Repositories;

internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly YouStoreDbContext _context;

    public CategoryRepository(YouStoreDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
    }

    public async Task DeleteAsync(Category category)
    {
        _context.Categories.Remove(category);
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsBySlugAsync(StoreId storeId, string slug)
    {
        return await _context.Categories.AnyAsync(c => c.StoreId == storeId && c.Slug == slug);
    }

    public async Task<Category?> GetByIdAsync(CategoryId categoryId)
    {
        return await _context.Categories.SingleOrDefaultAsync(c => c.Id == categoryId);
    }

    public async Task<IEnumerable<Category>> GetByStoreAsync(StoreId storeId)
    {
        return await _context.Categories
            .Where(c => c.StoreId == storeId)
            .AsNoTracking()
            .ToListAsync();
    }
}
