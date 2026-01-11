using MediatR;
using Microsoft.AspNetCore.Mvc;
using YouStore.Application.Features.Template.Queries;

namespace YouStore.Api.Controllers.Template;

[ApiController]
[Route("templates")]
public sealed class TemplateController : ControllerBase
{
    private readonly IMediator _mediator;

    public TemplateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var templates = await _mediator.Send(new ListTemplatesQuery());
        return Ok(templates);
    }
}
