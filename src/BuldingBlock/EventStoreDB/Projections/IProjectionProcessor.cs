using BuldingBlock.EventStoreDB.Events;
using MediatR;

namespace BuldingBlock.EventStoreDB.Projections
{
    public interface IProjectionProcessor
    {
        Task ProcessEventAsync<T>(StreamEvent<T> streamEvent, CancellationToken cancellationToken = default)
            where T : INotification;
    }

}
