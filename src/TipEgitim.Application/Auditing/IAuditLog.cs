using TipEgitim.SharedKernel;

namespace TipEgitim.Application.Auditing;

/// <summary>
/// Yalnızca ekleme yapılabilen denetim günlüğü. Tasarım gereği güncelleme ve silme üyesi yoktur;
/// uygulama kullanıcısı denetim kayıtlarını silemez (denetim standardı).
/// </summary>
public interface IAuditLog
{
    /// <summary>Kaydı günlüğün sonuna ekler.</summary>
    Task<Result> AppendAsync(AuditEntry entry, CancellationToken cancellationToken);

    /// <summary>Tüm kayıtları ekleme sırasıyla döndürür.</summary>
    Task<IReadOnlyList<AuditEntry>> ListAsync(CancellationToken cancellationToken);
}
