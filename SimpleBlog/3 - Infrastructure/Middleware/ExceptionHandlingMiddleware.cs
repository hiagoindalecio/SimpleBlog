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
                await _next(context); // passa para o próximo middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error caught by middleware");

                context.Response.ContentType = "application/json";

                context.Response.StatusCode = ex switch
                {
                    BusinessRuleException => (int)HttpStatusCode.BadRequest,
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    WebSocketConnectionException => (int)HttpStatusCode.InternalServerError,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                var result = new
                {
                    error = ex.Message
                };

                await context.Response.WriteAsJsonAsync(result);
            }
        }
    }
}
