# Cezzi Security reCAPTCHA Framework

<p align="center">
  <img src="src/Cezzi.Security.Recaptcha/.pack/cezzi-recaptcha.png" alt="Cezzi Security reCAPTCHA Logo" width="120" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Security.Recaptcha"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi.Security.Recaptcha-blue?logo=github" alt="GitHub Packages"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-security-recaptcha-cicd.yaml"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-security-recaptcha-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
</p>

---

## Overview

**Cezzi Security reCAPTCHA Framework** is a .NET library that provides a simple and efficient way to integrate Google's reCAPTCHA verification into your applications. It handles the verification process, error handling, and provides detailed verification results.

- **Target Framework:** .NET 9.0
- **License:** MIT
- **Author:** Ronaldo Vecchi
- **Repository:** [github.com/mtnvencenzo/cezzis-core](https://github.com/mtnvencenzo/cezzis-core)

---

## Features

- **Easy Integration:** Simple setup with dependency injection
- **Comprehensive Verification:** Full support for reCAPTCHA v2 and v3 verification
- **Detailed Results:** Rich verification results including scores and error codes
- **Error Handling:** Built-in error handling and status reporting
- **Configuration:** Flexible configuration through app settings
- **HTTP Client Management:** Efficient HTTP client handling with factory pattern

---

## Installation

This package is hosted on GitHub Packages. To use it, add the GitHub NuGet source and authenticate with your GitHub credentials or a personal access token (PAT).

**Add the GitHub NuGet source:**

```shell
nuget source Add -Name "github" -Source "https://nuget.pkg.github.com/mtnvencenzo/index.json" -Username YOUR_GITHUB_USERNAME -Password YOUR_GITHUB_TOKEN
```

**Install the package:**

```shell
Install-Package Cezzi.Security.Recaptcha --source "github"
```

Or via .NET CLI:

```shell
dotnet add package Cezzi.Security.Recaptcha --source "github"
```

> **Note:** Replace `YOUR_GITHUB_USERNAME` and `YOUR_GITHUB_TOKEN` with your GitHub username and a personal access token with `read:packages` scope.

---

## Usage Examples

### Configuration
Add the following to your `appsettings.json`:

```json
{
  "Recaptcha": {
    "SiteSecret": "your-recaptcha-secret-key",
    "SiteVerifyUrl": "https://www.google.com/recaptcha/api/siteverify"
  }
}
```

### Service Registration
```csharp
using Cezzi.Security.Recaptcha;
using Microsoft.Extensions.DependencyInjection;

public void ConfigureServices(IServiceCollection services)
{
    services.UseRecaptcha(Configuration);
}
```

### Basic Verification
```csharp
using Cezzi.Security.Recaptcha;
using Microsoft.Extensions.Options;

public class RecaptchaController : ControllerBase
{
    private readonly IRecaptchaSiteVerifyService _recaptchaService;
    private readonly RecaptchaConfig _config;

    public RecaptchaController(
        IRecaptchaSiteVerifyService recaptchaService,
        IOptions<RecaptchaConfig> config)
    {
        _recaptchaService = recaptchaService;
        _config = config.Value;
    }

    public async Task<IActionResult> Verify(string recaptchaResponse, string userIp)
    {
        var result = await _recaptchaService.Verify(recaptchaResponse, _config, userIp);

        if (result.VerificationStatus == RecaptchaVerificationStatus.Success)
        {
            // Verification successful
            return Ok(new { 
                success = true,
                score = result.Score,
                hostname = result.Hostname
            });
        }
        else
        {
            // Handle verification failure
            return BadRequest(new { 
                success = false,
                errors = result.ReturnCodes.Select(c => c.Message)
            });
        }
    }
}
```

### Handling Different Verification Codes
```csharp
using Cezzi.Security.Recaptcha;

public async Task HandleVerification(RecaptchaVerificationResult result)
{
    switch (result.VerificationStatus)
    {
        case RecaptchaVerificationStatus.Success:
            // Handle successful verification
            if (result.Score.HasValue)
            {
                // For reCAPTCHA v3, check the score
                if (result.Score.Value >= 0.5m)
                {
                    // High confidence - allow the action
                }
                else
                {
                    // Low confidence - require additional verification
                }
            }
            break;

        case RecaptchaVerificationStatus.Failed:
            // Handle verification failure
            foreach (var code in result.ReturnCodes)
            {
                switch (code.Code)
                {
                    case 100: // CommunicationError
                        // Handle communication error
                        break;
                    case 101: // SecretRequired
                        // Handle missing secret key
                        break;
                    case 102: // SecretInvalid
                        // Handle invalid secret key
                        break;
                    case 103: // VerificationCodeRequired
                        // Handle missing verification code
                        break;
                    case 104: // VerificationCodeInvalid
                        // Handle invalid verification code
                        break;
                    case 105: // InvalidRequest
                        // Handle invalid request
                        break;
                    case 106: // Timeout
                        // Handle timeout
                        break;
                }
            }
            break;

        case RecaptchaVerificationStatus.NotAttempted:
            // Handle case where verification was not attempted
            break;
    }
}
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
- [GitHub NuGet Package](https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Security.Recaptcha) 