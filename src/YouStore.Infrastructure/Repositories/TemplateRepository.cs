using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;
using YouStore.Infrastructure.Persistence;

namespace YouStore.Infrastructure.Repositories;

internal sealed class TemplateRepository : ITemplateRepository
{
    private readonly YouStoreDbContext _context;

    public TemplateRepository(YouStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Template>> GetAllAsync()
    {
        return await _context.Templates
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<Template?> GetByIdAsync(TemplateId id)
    {
        return _context.Templates
            .AsNoTracking()
            .SingleOrDefaultAsync(t => t.Id == id);
    }
}
