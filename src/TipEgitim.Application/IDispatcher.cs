using TipEgitim.SharedKernel;

namespace TipEgitim.Application;

/// <summary>Komut ve sorguları kayıtlı işleyicilere yönlendirir. Dış kütüphane kullanılmaz (ADR-001).</summary>
public interface IDispatcher
{
    Task<Result<TResponse>> SendAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken);

    Task<Result<TResponse>> QueryAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken);
}
