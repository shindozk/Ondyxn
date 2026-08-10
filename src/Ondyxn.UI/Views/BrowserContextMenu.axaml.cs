using Avalonia.Controls;

namespace Ondyxn.UI.Views;

public partial class BrowserContextMenu : MenuFlyout
{
    private Ondyxn.UI.ViewModels.BrowserViewModel? _vm;

    public void WireViewModel(Ondyxn.UI.ViewModels.BrowserViewModel vm)
    {
        _vm = vm;

        foreach (var item in Items)
        {
            if (item is not MenuItem mi) continue;
            mi.Click += OnMenuItemClick;
        }
    }

    private void OnMenuItemClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_vm is null || sender is not MenuItem mi) return;

        switch (mi.Header?.ToString())
        {
            case "Back":
                _vm.GoBackCommand.Execute(null);
                break;
            case "Forward":
                _vm.GoForwardCommand.Execute(null);
                break;
            case "Reload":
                _vm.ReloadCommand.Execute(null);
                break;
            case "Open link in new tab":
            case "Open link in new window":
                _vm.CreateNewTabCommand.Execute(null);
                break;
            case "View page source":
            case "Inspect element":
                _vm.OpenDevToolsCommand.Execute(null);
                break;
        }
    }
}
