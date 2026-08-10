using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Ondyxn.UI.ViewModels;

namespace Ondyxn.UI.Views;

public partial class CommandPalette : UserControl
{
    private CommandPaletteViewModel? _vm;

    public CommandPalette()
    {
        InitializeComponent();
        DataContextChanged += (_, _) => _vm = DataContext as CommandPaletteViewModel;
    }

    private void OnSearchKeyDown(object? sender, KeyEventArgs e)
    {
        if (_vm is null) return;

        switch (e.Key)
        {
            case Key.Escape:
                _vm.IsOpen = false;
                e.Handled = true;
                break;
            case Key.Down:
                _vm.MoveSelection(1);
                e.Handled = true;
                break;
            case Key.Up:
                _vm.MoveSelection(-1);
                e.Handled = true;
                break;
            case Key.Return:
                _vm.ExecuteSelected();
                e.Handled = true;
                break;
        }
    }

    private void OnCommandHover(object? sender, PointerEventArgs e)
    {
        if (sender is Border { DataContext: PaletteCommand cmd } &&
            DataContext is CommandPaletteViewModel vm)
        {
            var idx = vm.FilteredCommands.IndexOf(cmd);
            if (idx >= 0)
                vm.SelectedIndex = idx;
        }
    }
}
