using Microsoft.EntityFrameworkCore;
using Ondyxn.Core.Enums;
using Ondyxn.Core.Interfaces;
using Ondyxn.Core.Models;

namespace Ondyxn.Data.Repositories;

public class DownloadRepository : IDownloadService
{
    private readonly IDbContextFactory<BrowserDbContext> _factory;
    private readonly List<DownloadModel> _downloads = [];

    public IReadOnlyList<DownloadModel> Downloads => _downloads.AsReadOnly();

    public event EventHandler<DownloadModel>? DownloadStarted;
    public event EventHandler<DownloadModel>? DownloadProgress;
    public event EventHandler<DownloadModel>? DownloadCompleted;

    public DownloadRepository(IDbContextFactory<BrowserDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<DownloadModel> StartDownloadAsync(string url, string? fileName = null)
    {
        var download = new DownloadModel
        {
            Url = url,
            FileName = fileName ?? Path.GetFileName(new Uri(url).AbsolutePath),
            State = DownloadState.InProgress,
            StartedAt = DateTime.UtcNow
        };

        await using var db = await _factory.CreateDbContextAsync();
        db.Downloads.Add(download);
        await db.SaveChangesAsync();

        _downloads.Add(download);
        DownloadStarted?.Invoke(this, download);
        return download;
    }

    public async Task CancelDownloadAsync(Guid downloadId)
    {
        var download = _downloads.FirstOrDefault(d => d.Id == downloadId);
        if (download is null) return;

        download.State = DownloadState.Cancelled;
        download.CompletedAt = DateTime.UtcNow;

        await using var db = await _factory.CreateDbContextAsync();
        var entity = await db.Downloads.FindAsync(downloadId);
        if (entity is not null)
        {
            entity.State = DownloadState.Cancelled;
            entity.CompletedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
    }

    public async Task PauseDownloadAsync(Guid downloadId)
    {
        var download = _downloads.FirstOrDefault(d => d.Id == downloadId);
        if (download is null) return;

        download.State = DownloadState.Paused;

        await using var db = await _factory.CreateDbContextAsync();
        var entity = await db.Downloads.FindAsync(downloadId);
        if (entity is not null)
        {
            entity.State = DownloadState.Paused;
            await db.SaveChangesAsync();
        }
    }

    public async Task ResumeDownloadAsync(Guid downloadId)
    {
        var download = _downloads.FirstOrDefault(d => d.Id == downloadId);
        if (download is null) return;

        download.State = DownloadState.InProgress;

        await using var db = await _factory.CreateDbContextAsync();
        var entity = await db.Downloads.FindAsync(downloadId);
        if (entity is not null)
        {
            entity.State = DownloadState.InProgress;
            await db.SaveChangesAsync();
        }
    }

    public async Task ClearCompletedDownloadsAsync()
    {
        var completed = _downloads.Where(d =>
            d.State == DownloadState.Completed ||
            d.State == DownloadState.Cancelled ||
            d.State == DownloadState.Failed).ToList();

        foreach (var item in completed)
            _downloads.Remove(item);

        await using var db = await _factory.CreateDbContextAsync();
        await db.Downloads
            .Where(d => d.State == DownloadState.Completed ||
                        d.State == DownloadState.Cancelled ||
                        d.State == DownloadState.Failed)
            .ExecuteDeleteAsync();
    }

    /// <summary>
    /// Updates download progress (called by CEF download handler).
    /// </summary>
    public async Task UpdateProgressAsync(Guid downloadId, long receivedBytes, long totalBytes)
    {
        var download = _downloads.FirstOrDefault(d => d.Id == downloadId);
        if (download is null) return;

        download.ReceivedBytes = receivedBytes;
        download.TotalBytes = totalBytes;
        DownloadProgress?.Invoke(this, download);

        await using var db = await _factory.CreateDbContextAsync();
        var entity = await db.Downloads.FindAsync(downloadId);
        if (entity is not null)
        {
            entity.ReceivedBytes = receivedBytes;
            entity.TotalBytes = totalBytes;
            await db.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Marks a download as completed (called by CEF download handler).
    /// </summary>
    public async Task CompleteDownloadAsync(Guid downloadId, string? filePath = null)
    {
        var download = _downloads.FirstOrDefault(d => d.Id == downloadId);
        if (download is null) return;

        download.State = DownloadState.Completed;
        download.CompletedAt = DateTime.UtcNow;
        download.FilePath = filePath;
        DownloadCompleted?.Invoke(this, download);

        await using var db = await _factory.CreateDbContextAsync();
        var entity = await db.Downloads.FindAsync(downloadId);
        if (entity is not null)
        {
            entity.State = DownloadState.Completed;
            entity.CompletedAt = DateTime.UtcNow;
            entity.FilePath = filePath;
            await db.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Marks a download as failed (called by CEF download handler).
    /// </summary>
    public async Task FailDownloadAsync(Guid downloadId, string errorMessage)
    {
        var download = _downloads.FirstOrDefault(d => d.Id == downloadId);
        if (download is null) return;

        download.State = DownloadState.Failed;
        download.ErrorMessage = errorMessage;
        download.CompletedAt = DateTime.UtcNow;

        await using var db = await _factory.CreateDbContextAsync();
        var entity = await db.Downloads.FindAsync(downloadId);
        if (entity is not null)
        {
            entity.State = DownloadState.Failed;
            entity.ErrorMessage = errorMessage;
            entity.CompletedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
    }
}
