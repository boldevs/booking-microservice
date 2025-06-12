using FluentValidation;

namespace Flight.Aircrafts.Features.CreateAircraft;

public class CreateAircraftCommandValidator : AbstractValidator<CreateAircraftCommand>
{
    public CreateAircraftCommandValidator()
    {
        RuleFor(x => x.Model)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Model is required");

        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.ManufacturingYear)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("ManufacturingYear is required");
    }
}
