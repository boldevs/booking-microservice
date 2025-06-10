using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuldingBlock.Web
{
    public string TransformOutbound(object value)
    {
        // Slugify value
        return value == null
            ? null
            : Regex.Replace(value.ToString() ?? string.Empty, "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}