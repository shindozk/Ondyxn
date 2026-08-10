using System.Collections.Concurrent;

namespace Ondyxn.Engine.Services;

/// <summary>
/// Generates colored letter favicons for websites based on their domain name.
/// Provides consistent colors for well-known sites.
/// </summary>
public class FaviconLetterService
{
    private static readonly ConcurrentDictionary<string, string> _colorCache = new();

    /// <summary>
    /// Gets a display letter for a URL (first letter of domain).
    /// </summary>
    public string GetLetter(string url)
    {
        var domain = ExtractDomain(url);
        if (string.IsNullOrEmpty(domain)) return "?";
        return char.ToUpper(domain[0]).ToString();
    }

    /// <summary>
    /// Gets a consistent color hex for a URL based on the domain.
    /// Known sites get their brand colors; unknown sites get a deterministic color.
    /// </summary>
    public string GetColor(string url)
    {
        var domain = ExtractDomain(url).ToLowerInvariant();
        return _colorCache.GetOrAdd(domain, ComputeColor);
    }

    private static string ExtractDomain(string url)
    {
        try
        {
            if (url.StartsWith("http://") || url.StartsWith("https://"))
                return new Uri(url).Host;
            return url.Split('/')[0].Split(':')[0];
        }
        catch
        {
            return url;
        }
    }

    private static string ComputeColor(string domain)
    {
        // Well-known brand colors
        return domain switch
        {
            "google.com" or "google" => "#4285F4",
            "youtube.com" or "youtube" => "#FF0000",
            "github.com" or "github" => "#1DB954",
            "reddit.com" or "reddit" => "#FF4500",
            "x.com" or "twitter.com" or "twitter" => "#E8ECF4",
            "facebook.com" or "facebook" => "#1877F2",
            "instagram.com" or "instagram" => "#E4405F",
            "linkedin.com" or "linkedin" => "#0A66C2",
            "stackoverflow.com" or "stackoverflow" => "#F58025",
            "wikipedia.org" or "wikipedia" => "#FFFFFF",
            "amazon.com" or "amazon" => "#FF9900",
            "netflix.com" or "netflix" => "#E50914",
            "spotify.com" or "spotify" => "#1DB954",
            "discord.com" or "discord" => "#5865F2",
            "twitch.tv" or "twitch" => "#9146FF",
            "medium.com" or "medium" => "#00AB6C",
            "gitlab.com" or "gitlab" => "#FC6D26",
            "npmjs.com" or "npm" => "#CB3837",
            "docker.com" or "docker" => "#2496ED",
            "vscode.dev" or "code.visualstudio.com" => "#007ACC",
            _ => GenerateColorFromHash(domain)
        };
    }

    private static string GenerateColorFromHash(string domain)
    {
        // Generate a consistent color from the domain name hash
        var hash = domain.GetHashCode();
        var hue = Math.Abs(hash % 360);
        return HslToHex(hue, 0.65, 0.55);
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
