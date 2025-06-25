using HomeTask.Core.Interfaces.Implementations;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HomeTask.WebApi.Controllers;

[ApiController]
[Route("users")]
public class CommunicationController(WebSocketConnectionManager ConnectionManager) : ControllerBase
{
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