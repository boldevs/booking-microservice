using EasyCaching.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuldingBlock.Caching
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        private readonly IEasyCachingProvider _cachingProvider;
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;
        private readonly int _defaultCacheExpirationInHours = 1;

        public CachingBehavior(
            IEasyCachingProviderFactory cachingFactory,
            ILogger<CachingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
            _cachingProvider = cachingFactory.GetCachingProvider("mem");
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Check if the request is a cacheable one
            if (request is not ICacheRequest cacheRequest)
            {
                return await next();
            }

            var cacheKey = cacheRequest.CacheKey;

            // Try to get the response from cache
            var cachedResponse = await _cachingProvider.GetAsync<TResponse>(cacheKey, cancellationToken);
            if (cachedResponse.HasValue)
            {
                _logger.LogDebug("Cache hit for {TRequest}. CacheKey: {CacheKey}", typeof(TRequest).FullName, cacheKey);
                return cachedResponse.Value;
            }

            // Proceed with the actual handler
            var response = await next();

            // Determine expiration
            var expiration = cacheRequest.AbsoluteExpirationRelativeToNow ??
                             TimeSpan.FromHours(_defaultCacheExpirationInHours);

            // Cache the response
            await _cachingProvider.SetAsync(cacheKey, response, expiration, cancellationToken);

            _logger.LogDebug("Cached response for {TRequest}. CacheKey: {CacheKey}", typeof(TRequest).FullName, cacheKey);

            return response;
        }
    }
}
