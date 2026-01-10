using System;
using FluentAssertions;
using YouStore.Application.Features.Catalog.Product.Commands;
using Xunit;

namespace YouStore.Application.Tests;

public class SetProductDiscountCommandValidatorTests
{
    private readonly SetProductDiscountCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Discount_Invalid()
    {
        var command = new SetProductDiscountCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), -1, "desc");
        var result = _validator.Validate(command);
        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(SetProductDiscountCommand.DiscountPrice));
    }

    [Fact]
    public void Should_Not_Have_Error_When_Discount_Is_Valid()
    {
        var command = new SetProductDiscountCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 10, "desc");
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}
