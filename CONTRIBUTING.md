# Contributing to Ondyxn

Thank you for your interest in contributing to Ondyxn! This document provides guidelines and information for contributors.

## Getting Started

1. Fork the repository
2. Clone your fork
3. Create a new branch for your feature
4. Make your changes
5. Submit a pull request

## Development Setup

### Prerequisites

- .NET 10 SDK or later
- Visual Studio 2022 or VS Code with C# extension
- Git

### Building the Project

```bash
# Clone the repository
git clone https://github.com/your-username/Ondyxn.git
cd Ondyxn

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run --project src/Ondyxn
```

### Project Structure

```
Ondyxn/
├── src/
│   ├── Ondyxn/              # Main application entry point
│   ├── Ondyxn.Core/         # Core models and interfaces
│   ├── Ondyxn.Data/         # Data access layer (EF Core + SQLite)
│   ├── Ondyxn.Engine/       # CEF browser engine integration
│   └── Ondyxn.UI/           # Avalonia UI layer
├── tests/                   # Unit and integration tests
├── docs/                    # Documentation
├── scripts/                 # Build and utility scripts
├── tools/                   # Development tools
└── .github/                 # GitHub workflows and templates
```

## Code Style

- Follow C# coding conventions
- Use meaningful names for variables, methods, and classes
- Add XML documentation comments for public APIs
- Keep methods focused and concise
- Use async/await for asynchronous operations

## Pull Request Process

1. Update documentation if needed
2. Add tests for new features
3. Ensure all tests pass
4. Request review from maintainers

## Reporting Issues

- Use the issue templates provided
- Include reproduction steps
- Provide environment details
- Add screenshots if applicable

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
