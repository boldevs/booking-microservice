using BuldingBlock.Logging;
using BuldingBlock.Utils;
using BuldingBlock.Web;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var appOptions = builder.Services.GetOptions<AppOptions>("AppOptions");
Console.WriteLine(appOptions.Name);

builder.AddCustomSerilog();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// The gateway's only job is to be a reverse proxy.
// It doesn't need to know about JWTs; it just passes them along.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("Yarp"));

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseCorrelationId();
app.UseRouting();
app.UseHttpsRedirection();

// We remove Authentication and Authorization from the gateway itself.
// The downstream services (Flight, Booking, etc.) are responsible for it.


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapReverseProxy();
});

app.MapGet("/", x => x.Response.WriteAsync(appOptions.Name));

app.Run();
