# Cezzi Core Framework

<p align="center">
  <img src="https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi/Cezzi.Applications/src/Cezzi.Applications/.pack/cezzi-applications.png" alt="Cezzi Core Framework" width="200" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-applications-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi-blue?logo=github" alt="GitHub Packages"></a>
</p>

---

## Overview

Cezzi Core Framework is a comprehensive collection of .NET libraries designed to provide robust, reusable components for building modern applications. Each component is built with best practices in mind, focusing on performance, security, and maintainability.

## Core Components

| Component | Description |
|-----------|-------------|
| [![Cezzi.Applications](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi/Cezzi.Applications/src/Cezzi.Applications/.pack/cezzi-applications.png)](Cezzi.Applications)<br>**Cezzi.Applications** | Core utilities and helpers for application development, including logging, serialization, and health monitoring. |
| [![Cezzi.Caching](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi/Cezzi.Caching/src/Cezzi.Caching/.pack/cezzi-caching.png)](Cezzi.Caching)<br>**Cezzi.Caching** | Flexible caching framework with support for multiple providers and strategies. |
| [![Cezzi.Caching.Redis](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi/Cezzi.Caching.Redis/src/Cezzi.Caching.Redis/.pack/cezzi-redis.png)](Cezzi.Caching.Redis)<br>**Cezzi.Caching.Redis** | Redis implementation for the caching framework. |
| [![Cezzi.Data](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi/Cezzi.Data/src/Cezzi.Data/.pack/cezzi-data.png)](Cezzi.Data)<br>**Cezzi.Data** | Data access layer with support for various database operations and connection management. |
| [![Cezzi.Http](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi/Cezzi.Http/src/Cezzi.Http/.pack/cezzi-http.png)](Cezzi.Http)<br>**Cezzi.Http** | HTTP client framework with service client pattern and request/response handling. |
| [![Cezzi.OpenApi](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi/Cezzi.OpenApi/src/Cezzi.OpenApi/.pack/cezzi-openapi.png)](Cezzi.OpenApi)<br>**Cezzi.OpenApi** | OpenAPI/Swagger integration with custom filters and documentation support. |
| [![Cezzi.Security](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi/Cezzi.Security/src/Cezzi.Security/.pack/cezzi-security.png)](Cezzi.Security)<br>**Cezzi.Security** | Core security utilities including encryption and hashing. |
| [![Cezzi.Security.Identity.Tokens](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi/Cezzi.Security.Identity.Tokens/src/Cezzi.Security.Identity.Tokens/.pack/cezzi-security-tok.png)](Cezzi.Security.Identity.Tokens)<br>**Cezzi.Security.Identity.Tokens** | JWT and token-based authentication support. |
| [![Cezzi.Security.Pgp](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi/Cezzi.Security.Pgp/src/Cezzi.Security.Pgp/.pack/cezzi-security-pgp.png)](Cezzi.Security.Pgp)<br>**Cezzi.Security.Pgp** | PGP encryption and decryption utilities. |
| [![Cezzi.Security.Recaptcha](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi/Cezzi.Security.Recaptcha/src/Cezzi.Security.Recaptcha/.pack/cezzi-recaptcha.png)](Cezzi.Security.Recaptcha)<br>**Cezzi.Security.Recaptcha** | Google reCAPTCHA integration. |
| [![Cezzi.Sftp](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi/Cezzi.Sftp/src/Cezzi.Sftp/.pack/cezzi-sftp.png)](Cezzi.Sftp)<br>**Cezzi.Sftp** | SFTP client framework for secure file transfers. |
| [![Cezzi.Sftp.Renci](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi/Cezzi.Sftp.Renci/src/Cezzi.Sftp.Renci/.pack/cezzi-sftp-renci.png)](Cezzi.Sftp.Renci)<br>**Cezzi.Sftp.Renci** | Renci.SshNet-based implementation of the SFTP client. |
| [![Cezzi.Smtp](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi/Cezzi.Smtp/src/Cezzi.Smtp/.pack/cezzi-smtp.png)](Cezzi.Smtp)<br>**Cezzi.Smtp** | SMTP client framework with modern async interface and factory pattern. |

## Getting Started

Each component is available as a NuGet package on GitHub Packages. To use them, add the GitHub NuGet source:

```shell
nuget source Add -Name "github" -Source "https://nuget.pkg.github.com/mtnvencenzo/index.json" -Username YOUR_GITHUB_USERNAME -Password YOUR_GITHUB_TOKEN
```

Then install the desired packages:

```shell
dotnet add package Cezzi.[ComponentName] --source "github"
```

For detailed documentation and examples, please refer to each component's readme file.

## Contributing

Contributions are welcome! Please read our [Contributing Guide](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Author

- **Ronaldo Vecchi** - [GitHub](https://github.com/mtnvencenzo)
