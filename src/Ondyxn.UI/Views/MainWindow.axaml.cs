using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Ondyxn.Engine;
using Ondyxn.UI.Controls;
using Ondyxn.UI.Helpers;
using Ondyxn.UI.ViewModels;

namespace Ondyxn.UI.Views;

public partial class MainWindow : Window
{
    private BrowserViewModel? _viewModel;
    private FindInPage? _findInPage;

    public MainWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
        KeyDown += OnKeyDown;
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        _findInPage = this.FindControl<FindInPage>("FindInPageControl");
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

        // Use centralized shortcuts handler
        if (KeyboardShortcuts.HandleKeyDown(_viewModel, e, OmniboxControl, _findInPage))
        {
            e.Handled = true;
        }
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
