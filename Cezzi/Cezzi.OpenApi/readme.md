# Cezzi OpenApi Framework

<p align="center">
  <img src="src/Cezzi.OpenApi/.pack/cezzi-openapi.png" alt="Cezzi OpenApi Logo" width="120" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.OpenApi"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi.OpenApi-blue?logo=github" alt="GitHub Packages"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-openapi-cicd.yaml"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-openapi-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
</p>

---

## Overview

**Cezzi OpenApi Framework** is a powerful .NET library that extends Swashbuckle.AspNetCore to provide enhanced OpenAPI/Swagger documentation capabilities. It includes features for API filtering, documentation customization, and schema management.

- **Target Framework:** .NET 9.0
- **License:** MIT
- **Author:** Ronaldo Vecchi
- **Repository:** [github.com/mtnvencenzo/cezzis-core](https://github.com/mtnvencenzo/cezzis-core)

---

## Features

- **API Filtering:** Separate API documentation based on filters and namespaces
- **Documentation Customization:** Enhanced OpenAPI documentation with custom filters
- **Schema Management:** Automatic schema filtering and customization
- **Enum Support:** Enhanced enum documentation with excluded values
- **Camel Case Query Parameters:** Automatic conversion of query parameters to camel case
- **Markdown Support:** Rich documentation with markdown descriptions

---

## Installation

This package is hosted on GitHub Packages. To use it, add the GitHub NuGet source and authenticate with your GitHub credentials or a personal access token (PAT).

**Add the GitHub NuGet source:**

```shell
nuget source Add -Name "github" -Source "https://nuget.pkg.github.com/mtnvencenzo/index.json" -Username YOUR_GITHUB_USERNAME -Password YOUR_GITHUB_TOKEN
```

**Install the package:**

```shell
Install-Package Cezzi.OpenApi --source "github"
```

Or via .NET CLI:

```shell
dotnet add package Cezzi.OpenApi --source "github"
```

> **Note:** Replace `YOUR_GITHUB_USERNAME` and `YOUR_GITHUB_TOKEN` with your GitHub username and a personal access token with `read:packages` scope.

---

## Usage Examples

### Basic Setup
```csharp
using Cezzi.OpenApi;

// In Program.cs or Startup.cs
services.UseCezziOpenApi(Configuration);

// Configure Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.UseCezziOpenApiFilters();
});
```

### Configuration
```json
{
  "OpenApiFiltering": {
    "Filters": [
      {
        "SwaggerFilter": "api1",
        "ModelNamespaces": ["MyApp.Api1.Models"],
        "IsDefault": true,
        "DescriptionFileName": "api1-description.md",
        "BaseSpecUri": "https://api1.example.com",
        "BaseApiServerUrl": "https://api1.example.com",
        "OpenApiTitle": "API 1 Documentation",
        "OpenApiDescription": "API 1 Description"
      },
      {
        "SwaggerFilter": "api2",
        "ModelNamespaces": ["MyApp.Api2.Models"],
        "DescriptionFileName": "api2-description.md",
        "BaseSpecUri": "https://api2.example.com",
        "BaseApiServerUrl": "https://api2.example.com",
        "OpenApiTitle": "API 2 Documentation",
        "OpenApiDescription": "API 2 Description"
      }
    ]
  }
}
```

### Enum Documentation
```csharp
public class MyModel
{
    [OpenApiEnum(typeof(MyEnum), "ExcludedValue")]
    public string Status { get; set; }
}

public enum MyEnum
{
    Active,
    Inactive,
    ExcludedValue
}
```

### Custom Document Request
```csharp
public class MyController : ControllerBase
{
    private readonly OpenApiDocumentRequest _documentRequest;

    public MyController(OpenApiDocumentRequest documentRequest)
    {
        _documentRequest = documentRequest;
    }

    public IActionResult GetDocumentation()
    {
        _documentRequest.ApiFilterKey = "api1";
        _documentRequest.Context = "full";
        return Ok();
    }
}
```

### Filter Helpers
```csharp
using Cezzi.OpenApi;

var filterMap = OpenApiFilterMap.FromConfiguration(configuration);
string resolvedFilter = filterMap.ResolveFilter("api1");
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
- [GitHub NuGet Package](https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget/Cezzi.OpenApi)
