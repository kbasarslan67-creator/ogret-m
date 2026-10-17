using TipEgitim.Application.Auditing;
using TipEgitim.SharedKernel;

namespace TipEgitim.Infrastructure;

/// <summary>
/// Bellek içi, iş parçacığı güvenli ve yalnızca ekleme yapılabilen denetim günlüğü.
/// Yalnızca testlerde kullanılır; üretimde kayıtlar <see cref="Persistence.SqliteAuditLog"/> ile kalıcıdır
/// ve bu sınıf DI'a kaydedilmez.
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
