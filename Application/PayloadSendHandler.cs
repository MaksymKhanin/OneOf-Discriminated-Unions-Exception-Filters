using Api.Errors.Models;
using Application.Exceptions;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Application
{
    public record PayloadSendCommand(Guid TicketId)
        : IRequest<OneOf<Success, PayloadSourceNotFoundError, PayloadSourceDownloadError>>;

    internal class PayloadSendHandler : IRequestHandler<
            PayloadSendCommand, OneOf<Success, PayloadSourceNotFoundError, PayloadSourceDownloadError>>
    {
        public Task<OneOf<Success, PayloadSourceNotFoundError, PayloadSourceDownloadError>> Handle(PayloadSendCommand request, CancellationToken cancellationToken)
        {
            if (request.TicketId.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                throw new PayloadSourceNotFoundException(request.TicketId.ToString());
            }
            else if (request.TicketId.ToString() == "00000000-0000-0000-0000-000000000001")
            {
                throw new PayloadSourceDownloadException("Payload send error", request.TicketId.ToString());
            }


            return default;
        }
    }

    internal class PayloadSendErrorHandler : 
        IRequestExceptionHandler<PayloadSendCommand, OneOf<Success, PayloadSourceNotFoundError, PayloadSourceDownloadError>, PayloadSourceNotFoundException>,
        IRequestExceptionHandler<PayloadSendCommand, OneOf<Success, PayloadSourceNotFoundError, PayloadSourceDownloadError>, PayloadSourceDownloadException>
    {
        private readonly ILogger<PayloadSendErrorHandler> _logger;

        public PayloadSendErrorHandler(ILogger<PayloadSendErrorHandler> logger) => _logger = logger;

        public Task Handle(PayloadSendCommand request, PayloadSourceNotFoundException exception, RequestExceptionHandlerState<OneOf<Success, PayloadSourceNotFoundError, PayloadSourceDownloadError>> state, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "{Command} with {Param} {Value} could not be processed",
                 request.GetType().Name,
                 nameof(request.TicketId),
                 request.TicketId);
            state.SetHandled(new PayloadSourceNotFoundError(request.TicketId, exception));
            return Task.CompletedTask;
        }

        public Task Handle(PayloadSendCommand request, PayloadSourceDownloadException exception, RequestExceptionHandlerState<OneOf<Success, PayloadSourceNotFoundError, PayloadSourceDownloadError>> state, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "{Command} with {Param} {Value} could not be processed",
                request.GetType().Name,
                nameof(request.TicketId),
                request.TicketId);
            state.SetHandled(new PayloadSourceDownloadError(exception));
            return Task.CompletedTask;
        }
    }
}
