namespace BuildingBlocks.EventStoreDB.Events;

namespace BuldingBlock.EventStoreDB.Events
{
    public interface IProjection
    {
        void When(object @event);
    }
}