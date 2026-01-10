using System.Collections.Generic;
using MediatR;
using YouStore.Application.Models;

namespace YouStore.Application.Features.Template.Queries;

public sealed record ListTemplatesQuery() : IRequest<IEnumerable<TemplateDto>>;
