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

    // Tab drag-and-drop state
    private bool _isDraggingTab;
    private Point _dragStartPoint;
    private int _dragSourceIndex = -1;
    private const double DragThreshold = 5.0;

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
        // Save session before closing
        _ = _viewModel?.SaveSessionAsync();
        Close();
    }

    // Tab drag-and-drop handlers
    private void OnTabPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border && border.DataContext is TabViewModel tab && _viewModel is not null)
        {
            _dragStartPoint = e.GetPosition(this);
            _dragSourceIndex = _viewModel.Tabs.IndexOf(tab);
            _isDraggingTab = false;
            e.Pointer.Capture(border);
        }
    }

    private void OnTabPointerMoved(object? sender, PointerEventArgs e)
    {
        if (sender is Border border && e.Pointer.Captured == border)
        {
            var currentPoint = e.GetPosition(this);
            var diff = currentPoint - _dragStartPoint;

            if (!_isDraggingTab && (Math.Abs(diff.X) > DragThreshold || Math.Abs(diff.Y) > DragThreshold))
            {
                _isDraggingTab = true;
            }

            if (_isDraggingTab && _viewModel is not null)
            {
                // Find the tab under the cursor
                var tabs = _viewModel.Tabs;
                if (tabs.Count < 2) return;

                // Calculate which tab index we're over
                var hitIndex = GetTabIndexAtPosition(currentPoint.X);
                if (hitIndex >= 0 && hitIndex != _dragSourceIndex && hitIndex < tabs.Count)
                {
                    // Move the tab
                    _viewModel.MoveTab(_dragSourceIndex, hitIndex);
                    _dragSourceIndex = hitIndex;
                }
            }
        }
    }

    private void OnTabPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (sender is Border border)
        {
            e.Pointer.Capture(null);
            _isDraggingTab = false;
            _dragSourceIndex = -1;
        }
    }

    private int GetTabIndexAtPosition(double x)
    {
        if (_viewModel is null) return -1;

        // Find the tab strip container and calculate positions
        // This is a simplified approach - in production you'd want to use actual element positions
        var tabs = _viewModel.Tabs;
        if (tabs.Count == 0) return -1;

        // Estimate tab width based on typical tab pill size
        var tabWidth = 120.0; // Approximate width including spacing
        var startX = 50.0; // Logo width + margin

        var index = (int)((x - startX) / tabWidth);
        return Math.Clamp(index, 0, tabs.Count - 1);
    }

    private void OnBrowserPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (_viewModel is null) return;

        var props = e.GetCurrentPoint(this).Properties;
        if (!props.IsLeftButtonPressed && props.IsRightButtonPressed)
        {
            var contextMenu = new BrowserContextMenu();
            contextMenu.WireViewModel(_viewModel);
            contextMenu.ShowAt(CefBrowserHost);
            e.Handled = true;
        }
    }
}
