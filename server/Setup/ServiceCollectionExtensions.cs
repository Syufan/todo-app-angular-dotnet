using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Server.Application;
using Server.Infrastructure;

namespace Server.Setup;

public static class ServiceCollectionExtensions
{
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

        // Structured HTTP request logging
        services.AddHttpLogging(o =>
        {
            o.LoggingFields = HttpLoggingFields.RequestMethod
                            | HttpLoggingFields.RequestPath
                            | HttpLoggingFields.ResponseStatusCode
                            | HttpLoggingFields.Duration;
            o.RequestHeaders.Add("X-Correlation-Id");
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

    // Register the Correlation ID middleware
    public static IServiceCollection AddCorrelationId(this IServiceCollection services)
    {
        services.AddTransient<Middleware.CorrelationIdMiddleware>();
        return services;
    }
}
