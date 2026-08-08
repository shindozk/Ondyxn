using FluentAssertions;
using Ondyxn.Engine.AdBlock;
using Ondyxn.Engine.Handlers;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Ondyxn.Tests;

public class AdBlockTests
{
    [Fact]
    public void ShouldBlock_TrackingUrl_ReturnsTrue()
    {
        // Arrange
        var handler = new AdBlockHandler(NullLogger<AdBlockHandler>.Instance);
        var url = "https://www.google-analytics.com/analytics.js";

        // Act
        var result = handler.ShouldBlock(url);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldBlock_NormalUrl_ReturnsFalse()
    {
        // Arrange
        var handler = new AdBlockHandler(NullLogger<AdBlockHandler>.Instance);
        var url = "https://www.github.com";

        // Act
        var result = handler.ShouldBlock(url);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldBlock_AdDomain_ReturnsTrue()
    {
        // Arrange
        var handler = new AdBlockHandler(NullLogger<AdBlockHandler>.Instance);
        var url = "https://ad.doubleclick.net/tracking";

        // Act
        var result = handler.ShouldBlock(url);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void FilterListParser_ParseEasyListFormat_ReturnsCorrectRules()
    {
        // Arrange
        var filterContent = @"
! Test Filter List
||doubleclick.net^
||google-analytics.com^
##.ad-banner
@@||example.com^
";

        // Act
        var filterList = FilterListParser.Parse(filterContent, "Test List");

        // Assert
        filterList.Name.Should().Be("Test List");
        filterList.Rules.Should().HaveCount(4);
        filterList.Rules.Should().Contain(r => r.Pattern.Contains("doubleclick.net"));
        filterList.Rules.Should().Contain(r => r.IsException);
    }

    [Fact]
    public void GetStats_ReturnsCorrectCounts()
    {
        // Arrange
        var handler = new AdBlockHandler(NullLogger<AdBlockHandler>.Instance);

        // Act
        handler.ShouldBlock("https://google-analytics.com/test.js");
        handler.ShouldBlock("https://facebook.com/tr");
        var stats = handler.GetStats();

        // Assert
        stats.TotalBlocked.Should().Be(2);
        stats.FilterListsLoaded.Should().BeGreaterThan(0);
    }
}
