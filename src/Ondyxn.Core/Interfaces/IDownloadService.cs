using Ondyxn.Core.Models;

namespace Ondyxn.Core.Interfaces;

/// <summary>
/// Manages file downloads.
/// </summary>
public interface IDownloadService
{
    IReadOnlyList<DownloadModel> Downloads { get; }
    event EventHandler<DownloadModel>? DownloadStarted;
    event EventHandler<DownloadModel>? DownloadProgress;
    event EventHandler<DownloadModel>? DownloadCompleted;

    Task<DownloadModel> StartDownloadAsync(string url, string? fileName = null);
    Task CancelDownloadAsync(Guid downloadId);
    Task PauseDownloadAsync(Guid downloadId);
    Task ResumeDownloadAsync(Guid downloadId);
    Task ClearCompletedDownloadsAsync();
}
