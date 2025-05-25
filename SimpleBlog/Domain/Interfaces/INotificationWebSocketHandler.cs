namespace SimpleBlog.Domain.Interfaces
{
    public interface INotificationWebSocketHandler
    {
        Task HandleAsync(HttpContext context);
        Task SendNotificationToAllAsync(string message);
    }
}
