# Cezzi Security Framework

<p align="center">
  <img src="src/Cezzi.Security/.pack/cezzi-security.png" alt="Cezzi Security Logo" width="120" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Security"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi.Security-blue?logo=github" alt="GitHub Packages"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-security-cicd.yaml"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-security-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
</p>

---

## Overview

**Cezzi Security Framework** is a comprehensive .NET library providing robust cryptographic and hashing capabilities. It includes AES encryption/decryption, HMAC hashing, and property-level encryption utilities.

- **Target Framework:** .NET 9.0
- **License:** MIT
- **Author:** Ronaldo Vecchi
- **Repository:** [github.com/mtnvencenzo/cezzis-core](https://github.com/mtnvencenzo/cezzis-core)

---

## Features

- **AES Encryption:** Secure AES-256 encryption with CBC mode and PKCS7 padding
- **HMAC Hashing:** Support for HMAC-SHA256, HMAC-SHA384, and HMAC-SHA512
- **Property Encryption:** Extension methods for encrypting/decrypting object properties
- **PBKDF2 Support:** Optional PBKDF2 key derivation for enhanced security
- **Type-Safe Keys:** Strongly-typed crypto key management

---

## Installation

This package is hosted on GitHub Packages. To use it, add the GitHub NuGet source and authenticate with your GitHub credentials or a personal access token (PAT).

**Add the GitHub NuGet source:**

```shell
nuget source Add -Name "github" -Source "https://nuget.pkg.github.com/mtnvencenzo/index.json" -Username YOUR_GITHUB_USERNAME -Password YOUR_GITHUB_TOKEN
```

**Install the package:**

```shell
Install-Package Cezzi.Security --source "github"
```

Or via .NET CLI:

```shell
dotnet add package Cezzi.Security --source "github"
```

> **Note:** Replace `YOUR_GITHUB_USERNAME` and `YOUR_GITHUB_TOKEN` with your GitHub username and a personal access token with `read:packages` scope.

---

## Usage Examples

### AES Encryption
```csharp
using Cezzi.Security.Cryptography;

// Create a crypto key
var key = new CryptoKey("your-secret-key", "your-iv-vector");

// Create an AES crypto provider
var cryptoProvider = new AesCryptoProvider(key, usePbkdf2: true);

// Encrypt data
string encrypted = cryptoProvider.Encrypt("sensitive data");

// Decrypt data
string decrypted = cryptoProvider.Decrypt(encrypted);
```

### HMAC Hashing
```csharp
using Cezzi.Security;

// Generate HMAC-SHA256 hash
byte[] hash256 = Hashing.GenerateHMACSHA256("private-key", "data to hash");

// Generate HMAC-SHA384 hash
byte[] hash384 = Hashing.GenerateHMACSHA384("private-key", "data to hash");

// Generate HMAC-SHA512 hash
byte[] hash512 = Hashing.GenerateHMACSHA512("private-key", "data to hash");
```

### Property Encryption
```csharp
using Cezzi.Security.Cryptography;

public class User
{
    public string Name { get; set; }
    public string Ssn { get; set; }
}

// Create a user
var user = new User { Name = "John Doe", Ssn = "123-45-6789" };

// Encrypt a specific property
user = user.EncryptProperty(u => u.Ssn, cryptoProvider);

// Decrypt a specific property
user = user.DecryptProperty(u => u.Ssn, cryptoProvider);

// Async versions are also available
user = await user.EncryptPropertyAsync(u => u.Ssn, cryptoProvider);
user = await user.DecryptPropertyAsync(u => u.Ssn, cryptoProvider);
```

### Custom Crypto Provider
```csharp
using Cezzi.Security.Cryptography;

public class MyCryptoProvider : ICryptoProvider
{
    private readonly ICryptoKey _key;

    public MyCryptoProvider(ICryptoKey key)
    {
        _key = key;
    }

    public string Encrypt(string toEncrypt)
    {
        // Implement custom encryption
        return encryptedData;
    }

    public string Decrypt(string cipherText)
    {
        // Implement custom decryption
        return decryptedData;
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
- [GitHub NuGet Package](https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Security)
