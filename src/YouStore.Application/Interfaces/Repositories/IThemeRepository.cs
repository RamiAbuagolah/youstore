using System.Threading.Tasks;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Interfaces.Repositories;

public interface IThemeRepository
{
    Task<ThemeConfig?> GetByStoreIdAsync(StoreId storeId);
    Task AddAsync(ThemeConfig themeConfig);
}
