namespace Ondyxn.Core.Models;

/// <summary>
/// Represents a history entry.
/// </summary>
public class HistoryEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? FaviconUrl { get; set; }
    public DateTime VisitedAt { get; set; } = DateTime.UtcNow;
    public int VisitCount { get; set; } = 1;
}
