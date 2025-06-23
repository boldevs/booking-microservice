using BuldingBlock.Exception;
using System.Net;

namespace Identity.Identity.Exceptions;

public class LoginUserException : CustomException
{
    public LoginUserException(string message) : base(message, HttpStatusCode.Unauthorized)
    {
    }
}
