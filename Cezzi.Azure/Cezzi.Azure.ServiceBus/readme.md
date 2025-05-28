# Cezzi.Azure.ServiceBus

<p align="center">
  <img src="https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi.Azure/Cezzi.Azure.ServiceBus/src/Cezzi.Azure.ServiceBus/.pack/cezzi-azure-servicebus.png" alt="Cezzi.Azure.ServiceBus" width="200" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-applications-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi-blue?logo=github" alt="GitHub Packages"></a>
</p>

---

## Overview

Cezzi.Azure.ServiceBus is a .NET library that provides a simplified interface for working with Azure Service Bus. It wraps the Azure.Messaging.ServiceBus SDK to provide a more streamlined experience while maintaining full functionality.

## Features

- Simplified interface for common Service Bus operations
- Full support for Azure.Messaging.ServiceBus SDK features
- Async/await support throughout
- Proper cancellation token support
- ConfigureAwait(false) optimization
- Support for both queues and topics
- Message scheduling capabilities
- Session support
- Retry policies with configurable options
- Support for both Shared Access Key and Managed Identity authentication

## Installation

```shell
dotnet add package Cezzi.Azure.ServiceBus --source "github"
```

## Usage

### Basic Setup

```csharp
// Configure services
services.UseServiceBusMessagingServices();

// Create a configuration
var configuration = new SendConfiguration
{
    Label = "MyService",
    QueueOrTopicName = "my-queue",
    SendConnectionString = "your_connection_string",
    SendRetry = new SendRetryOptions
    {
        MaxRetries = 3,
        MaxRetryDelaySeconds = 30,
        OperationTimeoutInSeconds = 10,
        RetryDelaySeconds = 5
    }
};

// Get the message sender from DI
IServiceBusMessageSender messageSender = serviceProvider.GetRequiredService<IServiceBusMessageSender>();
```

### Sending Messages

```csharp
// Define your message type
public class OrderMessage
{
    public string OrderId { get; set; }
    public string CustomerId { get; set; }
    public decimal Amount { get; set; }
}

// Send a single message
var order = new OrderMessage
{
    OrderId = "123",
    CustomerId = "456",
    Amount = 99.99m
};

await messageSender.Send(
    message: order,
    configuration: configuration,
    correlationId: Guid.NewGuid().ToString(),
    contentType: "application/json",
    sessionId: "customer-456",
    enqueueDelay: TimeSpan.FromMinutes(5));

// Send multiple messages
var orders = new List<OrderMessage>
{
    new() { OrderId = "123", CustomerId = "456", Amount = 99.99m },
    new() { OrderId = "124", CustomerId = "456", Amount = 149.99m }
};

var sentCount = await messageSender.SendMessages(
    messages: orders,
    configuration: configuration,
    correlationId: Guid.NewGuid().ToString());

// Send raw JSON
var json = @"{""orderId"":""123"",""customerId"":""456"",""amount"":99.99}";
await messageSender.SendJson(
    json: json,
    configuration: configuration,
    correlationId: Guid.NewGuid().ToString());
```

### Error Handling

```csharp
try
{
    await messageSender.Send(
        message: order,
        configuration: configuration,
        correlationId: Guid.NewGuid().ToString());
}
catch (ServiceBusDataLossException<OrderMessage> ex)
{
    // Handle data loss exception
    var lostOrder = ex.DataObject;
    // Log or handle the lost order
}
catch (Exception ex)
{
    // Handle other exceptions
}
```

### Failure Handling

```csharp
async Task OnSendFailure(
    string message,
    ISendConfiguration configuration,
    string messageId,
    string correlationId,
    string contentType,
    string sessionId,
    TimeSpan? enqueueDelay,
    Exception ex)
{
    // Log the failure
    Console.WriteLine($"Failed to send message {messageId}: {ex.Message}");
    
    // Store the failed message for retry
    await StoreFailedMessage(message, messageId, correlationId);
}

// Use the failure handler
await messageSender.Send(
    message: order,
    configuration: configuration,
    correlationId: Guid.NewGuid().ToString(),
    onSendFailure: OnSendFailure);
```

## Best Practices

1. **Connection Management**: The library manages ServiceBusClient instances internally for optimal performance.

2. **Error Handling**: Always implement proper error handling, especially for network-related operations.

3. **Cancellation**: Use cancellation tokens for long-running operations to allow proper cancellation.

4. **Message Size**: Keep messages small and focused on a single responsibility.

5. **Retry Configuration**: Configure retry options based on your application's requirements and Service Bus quotas.

6. **Session Usage**: Use sessions when you need to maintain message order or group related messages.

7. **Correlation IDs**: Always provide correlation IDs to track message flow across your system.

## Contributing

Contributions are welcome! Please read our [Contributing Guide](../../CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](../../LICENSE) file for details.

## Author

- **Ronaldo Vecchi** - [GitHub](https://github.com/mtnvencenzo)
