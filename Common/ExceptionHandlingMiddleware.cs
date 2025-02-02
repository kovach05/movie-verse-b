using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Common;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is UnauthorizedAccessException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return context.Response.WriteAsJsonAsync(new ErrorResponse
            {
                Message = "Unauthorized access."
            });
        }

        if (exception is ArgumentException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return context.Response.WriteAsJsonAsync(new ErrorResponse
            {
                Message = "Invalid request.",
                Details = exception.Message
            });
        }

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var environment = context.RequestServices.GetService<IWebHostEnvironment>();
        var errorResponse = new ErrorResponse
        {
            Message = environment?.IsDevelopment() == true 
                ? "Internal server error" 
                : "An unexpected error occurred. Please try again later.",
            Details = environment?.IsDevelopment() == true ? exception.Message : null,
            StackTrace = environment?.IsDevelopment() == true ? exception.StackTrace : null
        };

        return context.Response.WriteAsJsonAsync(errorResponse);
    }

}