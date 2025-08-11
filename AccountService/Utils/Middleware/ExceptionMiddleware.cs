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

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
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

    private static async Task HandleException(HttpContext context, NotFoundException ex)
    {
        await Handle(context, ex.Message, (int)StatusCodeNotFound);
    }

    private static async Task HandleException(HttpContext context, ValidationException ex)
    {
        await Handle(context, ex.Message, (int)StatusCodeBadRequest);
    }

    private static async Task HandleException(HttpContext context, DbUpdateConcurrencyException ex)
    {
        await Handle(context, ex.Message, (int)StatusCodeConflict);
    }

    private static async Task HandleException(HttpContext context, Exception ex)
    {
        await Handle(context, ex.Message, (int)StatusCodeInternalServerError);
    }

    private static async Task Handle(HttpContext context, string message, int code)
    {
        context.Response.ContentType = HeaderApplicationJson;
        context.Response.StatusCode = code;
        await context.Response.WriteAsJsonAsync(new MbError(code, message));
    }
}