using AccountService.Utils.Exceptions;
using AccountService.Utils.Result;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AccountService.Utils.Middleware;

public class ExceptionMiddleware
{
    private const string HeaderApplicationJson = "application/json";
    private const HttpStatusCode StatusCodeNotFound = HttpStatusCode.NotFound;
    private const HttpStatusCode StatusCodeBadRequest = HttpStatusCode.BadRequest;
    private const HttpStatusCode StatusCodeInternalServerError = HttpStatusCode.InternalServerError;
    private const HttpStatusCode StatusCodeConflict = HttpStatusCode.Conflict;

    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    // ReSharper disable once UnusedMember.Global using ASP.NET middleware before request
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await HandleException(context, ex);
        }
        catch (ValidationException ex)
        {
            await HandleException(context, ex);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            await HandleException(context, ex);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext context, NotFoundException ex)
    {
        await Handle(context, ex, (int)StatusCodeNotFound);
    }

    private async Task HandleException(HttpContext context, ValidationException ex)
    {
        await Handle(context, ex, (int)StatusCodeBadRequest);
    }

    private async Task HandleException(HttpContext context, DbUpdateConcurrencyException ex)
    {
        await Handle(context, ex, (int)StatusCodeConflict);
    }

    private async Task HandleException(HttpContext context, Exception ex)
    {
        await Handle(context, ex, (int)StatusCodeInternalServerError);
    }

    private async Task Handle(HttpContext context, Exception exception, int code)
    {
        var logLevel = code < 500 ? LogLevel.Error : LogLevel.Critical;
        var requestId = (string)context.Items["X-Correlation-ID"]!;
        var causationId = (string)context.Items["X-Causation-ID"]!;

        _logger.Log(logLevel, exception, "requestId: {requestId}; causationId: {causationId}", requestId, causationId);
        
        context.Response.ContentType = HeaderApplicationJson;
        context.Response.StatusCode = code;
        await context.Response.WriteAsJsonAsync(new MbError(code, exception.Message));
    }
}