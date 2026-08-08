using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Xilium.CefGlue;
using Xilium.CefGlue.Common;
using Xilium.CefGlue.Common.Shared;

namespace Ondyxn.Engine;

/// <summary>
/// Bootstrap and manages the Chromium Embedded Framework lifecycle via CefGlue.
/// </summary>
public class CefBootstrap
{
    private readonly ILogger<CefBootstrap> _logger;
    private string? _cachePath;
    private bool _initialized;

    public CefBootstrap(ILogger<CefBootstrap> logger)
    {
        _logger = logger;
    }

    public bool IsInitialized => _initialized;

    public string CachePath => _cachePath ??= CreateCachePath();

    public void Initialize(
        bool enableGpu = true,
        bool enableLogging = false,
        bool windowlessRendering = true,
        int remoteDebuggingPort = 0)
    {
        if (_initialized)
        {
            _logger.LogWarning("CEF already initialized");
            return;
        }

        _logger.LogInformation("Initializing CEF engine at {CachePath}...", CachePath);

        try
        {
            var settings = new CefSettings
            {
                RootCachePath = CachePath,
                WindowlessRenderingEnabled = windowlessRendering,
                RemoteDebuggingPort = remoteDebuggingPort,
                PersistSessionCookies = true,
                PersistUserPreferences = true,
                MultiThreadedMessageLoop = true,
                ExternalMessagePump = false,
            };

            // Custom scheme for ondyxn:// protocol
            var customSchemes = new[]
            {
                new CustomScheme
                {
                    SchemeName = "ondyxn",
                    SchemeHandlerFactory = new OndyxnSchemeHandlerFactory()
                }
            };

            CefRuntimeLoader.Initialize(settings, customSchemes: customSchemes);

            _initialized = true;
            _logger.LogInformation("CEF engine initialized successfully (version: {Version})",
                CefRuntime.ChromeVersion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize CEF engine");
            throw;
        }
    }

    public void Shutdown()
    {
        if (!_initialized) return;

        _logger.LogInformation("Shutting down CEF engine...");
        try
        {
            CefRuntime.Shutdown();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error during CEF shutdown");
        }
        _initialized = false;
    }

    public void CleanupCache()
    {
        if (string.IsNullOrEmpty(_cachePath)) return;

        try
        {
            if (Directory.Exists(_cachePath))
                Directory.Delete(_cachePath, recursive: true);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to clean CEF cache at {Path}", _cachePath);
        }
    }

    private static string CreateCachePath()
    {
        var tempPath = Path.GetTempPath();
        return Path.Combine(tempPath, "Ondyxn_CEF_" + Guid.NewGuid().ToString("N"));
    }
}

/// <summary>
/// Handles ondyxn:// custom scheme requests (new tab page, settings, etc.).
/// </summary>
internal class OndyxnSchemeHandlerFactory : CefSchemeHandlerFactory
{
    protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
    {
        return new OndyxnSchemeHandler();
    }
}

internal class OndyxnSchemeHandler : CefResourceHandler
{
    private MemoryStream? _stream;
    private string _mimeType = "text/html";

    protected override bool Open(CefRequest request, out bool handleRequest, CefCallback callback)
    {
        var uri = new Uri(request.Url);
        var page = uri.Host.ToLowerInvariant();

        string html = page switch
        {
            "newtab" => GetNewTabHtml(),
            _ => GetNewTabHtml()
        };

        _mimeType = "text/html";
        _stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html));
        handleRequest = true;
        return true;
    }

    protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
    {
        responseLength = _stream?.Length ?? 0;
        response.MimeType = _mimeType;
        response.Status = 200;
        redirectUrl = string.Empty;
    }

    protected override bool Read(Stream stream, int bytesToRead, out int bytesRead, CefResourceReadCallback callback)
    {
        bytesRead = 0;

        if (_stream is null || _stream.Position >= _stream.Length)
            return false;

        var buffer = new byte[Math.Min(bytesToRead, (int)(_stream.Length - _stream.Position))];
        bytesRead = _stream.Read(buffer, 0, buffer.Length);
        stream.Write(buffer, 0, bytesRead);

        return bytesRead > 0;
    }

    protected override bool Skip(long bytesToSkip, out long bytesSkipped, CefResourceSkipCallback callback)
    {
        bytesSkipped = 0;

        if (_stream is null)
            return false;

        var available = _stream.Length - _stream.Position;
        var toSkip = Math.Min(bytesToSkip, available);
        _stream.Position += toSkip;
        bytesSkipped = toSkip;

        return true;
    }

    protected override void Cancel()
    {
        _stream?.Dispose();
        _stream = null;
    }

    private static string GetNewTabHtml()
    {
        return """
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body {
                    margin: 0; padding: 0;
                    background: #0B0E14;
                    color: #E8ECF4;
                    font-family: 'Inter', 'Segoe UI', sans-serif;
                    display: flex; align-items: center; justify-content: center;
                    height: 100vh;
                }
                h1 { font-size: 48px; color: #00D4FF; }
            </style>
        </head>
        <body>
            <h1>Ondyxn</h1>
        </body>
        </html>
        """;
    }
}
