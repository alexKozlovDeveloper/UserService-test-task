using System.Net.WebSockets;
using System.Text;

namespace HomeTask.Core.Interfaces.Implementations;

public class WebSocketConnectionManager : IWebSocketConnectionManager
{
    private readonly Dictionary<int, Dictionary<Guid, WebSocket>> _userSocketsMapping = [];

    private object _lock = new();

    public Guid AddSocket(int userId, WebSocket socket)
    {
        var socketId = Guid.NewGuid();

        lock (_lock) 
        {
            if (!_userSocketsMapping.TryGetValue(userId, out var userSockets))
            {
                userSockets = [];

                _userSocketsMapping.Add(userId, userSockets);
            }

            userSockets.Add(socketId, socket);
        }

        return socketId;
    }

    public async Task NotifyAsync(int userId, string message, CancellationToken ct)
    {
        var buffer = Encoding.UTF8.GetBytes(message);

       List<WebSocket> sockets;

        lock (_lock)
        {
            if (!_userSocketsMapping.TryGetValue(userId, out var userSockets))
            {
                return;
            }

            sockets = userSockets
                .Select(x => x.Value)
                .ToList();
        }

        foreach (var socket in sockets)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, endOfMessage: true, ct);
            }
        }
    }
}