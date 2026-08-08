using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Microsoft.Extensions.Logging;
using Ondyxn.Engine;

namespace Ondyxn.UI.Controls;

/// <summary>
/// Hosts an AvaloniaCefBrowser control inside the Avalonia visual tree.
/// Properly forwards pointer, keyboard, and focus events to the CEF browser.
/// </summary>
public partial class CefBrowserControl : UserControl
{
    private BrowserInstance? _browserInstance;
    private static readonly ILogger<CefBrowserControl> _logger =
        new LoggerFactory().CreateLogger<CefBrowserControl>();

    public CefBrowserControl()
    {
        InitializeComponent();

        // Ensure this control is focusable for keyboard events
        Focusable = true;

        // When user clicks on the browser area, ensure CEF gets focus
        PointerPressed += OnPointerPressed;
    }

    /// <summary>
    /// Attaches a BrowserInstance to this control, adding its CEF browser to the visual tree.
    /// </summary>
    public void AttachBrowser(BrowserInstance browserInstance)
    {
        _logger.LogInformation("AttachBrowser called for {Id}", browserInstance.Id);

        if (_browserInstance is not null)
            DetachBrowser();

        _browserInstance = browserInstance;

        Dispatcher.UIThread.Post(() =>
        {
            _logger.LogInformation("Adding browser control to visual tree");
            BrowserHost.Children.Clear();
            var control = browserInstance.BrowserControl;
            _logger.LogInformation("Browser control type: {Type}", control?.GetType().Name ?? "null");
            if (control is not null)
            {
                // Ensure the CEF browser control is focusable
                control.Focusable = true;

                BrowserHost.Children.Add(control);
                _logger.LogInformation("Browser control added. Children count: {Count}", BrowserHost.Children.Count);

                // Give initial focus to the browser
                Dispatcher.UIThread.Post(() =>
                {
                    control.Focus();
                }, DispatcherPriority.Input);
            }
        });
    }

    /// <summary>
    /// Detaches the current browser from this control.
    /// </summary>
    public void DetachBrowser()
    {
        if (_browserInstance is null) return;

        _logger.LogInformation("Detaching browser {Id}", _browserInstance.Id);

        Dispatcher.UIThread.Post(() =>
        {
            BrowserHost.Children.Clear();
        });

        _browserInstance = null;
    }

    /// <summary>
    /// Gets the currently attached browser instance.
    /// </summary>
    public BrowserInstance? CurrentBrowser => _browserInstance;

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // When user clicks on the browser area, ensure CEF gets focus
        if (_browserInstance?.BrowserControl is { } browser && !browser.IsFocused)
        {
            Dispatcher.UIThread.Post(() =>
            {
                browser.Focus();
            });
        }
    }
}
