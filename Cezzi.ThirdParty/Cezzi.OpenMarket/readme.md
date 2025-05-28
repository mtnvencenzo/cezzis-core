# Cezzi.OpenMarket

<p align="center">
  <img src="https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi.ThirdParty/Cezzi.OpenMarket/src/Cezzi.OpenMarket/.pack/cezzi-openmarket.png" alt="Cezzi.OpenMarket" width="200" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-applications-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi-blue?logo=github" alt="GitHub Packages"></a>
</p>

---

## Overview

Cezzi.OpenMarket is a .NET library that provides a simplified interface for working with OpenMarket's SMS API. It wraps the official OpenMarket API to provide a more streamlined experience while maintaining full functionality.

## Features

- Simplified interface for common SMS operations
- Full support for OpenMarket API features
- Async/await support throughout
- Proper cancellation token support
- ConfigureAwait(false) optimization
- Support for multiple message types (Text, HexEncodedText, Binary, WAP Push)
- Configurable message handling (Reject, Truncate, Segment)
- Comprehensive error handling
- Support for flash messages
- Configurable validity periods

## Installation

```shell
dotnet add package Cezzi.OpenMarket --source "github"
```

## Usage

### Basic Setup

```csharp
// Register the OpenMarket service with DI
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();

        services.RegisterServiceClient<IOpenMarketSmsClient, OpenMarketSmsClient>((sp, client) =>
        {
            var config = sp.GetRequiredService<IOptionsMonitor<OpenMarketSmsConfiguration>>();
            client.BaseAddress = new Uri(config.CurrentValue.Host);
        });
    }
}
```

### Sending SMS Messages

```csharp
// Create an SMS request
var request = new SmsSendRequest
{
    DestinationPhoneNumber = "1234567890",
    DestinationCountryCode = "1",
    TextContent = "Hello from Cezzi.OpenMarket!",
    Note1 = "Optional tracking ID",
    Note2 = "Optional additional info",
    ProgramId = "1234",
    Flash = false,
    MobileOperatorId = "383",
    Mlc = MlcType.Segment,
    ValidityPeriod = 259200,
    Type = SmsMessageType.Text
};

// Send the SMS
var result = await smsClient.SendAsync(
    body: request,
    originator: "YourSenderID",
    account: "your_account",
    password: "your_password");

// Check the result
if (result.SendStatus == SmsSendStatus.Accepted)
{
    // Message was accepted
    var requestId = result.RequestId;
    var location = result.Location;
}
else
{
    // Handle failure
    var errorMessage = result.DetailedMessage;
}
```

### Error Handling

```csharp
try
{
    var result = await smsClient.SendAsync(
        body: request,
        originator: "YourSenderID",
        account: "your_account",
        password: "your_password");
    
    if (result.SendStatus == SmsSendStatus.Failed)
    {
        // Handle failure
        Console.WriteLine($"Failed to send SMS: {result.DetailedMessage}");
    }
}
catch (Exception ex)
{
    // Handle exceptions
    Console.WriteLine($"Error sending SMS: {ex.Message}");
}
```

## Best Practices

1. **Credentials Management**: Store your OpenMarket credentials securely, preferably in Azure Key Vault or similar.

2. **Error Handling**: Always implement proper error handling for SMS operations.

3. **Message Types**: Choose the appropriate message type based on your content and requirements.

4. **Message Handling**: Configure MLC (Message Length Control) based on your application's needs.

5. **Validity Period**: Set appropriate validity periods for your messages.

6. **Flash Messages**: Use flash messages sparingly and only when necessary.

7. **Monitoring**: Set up monitoring and alerting for SMS operations.

## Contributing

Contributions are welcome! Please read our [Contributing Guide](../../CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](../../LICENSE) file for details.

## Author

- **Ronaldo Vecchi** - [GitHub](https://github.com/mtnvencenzo) 