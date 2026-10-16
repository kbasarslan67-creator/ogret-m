using Microsoft.Extensions.DependencyInjection;

namespace TipEgitim.Infrastructure;

/// <summary>Altyapı servislerinin kayıt noktası.</summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton(TimeProvider.System);
        return services;
    }
}
