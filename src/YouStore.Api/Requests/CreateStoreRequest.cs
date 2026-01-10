using System;

namespace YouStore.Api.Requests;

public sealed record CreateStoreRequest(string Name, string Slug, Guid TemplateId);
