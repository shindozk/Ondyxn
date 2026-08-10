# Ondyxn Browser

A modern, fast, and secure web browser built with **React Native Windows**, featuring **Material You** design and **Liquid Glass** effects.

[![Build](https://github.com/shindozk/Ondyxn/actions/workflows/build.yml/badge.svg)](https://github.com/shindozk/Ondyxn/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![React Native Windows](https://img.shields.io/badge/React%20Native%20Windows-0.78-blue)](https://microsoft.github.io/react-native-windows/)

## Preview

![Ondyxn Browser Preview](https://i.imgur.com/M9nYzRI.png)

## Features

- **Material You Design** - Complete M3 color system, typography, and spacing tokens
- **Liquid Glass Effects** - Glassmorphism UI with blur, transparency, and gradient overlays
- **Native Windows Rendering** - Uses Fabric renderer (Windows App SDK Composition)
- **Tab Management** - Multi-tab browsing with drag-and-drop support
- **Smart Omnibox** - Search suggestions, URL completion, and security indicators
- **Sidebar** - Bookmarks, history, and downloads management
- **Built-in Ad Blocker** - Native ad blocking with EasyList filter support
- **Security** - HTTPS enforcement, tracker blocking, site permissions
- **Session Restore** - Automatically restore tabs on startup
- **Find in Page** - Quick search with Ctrl+F
- **Zoom Controls** - Zoom in/out with Ctrl+/- or Ctrl+0 to reset
- **Developer Tools** - Built-in Chromium DevTools (F12)

## Getting Started

### Prerequisites

- [Node.js](https://nodejs.org/) >= 18
- [Visual Studio BuildTools](https://visualstudio.microsoft.com/) with C++ workload
- Windows 10/11 SDK

### Installation

```bash
# Clone the repository
git clone https://github.com/shindozk/Ondyxn.git
cd Ondyxn

# Navigate to UI project
cd src/Ondyxn.UI

# Install dependencies
npm install

# Initialize Windows native project
npx react-native init-windows

# Run the application
npm run windows
```

### Development

```bash
# Start Metro bundler
npm start

# Build for Windows (from another terminal)
npm run windows
```

## Project Structure

```
Ondyxn/
├── src/
│   ├── Ondyxn.UI/              # React Native Windows UI
│   │   ├── src/
│   │   │   ├── components/     # Reusable UI components
│   │   │   │   ├── Glass.tsx   # Liquid Glass effect component
│   │   │   │   ├── TabBar.tsx  # Browser tab strip
│   │   │   │   ├── Omnibox.tsx # Address bar with navigation
│   │   │   │   └── Sidebar.tsx # Side panel
│   │   │   ├── screens/        # Page-level components
│   │   │   │   ├── NewTabPage.tsx
│   │   │   │   └── SettingsPage.tsx
│   │   │   └── theme/          # Design system
│   │   │       ├── colors.ts   # Material You color tokens
│   │   │       └── typography.ts
│   │   └── windows/            # Native Windows code (C++)
│   ├── Ondyxn.Core/            # Core models and interfaces
│   ├── Ondyxn.Data/            # Data access (SQLite)
│   └── Ondyxn.Engine/          # Browser engine integration
├── docs/                       # Documentation
├── scripts/                    # Build scripts
├── tests/                      # Unit tests
└── README.md
```

## Tech Stack

- **React Native Windows** 0.78 - Native Windows rendering via Fabric
- **React** 19 - UI framework
- **TypeScript** - Type safety
- **react-native-linear-gradient** - Gradient effects
- **react-native-reanimated** - Smooth animations
- **react-native-gesture-handler** - Touch handling
- **Material Design 3** - Color system, typography, and components

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

## Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- [React Native Windows](https://microsoft.github.io/react-native-windows/) - Microsoft's React Native for Windows
- [Material Design 3](https://m3.material.io/) - Google's design system
- [React Native](https://reactnative.dev/) - Cross-platform mobile framework
