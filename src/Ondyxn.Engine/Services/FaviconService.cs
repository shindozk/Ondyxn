namespace Ondyxn.Engine.Services;

/// <summary>
/// Resolves favicons for URLs using Google's favicon service and local caching.
/// </summary>
public class FaviconService
{
    private readonly Dictionary<string, string> _cache = [];

    /// <summary>
    /// Gets the favicon URL for a given page URL.
    /// </summary>
    public string GetFaviconUrl(string pageUrl)
    {
        if (_cache.TryGetValue(pageUrl, out var cached))
            return cached;

        var domain = ExtractDomain(pageUrl);
        var faviconUrl = $"https://www.google.com/s2/favicons?domain={domain}&sz=64";
        _cache[pageUrl] = faviconUrl;
        return faviconUrl;
    }

    /// <summary>
    /// Extracts the domain from a URL.
    /// </summary>
    public static string ExtractDomain(string url)
    {
        try
        {
            var uri = new Uri(url);
            return uri.Host;
        }
        catch
        {
            return url;
        }
    }
}
