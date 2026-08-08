using Ondyxn.Core.Interfaces;

namespace Ondyxn.Engine.Services;

/// <summary>
/// Resolves search queries and URLs from omnibox input.
/// </summary>
public class OmniboxResolver
{
    private readonly ISettingsService? _settingsService;
    private readonly string _searchEngineTemplate;

    /// <summary>
    /// Creates a resolver that uses the current settings for search engine.
    /// </summary>
    public OmniboxResolver(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        _searchEngineTemplate = settingsService.Current.SearchEngine;
    }

    /// <summary>
    /// Creates a resolver with a fixed search engine template (for testing/fallback).
    /// </summary>
    public OmniboxResolver(string searchEngineTemplate = "https://www.google.com/search?q={0}")
    {
        _searchEngineTemplate = searchEngineTemplate;
    }

    /// <summary>
    /// Resolves the input text to a URL. If it looks like a URL, navigate to it.
    /// Otherwise, treat it as a search query.
    /// </summary>
    public string Resolve(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        input = input.Trim();

        // Already a full URL
        if (input.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            input.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return input;

        // Looks like a domain (has dot and no spaces)
        if (!input.Contains(' ') && input.Contains('.'))
            return $"https://{input}";

        // Use current search engine from settings if available
        var template = _settingsService?.Current?.SearchEngine ?? _searchEngineTemplate;
        return string.Format(template, Uri.EscapeDataString(input));
    }

    /// <summary>
    /// Checks if the input looks like a valid URL rather than a search query.
    /// </summary>
    public bool IsUrl(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        input = input.Trim();
        return input.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
               input.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
               (!input.Contains(' ') && input.Contains('.'));
    }
}
