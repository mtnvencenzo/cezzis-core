# Cezzi.SendGrid

<p align="center">
  <img src="https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi.ThirdParty/Cezzi.SendGrid/src/Cezzi.SendGrid/.pack/cezzi-sendgrid.png" alt="Cezzi.SendGrid" width="200" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-applications-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi-blue?logo=github" alt="GitHub Packages"></a>
</p>

---

## Overview

Cezzi.SendGrid is a .NET library that provides a simplified interface for working with SendGrid's email API. It wraps the official SendGrid SDK to provide a more streamlined experience while maintaining full functionality.

## Features

- Simplified interface for common email operations
- Full support for SendGrid SDK features
- Async/await support throughout
- Proper cancellation token support
- ConfigureAwait(false) optimization
- Support for HTML and plain text content
- Priority email support
- Configurable retry policies
- Comprehensive error handling

## Installation

```shell
dotnet add package Cezzi.SendGrid --source "github"
```

## Usage

### Basic Setup

```csharp
// Create a SendGrid configuration
var sendGridConfiguration = new SendGridConfiguration
{
    SendGridApiKey = "your_api_key",
    SendGridHost = "https://api.sendgrid.com",
    SendGridUrlPath = "mail/send",
    SendGridVersion = "v3",
    HttpErrorsAsExceptions = true,
    Reliability = new SendGridReliability
    {
        MaximumNumberOfRetries = 3,
        MinimumBackoffSeconds = 1,
        MaximumBackoffSeconds = 30,
        DeltaBackoffSeconds = 2
    }
};

// Create an HTTP client
var httpClient = new HttpClient();

// Create the email service
ISendGridEmailService emailService = SendGridEmailService.Create(httpClient);
```

### Sending Emails

```csharp
// Create email recipients
var fromEmail = new EmailValue("sender@example.com", "Sender Name");
var toEmails = new List<EmailValue>
{
    new("recipient1@example.com", "Recipient 1"),
    new("recipient2@example.com", "Recipient 2")
};

// Send an email
var success = await emailService.SendAsync(
    subject: "Test Email",
    fromEmail: fromEmail,
    tos: toEmails,
    sendGridConfiguration: sendGridConfiguration,
    htmlContent: "<h1>Hello World</h1><p>This is a test email.</p>",
    plainTextContent: "Hello World\nThis is a test email.",
    sendWithPriority: true);
```

### Error Handling

```csharp
try
{
    var success = await emailService.SendAsync(
        subject: "Test Email",
        fromEmail: fromEmail,
        tos: toEmails,
        sendGridConfiguration: sendGridConfiguration,
        htmlContent: "<h1>Hello World</h1>");
    
    if (!success)
    {
        // Handle failed email send
    }
}
catch (Exception ex)
{
    // Handle exceptions
}
```

## Best Practices

1. **API Key Management**: Store your SendGrid API key securely, preferably in Azure Key Vault or similar.

2. **Error Handling**: Always implement proper error handling for email operations.

3. **Retry Configuration**: Configure retry options based on your application's requirements.

4. **Content Types**: Always provide both HTML and plain text content for better email client compatibility.

5. **Priority Usage**: Use priority emails sparingly and only for truly urgent messages.

6. **Rate Limits**: Be aware of SendGrid's rate limits and implement appropriate throttling.

7. **Monitoring**: Set up monitoring and alerting for email operations.

## Contributing

Contributions are welcome! Please read our [Contributing Guide](../../CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](../../LICENSE) file for details.

## Author

- **Ronaldo Vecchi** - [GitHub](https://github.com/mtnvencenzo) 