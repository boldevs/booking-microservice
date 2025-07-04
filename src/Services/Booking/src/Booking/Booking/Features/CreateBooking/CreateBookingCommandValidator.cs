using FluentValidation;

namespace Booking.Booking.Features.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.FlightId)
                .NotNull()
                .WithMessage("FlightId is required!");

            RuleFor(x => x.PassengerId)
                .NotNull()
                .WithMessage("PassengerId is required!");
        }

    }
}
