namespace YouStore.Api.Requests;

public sealed record UpdateProductRequest(Guid CategoryId, string Name, string Description, decimal Price, string Currency);
