using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Errors
{
    internal sealed class PayloadSourceExceptionFilterAttribute : BasePayloadExceptionFilterAttribute
    {
        public PayloadSourceExceptionFilterAttribute(ILogger<PayloadSourceExceptionFilterAttribute> logger) : base(logger)
        {
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is PayloadSourceNotFoundException)
                SetResponse(context, val => new NotFoundObjectResult(val));

            if (context.Exception is PayloadSourceDownloadException)
                SetResponse(context, val => new BadRequestObjectResult(val));
        }
    }
}
