using Serilog;
using Server.Middleware;

namespace Server.Setup;

public static class WebApplicationExtensions
{
    public static WebApplication UseAppPipeline(this WebApplication app)
    {
        // Global exception handler
        app.UseExceptionHandler();

        // Injects a Correlation ID
        app.UseMiddleware<CorrelationIdMiddleware>();

        // Enable Serilog
        app.UseSerilogRequestLogging(opts =>
        {
            opts.EnrichDiagnosticContext = (diag, http) =>
            {
                var cid = http.Response.Headers["X-Correlation-Id"].ToString();
                diag.Set("CorrelationId", cid);
            };
        });

        // Enable CORS policy named "Client"
        app.UseCors("Client");

        // Only enable Swagger documentation in development environment
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Register attribute-routed controllers
        app.MapControllers();

        return app;
    }
}
