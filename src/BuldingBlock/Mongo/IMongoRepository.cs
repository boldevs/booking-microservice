using BuldingBlock.Domain.Model;

namespace BuldingBlock.Mongo
{
     public interface IMongoRepository<TEntity, in TId> : IRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
    {
    }
}
