using SimpleBlog.Application.Exceptions;
using SimpleBlog.Domain.Exceptions;
using SimpleBlog.Infrastructure.Exceptions;
using System.Net;

namespace SimpleBlog.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.WebSockets.IsWebSocketRequest)
                    _logger.LogError(ex, "[WebSocket] An error occurred during WebSocket communication.");
                else
                {
                    _logger.LogError(ex, "Error caught by middleware");

                    context.Response.ContentType = "application/json";

                    context.Response.StatusCode = ex switch
                    {
                        BusinessRuleException => (int)HttpStatusCode.BadRequest,
                        NotFoundException => (int)HttpStatusCode.NotFound,
                        WebSocketConnectionException => (int)HttpStatusCode.InternalServerError,
                        UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                        _ => (int)HttpStatusCode.InternalServerError
                    };

                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = ex.Message
                    });
                }
            }
        }
    }
}
