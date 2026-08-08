using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Ondyxn.UI.ViewModels;

namespace Ondyxn.UI.Controls;

public partial class Omnibox : UserControl
{
    private TextBox? _urlTextBox;
    private TextBlock? _urlDisplay;

    public Omnibox()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        _urlTextBox = this.FindControl<TextBox>("UrlTextBox");
        _urlDisplay = this.FindControl<TextBlock>("UrlDisplay");

        // Click on the display text to enter edit mode
        if (_urlDisplay is not null)
        {
            _urlDisplay.PointerPressed += (_, _) => EnterEditMode();
        }

        // Exit edit mode when clicking outside
        if (_urlTextBox is not null)
        {
            _urlTextBox.LostFocus += (_, _) => ExitEditMode();
        }
    }

    private void EnterEditMode()
    {
        if (_urlTextBox is null || _urlDisplay is null) return;

        Dispatcher.UIThread.Post(() =>
        {
            _urlDisplay.IsVisible = false;
            _urlTextBox.IsVisible = true;
            _urlTextBox.Focus();
            _urlTextBox.SelectAll();
        });
    }

    private void ExitEditMode()
    {
        if (_urlTextBox is null || _urlDisplay is null) return;

        Dispatcher.UIThread.Post(() =>
        {
            _urlTextBox.IsVisible = false;
            _urlDisplay.IsVisible = true;
        });
    }

    private void Omnibox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return && DataContext is BrowserViewModel vm)
        {
            // Navigate to the URL
            vm.NavigateCommand.Execute(null);
            ExitEditMode();
            this.Focus();
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            // Reset text and exit edit mode
            if (DataContext is BrowserViewModel vm2 && vm2.ActiveTab is not null)
            {
                vm2.OmniboxText = vm2.ActiveTab.Url;
            }
            ExitEditMode();
            this.Focus();
            e.Handled = true;
        }
    }

    /// <summary>
    /// Focus the URL text box and enter edit mode.
    /// </summary>
    public void FocusUrl()
    {
        EnterEditMode();
    }
}
