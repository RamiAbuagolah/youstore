using Microsoft.AspNetCore.Mvc;

namespace YouStore.Api.Controllers;

[ApiController]
[Route("")]
public sealed class RootController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { service = "YouStore.Api", status = "running" });
    }
}
