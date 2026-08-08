using System;
using System.IO;
using Avalonia;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Ondyxn;

internal static class Program
{
    private static string _cachePath = null!;
    private static string _logDir = null!;

    [STAThread]
    public static int Main(string[] args)
    {
        // Prepare log directory
        _logDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Ondyxn", "logs");
        Directory.CreateDirectory(_logDir);

        // Configure Serilog: console + rolling file
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.WithProperty("SourceContext", null)
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                path: Path.Combine(_logDir, "ondyxn-.log"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        // Generate unique cache path per process
        _cachePath = Path.Combine(
            Path.GetTempPath(),
            "Ondyxn_CEF_" + Guid.NewGuid().ToString("N"));

        AppDomain.CurrentDomain.ProcessExit += (_, _) => Cleanup();
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            Log.Fatal(e.ExceptionObject as Exception, "Unhandled exception");
        };

        try
        {
            // Attach a console listener so Debug.WriteLine also shows
            System.Diagnostics.Trace.Listeners.Add(
                new System.Diagnostics.TextWriterTraceListener(Console.Out));

            Log.Information("Starting Ondyxn...");
            Log.Information("Logs directory: {LogDir}", _logDir);

            // Initialize CEF BEFORE starting Avalonia
            InitializeCef();

            return BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Fatal error during startup");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void InitializeCef()
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss} INF] Starting CEF initialization...");
        try
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss} INF] Cache path: {_cachePath}");
            
            var bootstrap = new Engine.CefBootstrap(
                Microsoft.Extensions.Logging.Abstractions.NullLogger<Engine.CefBootstrap>.Instance);
            
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss} INF] Calling CefRuntimeLoader.Initialize()...");
            bootstrap.Initialize(
                enableGpu: true,
                enableLogging: true,
                windowlessRendering: true);
            
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss} INF] CEF initialized! Version: {Xilium.CefGlue.CefRuntime.ChromeVersion}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss} ERR] CEF FAILED: {ex.Message}");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss} ERR] {ex.StackTrace}");
            if (ex.InnerException != null)
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss} ERR] Inner: {ex.InnerException.Message}");
        }
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<UI.App>()
            .UsePlatformDetect()
            .With(new Win32PlatformOptions())
            .WithInterFont()
            .LogToTrace(Avalonia.Logging.LogEventLevel.Warning);
    }

    private static void Cleanup()
    {
        try
        {
            Log.Information("Shutting down CEF...");
            Xilium.CefGlue.CefRuntime.Shutdown();
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "CEF shutdown error");
        }

        try
        {
            if (Directory.Exists(_cachePath))
                Directory.Delete(_cachePath, recursive: true);
        }
        catch (UnauthorizedAccessException) { }
        catch (IOException) { }
    }
}
