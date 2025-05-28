# Cezzi SFTP Framework

<p align="center">
  <img src="src/Cezzi.Sftp/.pack/cezzi-sftp.png" alt="Cezzi SFTP Logo" width="120" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Sftp"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi.Sftp-blue?logo=github" alt="GitHub Packages"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-sftp-cicd.yaml"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-sftp-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
</p>

---

## Overview

**Cezzi SFTP Framework** is a .NET library that provides a robust and feature-rich implementation for SFTP (SSH File Transfer Protocol) operations. It offers a simple interface for file transfers, directory management, and file operations over SFTP with support for both password and key-based authentication.

- **Target Framework:** .NET 9.0
- **License:** MIT
- **Author:** Ronaldo Vecchi
- **Repository:** [github.com/mtnvencenzo/cezzis-core](https://github.com/mtnvencenzo/cezzis-core)

---

## Features

- **File Operations:** Upload, download, delete, and rename files
- **Directory Management:** Create and delete directories
- **File Listing:** List directory contents with search pattern support
- **Authentication:** Support for both password and private key authentication
- **Configuration:** Flexible connection settings with timeouts and retry options
- **Error Handling:** Comprehensive error handling with detailed exception information
- **Stream Support:** Direct stream-based file operations for efficient memory usage

---

## Installation

This package is hosted on GitHub Packages. To use it, add the GitHub NuGet source and authenticate with your GitHub credentials or a personal access token (PAT).

**Add the GitHub NuGet source:**

```shell
nuget source Add -Name "github" -Source "https://nuget.pkg.github.com/mtnvencenzo/index.json" -Username YOUR_GITHUB_USERNAME -Password YOUR_GITHUB_TOKEN
```

**Install the package:**

```shell
Install-Package Cezzi.Sftp --source "github"
```

Or via .NET CLI:

```shell
dotnet add package Cezzi.Sftp --source "github"
```

> **Note:** Replace `YOUR_GITHUB_USERNAME` and `YOUR_GITHUB_TOKEN` with your GitHub username and a personal access token with `read:packages` scope.

---

## Usage Examples

### Basic Configuration
```csharp
using Cezzi.Sftp;

// Create SFTP configuration
var config = new SftpConfig
{
    Hostname = "sftp.example.com",
    Username = "user",
    Password = "password",
    Port = 22,
    ConnectTimeout = TimeSpan.FromMinutes(5),
    OperationTimeout = TimeSpan.FromHours(1),
    ConnectRetryAttempts = 3
};

// Optional: Configure private key authentication
config.PrivateKey = new SftpPrivateKey
{
    Key = "-----BEGIN RSA PRIVATE KEY-----\n...\n-----END RSA PRIVATE KEY-----",
    Passphrase = "key-passphrase"
};
```

### File Operations
```csharp
using Cezzi.Sftp;
using System.IO;

// Create SFTP client
var sftpClient = new SftpClient(config);

// Upload a file
byte[] fileContent = File.ReadAllBytes("local-file.txt");
bool uploaded = sftpClient.UploadFileFromBytes(
    contents: fileContent,
    remoteFilePath: "/remote/path/file.txt",
    overwriteIfExists: true
);

// Upload from stream
using (var stream = File.OpenRead("large-file.txt"))
{
    bool uploaded = sftpClient.UploadFileFromStream(
        stream: stream,
        remoteFilePath: "/remote/path/large-file.txt"
    );
}

// Download a file
byte[] downloadedContent = sftpClient.GetFile("/remote/path/file.txt");
string fileText = sftpClient.GetFileText("/remote/path/file.txt");

// Delete a file
bool deleted = sftpClient.DeleteFile("/remote/path/file.txt");

// Rename a file
bool renamed = sftpClient.RenameFile(
    oldFileName: "/remote/path/old-name.txt",
    newFileName: "/remote/path/new-name.txt"
);
```

### Directory Operations
```csharp
using Cezzi.Sftp;

// Create directory
bool created = sftpClient.CreateDirectory("/remote/path/new-directory");

// Delete directory
bool deleted = sftpClient.DeleteDirectory("/remote/path/old-directory");

// List directory contents
var options = new SftpFileListOptions
{
    SearchPattern = "*.txt"  // Optional: filter files by pattern
};

var files = sftpClient.ListDirectory("/remote/path", options);
foreach (var file in files)
{
    Console.WriteLine($"Name: {file.Name}");
    Console.WriteLine($"Full Path: {file.FullName}");
    Console.WriteLine($"Size: {file.Length} bytes");
    Console.WriteLine($"Last Modified: {file.LastWriteTime}");
    Console.WriteLine($"Is Directory: {file.IsDirectory}");
}
```

### Batch Operations
```csharp
using Cezzi.Sftp;
using System.Collections.Generic;

// Upload multiple files
var fileContents = new List<byte[]>
{
    File.ReadAllBytes("file1.txt"),
    File.ReadAllBytes("file2.txt")
};

bool uploaded = sftpClient.UploadFilesFromBytes(
    contents: fileContents,
    remoteFilePath: "/remote/path/",
    overwriteIfExists: true
);

// Upload multiple files from streams
var streams = new List<Stream>
{
    File.OpenRead("file1.txt"),
    File.OpenRead("file2.txt")
};

bool uploaded = sftpClient.UploadFilesFromStreams(
    streams: streams,
    remoteFilePath: "/remote/path/",
    overwriteIfExists: true
);
```

### Error Handling
```csharp
using Cezzi.Sftp;

try
{
    // SFTP operations
    sftpClient.UploadFileFromBytes(content, remotePath);
}
catch (SftpException ex)
{
    // Handle SFTP-specific exceptions
    Console.WriteLine($"SFTP Error: {ex.Message}");
    
    // Access additional error information
    foreach (DictionaryEntry entry in ex.Data)
    {
        Console.WriteLine($"{entry.Key}: {entry.Value}");
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
- [GitHub NuGet Package](https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Sftp)
