using HomeTask.Core.Models;

namespace HomeTask.Core.Interfaces;

public interface IUserService
{
    Task<int> CreateUserAsync(CreateUserDataModel model, CancellationToken ct);

    Task<IList<UserDto>> GetUsersAsync(CancellationToken ct);

    Task UpdateUserRoleAsync(int userId, UserRole newRole, CancellationToken ct);
}
