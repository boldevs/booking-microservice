using EasyCaching.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuldingBlock.Caching;

/// <summary>
/// Caching pipeline behavior for MediatR requests that implement <see cref="ICacheRequest"/>.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public sealed class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    private readonly IEasyCachingProvider _cachingProvider;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;
    private const int DefaultCacheExpirationInHours = 1;

    public CachingBehavior(
        IEasyCachingProviderFactory cachingFactory,
        ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _cachingProvider = cachingFactory?.GetCachingProvider("mem")
            ?? throw new ArgumentNullException(nameof(cachingFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
    {
        if (request is not ICacheRequest cacheRequest)
        {
            return await next();
        }

        var cacheKey = cacheRequest.CacheKey;
        var cacheResult = await _cachingProvider.GetAsync<TResponse>(cacheKey, cancellationToken);

        if (cacheResult.HasValue)
        {
            _logger.LogDebug("Cache hit for {RequestType} ({CacheKey})", typeof(TRequest).Name, cacheKey);
            return cacheResult.Value;
        }

        var response = await next();

        var expiration = cacheRequest.AbsoluteExpirationRelativeToNow
            ?? TimeSpan.FromHours(DefaultCacheExpirationInHours);

        await _cachingProvider.SetAsync(cacheKey, response, expiration, cancellationToken);

        _logger.LogDebug("Cache set for {RequestType} ({CacheKey}) with expiration {Expiration}", typeof(TRequest).Name, cacheKey, expiration);

        return response;
    }
}
