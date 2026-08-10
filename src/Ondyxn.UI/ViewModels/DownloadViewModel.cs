using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Ondyxn.Core.Enums;
using Ondyxn.Core.Models;
using Ondyxn.Engine.Handlers;

namespace Ondyxn.UI.ViewModels;

public partial class DownloadViewModel : ObservableObject
{
    private readonly ILogger<DownloadViewModel>? _logger;

    [ObservableProperty] private int _activeDownloads;
    [ObservableProperty] private bool _hasActiveDownloads;

    public ObservableCollection<DownloadModel> Downloads { get; } = [];

    public DownloadViewModel(ILogger<DownloadViewModel>? logger = null)
    {
        _logger = logger;
    }

    public void WireDownloadHandler(OndyxnDownloadHandler handler)
    {
        handler.DownloadStarted += OnDownloadStarted;
        handler.DownloadProgress += OnDownloadProgress;
        handler.DownloadCompleted += OnDownloadCompleted;
    }

    private void OnDownloadStarted(object? sender, DownloadStartedEventArgs e)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            var model = new DownloadModel
            {
                Url = e.Url,
                FileName = e.FileName,
                TotalBytes = e.TotalBytes,
                State = DownloadState.InProgress,
                StartedAt = DateTime.UtcNow
            };
            Downloads.Insert(0, model);
            UpdateCounts();
            _logger?.LogInformation("Download started: {Name}", e.FileName);
        });
    }

    private void OnDownloadProgress(object? sender, DownloadProgressEventArgs e)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            var item = Downloads.FirstOrDefault(d => d.Url == GetUrlById(e.DownloadId));
            if (item is not null)
            {
                item.ReceivedBytes = e.ReceivedBytes;
                if (e.TotalBytes > 0) item.TotalBytes = e.TotalBytes;
            }
        });
    }

    private void OnDownloadCompleted(object? sender, DownloadCompletedEventArgs e)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            var item = Downloads.FirstOrDefault(d => d.State == DownloadState.InProgress);
            if (item is not null)
            {
                item.State = e.IsSuccess ? DownloadState.Completed : DownloadState.Failed;
                item.CompletedAt = DateTime.UtcNow;
                item.FilePath = e.FilePath;
                item.ErrorMessage = e.ErrorMessage;
            }
            UpdateCounts();
            _logger?.LogInformation("Download completed: {Id} success={Success}", e.DownloadId, e.IsSuccess);
        });
    }

    private void UpdateCounts()
    {
        ActiveDownloads = Downloads.Count(d => d.State == DownloadState.InProgress || d.State == DownloadState.Paused);
        HasActiveDownloads = ActiveDownloads > 0;
    }

    private static string GetUrlById(int id) => string.Empty;

    [RelayCommand]
    private void ClearCompleted()
    {
        var completed = Downloads.Where(d =>
            d.State == DownloadState.Completed ||
            d.State == DownloadState.Failed ||
            d.State == DownloadState.Cancelled).ToList();
        foreach (var item in completed)
            Downloads.Remove(item);
        UpdateCounts();
    }

    [RelayCommand]
    private void OpenFile(DownloadModel? model)
    {
        if (model?.FilePath is null) return;
        try
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = model.FilePath,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to open file: {Path}", model.FilePath);
        }
    }

    [RelayCommand]
    private void ShowInFolder(DownloadModel? model)
    {
        if (model?.FilePath is null) return;
        try
        {
            var dir = Path.GetDirectoryName(model.FilePath);
            if (dir is not null)
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = dir,
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(psi);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to show in folder: {Path}", model.FilePath);
        }
    }
}
