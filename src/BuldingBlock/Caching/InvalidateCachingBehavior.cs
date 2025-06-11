using EasyCaching.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuldingBlock.Caching
{
    public class InvalidateCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        private readonly IEasyCachingProvider _cachingProvider;
        private readonly ILogger<InvalidateCachingBehavior<TRequest, TResponse>> _logger;

        public InvalidateCachingBehavior(
            IEasyCachingProviderFactory cachingFactory,
            ILogger<InvalidateCachingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
            _cachingProvider = cachingFactory.GetCachingProvider("mem");
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // First run the next handler in the pipeline
            var response = await next();

            // Check if this request requires cache invalidation
            if (request is IInvalidateCacheRequest invalidateCacheRequest)
            {
                var cacheKey = invalidateCacheRequest.CacheKey;
                await _cachingProvider.RemoveAsync(cacheKey, cancellationToken);

                _logger.LogDebug("Invalidated cache for {TRequest}. CacheKey: {CacheKey}",
                    typeof(TRequest).FullName, cacheKey);
            }

            return response;
        }
    }
}
