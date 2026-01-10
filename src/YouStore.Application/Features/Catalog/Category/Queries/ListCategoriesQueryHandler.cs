using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Application.Models;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Features.Catalog.Category.Queries;

internal sealed class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public ListCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
    {
        var storeId = StoreId.From(request.StoreId);
        var categories = await _categoryRepository.GetByStoreAsync(storeId);
        return categories.Select(c => new CategoryDto(c.Id.Value, c.TenantId.Value, c.StoreId.Value, c.Name, c.Slug, c.Description, c.CreatedAt));
    }
}
