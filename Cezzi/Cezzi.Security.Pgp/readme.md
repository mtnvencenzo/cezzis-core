# Cezzi Security PGP Framework

<p align="center">
  <img src="src/Cezzi.Security.Pgp/.pack/cezzi-security-pgp.png" alt="Cezzi Security PGP Logo" width="120" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Security.Pgp"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi.Security.Pgp-blue?logo=github" alt="GitHub Packages"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-security-pgp-cicd.yaml"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-security-pgp-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
</p>

---

## Overview

**Cezzi Security PGP Framework** is a .NET library that provides a wrapper around the BouncyCastle.NetCore package for PGP (Pretty Good Privacy) encryption and decryption operations. It simplifies the process of encrypting and decrypting data using PGP keys while maintaining security best practices.

- **Target Framework:** .NET 9.0
- **License:** MIT
- **Author:** Ronaldo Vecchi
- **Repository:** [github.com/mtnvencenzo/cezzis-core](https://github.com/mtnvencenzo/cezzis-core)

---

## Features

- **PGP Encryption:** Encrypt data using public keys with support for integrity checking
- **PGP Decryption:** Decrypt data using private keys with passphrase protection
- **Key Management:** Support for both public and private key operations
- **Data Protection:** Optional integrity checking and armored output
- **Stream Support:** Efficient handling of data streams for large files
- **Compression:** Built-in support for data compression

---

## Installation

This package is hosted on GitHub Packages. To use it, add the GitHub NuGet source and authenticate with your GitHub credentials or a personal access token (PAT).

**Add the GitHub NuGet source:**

```shell
nuget source Add -Name "github" -Source "https://nuget.pkg.github.com/mtnvencenzo/index.json" -Username YOUR_GITHUB_USERNAME -Password YOUR_GITHUB_TOKEN
```

**Install the package:**

```shell
Install-Package Cezzi.Security.Pgp --source "github"
```

Or via .NET CLI:

```shell
dotnet add package Cezzi.Security.Pgp --source "github"
```

> **Note:** Replace `YOUR_GITHUB_USERNAME` and `YOUR_GITHUB_TOKEN` with your GitHub username and a personal access token with `read:packages` scope.

---

## Usage Examples

### Basic Encryption and Decryption
```csharp
using Cezzi.Security.Pgp;
using System;
using System.IO;
using System.Text;

// Create PGP service instance
var pgpService = new PgpService();

// Encrypt data using public key
byte[] publicKey = Encoding.ASCII.GetBytes("-----BEGIN PGP PUBLIC KEY BLOCK-----\n...\n-----END PGP PUBLIC KEY BLOCK-----");
byte[] dataToEncrypt = Encoding.ASCII.GetBytes("sensitive-data");
byte[] encryptedData = pgpService.Encrypt(dataToEncrypt, publicKey);

// Decrypt data using private key
string passphrase = "your-passphrase";
byte[] privateKey = Encoding.ASCII.GetBytes("-----BEGIN PGP PRIVATE KEY BLOCK-----\n...\n-----END PGP PRIVATE KEY BLOCK-----");
using var privateKeyStream = new MemoryStream(privateKey);
byte[] decryptedData = pgpService.Decrypt(encryptedData, privateKeyStream, passphrase);
```

### Advanced Encryption Options
```csharp
using Cezzi.Security.Pgp;
using Org.BouncyCastle.Bcpg.OpenPgp;

// Create PGP service instance
var pgpService = new PgpService();

// Read public key from stream
using var publicKeyStream = new MemoryStream(publicKeyBytes);
var pgpPublicKey = pgpService.ReadPublicKey(publicKeyStream);

// Encrypt with custom options
byte[] encryptedData = pgpService.Encrypt(
    inputData: dataToEncrypt,
    passphrase: pgpPublicKey,
    withIntegrityCheck: true,  // Enable integrity checking
    armor: true                // Enable ASCII armor
);
```

### Working with Files
```csharp
using Cezzi.Security.Pgp;
using System.IO;

// Create PGP service instance
var pgpService = new PgpService();

// Encrypt file
byte[] fileContent = File.ReadAllBytes("sensitive-file.txt");
byte[] encryptedFile = pgpService.Encrypt(fileContent, publicKey);
File.WriteAllBytes("sensitive-file.txt.pgp", encryptedFile);

// Decrypt file
byte[] encryptedContent = File.ReadAllBytes("sensitive-file.txt.pgp");
using var privateKeyStream = new FileStream("private-key.asc", FileMode.Open);
byte[] decryptedContent = pgpService.Decrypt(encryptedContent, privateKeyStream, passphrase);
File.WriteAllBytes("decrypted-file.txt", decryptedContent);
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
- [GitHub NuGet Package](https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Security.Pgp) 