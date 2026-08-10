using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ondyxn.Core.Interfaces;
using Ondyxn.Core.Models;
using Ondyxn.Engine.Services;

namespace Ondyxn.UI.ViewModels;

/// <summary>
/// ViewModel for the New Tab bento grid page.
/// </summary>
public partial class NewTabViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;
    private readonly IBookmarkService _bookmarkService;
    private readonly IHistoryService _historyService;
    private readonly FaviconLetterService _faviconLetterService;

    [ObservableProperty] private DateTime _currentTime = DateTime.Now;
    [ObservableProperty] private string _greeting = string.Empty;
    [ObservableProperty] private string _dateText = string.Empty;
    [ObservableProperty] private string _timeText = string.Empty;
    [ObservableProperty] private int _blockedTrackers = 0;
    [ObservableProperty] private int _topSitesCount = 0;
    [ObservableProperty] private int _bookmarksCount = 0;
    [ObservableProperty] private int _readingListCount = 0;
    [ObservableProperty] private int _activeDownloads = 0;
    [ObservableProperty] private string _weatherText = "Partly Cloudy";
    [ObservableProperty] private string _temperature = "22°C";
    [ObservableProperty] private string _weatherDetails = "Humidity 60% | Wind 12 km/h";

    public ObservableCollection<QuickLinkModel> QuickLinks { get; } = [];

    public NewTabViewModel(
        ISettingsService settingsService,
        IBookmarkService bookmarkService,
        IHistoryService historyService,
        FaviconLetterService faviconLetterService)
    {
        _settingsService = settingsService;
        _bookmarkService = bookmarkService;
        _historyService = historyService;
        _faviconLetterService = faviconLetterService;

        InitializeQuickLinks();
        UpdateTime();
        _ = LoadStatsAsync();
    }

    [RelayCommand]
    private void RefreshTime()
    {
        UpdateTime();
    }

    private void InitializeQuickLinks()
    {
        QuickLinks.Clear();

        // Default quick links
        var defaultLinks = new[]
        {
            ("New Tab", "ondyxn://newtab", "IconHome", (string?)null, (string?)null),
            ("Google", "https://www.google.com", null, "G", "#4285F4"),
            ("GitHub", "https://github.com", null, "G", "#1DB954"),
            ("YouTube", "https://www.youtube.com", null, "Y", "#FF0000"),
            ("Reddit", "https://www.reddit.com", null, "R", "#FF4500"),
            ("X", "https://x.com", null, "X", "#E8ECF4"),
        };

        foreach (var (title, url, iconKey, letter, color) in defaultLinks)
        {
            QuickLinks.Add(new QuickLinkModel
            {
                Title = title,
                Url = url,
                IconKey = iconKey,
                FaviconLetter = letter,
                FaviconColor = color
            });
        }
    }

    private async Task LoadStatsAsync()
    {
        try
        {
            var bookmarks = await _bookmarkService.SearchBookmarksAsync("");
            BookmarksCount = bookmarks.Count;

            var history = await _historyService.GetRecentHistoryAsync(100);
            TopSitesCount = history.Count;
        }
        catch (Exception)
        {
            // Silently handle errors during stats loading
        }
    }

    private void UpdateTime()
    {
        CurrentTime = DateTime.Now;
        DateText = CurrentTime.ToString("dddd, MMMM d");
        TimeText = CurrentTime.ToString("HH:mm");

        Greeting = CurrentTime.Hour switch
        {
            < 6 => "Good night",
            < 12 => "Good morning",
            < 17 => "Good afternoon",
            _ => "Good evening"
        };
    }
}
