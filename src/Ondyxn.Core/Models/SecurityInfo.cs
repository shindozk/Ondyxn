namespace Ondyxn.Core.Models;

/// <summary>
/// Security information for a website.
/// </summary>
public class SecurityInfo
{
    /// <summary>The URL being visited.</summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>Whether the connection is secure (HTTPS).</summary>
    public bool IsSecure { get; set; }

    /// <summary>Whether the certificate is valid.</summary>
    public bool IsCertificateValid { get; set; }

    /// <summary>Certificate issuer.</summary>
    public string? CertificateIssuer { get; set; }

    /// <summary>Certificate subject.</summary>
    public string? CertificateSubject { get; set; }

    /// <summary>Certificate expiration date.</summary>
    public DateTime? CertificateExpiration { get; set; }

    /// <summary>Protocol version (e.g., TLS 1.3).</summary>
    public string? ProtocolVersion { get; set; }

    /// <summary>Cipher suite used.</summary>
    public string? CipherSuite { get; set; }

    /// <summary>Whether mixed content is detected.</summary>
    public bool HasMixedContent { get; set; }

    /// <summary>Number of trackers blocked.</summary>
    public int TrackersBlocked { get; set; }

    /// <summary>Number of ads blocked.</summary>
    public int AdsBlocked { get; set; }

    /// <summary>Site permissions granted.</summary>
    public List<SitePermission> Permissions { get; set; } = [];

    /// <summary>Security summary for display.</summary>
    public string SecuritySummary => IsSecure
        ? "Connection is secure"
        : "Connection is not secure";

    /// <summary>Security level for UI.</summary>
    public SecurityLevel Level => (IsSecure, IsCertificateValid) switch
    {
        (true, true) => SecurityLevel.Secure,
        (true, false) => SecurityLevel.Warning,
        (false, _) => SecurityLevel.Insecure
    };
}

/// <summary>
/// A site-specific permission.
/// </summary>
public class SitePermission
{
    public string Site { get; set; } = string.Empty;
    public PermissionType Type { get; set; }
    public PermissionValue Value { get; set; } = PermissionValue.Ask;
}

public enum PermissionType
{
    Location,
    Camera,
    Microphone,
    Notifications,
    Javascript,
    Cookies,
    Popups,
    AutomaticDownloads
}

public enum PermissionValue
{
    Ask,
    Allow,
    Deny
}

public enum SecurityLevel
{
    Secure,
    Warning,
    Insecure
}
