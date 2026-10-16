using TipEgitim.Application;
using TipEgitim.SharedKernel;

namespace TipEgitim.Application.Tests;

public sealed class DispatcherTests
{
    [Fact]
    public async Task SendRoutesToRegisteredCommandHandler()
    {
        var services = new FakeServiceProvider().With<ICommandHandler<Ping, string>>(new PingHandler());
        var dispatcher = new Dispatcher(services);

        var result = await dispatcher.SendAsync<Ping, string>(new Ping("merhaba"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("merhaba", result.Value);
    }

    [Fact]
    public async Task QueryRoutesToRegisteredQueryHandler()
    {
        var services = new FakeServiceProvider().With<IQueryHandler<Ping, string>>(new PingHandler());
        var dispatcher = new Dispatcher(services);

        var result = await dispatcher.QueryAsync<Ping, string>(new Ping("sorgu"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("sorgu", result.Value);
    }

    [Fact]
    public async Task MissingHandlerReturnsUnexpectedFailure()
    {
        var dispatcher = new Dispatcher(new FakeServiceProvider());

        var result = await dispatcher.SendAsync<Ping, string>(new Ping("x"), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal("dispatcher.handler_missing", result.Error?.Code);
        Assert.Equal(ErrorType.Unexpected, result.Error?.Type);
    }

    private sealed record Ping(string Text);

    private sealed class PingHandler : ICommandHandler<Ping, string>, IQueryHandler<Ping, string>
    {
        public Task<Result<string>> HandleAsync(Ping command, CancellationToken cancellationToken) =>
            Task.FromResult(Result.Success(command.Text));
    }

    private sealed class FakeServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, object> _services = [];

        public FakeServiceProvider With<TService>(TService instance)
            where TService : notnull
        {
            _services[typeof(TService)] = instance;
            return this;
        }

        public object? GetService(Type serviceType) =>
            _services.GetValueOrDefault(serviceType);
    }
}
