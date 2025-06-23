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
                });

            if (!string.IsNullOrEmpty(jwtOptions.Audience))
            {
                services.AddAuthorization(options =>
                {
                    // Set the default policy for all [Authorize] attributes.
                    // This is a critical security enhancement.
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim("scope", jwtOptions.Audience)
                        .Build();

                    // You can keep the named policy as well for specific use cases.
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
