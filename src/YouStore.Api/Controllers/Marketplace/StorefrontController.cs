using Microsoft.AspNetCore.Mvc;
using YouStore.Application.Interfaces.Repositories;

namespace YouStore.Api.Controllers.Marketplace;

[ApiController]
[Route("s")]
public sealed class StorefrontController : ControllerBase
{
    private readonly IStoreReadModelRepository _stores;
    private readonly IProductReadModelRepository _products;

    public StorefrontController(IStoreReadModelRepository stores, IProductReadModelRepository products)
    {
        _stores = stores;
        _products = products;
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetStore(string slug)
    {
        var store = await _stores.GetBySlugAsync(slug);
        return store is null ? NotFound() : Ok(store);
    }

    [HttpGet("{slug}/products")]
    public async Task<IActionResult> ListProducts(string slug)
    {
        var items = await _products.ListByStoreAsync(slug);
        return Ok(items);
    }
}
