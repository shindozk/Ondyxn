using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Ondyxn.UI.Views;

public partial class SidebarPanel : UserControl
{
    private Border? _bookmarksPanel;
    private Border? _historyPanel;
    private Border? _downloadsPanel;

    public SidebarPanel()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        _bookmarksPanel = this.FindControl<Border>("BookmarksPanel");
        _historyPanel = this.FindControl<Border>("HistoryPanel");
        _downloadsPanel = this.FindControl<Border>("DownloadsPanel");

        // Default to bookmarks
        ShowSection(0);
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
}
