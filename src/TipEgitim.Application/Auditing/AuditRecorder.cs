using TipEgitim.SharedKernel;

namespace TipEgitim.Application.Auditing;

/// <summary>
/// Denetim kaydı üretir, girdileri doğrular ve <see cref="IAuditLog"/>'a ekler.
/// Zaman <see cref="TimeProvider"/> üzerinden alınır (test edilebilirlik için).
/// </summary>
public sealed class AuditRecorder(IAuditLog auditLog, TimeProvider timeProvider)
{
    /// <summary>Yeni bir denetim kaydı oluşturup günlüğe ekler.</summary>
    /// <returns>Girdi geçersizse doğrulama hatası; aksi halde günlük ekleme sonucu.</returns>
    public Task<Result> RecordAsync(
        string actor,
        string action,
        string targetType,
        string targetId,
        string? reason,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(auditLog);

        if (string.IsNullOrWhiteSpace(actor))
        {
            return Task.FromResult(Result.Failure(AuditErrors.ActorRequired));
        }

        if (string.IsNullOrWhiteSpace(action))
        {
            return Task.FromResult(Result.Failure(AuditErrors.ActionRequired));
        }

        if (string.IsNullOrWhiteSpace(targetType))
        {
            return Task.FromResult(Result.Failure(AuditErrors.TargetTypeRequired));
        }

        if (string.IsNullOrWhiteSpace(targetId))
        {
            return Task.FromResult(Result.Failure(AuditErrors.TargetIdRequired));
        }

        var entry = new AuditEntry(
            Guid.CreateVersion7(),
            timeProvider.GetUtcNow(),
            actor,
            action,
            targetType,
            targetId,
            reason);

        return auditLog.AppendAsync(entry, cancellationToken);
    }
}
