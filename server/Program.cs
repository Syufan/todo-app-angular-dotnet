using Serilog;
using Server.Setup;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for structured logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Replace default logger with Serilog
builder.Host.UseSerilog();

builder.Services
    .AddApiBasics(builder.Configuration)   // Add controllers, CORS, Swagger, ProblemDetails, HttpLogging
    .AddTodoInfrastructure()               // Register repository layer
    .AddCorrelationId();                   // Middleware to inject correlation ID

var app = builder.Build();

// Global error handler, logging, CORS, Swagger, routes
app.UseAppPipeline();

app.Run();

public partial class Program { }
