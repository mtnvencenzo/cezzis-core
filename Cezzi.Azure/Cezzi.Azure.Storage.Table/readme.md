# Cezzi.Azure.Storage.Table

<p align="center">
  <img src="https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi.Azure/Cezzi.Azure.Storage.Table/src/Cezzi.Azure.Storage.Table/.pack/cezzi-azure-storage-table.png" alt="Cezzi.Azure.Storage.Table" width="200" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-applications-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi-blue?logo=github" alt="GitHub Packages"></a>
</p>

---

## Overview

Cezzi.Azure.Storage.Table is a .NET library that provides a simplified interface for working with Azure Table Storage. It wraps the Azure.Data.Tables SDK to provide a more streamlined experience while maintaining full functionality.

## Features

- Simplified interface for common Table Storage operations
- Full support for Azure.Data.Tables SDK features
- Async/await support throughout
- Proper cancellation token support
- ConfigureAwait(false) optimization
- Strong typing with generics
- ETag support for optimistic concurrency

## Installation

```shell
dotnet add package Cezzi.Azure.Storage.Table --source "github"
```

## Usage

### Basic Setup

```csharp
// Create a TableClient instance
var tableClient = new TableClient(
    connectionString: "your_connection_string",
    tableName: "your_table_name");

// Create a proxy instance
ITableClientProxy proxy = new TableClientProxy();
```

### Entity Operations

```csharp
// Define your entity
public class Customer : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

// Add an entity
var customer = new Customer
{
    PartitionKey = "customers",
    RowKey = "customer1",
    Name = "John Doe",
    Email = "john@example.com"
};

await proxy.AddEntityAsync(tableClient, customer);

// Get an entity
var response = await proxy.GetEntityAsync<Customer>(
    tableClient,
    partitionKey: "customers",
    rowKey: "customer1");

var retrievedCustomer = response.Value;

// Update an entity
retrievedCustomer.Name = "John Smith";
await proxy.UpdateEntityAsync(
    tableClient,
    retrievedCustomer,
    retrievedCustomer.ETag,
    TableUpdateMode.Replace);
```

### Error Handling

```csharp
try
{
    var response = await proxy.GetEntityAsync<Customer>(
        tableClient,
        partitionKey: "customers",
        rowKey: "customer1");
    
    // Process the response
}
catch (RequestFailedException ex)
{
    // Handle Azure Storage specific errors
    switch (ex.Status)
    {
        case 404:
            // Entity not found
            break;
        case 409:
            // Conflict (e.g., ETag mismatch)
            break;
        default:
            // Handle other errors
            break;
    }
}
```

## Best Practices

1. **Connection Management**: Create and reuse TableClient instances rather than creating new ones for each operation.

2. **Error Handling**: Always implement proper error handling, especially for network-related operations.

3. **Cancellation**: Use cancellation tokens for long-running operations to allow proper cancellation.

4. **ETags**: Use ETags for optimistic concurrency control when updating entities.

5. **Partitioning**: Design your partition keys carefully to ensure good performance and scalability.

## Contributing

Contributions are welcome! Please read our [Contributing Guide](../../CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](../../LICENSE) file for details.

## Author

- **Ronaldo Vecchi** - [GitHub](https://github.com/mtnvencenzo)
