using MediatR;

namespace BuldingBlock.Domain.Event
{
    public interface IEvent : INotification
    {
        Guid EventId => Guid.NewGuid();
        public DateTime OccurredOn => DateTime.Now;
        public string EventType => GetType().AssemblyQualifiedName;
    }

}
