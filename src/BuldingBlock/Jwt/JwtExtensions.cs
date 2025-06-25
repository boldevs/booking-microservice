using BuldingBlock.Utils;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace BuldingBlock.Jwt
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwt(this IServiceCollection services)
        {
            var jwtOptions = services.GetOptions<JwtBearerOptions>("Jwt");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = jwtOptions.Authority;
                    options.TokenValidationParameters.ValidateAudience = false;

                    // This is the critical fix.
                    // When running in a Docker container, the ASPNETCORE_ENVIRONMENT is 'docker'.
                    // We need to tell the JWT handler to trust the self-signed certificates
                    // used by other containers (like the Identity service).
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "docker")
                    {
                        options.BackchannelHttpHandler = new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        };
                    }
                });

            if (!string.IsNullOrEmpty(jwtOptions.Audience))
            {
                services.AddAuthorization(options =>
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim("scope", jwtOptions.Audience)
                        .Build();

                    options.AddPolicy(nameof(ApiScope), policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("scope", jwtOptions.Audience);
                    });
                });
            }

            return services;
        }
    }
}
