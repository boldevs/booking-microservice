using System;
using System.Collections.Generic;
using BuldingBlock.Caching;
using Flight.Flights.Dtos;
using MediatR;

namespace Flight.Flights.Features.GetAvailableFlights;

public record GetAvailableFlightsQuery : IRequest<IEnumerable<FlightResponseDto>>, ICacheRequest
{
    public string CacheKey => "GetAvailableFlightsQuery";
    public TimeSpan? AbsoluteExpirationRelativeToNow => TimeSpan.FromHours(1);
}
