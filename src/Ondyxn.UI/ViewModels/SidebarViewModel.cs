using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ondyxn.Core.Interfaces;
using Ondyxn.Core.Models;

namespace Ondyxn.UI.ViewModels;

/// <summary>
/// ViewModel for the sidebar navigation and panels.
/// </summary>
public partial class SidebarViewModel : ObservableObject
{
    private readonly IHistoryService _historyService;
    private readonly IBookmarkService _bookmarkService;

    [ObservableProperty] private int _selectedSection;
    [ObservableProperty] private string _searchQuery = string.Empty;

    public ObservableCollection<HistoryEntry> HistoryItems { get; } = [];
    public ObservableCollection<BookmarkModel> BookmarkItems { get; } = [];

    public SidebarViewModel(IHistoryService historyService, IBookmarkService bookmarkService)
    {
        _historyService = historyService;
        _bookmarkService = bookmarkService;
    }

    [RelayCommand]
    private async Task LoadHistoryAsync()
    {
        var entries = await _historyService.GetRecentHistoryAsync(100);
        HistoryItems.Clear();
        foreach (var entry in entries)
            HistoryItems.Add(entry);
    }

    [RelayCommand]
    private async Task SearchHistoryAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery)) return;
        var results = await _historyService.SearchHistoryAsync(SearchQuery);
        HistoryItems.Clear();
        foreach (var entry in results)
            HistoryItems.Add(entry);
    }
}
