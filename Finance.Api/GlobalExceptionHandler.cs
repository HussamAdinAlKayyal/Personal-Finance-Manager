using Finance.Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Security.Authentication;

namespace Finance.Api;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> logger = logger;
    
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        var (statusCode, title, details) = GetTitleAndStatusCodeAndDetials(exception);
        httpContext.Response.StatusCode = statusCode;
        await Results.Problem(
            statusCode: statusCode, 
            title: title, 
            detail: details)
        .ExecuteAsync(httpContext);
        return true;
    }

    private static (int StatusCode, string Title, string? Details) GetTitleAndStatusCodeAndDetials(Exception exception)
    {
        return exception switch
        {
            ArgumentException or ValidationException => (StatusCodes.Status400BadRequest, "Bad request", exception.Message),
            AuthenticationException => (StatusCodes.Status401Unauthorized, "Unauthenticated access", exception.Message),
            UnauthorizedAccessException => (StatusCodes.Status403Forbidden, "Unauthorized access", exception.Message),
            EntityNotFoundException => (StatusCodes.Status404NotFound, "Resource not found", exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error happened", null),
        };
    }
}
