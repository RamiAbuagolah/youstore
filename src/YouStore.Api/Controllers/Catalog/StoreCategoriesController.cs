using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YouStore.Api.Extensions;
using YouStore.Api.Requests;
using YouStore.Application.Features.Catalog.Category.Commands;
using YouStore.Application.Features.Catalog.Category.Queries;

namespace YouStore.Api.Controllers.Catalog;

[ApiController]
[Route("stores/{storeId:guid}/categories")]
public sealed class StoreCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public StoreCategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> List(Guid storeId)
    {
        var categories = await _mediator.Send(new ListCategoriesQuery(storeId));
        return Ok(categories);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Guid storeId, CreateCategoryRequest request)
    {
        var tenantId = User.GetGuidClaim("tenantId");
        var command = new CreateCategoryCommand(tenantId, storeId, request.Name, request.Slug, request.Description);
        var category = await _mediator.Send(command);
        return Created($"/stores/{storeId}/categories/{category.Id}", category);
    }

    [Authorize]
    [HttpPut("{categoryId:guid}")]
    public async Task<IActionResult> Update(Guid storeId, Guid categoryId, UpdateCategoryRequest request)
    {
        var tenantId = User.GetGuidClaim("tenantId");
        var command = new UpdateCategoryCommand(categoryId, tenantId, storeId, request.Name, request.Slug, request.Description);
        var category = await _mediator.Send(command);
        return Ok(category);
    }

    [Authorize]
    [HttpDelete("{categoryId:guid}")]
    public async Task<IActionResult> Delete(Guid storeId, Guid categoryId)
    {
        var tenantId = User.GetGuidClaim("tenantId");
        await _mediator.Send(new DeleteCategoryCommand(categoryId, tenantId, storeId));
        return NoContent();
    }
}
