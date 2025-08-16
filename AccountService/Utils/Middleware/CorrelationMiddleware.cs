namespace AccountService.Utils.Middleware;

public class CorrelationMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // ReSharper disable once UnusedMember.Global using ASP.NET middleware before request
    public async Task InvokeAsync(HttpContext context)
    {
        var correlation = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
            ?? Guid.CreateVersion7().ToString();
        var causation = context.Request.Headers["X-Causation-ID"].FirstOrDefault()
                        ?? "";

        context.Items["X-Correlation-ID"] = correlation;
        context.Items["X-Causation-ID"] = causation;
        await _next(context);
        context.Response.Headers["X-Correlation-ID"] = correlation;
    }
}