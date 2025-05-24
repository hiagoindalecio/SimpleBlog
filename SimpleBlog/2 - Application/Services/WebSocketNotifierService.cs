using SimpleBlog.Application.Interfaces;

namespace SimpleBlog.Application.Services
{
    public class WebSocketNotifierService : IWebSocketNotifierService
    {
        public Task NotifyAsync(string message)
        {
            // TODO: Implementation for notifying via WebSocket.
            return Task.CompletedTask;
        }
    }
}
