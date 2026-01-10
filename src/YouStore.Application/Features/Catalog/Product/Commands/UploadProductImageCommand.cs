using MediatR;

namespace YouStore.Application.Features.Catalog.Product.Commands;

public sealed record UploadProductImageCommand(Guid TenantId, Guid StoreId, Guid ProductId, string FileName, string ContentType, byte[] Payload, bool IsPrimary) : IRequest<string>;
