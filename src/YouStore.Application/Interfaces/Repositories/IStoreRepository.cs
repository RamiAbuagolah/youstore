using System.Threading.Tasks;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Interfaces.Repositories;

public interface IStoreRepository
{
    Task AddAsync(Store store);
    Task<bool> ExistsBySlugAsync(TenantId tenantId, string slug);
    Task<Store?> GetByIdAsync(StoreId storeId);
}
