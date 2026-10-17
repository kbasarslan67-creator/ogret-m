using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TipEgitim.Infrastructure.Configuration;

/// <summary>Platform yapılandırmasının kayıt noktası.</summary>
public static class PlatformConfigurationExtensions
{
    /// <summary>
    /// "Platform" bölümünü <see cref="PlatformOptions"/> olarak bağlar ve uygulama başlarken doğrular.
    /// </summary>
    public static IServiceCollection AddPlatformConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddOptions<PlatformOptions>()
            .Bind(configuration.GetSection(PlatformOptions.SectionName))
            .ValidateOnStart();

        services.AddSingleton<IValidateOptions<PlatformOptions>, PlatformOptionsValidator>();
        return services;
    }
}
