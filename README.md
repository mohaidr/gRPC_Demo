# gRPC Demo: Client and Server in C#

This repository demonstrates a simple gRPC client and server implementation using .NET.

## 📄 Proto Definition
Create a file named `greet.proto` under the `Protos` folder:

```proto
syntax = "proto3";

option csharp_namespace = "GrpcServer";

package greet;

service Greeter {
  rpc SayHello (HelloRequest) returns (HelloReply);
}

message HelloRequest {
  string name = 1;
}

message HelloReply {
  string message = 1;
}
```

---

## 🛠️ Project Setup
Create two projects:

```bash
dotnet new grpc -o GrpcServer
dotnet new console -o GrpcClient
```

---

## 🚀 Server Configuration (`GrpcServer`)

### 1. Add Proto File
Save `greet.proto` in a folder called `Protos`.

### 2. Modify `GrpcServer.csproj`
```xml
<ItemGroup>
  <Protobuf Include="Protos\greet.proto" GrpcServices="Server" />
</ItemGroup>
```

### 3. Create the Service
Create `Services/GreeterService.cs`:
```csharp
using Grpc.Core;
using GrpcServer;

namespace GrpcServer.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = $"Hello, {request.Name}!"
            });
        }
    }
}
```

### 4. Register the Service
Modify `Program.cs`:
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<GrpcServer.Services.GreeterService>();
app.MapGet("/", () => "Use a gRPC client to communicate.");

app.Run();
```

---

## 💻 Client Configuration (`GrpcClient`)

### 1. Add Packages
```bash
cd GrpcClient
dotnet add package Grpc.Net.Client
dotnet add package Google.Protobuf
dotnet add package Grpc.Tools
```

### 2. Reference Proto File
Modify `GrpcClient.csproj`:
```xml
<ItemGroup>
  <Protobuf Include="..\GrpcServer\Protos\greet.proto" GrpcServices="Client" />
</ItemGroup>
```

### 3. Update `Program.cs`
```csharp
using Grpc.Net.Client;
using GrpcServer;

using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new Greeter.GreeterClient(channel);

Console.Write("Enter your name: ");
var name = Console.ReadLine();

var reply = await client.SayHelloAsync(new HelloRequest { Name = name });

Console.WriteLine("Greeting: " + reply.Message);
```

---

## ▶️ Running the Demo

### 1. Start the Server
```bash
cd GrpcServer
dotnet run
```

### 2. Run the Client in Another Terminal
```bash
cd GrpcClient
dotnet run
```

### ✅ Output
```
Enter your name: Hassan
Greeting: Hello, Hassan!
```

---

## 📌 Notes
- gRPC requires HTTP/2.
- For localhost development, certificates may be auto-generated.
- Ensure both projects target the same .NET version (e.g., net7.0 or net8.0).

---

## 📂 Folder Structure
```
GrpcServer
├── Protos
│   └── greet.proto
├── Services
│   └── GreeterService.cs
├── Program.cs
└── GrpcServer.csproj

GrpcClient
├── Program.cs
└── GrpcClient.csproj
```

---

Feel free to fork this repo and modify it for more advanced use cases like streaming, deadlines, and metadata!

