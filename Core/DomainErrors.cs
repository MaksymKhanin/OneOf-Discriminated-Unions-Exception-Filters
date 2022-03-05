namespace Api.Errors.Models
{
    public record PayloadError(ErrorCode Code, string Message, Exception? Exception = null);

    public record PayloadNotValidatedError(Guid TicketId, Exception? Exception = null) : PayloadError(ErrorCode.PayloadNotValidated, $"payload with ticket Id '{TicketId}' has not been validated yet", Exception);

    public record PayloadValidationFailedError(Guid TicketId, Exception? Exception = null) : PayloadError(ErrorCode.PayloadValidationFailed, $"The validation for given ticket Id '{TicketId}' failed", Exception);

    public record PayloadSourceNotFoundError(Guid TicketId, Exception? Exception = null) : PayloadError(ErrorCode.PayloadSourceNotFound, $"No file is associated with ticket id '{TicketId}'.", Exception);

    public record PayloadSourceDownloadError(Exception? Exception = null) : PayloadError(ErrorCode.PayloadSourceDownloadError, "The given Payload source could not be downloaded", Exception);

    public record PayloadDataExtractionError(Exception? Exception = null) : PayloadError(ErrorCode.PayloadDataExtractionError, "The extraction of the data from the given Payload resulted in an error", Exception);

    public record PayloadAlreadySentError(Guid TicketId, Exception? Exception = null)
        : PayloadError(ErrorCode.PayloadAlreadySent, $"This payload has already been sent. TicketId: {TicketId}", Exception);

    public record PayloadOperationNotInitializedError(Guid TicketId, Exception? Exception = null) : PayloadError(ErrorCode.PayloadOperationNotInitialized, $"The payload operation with Id '{TicketId}' has not been initialized", Exception);
    public record PayloadOperationAlreadyAcknowledgedError(Guid TicketId, Exception? Exception = null) : PayloadError(ErrorCode.PayloadOperationAlreadyAcknowledged, $"The payload operation with Id '{TicketId}' has already been acknowledged", Exception);


    public enum ErrorCode
    {
        UnhandledSystemError = 1,
        UnhandledPayloadError = 2,
        PayloadNotValidated = 1000,
        PayloadSourceNotFound = 1001,
        PayloadDataExtractionError = 1002,
        PayloadValidationFailed = 1003,
        PayloadAlreadySent = 1004,
        PayloadOperationNotInitialized = 1005,
        PayloadOperationAlreadyAcknowledged = 1006,
        PayloadSourceDownloadError = 1007
    }
}
