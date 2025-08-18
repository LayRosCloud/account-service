namespace AccountService.Utils.Middleware;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggerMiddleware> _logger;

    public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // ReSharper disable once UnusedMember.Global using ASP.NET middleware before request
    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = (string)context.Items["X-Correlation-ID"]!;
        var causationId = (string)context.Items["X-Causation-ID"]!;
        _logger.LogInformation("requestId: {requestId}; causationId: {causationId}; action: {method} {path}", requestId, causationId, context.Request.Method, context.Request.Path);
        await _next(context);
        _logger.LogInformation("requestId: {requestId}; causationId: {causationId};", requestId, causationId);
    }
}