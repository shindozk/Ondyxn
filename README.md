# Ondyxn Browser

A modern, fast, and secure web browser built with Avalonia UI and CefGlue.

[![Build](https://github.com/shindozk/Ondyxn/actions/workflows/build.yml/badge.svg)](https://github.com/shindozk/Ondyxn/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET 10](https://img.shields.io/badge/.NET-10.0-purple)](https://dotnet.microsoft.com/download/dotnet/10.0)

## Features

- **Modern UI** - Clean, glass-morphism design with smooth animations
- **Tab Management** - Drag-and-drop reordering, tab groups, pin tabs
- **Ad Blocking** - Native ad blocker with EasyList filter support
- **Security** - HTTPS enforcement, tracker blocking, site permissions
- **Session Restore** - Automatically restore tabs on startup
- **Find in Page** - Quick search with Ctrl+F
- **Zoom Controls** - Zoom in/out with Ctrl+/- or Ctrl+0 to reset
- **Developer Tools** - Built-in Chromium DevTools (F12)

## Preview

![Ondyxn Browser Preview](https://i.imgur.com/M9nYzRI.png)

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- Windows 10/11, macOS, or Linux

### Building from Source

```bash
# Clone the repository
git clone https://github.com/shindozk/Ondyxn.git
cd Ondyxn

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the application
dotnet run --project src/Ondyxn
```

### Using Build Scripts

```bash
# Windows (PowerShell)
.\scripts\build.ps1

# Linux/macOS
./scripts/build.sh
```

## Project Structure

```
Ondyxn/
├── .github/                 # GitHub workflows and templates
│   ├── workflows/           # CI/CD workflows
│   └── ISSUE_TEMPLATE/      # Issue templates
├── assets/                  # Static assets
│   └── images/              # Image assets
├── docs/                    # Documentation
├── scripts/                 # Build and utility scripts
├── src/                     # Source code
│   ├── Ondyxn/              # Main application entry point
│   ├── Ondyxn.Core/         # Core models and interfaces
│   ├── Ondyxn.Data/         # Data access layer (EF Core + SQLite)
│   ├── Ondyxn.Engine/       # CEF browser engine integration
│   └── Ondyxn.UI/           # Avalonia UI layer
├── tests/                   # Unit and integration tests
├── tools/                   # Development tools
├── .editorconfig            # Code style configuration
├── Directory.Build.props    # Shared project properties
├── Directory.Packages.props # Central package management
├── global.json              # .NET SDK version
├── nuget.config             # NuGet package sources
└── README.md                # This file
```

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Ctrl+T | New tab |
| Ctrl+W | Close tab |
| Ctrl+Tab | Next tab |
| Ctrl+Shift+Tab | Previous tab |
| Ctrl+L | Focus address bar |
| Ctrl+R | Reload page |
| Ctrl+F | Find in page |
| Ctrl+D | Bookmark page |
| Ctrl++ | Zoom in |
| Ctrl+- | Zoom out |
| Ctrl+0 | Reset zoom |
| F5 | Reload |
| F11 | Fullscreen |
| F12 | Developer Tools |
| Escape | Close overlays |

## Architecture

See [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) for detailed architecture documentation.

## Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- [Avalonia UI](https://avaloniaui.net/) - Cross-platform UI framework
- [CefGlue](https://github.com/nickvdyck/cef) - .NET bindings for CEF
- [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/NetCommunityToolkit) - MVVM toolkit
