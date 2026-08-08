using Ondyxn.Engine.Services;

namespace Ondyxn.Tests;

public class OmniboxResolverTests
{
    private readonly OmniboxResolver _resolver = new();

    [Fact]
    public void Resolve_EmptyInput_ReturnsEmpty()
    {
        var result = _resolver.Resolve("");
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Resolve_WhitespaceInput_ReturnsEmpty()
    {
        var result = _resolver.Resolve("   ");
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Resolve_HttpUrl_ReturnsSameUrl()
    {
        var result = _resolver.Resolve("http://example.com");
        Assert.Equal("http://example.com", result);
    }

    [Fact]
    public void Resolve_HttpsUrl_ReturnsSameUrl()
    {
        var result = _resolver.Resolve("https://google.com");
        Assert.Equal("https://google.com", result);
    }

    [Fact]
    public void Resolve_DomainWithDot_PrependsHttps()
    {
        var result = _resolver.Resolve("example.com");
        Assert.Equal("https://example.com", result);
    }

    [Fact]
    public void Resolve_SearchQuery_UsesSearchEngine()
    {
        var result = _resolver.Resolve("hello world");
        Assert.Contains("hello%20world", result);
        Assert.StartsWith("https://www.google.com/search?q=", result);
    }

    [Fact]
    public void Resolve_DomainWithSubdomain_PrependsHttps()
    {
        var result = _resolver.Resolve("sub.example.com");
        Assert.Equal("https://sub.example.com", result);
    }

    [Fact]
    public void IsUrl_HttpUrl_ReturnsTrue()
    {
        Assert.True(_resolver.IsUrl("http://example.com"));
    }

    [Fact]
    public void IsUrl_HttpsUrl_ReturnsTrue()
    {
        Assert.True(_resolver.IsUrl("https://example.com"));
    }

    [Fact]
    public void IsUrl_Domain_ReturnsTrue()
    {
        Assert.True(_resolver.IsUrl("example.com"));
    }

    [Fact]
    public void IsUrl_SearchQuery_ReturnsFalse()
    {
        Assert.False(_resolver.IsUrl("hello world"));
    }

    [Fact]
    public void IsUrl_Empty_ReturnsFalse()
    {
        Assert.False(_resolver.IsUrl(""));
    }

    [Fact]
    public void Resolve_UrlWithPort_PrependsHttps()
    {
        var result = _resolver.Resolve("localhost:3000");
        Assert.Equal("https://localhost:3000", result);
    }

    [Fact]
    public void Resolve_SpecialCharactersInSearch_EscapesProperly()
    {
        var result = _resolver.Resolve("c# programming & design");
        Assert.Contains("c%23", result);
        Assert.Contains("%26", result);
    }

    [Fact]
    public void Resolve_CustomSearchEngine_UsesProvidedTemplate()
    {
        var customResolver = new OmniboxResolver("https://duckduckgo.com/?q={0}");
        var result = customResolver.Resolve("test query");
        Assert.StartsWith("https://duckduckgo.com/?q=", result);
        Assert.Contains("test%20query", result);
    }
}
