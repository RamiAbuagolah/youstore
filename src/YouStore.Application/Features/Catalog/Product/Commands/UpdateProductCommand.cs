using System;
using MediatR;
using YouStore.Application.Models;

namespace YouStore.Application.Features.Catalog.Product.Commands;

public sealed record UpdateProductCommand(Guid ProductId, Guid TenantId, Guid StoreId, Guid CategoryId, string Name, string Description, decimal Price, string Currency) : IRequest<ProductDto>;
