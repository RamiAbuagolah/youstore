using FluentValidation;

namespace YouStore.Application.Features.Catalog.Category.Commands;

internal sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
        RuleFor(x => x.Slug).NotEmpty().MaximumLength(128);
    }
}
