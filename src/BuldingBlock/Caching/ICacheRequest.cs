namespace BuldingBlock.Caching
{
    public interface ICacheRequest
    {
        string CacheKey { get; }

        /// <summary>
        /// The relative expiration duration for the cache entry from now.
        /// If null, default expiration time is used.
        /// </summary>
        TimeSpan? AbsoluteExpirationRelativeToNow { get; }
    }
}
