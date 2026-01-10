using FluentValidation;

namespace YouStore.Application.Features.Catalog.Product.Commands;

internal sealed class SetProductDiscountCommandValidator : AbstractValidator<SetProductDiscountCommand>
{
    public SetProductDiscountCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.StoreId).NotEmpty();
        RuleFor(x => x.DiscountPrice).GreaterThan(0);
    }
}
