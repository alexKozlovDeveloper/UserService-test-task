using System.Net.WebSockets;

namespace HomeTask.Core.Interfaces;

// TODO: manager must know nothing about user ? only message delivery ?
public interface IWebSocketConnectionManager
{
    Guid AddSocket(int userId, WebSocket socket);
    void RemoveSocket(int userId, Guid socketId);
    Task NotifyAsync(int userId, string message, CancellationToken ct);
}
