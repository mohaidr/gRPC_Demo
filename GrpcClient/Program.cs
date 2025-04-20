using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer; // This comes from the auto-generated code

class Program
{
    static async Task Main(string[] args)
    {
        using var channel = GrpcChannel.ForAddress("https://cri2.azure-api.net/greet.Greeter/SayHello"); // Ensure this matches your server's address

        var client = new Greeter.GreeterClient(channel);

        var tasks = Enumerable.Range(0, 1).Select(async i =>
        {
            var reply = await client.SayHelloAsync(new HelloRequest { Name = $"User{i}" });
            Console.WriteLine($"Server Response {i}: {reply.Message}");
        });

        // Executes all tasks concurrently
        await Task.WhenAll(tasks);
    }
}
