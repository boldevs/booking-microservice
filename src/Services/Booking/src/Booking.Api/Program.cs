using Booking;
using Booking.Configuration;
using Booking.Data;
using Booking.Extensions;
using BuldingBlock.Domain;
using BuldingBlock.EFCore;
using BuldingBlock.EventStoreDB;
using BuldingBlock.IdsGenerator;
using BuldingBlock.Jwt;
using BuldingBlock.Logging;
using BuldingBlock.Mapster;
using BuldingBlock.MassTransit;
using BuldingBlock.OpenTelemetry;
using BuldingBlock.Swagger;
using BuldingBlock.Utils;
using BuldingBlock.Web;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var env = builder.Environment;

var appOptions = builder.Services.GetOptions<AppOptions>("AppOptions");
builder.Services.Configure<GrpcOptions>(options => configuration.GetSection("Grpc").Bind(options));

Console.WriteLine(appOptions.Name);

builder.Services.AddTransient<IBusPublisher, BusPublisher>();
builder.Services.AddCustomDbContext<BookingDbContext>(configuration, typeof(BookingRoot).Assembly);

builder.AddCustomSerilog();
builder.Services.AddJwt();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCustomSwagger(builder.Configuration, typeof(BookingRoot).Assembly);
builder.Services.AddCustomVersioning();
builder.Services.AddCustomMediatR();
builder.Services.AddValidatorsFromAssembly(typeof(BookingRoot).Assembly);
builder.Services.AddCustomProblemDetails();
builder.Services.AddCustomMapster(typeof(BookingRoot).Assembly);
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<IEventMapper, EventMapper>();
builder.Services.AddTransient<IBusPublisher, BusPublisher>();

builder.Services.AddCustomMassTransit(typeof(BookingRoot).Assembly, env);
builder.Services.AddCustomOpenTelemetry();
builder.Services.AddTransient<AuthHeaderHandler>();
SnowFlakIdGenerator.Configure(3);

// EventStoreDB Configuration
builder.Services.AddEventStore(configuration, typeof(BookingRoot).Assembly)
    .AddEventStoreDBSubscriptionToAll();

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
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseProblemDetails();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapMetrics();
});

app.MapGet("/", x => x.Response.WriteAsync(appOptions.Name));

app.Run();
