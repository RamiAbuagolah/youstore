using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YouStore.Application.Interfaces;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Application.Models;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Features.Merchant.Commands;

internal sealed class RegisterMerchantCommandHandler : IRequestHandler<RegisterMerchantCommand, MerchantDto>
{
    private readonly IMerchantRepository _merchantRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterMerchantCommandHandler(IMerchantRepository merchantRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        _merchantRepository = merchantRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<MerchantDto> Handle(RegisterMerchantCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var existing = await _merchantRepository.GetByEmailAsync(normalizedEmail);
        if (existing is not null)
        {
            throw new InvalidOperationException("Merchant with this email already exists.");
        }

        var tenantId = TenantId.New();
        var merchant = MerchantUser.Create(tenantId, normalizedEmail, _passwordHasher.Hash(request.Password));
        await _merchantRepository.AddAsync(merchant);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new MerchantDto(merchant.Id.Value, merchant.TenantId.Value, merchant.Email);
    }
}
