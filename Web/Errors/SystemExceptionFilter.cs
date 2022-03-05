using Core;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Api.Errors
{
    public sealed class SystemExceptionFilterAttribute : BaseExceptionFilterAttribute
    {
        public SystemExceptionFilterAttribute(ILogger<SystemExceptionFilterAttribute> logger) : base(logger)
        {
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is PayloadException)
            {
                return;
            }

            if (context.Exception is NotImplementedException)
            {
                SetResponse(context, HttpStatusCode.NotImplemented, "This operation is not yet implemented.");
            }
            else
            {
                SetResponse(context, HttpStatusCode.InternalServerError, "Sorry, something went wrong. Please, try again later.");
            }
        }
    }
}