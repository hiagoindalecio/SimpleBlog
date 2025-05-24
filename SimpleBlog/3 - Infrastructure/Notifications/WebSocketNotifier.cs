using SimpleBlog.Application.Interfaces;

namespace SimpleBlog.Infrastructure.Notifications
{
    public class WebSocketNotifier : IWebSocketNotifierService
    {
        public Task NotifyAsync(string message)
        {
            // Criar lógica de envio para os sockets conectados.
            Console.WriteLine($"[Notificação] {message}");
            return Task.CompletedTask;
        }
    }
}
