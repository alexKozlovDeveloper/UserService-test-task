using HomeTask.Core.Models;

namespace HomeTask.Core.Interfaces;

public interface IUserNotificationService
{
    Task NotifyUserUpdated(UserDto user);
}
