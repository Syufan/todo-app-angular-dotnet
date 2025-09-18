using Server.Setup;

var builder = WebApplication.CreateBuilder(args);

// Serilog for structured logging
builder.AddSerilogLogging();

builder.Services
    .AddApiBasics(builder.Configuration)   // Add controllers, CORS, Swagger, ProblemDetails, HttpLogging
    .AddTodoInfrastructure();               // Register repository layer

var app = builder.Build();

// Global error handler, logging, CORS, Swagger, routes
app.UseAppPipeline();

app.Run();

public partial class Program { }
