using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YouStore.Application.Interfaces;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Features.Catalog.Product.Commands;

internal sealed class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommand, string>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductImageRepository _productImageRepository;
    private readonly IBlobStorage _blobStorage;
    private readonly IUnitOfWork _unitOfWork;

    public UploadProductImageCommandHandler(
        IProductRepository productRepository,
        IProductImageRepository productImageRepository,
        IBlobStorage blobStorage,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _productImageRepository = productImageRepository;
        _blobStorage = blobStorage;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.From(request.ProductId);
        var tenantId = TenantId.From(request.TenantId);
        var storeId = StoreId.From(request.StoreId);

        var product = await _productRepository.GetByIdAsync(productId);
        if (product is null || product.StoreId != storeId || product.TenantId != tenantId)
        {
            throw new InvalidOperationException("Product not found.");
        }

        var blobName = $"stores/{storeId.Value}/products/{productId.Value}/{Guid.NewGuid()}-{request.FileName}";
        var url = await _blobStorage.UploadAsync("product-images", blobName, request.Payload, request.ContentType, cancellationToken);

        var image = ProductImage.Create(tenantId, storeId, productId, url, request.IsPrimary);
        await _productImageRepository.AddAsync(image);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return url;
    }
}
