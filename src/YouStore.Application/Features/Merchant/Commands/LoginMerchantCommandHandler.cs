using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YouStore.Application.Interfaces;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Application.Models;

namespace YouStore.Application.Features.Merchant.Commands;

internal sealed class LoginMerchantCommandHandler : IRequestHandler<LoginMerchantCommand, MerchantLoginResult>
{
    private readonly IMerchantRepository _merchantRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthTokenGenerator _tokenGenerator;

    public LoginMerchantCommandHandler(IMerchantRepository merchantRepository, IPasswordHasher passwordHasher, IAuthTokenGenerator tokenGenerator)
    {
        _merchantRepository = merchantRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<MerchantLoginResult> Handle(LoginMerchantCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var merchant = await _merchantRepository.GetByEmailAsync(normalizedEmail);
        if (merchant is null || !_passwordHasher.Verify(merchant.PasswordHash, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var token = _tokenGenerator.GenerateToken(merchant);
        return new MerchantLoginResult(token, merchant.Id.Value, merchant.TenantId.Value);
    }
}
