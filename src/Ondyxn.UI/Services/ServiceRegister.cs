using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ondyxn.Core.Interfaces;
using Ondyxn.Data;
using Ondyxn.Data.Configurations;
using Ondyxn.Data.Repositories;
using Ondyxn.Engine;
using Ondyxn.UI.ViewModels;
using Serilog;
using Serilog.Extensions.Logging;

namespace Ondyxn.UI.Services;

public static class ServiceRegister
{
    public static IServiceProvider ConfigureServices()
    {
        // Bridge Serilog → Microsoft.Extensions.Logging
        var serilogLogger = Log.Logger;

        var services = new ServiceCollection();

        // Logging: wire ILogger<T> to Serilog
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddProvider(new SerilogLoggerProvider(serilogLogger));
            builder.SetMinimumLevel(LogLevel.Debug);
        });

        // Database
        services.AddDbContextFactory<BrowserDbContext>(options =>
        {
            DatabaseConfigurator.Configure((DbContextOptionsBuilder<BrowserDbContext>)options);
        });

        // Engine
        services.AddSingleton<CefBootstrap>();
        services.AddSingleton<Engine.Services.FaviconService>();
        services.AddSingleton<Engine.Services.OmniboxResolver>(sp =>
        {
            var settings = sp.GetRequiredService<ISettingsService>();
            return new Engine.Services.OmniboxResolver(settings);
        });
        services.AddSingleton<Engine.Handlers.AdBlockHandler>();

        // Core services (real implementations with EF Core)
        services.AddSingleton<IBookmarkService, BookmarkRepository>();
        services.AddSingleton<IHistoryService, HistoryRepository>();
        services.AddSingleton<IDownloadService, DownloadRepository>();
        services.AddSingleton<ISessionService, SessionRepository>();
        services.AddSingleton<ISettingsService, SettingsRepository>();
        services.AddSingleton<ISecurityService, SecurityService>();

        // ViewModels
        services.AddTransient<BrowserViewModel>();
        services.AddTransient<SidebarViewModel>();
        services.AddTransient<NewTabViewModel>();
        services.AddTransient<SettingsViewModel>();

        return services.BuildServiceProvider();
    }
}
