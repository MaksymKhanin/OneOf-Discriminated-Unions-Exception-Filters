using Api.Errors.Models;
using Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayloadController : ControllerBase
    {
        #region Private Members

        private readonly ILogger<PayloadController> _logger;
        private readonly IMediator _mediator;

        #endregion

        public PayloadController(IMediator mediator, ILogger<PayloadController> logger) =>
           (_logger, _mediator) = (logger, mediator);


        [HttpPost("{ticketId:guid}/send")]
        public async Task<IActionResult> Send(Guid ticketId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Upload acknowledged {TicketId}", ticketId);

            var result = await _mediator.Send(new PayloadSendCommand(ticketId), cancellationToken);

            return result.Match<IActionResult>(
                _ => Accepted(),
                notFoundError => BadRequest(ErrorModel.FromPayloadError(notFoundError)),
                downloadError => BadRequest(ErrorModel.FromPayloadError(downloadError))
            );
        }
    }
}
