# Cezzi Caching Framework

<p align="center">
  <img src="src/Cezzi.Caching/.pack/cezzi-caching.png" alt="Cezzi Caching Logo" width="120" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Caching"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi.Caching-blue?logo=github" alt="GitHub Packages"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-caching-cicd.yaml"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-caching-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
</p>

---

## Overview

**Cezzi Caching Framework** is a flexible, extensible .NET library for application caching. It provides in-process memory caching, HTTP context caching, and a set of common interfaces and abstractions to support custom or external cache providers. Designed for performance, testability, and easy integration.

- **Target Framework:** .NET 9.0
- **License:** MIT
- **Author:** Ronaldo Vecchi
- **Repository:** [github.com/mtnvencenzo/cezzis-core](https://github.com/mtnvencenzo/cezzis-core)

---

## Features

- **In-Process Memory Caching:** Fast, thread-safe in-memory cache providers.
- **HTTP Context Caching:** Store cache items in the current HTTP context (for web apps).
- **No-Op Provider:** Easily disable caching for testing or special scenarios.
- **Cache Factory:** Compose and manage multiple cache providers by location.
- **Strongly-Typed API:** Generic, type-safe cache operations.
- **Cache Key Management:** Flexible cache key creation with region, base key, and expiration.
- **Cache Statistics:** Track hits, misses, and other cache metrics.
- **Extensible Interfaces:** Implement your own providers for distributed or external caches.

---

## Installation

This package is hosted on GitHub Packages. To use it, add the GitHub NuGet source and authenticate with your GitHub credentials or a personal access token (PAT).

**Add the GitHub NuGet source:**

```shell
nuget source Add -Name "github" -Source "https://nuget.pkg.github.com/mtnvencenzo/index.json" -Username YOUR_GITHUB_USERNAME -Password YOUR_GITHUB_TOKEN
```

**Install the package:**

```shell
Install-Package Cezzi.Caching --source "github"
```

Or via .NET CLI:

```shell
dotnet add package Cezzi.Caching --source "github"
```

> **Note:** Replace `YOUR_GITHUB_USERNAME` and `YOUR_GITHUB_TOKEN` with your GitHub username and a personal access token with `read:packages` scope.

---

## Usage Examples

### Basic In-Process Caching
```csharp
using Cezzi.Caching;
using Cezzi.Caching.Core;

// Create a cache provider (you'll need to implement InProcCacheProvideBase)
public class MyInProcCacheProvider : InProcCacheProvideBase
{
    public override CacheLocation Location => CacheLocation.InProcess;
    
    protected override (Dictionary<string, CacheItem> cache, int getHitCount, int getMissCount, int putCount, int hitCount, int missCount, int deleteHitCount, int deleteMissCount, int expiredHitCount, int serializationFailureCount) GetCacheData()
    {
        // Implement your cache storage logic here
        return (new Dictionary<string, CacheItem>(), 0, 0, 0, 0, 0, 0, 0, 0, 0);
    }
}

var cache = new MyInProcCacheProvider();
var key = new CacheKey("users", "user:123", expirationSeconds: 300);

// Put an item in the cache
var putResult = cache.Put(key, new User { Id = 123, Name = "Alice" });
if (putResult.Result.HasFlag(CacheResult.Put))
{
    // Item was successfully cached
}

// Get an item from the cache
var result = cache.Get<User>(key);
if (result.IsHit)
{
    var user = result.Object;
}
```

### Get or Put (Lazy Loading)
```csharp
var result = cache.GetOrPut(key, () => LoadUserFromDb(123));
if (result.IsHit)
{
    var user = result.Object;
}
```

### Using the No-Op Provider
```csharp
using Cezzi.Caching.Core;

var noCache = new DefaultNoCacheProvider();
var result = noCache.Put(key, "value"); // No caching will occur
```

### HTTP Context Caching (for ASP.NET Core)
```csharp
using Cezzi.Caching.Core;

var httpCache = new HttpContextCacheProvider(() => HttpContext.Items);
var result = httpCache.Put(key, "session-value");
```

### Using the Cache Factory
```csharp
using Cezzi.Caching.Core;

var factory = new DefaultCacheFactory()
    .AddProvider(CacheLocation.InProcess, new MyInProcCacheProvider())
    .AddProvider(CacheLocation.None, new DefaultNoCacheProvider());

var provider = factory.GetProvider(CacheLocation.InProcess);
```

### Cache Key Creation
```csharp
using Cezzi.Caching;

// Create a key with region and base key
var key = new CacheKey("products", "product:456");

// Create a key with expiration
var keyWithExpiration = new CacheKey("products", "product:456", expirationSeconds: 600);
```

### Checking Cache Statistics
```csharp
var stats = cache.GetStats();
if (stats.IsHit)
{
    foreach (var stat in stats.Statistics)
    {
        Console.WriteLine($"{stat.Name}: {stat.Value}");
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
- [GitHub NuGet Package](https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Caching)
- [Build Status](https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-caching-cicd.yaml)
