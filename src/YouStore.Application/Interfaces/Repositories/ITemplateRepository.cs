using System.Collections.Generic;
using System.Threading.Tasks;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Application.Interfaces.Repositories;

public interface ITemplateRepository
{
    Task<IEnumerable<Template>> GetAllAsync();
    Task<Template?> GetByIdAsync(TemplateId id);
}
