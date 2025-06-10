using BuldingBlock.Exception;

namespace Booking.Booking.Exceptions
{
    public class FlightNotFoundException : NotFoundException
    {
        public FlightNotFoundException() : base("Flight not found!")
        {
        }
    }
}
