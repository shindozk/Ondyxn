using Microsoft.EntityFrameworkCore;
using Ondyxn.Core.Interfaces;
using Ondyxn.Core.Models;

namespace Ondyxn.Data.Repositories;

/// <summary>
/// Security service implementation with SQLite storage for permissions.
/// </summary>
public class SecurityService : ISecurityService
{
    private readonly IDbContextFactory<BrowserDbContext> _factory;
    private int _blockedTrackers;
    private int _blockedAds;

    public event EventHandler<SecurityInfo>? SecurityInfoChanged;

    public SecurityService(IDbContextFactory<BrowserDbContext> factory)
    {
        _factory = factory;
    }

    public Task<SecurityInfo> GetSecurityInfoAsync(string url)
    {
        var info = new SecurityInfo
        {
            Url = url,
            IsSecure = url.StartsWith("https://", StringComparison.OrdinalIgnoreCase),
            IsCertificateValid = true, // CEF handles cert validation
            TrackersBlocked = _blockedTrackers,
            AdsBlocked = _blockedAds
        };

        // Extract domain for permissions
        if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            info.CertificateSubject = uri.Host;
        }

        return Task.FromResult(info);
    }

    public Task<bool> IsUrlSafeAsync(string url)
    {
        // Basic safety check - in production, use Google Safe Browsing API
        var blockedDomains = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "malware.com",
            "phishing.com",
            "scam.com"
        };

        if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return Task.FromResult(!blockedDomains.Contains(uri.Host));
        }

        return Task.FromResult(true);
    }

    public async Task<IReadOnlyList<SitePermission>> GetSitePermissionsAsync(string domain)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Set<SitePermission>()
            .Where(p => p.Site == domain)
            .ToListAsync();
    }

    public async Task SetSitePermissionAsync(string domain, PermissionType type, PermissionValue value)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var existing = await db.Set<SitePermission>()
            .FirstOrDefaultAsync(p => p.Site == domain && p.Type == type);

        if (existing is not null)
        {
            existing.Value = value;
        }
        else
        {
            db.Set<SitePermission>().Add(new SitePermission
            {
                Site = domain,
                Type = type,
                Value = value
            });
        }

        await db.SaveChangesAsync();
    }

    public int GetBlockedTrackersCount() => _blockedTrackers;

    public int GetBlockedAdsCount() => _blockedAds;

    public void IncrementBlockedTrackers() => _blockedTrackers++;

    public void IncrementBlockedAds() => _blockedAds++;

    public async Task ClearSitePermissionsAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        await db.Set<SitePermission>().ExecuteDeleteAsync();
    }

    public void NotifySecurityChanged(SecurityInfo info)
    {
        SecurityInfoChanged?.Invoke(this, info);
    }
}
