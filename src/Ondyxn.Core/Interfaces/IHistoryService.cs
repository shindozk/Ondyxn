using Ondyxn.Core.Models;

namespace Ondyxn.Core.Interfaces;

/// <summary>
/// Manages browser history.
/// </summary>
public interface IHistoryService
{
    Task RecordVisitAsync(string url, string title, string? faviconUrl = null);
    Task<IReadOnlyList<HistoryEntry>> SearchHistoryAsync(string query, int maxResults = 50);
    Task<IReadOnlyList<HistoryEntry>> GetRecentHistoryAsync(int maxEntries = 100);
    Task ClearHistoryAsync(DateTime? olderThan = null);
}
