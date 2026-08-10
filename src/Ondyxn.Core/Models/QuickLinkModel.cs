namespace Ondyxn.Core.Models;

/// <summary>
/// Represents a quick link shortcut on the new tab page.
/// </summary>
public class QuickLinkModel
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? FaviconLetter { get; set; }
    public string? FaviconColor { get; set; }
    public string? IconKey { get; set; }
}
