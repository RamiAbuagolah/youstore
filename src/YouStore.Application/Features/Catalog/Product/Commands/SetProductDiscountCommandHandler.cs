using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YouStore.Application.Interfaces;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Domain.Events;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Features.Catalog.Product.Commands;

internal sealed class SetProductDiscountCommandHandler : IRequestHandler<SetProductDiscountCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutboxService _outboxService;

    public SetProductDiscountCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IOutboxService outboxService)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _outboxService = outboxService;
    }

    public async Task<Unit> Handle(SetProductDiscountCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.From(request.ProductId);
        var tenantId = TenantId.From(request.TenantId);
        var storeId = StoreId.From(request.StoreId);

        var product = await _productRepository.GetByIdAsync(productId);
        if (product is null || product.StoreId != storeId || product.TenantId != tenantId)
        {
            throw new InvalidOperationException("Product not found.");
        }

        product.SetDiscount(request.DiscountPrice, request.Description);
        var @event = new ProductDiscountChangedEvent(product.Id.Value, product.StoreId.Value, product.TenantId.Value, product.DiscountPrice, product.DiscountDescription, product.UpdatedAt);
        await _outboxService.SaveEventAsync(@event, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
