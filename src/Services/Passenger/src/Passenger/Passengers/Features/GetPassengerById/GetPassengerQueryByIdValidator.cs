using FluentValidation;

namespace Passenger.Passengers.Features.GetPassengerById
{
    public class GetPassengerQueryByIdValidator : AbstractValidator<GetPassengerQueryById>
    {
        public GetPassengerQueryByIdValidator()
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Id is required!");
        }
    }
}
