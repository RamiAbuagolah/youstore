using System;
using FluentAssertions;
using YouStore.Application.Features.Catalog.Category.Commands;
using Xunit;

namespace YouStore.Application.Tests;

public class CreateCategoryCommandValidatorTests
{
    private readonly CreateCategoryCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Slug_Is_Empty()
    {
        var command = new CreateCategoryCommand(Guid.NewGuid(), Guid.NewGuid(), "Name", string.Empty, "desc");
        var result = _validator.Validate(command);
        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(CreateCategoryCommand.Slug));
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Command()
    {
        var command = new CreateCategoryCommand(Guid.NewGuid(), Guid.NewGuid(), "Name", "slug", "desc");
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}
