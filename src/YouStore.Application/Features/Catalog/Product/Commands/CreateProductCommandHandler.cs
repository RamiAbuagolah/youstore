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

internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutboxService _outboxService;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IOutboxService outboxService)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _outboxService = outboxService;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var tenantId = TenantId.From(request.TenantId);
        var storeId = StoreId.From(request.StoreId);
        var categoryId = CategoryId.From(request.CategoryId);

        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category is null || category.StoreId != storeId || category.TenantId != tenantId)
        {
            throw new InvalidOperationException("Category not found.");
        }

        var product = YouStore.Domain.Entities.Product.Create(tenantId, storeId, categoryId, request.Name, request.Description, request.Price, request.Currency);
        await _productRepository.AddAsync(product);
        category.AttachProduct(product.Id);

        var @event = new ProductCreatedEvent(
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
            product.CreatedAt);
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
