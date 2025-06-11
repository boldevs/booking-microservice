using BuldingBlock.Domain.Event;
using MediatR;

namespace BuldingBlock.EventStoreDB.Events
{
    public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IEvent
    {
    }
}
