namespace Ondyxn.Engine;

/// <summary>
/// Represents a single CEF browser instance with navigation and JS execution capabilities.
/// </summary>
public interface IBrowserInstance : IDisposable
{
    /// <summary>The unique identifier for this browser instance.</summary>
    Guid Id { get; }

    /// <summary>Current URL being displayed.</summary>
    string Url { get; }

    /// <summary>Page title.</summary>
    string Title { get; }

    /// <summary>Whether the page is currently loading.</summary>
    bool IsLoading { get; }

    /// <summary>Whether this is an incognito instance.</summary>
    bool IsIncognito { get; }

    /// <summary>Navigate to the specified URL.</summary>
    void Navigate(string url);

    /// <summary>Go back in navigation history.</summary>
    void GoBack();

    /// <summary>Go forward in navigation history.</summary>
    void GoForward();

    /// <summary>Reload the current page.</summary>
    void Reload();

    /// <summary>Stop loading the current page.</summary>
    void Stop();

    /// <summary>Execute JavaScript in the page context.</summary>
    Task<string> EvaluateJavaScriptAsync(string script);

    /// <summary>Get the current page's source HTML.</summary>
    Task<string> GetSourceAsync();

    /// <summary>Whether the browser can go back.</summary>
    bool CanGoBack { get; }

    /// <summary>Whether the browser can go forward.</summary>
    bool CanGoForward { get; }

    /// <summary>Zoom in the page.</summary>
    void ZoomIn();

    /// <summary>Zoom out the page.</summary>
    void ZoomOut();

    /// <summary>Reset zoom to default level.</summary>
    void ZoomReset();

    /// <summary>Set zoom to a specific level.</summary>
    void SetZoom(double level);

    /// <summary>Current zoom level.</summary>
    double ZoomLevel { get; }

    /// <summary>Raised when the page starts loading.</summary>
    event EventHandler<string>? LoadingStarted;

    /// <summary>Raised when the page finishes loading.</summary>
    event EventHandler<string>? LoadingFinished;

    /// <summary>Raised when the title changes.</summary>
    event EventHandler<string>? TitleChanged;

    /// <summary>Raised when the URL changes.</summary>
    event EventHandler<string>? UrlChanged;

    /// <summary>Raised when an error occurs.</summary>
    event EventHandler<string>? ErrorOccurred;
}
