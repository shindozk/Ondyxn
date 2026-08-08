using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ondyxn.Core.Enums;
using Ondyxn.Engine;

namespace Ondyxn.UI.ViewModels;

/// <summary>
/// ViewModel for a single browser tab. Wraps a real BrowserInstance backed by CEF.
/// </summary>
public partial class TabViewModel : ObservableObject, IDisposable
{
    private readonly BrowserInstance _browserInstance;

    [ObservableProperty] private string _title = "New Tab";
    [ObservableProperty] private string _url = string.Empty;
    [ObservableProperty] private string? _faviconUrl;
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private bool _isPinned;
    [ObservableProperty] private bool _isMuted;
    [ObservableProperty] private bool _isActive;
    [ObservableProperty] private TabGroup _group;
    [ObservableProperty] private TabState _state;

    public Guid Id => _browserInstance.Id;
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    /// <summary>
    /// The underlying BrowserInstance for CEF operations.
    /// </summary>
    public BrowserInstance BrowserInstance => _browserInstance;

    public bool CanGoBack => _browserInstance.CanGoBack;
    public bool CanGoForward => _browserInstance.CanGoForward;

    // Events for parent ViewModel to subscribe to
    public event EventHandler<string>? TitleChanged;
    public event EventHandler<string>? UrlChanged;
    public event EventHandler<string>? LoadingStarted;
    public event EventHandler<string>? LoadingFinished;

    public TabViewModel(BrowserInstance browserInstance)
    {
        _browserInstance = browserInstance;

        _url = browserInstance.Url;
        _title = ExtractTitleFromUrl(browserInstance.Url);

        // Forward CEF events
        browserInstance.TitleChanged += (_, title) =>
        {
            Title = title;
            TitleChanged?.Invoke(this, title);
        };
        browserInstance.UrlChanged += (_, url) =>
        {
            Url = url;
            UrlChanged?.Invoke(this, url);
        };
        browserInstance.LoadingStarted += (_, url) =>
        {
            IsLoading = true;
            State = TabState.Loading;
            LoadingStarted?.Invoke(this, url);
        };
        browserInstance.LoadingFinished += (_, url) =>
        {
            IsLoading = false;
            State = TabState.Ready;
            LoadingFinished?.Invoke(this, url);
        };
        browserInstance.ErrorOccurred += (_, error) =>
        {
            IsLoading = false;
            State = TabState.Error;
        };
    }

    public void Navigate(string? url)
    {
        if (string.IsNullOrEmpty(url)) return;
        _browserInstance.Navigate(url);
    }

    public void GoBack()
    {
        _browserInstance.GoBack();
    }

    public void GoForward()
    {
        _browserInstance.GoForward();
    }

    public void Reload()
    {
        _browserInstance.Reload();
    }

    public void Stop()
    {
        _browserInstance.Stop();
    }

    public void OpenDevTools()
    {
        _browserInstance.ShowDevTools();
    }

    public void ZoomIn() => _browserInstance.ZoomIn();
    public void ZoomOut() => _browserInstance.ZoomOut();
    public void ZoomReset() => _browserInstance.ZoomReset();
    public double ZoomLevel => _browserInstance.ZoomLevel;

    private static string ExtractTitleFromUrl(string url)
    {
        try
        {
            var uri = new Uri(url);
            return uri.Host;
        }
        catch
        {
            return url;
        }
    }

    public void Dispose()
    {
        _browserInstance.Dispose();
        GC.SuppressFinalize(this);
    }
}
