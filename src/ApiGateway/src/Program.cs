using BuldingBlock.Jwt;
using BuldingBlock.Logging;
using BuldingBlock.Utils;
using BuldingBlock.Web;
using Figgle;
using Microsoft.AspNetCore.Authentication;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var appOptions = builder.Services.GetOptions<AppOptions>("AppOptions");
Console.WriteLine(appOptions.Name);

builder.AddCustomSerilog();
builder.Services.AddJwt();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("Yarp"));


var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseCorrelationId();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapReverseProxy(proxyPipeline =>
    {
        proxyPipeline.Use(async (context, next) =>
        {
            var token = await context.GetTokenAsync("access_token");
            context.Request.Headers["Authorization"] = $"Bearer {token}";

            await next().ConfigureAwait(false);
        });
    });
});

app.MapGet("/", x => x.Response.WriteAsync(appOptions.Name));

app.Run();
