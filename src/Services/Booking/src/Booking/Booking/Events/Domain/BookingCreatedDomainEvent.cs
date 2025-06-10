namespace Booking.Booking.Events.Domain
{
    public record BookingCreatedDomainEvent(long Id, PassengerInfo PassengerInfo, Trip Trip, bool IsDeleted) : IDomainEvent;

}