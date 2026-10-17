using System.Net;
using System.Text.RegularExpressions;
using TipEgitim.Web.Navigation;

namespace TipEgitim.Web.Tests;

/// <summary>Arayüz kabuğu: ana sayfa, menü ve sağlık uç noktası.</summary>
public sealed partial class WebShellTests : IClassFixture<ShellFactory>
{
    private readonly ShellFactory _factory;

    public WebShellTests(ShellFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task S_UI_01_HomePageReturnsOkTurkishSkipLinkShell()
    {
        using var client = _factory.CreateClient();

        using var response = await client.GetAsync(new Uri("/", UriKind.Relative), CancellationToken.None);
        var html = await response.Content.ReadAsStringAsync(CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("<html lang=\"tr\">", html, StringComparison.Ordinal);
        Assert.Contains("class=\"skip-link\" href=\"#main-content\"", html, StringComparison.Ordinal);
        Assert.Contains("<main id=\"main-content\"", html, StringComparison.Ordinal);
        Assert.Contains("<header", html, StringComparison.Ordinal);
        Assert.Contains("<nav class=\"sidebar\"", html, StringComparison.Ordinal);
    }

    [Fact]
    public async Task S_UI_01_HomePageShowsInstitutionNameFromPlatformOptions()
    {
        using var client = _factory.CreateClient();

        using var response = await client.GetAsync(new Uri("/", UriKind.Relative), CancellationToken.None);
        var html = WebUtility.HtmlDecode(await response.Content.ReadAsStringAsync(CancellationToken.None));

        Assert.Contains(ShellFactory.InstitutionName, html, StringComparison.Ordinal);
    }

    [Fact]
    public async Task S_UI_02_MenuContainsEveryEntryAsLinkInSidebar()
    {
        using var client = _factory.CreateClient();

        using var response = await client.GetAsync(new Uri("/", UriKind.Relative), CancellationToken.None);
        var html = await response.Content.ReadAsStringAsync(CancellationToken.None);

        var sidebarStart = html.IndexOf("<nav class=\"sidebar\"", StringComparison.Ordinal);
        Assert.True(sidebarStart >= 0, "Kenar çubuğu gezinme alanı bulunamadı.");
        var sidebarEnd = html.IndexOf("</nav>", sidebarStart, StringComparison.Ordinal);
        var sidebar = html[sidebarStart..sidebarEnd];

        var links = AnchorPattern().Matches(sidebar)
            .Select(match => (Href: match.Groups["href"].Value, Label: WebUtility.HtmlDecode(match.Groups["label"].Value).Trim()))
            .ToArray();

        Assert.Equal(MainMenu.Items.Count, links.Length);
        foreach (var item in MainMenu.Items)
        {
            Assert.Contains(links, link => link.Href == item.Href && link.Label == item.Label);
        }
    }

    [Fact]
    public async Task S_UI_03_HealthEndpointReturnsOk()
    {
        using var client = _factory.CreateClient();

        using var response = await client.GetAsync(new Uri("/health", UriKind.Relative), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [GeneratedRegex("<a class=\"menu-link\" href=\"(?<href>[^\"]+)\"[^>]*>(?<label>[^<]+)</a>", RegexOptions.CultureInvariant)]
    private static partial Regex AnchorPattern();
}
