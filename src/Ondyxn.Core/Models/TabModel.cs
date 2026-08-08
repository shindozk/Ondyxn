namespace Ondyxn.Core.Models;

/// <summary>
/// Represents a single browser tab with its state and navigation history.
/// </summary>
public class TabModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "New Tab";
    public string Url { get; set; } = string.Empty;
    public string? FaviconUrl { get; set; }
    public bool IsLoading { get; set; }
    public bool IsPinned { get; set; }
    public bool IsMuted { get; set; }
    public Enums.TabGroup Group { get; set; } = Enums.TabGroup.None;
    public Enums.TabState State { get; set; } = Enums.TabState.Ready;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActiveAt { get; set; } = DateTime.UtcNow;
    public int Order { get; set; }
    public List<string> NavigationHistory { get; set; } = [];
    public int HistoryIndex { get; set; } = -1;

    public void PushHistory(string url)
    {
        if (HistoryIndex < NavigationHistory.Count - 1)
            NavigationHistory = NavigationHistory.Take(HistoryIndex + 1).ToList();
        NavigationHistory.Add(url);
        HistoryIndex = NavigationHistory.Count - 1;
    }

    public string? NavigateBack()
    {
        if (HistoryIndex > 0)
        {
            HistoryIndex--;
            return NavigationHistory[HistoryIndex];
        }
        return null;
    }

    public string? NavigateForward()
    {
        if (HistoryIndex < NavigationHistory.Count - 1)
        {
            HistoryIndex++;
            return NavigationHistory[HistoryIndex];
        }
        return null;
    }
}
