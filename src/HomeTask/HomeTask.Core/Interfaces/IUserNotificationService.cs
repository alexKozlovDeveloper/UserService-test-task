using HomeTask.Core.Models.Response;

namespace HomeTask.Core.Interfaces;

public interface IUserNotificationService
{
    Task NotifyUserUpdatedAsync(UserResponseModel user, CancellationToken ct);
}
