using Microsoft.Extensions.DependencyInjection;
using TipEgitim.Application.Auditing;

namespace TipEgitim.Infrastructure;

/// <summary>Altyapı servislerinin kayıt noktası.</summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<IAuditLog, InMemoryAuditLog>();
        return services;
    }
}
