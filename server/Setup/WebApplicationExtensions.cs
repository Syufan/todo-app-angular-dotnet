using Server.Middleware;

namespace Server.Setup;

public static class WebApplicationExtensions
{
    public static WebApplication UseAppPipeline(this WebApplication app)
    {
        app.UseExceptionHandler();

        app.UseMiddleware<CorrelationIdMiddleware>();

        app.UseHttpLogging();

        app.UseCors("Client");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();

        return app;
    }
}
