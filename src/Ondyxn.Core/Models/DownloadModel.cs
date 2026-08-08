using Ondyxn.Core.Enums;

namespace Ondyxn.Core.Models;

/// <summary>
/// Represents a download item.
/// </summary>
public class DownloadModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string? FilePath { get; set; }
    public string? MimeType { get; set; }
    public long TotalBytes { get; set; }
    public long ReceivedBytes { get; set; }
    public DownloadState State { get; set; } = DownloadState.Pending;
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }

    public double Progress => TotalBytes > 0
        ? (double)ReceivedBytes / TotalBytes * 100
        : 0;
}
