# Ondyxn Architecture

## Overview

Ondyxn is a modern web browser built with Avalonia UI and CefGlue (Chromium Embedded Framework). The architecture follows a clean separation of concerns with multiple layers.

## Project Layers

### 1. Ondyxn.Core
Core business logic, models, and interfaces.

- **Models**: Data transfer objects and domain models
- **Interfaces**: Service contracts
- **Enums**: Shared enumerations
- **Services**: Utility services (AppDataPaths, etc.)

### 2. Ondyxn.Data
Data access layer using Entity Framework Core with SQLite.

- **Repositories**: Service implementations
- **BrowserDbContext**: EF Core database context
- **Migrations**: Database schema migrations

### 3. Ondyxn.Engine
CEF browser engine integration.

- **BrowserInstance**: Wraps AvaloniaCefBrowser
- **Handlers**: CEF event handlers (requests, downloads, etc.)
- **AdBlock**: Native ad blocking with filter lists

### 4. Ondyxn.UI
Avalonia UI layer with MVVM pattern.

- **Views**: XAML views and windows
- **ViewModels**: MVVM ViewModels using CommunityToolkit.Mvvm
- **Controls**: Custom Avalonia controls
- **Styles**: XAML style resources

## Data Flow

```
User Action → UI (View) → ViewModel → Service → Repository → Database
                ↓
            CEF Browser → Web Content
```

## Key Components

### Browser Instance
Each tab creates a `BrowserInstance` that wraps an `AvaloniaCefBrowser` control. This allows independent browsing contexts with separate histories and states.

### Tab Management
`BrowserViewModel` manages the collection of tabs, handles tab creation, switching, and closure. It coordinates between the UI and the browser engine.

### Ad Blocking
`AdBlockHandler` intercepts network requests through CEF's request handler. It uses filter lists (EasyList format) to block ads, trackers, and malicious content.

### Data Persistence
- **Settings**: Stored in SQLite database
- **History**: Visit tracking with timestamps
- **Bookmarks**: User-saved pages with folders
- **Sessions**: Tab state for session restore

## Database Schema

```sql
-- Core tables
History          -- Browsing history
Bookmarks        -- Saved bookmarks
Downloads        -- Download history
Sessions         -- Saved sessions
TabSnapshots     -- Tab state for sessions
Settings         -- Browser settings
SitePermissions  -- Per-site permissions
```

## Security

- HTTPS enforcement
- Certificate validation
- Site permissions system
- Tracker blocking
- Malware protection

## Performance

- Lazy loading of browser instances
- Async/await for all I/O operations
- Background processing for non-UI tasks
- Efficient memory management with disposal patterns
