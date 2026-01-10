using System;

namespace YouStore.Application.Models;

public sealed record TemplateDto(Guid Id, string Name, string Description, string PreviewImageUrl);
