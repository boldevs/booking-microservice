using FluentValidation;

namespace Flight.Seats.Features.ReserveSeat;

public class ReserveSeatCommandValidator : AbstractValidator<ReserveSeatCommand>
{
    public ReserveSeatCommandValidator()
    {
        RuleFor(x => x.FlightId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("FlightId must not be empty");

        RuleFor(x => x.SeatNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("SeatNumber must not be empty");
    }
}
