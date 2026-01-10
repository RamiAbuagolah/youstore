using System.Collections.Generic;
using MediatR;
using YouStore.Application.Models;

namespace YouStore.Application.Features.Catalog.Product.Queries;

public sealed record ListProductsQuery(Guid StoreId) : IRequest<IEnumerable<ProductDto>>;
