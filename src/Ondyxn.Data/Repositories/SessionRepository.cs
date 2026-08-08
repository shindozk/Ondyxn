using Microsoft.EntityFrameworkCore;
using Ondyxn.Core.Interfaces;
using Ondyxn.Core.Models;

namespace Ondyxn.Data.Repositories;

public class SessionRepository : ISessionService
{
    private readonly IDbContextFactory<BrowserDbContext> _factory;
    private SessionModel? _currentSession;

    public SessionRepository(IDbContextFactory<BrowserDbContext> factory)
    {
        _factory = factory;
    }

    public async Task SaveSessionAsync(SessionModel session)
    {
        session.SavedAt = DateTime.UtcNow;

        await using var db = await _factory.CreateDbContextAsync();

        // Save or update the session
        var existing = await db.Sessions.FindAsync(session.Id);
        if (existing is not null)
        {
            existing.Name = session.Name;
            existing.SavedAt = session.SavedAt;
            existing.IsPrivate = session.IsPrivate;

            // Remove old snapshots and add new ones
            var oldSnapshots = await db.TabSnapshots
                .Where(t => t.SessionId == session.Id)
                .ToListAsync();
            db.TabSnapshots.RemoveRange(oldSnapshots);

            foreach (var tab in session.Tabs)
            {
                tab.SessionId = session.Id;
                db.TabSnapshots.Add(tab);
            }
        }
        else
        {
            db.Sessions.Add(session);
            foreach (var tab in session.Tabs)
            {
                tab.SessionId = session.Id;
                db.TabSnapshots.Add(tab);
            }
        }

        await db.SaveChangesAsync();
        _currentSession = session;
    }

    public async Task<IReadOnlyList<SessionModel>> GetSavedSessionsAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Sessions
            .OrderByDescending(s => s.SavedAt)
            .ToListAsync();
    }

    public async Task<SessionModel?> RestoreSessionAsync(Guid sessionId)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var session = await db.Sessions.FindAsync(sessionId);
        if (session is null) return null;

        // Load tabs for this session
        session.Tabs = await db.TabSnapshots
            .Where(t => t.SessionId == sessionId)
            .OrderBy(t => t.Order)
            .ToListAsync();

        _currentSession = session;
        return session;
    }

    public async Task DeleteSessionAsync(Guid sessionId)
    {
        await using var db = await _factory.CreateDbContextAsync();

        // Remove tabs first (foreign key)
        var tabs = await db.TabSnapshots
            .Where(t => t.SessionId == sessionId)
            .ToListAsync();
        db.TabSnapshots.RemoveRange(tabs);

        // Remove session
        var session = await db.Sessions.FindAsync(sessionId);
        if (session is not null)
            db.Sessions.Remove(session);

        await db.SaveChangesAsync();
    }

    public Task<SessionModel?> GetCurrentSessionAsync()
    {
        return Task.FromResult(_currentSession);
    }
}
