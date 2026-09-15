using EcommerceAPI.Enums;
using EcommerceAPI.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Middleware;

public class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            EcommerceException ex => ex.Error.Map(),
            DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, "Concurrency conflict"),
            DbUpdateException => (StatusCodes.Status500InternalServerError, "Database error"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        if(statusCode >= 500) 
        {
            logger.LogError(exception, "Unhandled exception ocurred");
        }
        else 
        {
            logger.LogWarning(exception, "Handled exception occurred: {Message}", exception.Message);
        }
        
        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext 
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails 
            {
                Status = statusCode,
                Type = exception.GetType().Name,
                Title = title,
                Detail = statusCode >= 500
                ? "An internal error occurred. Please try again late":
                exception.Message
            }
        });
    }
}
