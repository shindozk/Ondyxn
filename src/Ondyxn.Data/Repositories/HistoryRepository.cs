using Microsoft.EntityFrameworkCore;
using Ondyxn.Core.Interfaces;
using Ondyxn.Core.Models;

namespace Ondyxn.Data.Repositories;

public class HistoryRepository : IHistoryService
{
    private readonly IDbContextFactory<BrowserDbContext> _factory;

    public HistoryRepository(IDbContextFactory<BrowserDbContext> factory)
    {
        _factory = factory;
    }

    public async Task RecordVisitAsync(string url, string title, string? faviconUrl = null)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var existing = await db.History.FirstOrDefaultAsync(h => h.Url == url);
        if (existing is not null)
        {
            existing.VisitCount++;
            existing.VisitedAt = DateTime.UtcNow;
        }
        else
        {
            db.History.Add(new HistoryEntry
            {
                Url = url, Title = title, FaviconUrl = faviconUrl, VisitedAt = DateTime.UtcNow
            });
        }
        await db.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<HistoryEntry>> SearchHistoryAsync(string query, int maxResults = 50)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.History
            .Where(h => h.Url.Contains(query) || h.Title.Contains(query))
            .OrderByDescending(h => h.VisitedAt)
            .Take(maxResults)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<HistoryEntry>> GetRecentHistoryAsync(int maxEntries = 100)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.History
            .OrderByDescending(h => h.VisitedAt)
            .Take(maxEntries)
            .ToListAsync();
    }

    public async Task ClearHistoryAsync(DateTime? olderThan = null)
    {
        await using var db = await _factory.CreateDbContextAsync();
        if (olderThan.HasValue)
        {
            var entries = await db.History.Where(h => h.VisitedAt < olderThan.Value).ToListAsync();
            db.History.RemoveRange(entries);
        }
        else
        {
            await db.History.ExecuteDeleteAsync();
        }
        await db.SaveChangesAsync();
    }
}
