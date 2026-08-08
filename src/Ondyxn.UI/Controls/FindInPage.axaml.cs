using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;

namespace Ondyxn.UI.Controls;

public partial class FindInPage : UserControl
{
    private TextBox? _searchBox;
    private TextBlock? _matchCount;

    public event EventHandler<string>? SearchRequested;
    public event EventHandler? NextMatch;
    public event EventHandler? PreviousMatch;
    public event EventHandler? CloseRequested;

    public FindInPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        _searchBox = this.FindControl<TextBox>("SearchBox");
        _matchCount = this.FindControl<TextBlock>("MatchCount");
    }

    public void Show()
    {
        IsVisible = true;
        Dispatcher.UIThread.Post(() =>
        {
            _searchBox?.Focus();
            _searchBox?.SelectAll();
        });
    }

    public void Hide()
    {
        IsVisible = false;
    }

    public void UpdateMatchCount(int current, int total)
    {
        if (_matchCount is not null)
            _matchCount.Text = $"{current}/{total}";
    }

    private void OnSearchKeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not TextBox textBox) return;

        if (e.Key == Key.Return)
        {
            if (e.KeyModifiers.HasFlag(KeyModifiers.Shift))
                PreviousMatch?.Invoke(this, EventArgs.Empty);
            else
                NextMatch?.Invoke(this, EventArgs.Empty);
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            Hide();
            CloseRequested?.Invoke(this, EventArgs.Empty);
            e.Handled = true;
        }
    }

    private void OnPreviousClick(object? sender, RoutedEventArgs e)
    {
        PreviousMatch?.Invoke(this, EventArgs.Empty);
    }

    private void OnNextClick(object? sender, RoutedEventArgs e)
    {
        NextMatch?.Invoke(this, EventArgs.Empty);
    }

    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        Hide();
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
