using BuldingBlock.Exception;

namespace Booking.Booking.Exceptions
{
    public class BookingAlreadyExistException : ConfilctException
    {
        public BookingAlreadyExistException(string code = default) : base("Booking already exist!", code)
        {
        }
    }
}
