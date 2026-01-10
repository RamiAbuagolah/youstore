using System.IO;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using MediatR;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using YouStore.Application.Interfaces;
using YouStore.Application.Common.Behaviors;
using YouStore.Application.Features.Catalog.Category.Commands;
using YouStore.Application.Features.Catalog.Category.Queries;
using YouStore.Application.Features.Catalog.Product.Commands;
using YouStore.Application.Features.Catalog.Product.Queries;
using YouStore.Application.Features.Merchant.Commands;
using YouStore.Application.Features.Store.Commands;
using YouStore.Application.Features.Template.Queries;
using YouStore.Application.Features.Theme.Commands;
using YouStore.Api.Requests;
using YouStore.Api.Services;
using YouStore.Infrastructure.Extensions;
using YouStore.Infrastructure.Persistence;
using YouStore.Application.Interfaces.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://127.0.0.1:5000");

builder.Services.AddControllers();

builder.Services.AddProblemDetails(options =>
{
    options.Map<ValidationException>((context, ex) =>
    {
        var errors = ex.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(error => error.ErrorMessage!).ToArray());

        return new ValidationProblemDetails(errors)
        {
            Title = "Validation failed",
            Status = StatusCodes.Status400BadRequest
        };
    });

    options.Map<UnauthorizedAccessException>((context, ex) => new ProblemDetails
    {
        Title = "Unauthorized",
        Status = StatusCodes.Status401Unauthorized,
        Detail = ex.Message
    });

    options.Map<InvalidOperationException>((context, ex) => new ProblemDetails
    {
        Title = "Invalid operation",
        Status = StatusCodes.Status400BadRequest,
        Detail = ex.Message
    });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMediatR(typeof(RegisterMerchantCommand).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(RegisterMerchantCommand).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<IAuthTokenGenerator, JwtAuthTokenGenerator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "YouStore.Api";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "YouStore";
if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException("Jwt:Key must be configured.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseProblemDetails();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Ok(new { service = "YouStore.Api", status = "running" }))
    .WithName("root");

app.MapPost("/merchants/register", async (IMediator mediator, RegisterMerchantRequest request) =>
{
    var command = new RegisterMerchantCommand(request.Email, request.Password);
    var result = await mediator.Send(command);
    return Results.Created($"/merchants/{result.Id}", result);
});

app.MapPost("/merchants/login", async (IMediator mediator, LoginMerchantRequest request) =>
{
    var command = new LoginMerchantCommand(request.Email, request.Password);
    var result = await mediator.Send(command);
    return Results.Ok(result);
});

app.MapGet("/templates", async (IMediator mediator) =>
{
    var templates = await mediator.Send(new ListTemplatesQuery());
    return Results.Ok(templates);
});

app.MapPost("/stores", async (IMediator mediator, CreateStoreRequest request, ClaimsPrincipal user) =>
{
    var merchantId = GetGuidClaim(user, JwtRegisteredClaimNames.Sub);
    var tenantId = GetGuidClaim(user, "tenantId");
    var command = new CreateStoreCommand(merchantId, tenantId, request.Name, request.Slug, request.TemplateId);
    var store = await mediator.Send(command);
    return Results.Created($"/stores/{store.Id}", store);
})
.RequireAuthorization();

app.MapPost("/stores/{storeId:guid}/theme", async (IMediator mediator, Guid storeId, UpdateThemeRequest request, ClaimsPrincipal user) =>
{
    var tenantId = GetGuidClaim(user, "tenantId");
    var command = new UpdateThemeCommand(storeId, tenantId, request.PrimaryColor, request.AccentColor, request.FontFamily, request.Background);
    var theme = await mediator.Send(command);
    return Results.Ok(theme);
})
.RequireAuthorization();

app.MapGet("/stores/{storeId:guid}/categories", async (IMediator mediator, Guid storeId) =>
{
    var categories = await mediator.Send(new ListCategoriesQuery(storeId));
    return Results.Ok(categories);
});

app.MapPost("/stores/{storeId:guid}/categories", async (IMediator mediator, Guid storeId, CreateCategoryRequest request, ClaimsPrincipal user) =>
{
    var tenantId = GetGuidClaim(user, "tenantId");
    var command = new CreateCategoryCommand(tenantId, storeId, request.Name, request.Slug, request.Description);
    var category = await mediator.Send(command);
    return Results.Created($"/stores/{storeId}/categories/{category.Id}", category);
})
.RequireAuthorization();

app.MapPut("/stores/{storeId:guid}/categories/{categoryId:guid}", async (IMediator mediator, Guid storeId, Guid categoryId, UpdateCategoryRequest request, ClaimsPrincipal user) =>
{
    var tenantId = GetGuidClaim(user, "tenantId");
    var command = new UpdateCategoryCommand(categoryId, tenantId, storeId, request.Name, request.Slug, request.Description);
    var category = await mediator.Send(command);
    return Results.Ok(category);
})
.RequireAuthorization();

app.MapDelete("/stores/{storeId:guid}/categories/{categoryId:guid}", async (IMediator mediator, Guid storeId, Guid categoryId, ClaimsPrincipal user) =>
{
    var tenantId = GetGuidClaim(user, "tenantId");
    await mediator.Send(new DeleteCategoryCommand(categoryId, tenantId, storeId));
    return Results.NoContent();
})
.RequireAuthorization();

app.MapGet("/stores/{storeId:guid}/products", async (IMediator mediator, Guid storeId) =>
{
    var products = await mediator.Send(new ListProductsQuery(storeId));
    return Results.Ok(products);
});

app.MapPost("/stores/{storeId:guid}/products", async (IMediator mediator, Guid storeId, CreateProductRequest request, ClaimsPrincipal user) =>
{
    var tenantId = GetGuidClaim(user, "tenantId");
    var command = new CreateProductCommand(tenantId, storeId, request.CategoryId, request.Name, request.Description, request.Price, request.Currency);
    var product = await mediator.Send(command);
    return Results.Created($"/stores/{storeId}/products/{product.Id}", product);
})
.RequireAuthorization();

app.MapPut("/stores/{storeId:guid}/products/{productId:guid}", async (IMediator mediator, Guid storeId, Guid productId, UpdateProductRequest request, ClaimsPrincipal user) =>
{
    var tenantId = GetGuidClaim(user, "tenantId");
    var command = new UpdateProductCommand(productId, tenantId, storeId, request.CategoryId, request.Name, request.Description, request.Price, request.Currency);
    var product = await mediator.Send(command);
    return Results.Ok(product);
})
.RequireAuthorization();

app.MapPatch("/stores/{storeId:guid}/products/{productId:guid}/publish", async (IMediator mediator, Guid storeId, Guid productId, SetProductPublishRequest request, ClaimsPrincipal user) =>
{
    var tenantId = GetGuidClaim(user, "tenantId");
    await mediator.Send(new SetProductPublishStatusCommand(productId, tenantId, storeId, request.IsPublished));
    return Results.NoContent();
})
.RequireAuthorization();

app.MapPatch("/stores/{storeId:guid}/products/{productId:guid}/discount", async (IMediator mediator, Guid storeId, Guid productId, SetProductDiscountRequest request, ClaimsPrincipal user) =>
{
    var tenantId = GetGuidClaim(user, "tenantId");
    await mediator.Send(new SetProductDiscountCommand(productId, tenantId, storeId, request.DiscountPrice, request.Description));
    return Results.NoContent();
})
.RequireAuthorization();

app.MapDelete("/stores/{storeId:guid}/products/{productId:guid}/discount", async (IMediator mediator, Guid storeId, Guid productId, ClaimsPrincipal user) =>
{
    var tenantId = GetGuidClaim(user, "tenantId");
    await mediator.Send(new ClearProductDiscountCommand(productId, tenantId, storeId));
    return Results.NoContent();
})
.RequireAuthorization();

app.MapPost("/stores/{storeId:guid}/products/{productId:guid}/images", async (IMediator mediator, Guid storeId, Guid productId, IFormFile file, bool isPrimary, ClaimsPrincipal user) =>
{
    var tenantId = GetGuidClaim(user, "tenantId");
    await using var ms = new MemoryStream();
    await file.CopyToAsync(ms);
    var command = new UploadProductImageCommand(tenantId, storeId, productId, file.FileName, file.ContentType ?? "application/octet-stream", ms.ToArray(), isPrimary);
    var url = await mediator.Send(command);
    return Results.Created($"/stores/{storeId}/products/{productId}/images", new { url });
})
.RequireAuthorization();

app.MapGet("/stores/{storeId:guid}/products/{productId:guid}/images", async (IMediator mediator, Guid productId) =>
{
    var images = await mediator.Send(new GetProductImagesQuery(productId));
    return Results.Ok(images);
});

app.MapGet("/marketplace/stores", async (IStoreReadModelRepository stores, string? search) =>
{
    var result = await stores.ListAsync(search);
    return Results.Ok(result);
});

app.MapGet("/s/{slug}", async (IStoreReadModelRepository stores, string slug) =>
{
    var store = await stores.GetBySlugAsync(slug);
    return store is null ? Results.NotFound() : Results.Ok(store);
});

app.MapGet("/s/{slug}/products", async (IProductReadModelRepository products, string slug) =>
{
    var items = await products.ListByStoreAsync(slug);
    return Results.Ok(items);
});

app.MapGet("/products/search", async (IProductReadModelRepository products, string? search) =>
{
    var items = await products.SearchAsync(search);
    return Results.Ok(items);
});

app.MapGet("/health/db", async (YouStoreDbContext dbContext) =>
{
    var canConnect = await dbContext.Database.CanConnectAsync();
    return canConnect
        ? Results.Ok(new { status = "Database reachable" })
        : Results.Problem("Unable to reach the configured database");
});

app.MapHealthChecks("/health");

app.Run();

static Guid GetGuidClaim(ClaimsPrincipal principal, string claimType)
{
    var raw = principal.FindFirst(claimType)?.Value;
    return Guid.TryParse(raw, out var value)
        ? value
        : throw new UnauthorizedAccessException("Claim is missing or invalid.");
}
