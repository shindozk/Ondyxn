namespace Ondyxn.Engine.AdBlock;

/// <summary>
/// Represents an adblock filter list (like EasyList).
/// </summary>
public class FilterList
{
    /// <summary>Name of the filter list.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Description of the filter list.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>URL to download the filter list from.</summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>Whether this filter list is enabled.</summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>When this filter list was last updated.</summary>
    public DateTime LastUpdated { get; set; }

    /// <summary>The filter rules loaded from this list.</summary>
    public List<FilterRule> Rules { get; set; } = [];

    /// <summary>Category of the filter list.</summary>
    public FilterListCategory Category { get; set; }
}

public enum FilterListCategory
{
    Ads,
    Privacy,
    Malware,
    Social,
    Annoyances,
    Custom
}

/// <summary>
/// A single filter rule.
/// </summary>
public class FilterRule
{
    /// <summary>The raw filter rule text.</summary>
    public string Pattern { get; set; } = string.Empty;

    /// <summary>Type of filter rule.</summary>
    public FilterRuleType Type { get; set; }

    /// <summary>Options for the rule.</summary>
    public FilterRuleOptions Options { get; set; } = new();

    /// <summary>Whether this rule is an exception rule (starts with @@).</summary>
    public bool IsException { get; set; }

    /// <summary>Whether this is a comment or disabled rule.</summary>
    public bool IsDisabled { get; set; }
}

public enum FilterRuleType
{
    /// <summary>Domain-based blocking (||example.com^).</summary>
    Domain,

    /// <summary>URL pattern matching (||example.com/path).</summary>
    UrlPattern,

    /// <summary>CSS element hiding (#selector).</summary>
    ElementHiding,

    /// <summary>Script blocking.</summary>
    Script,

    /// <summary>Image blocking.</summary>
    Image,

    /// <summary>Third-party blocking.</summary>
    ThirdParty,

    /// <summary>Subdocument/frame blocking.</summary>
    Subdocument
}

public class FilterRuleOptions
{
    public bool MatchCase { get; set; }
    public bool ThirdParty { get; set; }
    public bool Domain { get; set; }
    public bool Image { get; set; }
    public bool Script { get; set; }
    public string? Domains { get; set; }
    public string? ExcludeDomains { get; set; }
}
