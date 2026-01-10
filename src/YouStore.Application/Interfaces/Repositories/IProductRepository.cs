using System.Collections.Generic;
using System.Threading.Tasks;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<Product?> GetByIdAsync(ProductId productId);
    Task<IEnumerable<Product>> GetByStoreAsync(StoreId storeId);
}
