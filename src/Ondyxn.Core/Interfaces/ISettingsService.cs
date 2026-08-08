using Ondyxn.Core.Models;

namespace Ondyxn.Core.Interfaces;

/// <summary>
/// Persists and loads browser settings.
/// </summary>
public interface ISettingsService
{
    BrowserSettings Current { get; }
    Task LoadAsync();
    Task SaveAsync();
    Task UpdateAsync(Action<BrowserSettings> update);
}
