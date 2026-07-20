using Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        // Customize ProblemDetails based on the specific exception type
        var problemDetails = exception switch
        {
            ValidationException validationEx => CreateValidationProblemDetails(validationEx, httpContext),
            UnauthorizedAccessException unauthorizedEx => CreateUnauthorizedProblemDetails(unauthorizedEx, httpContext),
            BusinessException bussinessEx => CreateBusinessProblemDetails(bussinessEx, httpContext),
            ExternalServiceException externalEx => CreateExternalServiceProblemDetails(externalEx, httpContext),
            DbUpdateConcurrencyException concurrencyEx => CreateConcurrencyProblemDetails(concurrencyEx, httpContext),
            _ => CreateInternalServerErrorProblemDetails(exception, httpContext)
        };

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static ProblemDetails CreateExternalServiceProblemDetails(ExternalServiceException ex, HttpContext context)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status503ServiceUnavailable,
            Title = "External Service Error",
            Detail = ex.Message,
            Instance = context.Request.Path
        };
    }

    private static ProblemDetails CreateBusinessProblemDetails(BusinessException ex, HttpContext context)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Business Rule Violation",
            Detail = ex.Message,
            Instance = context.Request.Path
        };
    }

    private static ProblemDetails CreateValidationProblemDetails(ValidationException ex, HttpContext context)
    {
        var errors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        return new HttpValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "Validation Failed",
            Detail = "One or more validation errors occurred.",
            Instance = context.Request.Path
        };
    }

    private static ProblemDetails CreateUnauthorizedProblemDetails(UnauthorizedAccessException ex, HttpContext context)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Detail = ex.Message,
            Instance = context.Request.Path
        };
    }

    private static ProblemDetails CreateInternalServerErrorProblemDetails(Exception ex, HttpContext context)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Detail = "An unexpected error occurred on the server.",
            Instance = context.Request.Path
        };
    }

    private static ProblemDetails CreateConcurrencyProblemDetails(DbUpdateConcurrencyException ex, HttpContext context)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Concurrency Conflict",
            Detail = "The resource was modified by another request.",
            Instance = context.Request.Path
        };
    }
}
