using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Api.Middleware;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        var (status, title, detail) = exception switch
        {
            ValidationException ve          => (400, "Validation Error", string.Join("; ", ve.Errors.Select(e => e.ErrorMessage))),
            DomainException de              => (400, "Domain Error", de.Message),
            NotFoundException nfe           => (404, "Not Found", nfe.Message),
            UnauthorizedAccessException uae => (401, "Unauthorized", uae.Message),
            _                               => (500, "Server Error", "An unexpected error occurred.")
        };

        if (status == 500)
            _logger.LogError(exception, "Unhandled exception");

        var problem = new ProblemDetails { Status = status, Title = title, Detail = detail };
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(problem, ct);
        return true;
    }
}
