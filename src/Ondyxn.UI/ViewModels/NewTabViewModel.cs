using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ondyxn.Core.Enums;
using Ondyxn.Core.Interfaces;
using Ondyxn.Core.Models;

namespace Ondyxn.UI.ViewModels;

/// <summary>
/// ViewModel for the New Tab bento grid page.
/// </summary>
public partial class NewTabViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;
    private readonly IBookmarkService _bookmarkService;

    [ObservableProperty] private DateTime _currentTime = DateTime.Now;
    [ObservableProperty] private string _greeting = string.Empty;
    [ObservableProperty] private string _dateText = string.Empty;

    public ObservableCollection<BookmarkModel> TopSites { get; } = [];

    public NewTabViewModel(ISettingsService settingsService, IBookmarkService bookmarkService)
    {
        _settingsService = settingsService;
        _bookmarkService = bookmarkService;
        UpdateTime();
    }

    [RelayCommand]
    private void RefreshTime()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        CurrentTime = DateTime.Now;
        DateText = CurrentTime.ToString("EEEE, MMMM d");

        Greeting = CurrentTime.Hour switch
        {
            < 6 => "Good night",
            < 12 => "Good morning",
            < 17 => "Good afternoon",
            _ => "Good evening"
        };
    }
}
