namespace BuldingBlock.Mongo
{
    public interface IMongoUnitOfWork<out TContext> : IUnitOfWork<TContext> where TContext : class, IMongoDbContext
    {
    }

}