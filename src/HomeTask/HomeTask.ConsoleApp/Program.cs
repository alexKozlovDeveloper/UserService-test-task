// See https://aka.ms/new-console-template for more information

using System.Net.WebSockets;
using System.Text;

Console.WriteLine("Home Task");

Thread.Sleep(5_000);

using var client = new ClientWebSocket();

var uri = new Uri("wss://localhost:7210/users/1:subscribe");

await client.ConnectAsync(uri, CancellationToken.None);

Console.WriteLine("Connected to server.");

var receiveBuffer = new byte[1024 * 4];

while (client.State == WebSocketState.Open)
{
    var result = await client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

    if (result.MessageType == WebSocketMessageType.Close)
    {
        Console.WriteLine("Server closed connection.");
        break;
    }

    var response = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);

    Console.WriteLine($"Received: {response}");
}

await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done", CancellationToken.None);

Console.WriteLine("Disconnected.");