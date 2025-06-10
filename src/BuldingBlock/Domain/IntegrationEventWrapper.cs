using BuildingBlocks.Domain.Event;

namespace BuldingBlock.Domain
{
    public record IntegrationEventWrapper<TDomainEventType>(TDomainEventType DomainEvent) : IIntegrationEvent
    where TDomainEventType : IDomainEvent;

}