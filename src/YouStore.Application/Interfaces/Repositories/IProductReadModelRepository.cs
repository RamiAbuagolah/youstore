using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YouStore.ReadModels;

namespace YouStore.Application.Interfaces.Repositories;

public interface IProductReadModelRepository
{
    Task<IEnumerable<ProductReadModel>> SearchAsync(string? search, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductReadModel>> ListByStoreAsync(string slug, CancellationToken cancellationToken = default);
}
