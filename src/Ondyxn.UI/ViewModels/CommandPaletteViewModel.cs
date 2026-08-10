using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Ondyxn.UI.ViewModels;

public partial class CommandPaletteViewModel : ObservableObject
{
    [ObservableProperty] private bool _isOpen;
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private int _selectedIndex;

    private readonly BrowserViewModel _browserVm;
    private List<PaletteCommand> _allCommands = [];

    public ObservableCollection<PaletteCommand> FilteredCommands { get; } = [];

    public CommandPaletteViewModel(BrowserViewModel browserVm)
    {
        _browserVm = browserVm;
        LoadCommands();
    }

    private void LoadCommands()
    {
        _allCommands =
        [
            new("New Tab", "N", "Create a new tab", () => _browserVm.CreateNewTabCommand.Execute(null)),
            new("Close Tab", "Ctrl+W", "Close current tab", () => _browserVm.CloseCurrentTabCommand.Execute(null)),
            new("Next Tab", "Ctrl+Tab", "Switch to next tab", () => _browserVm.NextTabCommand.Execute(null)),
            new("Previous Tab", "Ctrl+Shift+Tab", "Switch to previous tab", () => _browserVm.PreviousTabCommand.Execute(null)),
            new("Reload Page", "Ctrl+R", "Reload the current page", () => _browserVm.ReloadCommand.Execute(null)),
            new("Go Back", "Alt+Left", "Navigate back", () => _browserVm.GoBackCommand.Execute(null)),
            new("Go Forward", "Alt+Right", "Navigate forward", () => _browserVm.GoForwardCommand.Execute(null)),
            new("Go Home", "Alt+Home", "Navigate to home page", () => _browserVm.GoHomeCommand.Execute(null)),
            new("Bookmark Page", "Ctrl+D", "Toggle bookmark", () => _browserVm.ToggleBookmarkCommand.Execute(null)),
            new("Show Bookmarks", "Ctrl+Shift+B", "Open bookmarks panel", () => _browserVm.ToggleBookmarksCommand.Execute(null)),
            new("Show History", "Ctrl+H", "Open history panel", () => _browserVm.ToggleHistoryCommand.Execute(null)),
            new("Show Downloads", "Ctrl+J", "Open downloads panel", () => _browserVm.ToggleDownloadsCommand.Execute(null)),
            new("Save Session", "Ctrl+S", "Save current session", () => _ = _browserVm.SaveSessionAsync()),
            new("Settings", "Ctrl+,", "Open settings page", () => _browserVm.ToggleSettingsCommand.Execute(null)),
            new("Toggle Sidebar", "Ctrl+Shift+S", "Toggle sidebar visibility", () => _browserVm.ToggleSidebarCommand.Execute(null)),
            new("Zoom In", "Ctrl++", "Increase zoom level", () => _browserVm.ZoomInCommand.Execute(null)),
            new("Zoom Out", "Ctrl+-", "Decrease zoom level", () => _browserVm.ZoomOutCommand.Execute(null)),
            new("Reset Zoom", "Ctrl+0", "Reset zoom to 100%", () => _browserVm.ZoomResetCommand.Execute(null)),
            new("Developer Tools", "F12", "Open developer tools", () => _browserVm.OpenDevToolsCommand.Execute(null)),
        ];
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterCommands(value);
    }

    private void FilterCommands(string query)
    {
        FilteredCommands.Clear();
        var filtered = string.IsNullOrWhiteSpace(query)
            ? _allCommands
            : _allCommands.Where(c =>
                c.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                c.Shortcut.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                c.Description.Contains(query, StringComparison.OrdinalIgnoreCase));
        foreach (var cmd in filtered)
            FilteredCommands.Add(cmd);
        SelectedIndex = 0;
    }

    public void Toggle()
    {
        IsOpen = !IsOpen;
        if (IsOpen)
        {
            SearchText = string.Empty;
            FilterCommands(string.Empty);
        }
    }

    public void ExecuteSelected()
    {
        if (FilteredCommands.Count == 0) return;
        var idx = Math.Clamp(SelectedIndex, 0, FilteredCommands.Count - 1);
        FilteredCommands[idx].Action();
        IsOpen = false;
    }

    public void MoveSelection(int delta)
    {
        if (FilteredCommands.Count == 0) return;
        SelectedIndex = (SelectedIndex + delta + FilteredCommands.Count) % FilteredCommands.Count;
    }
}

public class PaletteCommand(string name, string shortcut, string description, Action action)
{
    public string Name { get; } = name;
    public string Shortcut { get; } = shortcut;
    public string Description { get; } = description;
    public Action Action { get; } = action;
}
