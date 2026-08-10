using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Ondyxn.UI.Converters;

public class BoolToBrushConverter : IValueConverter
{
    public IBrush TrueBrush { get; set; } = Brushes.White;
    public IBrush FalseBrush { get; set; } = Brushes.Transparent;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
            return b ? TrueBrush : FalseBrush;
        return FalseBrush;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public class LoadingToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // When loading is true, hide the control (use UnsetValue to not set visibility)
        // When loading is false, show the control
        if (value is bool isLoading)
            return !isLoading; // Return true when not loading (visible)
        return true;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public class EnumToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null || parameter is null) return false;
        return value.ToString() == parameter.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b && b && parameter is string s)
            return Enum.Parse(targetType, s);
        return AvaloniaProperty.UnsetValue;
    }
}

/// <summary>
/// Converts a URL string to a letter for favicon display.
/// </summary>
public class UrlToLetterConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string url && !string.IsNullOrEmpty(url))
        {
            try
            {
                var domain = new Uri(url).Host;
                return char.ToUpper(domain[0]).ToString();
            }
            catch
            {
                return "?";
            }
        }
        return "?";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

/// <summary>
/// Converts a URL string to a consistent color for favicon display.
/// </summary>
public class UrlToColorConverter : IValueConverter
{
    private static readonly Dictionary<string, string> BrandColors = new()
    {
        ["google.com"] = "#4285F4",
        ["youtube.com"] = "#FF0000",
        ["github.com"] = "#1DB954",
        ["reddit.com"] = "#FF4500",
        ["x.com"] = "#E8ECF4",
        ["twitter.com"] = "#E8ECF4",
        ["facebook.com"] = "#1877F2",
        ["instagram.com"] = "#E4405F",
        ["linkedin.com"] = "#0A66C2",
        ["stackoverflow.com"] = "#F58025",
        ["wikipedia.org"] = "#FFFFFF",
        ["amazon.com"] = "#FF9900",
        ["netflix.com"] = "#E50914",
        ["spotify.com"] = "#1DB954",
        ["discord.com"] = "#5865F2",
    };

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string url && !string.IsNullOrEmpty(url))
        {
            try
            {
                var domain = new Uri(url).Host.ToLowerInvariant();
                if (BrandColors.TryGetValue(domain, out var color))
                    return new SolidColorBrush(Color.Parse(color));

                // Generate consistent color from domain hash
                var hash = domain.GetHashCode();
                var hue = Math.Abs(hash % 360);
                var colorStr = HslToHex(hue, 0.65, 0.55);
                return new SolidColorBrush(Color.Parse(colorStr));
            }
            catch
            {
                return new SolidColorBrush(Color.Parse("#666666"));
            }
        }
        return new SolidColorBrush(Color.Parse("#666666"));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    private static string HslToHex(double h, double s, double l)
    {
        var c = (1 - Math.Abs(2 * l - 1)) * s;
        var x = c * (1 - Math.Abs(h / 60 % 2 - 1));
        var m = l - c / 2;

        double r, g, b;
        if (h < 60) { r = c; g = x; b = 0; }
        else if (h < 120) { r = x; g = c; b = 0; }
        else if (h < 180) { r = 0; g = c; b = x; }
        else if (h < 240) { r = 0; g = x; b = c; }
        else if (h < 300) { r = x; g = 0; b = c; }
        else { r = c; g = 0; b = x; }

        var ri = (int)Math.Round((r + m) * 255);
        var gi = (int)Math.Round((g + m) * 255);
        var bi = (int)Math.Round((b + m) * 255);

        return $"#{ri:X2}{gi:X2}{bi:X2}";
    }
}

/// <summary>
/// Converts a string to a Brush using Color.Parse.
/// </summary>
public class StringToBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string colorStr && !string.IsNullOrEmpty(colorStr))
        {
            try
            {
                return new SolidColorBrush(Color.Parse(colorStr));
            }
            catch
            {
                return new SolidColorBrush(Colors.Gray);
            }
        }
        return new SolidColorBrush(Colors.Gray);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
