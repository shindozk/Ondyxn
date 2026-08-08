using Avalonia.Controls;
using Avalonia.Interactivity;
using Ondyxn.Core.Interfaces;

namespace Ondyxn.UI.Views;

public partial class ClearBrowsingDataDialog : Window
{
    private readonly IHistoryService _historyService;
    private readonly ISecurityService _securityService;

    public bool DataCleared { get; private set; }

    public ClearBrowsingDataDialog(IHistoryService historyService, ISecurityService securityService)
    {
        InitializeComponent();
        _historyService = historyService;
        _securityService = securityService;
    }

    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void OnClearClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var timeRange = TimeRangeComboBox.SelectedIndex switch
            {
                0 => DateTime.Now.AddHours(-1),
                1 => DateTime.Now.AddDays(-1),
                2 => DateTime.Now.AddDays(-7),
                3 => DateTime.Now.AddDays(-28),
                _ => (DateTime?)null
            };

            if (BrowsingHistoryCheck.IsChecked == true)
            {
                await _historyService.ClearHistoryAsync(timeRange);
            }

            if (SiteSettingsCheck.IsChecked == true)
            {
                await _securityService.ClearSitePermissionsAsync();
            }

            // Note: Cookies and cache clearing would require CEF integration
            // For now, we clear what we can through the services

            DataCleared = true;
            Close();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to clear browsing data: {ex.Message}");
        }
    }
}
