namespace BuldingBlock.Mongo
{
    public interface ITransactionAble
    {
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    }

}