using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using YouStore.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace YouStore.Infrastructure.Services;

internal sealed class AzureBlobStorage : IBlobStorage
{
    private readonly BlobServiceClient _client;

    public AzureBlobStorage(IConfiguration configuration)
    {
        var connectionString = configuration["AzConnectionString"] ?? throw new InvalidOperationException("AzConnectionString must be provided for blob uploads.");
        _client = new BlobServiceClient(connectionString);
    }

    public async Task<string> UploadAsync(string container, string blobName, byte[] content, string contentType, CancellationToken cancellationToken = default)
    {
        var containerClient = _client.GetBlobContainerClient(container);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);
        var blobClient = containerClient.GetBlobClient(blobName);
        await using var stream = new MemoryStream(content);
        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);
        return blobClient.Uri.ToString();
    }
}
