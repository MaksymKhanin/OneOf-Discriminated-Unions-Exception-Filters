using Api.Errors.Models;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Errors
{
    internal abstract class BasePayloadExceptionFilterAttribute : BaseExceptionFilterAttribute
    {
        protected BasePayloadExceptionFilterAttribute(ILogger logger) : base(logger) { }

        internal virtual void SetResponse(ExceptionContext context, Func<ErrorModel, ObjectResult> resultCreator)
        {
            context.Result = resultCreator(new ErrorModel(ErrorCode.UnhandledPayloadError, context.Exception.Message));

            context.ExceptionHandled = true;

            Log(context);
        }
    }
}