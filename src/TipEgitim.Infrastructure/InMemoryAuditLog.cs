using TipEgitim.Application.Auditing;
using TipEgitim.SharedKernel;

namespace TipEgitim.Infrastructure;

/// <summary>
/// Bellek içi, iş parçacığı güvenli ve yalnızca ekleme yapılabilen denetim günlüğü.
/// Kalıcı PostgreSQL depolaması sonraki dilimde gelecektir; bu sınıf o zamana kadar geçici uygulamadır
/// ve uygulama yeniden başlatıldığında kayıtlar kaybolur.
/// </summary>
public sealed class InMemoryAuditLog : IAuditLog
{
    private readonly object _gate = new();
    private readonly List<AuditEntry> _entries = [];

    /// <inheritdoc />
    public Task<Result> AppendAsync(AuditEntry entry, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entry);
        cancellationToken.ThrowIfCancellationRequested();

        lock (_gate)
        {
            _entries.Add(entry);
        }

        return Task.FromResult(Result.Success());
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<AuditEntry>> ListAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        lock (_gate)
        {
            IReadOnlyList<AuditEntry> snapshot = _entries.ToArray();
            return Task.FromResult(snapshot);
        }
    }
}
