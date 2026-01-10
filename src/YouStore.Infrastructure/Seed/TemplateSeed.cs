using System;
using System.Collections.Generic;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;

namespace YouStore.Infrastructure.Seed;

public static class TemplateSeed
{
    public static IReadOnlyCollection<Template> AllTemplates { get; } = new List<Template>
    {
        new Template(TemplateId.From(Guid.Parse("08f8a83b-f0a9-4b80-8bbe-1ecb1c3f4f11")), "Modern", "Clean lines and a focus on imagery.", "https://example.com/templates/modern.png"),
        new Template(TemplateId.From(Guid.Parse("7ad10a1f-4aa3-4d9c-8dd0-96a5a9b1b33f")), "Editorial", "Magazine-style layout with featured stories.", "https://example.com/templates/editorial.png"),
        new Template(TemplateId.From(Guid.Parse("6d06c216-5d0f-4c6b-8613-a5c4b0c2b3c7")), "Boutique", "Soft colors and elegant typography for premium brands.", "https://example.com/templates/boutique.png")
    };
}
