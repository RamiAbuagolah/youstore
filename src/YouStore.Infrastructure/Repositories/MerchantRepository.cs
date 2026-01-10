using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;
using YouStore.Infrastructure.Persistence;

namespace YouStore.Infrastructure.Repositories;

internal sealed class MerchantRepository : IMerchantRepository
{
    private readonly YouStoreDbContext _context;

    public MerchantRepository(YouStoreDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(MerchantUser merchant)
    {
        await _context.MerchantUsers.AddAsync(merchant);
    }

    public Task<MerchantUser?> GetByEmailAsync(string email)
    {
        return _context.MerchantUsers
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Email == email.ToLowerInvariant());
    }

    public Task<MerchantUser?> GetByIdAsync(MerchantId id)
    {
        return _context.MerchantUsers
            .SingleOrDefaultAsync(m => m.Id == id);
    }
}
