# Cezzi Security Identity Tokens Framework

<p align="center">
  <img src="src/Cezzi.Security.Identity.Tokens/.pack/cezzi-security-tok.png" alt="Cezzi Security Identity Tokens Logo" width="120" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Security.Identity.Tokens"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi.Security.Identity.Tokens-blue?logo=github" alt="GitHub Packages"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-security-identity-tokens-cicd.yaml"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-security-identity-tokens-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
</p>

---

## Overview

**Cezzi Security Identity Tokens Framework** is a comprehensive .NET library for JWT (JSON Web Token) handling and validation. It provides robust token generation, validation, and claim management capabilities with support for multiple signing algorithms and encryption requirements.

- **Target Framework:** .NET 9.0
- **License:** MIT
- **Author:** Ronaldo Vecchi
- **Repository:** [github.com/mtnvencenzo/cezzis-core](https://github.com/mtnvencenzo/cezzis-core)

---

## Features

- **JWT Generation:** Create signed JWT tokens with customizable claims and headers
- **Token Validation:** Comprehensive token validation with detailed error reporting
- **Multiple Algorithms:** Support for HMAC-SHA256, HMAC-SHA384, and HMAC-SHA512
- **Claim Management:** Built-in claim types with encryption requirements
- **Scope Validation:** Resource server validation with scope and subscope support
- **Configuration:** Flexible configuration through app settings

---

## Installation

This package is hosted on GitHub Packages. To use it, add the GitHub NuGet source and authenticate with your GitHub credentials or a personal access token (PAT).

**Add the GitHub NuGet source:**

```shell
nuget source Add -Name "github" -Source "https://nuget.pkg.github.com/mtnvencenzo/index.json" -Username YOUR_GITHUB_USERNAME -Password YOUR_GITHUB_TOKEN
```

**Install the package:**

```shell
Install-Package Cezzi.Security.Identity.Tokens --source "github"
```

Or via .NET CLI:

```shell
dotnet add package Cezzi.Security.Identity.Tokens --source "github"
```

> **Note:** Replace `YOUR_GITHUB_USERNAME` and `YOUR_GITHUB_TOKEN` with your GitHub username and a personal access token with `read:packages` scope.

---

## Usage Examples

### Token Generation
```csharp
using Cezzi.Security.Identity.Tokens.Jwt;

// Create JWT parameters
var parameters = new JwtParameters("your-secret-key", JwtAlgorithmType.HMAC_256);

// Set required header items
parameters.HeaderItems[JwtHeaderItemType.Audience] = "your-audience";
parameters.HeaderItems[JwtHeaderItemType.Issuer] = "your-issuer";
parameters.HeaderItems[JwtHeaderItemType.Expires] = DateTime.UtcNow.AddHours(1).ToString("o");

// Add claims
parameters.Claims[JwtClaimType.jti.ToString()] = Guid.NewGuid().ToString();
parameters.Claims[JwtClaimType.email.ToString()] = "user@example.com";
parameters.Claims[JwtClaimType.userid.ToString()] = "12345";

// Generate token
string token = JwtHandler.GenerateNew(parameters);
```

### Token Validation
```csharp
using Cezzi.Security.Identity.Tokens.Jwt;

// Create validation parameters
var parameters = new JwtParameters("your-secret-key", JwtAlgorithmType.HMAC_256);
parameters.HeaderItems[JwtHeaderItemType.Audience] = "your-audience";
parameters.HeaderItems[JwtHeaderItemType.Issuer] = "your-issuer";

// Validate token
var validation = JwtHandler.Validate(token, parameters);

if (validation.IsAuthenticated)
{
    // Access claims
    foreach (var claim in validation.Claims)
    {
        Console.WriteLine($"{claim.Name}: {claim.Value}");
    }
}
else
{
    Console.WriteLine($"Validation failed: {validation.Reason}");
}
```

### Resource Server Validation
```csharp
using Cezzi.Security.Identity.Tokens;

// Configure validation parameters in app settings
// Format: scope,subscope,algtype,sharedKey,audience,issuer||scope,subscope,algtype,sharedKey,audience,issuer
var appSetting = "api,read,HMAC_256,your-key,your-audience,your-issuer";

// Create resolver
var resolver = new AppSettingsTokenScopeValidationParameterResolver(appSetting);

// Get validation parameters for specific scope
var validationParams = resolver.GetTokenValidationParameters("api", "read");
```

### Claim Management
```csharp
using Cezzi.Security.Identity.Tokens.Jwt;

// Check if a claim type requires encryption
bool requiresEncryption = JwtClaimType.email.IsEncrypted(); // Returns true

// Get specific claim value
string email = JwtHandler.GetClaimValue(token, JwtClaimType.email.ToString());

// Get multiple claim values
var claims = JwtHandler.GetClaimValues(token, 
    JwtClaimType.email.ToString(),
    JwtClaimType.userid.ToString());
```

### Token Information
```csharp
using Cezzi.Security.Identity.Tokens.Jwt;

// Get token ID
string tokenId = JwtHandler.GetTokenId(token);

// Decode token for inspection
var decodedToken = JwtHandler.DecodeJwtToken(token);
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
- [GitHub NuGet Package](https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Security.Identity.Tokens)
