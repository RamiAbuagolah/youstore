using MediatR;

namespace YouStore.Application.Features.Catalog.Product.Commands;

public sealed record SetProductDiscountCommand(Guid ProductId, Guid TenantId, Guid StoreId, decimal DiscountPrice, string Description) : IRequest;
