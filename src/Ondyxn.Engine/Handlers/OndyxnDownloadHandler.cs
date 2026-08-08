using Microsoft.Extensions.Logging;
using Xilium.CefGlue;

namespace Ondyxn.Engine.Handlers;

/// <summary>
/// CEF download handler that manages file downloads initiated by the browser.
/// </summary>
public sealed class OndyxnDownloadHandler : CefDownloadHandler
{
    private readonly ILogger<OndyxnDownloadHandler>? _logger;
    private readonly string _downloadDirectory;

    public event EventHandler<DownloadStartedEventArgs>? DownloadStarted;
    public event EventHandler<DownloadProgressEventArgs>? DownloadProgress;
    public event EventHandler<DownloadCompletedEventArgs>? DownloadCompleted;

    public OndyxnDownloadHandler(ILogger<OndyxnDownloadHandler>? logger = null, string? downloadDirectory = null)
    {
        _logger = logger;
        _downloadDirectory = downloadDirectory ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads");
    }

    protected override bool CanDownload(CefBrowser browser, string url, string requestMethod)
    {
        return true;
    }

    protected override void OnBeforeDownload(CefBrowser browser, CefDownloadItem downloadItem, string suggestedName, CefBeforeDownloadCallback callback)
    {
        var targetPath = Path.Combine(_downloadDirectory, suggestedName);

        Directory.CreateDirectory(_downloadDirectory);

        _logger?.LogInformation("Starting download: {Name} -> {Path}", suggestedName, targetPath);

        DownloadStarted?.Invoke(this, new DownloadStartedEventArgs
        {
            DownloadId = (int)downloadItem.Id,
            Url = downloadItem.Url,
            FileName = suggestedName,
            TotalBytes = (long)downloadItem.TotalBytes
        });

        callback.Continue(targetPath, showDialog: false);
    }

    protected override void OnDownloadUpdated(CefBrowser browser, CefDownloadItem downloadItem, CefDownloadItemCallback callback)
    {
        if (downloadItem.IsComplete)
        {
            _logger?.LogInformation("Download completed: {Id}", downloadItem.Id);
            DownloadCompleted?.Invoke(this, new DownloadCompletedEventArgs
            {
                DownloadId = (int)downloadItem.Id,
                IsSuccess = true,
                FilePath = downloadItem.FullPath
            });
        }
        else if (downloadItem.IsCanceled)
        {
            _logger?.LogInformation("Download cancelled: {Id}", downloadItem.Id);
            DownloadCompleted?.Invoke(this, new DownloadCompletedEventArgs
            {
                DownloadId = (int)downloadItem.Id,
                IsSuccess = false,
                ErrorMessage = "Download cancelled"
            });
        }
        else
        {
            DownloadProgress?.Invoke(this, new DownloadProgressEventArgs
            {
                DownloadId = (int)downloadItem.Id,
                ReceivedBytes = (long)downloadItem.ReceivedBytes,
                TotalBytes = (long)downloadItem.TotalBytes
            });
        }
    }
}

public class DownloadStartedEventArgs : EventArgs
{
    public int DownloadId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long TotalBytes { get; set; }
}

public class DownloadProgressEventArgs : EventArgs
{
    public int DownloadId { get; set; }
    public long ReceivedBytes { get; set; }
    public long TotalBytes { get; set; }
}

public class DownloadCompletedEventArgs : EventArgs
{
    public int DownloadId { get; set; }
    public bool IsSuccess { get; set; }
    public string? FilePath { get; set; }
    public string? ErrorMessage { get; set; }
}
