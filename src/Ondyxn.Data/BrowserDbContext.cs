using Microsoft.EntityFrameworkCore;
using Ondyxn.Core.Models;

namespace Ondyxn.Data;

/// <summary>
/// EF Core database context for Ondyxn browser data.
/// </summary>
public class BrowserDbContext : DbContext
{
    public DbSet<HistoryEntry> History { get; set; } = null!;
    public DbSet<BookmarkModel> Bookmarks { get; set; } = null!;
    public DbSet<DownloadModel> Downloads { get; set; } = null!;
    public DbSet<SessionModel> Sessions { get; set; } = null!;
    public DbSet<TabSnapshot> TabSnapshots { get; set; } = null!;
    public DbSet<BrowserSettingsEntity> Settings { get; set; } = null!;

    public BrowserDbContext(DbContextOptions<BrowserDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<HistoryEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Url).HasMaxLength(2048);
            entity.Property(e => e.Title).HasMaxLength(512);
            entity.HasIndex(e => e.Url);
            entity.HasIndex(e => e.VisitedAt);
        });

        modelBuilder.Entity<BookmarkModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Url).HasMaxLength(2048);
            entity.Property(e => e.Title).HasMaxLength(512);
            entity.Property(e => e.Folder).HasMaxLength(256);
            entity.HasIndex(e => e.Url);
        });

        modelBuilder.Entity<DownloadModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Url).HasMaxLength(2048);
            entity.Property(e => e.FileName).HasMaxLength(512);
            entity.Property(e => e.FilePath).HasMaxLength(2048);
        });

        modelBuilder.Entity<SessionModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<TabSnapshot>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Url).HasMaxLength(2048);
            entity.Property(e => e.Title).HasMaxLength(512);
            entity.HasIndex(e => e.SessionId);
        });

        modelBuilder.Entity<BrowserSettingsEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SearchEngine).HasMaxLength(512);
            entity.Property(e => e.HomePage).HasMaxLength(512);
        });
    }
}
