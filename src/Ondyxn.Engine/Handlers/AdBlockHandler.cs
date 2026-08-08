using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Ondyxn.Engine.Handlers;

/// <summary>
/// Handles ad and tracker blocking by intercepting network requests.
/// Uses filter lists similar to uBlock Origin.
/// </summary>
public class AdBlockHandler
{
    private readonly ILogger<AdBlockHandler> _logger;
    private readonly ConcurrentBag<AdBlock.FilterList> _filterLists = [];
    private readonly HashSet<string> _blockedDomains = [];
    private int _blockedCount;

    public AdBlockHandler(ILogger<AdBlockHandler> logger)
    {
        _logger = logger;
        LoadDefaultFilters();
    }

    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Total number of requests blocked this session.
    /// </summary>
    public int BlockedCount => _blockedCount;

    /// <summary>
    /// Determines whether a request URL should be blocked.
    /// </summary>
    public bool ShouldBlock(string url)
    {
        if (!IsEnabled) return false;

        // Fast path: check domain-based blocking
        if (ShouldBlockDomain(url))
        {
            Interlocked.Increment(ref _blockedCount);
            return true;
        }

        // Slow path: check filter rules
        foreach (var list in _filterLists)
        {
            if (!list.IsEnabled) continue;

            foreach (var rule in list.Rules)
            {
                if (rule.IsDisabled || rule.IsException) continue;

                if (AdBlock.FilterListParser.MatchesUrl(url, rule))
                {
                    _logger.LogDebug("Blocked request: {Url} (rule: {Pattern})", url, rule.Pattern);
                    Interlocked.Increment(ref _blockedCount);
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Load a filter list from text content.
    /// </summary>
    public void LoadFilterList(string content, string name)
    {
        var list = AdBlock.FilterListParser.Parse(content, name);
        _filterLists.Add(list);
        _logger.LogInformation("Loaded filter list: {Name} with {Count} rules", name, list.Rules.Count);
    }

    /// <summary>
    /// Add a custom blocking pattern.
    /// </summary>
    public void AddBlockingPattern(string domain)
    {
        _blockedDomains.Add(domain.ToLowerInvariant());
    }

    /// <summary>
    /// Remove a custom blocking pattern.
    /// </summary>
    public void RemoveBlockingPattern(string domain)
    {
        _blockedDomains.Remove(domain.ToLowerInvariant());
    }

    /// <summary>
    /// Get statistics about blocked requests.
    /// </summary>
    public AdBlockStats GetStats()
    {
        return new AdBlockStats
        {
            TotalBlocked = _blockedCount,
            FilterListsLoaded = _filterLists.Count,
            CustomPatterns = _blockedDomains.Count
        };
    }

    private bool ShouldBlockDomain(string url)
    {
        try
        {
            var uri = new Uri(url);
            var host = uri.Host.ToLowerInvariant();

            // Check exact domain match
            if (_blockedDomains.Contains(host))
                return true;

            // Check parent domains
            var parts = host.Split('.');
            for (int i = 1; i < parts.Length; i++)
            {
                var domain = string.Join('.', parts[i..]);
                if (_blockedDomains.Contains(domain))
                    return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    private void LoadDefaultFilters()
    {
        // Common ad/tracker domains
        var domains = new[]
        {
            // Google Ads
            "doubleclick.net",
            "googlesyndication.com",
            "googleadservices.com",
            "googleadsserving.cn",
            "adservice.google.com",

            // Analytics
            "google-analytics.com",
            "googletagmanager.com",
            "googletagservices.com",

            // Facebook
            "facebook.com/tr",
            "connect.facebook.net",
            "facebook.net",

            // Twitter
            "analytics.twitter.com",
            "ads-twitter.com",
            "platform.twitter.com",

            // LinkedIn
            "ads.linkedin.com",
            "snap.licdn.com",

            // Common trackers
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

            // Ad networks
            "bluekai.com",
            "rubiconproject.com",
            "pubmatic.com",
            "adnxs.com",
            "casalemedia.com",
            "turn.com",
            "demdex.net",
            "everesttech.net",
            "doubleverify.com",
            "moat.com",

            // Social trackers
            "pinterest.com/ct/",
            "platform.instagram.com",
            "sc-static.net",

            // Cryptomining
            "coinhive.com",
            "coin-hive.com",
            "crypto-loot.com",

            // Malware
            "malware.com",
            "phishing.com"
        };

        foreach (var domain in domains)
        {
            _blockedDomains.Add(domain);
        }

        // Load built-in filter list
        var builtInFilters = @"
! Ondyxn Built-in Filter List
! Last updated: 2024

! Google Ads
||doubleclick.net^
||googlesyndication.com^
||googleadservices.com^
||adservice.google.com^

! Analytics
||google-analytics.com^
||googletagmanager.com^

! Facebook
||facebook.com/tr^
||connect.facebook.net^

! Twitter
||analytics.twitter.com^
||ads-twitter.com^

! Common trackers
||hotjar.com^
||mixpanel.com^
||segment.io^
||amplitude.com^
||optimizely.com^

! Ad networks
||bluekai.com^
||rubiconproject.com^
||pubmatic.com^
||adnxs.com^
||casalemedia.com^
||demdex.net^
||everesttech.net^

! Crypto mining
||coinhive.com^
||coin-hive.com^
";

        LoadFilterList(builtInFilters, "Ondyxn Built-in");
        _logger.LogInformation("Loaded {Count} blocking domains", _blockedDomains.Count);
    }
}

public class AdBlockStats
{
    public int TotalBlocked { get; set; }
    public int FilterListsLoaded { get; set; }
    public int CustomPatterns { get; set; }
}
