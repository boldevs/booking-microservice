using FluentValidation;

namespace Identity.Identity.Features.RegisterNewUser;

public class RegisterNewUserValidator : AbstractValidator<RegisterNewUserCommand>
{
    public RegisterNewUserValidator()
    {
        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Please enter the password");

        RuleFor(x => x.ConfirmPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Please enter the confirmation password");

        RuleFor(x => x.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Please enter the username");

        RuleFor(x => x.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Please enter the first name");

        RuleFor(x => x.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Please enter the last name");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Please enter the email")
            .EmailAddress().WithMessage("A valid email is required");

        RuleFor(x => x).Custom((x, context) =>
        {
            if (!string.IsNullOrWhiteSpace(x.Password) &&
                !string.IsNullOrWhiteSpace(x.ConfirmPassword) &&
                x.Password != x.ConfirmPassword)
            {
                context.AddFailure(nameof(x.ConfirmPassword), "Passwords should match");
            }
        });
    }
}
