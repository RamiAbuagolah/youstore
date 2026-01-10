using System.Threading;
using System.Threading.Tasks;

namespace YouStore.Application.Interfaces;

public interface IBlobStorage
{
    Task<string> UploadAsync(string container, string blobName, byte[] content, string contentType, CancellationToken cancellationToken = default);
}
