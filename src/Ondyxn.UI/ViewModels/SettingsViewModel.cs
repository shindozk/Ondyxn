using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ondyxn.Core.Enums;
using Ondyxn.Core.Interfaces;

namespace Ondyxn.UI.ViewModels;

/// <summary>
/// ViewModel for the settings page.
/// </summary>
public partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;

    [ObservableProperty] private ThemeMode _selectedTheme;
    [ObservableProperty] private string _accentColor;
    [ObservableProperty] private bool _enableAdBlocking;
    [ObservableProperty] private bool _enableTrackerBlocking;
    [ObservableProperty] private bool _enableHardwareAcceleration;
    [ObservableProperty] private bool _restoreSessionOnStartup;
    [ObservableProperty] private bool _showSidebar;
    [ObservableProperty] private string _homePage;
    [ObservableProperty] private string _searchEngine;
    [ObservableProperty] private bool _enableSmoothScrolling;

    public IAsyncRelayCommand SaveCommand { get; }

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        var s = settingsService.Current;
        _selectedTheme = s.Theme;
        _accentColor = s.AccentColor;
        _enableAdBlocking = s.EnableAdBlocking;
        _enableTrackerBlocking = s.EnableTrackerBlocking;
        _enableHardwareAcceleration = s.EnableHardwareAcceleration;
        _restoreSessionOnStartup = s.RestoreSessionOnStartup;
        _showSidebar = s.ShowSidebar;
        _homePage = s.HomePage;
        _searchEngine = s.SearchEngine;
        _enableSmoothScrolling = s.EnableSmoothScrolling;
        SaveCommand = new AsyncRelayCommand(SaveAsync);
    }

    private async Task SaveAsync()
    {
        await _settingsService.UpdateAsync(s =>
        {
            s.Theme = SelectedTheme;
            s.AccentColor = AccentColor;
            s.EnableAdBlocking = EnableAdBlocking;
            s.EnableTrackerBlocking = EnableTrackerBlocking;
            s.EnableHardwareAcceleration = EnableHardwareAcceleration;
            s.RestoreSessionOnStartup = RestoreSessionOnStartup;
            s.ShowSidebar = ShowSidebar;
            s.HomePage = HomePage;
            s.SearchEngine = SearchEngine;
            s.EnableSmoothScrolling = EnableSmoothScrolling;
        });
    }

    public void SetSearchEngineTemplate(string template)
    {
        SearchEngine = template;
    }

    [RelayCommand]
    private void Cancel()
    {
        // Reload current settings to discard changes
        var s = _settingsService.Current;
        SelectedTheme = s.Theme;
        AccentColor = s.AccentColor;
        EnableAdBlocking = s.EnableAdBlocking;
        EnableTrackerBlocking = s.EnableTrackerBlocking;
        EnableHardwareAcceleration = s.EnableHardwareAcceleration;
        RestoreSessionOnStartup = s.RestoreSessionOnStartup;
        ShowSidebar = s.ShowSidebar;
        HomePage = s.HomePage;
        SearchEngine = s.SearchEngine;
        EnableSmoothScrolling = s.EnableSmoothScrolling;
    }
}
