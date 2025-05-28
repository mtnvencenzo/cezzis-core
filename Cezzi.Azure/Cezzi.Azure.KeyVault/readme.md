# Cezzi.Azure.KeyVault

<p align="center">
  <img src="https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi.Azure/Cezzi.Azure.KeyVault/src/Cezzi.Azure.KeyVault/.pack/cezzi-azure-keyvault.png" alt="Cezzi.Azure.KeyVault" width="200" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-applications-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi-blue?logo=github" alt="GitHub Packages"></a>
</p>

---

## Overview

Cezzi.Azure.KeyVault is a .NET library that provides a simplified interface for working with Azure Key Vault. It integrates with the Azure Key Vault SDK to provide a streamlined experience for managing secrets, keys, and certificates.

## Features

- Simplified interface for common Key Vault operations
- Full support for Azure Key Vault SDK features
- Async/await support throughout
- Proper cancellation token support
- ConfigureAwait(false) optimization
- Support for both secrets and certificates
- Configurable key prefix for organization
- Integration with Azure Managed Identity
- Support for both development and production environments

## Installation

```shell
dotnet add package Cezzi.Azure.KeyVault --source "github"
```

## Usage

### Basic Setup

```csharp
// Configure services
services.Configure<KeyVaultApplicationSettings>(configuration.GetSection(KeyVaultApplicationSettings.SectionName));

// Add Key Vault configuration
services.AddAzureKeyVault(configuration);

// Get settings from DI
var keyVaultSettings = serviceProvider.GetRequiredService<IOptions<KeyVaultApplicationSettings>>().Value;
```

### Configuration

```json
{
  "KeyVaultSettings": {
    "UseKeyVaultSecrets": true,
    "KeyVaultKeyPrefix": "myapp-",
    "KeyVaultBaseUri": "https://my-keyvault.vault.azure.net/"
  }
}
```

### Accessing Secrets

```csharp
// Get a secret
var secret = await keyVaultClient.GetSecretAsync("myapp-database-connection");

// Get a certificate
var certificate = await keyVaultClient.GetCertificateAsync("myapp-ssl-cert");

// Get a key
var key = await keyVaultClient.GetKeyAsync("myapp-encryption-key");
```

### Error Handling

```csharp
try
{
    var secret = await keyVaultClient.GetSecretAsync("myapp-database-connection");
    // Use the secret
}
catch (RequestFailedException ex)
{
    // Handle Azure Key Vault specific errors
    switch (ex.Status)
    {
        case 404:
            // Secret not found
            break;
        case 403:
            // Access denied
            break;
        default:
            // Handle other errors
            break;
    }
}
```

## Best Practices

1. **Secret Management**: Store all sensitive information in Key Vault, not in configuration files.

2. **Key Prefixing**: Use key prefixes to organize your secrets by application or environment.

3. **Access Control**: Implement proper access control using Azure RBAC.

4. **Error Handling**: Always implement proper error handling for Key Vault operations.

5. **Caching**: Consider caching frequently accessed secrets to reduce latency.

6. **Rotation**: Implement a process for regular secret rotation.

7. **Monitoring**: Set up monitoring and alerting for Key Vault operations.

## Contributing

Contributions are welcome! Please read our [Contributing Guide](../../CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](../../LICENSE) file for details.

## Author

- **Ronaldo Vecchi** - [GitHub](https://github.com/mtnvencenzo)
