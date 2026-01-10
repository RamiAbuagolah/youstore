using FluentValidation;

namespace YouStore.Application.Features.Merchant.Commands;

internal sealed class RegisterMerchantCommandValidator : AbstractValidator<RegisterMerchantCommand>
{
    public RegisterMerchantCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}
