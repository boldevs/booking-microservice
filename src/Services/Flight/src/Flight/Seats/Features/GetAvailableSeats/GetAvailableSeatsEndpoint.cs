using System.Threading;
using System.Threading.Tasks;
using BuldingBlock.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Flight.Seats.Features.GetAvailableSeats;

[Route("/flight/get-available-seats")]
public class GetAvailableSeatsEndpoint : BaseController
{
    [Authorize]
    [HttpGet("{flightId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Get available seats", Description = "Get available seats")]
    public async Task<ActionResult> GetAvailableSeats([FromRoute] GetAvailableSeatsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}
