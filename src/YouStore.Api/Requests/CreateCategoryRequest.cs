namespace YouStore.Api.Requests;

public sealed record CreateCategoryRequest(string Name, string Slug, string? Description);
