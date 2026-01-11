using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YouStore.Api.Extensions;
using YouStore.Api.Requests;
using YouStore.Application.Features.Theme.Commands;

namespace YouStore.Api.Controllers.Theme;

[ApiController]
[Route("stores/{storeId:guid}/theme")]
public sealed class ThemeController : ControllerBase
{
    private readonly IMediator _mediator;

    public ThemeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Update(Guid storeId, UpdateThemeRequest request)
    {
        var tenantId = User.GetGuidClaim("tenantId");
        var command = new UpdateThemeCommand(storeId, tenantId, request.PrimaryColor, request.AccentColor, request.FontFamily, request.Background);
        var theme = await _mediator.Send(command);
        return Ok(theme);
    }
}
