namespace Api.Errors.Models
{
    internal record ErrorModel(ErrorCode ErrorCode, string ErrorMessage)
    {
        internal static ErrorModel FromPayloadError(PayloadError error) => new(error.Code, error.Message);
    }
}
