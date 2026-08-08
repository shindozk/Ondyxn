using Ondyxn.Core.Models;

namespace Ondyxn.Core.Interfaces;

/// <summary>
/// Manages bookmarks CRUD operations.
/// </summary>
public interface IBookmarkService
{
    IReadOnlyList<BookmarkModel> Bookmarks { get; }
    Task<BookmarkModel> AddBookmarkAsync(string title, string url, string? faviconUrl = null, string? folder = null);
    Task RemoveBookmarkAsync(Guid id);
    Task<IReadOnlyList<BookmarkModel>> SearchBookmarksAsync(string query);
    Task<bool> IsBookmarkedAsync(string url);
    Task ToggleBookmarkAsync(string url, string title, string? faviconUrl = null);
}
