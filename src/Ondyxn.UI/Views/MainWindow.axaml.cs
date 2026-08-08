using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Ondyxn.Engine;
using Ondyxn.UI.Controls;
using Ondyxn.UI.ViewModels;

namespace Ondyxn.UI.Views;

public partial class MainWindow : Window
{
    private BrowserViewModel? _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
        KeyDown += OnKeyDown;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is BrowserViewModel vm)
        {
            _viewModel = vm;
            vm.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(BrowserViewModel.ActiveTab))
                {
                    AttachActiveTabToBrowser();
                }
            };

            // Attach the initial tab if already set
            if (vm.ActiveTab is not null)
            {
                AttachActiveTabToBrowser();
            }
        }
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (_viewModel is null) return;

        var ctrl = e.KeyModifiers.HasFlag(KeyModifiers.Control);
        var shift = e.KeyModifiers.HasFlag(KeyModifiers.Shift);
        var alt = e.KeyModifiers.HasFlag(KeyModifiers.Alt);

        // Ctrl+key shortcuts - only intercept browser-level shortcuts
        if (ctrl && !alt)
        {
            switch (e.Key)
            {
                case Key.T: // Ctrl+T: New tab
                    _viewModel.CreateNewTabCommand.Execute(null);
                    e.Handled = true;
                    break;
                case Key.W: // Ctrl+W: Close tab
                    _viewModel.CloseCurrentTabCommand.Execute(null);
                    e.Handled = true;
                    break;
                case Key.Tab: // Ctrl+Tab / Ctrl+Shift+Tab: Next/Previous tab
                    if (shift)
                        _viewModel.PreviousTabCommand.Execute(null);
                    else
                        _viewModel.NextTabCommand.Execute(null);
                    e.Handled = true;
                    break;
                case Key.L: // Ctrl+L: Focus omnibox
                    OmniboxControl.FocusUrl();
                    e.Handled = true;
                    break;
                case Key.R: // Ctrl+R: Reload
                    _viewModel.ReloadCommand.Execute(null);
                    e.Handled = true;
                    break;
                case Key.F: // Ctrl+F: Let browser handle find-in-page
                    // Don't handle - let CEF browser handle Ctrl+F for in-page search
                    break;
                case Key.P: // Ctrl+P: Let browser handle print
                    // Don't handle - let CEF browser handle Ctrl+P for printing
                    break;
                case Key.U: // Ctrl+U: Let browser handle view source
                    // Don't handle - let CEF browser handle Ctrl+U
                    break;
                case Key.J: // Ctrl+J: Toggle downloads
                    _viewModel.ToggleDownloadsCommand.Execute(null);
                    e.Handled = true;
                    break;
            }
        }
        // Alt+key shortcuts
        else if (alt && !ctrl)
        {
            switch (e.Key)
            {
                case Key.Left: // Alt+Left: Go back
                    _viewModel.GoBackCommand.Execute(null);
                    e.Handled = true;
                    break;
                case Key.Right: // Alt+Right: Go forward
                    _viewModel.GoForwardCommand.Execute(null);
                    e.Handled = true;
                    break;
                case Key.Home: // Alt+Home: Go home
                    _viewModel.GoHomeCommand.Execute(null);
                    e.Handled = true;
                    break;
            }
        }
        // Single key shortcuts - only intercept non-typing keys
        else if (!ctrl && !alt)
        {
            switch (e.Key)
            {
                case Key.F5: // F5: Reload
                    _viewModel.ReloadCommand.Execute(null);
                    e.Handled = true;
                    break;
                case Key.F12: // F12: DevTools
                    _viewModel.OpenDevToolsCommand.Execute(null);
                    e.Handled = true;
                    break;
                case Key.BrowserBack: // Browser back button
                    _viewModel.GoBackCommand.Execute(null);
                    e.Handled = true;
                    break;
                case Key.BrowserForward: // Browser forward button
                    _viewModel.GoForwardCommand.Execute(null);
                    e.Handled = true;
                    break;
            }
        }
        // All other keys (typing, Escape, etc.) are NOT handled here
        // so they pass through to the focused CEF browser control
    }

    /// <summary>
    /// Connects the active tab's CEF browser to the CefBrowserControl in the UI.
    /// </summary>
    private void AttachActiveTabToBrowser()
    {
        if (_viewModel?.ActiveTab is null)
        {
            System.Diagnostics.Debug.WriteLine("AttachActiveTabToBrowser: ActiveTab is null");
            return;
        }

        var tab = _viewModel.ActiveTab;
        System.Diagnostics.Debug.WriteLine($"AttachActiveTabToBrowser: Attaching tab {tab.Title}, CefBrowserHost={CefBrowserHost?.GetType().Name ?? "NULL"}");
        
        if (CefBrowserHost is null)
        {
            System.Diagnostics.Debug.WriteLine("ERROR: CefBrowserHost is null!");
            return;
        }
        
        Dispatcher.UIThread.Post(() =>
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Dispatcher: Calling AttachBrowser...");
                CefBrowserHost.AttachBrowser(tab.BrowserInstance);
                System.Diagnostics.Debug.WriteLine("Dispatcher: AttachBrowser completed");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in AttachBrowser: {ex.Message}");
            }
        });
    }

    private void Minimize_Click(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void Maximize_Click(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
