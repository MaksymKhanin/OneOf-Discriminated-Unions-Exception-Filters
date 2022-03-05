using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection AddPayloadApplicationMediatR(this IServiceCollection services)
            => services.AddMediatR(Assembly.GetExecutingAssembly());
    }
}
