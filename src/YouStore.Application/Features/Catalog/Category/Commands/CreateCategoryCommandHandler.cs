using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YouStore.Application.Interfaces;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Application.Models;
using YouStore.Domain.Events;
using YouStore.Domain.ValueObjects;
using DomainCategory = YouStore.Domain.Entities.Category;

namespace YouStore.Application.Features.Catalog.Category.Commands;

internal sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutboxService _outboxService;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IOutboxService outboxService)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _outboxService = outboxService;
    }

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var storeId = StoreId.From(request.StoreId);
        var tenantId = TenantId.From(request.TenantId);
        var slug = request.Slug.Trim().ToLowerInvariant();
        if (await _categoryRepository.ExistsBySlugAsync(storeId, slug))
        {
            throw new InvalidOperationException("Category slug already exists for this store.");
        }

        var category = DomainCategory.Create(tenantId, storeId, request.Name, slug, request.Description);
        await _categoryRepository.AddAsync(category);
        var @event = new CategoryCreatedEvent(category.Id.Value, category.StoreId.Value, category.TenantId.Value, category.Name, category.Slug, category.CreatedAt);
        await _outboxService.SaveEventAsync(@event, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CategoryDto(category.Id.Value, tenantId.Value, storeId.Value, category.Name, category.Slug, category.Description, category.CreatedAt);
    }
}
