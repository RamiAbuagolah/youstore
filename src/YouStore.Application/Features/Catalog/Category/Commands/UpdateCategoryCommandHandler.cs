using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YouStore.Application.Interfaces;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Application.Models;
using YouStore.Domain.Events;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Features.Catalog.Category.Commands;

internal sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutboxService _outboxService;

    public UpdateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IOutboxService outboxService)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _outboxService = outboxService;
    }

    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.From(request.CategoryId);
        var tenantId = TenantId.From(request.TenantId);
        var storeId = StoreId.From(request.StoreId);
        var slug = request.Slug.Trim().ToLowerInvariant();

        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category is null || category.StoreId != storeId || category.TenantId != tenantId)
        {
            throw new InvalidOperationException("Category not found.");
        }

        category.Update(request.Name, slug, request.Description);
        var @event = new CategoryUpdatedEvent(category.Id.Value, category.StoreId.Value, category.TenantId.Value, category.Name, category.Slug, DateTime.UtcNow);
        await _outboxService.SaveEventAsync(@event, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CategoryDto(category.Id.Value, tenantId.Value, storeId.Value, category.Name, category.Slug, category.Description, category.CreatedAt);
    }
}
