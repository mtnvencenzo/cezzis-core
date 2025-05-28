# Cezzi.ThirdParty

<p align="center">
  <img src="https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi.ThirdParty/cezzi.png" alt="Cezzi.ThirdParty" width="200" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-applications-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi-blue?logo=github" alt="GitHub Packages"></a>
</p>

---

## Overview

Cezzi.ThirdParty is a collection of .NET libraries that provide simplified interfaces for working with various third-party services. These libraries wrap the official SDKs to provide a more streamlined experience while maintaining full functionality.

## Components

| Component | Description |
|-----------|-------------|
| [![Cezzi.SendGrid](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi.ThirdParty/Cezzi.SendGrid/src/Cezzi.SendGrid/.pack/cezzi-sendgrid.png)](Cezzi.SendGrid/readme.md)<br>[Cezzi.SendGrid](Cezzi.SendGrid/readme.md) | A library for working with SendGrid's email API, providing a simplified interface for sending emails with support for HTML and plain text content, priority emails, and configurable retry policies. |
| [![Cezzi.OpenMarket](https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/Cezzi.ThirdParty/Cezzi.OpenMarket/src/Cezzi.OpenMarket/.pack/cezzi-openmarket.png)](Cezzi.OpenMarket/readme.md)<br>[Cezzi.OpenMarket](Cezzi.OpenMarket/readme.md) | A library for working with OpenMarket's SMS API, providing a simplified interface for sending SMS messages with support for multiple message types, configurable message handling, and comprehensive error handling. |

## Common Features

All components in the Cezzi.ThirdParty collection share these common features:

- Simplified interfaces for common operations
- Full support for official SDK features
- Async/await support throughout
- Proper cancellation token support
- ConfigureAwait(false) optimization
- Comprehensive error handling
- Support for both development and production environments

## Installation

Each component can be installed individually via NuGet:

```shell
# SendGrid Email Service
dotnet add package Cezzi.SendGrid --source "github"

# OpenMarket SMS Service
dotnet add package Cezzi.OpenMarket --source "github"
```

## Best Practices

1. **Credentials Management**: Store all service credentials securely, preferably in Azure Key Vault or similar.

2. **Error Handling**: Implement proper error handling for all service operations.

3. **Retry Policies**: Configure appropriate retry policies for each service.

4. **Monitoring**: Set up monitoring and alerting for all service operations.

5. **Rate Limits**: Be aware of service rate limits and implement appropriate throttling.

6. **Content Types**: Use appropriate content types and formats for each service.

7. **Documentation**: Keep documentation up to date with any changes to the services.

## Contributing

Contributions are welcome! Please read our [Contributing Guide](../../CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](../../LICENSE) file for details.

## Author

- **Ronaldo Vecchi** - [GitHub](https://github.com/mtnvencenzo)
