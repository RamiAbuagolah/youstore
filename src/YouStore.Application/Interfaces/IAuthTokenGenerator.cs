using YouStore.Domain.Entities;

namespace YouStore.Application.Interfaces;

public interface IAuthTokenGenerator
{
    string GenerateToken(MerchantUser merchant);
}
