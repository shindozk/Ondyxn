using Ondyxn.Core.Enums;
using Ondyxn.Core.Models;

namespace Ondyxn.Tests;

public class SettingsRepositoryTests
{
    [Fact]
    public void BrowserSettings_DefaultValues()
    {
        var settings = new BrowserSettings();
        Assert.Equal(ThemeMode.Dark, settings.Theme);
        Assert.Equal("#00D4FF", settings.AccentColor);
        Assert.True(settings.EnableAdBlocking);
        Assert.True(settings.EnableTrackerBlocking);
        Assert.True(settings.EnableJavaScript);
        Assert.True(settings.EnableHardwareAcceleration);
        Assert.True(settings.RestoreSessionOnStartup);
        Assert.True(settings.ShowSidebar);
        Assert.Equal("ondyxn://newtab", settings.HomePage);
        Assert.Equal("https://www.google.com/search?q={0}", settings.SearchEngine);
        Assert.Equal(100, settings.DefaultZoomLevel);
    }

    [Fact]
    public void BrowserSettingsEntity_FromModel_ConvertsCorrectly()
    {
        var settings = new BrowserSettings
        {
            Theme = ThemeMode.Light,
            AccentColor = "#FF0000",
            EnableAdBlocking = false,
            HomePage = "https://example.com"
        };

        var entity = BrowserSettingsEntity.FromModel(settings);

        Assert.Equal("Light", entity.Theme);
        Assert.Equal("#FF0000", entity.AccentColor);
        Assert.False(entity.EnableAdBlocking);
        Assert.Equal("https://example.com", entity.HomePage);
    }

    [Fact]
    public void BrowserSettingsEntity_ToModel_ConvertsCorrectly()
    {
        var entity = new BrowserSettingsEntity
        {
            Theme = "Dark",
            AccentColor = "#00D4FF",
            EnableAdBlocking = true,
            HomePage = "ondyxn://newtab"
        };

        var model = entity.ToModel();

        Assert.Equal(ThemeMode.Dark, model.Theme);
        Assert.Equal("#00D4FF", model.AccentColor);
        Assert.True(model.EnableAdBlocking);
        Assert.Equal("ondyxn://newtab", model.HomePage);
    }

    [Fact]
    public void BrowserSettingsEntity_RoundTrip_PreservesValues()
    {
        var original = new BrowserSettings
        {
            Theme = ThemeMode.System,
            AccentColor = "#7C3AED",
            EnableAdBlocking = false,
            EnableTrackerBlocking = false,
            EnableJavaScript = false,
            EnableHardwareAcceleration = false,
            RestoreSessionOnStartup = false,
            ShowSidebar = false,
            EnableTabGroups = false,
            HomePage = "https://custom.com",
            SearchEngine = "https://duckduckgo.com/?q={0}",
            DefaultZoomLevel = 150,
            EnableSmoothScrolling = false,
            EnableGPUAcceleration = false,
            Language = "pt-BR"
        };

        var entity = BrowserSettingsEntity.FromModel(original);
        var restored = entity.ToModel();

        Assert.Equal(original.Theme, restored.Theme);
        Assert.Equal(original.AccentColor, restored.AccentColor);
        Assert.Equal(original.EnableAdBlocking, restored.EnableAdBlocking);
        Assert.Equal(original.EnableTrackerBlocking, restored.EnableTrackerBlocking);
        Assert.Equal(original.EnableJavaScript, restored.EnableJavaScript);
        Assert.Equal(original.EnableHardwareAcceleration, restored.EnableHardwareAcceleration);
        Assert.Equal(original.RestoreSessionOnStartup, restored.RestoreSessionOnStartup);
        Assert.Equal(original.ShowSidebar, restored.ShowSidebar);
        Assert.Equal(original.EnableTabGroups, restored.EnableTabGroups);
        Assert.Equal(original.HomePage, restored.HomePage);
        Assert.Equal(original.SearchEngine, restored.SearchEngine);
        Assert.Equal(original.DefaultZoomLevel, restored.DefaultZoomLevel);
        Assert.Equal(original.EnableSmoothScrolling, restored.EnableSmoothScrolling);
        Assert.Equal(original.EnableGPUAcceleration, restored.EnableGPUAcceleration);
        Assert.Equal(original.Language, restored.Language);
    }

    [Fact]
    public void DownloadModel_Progress_CalculatesCorrectly()
    {
        var download = new DownloadModel
        {
            TotalBytes = 1000,
            ReceivedBytes = 500
        };

        Assert.Equal(50.0, download.Progress);
    }

    [Fact]
    public void DownloadModel_Progress_ZeroTotal_ReturnsZero()
    {
        var download = new DownloadModel
        {
            TotalBytes = 0,
            ReceivedBytes = 0
        };

        Assert.Equal(0.0, download.Progress);
    }

    [Fact]
    public void SessionModel_DefaultValues()
    {
        var session = new SessionModel();
        Assert.Equal("Session", session.Name);
        Assert.Empty(session.Tabs);
        Assert.False(session.IsPrivate);
    }

    [Fact]
    public void TabSnapshot_DefaultValues()
    {
        var tab = new TabSnapshot();
        Assert.Equal(string.Empty, tab.Url);
        Assert.Equal(string.Empty, tab.Title);
        Assert.False(tab.IsPinned);
        Assert.Equal(TabGroup.None, tab.Group);
    }
}
