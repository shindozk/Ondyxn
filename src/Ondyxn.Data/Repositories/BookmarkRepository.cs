using Microsoft.EntityFrameworkCore;
using Ondyxn.Core.Interfaces;
using Ondyxn.Core.Models;

namespace Ondyxn.Data.Repositories;

public class BookmarkRepository : IBookmarkService
{
    private readonly IDbContextFactory<BrowserDbContext> _factory;
    private List<BookmarkModel> _cache = [];

    public IReadOnlyList<BookmarkModel> Bookmarks => _cache.AsReadOnly();

    public BookmarkRepository(IDbContextFactory<BrowserDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<BookmarkModel> AddBookmarkAsync(string title, string url, string? faviconUrl = null, string? folder = null)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var bookmark = new BookmarkModel { Title = title, Url = url, FaviconUrl = faviconUrl, Folder = folder };
        db.Bookmarks.Add(bookmark);
        await db.SaveChangesAsync();
        _cache.Add(bookmark);
        return bookmark;
    }

    public async Task RemoveBookmarkAsync(Guid id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var bookmark = await db.Bookmarks.FindAsync(id);
        if (bookmark is not null)
        {
            db.Bookmarks.Remove(bookmark);
            await db.SaveChangesAsync();
            _cache.RemoveAll(b => b.Id == id);
        }
    }

    public async Task<IReadOnlyList<BookmarkModel>> SearchBookmarksAsync(string query)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Bookmarks
            .Where(b => b.Title.Contains(query) || b.Url.Contains(query))
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<bool> IsBookmarkedAsync(string url)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Bookmarks.AnyAsync(b => b.Url == url);
    }

    public async Task ToggleBookmarkAsync(string url, string title, string? faviconUrl = null)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var existing = await db.Bookmarks.FirstOrDefaultAsync(b => b.Url == url);
        if (existing is not null)
        {
            db.Bookmarks.Remove(existing);
            _cache.RemoveAll(b => b.Url == url);
        }
        else
        {
            var bookmark = new BookmarkModel { Title = title, Url = url, FaviconUrl = faviconUrl };
            db.Bookmarks.Add(bookmark);
            _cache.Add(bookmark);
        }
        await db.SaveChangesAsync();
    }
}
