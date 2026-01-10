using MediatR;
using YouStore.Application.Models;

namespace YouStore.Application.Features.Catalog.Category.Commands;

public sealed record UpdateCategoryCommand(Guid CategoryId, Guid TenantId, Guid StoreId, string Name, string Slug, string? Description) : IRequest<CategoryDto>;
