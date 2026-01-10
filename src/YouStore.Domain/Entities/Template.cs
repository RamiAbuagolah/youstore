using System;
using YouStore.Domain.Common;
using YouStore.Domain.ValueObjects;

namespace YouStore.Domain.Entities;

public class Template : Entity<TemplateId>, IAggregateRoot
{
    private Template() { }

    public Template(TemplateId id, string name, string description, string previewImageUrl)
    {
        Id = id;
        Name = name;
        Description = description;
        PreviewImageUrl = previewImageUrl;
    }

    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string PreviewImageUrl { get; private set; } = null!;
}
