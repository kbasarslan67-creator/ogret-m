using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TipEgitim.Application.Auditing;
using TipEgitim.Infrastructure.Configuration;
using TipEgitim.Infrastructure.Persistence;

namespace TipEgitim.Infrastructure;

/// <summary>Altyapı servislerinin kayıt noktası.</summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Platform yapılandırmasını (<see cref="PlatformOptions"/>) bağlar ve SQLite denetim günlüğünü kaydeder.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddPlatformConfiguration(configuration);
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<IAuditLog, SqliteAuditLog>();
        return services;
    }
}
