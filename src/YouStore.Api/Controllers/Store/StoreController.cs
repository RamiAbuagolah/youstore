using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YouStore.Api.Extensions;
using YouStore.Api.Requests;
using YouStore.Application.Features.Store.Commands;

namespace YouStore.Api.Controllers.Store;

[ApiController]
[Route("stores")]
public sealed class StoreController : ControllerBase
{
    private readonly IMediator _mediator;

    public StoreController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateStoreRequest request)
    {
        var merchantId = User.GetGuidClaim(JwtRegisteredClaimNames.Sub);
        var tenantId = User.GetGuidClaim("tenantId");
        var command = new CreateStoreCommand(merchantId, tenantId, request.Name, request.Slug, request.TemplateId);
        var store = await _mediator.Send(command);
        return Created($"/stores/{store.Id}", store);
    }
}
