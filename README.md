# Cezzi Core Framework

<p align="center">
  <img src="https://raw.githubusercontent.com/mtnvencenzo/cezzis-core/main/.readme/cezzi.png" alt="Cezzi Core Framework" width="200" />
</p>

<p align="center">
  <a href="https://github.com/mtnvencenzo/cezzis-core/actions"><img src="https://github.com/mtnvencenzo/cezzis-core/actions/workflows/cezzi-applications-cicd.yaml/badge.svg" alt="Build Status"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
  <a href="https://github.com/mtnvencenzo/cezzis-core/pkgs/nuget"><img src="https://img.shields.io/badge/GitHub%20Packages-Cezzi-blue?logo=github" alt="GitHub Packages"></a>
</p>

## Table of Contents

- [Overview](#overview)
- [Environment Setup](#environment-setup)
- [Core Components](#core-components)
  - [Applications](#applications)
  - [Caching](#caching)
  - [Data](#data)
  - [HTTP](#http)
  - [OpenAPI](#openapi)
  - [Security](#security)
  - [SFTP](#sftp)
  - [SMTP](#smtp)
- [Azure Integration](#azure-integration)
  - [Storage](#azure-storage)
  - [Service Bus](#azure-service-bus)
  - [Key Vault](#azure-key-vault)
- [Third-Party Services](#third-party-services)
  - [SendGrid](#sendgrid)
  - [OpenMarket](#openmarket)
- [Getting Started](#getting-started)
- [Contributing](#contributing)
- [License](#license)
- [Author](#author)

## Overview

Cezzi Core Framework is a comprehensive collection of .NET libraries designed to simplify and standardize common development tasks. The framework provides a set of well-structured, maintainable, and reusable components that follow best practices and modern development patterns.

## Environment Setup

Before getting started with the framework, please review our [Environment Setup Guide](.readme/env-setup.md) for detailed instructions on configuring your development environment, including:
- Required tools and SDKs
- Development environment configuration
- Build and test setup
- Contributing guidelines

## Core Components

### Applications
- **Cezzi.Applications**: Base library for building robust .NET applications

### Caching
- **Cezzi.Caching**: Core caching abstractions and implementations
- **Cezzi.Caching.Redis**: Redis caching integration

### Data
- **Cezzi.Data**: Data access and manipulation utilities

### HTTP
- **Cezzi.Http**: HTTP client and request handling utilities

### OpenAPI
- **Cezzi.OpenApi**: OpenAPI/Swagger integration and utilities

### Security
- **Cezzi.Security**: Security and encryption utilities
- **Cezzi.Security.Identity.Tokens**: Identity and token management
- **Cezzi.Security.Pgp**: PGP encryption support
- **Cezzi.Security.Recaptcha**: Google reCAPTCHA integration

### SFTP
- **Cezzi.Sftp**: SFTP client abstractions
- **Cezzi.Sftp.Renci**: Renci SSH.NET-based SFTP implementation

### SMTP
- **Cezzi.Smtp**: SMTP email client utilities

## Azure Integration

### Azure Storage
- **Cezzi.Azure.Storage.Table**: Azure Table Storage integration
- **Cezzi.Azure.Storage.Blob**: Azure Blob Storage integration

### Azure Service Bus
- **Cezzi.Azure.ServiceBus**: Azure Service Bus messaging integration

### Azure Key Vault
- **Cezzi.Azure.KeyVault**: Azure Key Vault secrets management

## Third-Party Services

### SendGrid
- **Cezzi.SendGrid**: Email service integration with SendGrid

### OpenMarket
- **Cezzi.OpenMarket**: SMS service integration with OpenMarket

## Getting Started

1. Choose the components you need for your project
2. Install the required NuGet packages from GitHub Packages
3. Follow the component-specific documentation for setup and usage

Example installation:
```shell
dotnet add package Cezzi.Applications --source "github"
dotnet add package Cezzi.Caching.Redis --source "github"
dotnet add package Cezzi.Security --source "github"
```

## Contributing

We welcome contributions! Please read our [Contributing Guide](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Author

- **Ronaldo Vecchi** - [GitHub](https://github.com/mtnvencenzo)
