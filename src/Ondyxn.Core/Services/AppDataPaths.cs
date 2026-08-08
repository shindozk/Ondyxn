using System;

namespace Ondyxn.Core.Services;

/// <summary>
/// Centralized paths for browser data storage in AppData.
/// </summary>
public static class AppDataPaths
{
    private static readonly Lazy<string> BaseDirLazy = new(() =>
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(appData, "Ondyxn");
        Directory.CreateDirectory(dir);
        return dir;
    });

    private static readonly Lazy<string> LocalDirLazy = new(() =>
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dir = Path.Combine(localAppData, "Ondyxn");
        Directory.CreateDirectory(dir);
        return dir;
    });

    /// <summary>Base AppData directory (Roaming)</summary>
    public static string BaseDirectory => BaseDirLazy.Value;

    /// <summary>Local AppData directory</summary>
    public static string LocalDirectory => LocalDirLazy.Value;

    /// <summary>Database directory</summary>
    public static string DatabaseDirectory => CreateSubDirectory(BaseDirectory, "Data");

    /// <summary>Database file path</summary>
    public static string DatabasePath => Path.Combine(DatabaseDirectory, "browser.db");

    /// <summary>Logs directory</summary>
    public static string LogsDirectory => CreateSubDirectory(LocalDirectory, "logs");

    /// <summary>Cache directory</summary>
    public static string CacheDirectory => CreateSubDirectory(LocalDirectory, "cache");

    /// <summary>Cookies directory</summary>
    public static string CookiesDirectory => CreateSubDirectory(BaseDirectory, "Cookies");

    /// <summary>Downloads directory</summary>
    public static string DownloadsDirectory => CreateSubDirectory(BaseDirectory, "Downloads");

    /// <summary>Profiles directory</summary>
    public static string ProfilesDirectory => CreateSubDirectory(BaseDirectory, "Profiles");

    /// <summary>Extensions directory</summary>
    public static string ExtensionsDirectory => CreateSubDirectory(BaseDirectory, "Extensions");

    /// <summary>Backups directory</summary>
    public static string BackupsDirectory => CreateSubDirectory(BaseDirectory, "Backups");

    /// <summary>Temp directory for this session</summary>
    public static string TempDirectory => CreateSubDirectory(Path.GetTempPath(), "Ondyxn_" + Environment.ProcessId);

    /// <summary>Get profile directory for a specific profile</summary>
    public static string GetProfileDirectory(string profileName = "Default")
    {
        return CreateSubDirectory(ProfilesDirectory, profileName);
    }

    /// <summary>Get backup file path for database</summary>
    public static string GetBackupPath(string name)
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        return Path.Combine(BackupsDirectory, $"{name}_{timestamp}.db");
    }

    private static string CreateSubDirectory(string basePath, string subDir)
    {
        var path = Path.Combine(basePath, subDir);
        Directory.CreateDirectory(path);
        return path;
    }

    /// <summary>
    /// Ensure all directories exist.
    /// </summary>
    public static void EnsureDirectories()
    {
        _ = BaseDirectory;
        _ = LocalDirectory;
        _ = DatabaseDirectory;
        _ = LogsDirectory;
        _ = CacheDirectory;
        _ = CookiesDirectory;
        _ = DownloadsDirectory;
        _ = ProfilesDirectory;
        _ = ExtensionsDirectory;
        _ = BackupsDirectory;
        _ = TempDirectory;
    }

    /// <summary>
    /// Get total size of a directory in bytes.
    /// </summary>
    public static long GetDirectorySize(string path)
    {
        if (!Directory.Exists(path)) return 0;

        long size = 0;
        foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
        {
            try
            {
                size += new FileInfo(file).Length;
            }
            catch { }
        }
        return size;
    }

    /// <summary>
    /// Format bytes to human readable string.
    /// </summary>
    public static string FormatBytes(long bytes)
    {
        string[] sizes = ["B", "KB", "MB", "GB", "TB"];
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}
