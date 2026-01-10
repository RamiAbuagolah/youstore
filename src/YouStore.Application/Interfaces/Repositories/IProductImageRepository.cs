using System.Collections.Generic;
using System.Threading.Tasks;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Interfaces.Repositories;

public interface IProductImageRepository
{
    Task AddAsync(ProductImage image);
    Task<IEnumerable<ProductImage>> GetByProductAsync(ProductId productId);
}
