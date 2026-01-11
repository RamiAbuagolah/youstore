using System.Linq;
using System.Text;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using YouStore.Application.Common.Behaviors;
using YouStore.Application.Features.Merchant.Commands;
using YouStore.Application.Interfaces;
using YouStore.Api.Services;
using YouStore.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

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

app.MapControllers();

app.Run();
