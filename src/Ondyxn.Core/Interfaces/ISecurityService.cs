using Ondyxn.Core.Models;

namespace Ondyxn.Core.Interfaces;

/// <summary>
/// Manages browser security features.
/// </summary>
public interface ISecurityService
{
    /// <summary>Get security info for a URL.</summary>
    Task<SecurityInfo> GetSecurityInfoAsync(string url);

    /// <summary>Check if a URL is safe (not phishing/malware).</summary>
    Task<bool> IsUrlSafeAsync(string url);

    /// <summary>Get site permissions for a domain.</summary>
    Task<IReadOnlyList<SitePermission>> GetSitePermissionsAsync(string domain);

    /// <summary>Set a site permission.</summary>
    Task SetSitePermissionAsync(string domain, PermissionType type, PermissionValue value);

    /// <summary>Get blocked trackers count for current session.</summary>
    int GetBlockedTrackersCount();

    /// <summary>Get blocked ads count for current session.</summary>
    int GetBlockedAdsCount();

    /// <summary>Clear all site permissions.</summary>
    Task ClearSitePermissionsAsync();

    /// <summary>Raised when security state changes.</summary>
    event EventHandler<SecurityInfo>? SecurityInfoChanged;
}
