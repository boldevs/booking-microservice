using System;
using System.Linq;
using MediatR;

namespace BuldingBlock.Caching
{
    public interface IInvalidateCacheRequest
    {
        string CacheKey { get; }
    }
}