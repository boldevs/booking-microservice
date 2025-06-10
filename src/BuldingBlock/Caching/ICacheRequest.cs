using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuldingBlock.Caching
{
    public interface ICacheRequest
    {
        string CacheKey { get; }
        DateTime? AbsoluteExpirationRelativeToNow { get; }
    }

}