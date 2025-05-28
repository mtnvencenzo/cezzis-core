# Cezzi Applications Framework

<p align="center">
  <img src="src/Cezzi.Applications/.pack/cezzi-applications.png" alt="Cezzi Applications Logo" width="120" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Applications"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi.Applications-blue?logo=github" alt="GitHub Packages"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-applications-cicd.yaml"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-applications-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
</p>

---

## Overview

**Cezzi Applications Framework** is a robust, modular .NET library providing a suite of utilities and extensions for building high-quality, maintainable applications. It includes guard utilities, logging helpers, retry logic, serialization, validation, and a rich set of extension methods for common .NET types.

- **Target Framework:** .NET 9.0
- **License:** MIT
- **Author:** Ronaldo Vecchi
- **Repository:** [github.com/mtnvencenzo/cezzis-core](https://github.com/mtnvencenzo/cezzis-core)

---

## Features

- **Guard Utilities:** Defensive programming helpers for argument validation.
- **Logging Monikers:** Standardized property names for structured logging.
- **Serialization:** XML serialization/deserialization helpers.
- **Extensions:** Extension methods for objects, enums, tasks, and reflection.
- **Text Utilities:** Base64 encoding/decoding and code page helpers.
- **IO Utilities:** GZip compression and decompression.
- **Health Monitoring:** Health advisor interface for circuit breaker patterns.
- **Comparers:** Alphabetic string comparers.
- **Miscellaneous:** Stopwatch, alphabetical char converter.

---

## Installation

This package is hosted on GitHub Packages. To use it, add the GitHub NuGet source and authenticate with your GitHub credentials or a personal access token (PAT).

**Add the GitHub NuGet source:**

```shell
nuget source Add -Name "github" -Source "https://nuget.pkg.github.com/mtnvencenzo/index.json" -Username YOUR_GITHUB_USERNAME -Password YOUR_GITHUB_TOKEN
```

**Install the package:**

```shell
Install-Package Cezzi.Applications --source "github"
```

Or via .NET CLI:

```shell
dotnet add package Cezzi.Applications --source "github"
```

> **Note:** Replace `YOUR_GITHUB_USERNAME` and `YOUR_GITHUB_TOKEN` with your GitHub username and a personal access token with `read:packages` scope.

---

## Usage Examples

### Guard Utilities
```csharp
using Cezzi.Applications;

// Basic null checks
Guard.NotNull(someObject, nameof(someObject));
Guard.NotNullOrWhiteSpace(someString, nameof(someString));

// Numeric validations
Guard.Positive(value, nameof(value));
Guard.Negative(value, nameof(value));

// Collection validations
Guard.NotEmpty(list, nameof(list));
Guard.OneOrLess(list, nameof(list));

// Type validations
Guard.OfType<MyType>(value, nameof(value));
Guard.NotDefault(value, nameof(value));
```

### Logging Monikers
```csharp
using Cezzi.Applications.Logging;

var monikers = new AppMonikers();
logger.LogInformation($"User: {monikers.Principal}, Path: {monikers.Path}");
logger.LogInformation($"SQL: {monikers.SqlCommand}, Duration: {monikers.SqlDuration}");
```

### Serialization
```csharp
using Cezzi.Applications.Serialization;

var serializer = new XmlSerializer<MyType>();
string xml = serializer.ToXml(myObject);
var obj = serializer.FromXml(xml);
```

### Extensions
```csharp
using Cezzi.Applications.Extensions;

// Object extensions
var result = obj.Project(o => o.Property);
var types = obj.GetSubTypes();
bool isIn = value.IsIn(items);
var defaultValue = value.WhenNull(defaultValue);

// Enum extensions
var enumValue = "Value".AsEnum<MyEnum>(ignoreCase: true);
var enumValue2 = 1.AsEnum<MyEnum>();

// Task extensions
tasks.AddTask(someTask);

// Reflection extensions
bool isOverride = methodInfo.IsOverride();
bool isPropertyOverride = propertyInfo.IsOverride();
```

### Text Utilities
```csharp
using Cezzi.Applications.Text;

// Base64 encoding/decoding
string encoded = Base64.Encode(bytes);
byte[] decoded = Base64.Decode(encoded);

// Code pages
var codePage = CodePage.windows1252;
```

### IO Utilities
```csharp
using Cezzi.Applications.IO;

byte[] compressed = GZip.Compress(data);
byte[] decompressed = GZip.DeCompress(compressed);
```

### Health Monitoring
```csharp
using Cezzi.Applications.Health;

public class MyHealthAdvisor : IHealthAdvisor
{
    public bool IsHealthy()
    {
        // Implement health check logic
        return true;
    }

    public void RecordFailure()
    {
        // Record failure
    }

    public void RecordSuccess()
    {
        // Record success
    }
}
```

### Comparers
```csharp
using Cezzi.Applications.Compare;

var comparer = new AscendingAlphabeticComparer();
var sorted = list.OrderBy(x => x, comparer);
```

### Miscellaneous
```csharp
using Cezzi.Applications;

// Alphabetical char conversion
char c = AlphabeticalCharConverter.FromInteger(1); // 'A'
int i = AlphabeticalCharConverter.ToInteger('C'); // 3

// Stopwatch
var stopwatch = new StopWatch();
// ... do some work ...
long elapsedMs = stopwatch.Elapsed();
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
- [GitHub NuGet Package](https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.Applications)
