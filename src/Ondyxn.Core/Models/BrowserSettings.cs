using Ondyxn.Core.Enums;

namespace Ondyxn.Core.Models;

/// <summary>
/// Browser-wide settings.
/// </summary>
public class BrowserSettings
{
    public ThemeMode Theme { get; set; } = ThemeMode.Dark;
    public string AccentColor { get; set; } = "#00D4FF";
    public bool EnableAdBlocking { get; set; } = true;
    public bool EnableTrackerBlocking { get; set; } = true;
    public bool EnableJavaScript { get; set; } = true;
    public bool EnableHardwareAcceleration { get; set; } = true;
    public bool RestoreSessionOnStartup { get; set; } = true;
    public bool ShowSidebar { get; set; } = true;
    public bool EnableTabGroups { get; set; } = true;
    public string HomePage { get; set; } = "ondyxn://newtab";
    public string SearchEngine { get; set; } = "https://www.google.com/search?q={0}";
    public int DefaultZoomLevel { get; set; } = 100;
    public bool EnableSmoothScrolling { get; set; } = true;
    public bool EnableGPUAcceleration { get; set; } = true;
    public string Language { get; set; } = "en-US";
}
