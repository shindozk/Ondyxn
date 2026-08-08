using System.Text.RegularExpressions;

namespace Ondyxn.Engine.AdBlock;

/// <summary>
/// Parses adblock filter lists in EasyList format.
/// </summary>
public static partial class FilterListParser
{
    /// <summary>
    /// Parse a filter list from text content.
    /// </summary>
    public static FilterList Parse(string content, string name = "Custom List")
    {
        var list = new FilterList
        {
            Name = name,
            LastUpdated = DateTime.UtcNow
        };

        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            // Skip empty lines and comments
            if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith('!') || trimmed.StartsWith('['))
                continue;

            // Parse exception rules (@@)
            if (trimmed.StartsWith("@@"))
            {
                var rule = ParseRule(trimmed[2..]);
                rule.IsException = true;
                list.Rules.Add(rule);
                continue;
            }

            // Parse element hiding rules (#, #@#, ##, #?#)
            if (trimmed.Contains("##") || trimmed.Contains("#@#") || trimmed.Contains("#?#"))
            {
                list.Rules.Add(new FilterRule
                {
                    Pattern = trimmed,
                    Type = FilterRuleType.ElementHiding
                });
                continue;
            }

            // Parse regular filter rules
            var filterRule = ParseRule(trimmed);
            list.Rules.Add(filterRule);
        }

        return list;
    }

    private static FilterRule ParseRule(string pattern)
    {
        var rule = new FilterRule
        {
            Pattern = pattern,
            Type = DetermineRuleType(pattern),
            Options = ParseOptions(pattern)
        };

        return rule;
    }

    private static FilterRuleType DetermineRuleType(string pattern)
    {
        // Domain blocking: ||domain.com^
        if (pattern.StartsWith("||") && pattern.EndsWith('^'))
            return FilterRuleType.Domain;

        // Script blocking
        if (pattern.Contains("script") || pattern.StartsWith("&script="))
            return FilterRuleType.Script;

        // Image blocking
        if (pattern.Contains(".jpg") || pattern.Contains(".png") || pattern.Contains(".gif") ||
            pattern.Contains(".webp") || pattern.Contains(".svg"))
            return FilterRuleType.Image;

        // Third-party
        if (pattern.Contains("$third-party"))
            return FilterRuleType.ThirdParty;

        // Subdocument
        if (pattern.Contains("$subdocument") || pattern.Contains("$frame"))
            return FilterRuleType.Subdocument;

        // Default to URL pattern
        return FilterRuleType.UrlPattern;
    }

    private static FilterRuleOptions ParseOptions(string pattern)
    {
        var options = new FilterRuleOptions();

        // Check for options after $
        var dollarIndex = pattern.LastIndexOf('$');
        if (dollarIndex >= 0)
        {
            var optionsStr = pattern[(dollarIndex + 1)..];
            var optionParts = optionsStr.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var option in optionParts)
            {
                var opt = option.Trim().ToLowerInvariant();
                switch (opt)
                {
                    case "third-party":
                    case "3p":
                        options.ThirdParty = true;
                        break;
                    case "script":
                        options.Script = true;
                        break;
                    case "image":
                        options.Image = true;
                        break;
                    case "match-case":
                    case "matchcase":
                        options.MatchCase = true;
                        break;
                }

                // Domain options: domain=example.com|~other.com
                if (opt.StartsWith("domain="))
                {
                    options.Domains = opt[7..];
                }
            }
        }

        return options;
    }

    /// <summary>
    /// Check if a URL matches a filter rule.
    /// </summary>
    public static bool MatchesUrl(string url, FilterRule rule)
    {
        if (rule.IsDisabled) return false;

        var pattern = rule.Pattern;

        // Remove options from pattern for matching
        var dollarIndex = pattern.LastIndexOf('$');
        if (dollarIndex >= 0)
        {
            pattern = pattern[..dollarIndex];
        }

        // Exception rules match differently
        if (rule.IsException)
        {
            return !MatchesPattern(url, pattern);
        }

        return MatchesPattern(url, pattern);
    }

    private static bool MatchesPattern(string url, string pattern)
    {
        // Domain blocking: ||domain.com^
        if (pattern.StartsWith("||") && pattern.EndsWith('^'))
        {
            var domain = pattern[2..^1];
            return url.Contains(domain, StringComparison.OrdinalIgnoreCase);
        }

        // Simple contains check for most patterns
        if (!pattern.Contains('*') && !pattern.Contains('^'))
        {
            return url.Contains(pattern, StringComparison.OrdinalIgnoreCase);
        }

        // Convert filter pattern to regex
        try
        {
            var regexPattern = FilterPatternToRegex(pattern);
            return Regex.IsMatch(url, regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
        catch
        {
            // Fallback to simple contains
            return url.Contains(pattern, StringComparison.OrdinalIgnoreCase);
        }
    }

    private static string FilterPatternToRegex(string pattern)
    {
        // Escape special regex characters except * and ^
        var result = Regex.Escape(pattern);

        // Convert filter syntax to regex
        result = result.Replace("\\*\\|\\|", "https?://(.*\\.)?");  // || at start
        result = result.Replace("\\^", "([\\w\\-.%]+|$)");          // ^ separator
        result = result.Replace("\\*", ".*");                        // * wildcard

        return $"^{result}$";
    }
}
