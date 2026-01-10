using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Application.Models;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Features.Catalog.Product.Queries;

internal sealed class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQuery, IEnumerable<ProductImageDto>>
{
    private readonly IProductImageRepository _productImageRepository;

    public GetProductImagesQueryHandler(IProductImageRepository productImageRepository)
    {
        _productImageRepository = productImageRepository;
    }

    public async Task<IEnumerable<ProductImageDto>> Handle(GetProductImagesQuery request, CancellationToken cancellationToken)
    {
        var productId = ProductId.From(request.ProductId);
        var images = await _productImageRepository.GetByProductAsync(productId);
        return images.Select(i => new ProductImageDto(i.Id.Value, i.TenantId.Value, i.StoreId.Value, i.ProductId.Value, i.Url, i.IsPrimary, i.UploadedAt));
    }
}
