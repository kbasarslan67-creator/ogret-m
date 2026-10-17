using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TipEgitim.Infrastructure.Configuration;

namespace TipEgitim.Infrastructure.Tests;

public sealed class PlatformConfigurationTests
{
    [Fact]
    public void S_CFG_01_ValidConfigurationBindsPlatformSection()
    {
        var provider = BuildProvider(new Dictionary<string, string?>
        {
            ["Platform:InstitutionName"] = "Test Tıp Fakültesi",
            ["Platform:DatabaseConnectionName"] = "TestDb",
            ["Platform:DefaultCulture"] = "en-US",
        });

        var options = provider.GetRequiredService<IOptions<PlatformOptions>>().Value;

        Assert.Equal("Test Tıp Fakültesi", options.InstitutionName);
        Assert.Equal("TestDb", options.DatabaseConnectionName);
        Assert.Equal("en-US", options.DefaultCulture);
    }

    [Fact]
    public void S_CFG_02_MissingInstitutionNameFailsValidationWithMessage()
    {
        var provider = BuildProvider(new Dictionary<string, string?>
        {
            ["Platform:DatabaseConnectionName"] = "TestDb",
        });

        var exception = Assert.Throws<OptionsValidationException>(
            () => provider.GetRequiredService<IOptions<PlatformOptions>>().Value);

        Assert.Contains("InstitutionName", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void S_CFG_03_DefaultCultureIsTrTR()
    {
        var provider = BuildProvider(new Dictionary<string, string?>
        {
            ["Platform:InstitutionName"] = "Test Tıp Fakültesi",
            ["Platform:DatabaseConnectionName"] = "TestDb",
        });

        var options = provider.GetRequiredService<IOptions<PlatformOptions>>().Value;

        Assert.Equal("tr-TR", options.DefaultCulture);
    }

    [Fact]
    public void S_CFG_04_BlankDatabasePathFailsValidationWithMessage()
    {
        var provider = BuildProvider(new Dictionary<string, string?>
        {
            ["Platform:InstitutionName"] = "Test Tıp Fakültesi",
            ["Platform:DatabaseConnectionName"] = "TestDb",
            ["Platform:DatabasePath"] = "   ",
        });

        var exception = Assert.Throws<OptionsValidationException>(
            () => provider.GetRequiredService<IOptions<PlatformOptions>>().Value);

        Assert.Contains("DatabasePath", exception.Message, StringComparison.Ordinal);
    }

    private static ServiceProvider BuildProvider(Dictionary<string, string?> values)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();

        var services = new ServiceCollection();
        services.AddPlatformConfiguration(configuration);
        return services.BuildServiceProvider();
    }
}
