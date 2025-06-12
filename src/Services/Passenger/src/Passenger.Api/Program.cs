using BuldingBlock.Domain;
using BuldingBlock.EFCore;
using BuldingBlock.Exception;
using BuldingBlock.IdsGenerator;
using BuldingBlock.Jwt;
using BuldingBlock.Logging;
using BuldingBlock.Mapster;
using BuldingBlock.MassTransit;
using BuldingBlock.OpenTelemetry;
using BuldingBlock.Swagger;
using BuldingBlock.Utils;
using BuldingBlock.Web;
using Figgle;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Passenger;
using Passenger.Data;
using Passenger.Extensions;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var env = builder.Environment;

var appOptions = builder.Services.GetOptions<AppOptions>("AppOptions");
Console.WriteLine(appOptions.Name);

builder.Services.AddTransient<IBusPublisher, BusPublisher>();
builder.Services.AddCustomDbContext<PassengerDbContext>(configuration, typeof(PassengerRoot).Assembly);
builder.AddCustomSerilog();
builder.Services.AddJwt();
builder.Services.AddControllers();
builder.Services.AddCustomSwagger(builder.Configuration, typeof(PassengerRoot).Assembly);
builder.Services.AddCustomVersioning();
builder.Services.AddCustomMediatR();
builder.Services.AddValidatorsFromAssembly(typeof(PassengerRoot).Assembly);
builder.Services.AddCustomProblemDetails();
builder.Services.AddCustomMapster(typeof(PassengerRoot).Assembly);
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<IEventMapper, EventMapper>();
builder.Services.AddTransient<IBusPublisher, BusPublisher>();

builder.Services.AddCustomMassTransit(typeof(PassengerRoot).Assembly, env);
builder.Services.AddCustomOpenTelemetry();
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GrpcExceptionInterceptor>();
});
builder.Services.AddMagicOnion();

SnowFlakIdGenerator.Configure(2);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
    app.UseCustomSwagger(provider);
}

app.UseSerilogRequestLogging();
app.UseMigrations(env);
app.UseCorrelationId();
app.UseRouting();
app.UseHttpMetrics();
app.UseProblemDetails();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapMetrics();
    endpoints.MapMagicOnionService();
});

app.MapGet("/", x => x.Response.WriteAsync(appOptions.Name));

app.Run();
