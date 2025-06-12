using Flight.Airports.Features.CreateAirport;
using Flight.Seats.Models;
using FluentValidation;

namespace Flight.Seats.Features.CreateSeat;

public class CreateSeatCommandValidator : AbstractValidator<CreateSeatCommand>
{
    public CreateSeatCommandValidator()
    {
        RuleFor(x => x.SeatNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("SeatNumber is required");

        RuleFor(x => x.FlightId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("FlightId is required");

        RuleFor(x => x.Class)
            .Cascade(CascadeMode.Stop)
            .Must(p => p.GetType().IsEnum &&
                      (p == SeatClass.FirstClass ||
                       p == SeatClass.Business ||
                       p == SeatClass.Economy))
            .WithMessage("Class must be FirstClass, Business or Economy");
    }
}
