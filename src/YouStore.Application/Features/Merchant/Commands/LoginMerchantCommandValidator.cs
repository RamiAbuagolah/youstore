using FluentValidation;

namespace YouStore.Application.Features.Merchant.Commands;

internal sealed class LoginMerchantCommandValidator : AbstractValidator<LoginMerchantCommand>
{
    public LoginMerchantCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
