using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using YouStore.Infrastructure.Persistence;

namespace YouStore.Api.Controllers.Health;

[ApiController]
[Route("health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Check([FromServices] HealthCheckService healthChecks)
    {
        var report = await healthChecks.CheckHealthAsync();
        if (report.Status == HealthStatus.Healthy)
        {
            return Ok(new { status = "Healthy" });
        }

        return StatusCode(StatusCodes.Status503ServiceUnavailable, new { status = report.Status.ToString() });
    }

    [HttpGet("db")]
    public async Task<IActionResult> CheckDatabase([FromServices] YouStoreDbContext dbContext)
    {
        var canConnect = await dbContext.Database.CanConnectAsync();
        return canConnect
            ? Ok(new { status = "Database reachable" })
            : Problem("Unable to reach the configured database");
    }
}
