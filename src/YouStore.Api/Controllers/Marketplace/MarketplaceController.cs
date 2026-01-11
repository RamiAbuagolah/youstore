using Microsoft.AspNetCore.Mvc;
using YouStore.Application.Interfaces.Repositories;

namespace YouStore.Api.Controllers.Marketplace;

[ApiController]
[Route("marketplace")]
public sealed class MarketplaceController : ControllerBase
{
    private readonly IStoreReadModelRepository _stores;

    public MarketplaceController(IStoreReadModelRepository stores)
    {
        _stores = stores;
    }

    [HttpGet("stores")]
    public async Task<IActionResult> ListStores([FromQuery] string? search)
    {
        var result = await _stores.ListAsync(search);
        return Ok(result);
    }
}
