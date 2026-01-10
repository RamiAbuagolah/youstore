using FluentValidation;

namespace YouStore.Application.Features.Catalog.Product.Commands;

internal sealed class SetProductPublishStatusCommandValidator : AbstractValidator<SetProductPublishStatusCommand>
{
    public SetProductPublishStatusCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.StoreId).NotEmpty();
    }
}
