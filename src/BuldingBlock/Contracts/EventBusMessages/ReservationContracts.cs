using BuldingBlock.Domain.Event;

namespace BuldingBlock.Contracts.EventBus.Messages;

public record BookingCreated(long Id) : IIntegrationEvent;
