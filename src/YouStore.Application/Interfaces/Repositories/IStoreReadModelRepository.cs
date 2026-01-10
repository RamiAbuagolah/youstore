using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YouStore.ReadModels;

namespace YouStore.Application.Interfaces.Repositories;

public interface IStoreReadModelRepository
{
    Task<IEnumerable<StoreReadModel>> ListAsync(string? search, CancellationToken cancellationToken = default);
    Task<StoreReadModel?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
