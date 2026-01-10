using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;
using YouStore.Infrastructure.Persistence;

namespace YouStore.Infrastructure.Repositories;

internal sealed class ProductImageRepository : IProductImageRepository
{
    private readonly YouStoreDbContext _context;

    public ProductImageRepository(YouStoreDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ProductImage image)
    {
        await _context.ProductImages.AddAsync(image);
    }

    public async Task<IEnumerable<ProductImage>> GetByProductAsync(ProductId productId)
    {
        return await _context.ProductImages
            .Where(pi => pi.ProductId == productId)
            .AsNoTracking()
            .ToListAsync();
    }
}
