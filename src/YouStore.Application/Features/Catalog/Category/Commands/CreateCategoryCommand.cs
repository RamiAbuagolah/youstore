using MediatR;
using YouStore.Application.Models;

namespace YouStore.Application.Features.Catalog.Category.Commands;

public sealed record CreateCategoryCommand(Guid TenantId, Guid StoreId, string Name, string Slug, string? Description) : IRequest<CategoryDto>;
