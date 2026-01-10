using FluentValidation;

namespace YouStore.Application.Features.Theme.Commands;

internal sealed class UpdateThemeCommandValidator : AbstractValidator<UpdateThemeCommand>
{
    public UpdateThemeCommandValidator()
    {
        RuleFor(x => x.StoreId).NotEmpty();
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.PrimaryColor).NotEmpty();
        RuleFor(x => x.AccentColor).NotEmpty();
        RuleFor(x => x.FontFamily).NotEmpty();
        RuleFor(x => x.Background).NotEmpty();
    }
}
