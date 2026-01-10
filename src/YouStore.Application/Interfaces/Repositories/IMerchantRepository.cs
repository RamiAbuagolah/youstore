using System.Threading.Tasks;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Interfaces.Repositories;

public interface IMerchantRepository
{
    Task AddAsync(MerchantUser merchant);
    Task<MerchantUser?> GetByEmailAsync(string email);
    Task<MerchantUser?> GetByIdAsync(MerchantId id);
}
