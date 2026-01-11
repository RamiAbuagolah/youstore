using System.IO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using YouStore.Api.Extensions;
using YouStore.Api.Requests;
using YouStore.Application.Features.Catalog.Product.Commands;
using YouStore.Application.Features.Catalog.Product.Queries;

namespace YouStore.Api.Controllers.Catalog;

[ApiController]
[Route("stores/{storeId:guid}/products")]
public sealed class StoreProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StoreProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> List(Guid storeId)
    {
        var products = await _mediator.Send(new ListProductsQuery(storeId));
        return Ok(products);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Guid storeId, CreateProductRequest request)
    {
        var tenantId = User.GetGuidClaim("tenantId");
        var command = new CreateProductCommand(tenantId, storeId, request.CategoryId, request.Name, request.Description, request.Price, request.Currency);
        var product = await _mediator.Send(command);
        return Created($"/stores/{storeId}/products/{product.Id}", product);
    }

    [Authorize]
    [HttpPut("{productId:guid}")]
    public async Task<IActionResult> Update(Guid storeId, Guid productId, UpdateProductRequest request)
    {
        var tenantId = User.GetGuidClaim("tenantId");
        var command = new UpdateProductCommand(productId, tenantId, storeId, request.CategoryId, request.Name, request.Description, request.Price, request.Currency);
        var product = await _mediator.Send(command);
        return Ok(product);
    }

    [Authorize]
    [HttpPatch("{productId:guid}/publish")]
    public async Task<IActionResult> Publish(Guid storeId, Guid productId, SetProductPublishRequest request)
    {
        var tenantId = User.GetGuidClaim("tenantId");
        await _mediator.Send(new SetProductPublishStatusCommand(productId, tenantId, storeId, request.IsPublished));
        return NoContent();
    }

    [Authorize]
    [HttpPatch("{productId:guid}/discount")]
    public async Task<IActionResult> SetDiscount(Guid storeId, Guid productId, SetProductDiscountRequest request)
    {
        var tenantId = User.GetGuidClaim("tenantId");
        await _mediator.Send(new SetProductDiscountCommand(productId, tenantId, storeId, request.DiscountPrice, request.Description));
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{productId:guid}/discount")]
    public async Task<IActionResult> ClearDiscount(Guid storeId, Guid productId)
    {
        var tenantId = User.GetGuidClaim("tenantId");
        await _mediator.Send(new ClearProductDiscountCommand(productId, tenantId, storeId));
        return NoContent();
    }

    [Authorize]
    [HttpPost("{productId:guid}/images")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(Guid storeId, Guid productId, [FromForm] UploadProductImageRequest request)
    {
        var tenantId = User.GetGuidClaim("tenantId");
        await using var ms = new MemoryStream();
        await request.File!.CopyToAsync(ms);
        var command = new UploadProductImageCommand(tenantId, storeId, productId, request.File.FileName, request.File.ContentType ?? "application/octet-stream", ms.ToArray(), request.IsPrimary);
        var url = await _mediator.Send(command);
        return Created($"/stores/{storeId}/products/{productId}/images", new { url });
    }

    [HttpGet("{productId:guid}/images")]
    public async Task<IActionResult> ListImages(Guid productId)
    {
        var images = await _mediator.Send(new GetProductImagesQuery(productId));
        return Ok(images);
    }
}
