using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using Server.Application;
using Server.Infrastructure;

namespace Server.Setup;

public static class ServiceCollectionExtensions
{
    // Add Serilog
    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, sp, cfg) => cfg
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day)
        );
        return builder;
    }

    // Registers core API-level services and configurations
    public static IServiceCollection AddApiBasics(
        this IServiceCollection services,
        IConfiguration config)
    {
        // Controllers
        services.AddControllers()
            .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = false);

        // RFC7807 ProblemDetails
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = ctx =>
            {
                ctx.ProblemDetails.Extensions["traceId"] = ctx.HttpContext.TraceIdentifier;
                if (ctx.HttpContext.Request.Headers.TryGetValue("X-Correlation-Id", out var cid) &&
                    !string.IsNullOrWhiteSpace(cid))
                {
                    ctx.ProblemDetails.Extensions["correlationId"] = cid.ToString();
                }
            };
        });

        // CORS
        var allowedOrigins = config.GetSection("Cors:AllowedOrigins").Get<string[]>()
                             ?? new[] { "http://localhost:4200" };
        services.AddCors(options =>
        {
            options.AddPolicy("Client", p =>
                p.WithOrigins(allowedOrigins)
                 .AllowAnyHeader()
                 .AllowAnyMethod());
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    // Registers the infrastructure dependencies
    public static IServiceCollection AddTodoInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
        return services;
    }
}
