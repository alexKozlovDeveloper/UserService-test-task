using HomeTask.Core.Models;

namespace HomeTask.Core.Interfaces.Implementations;

public class EventService(IWebSocketConnectionManager WebSocketConnectionManager) : IEventService
{
    public async Task SendAsync(UserUpdatedEvent eventModel, CancellationToken ct)
    {
        var message = System.Text.Json.JsonSerializer.Serialize(eventModel.User);

        await WebSocketConnectionManager.NotifyAsync(eventModel.User.Id, message, ct);
    }
}
