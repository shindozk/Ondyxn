using Avalonia.Controls;
using Avalonia.Interactivity;
using Ondyxn.UI.ViewModels;

namespace Ondyxn.UI.Views;

public partial class SettingsPage : UserControl
{
    private SettingsViewModel? _vm;

    public SettingsPage()
    {
        InitializeComponent();
        DataContextChanged += (_, _) => _vm = DataContext as SettingsViewModel;
    }

    private void OnSearchEngineChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox combo && combo.SelectedItem is ComboBoxItem item && item.Tag is string template)
        {
            _vm?.SetSearchEngineTemplate(template);
        }
    }
}
