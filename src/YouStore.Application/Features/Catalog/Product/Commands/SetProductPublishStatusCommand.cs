using MediatR;

namespace YouStore.Application.Features.Catalog.Product.Commands;

public sealed record SetProductPublishStatusCommand(Guid ProductId, Guid TenantId, Guid StoreId, bool IsPublished) : IRequest;
