using Microsoft.Extensions.Logging;
using Moq;
using Ondyxn.Core.Interfaces;
using Ondyxn.Core.Models;
using Ondyxn.Engine;
using Ondyxn.Engine.Handlers;
using Ondyxn.Engine.Services;
using Ondyxn.UI.ViewModels;

namespace Ondyxn.Tests;

public class BrowserViewModelTests
{
    private readonly Mock<IBookmarkService> _bookmarkService = new();
    private readonly Mock<IHistoryService> _historyService = new();
    private readonly Mock<IDownloadService> _downloadService = new();
    private readonly Mock<ISettingsService> _settingsService = new();
    private readonly Mock<ISessionService> _sessionService = new();
    private readonly Mock<ILogger<BrowserViewModel>> _logger = new();
    private readonly OmniboxResolver _omniboxResolver;
    private readonly FaviconService _faviconService = new();
    private readonly FaviconLetterService _faviconLetterService = new();
    private readonly CefBootstrap _cefBootstrap;
    private readonly AdBlockHandler _adBlockHandler;

    public BrowserViewModelTests()
    {
        _settingsService.Setup(s => s.Current).Returns(new BrowserSettings());
        _omniboxResolver = new OmniboxResolver(_settingsService.Object);
        _cefBootstrap = new CefBootstrap(Mock.Of<ILogger<CefBootstrap>>());
        _adBlockHandler = new AdBlockHandler(NullLogger<AdBlockHandler>.Instance);
    }

    private BrowserViewModel CreateViewModel()
    {
        return new BrowserViewModel(
            _bookmarkService.Object,
            _historyService.Object,
            _downloadService.Object,
            _settingsService.Object,
            _sessionService.Object,
            _omniboxResolver,
            _faviconService,
            _faviconLetterService,
            _cefBootstrap,
            _adBlockHandler,
            _logger.Object);
    }

    [Fact]
    public void Constructor_CreatesNewTab()
    {
        var vm = CreateViewModel();
        Assert.Single(vm.Tabs);
        Assert.NotNull(vm.ActiveTab);
    }

    [Fact]
    public void Constructor_SetsNewTabPageVisible()
    {
        var vm = CreateViewModel();
        Assert.True(vm.IsNewTabPage);
    }

    [Fact]
    public void CreateNewTab_AddsToTabsCollection()
    {
        var vm = CreateViewModel();
        vm.CreateNewTabCommand.Execute(null);
        Assert.Equal(2, vm.Tabs.Count);
    }

    [Fact]
    public void CreateNewTab_SetsActiveTab()
    {
        var vm = CreateViewModel();
        var initialTab = vm.ActiveTab;
        vm.CreateNewTabCommand.Execute(null);
        Assert.NotEqual(initialTab, vm.ActiveTab);
    }

    [Fact]
    public void CloseTab_RemovesFromCollection()
    {
        var vm = CreateViewModel();
        var tab = vm.ActiveTab!;
        vm.CloseTabCommand.Execute(tab);
        Assert.Empty(vm.Tabs);
    }

    [Fact]
    public void CloseTab_LastTab_CreatesNewTab()
    {
        var vm = CreateViewModel();
        var tab = vm.ActiveTab!;
        vm.CloseTabCommand.Execute(tab);
        Assert.Single(vm.Tabs);
        Assert.NotNull(vm.ActiveTab);
    }

    [Fact]
    public void Navigate_UpdatesOmniboxText()
    {
        var vm = CreateViewModel();
        vm.OmniboxText = "https://example.com";
        vm.NavigateCommand.Execute(null);
        Assert.Equal("https://example.com", vm.OmniboxText);
    }

    [Fact]
    public void ToggleSidebar_TogglesVisibility()
    {
        var vm = CreateViewModel();
        var initial = vm.IsSidebarVisible;
        vm.ToggleSidebarCommand.Execute(null);
        Assert.NotEqual(initial, vm.IsSidebarVisible);
    }

    [Fact]
    public void TogglePrivateMode_TogglesMode()
    {
        var vm = CreateViewModel();
        Assert.False(vm.IsPrivateMode);
        vm.TogglePrivateModeCommand.Execute(null);
        Assert.True(vm.IsPrivateMode);
    }

    [Fact]
    public void NextTab_CyclesThroughTabs()
    {
        var vm = CreateViewModel();
        vm.CreateNewTabCommand.Execute(null);
        vm.CreateNewTabCommand.Execute(null);
        var first = vm.ActiveTab;
        vm.NextTabCommand.Execute(null);
        Assert.NotEqual(first, vm.ActiveTab);
    }

    [Fact]
    public void PreviousTab_CyclesBackward()
    {
        var vm = CreateViewModel();
        vm.CreateNewTabCommand.Execute(null);
        var first = vm.ActiveTab;
        vm.PreviousTabCommand.Execute(null);
        Assert.NotEqual(first, vm.ActiveTab);
    }
}
