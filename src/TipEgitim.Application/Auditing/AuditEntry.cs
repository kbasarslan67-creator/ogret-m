namespace TipEgitim.Application.Auditing;

/// <summary>
/// Değiştirilemez bir denetim kaydı: kim (<see cref="Actor"/>), ne (<see cref="Action"/>), ne zaman
/// (<see cref="OccurredAtUtc"/>) ve gerekiyorsa neden (<see cref="Reason"/>).
/// </summary>
/// <param name="Id">Kaydın benzersiz kimliği (zaman sıralı UUID v7).</param>
/// <param name="OccurredAtUtc">Olayın UTC zamanı.</param>
/// <param name="Actor">İşlemi yapan kullanıcı veya sistem bileşeni.</param>
/// <param name="Action">Yapılan işlem (örn. "curriculum.year_frozen").</param>
/// <param name="TargetType">Etkilenen varlık türü.</param>
/// <param name="TargetId">Etkilenen varlığın kimliği.</param>
/// <param name="Reason">Gerektiğinde işlemin gerekçesi.</param>
public sealed record AuditEntry(
    Guid Id,
    DateTimeOffset OccurredAtUtc,
    string Actor,
    string Action,
    string TargetType,
    string TargetId,
    string? Reason);
