using HomeTask.Core.Models.Response;

namespace HomeTask.Core.Interfaces.Implementations;

public class UserNotificationService : IUserNotificationService
{
    public Task NotifyUserUpdated(UserResponseModel user)
    {
        return Task.CompletedTask;
    }
}
