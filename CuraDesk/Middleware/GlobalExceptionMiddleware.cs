using CuraDesk.Business.Exceptions;
using CuraDesk.Exceptions;

namespace CuraDesk.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate requestDelegate, ILogger<GlobalExceptionMiddleware> logger)
        {
           _next=requestDelegate;
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
                _logger.LogError(
                    ex,
                    "An unhandled exception occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }
        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            int statusCode;
            string message;
            switch(ex)
            {
                case AlreadyExistsException:
                    statusCode = StatusCodes.Status409Conflict;
                    message= ex.Message;
                    break;

                case NotFoundException:
                    statusCode=StatusCodes.Status404NotFound;
                    message= ex.Message;
                    break;

                case PasswordMisMatchException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    message = ex.Message;
                    break;

                case TimeMissMatchException:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = ex.Message;
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "An unexpected error occurred.";
                    break;
            }
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(new
            {
                statusCode,
                message
            });
        }
    }
}
