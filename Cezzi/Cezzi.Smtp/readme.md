# Cezzi SMTP Framework

<p align="center">
  <img src="src/Cezzi.Smtp/.pack/cezzi-smtp.png" alt="Cezzi SMTP Logo" width="120" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Smtp"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi.Smtp-blue?logo=github" alt="GitHub Packages"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-smtp-cicd.yaml"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-smtp-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
</p>

---

## Overview

**Cezzi SMTP Framework** is a .NET library that provides a clean and flexible interface for sending emails using SMTP. It wraps the standard .NET `SmtpClient` with a more modern async interface and provides a factory pattern for creating SMTP clients with different configurations.

- **Target Framework:** .NET 9.0
- **License:** MIT
- **Author:** Ronaldo Vecchi
- **Repository:** [github.com/mtnvencenzo/cezzis-core](https://github.com/mtnvencenzo/cezzis-core)

---

## Features

- **Modern Async Interface:** All email operations are async with cancellation token support
- **Flexible Configuration:** Support for various SMTP server configurations
- **Factory Pattern:** Easy creation of SMTP clients with different settings
- **SSL Support:** Built-in support for secure SMTP connections
- **Credential Management:** Support for both default and custom credentials
- **Delivery Methods:** Configurable delivery methods for different scenarios

---

## Installation

This package is hosted on GitHub Packages. To use it, add the GitHub NuGet source and authenticate with your GitHub credentials or a personal access token (PAT).

**Add the GitHub NuGet source:**

```shell
nuget source Add -Name "github" -Source "https://nuget.pkg.github.com/mtnvencenzo/index.json" -Username YOUR_GITHUB_USERNAME -Password YOUR_GITHUB_TOKEN
```

**Install the package:**

```shell
Install-Package Cezzi.Smtp --source "github"
```

Or via .NET CLI:

```shell
dotnet add package Cezzi.Smtp --source "github"
```

> **Note:** Replace `YOUR_GITHUB_USERNAME` and `YOUR_GITHUB_TOKEN` with your GitHub username and a personal access token with `read:packages` scope.

---

## Usage Examples

### Basic Configuration
```csharp
using Cezzi.Smtp;
using System.Net;
using System.Net.Mail;

// Create SMTP client using factory
var factory = new SmtpClientFactory();
var smtpClient = factory.CreateClient("smtp.example.com", 587);

// Configure client
smtpClient.EnableSsl = true;
smtpClient.Credentials = new NetworkCredential("username", "password");
```

### Sending Emails
```csharp
using Cezzi.Smtp;
using System.Net.Mail;

// Create email message
var message = new MailMessage
{
    From = new MailAddress("sender@example.com"),
    Subject = "Test Email",
    Body = "This is a test email.",
    IsBodyHtml = false
};

message.To.Add(new MailAddress("recipient@example.com"));

// Add attachments
message.Attachments.Add(new Attachment("path/to/file.pdf"));

// Send email
await smtpClient.SendMailAsync(message);
```

### Using Different Delivery Methods
```csharp
using Cezzi.Smtp;
using System.Net.Mail;

// Configure for network delivery
smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

// Or use pickup directory for local testing
smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
smtpClient.PickupDirectoryLocation = @"C:\Emails";
```

### Using Default Credentials
```csharp
using Cezzi.Smtp;

// Use default network credentials
smtpClient.UseDefaultCredentials = true;
```

### Error Handling
```csharp
using Cezzi.Smtp;
using System.Net.Mail;

try
{
    await smtpClient.SendMailAsync(message);
}
catch (SmtpException ex)
{
    Console.WriteLine($"Failed to send email: {ex.Message}");
    Console.WriteLine($"Status Code: {ex.StatusCode}");
}
finally
{
    smtpClient.Dispose();
}
```

### Dependency Injection
```csharp
using Cezzi.Smtp;
using Microsoft.Extensions.DependencyInjection;

// Register services
services.AddSingleton<ISmtpClientFactory, SmtpClientFactory>();
services.AddScoped<ISmtpMailClient>(sp =>
{
    var factory = sp.GetRequiredService<ISmtpClientFactory>();
    var client = factory.CreateClient("smtp.example.com", 587);
    client.EnableSsl = true;
    client.Credentials = new NetworkCredential("username", "password");
    return client;
});
```

---

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request. Ensure all tests pass and follow the existing code style.

To run tests:
```shell
dotnet test
```

---

## License

This project is licensed under the MIT License. See the [LICENSE](../LICENSE) file for details.

---

## Links
- [GitHub Repository](https://github.com/mtnvencenzo/cezzis-core)
- [GitHub NuGet Package](https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Smtp) 