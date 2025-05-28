# Cezzi HTTP Framework

<p align="center">
  <img src="src/Cezzi.Http/.pack/cezzi-http.png" alt="Cezzi HTTP Logo" width="120" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Http"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi.Http-blue?logo=github" alt="GitHub Packages"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-http-cicd.yaml"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-http-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
</p>

---

## Overview

**Cezzi HTTP Framework** is a powerful .NET library that simplifies HTTP communication in your applications. It provides a robust set of tools for making HTTP requests, handling responses, and managing HTTP-related concerns with a focus on reliability, performance, and ease of use.

- **Target Framework:** .NET 9.0
- **License:** MIT
- **Author:** Ronaldo Vecchi
- **Repository:** [github.com/mtnvencenzo/cezzis-core](https://github.com/mtnvencenzo/cezzis-core)

---

## Features

- **HTTP Client Management:** Efficient HTTP client lifecycle management and pooling
- **Request/Response Handling:** Simplified request creation and response processing
- **Retry Policies:** Configurable retry mechanisms for transient failures
- **Circuit Breaker:** Built-in circuit breaker pattern implementation
- **Request/Response Logging:** Comprehensive logging of HTTP traffic
- **Authentication:** Support for various authentication schemes
- **Content Serialization:** Automatic JSON/XML serialization and deserialization
- **Error Handling:** Robust error handling and exception management
- **Metrics Collection:** Built-in metrics for monitoring HTTP operations

---

## Installation

This package is hosted on GitHub Packages. To use it, add the GitHub NuGet source and authenticate with your GitHub credentials or a personal access token (PAT).

**Add the GitHub NuGet source:**

```shell
nuget source Add -Name "github" -Source "https://nuget.pkg.github.com/mtnvencenzo/index.json" -Username YOUR_GITHUB_USERNAME -Password YOUR_GITHUB_TOKEN
```

**Install the package:**

```shell
Install-Package Cezzi.Http --source "github"
```

Or via .NET CLI:

```shell
dotnet add package Cezzi.Http --source "github"
```

> **Note:** Replace `YOUR_GITHUB_USERNAME` and `YOUR_GITHUB_TOKEN` with your GitHub username and a personal access token with `read:packages` scope.

---

## Usage Examples

### Registering a Service Client
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.RegisterServiceClient<IUserService, UserService>((sp, client) =>
    {
        client.BaseAddress = new Uri("https://api.example.com");
        client.Timeout = TimeSpan.FromSeconds(30);
        client.DefaultRequestHeaders.Add("User-Agent", "CezziApp/1.0");
    });
}
```

### Implementing a Service Client
```csharp
public interface IUserService
{
    Task<User> GetUserAsync(int userId);
    Task<User> CreateUserAsync(User user);
}

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpClientSender _sender;

    public UserService(HttpClient httpClient, IHttpClientSender sender)
    {
        _httpClient = httpClient;
        _sender = sender;
    }

    public async Task<User> GetUserAsync(int userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"users/{userId}");
        var response = await _sender.Send(_httpClient, request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<User>();
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "users")
        {
            Content = JsonContent.Create(user)
        };
        var response = await _sender.Send(_httpClient, request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<User>();
    }
}
```

### Using the Service Client Factory
```csharp
public class UserController
{
    private readonly IServiceClientFactory<IUserService> _serviceFactory;

    public UserController(IServiceClientFactory<IUserService> serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }

    public async Task<IActionResult> GetUser(int id)
    {
        var userService = _serviceFactory.GetInstance();
        var user = await userService.GetUserAsync(id);
        return Ok(user);
    }
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
- [GitHub NuGet Package](https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Http)
- [Build Status](https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-http-cicd.yaml)
