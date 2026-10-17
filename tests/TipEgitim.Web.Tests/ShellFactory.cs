using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace TipEgitim.Web.Tests;

/// <summary>Uygulamayı yapılandırmayı bellek içinde vererek başlatır; appsettings değişikliklerine bağlı kalmaz.</summary>
public sealed class ShellFactory : WebApplicationFactory<Program>
{
    /// <summary>Test kurum adı; ana sayfa testleri bunu bekler.</summary>
    public const string InstitutionName = "Test Tıp Fakültesi";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Platform:InstitutionName"] = InstitutionName,
                ["Platform:DatabaseConnectionName"] = "TestDb",
                ["Platform:DefaultCulture"] = "tr-TR",
            });
        });
    }
}
