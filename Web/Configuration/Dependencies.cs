using Api.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Api.Configuration
{
    public static class Dependencies
    {
        internal static MvcOptions AddErrorFilters(this MvcOptions options)
        {
            options.Filters.Add<PayloadSourceExceptionFilterAttribute>(2);
            options.Filters.Add<PayloadExceptionFilterAttribute>(1);
            options.Filters.Add<SystemExceptionFilterAttribute>(0);

            return options;
        }
    }
}
       
