using Ondyxn.Core.Enums;

namespace Ondyxn.Core.Models;

/// <summary>
/// Represents a browser session that can be restored.
/// </summary>
public class SessionModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "Session";
    public List<TabSnapshot> Tabs { get; set; } = [];
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    public bool IsPrivate { get; set; }
}

/// <summary>
/// A serializable snapshot of a tab for session restore.
/// </summary>
public class TabSnapshot
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SessionId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool IsPinned { get; set; }
    public Enums.TabGroup Group { get; set; }
    public int Order { get; set; }
}
