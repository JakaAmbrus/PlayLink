using System.Net;
using System.Text.Json;
using Shield.Api.Common.Exceptions;
using ApplicationException = Shield.Api.Common.Exceptions.ApplicationException;

namespace Shield.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
        catch (ValidationException ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex.StatusCode, ex.Message, ex.ValidationErrors);
        }
        catch (ApplicationException ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex.StatusCode, ex.Message, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error has occurred.");
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error has occurred.", null);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message, IReadOnlyDictionary<string, string[]>? errors)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            StatusCode = (int)statusCode,
            Message = message,
            Errors = errors
        };

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}