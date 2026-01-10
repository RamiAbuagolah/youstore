using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Application.Models;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Features.Catalog.Product.Queries;

internal sealed class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public ListProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
    {
        var storeId = StoreId.From(request.StoreId);
        var products = await _productRepository.GetByStoreAsync(storeId);
        return products.Select(p => new ProductDto(
            p.Id.Value,
            p.TenantId.Value,
            p.StoreId.Value,
            p.CategoryId.Value,
            p.Name,
            p.Description,
            p.Price,
            p.Currency,
            p.DiscountPrice,
            p.DiscountDescription,
            p.IsPublished,
            p.CreatedAt,
            p.UpdatedAt));
    }
}
