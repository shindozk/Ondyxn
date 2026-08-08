using Avalonia.Controls;
using Avalonia.Threading;
using Microsoft.Extensions.Logging;
using Ondyxn.Engine;

namespace Ondyxn.UI.Controls;

/// <summary>
/// Hosts an AvaloniaCefBrowser control inside the Avalonia visual tree.
/// 
/// IMPORTANT: This control must NOT set Background, as Avalonia treats controls
/// with a Background as hit-testable, which intercepts pointer events before
/// they reach the CEF browser. The AvaloniaCefBrowser handles its own input
/// via CEF's native event loop.
/// </summary>
public partial class CefBrowserControl : UserControl
{
    private BrowserInstance? _browserInstance;
    private static readonly ILogger<CefBrowserControl> _logger =
        new LoggerFactory().CreateLogger<CefBrowserControl>();

    public CefBrowserControl()
    {
        InitializeComponent();
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
                BrowserHost.Children.Add(control);
                _logger.LogInformation("Browser control added. Children count: {Count}", BrowserHost.Children.Count);

                // Give initial focus to the browser after it's attached
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
}
