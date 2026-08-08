namespace Ondyxn.Core.Models;

/// <summary>
/// Represents a bookmark entry.
/// </summary>
public class BookmarkModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? FaviconUrl { get; set; }
    public string? Folder { get; set; }
    public bool IsFolder { get; set; }
    public Guid? ParentId { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastVisitedAt { get; set; }
}
