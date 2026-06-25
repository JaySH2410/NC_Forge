using System.Text.Json;
using test.Shared.Exceptions;
using test.Shared.Responses;

namespace test.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
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
        catch (Exception exception)
        {
            await HandleExceptionAsync(
                context,
                exception);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var traceId = context.TraceIdentifier;

        context.Response.ContentType = "application/json";
        context.Response.Headers["X-Trace-Id"] = traceId;

        ApiResponse response;

        switch (exception)
        {
            case ValidationException validationException:

                _logger.LogWarning(
                    validationException,
                    validationException.Message);

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                response = new ApiResponse
                {
                    IsSuccess = false,
                    Message = validationException.Message,
                    Errors = validationException.Errors.ToDictionary(
                        x => x.Key,
                        x => x.Value),
                    TraceId = traceId,
                    Timestamp = DateTimeOffset.UtcNow
                };

                break;

            case UnauthorizedException unauthorizedException:

                _logger.LogWarning(
                    unauthorizedException,
                    unauthorizedException.Message);

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                response = new ApiResponse
                {
                    IsSuccess = false,
                    Message = unauthorizedException.Message,
                    TraceId = traceId,
                    Timestamp = DateTimeOffset.UtcNow
                };

                break;

            case NotFoundException notFoundException:

                _logger.LogWarning(
                    notFoundException,
                    notFoundException.Message);

                context.Response.StatusCode = StatusCodes.Status404NotFound;

                response = new ApiResponse
                {
                    IsSuccess = false,
                    Message = notFoundException.Message,
                    TraceId = traceId,
                    Timestamp = DateTimeOffset.UtcNow
                };

                break;

            case BusinessException businessException:

                _logger.LogWarning(
                    businessException,
                    businessException.Message);

                context.Response.StatusCode = StatusCodes.Status409Conflict;

                response = new ApiResponse
                {
                    IsSuccess = false,
                    Message = businessException.Message,
                    TraceId = traceId,
                    Timestamp = DateTimeOffset.UtcNow
                };

                break;

            default:

                _logger.LogError(
                    exception,
                    exception.Message);

                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                response = new ApiResponse
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred",
                    TraceId = traceId,
                    Timestamp = DateTimeOffset.UtcNow
                };

                break;
        }

        await context.Response.WriteAsJsonAsync(response);
    }
}