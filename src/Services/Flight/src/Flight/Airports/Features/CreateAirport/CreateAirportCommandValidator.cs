using FluentValidation;

namespace Flight.Airports.Features.CreateAirport;

public class CreateAirportCommandValidator : AbstractValidator<CreateAirportCommand>
{
    public CreateAirportCommandValidator()
    {
        RuleFor(x => x.Code)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Code is required");

        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.Address)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Address is required");
    }
}
