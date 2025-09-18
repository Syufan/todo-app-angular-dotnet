namespace Server.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        var cid = ctx.Request.Headers["X-Correlation-Id"].ToString();
        if (string.IsNullOrWhiteSpace(cid))
            cid = System.Diagnostics.Activity.Current?.Id ?? ctx.TraceIdentifier;

        ctx.Response.Headers["X-Correlation-Id"] = cid;

        using (Serilog.Context.LogContext.PushProperty("CorrelationId", cid))
            await _next(ctx);
    }
}
