# Cezzi.Azure.Storage.Blob

<p align="center">
  <img src="https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi.Azure/Cezzi.Azure.Storage.Blob/src/Cezzi.Azure.Storage.Blob/.pack/cezzi-azure-storage-blob.png" alt="Cezzi.Azure.Storage.Blob" width="200" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-applications-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi-blue?logo=github" alt="GitHub Packages"></a>
</p>

---

## Overview

Cezzi.Azure.Storage.Blob is a .NET library that provides a simplified interface for working with Azure Blob Storage. It wraps the Azure.Storage.Blobs SDK to provide a more streamlined experience while maintaining full functionality.

## Features

- Simplified interface for common Blob Storage operations
- Full support for Azure.Storage.Blobs SDK features
- Async/await support throughout
- Proper cancellation token support
- ConfigureAwait(false) optimization
- Support for blob versioning
- Efficient blob listing with filtering options

## Installation

```shell
dotnet add package Cezzi.Azure.Storage.Blob --source "github"
```

## Usage

### Basic Setup

```csharp
// Create a BlobContainerClient instance
var blobContainerClient = new BlobContainerClient(
    connectionString: "your_connection_string",
    containerName: "your_container_name");

// Create a proxy instance
IBlobClientProxy proxy = new BlobClientProxy();
```

### Blob Operations

```csharp
// Upload a blob
using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello, World!"));
var contentInfo = await proxy.UploadBlob(
    blobContainerClient,
    blobName: "example.txt",
    stream: stream);

// List blobs
var blobs = await proxy.GetBlobs(
    blobContainerClient,
    includeDeleted: false,
    onlyLatestVersion: true,
    prefix: "example");

// Get blob content
var content = await proxy.GetBlobContent(
    blobContainerClient,
    blobName: "example.txt");

// Delete a blob
await proxy.DeleteBlob(
    blobContainerClient,
    blobName: "example.txt");

// Rename a blob
await proxy.RenameBlob(
    blobContainerClient,
    blobName: "old-name.txt",
    newName: "new-name.txt");

// Check if blob exists
var exists = await proxy.BlobExists(
    blobContainerClient,
    blobName: "example.txt");
```

### Error Handling

```csharp
try
{
    var content = await proxy.GetBlobContent(
        blobContainerClient,
        blobName: "example.txt");
    
    // Process the content
}
catch (RequestFailedException ex)
{
    // Handle Azure Storage specific errors
    switch (ex.Status)
    {
        case 404:
            // Blob not found
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

1. **Connection Management**: Create and reuse BlobContainerClient instances rather than creating new ones for each operation.

2. **Error Handling**: Always implement proper error handling, especially for network-related operations.

3. **Cancellation**: Use cancellation tokens for long-running operations to allow proper cancellation.

4. **Stream Management**: Ensure proper disposal of streams after use.

5. **Naming Conventions**: Use consistent and meaningful blob names that reflect your application's structure.

## Contributing

Contributions are welcome! Please read our [Contributing Guide](../../CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](../../LICENSE) file for details.

## Author

- **Ronaldo Vecchi** - [GitHub](https://github.com/mtnvencenzo)
