<div align="center">

# 🌐 Ondyxn

**A modern, open-source web browser built with Avalonia UI and CefGlue**

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Avalonia](https://img.shields.io/badge/Avalonia-11-purple?style=for-the-badge)](https://avaloniaui.net/)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![CEF](https://img.shields.io/badge/CEF-Chromium-4285F4?style=for-the-badge)](https://bitbucket.org/chromiumembedded/cef/)
[![License](https://img.shields.io/badge/License-MIT-blue?style=for-the-badge)](LICENSE)

<img src="docs/preview.png" alt="Ondyxn Preview" width="800"/>

*A lightweight, fast, and customizable browser with a modern interface*

</div>

---

## ✨ Features

- 🎨 **Modern UI** — Glassmorphism design with rounded corners and blur effects
- 🔖 **Tab System** — Easily manage multiple tabs
- 📑 **New Tab Page** — Customizable homepage with quick links
- 🔍 **Smart Omnibox** — Address bar with integrated search
- 📥 **Download Manager** — Track and manage your downloads
- 🔖 **Bookmarks** — Save and organize your favorite sites
- 📜 **History** — Browse through your visit history
- 🛡️ **Ad Blocking** — Built-in ad protection
- ⌨️ **Keyboard Shortcuts** — Boost productivity with Chrome-like shortcuts
- 🌙 **Private Mode** — Browse without saving data
- 📊 **DevTools** — Built-in developer tools (F12)

## 🚀 Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| `Ctrl+T` | New tab |
| `Ctrl+W` | Close tab |
| `Ctrl+Tab` | Next tab |
| `Ctrl+Shift+Tab` | Previous tab |
| `Ctrl+L` | Focus address bar |
| `Ctrl+R` | Reload page |
| `Ctrl+J` | Open downloads |
| `Ctrl+D` | Add bookmark |
| `F5` | Reload page |
| `F12` | Open DevTools |
| `Alt+←` | Go back |
| `Alt+→` | Go forward |

## 📋 Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- Windows 10/11 (macOS and Linux support in development)

## 🛠️ Building from Source

```bash
# Clone the repository
git clone https://github.com/shindozk/Ondyxn.git
cd Ondyxn

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the browser
dotnet run --project src/Ondyxn
```

## 📁 Project Structure

```
Ondyxn/
├── src/
│   ├── Ondyxn/                    # Main entry point
│   ├── Ondyxn.Core/               # Shared models, interfaces, and enums
│   ├── Ondyxn.Data/               # Data layer (SQLite + Entity Framework)
│   ├── Ondyxn.Engine/             # CEF integration (CefGlue) and browser services
│   ├── Ondyxn.UI/                 # User interface (Avalonia XAML)
│   └── Ondyxn.Tests/              # Unit tests
├── docs/                          # Documentation and images
├── Ondyxn.sln                     # Solution file
└── README.md
```

### 🏗️ Architecture

| Layer | Responsibility |
|-------|---------------|
| **Ondyxn.Core** | Data models, interfaces, and enums |
| **Ondyxn.Data** | SQLite persistence via Entity Framework |
| **Ondyxn.Engine** | CefGlue integration, network handlers, navigation |
| **Ondyxn.UI** | Avalonia interface, ViewModels, custom controls |

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run specific tests
dotnet test src/Ondyxn.Tests
```

## 🤝 Contributing

Contributions are very welcome! Follow these steps:

1. Fork the project
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

## 🔗 Useful Links

- [Avalonia UI](https://avaloniaui.net/) — Cross-platform UI framework
- [CefGlue](https://github.com/nickvdyck/cefglue) — CEF bindings for .NET
- [CefGlue.Avalonia](https://github.com/nickvdyck/cefglue) — CefGlue integration with Avalonia

---

<div align="center">

Made with ❤️ by [shindozk](https://github.com/shindozk)

</div>
