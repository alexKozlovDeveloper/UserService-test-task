using HomeTask.Core.Interfaces;
using HomeTask.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeTask.WebApi.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService UserService) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken ct)
    {
        return await UserService.GetUsersAsync(ct);
    }

    [HttpPost]
    public async Task<int> CreateUsersAsync(
        [FromBody] CreateUserDataModel model, 
        CancellationToken ct
        )
    {
        return await UserService.CreateUserAsync(model, ct);
    }

    [HttpPost("{userId}:update-user-role")]
    public async Task UpdateUserRoleAsync(
        [FromRoute] int userId,
        [FromBody] UserRole newRole,
        CancellationToken ct
        )
    {
        await UserService.UpdateUserRoleAsync(userId, newRole, ct);
    }
}
