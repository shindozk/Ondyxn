using System.Collections.ObjectModel;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Ondyxn.Core.Enums;
using Ondyxn.Core.Interfaces;
using Ondyxn.Core.Models;
using Ondyxn.Engine;

namespace Ondyxn.UI.ViewModels;

/// <summary>
/// Main ViewModel for the browser window. Orchestrates tabs, navigation, and UI state.
/// Backed by real CEF browser instances via CefBootstrap and BrowserInstance.
/// </summary>
public partial class BrowserViewModel : ObservableObject
{
    private readonly IBookmarkService _bookmarkService;
    private readonly IHistoryService _historyService;
    private readonly IDownloadService _downloadService;
    private readonly ISettingsService _settingsService;
    private readonly Engine.Services.FaviconService _faviconService;
    private readonly Engine.Services.FaviconLetterService _faviconLetterService;
    private readonly CefBootstrap _cefBootstrap;
    private readonly ILogger<BrowserViewModel> _logger;

    [ObservableProperty] private TabViewModel? _activeTab;
    [ObservableProperty] private string _omniboxText = string.Empty;
    [ObservableProperty] private bool _isPrivateMode;
    [ObservableProperty] private bool _isSidebarVisible = false;
    [ObservableProperty] private bool _isCommandPaletteOpen;
    [ObservableProperty] private int _selectedSidebarIndex;
    [ObservableProperty] private string _statusText = "Ready";
    [ObservableProperty] private bool _isDownloading;
    [ObservableProperty] private BrowserSettings _settings;
    [ObservableProperty] private bool _isNewTabPage = true;
    [ObservableProperty] private bool _isBrowserVisible = false;
    [ObservableProperty] private bool _isSettingsVisible = false;
    [ObservableProperty] private bool _isLoading = false;
    [ObservableProperty] private int _blockedAdsCount = 0;
    [ObservableProperty] private int _blockedTrackersCount = 0;
    [ObservableProperty] private string _greeting = string.Empty;
    [ObservableProperty] private string _dateText = string.Empty;
    [ObservableProperty] private NewTabViewModel? _newTabViewModel;
    [ObservableProperty] private DownloadViewModel? _downloadViewModel;
    [ObservableProperty] private CommandPaletteViewModel? _commandPaletteViewModel;
    [ObservableProperty] private ThemeMode _currentTheme = ThemeMode.Dark;

    public ObservableCollection<TabViewModel> Tabs { get; } = [];

    private readonly Engine.Services.OmniboxResolver _omniboxResolver;

    private readonly ISessionService _sessionService;
    private readonly Engine.Handlers.AdBlockHandler _adBlockHandler;

    public BrowserViewModel(
        IBookmarkService bookmarkService,
        IHistoryService historyService,
        IDownloadService downloadService,
        ISettingsService settingsService,
        ISessionService sessionService,
        Engine.Services.OmniboxResolver omniboxResolver,
        Engine.Services.FaviconService faviconService,
        Engine.Services.FaviconLetterService faviconLetterService,
        CefBootstrap cefBootstrap,
        Engine.Handlers.AdBlockHandler adBlockHandler,
        ILogger<BrowserViewModel> logger)
    {
        _bookmarkService = bookmarkService;
        _historyService = historyService;
        _downloadService = downloadService;
        _settingsService = settingsService;
        _sessionService = sessionService;
        _omniboxResolver = omniboxResolver;
        _faviconService = faviconService;
        _faviconLetterService = faviconLetterService;
        _cefBootstrap = cefBootstrap;
        _adBlockHandler = adBlockHandler;
        _logger = logger;
        _settings = settingsService.Current;
        UpdateGreeting();
        
        // Initialize NewTabViewModel
        NewTabViewModel = new NewTabViewModel(settingsService, bookmarkService, historyService, faviconLetterService);

        // Initialize DownloadViewModel
        DownloadViewModel = new DownloadViewModel(_logger as ILogger<DownloadViewModel>);

        // Initialize CommandPaletteViewModel
        CommandPaletteViewModel = new CommandPaletteViewModel(this);

        // Apply theme from settings
        CurrentTheme = _settings.Theme;

        // Restore session on startup if enabled
        _ = RestoreSessionAsync();
    }

    private async Task RestoreSessionAsync()
    {
        try
        {
            if (_settings.RestoreSessionOnStartup)
            {
                var sessions = await _sessionService.GetSavedSessionsAsync();
                var lastSession = sessions.FirstOrDefault();
                if (lastSession is not null && lastSession.Tabs.Count > 0)
                {
                    foreach (var tabSnapshot in lastSession.Tabs.OrderBy(t => t.Order))
                    {
                        CreateNewTab(tabSnapshot.Url);
                    }
                    // Close the initial new tab if we restored tabs
                    if (Tabs.Count > 1 && Tabs[0].Url == "ondyxn://newtab")
                    {
                        CloseTab(Tabs[0]);
                    }
                    _logger.LogInformation("Session restored with {Count} tabs", lastSession.Tabs.Count);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to restore session");
        }

        // Default: create new tab
        CreateNewTab("ondyxn://newtab");
    }

    public async Task SaveSessionAsync()
    {
        try
        {
            var session = new Ondyxn.Core.Models.SessionModel
            {
                Name = "Last Session",
                IsPrivate = IsPrivateMode,
                Tabs = Tabs.Select((tab, index) => new Ondyxn.Core.Models.TabSnapshot
                {
                    Url = tab.Url,
                    Title = tab.Title,
                    Order = index,
                    IsPinned = tab.IsPinned
                }).ToList()
            };
            await _sessionService.SaveSessionAsync(session);
            _logger.LogInformation("Session saved with {Count} tabs", Tabs.Count);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to save session");
        }
    }

    [RelayCommand]
    private void CreateNewTab(string? url = null)
    {
        var targetUrl = url ?? "ondyxn://newtab";
        _logger.LogInformation("Creating new tab: {Url}", targetUrl);

        var browserInstance = new BrowserInstance(
            targetUrl,
            IsPrivateMode,
            _logger as ILogger<BrowserInstance>,
            _adBlockHandler);
        _logger.LogInformation("BrowserInstance created for: {Url}", targetUrl);

        var tabVm = new TabViewModel(browserInstance);
        tabVm.TitleChanged += (_, title) =>
        {
            Dispatcher.UIThread.Post(() => tabVm.Title = title);
        };
        tabVm.UrlChanged += (_, url) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                tabVm.Url = url;
                if (tabVm == ActiveTab)
                {
                    OmniboxText = url;
                    IsNewTabPage = url.StartsWith("ondyxn://newtab");
                }
            });
        };
        tabVm.LoadingStarted += (_, url) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                tabVm.IsLoading = true;
                if (tabVm == ActiveTab)
                {
                    StatusText = "Loading...";
                    IsLoading = true;
                }
            });
        };
        tabVm.LoadingFinished += (_, url) =>
        {
            Dispatcher.UIThread.Post(async () =>
            {
                tabVm.IsLoading = false;
                if (tabVm == ActiveTab)
                {
                    StatusText = "Ready";
                    IsLoading = false;
                }
                await _historyService.RecordVisitAsync(url, tabVm.Title);
                // Fetch favicon for the loaded page
                if (!string.IsNullOrEmpty(url) && !url.StartsWith("ondyxn://"))
                {
                    tabVm.FaviconUrl = _faviconService.GetFaviconUrl(url);
                }
            });
        };

        Tabs.Add(tabVm);
        ActiveTab = tabVm;
        OmniboxText = targetUrl;
        IsBrowserVisible = !IsNewTabPage;
    }

    [RelayCommand]
    private void CloseTab(TabViewModel? tab)
    {
        if (tab is null) return;
        _logger.LogInformation("Closing tab: {Title}", tab.Title);

        var index = Tabs.IndexOf(tab);
        tab.Dispose();
        Tabs.Remove(tab);

        if (Tabs.Count == 0)
        {
            CreateNewTab();
            return;
        }

        if (ActiveTab == tab)
        {
            var newIndex = Math.Min(index, Tabs.Count - 1);
            ActiveTab = Tabs[newIndex];
        }
    }

    [RelayCommand]
    private void Navigate()
    {
        if (ActiveTab is null) return;

        var url = _omniboxResolver.Resolve(OmniboxText);
        _logger.LogInformation("Navigating to: {Url}", url);
        
        // Immediately update UI state
        IsNewTabPage = false;
        IsBrowserVisible = true;
        StatusText = "Loading...";
        OmniboxText = url;
        ActiveTab.Title = url.Contains("://") ? new Uri(url).Host : url;
        
        // Navigate the browser
        ActiveTab.Navigate(url);
    }

    [RelayCommand]
    private void GoBack()
    {
        ActiveTab?.GoBack();
    }

    [RelayCommand]
    private void GoForward()
    {
        ActiveTab?.GoForward();
    }

    [RelayCommand]
    private void Reload()
    {
        ActiveTab?.Reload();
    }

    [RelayCommand]
    private void Stop()
    {
        ActiveTab?.Stop();
    }

    [RelayCommand]
    private async Task ToggleBookmark()
    {
        if (ActiveTab is null) return;
        await _bookmarkService.ToggleBookmarkAsync(ActiveTab.Url, ActiveTab.Title, ActiveTab.FaviconUrl);
    }

    [RelayCommand]
    private void TogglePrivateMode()
    {
        IsPrivateMode = !IsPrivateMode;
        StatusText = IsPrivateMode ? "Private browsing enabled" : "Private browsing disabled";
    }

    [RelayCommand]
    private void ToggleSidebar()
    {
        IsSidebarVisible = !IsSidebarVisible;
    }

    [RelayCommand]
    private void ToggleSettings()
    {
        IsSettingsVisible = !IsSettingsVisible;
    }

    [RelayCommand]
    private void ToggleCommandPalette()
    {
        CommandPaletteViewModel?.Toggle();
    }

    [RelayCommand]
    private void ApplyTheme(string? themeName)
    {
        CurrentTheme = themeName switch
        {
            "Light" => ThemeMode.Light,
            "Dark" => ThemeMode.Dark,
            _ => ThemeMode.System
        };
        _ = _settingsService.UpdateAsync(s => s.Theme = CurrentTheme);
        _logger.LogInformation("Theme changed to: {Theme}", CurrentTheme);
    }

    [RelayCommand]
    private void ToggleDownloads()
    {
        SelectedSidebarIndex = SelectedSidebarIndex == 3 ? 0 : 3;
        IsSidebarVisible = true;
    }

    [RelayCommand]
    private void ToggleBookmarks()
    {
        SelectedSidebarIndex = 1;
        IsSidebarVisible = true;
        _logger.LogInformation("Toggled bookmarks panel");
    }

    [RelayCommand]
    private void ToggleHistory()
    {
        SelectedSidebarIndex = 2;
        IsSidebarVisible = true;
        _logger.LogInformation("Toggled history panel");
    }

    [RelayCommand]
    private void ToggleCollections()
    {
        SelectedSidebarIndex = 4;
        IsSidebarVisible = true;
        _logger.LogInformation("Toggled collections panel");
    }

    [RelayCommand]
    private void GoHome()
    {
        if (ActiveTab is null) return;
        ActiveTab.Navigate(Settings.HomePage);
    }

    [RelayCommand]
    private void OpenDevTools()
    {
        ActiveTab?.OpenDevTools();
    }

    [RelayCommand]
    private void ZoomIn()
    {
        ActiveTab?.ZoomIn();
    }

    [RelayCommand]
    private void ZoomOut()
    {
        ActiveTab?.ZoomOut();
    }

    [RelayCommand]
    private void ZoomReset()
    {
        ActiveTab?.ZoomReset();
    }

    [RelayCommand]
    private void CloseCurrentTab()
    {
        CloseTab(ActiveTab);
    }

    [RelayCommand]
    private void NextTab()
    {
        if (Tabs.Count < 2) return;
        var currentIndex = Tabs.IndexOf(ActiveTab!);
        var nextIndex = (currentIndex + 1) % Tabs.Count;
        ActiveTab = Tabs[nextIndex];
    }

    [RelayCommand]
    private void PreviousTab()
    {
        if (Tabs.Count < 2) return;
        var currentIndex = Tabs.IndexOf(ActiveTab!);
        var prevIndex = (currentIndex - 1 + Tabs.Count) % Tabs.Count;
        ActiveTab = Tabs[prevIndex];
    }

    /// <summary>
    /// Move a tab from one position to another (for drag-and-drop reordering).
    /// </summary>
    public void MoveTab(int oldIndex, int newIndex)
    {
        if (oldIndex < 0 || oldIndex >= Tabs.Count || newIndex < 0 || newIndex >= Tabs.Count)
            return;
        if (oldIndex == newIndex) return;

        var tab = Tabs[oldIndex];
        Tabs.Move(oldIndex, newIndex);
        _logger.LogDebug("Tab moved from {OldIndex} to {NewIndex}", oldIndex, newIndex);
    }

    partial void OnIsNewTabPageChanged(bool value)
    {
        IsBrowserVisible = !value;
        _logger.LogDebug("IsNewTabPage changed to: {Value}", value);
    }

    partial void OnActiveTabChanged(TabViewModel? value)
    {
        if (value is not null)
        {
            OmniboxText = value.Url;
            StatusText = value.IsLoading ? "Loading..." : "Ready";
            IsNewTabPage = value.Url.StartsWith("ondyxn://newtab");
            _logger.LogDebug("Active tab changed to: {Title} ({Url})", value.Title, value.Url);
        }
    }

    private void UpdateGreeting()
    {
        var now = DateTime.Now;
        DateText = now.ToString("dddd, MMMM d");
        Greeting = now.Hour switch
        {
            < 6 => "Good night",
            < 12 => "Good morning",
            < 17 => "Good afternoon",
            _ => "Good evening"
        };
    }
}
