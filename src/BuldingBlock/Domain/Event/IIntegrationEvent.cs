using MassTransit;
using MassTransit.Topology;

namespace BuldingBlock.Domain.Event
{
    [ExcludeFromTopology]
    public interface IIntegrationEvent : IEvent
    {
    }
}
