using System.Net.Http;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Middleware.Common
{
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
            catch (ApiException ex)
            {
                _logger.LogWarning(ex, "API Exception: {Message}", ex.Message);
                await HandleApiExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                await HandleGenericExceptionAsync(context, ex);
            }
        }

        private static Task HandleApiExceptionAsync(HttpContext context, ApiException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.StatusCode;

            var response = ApiResponse.ErrorResponse(
                exception.Message,
                exception.StatusCode
            );

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static Task HandleGenericExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = ApiResponse.ErrorResponse(
                "An internal server error occurred",
                (int)HttpStatusCode.InternalServerError
            );

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
