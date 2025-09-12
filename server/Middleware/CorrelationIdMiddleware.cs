using System.Diagnostics;

namespace Server.Middleware;

public sealed class CorrelationIdMiddleware : IMiddleware
{
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    // Inject the logger
    public CorrelationIdMiddleware(ILogger<CorrelationIdMiddleware> logger)
    {
        _logger = logger;
    }

    // Main middleware logic
    public async Task InvokeAsync(HttpContext ctx, RequestDelegate next)
    {
        var correlationId =
            (ctx.Request.Headers.TryGetValue("X-Correlation-Id", out var cid) && !string.IsNullOrWhiteSpace(cid))
                ? cid.ToString()
                : (Activity.Current?.Id ?? ctx.TraceIdentifier);

        ctx.Response.Headers["X-Correlation-Id"] = correlationId;

        using (_logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
        {
            var sw = Stopwatch.StartNew();
            await next(ctx);
            sw.Stop();

            _logger.LogInformation("HTTP {Method} {Path} => {Status} in {ElapsedMs} ms",
                ctx.Request.Method, ctx.Request.Path, ctx.Response.StatusCode, sw.ElapsedMilliseconds);
        }
    }
}
