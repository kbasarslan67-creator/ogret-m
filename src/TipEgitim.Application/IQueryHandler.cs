using TipEgitim.SharedKernel;

namespace TipEgitim.Application;

/// <summary>Durum değiştirmeyen bir sorgunun işleyicisi.</summary>
/// <typeparam name="TQuery">Sorgu türü.</typeparam>
/// <typeparam name="TResponse">Başarıda dönen değer.</typeparam>
public interface IQueryHandler<in TQuery, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
