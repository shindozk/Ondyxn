using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ondyxn.Core.Interfaces;
using Ondyxn.Data;
using Ondyxn.Engine;
using Ondyxn.UI.Services;
using Ondyxn.UI.ViewModels;
using Ondyxn.UI.Views;

namespace Ondyxn.UI;

public class App : Application
{
    public static IServiceProvider? Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        try
        {
            Log("Configuring services...");
            Services = ServiceRegister.ConfigureServices();

            // Ensure database is created
            Log("Creating database...");
            using (var scope = Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<BrowserDbContext>>();
                using var db = dbContext.CreateDbContext();
                db.Database.EnsureCreated();
            }
            Log("Database ready.");

            // Load settings from database
            Log("Loading settings...");
            var settingsService = Services.GetRequiredService<ISettingsService>();
            settingsService.LoadAsync().GetAwaiter().GetResult();
            Log("Settings loaded.");

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Log("Creating main window...");
                desktop.MainWindow = new MainWindow
                {
                    DataContext = Services.GetRequiredService<BrowserViewModel>()
                };
                Log("Main window created.");
            }

            base.OnFrameworkInitializationCompleted();
            Log("App initialization completed.");
        }
        catch (Exception ex)
        {
            LogError("App initialization error", ex);
            throw;
        }
    }

    private static void Log(string message)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss} INF] {message}");
    }

    private static void LogError(string message, Exception ex)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss} ERR] {message}: {ex.Message}");
    }
}
