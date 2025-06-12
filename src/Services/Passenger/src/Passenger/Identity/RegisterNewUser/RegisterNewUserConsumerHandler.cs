using Ardalis.GuardClauses;
using BuldingBlock.Contracts.EventBus.Messages;
using BuldingBlock.IdsGenerator;
using MassTransit;
using Passenger.Data;

namespace Passenger.Identity.RegisterNewUser;

public class RegisterNewUserConsumerHandler : IConsumer<UserCreated>
{
    private readonly PassengerDbContext _passengerDbContext;

    public RegisterNewUserConsumerHandler(PassengerDbContext passengerDbContext)
    {
        _passengerDbContext = passengerDbContext;
    }

    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        Guard.Against.Null(context.Message, nameof(UserCreated));

        var passenger = Passengers.Models.Passenger.Create(context.Message.Id, context.Message.Name, context.Message.PassportNumber);

        await _passengerDbContext.AddAsync(passenger);

        await _passengerDbContext.SaveChangesAsync();
    }
}
