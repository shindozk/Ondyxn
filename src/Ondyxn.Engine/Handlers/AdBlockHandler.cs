using Microsoft.Extensions.Logging;

namespace Ondyxn.Engine.Handlers;

/// <summary>
/// Handles ad and tracker blocking by intercepting network requests.
/// Uses filter lists similar to uBlock Origin.
/// </summary>
public class AdBlockHandler
{
    private readonly ILogger<AdBlockHandler> _logger;
    private readonly HashSet<string> _blockPatterns = [];

    public AdBlockHandler(ILogger<AdBlockHandler> logger)
    {
        _logger = logger;
        LoadDefaultFilters();
    }

    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Determines whether a request URL should be blocked.
    /// </summary>
    public bool ShouldBlock(string url)
    {
        if (!IsEnabled) return false;

        foreach (var pattern in _blockPatterns)
        {
            if (url.Contains(pattern, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogDebug("Blocked request: {Url} (matched: {Pattern})", url, pattern);
                return true;
            }
        }
        return false;
    }

    private void LoadDefaultFilters()
    {
        // Common ad/tracker domains
        _blockPatterns.UnionWith([
            "doubleclick.net",
            "googlesyndication.com",
            "google-analytics.com",
            "googletagmanager.com",
            "facebook.com/tr",
            "connect.facebook.net",
            "analytics.twitter.com",
            "ads-twitter.com",
            "ads.linkedin.com",
            "hotjar.com",
            "mixpanel.com",
            "segment.io",
            "amplitude.com",
            "optimizely.com",
            "crazyegg.com",
            "heap.io",
            "mouseflow.com",
            "clicky.com",
            "quantserve.com",
            "scorecardresearch.com",
            "bluekai.com",
            "rubiconproject.com",
            "pubmatic.com",
            "adnxs.com",
            "casalemedia.com",
            "turn.com",
            "demdex.net",
            "everesttech.net"
        ]);

        _logger.LogInformation("Loaded {Count} ad/tracker filter patterns", _blockPatterns.Count);
    }
}
