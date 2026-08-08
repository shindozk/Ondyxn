using Avalonia.Input;
using Ondyxn.UI.ViewModels;

namespace Ondyxn.UI.Helpers;

/// <summary>
/// Centralized keyboard shortcuts for the browser.
/// </summary>
public static class KeyboardShortcuts
{
    /// <summary>
    /// Handle keyboard shortcuts and return true if the event was handled.
    /// </summary>
    public static bool HandleKeyDown(BrowserViewModel vm, KeyEventArgs e, Controls.Omnibox? omnibox, Controls.FindInPage? findInPage)
    {
        var ctrl = e.KeyModifiers.HasFlag(KeyModifiers.Control);
        var shift = e.KeyModifiers.HasFlag(KeyModifiers.Shift);
        var alt = e.KeyModifiers.HasFlag(KeyModifiers.Alt);

        // Ctrl+key shortcuts
        if (ctrl && !alt)
        {
            switch (e.Key)
            {
                case Key.T: // New tab
                    vm.CreateNewTabCommand.Execute(null);
                    return true;

                case Key.W: // Close tab
                    vm.CloseCurrentTabCommand.Execute(null);
                    return true;

                case Key.Tab: // Next/Previous tab
                    if (shift)
                        vm.PreviousTabCommand.Execute(null);
                    else
                        vm.NextTabCommand.Execute(null);
                    return true;

                case Key.L: // Focus omnibox
                    omnibox?.FocusUrl();
                    return true;

                case Key.R: // Reload
                    vm.ReloadCommand.Execute(null);
                    return true;

                case Key.F: // Find in page
                    findInPage?.Show();
                    return true;

                case Key.J: // Toggle downloads
                    vm.ToggleDownloadsCommand.Execute(null);
                    return true;

                case Key.D: // Toggle bookmark
                    vm.ToggleBookmarkCommand.Execute(null);
                    return true;

                case Key.OemPlus: // Zoom in
                    vm.ZoomInCommand.Execute(null);
                    return true;

                case Key.OemMinus: // Zoom out
                    vm.ZoomOutCommand.Execute(null);
                    return true;

                case Key.D0: // Reset zoom
                    vm.ZoomResetCommand.Execute(null);
                    return true;

                case Key.H: // Toggle sidebar (history)
                    vm.ToggleSidebarCommand.Execute(null);
                    return true;

                case Key.B: // Toggle sidebar (bookmarks)
                    vm.IsSidebarVisible = true;
                    vm.SelectedSidebarIndex = 0;
                    return true;
            }
        }

        // Alt+key shortcuts
        if (alt && !ctrl)
        {
            switch (e.Key)
            {
                case Key.Left: // Go back
                    vm.GoBackCommand.Execute(null);
                    return true;

                case Key.Right: // Go forward
                    vm.GoForwardCommand.Execute(null);
                    return true;

                case Key.Home: // Go home
                    vm.GoHomeCommand.Execute(null);
                    return true;

                case Key.D: // Bookmark page
                    vm.ToggleBookmarkCommand.Execute(null);
                    return true;
            }
        }

        // Single key shortcuts
        if (!ctrl && !alt)
        {
            switch (e.Key)
            {
                case Key.F5: // Reload
                    vm.ReloadCommand.Execute(null);
                    return true;

                case Key.F11: // Fullscreen (toggle)
                    // TODO: Implement fullscreen
                    return true;

                case Key.F12: // DevTools
                    vm.OpenDevToolsCommand.Execute(null);
                    return true;

                case Key.Escape: // Close find in page
                    if (findInPage is { IsVisible: true })
                    {
                        findInPage.Hide();
                        return true;
                    }
                    break;
            }
        }

        return false;
    }
}
