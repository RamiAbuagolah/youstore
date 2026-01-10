using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;
using YouStore.Infrastructure.Persistence;

namespace YouStore.Infrastructure.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly YouStoreDbContext _context;

    public ProductRepository(YouStoreDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public async Task<Product?> GetByIdAsync(ProductId productId)
    {
        return await _context.Products.SingleOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<IEnumerable<Product>> GetByStoreAsync(StoreId storeId)
    {
        return await _context.Products
            .Where(p => p.StoreId == storeId)
            .AsNoTracking()
            .ToListAsync();
    }
}
