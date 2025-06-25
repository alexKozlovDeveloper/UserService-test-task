using HomeTask.Core.Entities;
using HomeTask.Core.Interfaces;
using HomeTask.Core.Interfaces.Implementations;
using HomeTask.Core.Models;
using HomeTask.Core.Models.Response;
using Moq;

namespace HomeTask.Core.Tests;

public class EventServiceTests
{
    [Fact]
    public async Task SendAsync_SuccessfullySented()
    {
        // Arrange
        var webSocketConnectionManager = new Mock<IWebSocketConnectionManager>();

        int? resultUserId = null;
        string resultMessage = null;

        webSocketConnectionManager
            .Setup(x => x.NotifyAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<int, string, CancellationToken>((userId, message, ct) => 
            { 
                resultUserId = userId; 
                resultMessage = message; 
            })
            .Returns(Task.CompletedTask);

        var eventService = new EventService(webSocketConnectionManager.Object);

        var model = new UserUpdatedEvent
        {
            User = new UserResponseModel
            {
                Id = 1,
                Name = "Jon",
                Role = UserRole.User
            }
        };

        var expectedUserId = 1;
        var expectedMessage = System.Text.Json.JsonSerializer.Serialize(model.User);

        // Act
        await eventService.SendAsync(model, CancellationToken.None);

        // Assert
        Assert.NotNull(resultUserId);
        Assert.NotNull(resultMessage);

        Assert.Equal(expectedUserId, resultUserId);
        Assert.Equal(expectedMessage, resultMessage);
    }
}