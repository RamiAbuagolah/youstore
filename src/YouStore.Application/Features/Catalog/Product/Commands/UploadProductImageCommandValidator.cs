using FluentValidation;

namespace YouStore.Application.Features.Catalog.Product.Commands;

internal sealed class UploadProductImageCommandValidator : AbstractValidator<UploadProductImageCommand>
{
    public UploadProductImageCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.Payload).NotEmpty();
    }
}
