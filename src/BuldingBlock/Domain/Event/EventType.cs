
namespace BuldingBlock.Domain.Event
{
    [Flags]
    public enum EventType
    {
        IntegrationEvent = 1,
        DomainEvent = 2,
    }

}