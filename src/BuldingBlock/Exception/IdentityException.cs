using System.Net;

namespace BuldingBlock.Exception
{
    public IdentityException(string message = default, HttpStatusCode statusCode = default)
        : base(message, statusCode)
    {
    }
}