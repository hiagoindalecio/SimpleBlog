using SimpleBlog.Application.Interfaces;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace SimpleBlog.Infrastructure.WebSockets
{
    public class NotificationWebSocketHandler : INotificationWebSocketHandler
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();
        private static readonly Dictionary<string, int> _connectionsPerIp = [];

        public async Task HandleAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                lock (_connectionsPerIp)
                {
                    if (!_connectionsPerIp.ContainsKey(ip))
                        _connectionsPerIp[ip] = 0;

                    _connectionsPerIp[ip]++;
                    if (_connectionsPerIp[ip] > 5) // Max 5 per IP
                    {
                        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        return;
                    }
                }

                string socketId = Guid.NewGuid().ToString();
                using var socket = await context.WebSockets.AcceptWebSocketAsync();
                _sockets.TryAdd(socketId, socket);

                while (socket.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024 * 4];
                    var receiveResult = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    while (!receiveResult.CloseStatus.HasValue)
                    {
                        try
                        {
                            var message = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);

                            Console.WriteLine($"[WebSocket] Unexpected client message received: {message}");
                        }
                        catch
                        { }

                        await SendErrorAsync(socket, "Receiving messages is not supported.");
                        await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Messages not allowed", CancellationToken.None);
                        return;
                    }
                }

                _sockets.TryRemove(socketId, out _);
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);

                lock (_connectionsPerIp)
                {
                    _connectionsPerIp[ip]--;
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        public async Task SendNotificationToAllAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var socket in _sockets.Values.Where(s => s.State == WebSocketState.Open))
                await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private static async Task SendErrorAsync(WebSocket socket, string errorMessage)
        {
            var errorJson = JsonSerializer.Serialize(new { error = errorMessage });
            var bytes = Encoding.UTF8.GetBytes(errorJson);
            await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
