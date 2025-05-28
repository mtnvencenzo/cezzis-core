# Cezzi Caching Redis Framework

<p align="center">
  <img src="src/Cezzi.Caching.Redis/.pack/cezzi-redis.png" alt="Cezzi Caching Redis Logo" width="120" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Caching.Redis"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi.Caching.Redis-blue?logo=github" alt="GitHub Packages"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-caching-redis-cicd.yaml"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-caching-redis-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
</p>

---

## Overview

**Cezzi Caching Redis Framework** is a powerful .NET library that provides a wrapper around the [StackExchange.Redis](https://stackexchange.github.io/StackExchange.Redis/) client for Redis caching. It offers a simplified interface for working with Redis while maintaining all the power and flexibility of the underlying StackExchange.Redis library.

- **Target Framework:** .NET 9.0
- **License:** MIT
- **Author:** Ronaldo Vecchi
- **Repository:** [github.com/mtnvencenzo/cezzis-core](https://github.com/mtnvencenzo/cezzis-core)

---

## Features

- **Redis Connection Management:** Simplified connection handling with automatic reconnection support
- **Configuration Options:** Flexible configuration through dependency injection
- **StackExchange.Redis Integration:** Full access to the powerful StackExchange.Redis client
- **Connection Pooling:** Efficient connection management
- **Error Handling:** Robust error handling and retry mechanisms with configurable retry attempts

---

## Installation

This package is hosted on GitHub Packages. To use it, add the GitHub NuGet source and authenticate with your GitHub credentials or a personal access token (PAT).

**Add the GitHub NuGet source:**

```shell
nuget source Add -Name "github" -Source "https://nuget.pkg.github.com/mtnvencenzo/index.json" -Username YOUR_GITHUB_USERNAME -Password YOUR_GITHUB_TOKEN
```

**Install the package:**

```shell
Install-Package Cezzi.Caching.Redis --source "github"
```

Or via .NET CLI:

```shell
dotnet add package Cezzi.Caching.Redis --source "github"
```

> **Note:** Replace `YOUR_GITHUB_USERNAME` and `YOUR_GITHUB_TOKEN` with your GitHub username and a personal access token with `read:packages` scope.

---

## Usage Examples

### Basic Configuration
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.UseStackExchangeRedisServices(sp => new RedisConfig
    {
        ConnectionString = "your-redis-connection-string",
        Password = "your-password",
        ReconnectMinFrequency = 60,
        ReconnectErrorThreshold = 30,
        RetryMaxAttempts = 5
    });
}
```

### Using Redis Connection
```csharp
public class RedisService
{
    private readonly RedisConnection _redisConnection;

    public RedisService(RedisConnection redisConnection)
    {
        _redisConnection = redisConnection;
    }

    public async Task<string> GetValueAsync(string key)
    {
        var db = _redisConnection.GetDatabase();
        return await db.StringGetAsync(key);
    }

    public async Task SetValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        var db = _redisConnection.GetDatabase();
        await db.StringSetAsync(key, value, expiry);
    }
}
```

### Using Redis with Retry Support
```csharp
public class RedisService
{
    private readonly RedisConnection _redisConnection;

    public RedisService(RedisConnection redisConnection)
    {
        _redisConnection = redisConnection;
    }

    public async Task<bool> AddToSetAsync(string key, string value)
    {
        var db = _redisConnection.GetDatabase();
        return await db.SetAddAsync(key, value);
    }

    public async Task<string[]> GetSetMembersAsync(string key)
    {
        var db = _redisConnection.GetDatabase();
        var values = await db.SetMembersAsync(key);
        return values.Select(v => v.ToString()).ToArray();
    }
}
```

### Duplicate Request Detection Example
```csharp
public class RedisDuplicateRequestDetector
{
    private readonly RedisConnection _redisConnection;

    public RedisDuplicateRequestDetector(RedisConnection redisConnection)
    {
        _redisConnection = redisConnection;
    }

    public async Task<DuplicateDetectionResult> Verify(string key, CancellationToken cancellationToken = default)
    {
        var set = await _redisConnection.GetDatabase().StringSetAsync(
            key: $"myprefix{key}",
            value: true,
            expiry: TimeSpan.FromSeconds(60),
            when: When.NotExists);

        return new DuplicateDetectionResult(!set);
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
- [GitHub NuGet Package](https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Caching.Redis)
- [Build Status](https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-caching-redis-cicd.yaml)
