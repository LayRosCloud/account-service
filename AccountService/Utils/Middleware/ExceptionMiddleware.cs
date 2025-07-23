using AccountService.Utils.Exceptions;
using FluentValidation;
using System.Net;

namespace AccountService.Utils.Middleware;

public class ExceptionMiddleware
{
    private const string HeaderApplicationJson = "application/json";
    private const HttpStatusCode StatusCodeNotFound = HttpStatusCode.NotFound;
    private const HttpStatusCode StatusCodeBadRequest = HttpStatusCode.BadRequest;
    private const HttpStatusCode StatusCodeInternalServerError = HttpStatusCode.InternalServerError;

    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

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
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private static async Task HandleException(HttpContext context, NotFoundException ex)
    {
        context.Response.ContentType = HeaderApplicationJson;
        context.Response.StatusCode = (int)StatusCodeNotFound;
        await context.Response.WriteAsync(ex.Message);
    }

    private static async Task HandleException(HttpContext context, ValidationException ex)
    {
        context.Response.ContentType = HeaderApplicationJson;
        context.Response.StatusCode = (int)StatusCodeBadRequest;
        await context.Response.WriteAsync(ex.Message);
    }

    private static async Task HandleException(HttpContext context, Exception ex)
    {
        context.Response.ContentType = HeaderApplicationJson;
        context.Response.StatusCode = (int)StatusCodeInternalServerError;
        await context.Response.WriteAsync(ex.Message);
    }

}