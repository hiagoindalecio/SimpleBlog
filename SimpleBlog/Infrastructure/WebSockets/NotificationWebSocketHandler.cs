using SimpleBlog.Application.Interfaces;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace SimpleBlog.Infrastructure.WebSockets
{
    public class NotificationWebSocketHandler : INotificationWebSocketHandler
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();

        public async Task HandleAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                using var socket = await context.WebSockets.AcceptWebSocketAsync();
                string socketId = Guid.NewGuid().ToString();
                _sockets.TryAdd(socketId, socket);

                while (socket.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024 * 4];
                    await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }

                _sockets.TryRemove(socketId, out _);
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        public async Task SendNotificationToAllAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var socket in _sockets.Values.Where(s => s.State == WebSocketState.Open))
            {
                await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
