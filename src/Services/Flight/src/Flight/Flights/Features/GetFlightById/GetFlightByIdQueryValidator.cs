using FluentValidation;

namespace Flight.Flights.Features.GetFlightById;

public class GetFlightByIdQueryValidator : AbstractValidator<GetFlightByIdQuery>
{
    public GetFlightByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Id is required!");
    }
}
