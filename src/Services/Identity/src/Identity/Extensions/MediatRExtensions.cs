using BuldingBlock.EFCore;
using BuldingBlock.Logging;
using BuldingBlock.Validation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Extensions;

public static class MediatRExtensions
{
    public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IdentityRoot>());
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(EfIdentityTxBehavior<,>));

        return services;
    }

}
