using Microsoft.AspNetCore.Http;

namespace YouStore.Api.Requests;

public sealed class UploadProductImageRequest
{
    public IFormFile? File { get; set; }
    public bool IsPrimary { get; set; }
}
