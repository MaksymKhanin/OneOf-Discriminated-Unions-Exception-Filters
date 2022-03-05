using Api.Errors.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Api.Errors
{
    public abstract class BaseExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        protected BaseExceptionFilterAttribute(ILogger logger) => _logger = logger;

        internal virtual void SetResponse(ExceptionContext context, HttpStatusCode code, string message)
        {
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;
            context.Result = new ObjectResult(new ErrorModel(ErrorCode.UnhandledSystemError, message));

            context.ExceptionHandled = true;
            
            Log(context);
        }

        protected virtual void Log(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Exception occured during request {Method} {Route}?{Query}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path, 
                context.HttpContext.Request.QueryString);
        }
    }
}