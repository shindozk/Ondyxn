using Ondyxn.Core.Models;

namespace Ondyxn.Core.Interfaces;

/// <summary>
/// Manages session save/restore.
/// </summary>
public interface ISessionService
{
    Task SaveSessionAsync(SessionModel session);
    Task<IReadOnlyList<SessionModel>> GetSavedSessionsAsync();
    Task<SessionModel?> RestoreSessionAsync(Guid sessionId);
    Task DeleteSessionAsync(Guid sessionId);
    Task<SessionModel?> GetCurrentSessionAsync();
}
