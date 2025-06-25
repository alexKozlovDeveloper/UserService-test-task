using HomeTask.Core.Models.Response;

namespace HomeTask.Core.Interfaces.Implementations;

public class UserNotificationService(IWebSocketConnectionManager WebSocketConnectionManager) : IUserNotificationService
{
    public async Task NotifyUserUpdatedAsync(UserResponseModel user, CancellationToken ct)
    {
        var message = System.Text.Json.JsonSerializer.Serialize(user);

        await WebSocketConnectionManager.NotifyAsync(user.Id, message, ct);
    }
}
