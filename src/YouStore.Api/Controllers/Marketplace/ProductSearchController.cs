using Microsoft.AspNetCore.Mvc;
using YouStore.Application.Interfaces.Repositories;

namespace YouStore.Api.Controllers.Marketplace;

[ApiController]
[Route("products")]
public sealed class ProductSearchController : ControllerBase
{
    private readonly IProductReadModelRepository _products;

    public ProductSearchController(IProductReadModelRepository products)
    {
        _products = products;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? search)
    {
        var items = await _products.SearchAsync(search);
        return Ok(items);
    }
}
