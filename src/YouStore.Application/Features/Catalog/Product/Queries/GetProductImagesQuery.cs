using System.Collections.Generic;
using MediatR;
using YouStore.Application.Models;

namespace YouStore.Application.Features.Catalog.Product.Queries;

public sealed record GetProductImagesQuery(Guid ProductId) : IRequest<IEnumerable<ProductImageDto>>;
