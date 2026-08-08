using Avalonia;
using Avalonia.Threading;
using Microsoft.Extensions.Logging;
using Ondyxn.Engine.Handlers;
using Xilium.CefGlue;
using Xilium.CefGlue.Avalonia;
using Xilium.CefGlue.Common.Events;

namespace Ondyxn.Engine;

/// <summary>
/// Wraps an AvaloniaCefBrowser control, providing a clean interface for navigation,
/// JS execution, and lifecycle events. The underlying AvaloniaCefBrowser control
/// should be added to the visual tree by the UI layer.
/// </summary>
public class BrowserInstance : IBrowserInstance
{
    private readonly ILogger<BrowserInstance>? _logger;
    private readonly AdBlockHandler? _adBlockHandler;
    private readonly AvaloniaCefBrowser _browser;
    private string _url;
    private string _title = string.Empty;
    private bool _isLoading;
    private int _retryCount;
    private const int MaxRetries = 3;

    public Guid Id { get; } = Guid.NewGuid();
    public string Url => _url;
    public string Title => _title;
    public bool IsLoading => _isLoading;
    public bool IsIncognito { get; }

    /// <summary>
    /// The underlying AvaloniaCefBrowser control. Add this to the visual tree to display the browser.
    /// </summary>
    public AvaloniaCefBrowser BrowserControl => _browser;

    public bool CanGoBack => _browser.CanGoBack;
    public bool CanGoForward => _browser.CanGoForward;

    public event EventHandler<string>? LoadingStarted;
    public event EventHandler<string>? LoadingFinished;
    public event EventHandler<string>? TitleChanged;
    public event EventHandler<string>? UrlChanged;
    public event EventHandler<string>? ErrorOccurred;

    public BrowserInstance(string url, bool isIncognito = false, ILogger<BrowserInstance>? logger = null, AdBlockHandler? adBlockHandler = null)
    {
        _url = url;
        IsIncognito = isIncognito;
        _logger = logger;
        _adBlockHandler = adBlockHandler;

        _browser = new AvaloniaCefBrowser();
        _logger?.LogInformation("[{Id}] BrowserInstance created", Id);

        // Wire up CEF events
        _browser.AddressChanged += OnAddressChanged;
        _browser.TitleChanged += OnTitleChanged;
        _browser.LoadStart += OnLoadStart;
        _browser.LoadEnd += OnLoadEnd;
        _browser.LoadError += OnLoadError;

        // Navigate to initial URL
        _logger?.LogInformation("[{Id}] Setting initial URL: {Url}", Id, url);
        _browser.Address = url;
    }

    public void Navigate(string url)
    {
        _url = url;
        _logger?.LogDebug("[{Id}] Navigating to: {Url}", Id, url);
        
        // Fire URL changed event immediately so UI updates
        UrlChanged?.Invoke(this, url);
        
        // Then navigate the browser
        _browser.Address = url;
    }

    public void GoBack()
    {
        if (_browser.CanGoBack)
        {
            _browser.GoBack();
            _logger?.LogDebug("[{Id}] Navigating back", Id);
        }
    }

    public void GoForward()
    {
        if (_browser.CanGoForward)
        {
            _browser.GoForward();
            _logger?.LogDebug("[{Id}] Navigating forward", Id);
        }
    }

    public void Reload()
    {
        _browser.Reload();
        _logger?.LogDebug("[{Id}] Reloading", Id);
    }

    public void Stop()
    {
        // AvaloniaCefBrowser doesn't have Stop method
        _logger?.LogDebug("[{Id}] Stop requested (not implemented)", Id);
    }

    public async Task<string> EvaluateJavaScriptAsync(string script)
    {
        try
        {
            return await _browser.EvaluateJavaScript<string>(script);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "[{Id}] JavaScript evaluation failed", Id);
            return string.Empty;
        }
    }

    public async Task<string> GetSourceAsync()
    {
        try
        {
            return await _browser.EvaluateJavaScript<string>("return document.documentElement.outerHTML");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "[{Id}] Failed to get source", Id);
            return string.Empty;
        }
    }

    public void ShowDevTools()
    {
        _browser.ShowDeveloperTools();
    }

    private void OnAddressChanged(object? sender, string address)
    {
        _url = address;
        UrlChanged?.Invoke(this, address);
        _logger?.LogDebug("[{Id}] URL changed: {Url}", Id, address);
    }

    private void OnTitleChanged(object? sender, string title)
    {
        _title = title;
        TitleChanged?.Invoke(this, title);
    }

    private void OnLoadStart(object? sender, LoadStartEventArgs e)
    {
        if (e.Frame.Browser.IsPopup || !e.Frame.IsMain) return;

        _isLoading = true;
        LoadingStarted?.Invoke(this, e.Frame.Url);
    }

    private void OnLoadEnd(object? sender, LoadEndEventArgs e)
    {
        if (e.Frame.Browser.IsPopup || !e.Frame.IsMain) return;

        _isLoading = false;
        _retryCount = 0; // Reset retry count on successful load
        LoadingFinished?.Invoke(this, e.Frame.Url);
    }

    private void OnLoadError(object? sender, LoadErrorEventArgs e)
    {
        if (e.Frame.Browser.IsPopup || !e.Frame.IsMain) return;

        _isLoading = false;
        var errorMsg = $"Error {e.ErrorCode}: {e.ErrorText}";
        _logger?.LogWarning("[{Id}] Load error: {Error}", Id, errorMsg);

        // Retry on network errors (ErrorCode 3 = ERR_ABORTED is user-initiated, don't retry)
        if (e.ErrorCode != CefErrorCode.Aborted && _retryCount < MaxRetries)
        {
            _retryCount++;
            _logger?.LogInformation("[{Id}] Retrying load (attempt {Retry}/{Max})", Id, _retryCount, MaxRetries);
            Dispatcher.UIThread.Post(() =>
            {
                _browser.Reload();
            });
            return;
        }

        _retryCount = 0;
        ErrorOccurred?.Invoke(this, errorMsg);
    }

    public void Dispose()
    {
        _browser.AddressChanged -= OnAddressChanged;
        _browser.TitleChanged -= OnTitleChanged;
        _browser.LoadStart -= OnLoadStart;
        _browser.LoadEnd -= OnLoadEnd;
        _browser.LoadError -= OnLoadError;
        _browser.Dispose();
        GC.SuppressFinalize(this);
    }
}
