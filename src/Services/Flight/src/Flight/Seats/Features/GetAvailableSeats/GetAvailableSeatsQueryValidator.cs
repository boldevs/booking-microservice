using FluentValidation;

namespace Flight.Seats.Features.GetAvailableSeats;

public class GetAvailableSeatsQueryValidator : AbstractValidator<GetAvailableSeatsQuery>
{
    public GetAvailableSeatsQueryValidator()
    {
        RuleFor(x => x.FlightId)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("FlightId is required!");
    }
}
