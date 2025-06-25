using HomeTask.Core.Interfaces.Implementations;
using Moq;
using System.Net.WebSockets;
using System.Text;

namespace HomeTask.Core.Tests;

public class WebSocketConnectionManagerTests
{
    [Fact]
    public async Task NotifyAsync_SuccessfullyNotified()
    {
        // Arrange
        var socket = new Mock<WebSocket>();

        ArraySegment<byte>? resultBytes = null;
        WebSocketMessageType? resultType = null;
        bool? resultEndOfMessage = null;

        socket
            .Setup(x => x.State)
            .Returns(WebSocketState.Open);

        socket
            .Setup(x => x.SendAsync(It.IsAny<ArraySegment<byte>>(), It.IsAny<WebSocketMessageType>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .Callback<ArraySegment<byte>, WebSocketMessageType, bool, CancellationToken>((bytes, type, endOfMessage, ct) =>
            {
                resultBytes = bytes;
                resultType = type;
                resultEndOfMessage = endOfMessage;
            })
            .Returns(Task.CompletedTask);

        var userId = 1;
        var message = "{\"Id\":1,\"Name\":\"Jon\",\"Role\":1}";

        var buffer = Encoding.UTF8.GetBytes(message);

        var webSocketConnectionManager = new WebSocketConnectionManager();

        // Act
        webSocketConnectionManager.AddSocket(userId, socket.Object);

        await webSocketConnectionManager.NotifyAsync(userId, message, CancellationToken.None);

        // Assert
        Assert.NotNull(resultBytes);
        Assert.NotNull(resultType);
        Assert.NotNull(resultEndOfMessage);

        var resultMessage = Encoding.UTF8.GetString(resultBytes.Value.Array!, 0, buffer.Length);

        Assert.Equal(message, resultMessage);
        Assert.True(resultEndOfMessage);
        Assert.Equal(WebSocketMessageType.Text, resultType);
    }
}