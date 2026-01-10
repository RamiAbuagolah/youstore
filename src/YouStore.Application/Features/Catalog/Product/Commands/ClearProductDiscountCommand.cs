using MediatR;

namespace YouStore.Application.Features.Catalog.Product.Commands;

public sealed record ClearProductDiscountCommand(Guid ProductId, Guid TenantId, Guid StoreId) : IRequest;
