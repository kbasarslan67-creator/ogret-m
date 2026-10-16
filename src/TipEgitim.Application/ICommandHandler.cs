using TipEgitim.SharedKernel;

namespace TipEgitim.Application;

/// <summary>Durum değiştiren bir komutun işleyicisi.</summary>
/// <typeparam name="TCommand">Komut türü.</typeparam>
/// <typeparam name="TResponse">Başarıda dönen değer.</typeparam>
public interface ICommandHandler<in TCommand, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken);
}
