namespace BuldingBlock.Caching
{
    /// <summary>
    /// Interface for cacheable requests.
    /// </summary>
    public interface ICacheRequest
    {
        /// <summary>
        /// Gets the unique cache key for this request.
        /// </summary>
        string CacheKey { get; }

        /// <summary>
        /// Gets the relative expiration for the cache entry. If null, the default is used.
        /// </summary>
        TimeSpan? AbsoluteExpirationRelativeToNow { get; }
    }
}
