using System.Net;
using BuldingBlock.Exception;

namespace Flight.Aircrafts.Exceptions;

public class AircraftAlreadyExistException : AppException
{
    public AircraftAlreadyExistException() : base("Flight already exist!", HttpStatusCode.Conflict)
    {
    }
}
