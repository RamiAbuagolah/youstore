namespace YouStore.Api.Requests;

public sealed record UpdateCategoryRequest(string Name, string Slug, string? Description);
