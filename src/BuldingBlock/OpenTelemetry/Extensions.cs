using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration; // 👈 Required for IConfiguration
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using BuldingBlock.Utils;
using BuldingBlock.Web;


namespace BuldingBlock.OpenTelemetry
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomOpenTelemetry(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var appName = serviceProvider
                .GetRequiredService<IConfiguration>()
                .GetSection("AppOptions")["Name"] ?? "BookingService";

            services.AddOpenTelemetry()
                .WithTracing(builder => builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(appName))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddMassTransitInstrumentation()
                    .AddJaegerExporter());

            return services;
        }
    }
}
