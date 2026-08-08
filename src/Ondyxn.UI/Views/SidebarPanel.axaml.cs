using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Ondyxn.Core.Interfaces;
using Ondyxn.Core.Models;

namespace Ondyxn.UI.Views;

public partial class SidebarPanel : UserControl
{
    private Border? _bookmarksPanel;
    private Border? _historyPanel;
    private Border? _downloadsPanel;
    private ItemsControl? _bookmarksList;
    private ItemsControl? _historyList;
    private ItemsControl? _downloadsList;

    private readonly IBookmarkService? _bookmarkService;
    private readonly IHistoryService? _historyService;
    private readonly IDownloadService? _downloadService;

    private readonly List<BookmarkModel> _bookmarks = [];
    private readonly List<HistoryEntry> _history = [];
    private readonly List<DownloadModel> _downloads = [];

    public SidebarPanel()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    public SidebarPanel(IBookmarkService bookmarkService, IHistoryService historyService, IDownloadService downloadService)
        : this()
    {
        _bookmarkService = bookmarkService;
        _historyService = historyService;
        _downloadService = downloadService;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        _bookmarksPanel = this.FindControl<Border>("BookmarksPanel");
        _historyPanel = this.FindControl<Border>("HistoryPanel");
        _downloadsPanel = this.FindControl<Border>("DownloadsPanel");
        _bookmarksList = this.FindControl<ItemsControl>("BookmarksList");
        _historyList = this.FindControl<ItemsControl>("HistoryList");
        _downloadsList = this.FindControl<ItemsControl>("DownloadsList");

        // Default to bookmarks
        ShowSection(0);
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            if (_bookmarkService is not null)
            {
                var bookmarks = await _bookmarkService.SearchBookmarksAsync("");
                Dispatcher.UIThread.Post(() =>
                {
                    _bookmarks.Clear();
                    _bookmarks.AddRange(bookmarks);
                    if (_bookmarksList is not null) _bookmarksList.ItemsSource = _bookmarks;
                });
            }

            if (_historyService is not null)
            {
                var history = await _historyService.GetRecentHistoryAsync(50);
                Dispatcher.UIThread.Post(() =>
                {
                    _history.Clear();
                    _history.AddRange(history);
                    if (_historyList is not null) _historyList.ItemsSource = _history;
                });
            }

            if (_downloadService is not null)
            {
                Dispatcher.UIThread.Post(() =>
                {
                    _downloads.Clear();
                    _downloads.AddRange(_downloadService.Downloads);
                    if (_downloadsList is not null) _downloadsList.ItemsSource = _downloads;
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to load sidebar data: {ex.Message}");
        }
    }

    private void OnSectionClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string tag && int.TryParse(tag, out var index))
        {
            ShowSection(index);
        }
    }

    private void ShowSection(int index)
    {
        if (_bookmarksPanel is not null) _bookmarksPanel.IsVisible = index == 0;
        if (_historyPanel is not null) _historyPanel.IsVisible = index == 1;
        if (_downloadsPanel is not null) _downloadsPanel.IsVisible = index == 2;
    }

    public void Refresh()
    {
        _ = LoadDataAsync();
    }
}
