using System.Threading;
using System.Threading.Tasks;
using BuldingBlock.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Flight.Seats.Features.ReserveSeat;

[Route("/flight/reserve-seat")]
public class ReserveSeatEndpoint : BaseController
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Reserve seat", Description = "Reserve seat")]
    public async Task<ActionResult> ReserveSeat([FromBody] ReserveSeatCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}
