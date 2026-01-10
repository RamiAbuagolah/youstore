using System.Collections.Generic;
using System.Threading.Tasks;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<Category?> GetByIdAsync(CategoryId categoryId);
    Task<bool> ExistsBySlugAsync(StoreId storeId, string slug);
    Task<IEnumerable<Category>> GetByStoreAsync(StoreId storeId);
    Task DeleteAsync(Category category);
}
