# Ondyxn Browser UI

A modern browser built with **React Native Windows** featuring **Material You** design and **Liquid Glass** effects.

## Features

- 🎨 **Material You Design System** - Complete M3 color tokens, typography, and spacing
- ✨ **Liquid Glass Effects** - Glassmorphism UI with blur, transparency, and gradient overlays
- 🪟 **Native Windows Rendering** - Uses Fabric renderer (Windows App SDK Composition)
- 🔒 **Built-in Ad Blocker** - Native ad blocking with EasyList filter support
- 📑 **Tab Management** - Multi-tab browsing with drag-and-drop support
- 🔍 **Smart Omnibox** - Search suggestions, URL completion, and security indicators
- 📚 **Sidebar** - Bookmarks, history, and downloads management
- ⚙️ **Settings** - Theme, search engine, privacy, and performance settings

## Architecture

```
src/
├── components/          # Reusable UI components
│   ├── Glass.tsx       # Liquid Glass effect component
│   ├── TabBar.tsx      # Browser tab strip
│   ├── Omnibox.tsx     # Address bar with navigation
│   └── Sidebar.tsx     # Side panel (bookmarks, history, downloads)
├── screens/            # Page-level components
│   ├── NewTabPage.tsx  # New tab page with quick links
│   └── SettingsPage.tsx # Browser settings
├── theme/              # Design system
│   ├── colors.ts       # Material You color tokens
│   └── typography.ts   # Typography and spacing tokens
└── hooks/              # Custom React hooks
```

## Design Tokens

### Colors (Material You)
- **Primary**: Cyan (#06B6D4)
- **Secondary**: Purple (#8B5CF6)
- **Background**: Dark (#09090B)
- **Surface**: Elevated (#0F0F12)

### Glass Effects
- **Default**: 72% opacity, subtle border
- **Light**: 65% opacity, lighter background
- **Ultra**: 85% opacity, for overlays
- **Tab**: 60% opacity, for tab elements
- **Omnibox**: 70% opacity, for address bar

## Getting Started

### Prerequisites
- Node.js >= 18
- Visual Studio BuildTools with C++ workload
- Windows 10/11 SDK

### Installation
```bash
# Install dependencies
npm install

# Initialize Windows native project
npx react-native init-windows

# Run the app
npx react-native run-windows
```

### Development
```bash
# Start Metro bundler
npm start

# Build for Windows (from another terminal)
npm run windows
```

## Tech Stack

- **React Native Windows** 0.78 - Native Windows rendering
- **React** 19 - UI framework
- **TypeScript** - Type safety
- **react-native-linear-gradient** - Gradient effects
- **react-native-reanimated** - Animations
- **react-native-gesture-handler** - Touch handling
