namespace SimpleBlog.Application.Interfaces
{
    public interface IWebSocketNotifierService
    {
        Task NotifyAsync(string message);
    }
}
