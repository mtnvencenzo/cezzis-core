# Cezzi.Azure

<p align="center">
  <img src="https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi.Azure/Cezzi.Azure.Storage.Table/src/Cezzi.Azure.Storage.Table/.pack/cezzi-azure-storage-table.png" alt="Cezzi.Azure" width="200" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-applications-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi-blue?logo=github" alt="GitHub Packages"></a>
</p>

---

## Overview

Cezzi.Azure is a collection of .NET libraries that provide simplified interfaces for working with various Azure services. These libraries wrap the official Azure SDKs to provide a more streamlined experience while maintaining full functionality.

## Components

| Component | Description |
|-----------|-------------|
| [![Cezzi.Azure.Storage.Table](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi.Azure/Cezzi.Azure.Storage.Table/src/Cezzi.Azure.Storage.Table/.pack/cezzi-azure-storage-table.png)](Cezzi.Azure.Storage.Table/readme.md)<br>[Cezzi.Azure.Storage.Table](Cezzi.Azure.Storage.Table/readme.md) | A library for working with Azure Table Storage, providing a simplified interface for common operations like adding, updating, and retrieving entities. |
| [![Cezzi.Azure.Storage.Blob](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi.Azure/Cezzi.Azure.Storage.Blob/src/Cezzi.Azure.Storage.Blob/.pack/cezzi-azure-storage-blob.png)](Cezzi.Azure.Storage.Blob/readme.md)<br>[Cezzi.Azure.Storage.Blob](Cezzi.Azure.Storage.Blob/readme.md) | A library for working with Azure Blob Storage, providing a simplified interface for common operations like uploading, downloading, and managing blobs. |
| [![Cezzi.Azure.ServiceBus](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi.Azure/Cezzi.Azure.ServiceBus/src/Cezzi.Azure.ServiceBus/.pack/cezzi-azure-servicebus.png)](Cezzi.Azure.ServiceBus/readme.md)<br>[Cezzi.Azure.ServiceBus](Cezzi.Azure.ServiceBus/readme.md) | A library for working with Azure Service Bus, providing a simplified interface for sending and receiving messages through queues and topics. |
| [![Cezzi.Azure.KeyVault](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi.Azure/Cezzi.Azure.KeyVault/src/Cezzi.Azure.KeyVault/.pack/cezzi-azure-keyvault.png)](Cezzi.Azure.KeyVault/readme.md)<br>[Cezzi.Azure.KeyVault](Cezzi.Azure.KeyVault/readme.md) | A library for working with Azure Key Vault, providing a simplified interface for managing secrets, keys, and certificates. |

## Common Features

All components in the Cezzi.Azure collection share these common features:

- Simplified interfaces for common operations
- Full support for official Azure SDK features
- Async/await support throughout
- Proper cancellation token support
- ConfigureAwait(false) optimization
- Comprehensive error handling
- Integration with Azure Managed Identity
- Support for both development and production environments

## Installation

Each component can be installed individually via NuGet:

```shell
# Azure Storage Table
dotnet add package Cezzi.Azure.Storage.Table --source "github"

# Azure Storage Blob
dotnet add package Cezzi.Azure.Storage.Blob --source "github"

# Azure Service Bus
dotnet add package Cezzi.Azure.ServiceBus --source "github"

# Azure Key Vault
dotnet add package Cezzi.Azure.KeyVault --source "github"
```

## Best Practices

1. **Configuration**: Use Azure Key Vault for storing sensitive configuration values.

2. **Error Handling**: Implement proper error handling for all Azure service operations.

3. **Retry Policies**: Configure appropriate retry policies for each service.

4. **Monitoring**: Set up monitoring and alerting for all Azure service operations.

5. **Security**: Use Azure Managed Identity when possible for authentication.

6. **Cost Management**: Monitor and optimize resource usage to control costs.

7. **Documentation**: Keep documentation up to date with any changes to the services.

## Contributing

Contributions are welcome! Please read our [Contributing Guide](../../CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](../../LICENSE) file for details.

## Author

- **Ronaldo Vecchi** - [GitHub](https://github.com/mtnvencenzo)
