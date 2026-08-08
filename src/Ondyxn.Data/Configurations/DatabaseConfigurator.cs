using Microsoft.EntityFrameworkCore;

namespace Ondyxn.Data.Configurations;

public static class DatabaseConfigurator
{
    private static string? _dbPath;

    public static string GetDatabasePath()
    {
        if (_dbPath is not null) return _dbPath;

        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(appData, "Ondyxn");
        Directory.CreateDirectory(dir);
        _dbPath = Path.Combine(dir, "browser.db");
        return _dbPath;
    }

    public static DbContextOptionsBuilder<BrowserDbContext> Configure(DbContextOptionsBuilder<BrowserDbContext> builder)
    {
        var dbPath = GetDatabasePath();
        builder.UseSqlite($"Data Source={dbPath}");
        return builder;
    }
}
