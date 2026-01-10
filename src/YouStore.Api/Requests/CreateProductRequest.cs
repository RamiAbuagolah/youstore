namespace YouStore.Api.Requests;

public sealed record CreateProductRequest(Guid CategoryId, string Name, string Description, decimal Price, string Currency);
