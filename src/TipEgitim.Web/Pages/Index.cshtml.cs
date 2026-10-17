using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using TipEgitim.Infrastructure.Configuration;

namespace TipEgitim.Web.Pages;

/// <summary>Ana sayfa: kurum adını Platform yapılandırmasından okur.</summary>
public sealed class IndexModel(IOptions<PlatformOptions> platformOptions) : PageModel
{
    /// <summary>Ekranda gösterilecek kurum adı.</summary>
    public string InstitutionName { get; private set; } = string.Empty;

    /// <summary>GET isteğinde kurum adını yükler.</summary>
    public void OnGet()
    {
        InstitutionName = platformOptions.Value.InstitutionName;
    }
}
