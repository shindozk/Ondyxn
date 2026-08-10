using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Styling;
using Microsoft.Extensions.Logging;
using Ondyxn.Core.Enums;
using Ondyxn.Core.Interfaces;

namespace Ondyxn.UI.Services;

public class ThemeManager
{
    private readonly ISettingsService _settingsService;
    private readonly ILogger<ThemeManager>? _logger;

    public ThemeManager(ISettingsService settingsService, ILogger<ThemeManager>? logger = null)
    {
        _settingsService = settingsService;
        _logger = logger;
        ApplyTheme(settingsService.Current.Theme);
    }

    public void ApplyTheme(ThemeMode theme)
    {
        if (Application.Current is null) return;

        var variant = theme switch
        {
            ThemeMode.Light => ThemeVariant.Light,
            ThemeMode.Dark => ThemeVariant.Dark,
            _ => ThemeVariant.Dark
        };

        Application.Current.RequestedThemeVariant = variant;
        _logger?.LogInformation("Applied theme: {Theme}", theme);
    }

    public void ApplyAccentColor(string hexColor)
    {
        if (string.IsNullOrEmpty(hexColor) || Application.Current is null) return;
        try
        {
            var color = Avalonia.Media.Color.Parse(hexColor);
            Application.Current.Resources["M3Primary"] = color;
            _logger?.LogInformation("Applied accent color: {Color}", hexColor);
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Invalid accent color: {Color}", hexColor);
        }
    }
}
