using Microsoft.Extensions.Logging;
using Xilium.CefGlue;

namespace Ondyxn.Engine.Handlers;

/// <summary>
/// Custom CEF client that integrates Ondyxn's request and download handlers.
/// </summary>
public sealed class OndyxnCefClient : CefClient
{
    private readonly OndyxnRequestHandler _requestHandler;
    private readonly OndyxnDownloadHandler _downloadHandler;

    public OndyxnCefClient(
        AdBlockHandler adBlockHandler,
        ILoggerFactory? loggerFactory = null,
        string? downloadDirectory = null)
    {
        _requestHandler = new OndyxnRequestHandler(adBlockHandler, loggerFactory?.CreateLogger<OndyxnRequestHandler>());
        _downloadHandler = new OndyxnDownloadHandler(loggerFactory?.CreateLogger<OndyxnDownloadHandler>(), downloadDirectory);
    }

    /// <summary>
    /// Gets the download handler for subscribing to download events.
    /// </summary>
    public OndyxnDownloadHandler DownloadHandler => _downloadHandler;

    protected override CefRequestHandler GetRequestHandler()
    {
        return _requestHandler;
    }

    protected override CefDownloadHandler? GetDownloadHandler()
    {
        return _downloadHandler;
    }
}
