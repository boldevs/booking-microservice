using BuldingBlock.Domain.Event;

namespace BuldingBlock.Contracts.EventBus.Messages;

public record UserCreated(long Id, string Name, string PassportNumber) : IIntegrationEvent;
