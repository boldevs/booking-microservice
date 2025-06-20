using System.Data;
using BuldingBlock.Domain.Event;
using Microsoft.EntityFrameworkCore;

namespace BuldingBlock.EFCore
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;
        IReadOnlyList<IDomainEvent> GetDomainEvents();
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
