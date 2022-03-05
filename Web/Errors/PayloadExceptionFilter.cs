using Application.Exceptions;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Errors
{
    internal sealed class PayloadExceptionFilterAttribute : BasePayloadExceptionFilterAttribute
    {
        public PayloadExceptionFilterAttribute(ILogger<PayloadExceptionFilterAttribute> logger) : base(logger) { }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is not PayloadException)
            {
                return;
            }

            SetResponse(context, errorModel => new BadRequestObjectResult(errorModel));
        }
    }
}