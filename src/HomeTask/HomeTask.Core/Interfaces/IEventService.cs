using HomeTask.Core.Models;

namespace HomeTask.Core.Interfaces;

public interface IEventService
{
    // TODO: use MediatR ? 
    // TODO: make it general ?
    Task SendAsync(UserUpdatedEvent eventModel, CancellationToken ct);
}
