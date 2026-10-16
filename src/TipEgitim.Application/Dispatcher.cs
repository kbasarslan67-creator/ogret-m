using TipEgitim.SharedKernel;

namespace TipEgitim.Application;

/// <summary>Varsayılan yönlendirici: işleyiciyi bağımlılık kapsayıcısından çözer.</summary>
public sealed class Dispatcher(IServiceProvider services) : IDispatcher
{
    public Task<Result<TResponse>> SendAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        return services.GetService(typeof(ICommandHandler<TCommand, TResponse>)) is ICommandHandler<TCommand, TResponse> handler
            ? handler.HandleAsync(command, cancellationToken)
            : Task.FromResult(HandlerMissing<TResponse>(typeof(TCommand)));
    }

    public Task<Result<TResponse>> QueryAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return services.GetService(typeof(IQueryHandler<TQuery, TResponse>)) is IQueryHandler<TQuery, TResponse> handler
            ? handler.HandleAsync(query, cancellationToken)
            : Task.FromResult(HandlerMissing<TResponse>(typeof(TQuery)));
    }

    private static Result<TResponse> HandlerMissing<TResponse>(Type requestType) =>
        Result.Failure<TResponse>(new ResultError(
            "dispatcher.handler_missing",
            $"'{requestType.Name}' için işleyici kayıtlı değil.",
            ErrorType.Unexpected));
}
