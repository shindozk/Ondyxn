using Microsoft.EntityFrameworkCore;
using Ondyxn.Core.Interfaces;
using Ondyxn.Core.Models;

namespace Ondyxn.Data.Repositories;

public class SettingsRepository : ISettingsService
{
    private readonly IDbContextFactory<BrowserDbContext> _factory;
    private BrowserSettings _current = new();
    private bool _loaded;

    public BrowserSettings Current => _current;

    public SettingsRepository(IDbContextFactory<BrowserDbContext> factory)
    {
        _factory = factory;
    }

    public async Task LoadAsync()
    {
        if (_loaded) return;
        
        await using var db = await _factory.CreateDbContextAsync();
        var entity = await db.Settings.FirstOrDefaultAsync();
        if (entity is not null)
        {
            _current = entity.ToModel();
        }
        _loaded = true;
    }

    public async Task SaveAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        var existing = await db.Settings.FirstOrDefaultAsync();
        if (existing is not null)
        {
            // Update existing
            existing.Theme = _current.Theme.ToString();
            existing.AccentColor = _current.AccentColor;
            existing.EnableAdBlocking = _current.EnableAdBlocking;
            existing.EnableTrackerBlocking = _current.EnableTrackerBlocking;
            existing.EnableJavaScript = _current.EnableJavaScript;
            existing.EnableHardwareAcceleration = _current.EnableHardwareAcceleration;
            existing.RestoreSessionOnStartup = _current.RestoreSessionOnStartup;
            existing.ShowSidebar = _current.ShowSidebar;
            existing.EnableTabGroups = _current.EnableTabGroups;
            existing.HomePage = _current.HomePage;
            existing.SearchEngine = _current.SearchEngine;
            existing.DefaultZoomLevel = _current.DefaultZoomLevel;
            existing.EnableSmoothScrolling = _current.EnableSmoothScrolling;
            existing.EnableGPUAcceleration = _current.EnableGPUAcceleration;
            existing.Language = _current.Language;
        }
        else
        {
            // Insert new
            var entity = BrowserSettingsEntity.FromModel(_current);
            db.Settings.Add(entity);
        }
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Action<BrowserSettings> update)
    {
        update(_current);
        await SaveAsync();
    }
}
