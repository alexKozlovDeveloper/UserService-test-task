using HomeTask.Core.Models;

namespace HomeTask.Core.Interfaces.Implementations;

public class UserNotificationService : IUserNotificationService
{
    public Task NotifyUserUpdated(UserDto user)
    {
        return Task.CompletedTask;
    }
}
