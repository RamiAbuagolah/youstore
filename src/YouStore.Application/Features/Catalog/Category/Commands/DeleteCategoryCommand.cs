using MediatR;

namespace YouStore.Application.Features.Catalog.Category.Commands;

public sealed record DeleteCategoryCommand(Guid CategoryId, Guid TenantId, Guid StoreId) : IRequest;
