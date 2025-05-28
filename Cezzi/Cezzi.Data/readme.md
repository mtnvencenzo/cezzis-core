# Cezzi Data Framework

<p align="center">
  <img src="src/Cezzi.Data/.pack/cezzi-data.png" alt="Cezzi Data Logo" width="120" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Data"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi.Data-blue?logo=github" alt="GitHub Packages"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-data-cicd.yaml"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-data-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
</p>

---

## Overview

**Cezzi Data Framework** is a comprehensive .NET library that provides robust data access and manipulation capabilities. It offers a set of tools and abstractions for working with various data sources, implementing best practices for data access patterns, and ensuring type safety throughout your application.

- **Target Framework:** .NET 9.0
- **License:** MIT
- **Author:** Ronaldo Vecchi
- **Repository:** [github.com/mtnvencenzo/cezzis-core](https://github.com/mtnvencenzo/cezzis-core)

---

## Features

- **Data Access Patterns:** Implementation of common data access patterns and best practices
- **Type Safety:** Strong typing throughout the data access layer
- **Connection Management:** Efficient connection pooling and lifecycle management
- **Extensibility:** Easy to extend and customize for specific needs

---

## Installation

This package is hosted on GitHub Packages. To use it, add the GitHub NuGet source and authenticate with your GitHub credentials or a personal access token (PAT).

**Add the GitHub NuGet source:**

```shell
nuget source Add -Name "github" -Source "https://nuget.pkg.github.com/mtnvencenzo/index.json" -Username YOUR_GITHUB_USERNAME -Password YOUR_GITHUB_TOKEN
```

**Install the package:**

```shell
Install-Package Cezzi.Data --source "github"
```

Or via .NET CLI:

```shell
dotnet add package Cezzi.Data --source "github"
```

> **Note:** Replace `YOUR_GITHUB_USERNAME` and `YOUR_GITHUB_TOKEN` with your GitHub username and a personal access token with `read:packages` scope.

---

## Usage Examples

### Basic Data Access
```csharp
public class UserRepository
{
    private readonly IDataAccessor _dataAccessor;
    private readonly IConnectionStringProvider _connectionProvider;

    public UserRepository(IDataAccessor dataAccessor, IConnectionStringProvider connectionProvider)
    {
        _dataAccessor = dataAccessor;
        _connectionProvider = connectionProvider;
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        using var connection = new SqlConnection(_connectionProvider["DefaultConnection"]);
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Users WHERE Id = @UserId";
        command.Parameters.AddWithValue("@UserId", userId);

        await _dataAccessor.OpenAsync(connection);
        using var reader = await _dataAccessor.ExecuteReaderAsync(command);
        var safeReader = new SafeDataReader(reader);

        if (safeReader.Read())
        {
            return new User
            {
                Id = safeReader.GetInt32("Id"),
                Name = safeReader.GetString("Name"),
                Email = safeReader.GetString("Email"),
                CreatedDate = safeReader.GetUTCDateTimeValueSafe("CreatedDate")
            };
        }

        return null;
    }
}
```

### Using Safe Data Reader
```csharp
public class OrderRepository
{
    private readonly IDataAccessor _dataAccessor;

    public OrderRepository(IDataAccessor dataAccessor)
    {
        _dataAccessor = dataAccessor;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        using var connection = new SqlConnection(_connectionProvider["DefaultConnection"]);
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Orders";

        await _dataAccessor.OpenAsync(connection);
        using var reader = await _dataAccessor.ExecuteReaderAsync(command);
        var safeReader = new SafeDataReader(reader);

        var orders = new List<Order>();
        while (safeReader.Read())
        {
            orders.Add(new Order
            {
                Id = safeReader.GetInt32("Id"),
                CustomerId = safeReader.GetInt32("CustomerId"),
                OrderDate = safeReader.GetUTCDateTimeValueSafe("OrderDate"),
                TotalAmount = safeReader.GetDecimal("TotalAmount"),
                Status = safeReader.GetEnum<OrderStatus>("Status")
            });
        }

        return orders;
    }
}
```

### Using Data Extensions
```csharp
public class ProductRepository
{
    private readonly IDataAccessor _dataAccessor;

    public async Task<Product> GetProductAsync(int productId)
    {
        using var connection = new SqlConnection(_connectionProvider["DefaultConnection"]);
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Products WHERE Id = @ProductId";
        command.Parameters.AddWithValue("@ProductId", productId);

        await _dataAccessor.OpenAsync(connection);
        using var reader = await _dataAccessor.ExecuteReaderAsync(command);

        if (reader.Read())
        {
            return new Product
            {
                Id = reader.GetValueSafe<int>("Id"),
                Name = reader.GetValueSafe<string>("Name"),
                Price = reader.GetValueSafe<decimal>("Price"),
                LastModified = reader.GetUTCDateTimeValueSafe("LastModified")
            };
        }

        return null;
    }
}
```

### Using Connection String Provider
```csharp
public void ConfigureServices(IServiceCollection services)
{
    var connectionProvider = new DefaultConnectionStringProvider()
        .AddConnectionString("DefaultConnection", "Server=myserver;Database=mydb;Trusted_Connection=True;")
        .AddConnectionString("ReadOnlyConnection", "Server=myserver;Database=mydb;ApplicationIntent=ReadOnly;");

    services.AddSingleton<IConnectionStringProvider>(connectionProvider);
    services.AddScoped<IDataAccessor, DataAccessor>();
}
```


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
- [GitHub NuGet Package](https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Data)
- [Build Status](https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-data-cicd.yaml)
