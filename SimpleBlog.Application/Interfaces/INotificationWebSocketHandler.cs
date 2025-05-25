using Microsoft.AspNetCore.Http;

namespace SimpleBlog.Application.Interfaces
{
    public interface INotificationWebSocketHandler
    {
        Task HandleAsync(HttpContext context);
        Task SendNotificationToAllAsync(string message);
    }
}
