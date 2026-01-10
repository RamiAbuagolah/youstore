using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YouStore.Application.Interfaces;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Application.Models;
using YouStore.Domain.Entities;
using DomainStore = YouStore.Domain.Entities.Store;
using YouStore.Domain.Events;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Features.Store.Commands;

internal sealed class CreateStoreCommandHandler : IRequestHandler<CreateStoreCommand, StoreDto>
{
    private readonly IMerchantRepository _merchantRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly ITemplateRepository _templateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutboxService _outboxService;

    public CreateStoreCommandHandler(
        IMerchantRepository merchantRepository,
        IStoreRepository storeRepository,
        ITemplateRepository templateRepository,
        IUnitOfWork unitOfWork,
        IOutboxService outboxService)
    {
        _merchantRepository = merchantRepository;
        _storeRepository = storeRepository;
        _templateRepository = templateRepository;
        _unitOfWork = unitOfWork;
        _outboxService = outboxService;
    }

    public async Task<StoreDto> Handle(CreateStoreCommand request, CancellationToken cancellationToken)
    {
        var tenantId = TenantId.From(request.TenantId);
        var merchantId = MerchantId.From(request.MerchantId);
        var templateId = TemplateId.From(request.TemplateId);

        var merchant = await _merchantRepository.GetByIdAsync(merchantId);
        if (merchant is null || merchant.TenantId != tenantId)
        {
            throw new InvalidOperationException("Merchant not found.");
        }

        var template = await _templateRepository.GetByIdAsync(templateId);
        if (template is null)
        {
            throw new InvalidOperationException("Selected template does not exist.");
        }

        var slug = request.Slug.Trim().ToLowerInvariant();
        if (await _storeRepository.ExistsBySlugAsync(tenantId, slug))
        {
            throw new InvalidOperationException("Store slug already in use.");
        }

        var store = DomainStore.CreateTenantStore(tenantId, merchantId, request.Name, slug, template.Id);
        await _storeRepository.AddAsync(store);

        var @event = new StoreCreatedEvent(store.Id.Value, tenantId.Value, merchantId.Value, store.Name, store.Slug, store.TemplateId.Value, store.CreatedAt);
        await _outboxService.SaveEventAsync(@event, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new StoreDto(store.Id.Value, store.TenantId.Value, store.MerchantId.Value, store.Name, store.Slug, store.TemplateId.Value, store.CreatedAt);
    }
}
