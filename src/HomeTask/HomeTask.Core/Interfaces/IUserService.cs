using HomeTask.Core.Entities;
using HomeTask.Core.Models.Request;
using HomeTask.Core.Models.Response;

namespace HomeTask.Core.Interfaces;

public interface IUserService
{
    Task<int> CreateUserAsync(CreateUserRequestModel model, CancellationToken ct);

    Task<IList<UserResponseModel>> GetUsersAsync(CancellationToken ct);

    Task UpdateUserRoleAsync(int userId, UserRole newRole, CancellationToken ct);
}
