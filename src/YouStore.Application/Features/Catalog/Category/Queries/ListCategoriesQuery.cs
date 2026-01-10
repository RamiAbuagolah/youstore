using System.Collections.Generic;
using MediatR;
using YouStore.Application.Models;

namespace YouStore.Application.Features.Catalog.Category.Queries;

public sealed record ListCategoriesQuery(Guid StoreId) : IRequest<IEnumerable<CategoryDto>>;
