using HomeTask.Core.Entities;
using HomeTask.Core.Interfaces;
using HomeTask.Core.Models.Request;
using HomeTask.Core.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HomeTask.WebApi.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService UserService, IWebSocketConnectionManager ConnectionManager) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<UserResponseModel>> GetUsersAsync(CancellationToken ct)
    {
        return await UserService.GetUsersAsync(ct);
    }

    [HttpPost]
    public async Task<int> CreateUsersAsync(
        [FromBody] CreateUserRequestModel model, 
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

    [HttpGet("{userId}:subscribe")]
    public async Task Subscribe(int userId)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            ConnectionManager.AddSocket(userId, socket);

            while (socket.State == System.Net.WebSockets.WebSocketState.Open)
            {
                Thread.Sleep(1000);
            }
        }
        else
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
