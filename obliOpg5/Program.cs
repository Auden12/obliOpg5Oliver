using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

TcpListener listener = new TcpListener(IPAddress.Any, 7);
listener.Start();
while (true)
{
    TcpClient socket = listener.AcceptTcpClient();
    Task.Run(() => HandleClient(socket));
}

void HandleClient(TcpClient socket)
{
    NetworkStream ns = socket.GetStream();
    StreamReader reader = new StreamReader(ns);
    StreamWriter writer = new StreamWriter(ns);
    int count = 0;
    while (socket.Connected)
    {
        string message = reader.ReadLine().ToLower();
        count++;
        Console.WriteLine(message);
        if (message == "stop")
        {
            var response = new { message = "Goodbye world" };
            writer.WriteLine(JsonSerializer.Serialize(response));
            writer.Flush();
            socket.Close();
        }
        else if (message.Contains("random number"))
        {
            writer.WriteLine(JsonSerializer.Serialize(new { message = "give input to get random number" }));
            writer.Flush();
            string TalR = reader.ReadLine();
            var range = JsonSerializer.Deserialize<int[]>(TalR);
            int x = range[0];
            int y = range[1];
            Console.WriteLine(x);
            Console.WriteLine(y);

            Random random = new Random();
            int n = random.Next(x, y);
            var response = new { randomNumber = n };
            writer.WriteLine(JsonSerializer.Serialize(response));
            writer.Flush();
        }
        else if (message.Contains("add"))
        {
            writer.WriteLine(JsonSerializer.Serialize(new { message = "adding" }));
            writer.Flush();
            string TalA = reader.ReadLine();
            var numbers = JsonSerializer.Deserialize<int[]>(TalA);
            int x1 = numbers[0];
            int y1 = numbers[1];
            var response = new { result = x1 + y1 };
            writer.WriteLine(JsonSerializer.Serialize(response));
            writer.Flush();
        }
        else if (message.Contains("subtract"))
        {
            writer.WriteLine(JsonSerializer.Serialize(new { message = "subtracting" }));
            writer.Flush();
            string TalS = reader.ReadLine();
            var numbers = JsonSerializer.Deserialize<int[]>(TalS);
            int x2 = numbers[0];
            int y2 = numbers[1];
            var response = new { result = x2 - y2 };
            writer.WriteLine(JsonSerializer.Serialize(response));
            writer.Flush();
        }
    }
}