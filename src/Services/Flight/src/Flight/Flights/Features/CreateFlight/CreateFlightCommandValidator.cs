using Flight.Flights.Models;
using FluentValidation;

namespace Flight.Flights.Features.CreateFlight;

public class CreateFlightCommandValidator : AbstractValidator<CreateFlightCommand>
{
    public CreateFlightCommandValidator()
    {
        RuleFor(x => x.Price)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");

        RuleFor(x => x.Status)
            .Cascade(CascadeMode.Stop)
            .Must(p => (p.GetType().IsEnum &&
                        (p == FlightStatus.Flying ||
                         p == FlightStatus.Canceled ||
                         p == FlightStatus.Delay ||
                         p == FlightStatus.Completed)))
            .WithMessage("Status must be Flying, Delay, Canceled or Completed");

        RuleFor(x => x.AircraftId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("AircraftId must be not empty");

        RuleFor(x => x.DepartureAirportId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("DepartureAirportId must be not empty");

        RuleFor(x => x.ArriveAirportId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ArriveAirportId must be not empty");

        RuleFor(x => x.DurationMinutes)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("DurationMinutes must be greater than 0");

        RuleFor(x => x.FlightDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("FlightDate must be not empty");
    }
}
