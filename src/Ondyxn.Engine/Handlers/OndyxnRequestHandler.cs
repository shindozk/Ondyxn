using Microsoft.Extensions.Logging;
using Xilium.CefGlue;

namespace Ondyxn.Engine.Handlers;

/// <summary>
/// CEF request handler for Ondyxn browser.
/// </summary>
public sealed class OndyxnRequestHandler : CefRequestHandler
{
    private readonly AdBlockHandler _adBlockHandler;
    private readonly ILogger? _logger;

    public OndyxnRequestHandler(AdBlockHandler adBlockHandler, ILogger? logger = null)
    {
        _adBlockHandler = adBlockHandler;
        _logger = logger;
    }

    protected override CefResourceRequestHandler GetResourceRequestHandler(
        CefBrowser browser,
        CefFrame frame,
        CefRequest request,
        bool isNavigation,
        bool isDownload,
        string requestInitiator,
        ref bool disableDefaultHandling)
    {
        disableDefaultHandling = false;
        return new OndyxnResourceRequestHandler(_adBlockHandler);
    }

    protected override bool OnBeforeBrowse(CefBrowser browser, CefFrame frame, CefRequest request, bool userGesture, bool isRedirect)
    {
        return false;
    }
}

/// <summary>
/// CEF resource request handler for Ondyxn browser.
/// </summary>
public sealed class OndyxnResourceRequestHandler : CefResourceRequestHandler
{
    private readonly AdBlockHandler _adBlockHandler;

    public OndyxnResourceRequestHandler(AdBlockHandler adBlockHandler)
    {
        _adBlockHandler = adBlockHandler;
    }

    protected override CefCookieAccessFilter GetCookieAccessFilter(CefBrowser browser, CefFrame frame, CefRequest request)
    {
        return null!;
    }
}
