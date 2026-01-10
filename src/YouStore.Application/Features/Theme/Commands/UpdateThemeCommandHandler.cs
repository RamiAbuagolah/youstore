using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YouStore.Application.Interfaces;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Application.Models;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Features.Theme.Commands;

internal sealed class UpdateThemeCommandHandler : IRequestHandler<UpdateThemeCommand, ThemeDto>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IThemeRepository _themeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateThemeCommandHandler(IStoreRepository storeRepository, IThemeRepository themeRepository, IUnitOfWork unitOfWork)
    {
        _storeRepository = storeRepository;
        _themeRepository = themeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ThemeDto> Handle(UpdateThemeCommand request, CancellationToken cancellationToken)
    {
        var tenantId = TenantId.From(request.TenantId);
        var storeId = StoreId.From(request.StoreId);

        var store = await _storeRepository.GetByIdAsync(storeId);
        if (store is null || store.TenantId != tenantId)
        {
            throw new InvalidOperationException("Store not found for tenant.");
        }

        var theme = await _themeRepository.GetByStoreIdAsync(storeId);
        if (theme is null)
        {
            theme = new ThemeConfig(storeId, tenantId, request.PrimaryColor, request.AccentColor, request.FontFamily, request.Background);
            await _themeRepository.AddAsync(theme);
        }
        else
        {
            theme.Update(request.PrimaryColor, request.AccentColor, request.FontFamily, request.Background);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new ThemeDto(theme.PrimaryColor, theme.AccentColor, theme.FontFamily, theme.Background);
    }
}
