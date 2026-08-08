using System.ComponentModel.DataAnnotations;

namespace Ondyxn.Core.Models;

/// <summary>
/// EF Core entity for settings. Linked 1:1 with BrowserSettings.
/// </summary>
public class BrowserSettingsEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Theme { get; set; } = "Dark";
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

    public BrowserSettings ToModel() => new()
    {
        Theme = Enum.Parse<Enums.ThemeMode>(Theme),
        AccentColor = AccentColor,
        EnableAdBlocking = EnableAdBlocking,
        EnableTrackerBlocking = EnableTrackerBlocking,
        EnableJavaScript = EnableJavaScript,
        EnableHardwareAcceleration = EnableHardwareAcceleration,
        RestoreSessionOnStartup = RestoreSessionOnStartup,
        ShowSidebar = ShowSidebar,
        EnableTabGroups = EnableTabGroups,
        HomePage = HomePage,
        SearchEngine = SearchEngine,
        DefaultZoomLevel = DefaultZoomLevel,
        EnableSmoothScrolling = EnableSmoothScrolling,
        EnableGPUAcceleration = EnableGPUAcceleration,
        Language = Language
    };

    public static BrowserSettingsEntity FromModel(BrowserSettings s) => new()
    {
        Theme = s.Theme.ToString(),
        AccentColor = s.AccentColor,
        EnableAdBlocking = s.EnableAdBlocking,
        EnableTrackerBlocking = s.EnableTrackerBlocking,
        EnableJavaScript = s.EnableJavaScript,
        EnableHardwareAcceleration = s.EnableHardwareAcceleration,
        RestoreSessionOnStartup = s.RestoreSessionOnStartup,
        ShowSidebar = s.ShowSidebar,
        EnableTabGroups = s.EnableTabGroups,
        HomePage = s.HomePage,
        SearchEngine = s.SearchEngine,
        DefaultZoomLevel = s.DefaultZoomLevel,
        EnableSmoothScrolling = s.EnableSmoothScrolling,
        EnableGPUAcceleration = s.EnableGPUAcceleration,
        Language = s.Language
    };
}
