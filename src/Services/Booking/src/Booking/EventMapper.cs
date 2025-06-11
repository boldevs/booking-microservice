using Booking.Booking.Events.Domain;
using BuldingBlock.Contracts.EventBus.Messages;
using BuldingBlock.Domain;
using BuldingBlock.Domain.Event;

namespace Booking
{
    public sealed class EventMapper : IEventMapper
    {
        public IEnumerable<IIntegrationEvent> MapAll(IEnumerable<IDomainEvent> events)
        {
            return events.Select(Map);
        }

        public IIntegrationEvent Map(IDomainEvent @event)
        {
            return @event switch
            {
                BookingCreatedDomainEvent e => new BookingCreated(e.Id),
                _ => null
            };
        }
    }

}
