using FluentValidation;
using Passenger.Passengers.Models;

namespace Passenger.Passengers.Features.CompleteRegisterPassenger
{
    public class CompleteRegisterPassengerCommandValidator : AbstractValidator<CompleteRegisterPassengerCommand>
    {
        public CompleteRegisterPassengerCommandValidator()
        {
            // Optional: globally stop validation on first failure in the entire validator
            // this.CascadeMode = CascadeMode.Stop;  // Uncomment if you want global cascade mode

            RuleFor(x => x.PassportNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The PassportNumber is required!");

            RuleFor(x => x.Age)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("The Age must be greater than 0!");

            RuleFor(x => x.PassengerType)
                .Cascade(CascadeMode.Stop)
                .IsInEnum().WithMessage("PassengerType must be a valid enum value")
                .Must(p => p == PassengerType.Male ||
                           p == PassengerType.Female ||
                           p == PassengerType.Baby ||
                           p == PassengerType.Unknown)
                .WithMessage("PassengerType must be Male, Female, Baby, or Unknown");
        }
    }
}
