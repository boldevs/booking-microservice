using BuldingBlock.Domain.Event;

namespace Passenger.Passengers.Events.Domain;

public record PassengerCreatedDomainEvent(string FlightNumber) : IDomainEvent;
