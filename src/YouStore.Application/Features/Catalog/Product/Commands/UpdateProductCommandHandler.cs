using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YouStore.Application.Interfaces;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Application.Models;
using YouStore.Domain.Events;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Features.Catalog.Product.Commands;

internal sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutboxService _outboxService;

    public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IOutboxService outboxService)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _outboxService = outboxService;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.From(request.ProductId);
        var tenantId = TenantId.From(request.TenantId);
        var storeId = StoreId.From(request.StoreId);

        var product = await _productRepository.GetByIdAsync(productId);
        if (product is null || product.StoreId != storeId || product.TenantId != tenantId)
        {
            throw new InvalidOperationException("Product not found.");
        }

        product.UpdateDetails(request.Name, request.Description, request.Price, request.Currency);
        var @event = new ProductUpdatedEvent(
            product.Id.Value,
            product.StoreId.Value,
            product.TenantId.Value,
            product.CategoryId.Value,
            product.Name,
            product.Description,
            product.Price,
            product.Currency,
            product.IsPublished,
            product.DiscountPrice,
            product.DiscountDescription,
            product.CreatedAt,
            product.UpdatedAt);
        await _outboxService.SaveEventAsync(@event, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(product);
    }

    private static ProductDto Map(YouStore.Domain.Entities.Product product)
        => new(
            product.Id.Value,
            product.TenantId.Value,
            product.StoreId.Value,
            product.CategoryId.Value,
            product.Name,
            product.Description,
            product.Price,
            product.Currency,
            product.DiscountPrice,
            product.DiscountDescription,
            product.IsPublished,
            product.CreatedAt,
            product.UpdatedAt);
}
