using MediatR;
using Microsoft.AspNetCore.Mvc;
using YouStore.Api.Requests;
using YouStore.Application.Features.Merchant.Commands;

namespace YouStore.Api.Controllers.Merchant;

[ApiController]
[Route("merchants")]
public sealed class MerchantController : ControllerBase
{
    private readonly IMediator _mediator;

    public MerchantController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterMerchantRequest request)
    {
        var command = new RegisterMerchantCommand(request.Email, request.Password);
        var result = await _mediator.Send(command);
        return Created($"/merchants/{result.Id}", result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginMerchantRequest request)
    {
        var command = new LoginMerchantCommand(request.Email, request.Password);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
