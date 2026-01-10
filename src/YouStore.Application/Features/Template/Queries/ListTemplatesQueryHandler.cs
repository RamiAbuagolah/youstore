using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Application.Models;

namespace YouStore.Application.Features.Template.Queries;

internal sealed class ListTemplatesQueryHandler : IRequestHandler<ListTemplatesQuery, IEnumerable<TemplateDto>>
{
    private readonly ITemplateRepository _templateRepository;

    public ListTemplatesQueryHandler(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task<IEnumerable<TemplateDto>> Handle(ListTemplatesQuery request, CancellationToken cancellationToken)
    {
        var templates = await _templateRepository.GetAllAsync();
        return templates.Select(t => new TemplateDto(t.Id.Value, t.Name, t.Description, t.PreviewImageUrl));
    }
}
