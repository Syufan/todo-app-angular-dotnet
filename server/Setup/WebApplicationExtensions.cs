using Server.Middleware;

namespace Server.Setup;

public static class WebApplicationExtensions
{
    public static WebApplication UseAppPipeline(this WebApplication app)
    {
        // Global exception handler
        app.UseExceptionHandler();

        // Structured Logging
        app.UseMiddleware<CorrelationIdMiddleware>();

        // Built in structured logging
        app.UseHttpLogging();

        // Enable CORS policy named "Client" (typically for frontend access)
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
