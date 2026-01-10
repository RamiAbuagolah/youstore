using System.Threading;
using System.Threading.Tasks;

namespace YouStore.Application.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
