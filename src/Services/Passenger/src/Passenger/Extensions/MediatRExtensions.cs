using BuldingBlock.EFCore;
using BuldingBlock.Logging;
using BuldingBlock.Validation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Passenger.Extensions;

public static class MediatRExtensions
{
    public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
    {
        services.AddMediatR(typeof(PassengerRoot).Assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(EfTxBehavior<,>));

        return services;
    }
}
